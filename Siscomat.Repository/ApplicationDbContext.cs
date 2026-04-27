using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Siscomat.Core.Entities;

namespace Siscomat.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Gestor> Gestores { get; set; }
        public DbSet<Participante> Participantes { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Plantilla> Plantillas { get; set; }
        public DbSet<Constancia> Constancias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Participante>()
                .HasKey(p => p.Folio);
            modelBuilder.Entity<Participante>()
                .Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Gestor>()
                .Property(g => g.Correo)
                .IsRequired()
                .HasMaxLength(150);
            modelBuilder.Entity<Gestor>()
                .HasIndex(g => g.Correo)
                .IsUnique();
            modelBuilder.Entity<Gestor>()
                .Property(g => g.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<Plantilla>()
                .Property(p => p.path)
                .IsRequired();
        }
    }
}
