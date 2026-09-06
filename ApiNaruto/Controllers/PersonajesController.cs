using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiNaruto.Models;

namespace ApiNaruto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonajesController : ControllerBase
{
    private readonly IConfiguration _config;

    public PersonajesController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public IActionResult GetPersonajes()
    {
        string? connectionString = _config.GetConnectionString("DefaultConnection");

        using (var connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT Id, Nombre, Clan, Aldea, TipoChakra FROM Personajes";
            var personajes = connection.Query<Personaje>(sql);
            return Ok(personajes);
        }
    }

    [HttpPost]
    public IActionResult GuardarPersonaje([FromBody] Personaje personaje)
    {
        string? connectionString = _config.GetConnectionString("DefaultConnection");

        using (var connection = new SqlConnection(connectionString))
        {
            string sql = @"INSERT INTO Personajes (Nombre, Clan, Aldea, TipoChakra) 
                           VALUES (@Nombre, @Clan, @Aldea, @TipoChakra)";

            var filasAfectadas = connection.Execute(sql, personaje);

            if (filasAfectadas > 0)
            {
                return Ok("Personaje guardado exitosamente");
            }

            return BadRequest("No se pudo guardar el registro");
        }
    }
}