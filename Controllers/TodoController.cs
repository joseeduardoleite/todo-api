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
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _database;

        public TodoController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult ListarTodo()
        {
            var todos = _database.Todos.Include(p => p.Pessoa)
                                        .Include(x => x.Pessoa.Profissao)
                                        .Include(x => x.Pessoa.Profissao.Tecnologia)
                                        .Include(x => x.Pessoa.Profissao.LocalTrabalho)
                                        .Include(t => t.Tarefa).Where(x => x.isConcluido == false).ToList();

            return Ok(todos);
        }

        [HttpPost]
        public IActionResult CriarTodo([FromBody] Todo todo)
        {
            if (ModelState.IsValid) {
                var pessoa = _database.Pessoas.Include(p => p.Profissao)
                                              .Include(x => x.Profissao.LocalTrabalho)
                                              .Include(x => x.Profissao.Tecnologia)
                                              .FirstOrDefault(x => x.Id == todo.PessoaId);
                                              
                var tarefa = _database.Tarefas.FirstOrDefault(x => x.Id == todo.TarefaId);

                todo.Pessoa = pessoa;
                todo.Tarefa = tarefa;

                _database.Todos.Add(todo);
                _database.SaveChanges();

                Response.StatusCode = 201;
                return new ObjectResult("");
            }
            else {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public IActionResult EditarTodo([FromBody] Todo todo)
        {
            var todoNova = _database.Todos.Include(p => p.Pessoa)
                                          .Include(x => x.Pessoa.Profissao)
                                          .Include(x => x.Pessoa.Profissao.LocalTrabalho).Include(x => x.Pessoa.Profissao.Tecnologia)
                                          .Include(p => p.Tarefa)
                                          .FirstOrDefault(x => x.Id == todo.Id);

            todoNova.Nome = todo.Nome;
            todoNova.PessoaId = todo.PessoaId;
            todoNova.TarefaId = todo.TarefaId;
            
            todoNova.Pessoa = _database.Pessoas.Include(t => t.Profissao).FirstOrDefault(x => x.Id == todo.PessoaId);
            todoNova.Tarefa = _database.Tarefas.FirstOrDefault(o => o.Id == todo.TarefaId);

            _database.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarTodo(int id)
        {
            var todo = _database.Todos.Include(x => x.Tarefa).Include(x => x.Pessoa).FirstOrDefault(x => x.Id == id);

            _database.Todos.Remove(todo);
            _database.SaveChanges();

            return Ok();
        }

        [HttpPatch]
        [Route("completartodo/{id}")]
        public IActionResult CompletarTodo(int id)
        {
            var todo = _database.Todos.Include(x => x.Pessoa)
                                      .Include(x => x.Pessoa.Profissao)
                                      .Include(x => x.Pessoa.Profissao.LocalTrabalho)
                                      .Include(x => x.Pessoa.Profissao.Tecnologia)
                                      .Include(x => x.Tarefa)
                                      .FirstOrDefault(p => p.Id == id);

            todo.isConcluido = true;

            _database.SaveChanges();

            return new ObjectResult("");
        }
    }
}