using DigitalBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBank.Infra.Data.EntitiesConfiguration;

public class ContaConfiguration : IEntityTypeConfiguration<Conta>
{
    public void Configure(EntityTypeBuilder<Conta> builder)
    {
        builder.ToTable("CONTAS");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Documento)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.Documento).IsUnique();

        builder.Property(c => c.Saldo);
        builder.Property(c => c.Status);
        builder.Property(c => c.DataAbertura);
        builder.Property(c => c.DataInativacao);
        builder.Property(c => c.UsuarioInativacao).HasMaxLength(100);

        builder.HasMany(c => c.TransferenciasOrigem)
            .WithOne(t => t.ContaOrigem)
            .HasForeignKey(t => t.ContaOrigemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.TransferenciasDestino)
            .WithOne(t => t.ContaDestino)
            .HasForeignKey(t => t.ContaDestinoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
