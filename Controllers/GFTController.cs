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
    public class GFTController : ControllerBase
    {
        private readonly AppDbContext _database;

        public GFTController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult ListarGFT()
        {
            var gft = _database.GFts.ToList();

            return Ok(gft);
        }

        [HttpPost]
        public IActionResult CriarGFT([FromBody] GFT gftTemp)
        {
            GFT gft = new GFT();
            gft.Nome = gftTemp.Nome;
            gft.Cep = gftTemp.Cep;
            gft.Estado = gftTemp.Estado;
            gft.Cidade = gftTemp.Cidade;
            gft.Telefone = gftTemp.Telefone;

            _database.GFts.Add(gft);
            _database.SaveChanges();

            Response.StatusCode = 201;
            return new ObjectResult("");
        }

        [HttpPut]
        public IActionResult EditarGFT([FromBody] GFT gft)
        {
            var gftBase = _database.GFts.FirstOrDefault(p => p.Id == gft.Id);

            gftBase.Nome = gft.Nome;
            gftBase.Telefone = gft.Telefone;
            gftBase.Estado = gft.Estado;
            gftBase.Cidade = gft.Cidade;
            gftBase.Cep = gft.Cep;

            _database.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarGFT(int id)
        {
            var gft = _database.GFts.FirstOrDefault(x => x.Id == id);
            
            _database.GFts.Remove(gft);
            _database.SaveChanges();

            return Ok();
        }
    }
}