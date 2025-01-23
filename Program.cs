// Importowanie wymaganych przestrzeni nazw
using Microsoft.OpenApi.Models;
using YamlDotNet.Serialization;
using System.Text;
using Newtonsoft.Json;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

// Inicjalizacja budowniczego aplikacji
var builder = WebApplication.CreateBuilder(args);

// Konfiguracja usług Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Storage Service API",
        Version = "v1",
        Description = "Serwis do przechowywania danych w formacie JSON z obsługą różnych formatów wejściowych"
    });
});

var app = builder.Build();

// Konfiguracja middleware Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Storage Service v1");
    c.RoutePrefix = "swagger"; // Dostęp do Swagger UI pod /swagger
});

// In-memory storage dla przechowywania danych
var storage = new Dictionary<int, string>();
var indexCounter = 0; // Licznik dla generowania unikalnych indeksów

// Endpoint przekierowujący na Swagger UI (ukryty w dokumentacji)
app.MapGet("/", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();

// Endpoint do przesyłania danych w różnych formatach
app.MapPost("/data", async (HttpContext context) =>
{
    try
    {
        // Weryfikacja typu zawartości
        var contentType = context.Request.ContentType?.Split(';')[0].Trim().ToLower();
        var allowedTypes = new[] { "application/json", "application/xml", "text/xml", "application/yaml", "text/yaml" };

        if (!allowedTypes.Contains(contentType))
            return Results.BadRequest("Nieobsługiwany format danych");

        // Odczyt zawartości żądania
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
        var content = await reader.ReadToEndAsync();

        // Konwersja do JSON w zależności od formatu
        string jsonContent = contentType switch
        {
            "application/json" => ValidateJson(content),
            var x when x.Contains("xml") => ConvertXmlToJson(content),
            var y when y.Contains("yaml") => ConvertYamlToJson(content),
            _ => throw new InvalidOperationException()
        };

        // Zapis do pamięci z atomowym zwiększeniem indeksu
        var newIndex = Interlocked.Increment(ref indexCounter);
        storage[newIndex] = jsonContent;

        return Results.Created($"/data/{newIndex}", new { Index = newIndex });
    }
    catch (JsonException)
    {
        return Results.BadRequest("Nieprawidłowy format JSON");
    }
    catch (System.Xml.XmlException ex)
    {
        return Results.BadRequest($"Błąd formatu XML: {ex.Message}");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Błąd konwersji: {ex.Message}");
    }
})
.WithOpenApi(operation => new OpenApiOperation(operation)
{
    // Konfiguracja dokumentacji OpenAPI dla tego endpointu
    Summary = "Zapisz dane w dowolnym formacie",
    Description = "Akceptuje dane w formatach JSON/XML/YAML, konwertuje i zapisuje jako JSON",
    RequestBody = new OpenApiRequestBody
    {
        Content = new Dictionary<string, OpenApiMediaType>
        {
            ["application/json"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema { Type = "object" },
                Example = new OpenApiString("{\"klucz\":\"wartość\"}")
            },
            ["application/xml"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema { Type = "string" },
                Example = new OpenApiString("<root><klucz>wartość</klucz></root>")
            },
            ["application/yaml"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema { Type = "string" },
                Example = new OpenApiString("klucz: wartość")
            }
        }
    }
});

/// <summary>
/// Waliduje i formatuje JSON
/// </summary>
/// <param name="content">Surowy ciąg JSON</param>
/// <returns>Sformatowany JSON</returns>
string ValidateJson(string content)
{
    // Parsowanie i ponowna serializacja dla formatowania
    var parsed = JToken.Parse(content);
    return parsed.ToString(Formatting.Indented);
}

/// <summary>
/// Konwertuje XML na JSON przy użyciu Newtonsoft.Json
/// </summary>
/// <param name="xmlContent">Dane wejściowe w XML</param>
/// <returns>Dane w formacie JSON</returns>
string ConvertXmlToJson(string xmlContent)
{
    try
    {
        var xmlDoc = XDocument.Parse(xmlContent);
        return JsonConvert.SerializeXNode(xmlDoc, Formatting.Indented);
    }
    catch (Exception ex)
    {
        throw new System.Xml.XmlException("Błąd konwersji XML", ex);
    }
}

/// <summary>
/// Konwertuje YAML na JSON przy użyciu YamlDotNet
/// </summary>
/// <param name="yamlContent">Dane wejściowe w YAML</param>
/// <returns>Dane w formacie JSON</returns>
string ConvertYamlToJson(string yamlContent)
{
    var deserializer = new DeserializerBuilder().Build();
    var yamlObject = deserializer.Deserialize(new StringReader(yamlContent));

    var serializer = new SerializerBuilder()
        .JsonCompatible()
        .Build();

    return serializer.Serialize(yamlObject);
}

// Endpoint do pobierania zapisanych danych
app.MapGet("/data/{index}", (int index) =>
{
    return storage.TryGetValue(index, out var content)
        ? Results.Text(content, "application/json")
        : Results.NotFound();
});

// Uruchomienie aplikacji
app.Run();