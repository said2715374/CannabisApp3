using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CannabisApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Provenances> Provenances { get; set; }
        public DbSet<Inventaire> Inventaire { get; set; }
        public DbSet<Plantes> Plantes { get; set; }
        public DbSet<Historique_Plantes> HistoriquePlantes { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Roles> Enterposage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
             "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;",
             
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provenances>().HasKey(p => p.IdProvenance);
            modelBuilder.Entity<Historique_Plantes>().HasKey(h => h.IdHistorique);
            modelBuilder.Entity<Historique_Plantes>()
                .HasOne<Utilisateur>()
                .WithMany()
                .HasForeignKey(h => h.IdUtilisateur)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Historique_Plantes>()
                .HasOne<Plantes>()
                .WithMany()
                .HasForeignKey(h => h.IdPlante)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Roles>().HasKey(r => r.IdRole);
            modelBuilder.Entity<Plantes>().HasKey(p => p.IdPlante);
            modelBuilder.Entity<Plantes>()
                .HasOne<Provenances>()
                .WithMany()
                .HasForeignKey(p => p.IdProvenance)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Inventaire>().HasKey(i => i.IdInventaire);
            modelBuilder.Entity<Inventaire>()
                .HasOne<Plantes>()
                .WithMany()
                .HasForeignKey(i => i.IdPlante)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Utilisateur>().HasKey(u => u.IdUtilisateur);
            modelBuilder.Entity<Utilisateur>()
                .HasOne<Roles>()
                .WithMany()
                .HasForeignKey(u => u.IdRole)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Enterposage>().HasKey(e => e.id);
            modelBuilder.Entity<Plantes>()
                .HasOne<Enterposage>()
                .WithMany()
                .HasForeignKey(p => p.id_Enterposage)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }

    public class Utilisateur
    {
        public int IdUtilisateur { get; set; }
        public string NomUtilisateur { get; set; }
        public string MotDePasse { get; set; }
        public int IdRole { get; set; }
    }

    public class Provenances
    {
        public int IdProvenance { get; set; }
        public string Ville { get; set; }
        public string Province { get; set; }
        public string Pays { get; set; }
    }

    public class Inventaire
    {
        public int IdInventaire { get; set; }
        public int IdPlante { get; set; }
        public int Quantite { get; set; }
        public DateTime DerniereVerification { get; set; }
    }

    public class Plantes
    {
        public int IdPlante { get; set; }
       
        
        public string CodeQr { get; set; }
        public int IdProvenance { get; set; }
        public Provenances Provenance { get; set; } // Propriété de navigation
        public int EtatSante { get; set; }
        public bool NombrePlantesActives { get; set; }
        public DateTime DateExpiration { get; set; }
        public DateTime CreeLe { get; set; }
        public string Stade { get; set; }
        public string Note { get; set; }
        public string Identification { get; set; }
        public int id_Enterposage { get; set; }
        public string Quentite { get; set; }
        public string Discription { get; set; }


        public string ProvenanceInfo => $"{Provenance.Ville}, {Provenance.Province}";
    }


    public class Roles
    {
        public int IdRole { get; set; }
        public string NomRole { get; set; }
    }
}
