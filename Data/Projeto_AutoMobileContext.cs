using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;

namespace Projeto_AutoMobile.Data

{
    public class Projeto_AutoMobileContext : DbContext
    {

        public Projeto_AutoMobileContext(DbContextOptions<Projeto_AutoMobileContext> options)
             : base(options)
        {
        }

        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Camiao> Camioes { get; set; }
        public DbSet<Camioneta> Camionetas { get; set; }

        public DbSet<Mota> Motas { get; set; }
        public DbSet<Carro> Carros { get; set; }

        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Veiculo>()
                .HasDiscriminator<string>("TipoVeiculo")
                .HasValue<Camiao>("Camiao")
                .HasValue<Camioneta>("Camioneta")
                .HasValue<Mota>("Mota")
                .HasValue<Carro>("Carro");

            // Preco com precision 
            modelBuilder.Entity<Reserva>()
                .Property(e => e.PrecoEstimado)
                .HasPrecision(8, 2);


            // Preco com precision
            modelBuilder.Entity<Veiculo>()
                .Property(p => p.PrecoDia)
                .HasPrecision(8, 2);


            // Matrícula única para veículos
            modelBuilder.Entity<Veiculo>()
                .HasIndex(v => v.Matricula)
                .IsUnique();

            // NIF único para clientes
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.NIF)
                .IsUnique();
            // Carta de Condução única
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CartaConducao)
                .IsUnique();

            // Email único
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Email)
                .IsUnique();

            //Criar empresa
            modelBuilder.Entity<Empresa>().HasData(new Empresa
            {
                Id = 1,
                DataAtual = DateTime.Today
            });
        }
    }
}
