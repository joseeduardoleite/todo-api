using System;
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
    public class TarefaController : ControllerBase
    {
        private readonly AppDbContext _database;

        public TarefaController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult ListarTarefa()
        {
            var tarefas = _database.Tarefas.Where(x => x.isConcluida == false).ToList();

            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult CriarTarefa([FromBody] Tarefa tarefaTemp)
        {
            Tarefa tarefa = new Tarefa();
            tarefa.Descricao = tarefaTemp.Descricao;
            tarefa.DataInicio = DateTime.Now;
            tarefa.isConcluida = false;

            _database.Tarefas.Add(tarefa);
            _database.SaveChanges();

            Response.StatusCode = 201;
            return new ObjectResult("");
        }

        [HttpPut]
        public IActionResult EditarTarefa([FromBody] Tarefa tarefa)
        {
            var tarefaBase = _database.Tarefas.FirstOrDefault(p => p.Id == tarefa.Id);

            tarefaBase.Descricao = tarefa.Descricao;
            tarefaBase.DataInicio = DateTime.Now;

            _database.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarTarefa(int id)
        {
            var tarefaBase = _database.Tarefas.FirstOrDefault(p => p.Id == id);

            _database.Tarefas.Remove(tarefaBase);
            _database.SaveChanges();

            return Ok();
        }

        [HttpPatch]
        [Route("completartarefa/{id}")]
        public IActionResult CompletarTarefa(int id)
        {
            var task = _database.Tarefas.FirstOrDefault(x => x.Id == id);

            task.isConcluida = true;

            _database.SaveChanges();

            return new ObjectResult("");
        }
    }
}