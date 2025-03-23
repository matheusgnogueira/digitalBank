using DigitalBank.Util.Enums;
using DigitalBank.Util.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalBank.Domain.Entities;

[Table("CONTAS")]
public partial class Conta
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("nome")]
    [MaxLength(100)]
    public string Nome { get; private set; }

    [Required]
    [Column("documento")]
    [MaxLength(20)]
    public string Documento { get; private set; }

    [Column("saldo")]
    public decimal Saldo { get; private set; }

    [Column("data_abertura")]
    public DateTime DataAbertura { get; private set; }

    [Column("status")]
    public StatusConta Status { get; private set; }
    [Column("data_inativacao")]
    public DateTime? DataInativacao { get; private set; }

    [Column("usuario_inativacao")]
    [MaxLength(100)]
    public string? UsuarioInativacao { get; private set; }

    [InverseProperty("ContaOrigem")]
    public virtual ICollection<Transferencia> TransferenciasOrigem { get; private set; } = new List<Transferencia>();

    [InverseProperty("ContaDestino")]
    public virtual ICollection<Transferencia> TransferenciasDestino { get; private set; } = new List<Transferencia>();

    protected Conta() { }

    public Conta(string nome, string documento)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new DomainException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(documento)) throw new DomainException("Documento é obrigatório.");

        Id = Guid.NewGuid();
        Nome = nome;
        Documento = documento;
        Saldo = 1000;
        DataAbertura = DateTime.Now;
        Status = StatusConta.Ativa;
    }

    public void Inativar(string usuario)
    {
        if (!EstaAtiva())
            throw new DomainException("Conta já está inativa.");

        Status = StatusConta.Inativa;
        DataInativacao = DateTime.Now;
        UsuarioInativacao = usuario;
    }

    public void Debitar(decimal valor)
    {
        if (!EstaAtiva())
            throw new DomainException("A conta está inativa. Não é possível realizar a transferência.");

        if (!TemSaldoSuficiente(valor))
            throw new DomainException("Saldo insuficiente para realizar a transferência.");

        Saldo -= valor;
    }

    public void Creditar(decimal valor)
    {
        if (!EstaAtiva())
            throw new DomainException("A conta está inativa. Não é possível receber transferências.");

        Saldo += valor;
    }

    public bool EstaAtiva() => Status == StatusConta.Ativa;

    public bool TemSaldoSuficiente(decimal valor) => Saldo >= valor;
}
