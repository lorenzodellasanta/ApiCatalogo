using ApiCatalogo.Models.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.ApiEndpoints
{
    public static class Categorias
    {

        public static void MapCategorias (this WebApplication app)
        {

            //EndPoint de Listagem
            app.MapGet("/listaCategorias", async (CatalogoContext db) =>
            await db.Categorias.ToListAsync()).WithTags("Categorias").RequireAuthorization();

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

        }
    }
}
