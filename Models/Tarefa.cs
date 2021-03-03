using System;

namespace TodoAPI.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool isConcluida { get; set; }
        public DateTime DataInicio { get; set; }
    }
}