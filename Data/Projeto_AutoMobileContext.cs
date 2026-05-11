using Microsoft.EntityFrameworkCore;
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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Veiculo>()
        //        .HasDiscriminator<string>("TipoVeiculo")
        //        .HasValue<Camiao>("Camiao")
        //        .HasValue<Camioneta>("Camioneta")
        //        .HasValue<Mota>("Mota")
        //        .HasValue<Carro>("Carro");

        //}
    }
}
