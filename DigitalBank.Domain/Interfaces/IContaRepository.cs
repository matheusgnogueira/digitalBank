using DigitalBank.Domain.Entities;

namespace DigitalBank.Domain.Interfaces
{
    public interface IContaRepository
    {
        Task<Conta?> ObterPorDocumentoAsync(string documento);
        Task<Conta?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Conta>> BuscarAsync(string? nome, string? documento);
        Task InserirAsync(Conta conta);
        Task AtualizarAsync(Conta conta);
    }
}
