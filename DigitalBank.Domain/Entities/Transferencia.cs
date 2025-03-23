using DigitalBank.Util.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalBank.Domain.Entities;

[Table("TRANSFERENCIAS")]
public partial class Transferencia
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("conta_origem_id")]
    public Guid ContaOrigemId { get; private set; }

    [Required]
    [Column("conta_destino_id")]
    public Guid ContaDestinoId { get; private set; }

    [Required]
    [Column("valor")]
    public decimal Valor { get; private set; }

    [Column("data_transferencia")]
    public DateTime DataTransferencia { get; private set; }

    [ForeignKey("ContaOrigemId")]
    [InverseProperty("TransferenciasOrigem")]
    public virtual Conta ContaOrigem { get; private set; }

    [ForeignKey("ContaDestinoId")]
    [InverseProperty("TransferenciasDestino")]
    public virtual Conta ContaDestino { get; private set; }

    protected Transferencia() { }

    public Transferencia(Guid contaOrigemId, Guid contaDestinoId, decimal valor)
    {
        if (valor <= 0)
            throw new DomainException("Valor da transferência deve ser maior que zero.");

        Id = Guid.NewGuid();
        ContaOrigemId = contaOrigemId;
        ContaDestinoId = contaDestinoId;
        Valor = valor;
        DataTransferencia = DateTime.Now;
    }
}
