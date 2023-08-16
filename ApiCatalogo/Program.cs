using ApiCatalogo.Models;
using ApiCatalogo.Models.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CatalogoContext>(options =>
               options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.//Adiciona o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/*-------------------------------------Definindo EndPoints de Categorias-----------------------------*/
//EndPoint de Listagem
app.MapGet("/listaCategorias", async (CatalogoContext db) => await db.Categorias.ToListAsync());

//EndPoint de Listagem por Id
app.MapGet("/listagemCategorias/{id:int}", async (int id, CatalogoContext db) =>
{
    return await db.Categorias.FindAsync(id)
        is Categoria categoria
                ? Results.Ok(categoria)
                : Results.NotFound("Categoria não encontrada");
});

//EndPoint de Inserção
app.MapPost("/categorias", async (Categoria categoria, CatalogoContext db) => 
{

        db.Categorias.Add(categoria);
        await db.SaveChangesAsync();

        return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);

});

//EndPoint de Alteração  

app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, CatalogoContext db) =>
{

    if (categoria.CategoriaId != id)
    {
        return Results.BadRequest("Algum Problema Ocorreu :(");
    }

    var categoriaDB = await db.Categorias.FindAsync(id);

    if (categoriaDB is null) return Results.NotFound("Categoria não Encontrada");

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);
});

//EndPoint de Exclusão
app.MapDelete("/categorias/{id:int}", async (int id, CatalogoContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);

    if (categoria is not null)
    {
        db.Categorias.Remove(categoria);
        await db.SaveChangesAsync();
    }

    return Results.NoContent();
});

/*-------------------------------------Definindo EndPoints de Produtos-------------------------------*/

app.MapGet("/produtos", async (CatalogoContext db) => await db.Produtos.ToListAsync());

app.MapGet("/produtos/{id:int}", async (int id, CatalogoContext db) =>
{

    return await db.Produtos.FindAsync(id)
        is Produto produto
        ? Results.Ok(produto)
        : Results.NotFound();
});

app.MapPost("/produtos/", async (Produto produto, CatalogoContext db) =>
{

    db.Produtos.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.ProdutoId}", produto);
});

app.MapPut("/produtos/{id:int}", async (int id, Produto produto, CatalogoContext db) =>
{

    if (produto.ProdutoId != id)
    {
        return Results.BadRequest();
    }

    var produtoDB = await db.Produtos.FindAsync(id);

    if(produtoDB is null) return Results.NotFound();

    produtoDB.Nome = produto.Nome;
    produtoDB.Descricao = produto.Descricao;
    produtoDB.Preco = produto.Preco;
    produtoDB.DataCompra = produto.DataCompra;
    produtoDB.Estoque = produto.Estoque;
    produtoDB.Imagem = produto.Imagem;
    produtoDB.CategoriaId = produto.CategoriaId;

    await db.SaveChangesAsync();
    return Results.Ok(produtoDB);
});

app.MapDelete("/produtos/{id:int}", async (int id, CatalogoContext db) =>
{

    var produto = await db.Produtos.FindAsync(id);

    if(produto is not null)
    {

        db.Produtos.Remove(produto);
        await db.SaveChangesAsync();
    }

    return Results.NoContent();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();
