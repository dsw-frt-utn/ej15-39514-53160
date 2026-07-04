using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data
{
    public class Dsw2026Ej15DbContext: DbContext
    {

        //el contexto trabaja con las entidades o debe saber de ellas
        // y persistirlas o traerlas de la persistencia
        // en este caso (hay otras configuraciones) se definen las colecciones que seran una abstraccion en la base de datos.
        //dbset enlaza nuestras entidades con la base de datos
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Speciality> Specialities { get; set; }

        // dbbuilder (el options que esta en el program) crea una instancia de options y se la pasa a este constructor
        public Dsw2026Ej15DbContext(DbContextOptions<Dsw2026Ej15DbContext> options) : base(options)
        {}

        //configuro caracteristicas de las migraciones con FluentApi
        //tambien puedo hacer configuraciones individuales
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>(e =>
            {
                e.ToTable("Doctors");
                e.Property(p => p.Name).HasMaxLength(100).IsRequired();
                e.Property(p => p.LicenseNumber).HasMaxLength(50).IsRequired();
                e.HasIndex(p => p.LicenseNumber).IsUnique();
                e.HasOne(d => d.Speciality).WithMany().HasForeignKey(d => d.SpecialityId);
            });

            modelBuilder.Entity<Speciality>(e =>
            {
                e.ToTable("Specialities");
                e.Property(p => p.Name).HasMaxLength(100).IsRequired();
                e.Property(p => p.Description).HasMaxLength(200).IsRequired();
            });
        }
    }
}
