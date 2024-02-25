using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace RestAPI_NET.Models;

public partial class Iesdawivan02Context : DbContext
{
    public Iesdawivan02Context()
    {
    }

    public Iesdawivan02Context(DbContextOptions<Iesdawivan02Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Marcas> Marcas { get; set; }

    public virtual DbSet<Relojes> Relojes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    /*#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=db4free.net;database=iesdawivan02;user id=ivan_fdez02;password=palomeras", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.3.0-mysql"));
    */
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Marcas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("marcas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Relojes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("relojes");

            entity.HasIndex(e => e.IdMarca, "id_marca");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdMarca).HasColumnName("id_marca");
            entity.Property(e => e.Imagen).HasColumnName("imagen");
            entity.Property(e => e.Imagenblob)
                .HasMaxLength(255)
                .HasColumnName("imagenblob");
            entity.Property(e => e.Modelo)
                .HasMaxLength(255)
                .HasColumnName("modelo");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.oMarca).WithMany(p => p.Relojes)
                .HasForeignKey(d => d.IdMarca)
                .HasConstraintName("relojes_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
