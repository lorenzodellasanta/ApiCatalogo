using ApiCatalogo.Models;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiCatalogo.ApiEndpoints
{
    public static class Autenticacao
    {

        public static void MapAutenticacao( this WebApplication app)
        {
            app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
            {
                if (userModel == null)
                {
                    return Results.BadRequest("Login Inválido");
                }

                if (userModel.UserName == "lorenzo" && userModel.Password == "lorenzo2502")
                {
                    var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"],
                        app.Configuration["Jwt:Issuer"],
                        app.Configuration["Jwt:Audience"],
                        userModel);
                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    return Results.BadRequest("Login Iválido");
                }
            }).Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status200OK)
              .WithName("Login")
              .WithTags("Autenticação");
        }
    }
}
