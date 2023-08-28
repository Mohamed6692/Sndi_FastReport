using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ActeAdministratif.Models;

namespace ActeAdministratif.Data
{
    public class SNDIContext : DbContext
    {
        public SNDIContext (DbContextOptions<SNDIContext> options)
            : base(options)
        {
        }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Document>()
                .HasIndex(d => d.Numero) // Indiquez la propriété à indexer
                .IsUnique(); // Indiquez que l'index doit être unique
        }

        public DbSet<ActeAdministratif.Models.Filiation>? Filiation { get; set; } = default!;
        public DbSet<ActeAdministratif.Models.Document> Document { get; set; } = default!;
        public DbSet<ActeAdministratif.Models.Enregistrer>? Enregistrer { get; set; } = default!;
        public DbSet<ActeAdministratif.Models.DemandeInit>? DemandeInit { get; set; } = default!;
        public DbSet<ActeAdministratif.Models.Country>? T_CONF_COUNTRY { get; set; } = default!;

    }
}
