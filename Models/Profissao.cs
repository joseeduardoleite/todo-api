using System;

namespace TodoAPI.Models
{
    public class Profissao
    {
        public int Id { get; set; }
        public string Projeto { get; set; }
        public string Cargo { get; set; }
        public DateTime Inicio { get; set; }
        public int TecnologiaId { get; set; }
        public int LocalTrabalhoId { get; set; }
        public Tecnologia Tecnologia { get; set; }
        public GFT LocalTrabalho { get; set; }

    }
}