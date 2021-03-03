namespace TodoAPI.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool isConcluido { get; set; }
        public int PessoaId { get; set; }
        public int TarefaId { get; set; }
        public Pessoa Pessoa { get; set; }
        public Tarefa Tarefa { get; set; }

    }
}