using DigitalBank.Domain.Entities;

namespace DigitalBank.Domain.Interfaces
{
    public interface ITransferenciaRepository
    {
        Task InserirAsync(Transferencia transferencia);
        Task<IEnumerable<Transferencia>> ListarPorContaIdAsync(Guid contaId);
    }
}
