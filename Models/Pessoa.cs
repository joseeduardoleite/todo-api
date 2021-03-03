namespace TodoAPI.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int ProfissaoId { get; set; }
        public Profissao Profissao { get; set; }
    }
}