using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Xml.Linq;
using YamlDotNet.Serialization;
using System.Net.Http.Headers;
using Microsoft.OpenApi.Any;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Formatting Service API",
        Version = "v1",
        Description = "Service for converting stored data to different formats"
    });
});

// Konfiguracja HttpClient dla komunikacji z Storage Service
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Formatting Service v1");
    c.RoutePrefix = "swagger";
});

app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription();

app.MapGet("/convert/{index}/{format}", async (int index, string format, HttpClient httpClient) =>
{
    try
    {
        // Pobierz dane z Storage Service
        
        var response = await httpClient.GetAsync($"https://localhost:5000/data/{index}");
        
        if (!response.IsSuccessStatusCode)
            return Results.Problem("Storage service unavailable", statusCode: 503);

        var jsonContent = await response.Content.ReadAsStringAsync();

        // Konwersja do żądanego formatu
        return format.ToLower() switch
        {
            "json" => Results.Text(jsonContent, "application/json"),
            "xml" => Results.Text(ConvertJsonToXml(jsonContent), "application/xml"),
            "yaml" => Results.Text(ConvertJsonToYaml(jsonContent), "application/yaml"),
            _ => Results.BadRequest("Unsupported format")
        };
    }
    catch (Exception ex)
    {
        return Results.Problem($"Conversion error: {ex.Message}");
    }
})
.WithOpenApi(operation => new OpenApiOperation(operation)
{
    Summary = "Convert stored data",
    Parameters = new List<OpenApiParameter>
    {
        new OpenApiParameter
        {
            Name = "index",
            In = ParameterLocation.Path,
            Required = true,
            Schema = new OpenApiSchema { Type = "integer" }
        },
        new OpenApiParameter
        {
            Name = "format",
            In = ParameterLocation.Path,
            Required = true,
            Schema = new OpenApiSchema 
            { 
                Type = "string",
                Enum = new List<IOpenApiAny>
                {
                    new OpenApiString("json"),
                    new OpenApiString("xml"),
                    new OpenApiString("yaml")
                }
            }
        }
    }
});

// Metody konwersji
string ConvertJsonToXml(string jsonContent)
{
    try
    {
        var xmlNode = JsonConvert.DeserializeXNode(jsonContent, "Root");
        return xmlNode?.ToString() ?? throw new InvalidOperationException();
    }
    catch
    {
        throw new ArgumentException("Invalid JSON structure for XML conversion");
    }
}

string ConvertJsonToYaml(string jsonContent)
{
    try
    {
        var deserializer = new DeserializerBuilder().Build();
        var jsonObject = deserializer.Deserialize(new StringReader(jsonContent));
        
        var serializer = new SerializerBuilder().Build();
        return serializer.Serialize(jsonObject);
    }
    catch
    {
        throw new ArgumentException("Invalid JSON structure for YAML conversion");
    }
}

app.Run();
