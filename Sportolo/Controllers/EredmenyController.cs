using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Sportolo.Models;
using Sportolo.Models.DTOs;
using System.Collections.Generic;

namespace Sportolo.Controllers
{
    [Route("eredmeny")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        public string ConnectionString = "server=localhost;port = 3306; user=root; password=; database=sportolo13b";
        [HttpGet]
        public object GetAllEredmeny()
        {
            List<Eredmeny> list = new List<Eredmeny>();
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = "SELECT * FROM eredmeny";

            var cmd = new MySqlCommand(sql, connector);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var result = new Eredmeny
                {
                    Id = datareader.GetInt32(0),
                    Competition = datareader.GetString(1),
                    Description = datareader.GetString(2),
                    ResultTime = datareader.GetDateTime(3),
                    UpdateTime = datareader.GetDateTime(4),
                    SportoloId = datareader.GetInt32(5),
                };
                list.Add(result);
            }
             

            connector.Close();

            return new { message = "Sikeres lekérdezés.", result = list };
        }

        [HttpGet("byId")]

        public object GetEredmenyById([FromQuery]int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = "SELECT * FROM eredmeny WHERE id = @id";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            datareader.Read();
            
            var eredmeny = new Eredmeny
            {
                    Id = datareader.GetInt32(0),
                    Competition = datareader.GetString(1),
                    Description = datareader.GetString(2),
                    ResultTime = datareader.GetDateTime(3),
                    UpdateTime = datareader.GetDateTime(4),
                    SportoloId = datareader.GetInt32(5),
            };
            


            connector.Close();

            return new { message = "Sikeres lekérdezés.", result = eredmeny };
        }

        [HttpPost]

        public object AddNewEredmeny(AddEredmenyDto addEredmenyDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"INSERT INTO `eredmeny`(`competition`, `description`, `resultTime`, `updateTime`, `sportoloId`) VALUES (@competition,@description,@resultTime,@updateTime,@sportoloId)";

            var cmd = new MySqlCommand(@sql, connector);

            cmd.Parameters.AddWithValue("@competition", addEredmenyDto.Competition);
            cmd.Parameters.AddWithValue("@description", addEredmenyDto.Description);
            cmd.Parameters.AddWithValue("@resultTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@sportoloId", addEredmenyDto.SportoloId);

            cmd.ExecuteNonQuery();

            connector.Close();
            return new { message = "Sikeres feltöltés.", result = addEredmenyDto };
        }


        
        [HttpPut]
        public IActionResult Update(Eredmeny frissitettEredmeny)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                connection.Open();

               
                DateTime aktualisIdo = DateTime.Now;

                string query = @"UPDATE eredmeny 
                                 SET Competition = @Competition, 
                                     Description = @Description, 
                                     ResultTime = @ResultTime, 
                                     UpdateTime = @UpdateTime, 
                                     SportoloId = @SportoloId 
                                 WHERE Id = @Id";

                using var command = new MySqlCommand(query, connection);

                
                command.Parameters.AddWithValue("@Id", frissitettEredmeny.Id);

                command.Parameters.AddWithValue("@Competition", frissitettEredmeny.Competition);
                command.Parameters.AddWithValue("@Description", frissitettEredmeny.Description);
                command.Parameters.AddWithValue("@ResultTime", frissitettEredmeny.ResultTime);
                command.Parameters.AddWithValue("@UpdateTime", aktualisIdo);
                command.Parameters.AddWithValue("@SportoloId", frissitettEredmeny.SportoloId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound("Nem található módosítandó eredmény ezzel az ID-val.");
                }

                return Ok("Az eredmény sikeresen frissítve.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a módosítás során: {ex.Message}");
            }
        }



        [HttpDelete]

        public object DeleteEredmeny([FromBody] int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"DELETE FROM `eredmeny` WHERE id = @id";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connector.Close();

            return new { message = "Sikeres törlés.", result = "" };
        }

    }
}
