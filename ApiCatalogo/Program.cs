using ApiCatalogo.ApiEndpoints;
using ApiCatalogo.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

//EndPoints para Login
app.MapAutenticacao();

//EndPoints para Categoria
app.MapCategorias();

//EndPoints para Produtos
app.MapProdutos();

var environment = app.Environment;

app.UseExceptionHandling(environment)
   .UseSwaggerMiddleware()
   .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
