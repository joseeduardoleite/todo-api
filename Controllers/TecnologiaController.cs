using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Data;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class TecnologiaController : ControllerBase
    {
        private readonly AppDbContext _database;

        public TecnologiaController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult ListarTecnologia()
        {
            var tecnologia = _database.Tecnologias.ToList();

            return Ok(tecnologia);
        }

        [HttpPost]
        public IActionResult CriarTecnologia([FromBody] Tecnologia tecnologiaTemp)
        {
            Tecnologia tecnologia = new Tecnologia();
            tecnologia.Nome = tecnologiaTemp.Nome;

            _database.Tecnologias.Add(tecnologia);
            _database.SaveChanges();

            Response.StatusCode = 201;
            return new ObjectResult("");
        }

        [HttpPut]
        public IActionResult EditarTecnologia([FromBody] Tecnologia tecnologia)
        {
            var tec = _database.Tecnologias.FirstOrDefault(p => p.Id == tecnologia.Id);

            tec.Nome = tecnologia.Nome;

            _database.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarTecnologia(int id)
        {
            var tecnologia = _database.Tecnologias.FirstOrDefault(p => p.Id == id);

            _database.Tecnologias.Remove(tecnologia);
            _database.SaveChanges();

            return Ok();
        }
    }
}