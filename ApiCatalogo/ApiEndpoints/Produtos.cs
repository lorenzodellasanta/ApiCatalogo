using ApiCatalogo.Models.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.ApiEndpoints
{
    public static class Produtos
    {

        public static void MapProdutos(this WebApplication app)
        {
            app.MapGet("/produtos", async (CatalogoContext db) =>
                await db.Produtos.ToListAsync()).WithTags("Produtos").RequireAuthorization();

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

                if (produtoDB is null) return Results.NotFound();

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

                if (produto is not null)
                {

                    db.Produtos.Remove(produto);
                    await db.SaveChangesAsync();
                }

                return Results.NoContent();
            });
        }
    }
}
