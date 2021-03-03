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
    public class PessoaController : ControllerBase
    {
        private readonly AppDbContext _database;

        public PessoaController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult ListarPessoa()
        {
            var person = _database.Pessoas.Include(p => p.Profissao).Include(x => x.Profissao.Tecnologia).Include(p => p.Profissao.LocalTrabalho).ToList();

            return Ok(person);
        }

        [HttpPost]
        public IActionResult CriarPessoa(Pessoa pessoa)
        {
            if (ModelState.IsValid) {

                var profissao = _database.Profissoes.Include(p => p.LocalTrabalho).Include(x => x.Tecnologia).FirstOrDefault(p => p.Id == pessoa.ProfissaoId);
                profissao.LocalTrabalho = _database.GFts.FirstOrDefault(x => x.Id == profissao.LocalTrabalhoId);
                profissao.Tecnologia = _database.Tecnologias.FirstOrDefault(x => x.Id == profissao.TecnologiaId);

                pessoa.Profissao = profissao;

                _database.Pessoas.Add(pessoa);
                _database.SaveChanges();

                Response.StatusCode = 201;
                return new ObjectResult("");
            }
            else {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public IActionResult EditarPessoa([FromBody] Pessoa pessoa)
        {
            var person = _database.Pessoas.Include(p => p.Profissao).FirstOrDefault(p => p.Id == pessoa.Id);

            person.Nome = pessoa.Nome;
            person.ProfissaoId = pessoa.ProfissaoId;
            person.Telefone = pessoa.Telefone;
            person.Email = pessoa.Email;

            person.Profissao = _database.Profissoes.Include(p => p.LocalTrabalho).Include(p => p.Tecnologia).FirstOrDefault(p => p.Id == pessoa.ProfissaoId);

            _database.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarPessoa(int id)
        {
            var pessoa = _database.Pessoas.Include(p => p.Profissao).FirstOrDefault(p => p.Id == id);

            _database.Pessoas.Remove(pessoa);
            _database.SaveChanges();

            return Ok();
        }
    }
}