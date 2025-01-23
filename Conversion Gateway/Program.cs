var builder = WebApplication.CreateBuilder(args);

// Dodaj usługi do kontenera DI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Konfiguracja pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpointy
app.MapGet("/", () => "Hello World!")
   .WithName("GetHelloWorld")
   .WithOpenApi();

app.MapGet("/hello/{name}", (string name) => $"Hello, {name}!")
   .WithName("GetHelloByName")
   .WithOpenApi();

app.Run();