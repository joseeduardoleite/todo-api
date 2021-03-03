using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ProfissaoController : ControllerBase
    {
        private readonly AppDbContext _database;

        public ProfissaoController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult ListarProfissao()
        {
            var profissao = _database.Profissoes.Include(p => p.LocalTrabalho).Include(x => x.Tecnologia).ToList();

            return Ok(profissao);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult ListarProfissaoId(int id)
        {
            var profissao = _database.Profissoes.Include(p => p.LocalTrabalho).Include(x => x.Tecnologia).AsNoTracking().FirstOrDefault(p => p.Id == id);
            return Ok(profissao);
        }

        [HttpGet]
        [Route("gft/{id:int}")]
        public IActionResult ListarProfissaoGftId(int id)
        {
            var profissaoGftId = _database.Profissoes.Include(p => p.LocalTrabalho).Include(x => x.Tecnologia).AsNoTracking().Where(x => x.LocalTrabalho.Id == id).ToList();
            return Ok(profissaoGftId);
        }

        [HttpPost]
        public IActionResult CriarProfissao([FromBody] Profissao profissao)
        {
            if (ModelState.IsValid) {

                profissao.Inicio = DateTime.Now;
                
                _database.Profissoes.Add(profissao);
                _database.SaveChanges();

                Response.StatusCode = 201;
                return new ObjectResult("");
            }
            else {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public IActionResult EditarProfissao([FromBody] Profissao profissao)
        {
            var profissaoBase = _database.Profissoes.Include(p => p.LocalTrabalho).Include(x => x.Tecnologia).FirstOrDefault(p => p.Id == profissao.Id);

            profissaoBase.Cargo = profissao.Cargo;
            profissaoBase.LocalTrabalhoId = profissao.LocalTrabalhoId;
            profissaoBase.Projeto = profissao.Projeto;
            profissaoBase.TecnologiaId = profissao.TecnologiaId;
            profissaoBase.Inicio = DateTime.Now;

            profissaoBase.LocalTrabalho = _database.GFts.FirstOrDefault(p => p.Id == profissao.LocalTrabalhoId);
            profissaoBase.Tecnologia = _database.Tecnologias.FirstOrDefault(x => x.Id == profissao.TecnologiaId);

            _database.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarProfissao(int id)
        {
            var profissao = _database.Profissoes.Include(p => p.LocalTrabalho).Include(x => x.Tecnologia).FirstOrDefault(x => x.Id == id);

            _database.Profissoes.Remove(profissao);
            _database.SaveChanges();
            
            return Ok();
        }
    }
}