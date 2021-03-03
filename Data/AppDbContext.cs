using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;

namespace TodoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Tecnologia> Tecnologias { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Profissao> Profissoes { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<GFT> GFts { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}