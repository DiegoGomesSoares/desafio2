using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Payment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Products> Get()
        {
            var listProducts = new List<Products>();
            var cs = @"
  Server=mssql-server, 1433;
  Database=Payment;
  User Id=sa;
  Password=senha#2021";

            using (var connection = new SqlConnection(cs))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText =
@"
SELECT id, nome, quantidade
FROM [produtos]";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listProducts.Add(new Products
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nome = reader["nome"].ToString(),
                        Qunatidade = Convert.ToInt32(reader["quantidade"]),
                    });
                }
            };

            return listProducts;
        }
    }
}
