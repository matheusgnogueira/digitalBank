using DigitalBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBank.Infra.Data.EntitiesConfiguration;

public class TransferenciaConfiguration : IEntityTypeConfiguration<Transferencia>
{
    public void Configure(EntityTypeBuilder<Transferencia> builder)
    {
        builder.ToTable("TRANSFERENCIAS");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Valor).IsRequired();
        builder.Property(t => t.DataTransferencia).IsRequired();
    }
}
