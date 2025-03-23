using DigitalBank.Domain.Entities;
using DigitalBank.Domain.Interfaces;
using DigitalBank.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalBank.Infra.Data.Repositories;

public class TransferenciaRepository : ITransferenciaRepository
{
    private readonly AppDbContext _context;

    public TransferenciaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transferencia>> ListarPorContaIdAsync(Guid contaId)
    {
        return await _context.Transferencias
            .AsNoTracking()
            .Where(t => t.ContaOrigemId == contaId || t.ContaDestinoId == contaId)
            .ToListAsync();
    }

    public async Task InserirAsync(Transferencia transferencia)
    {
        await _context.Transferencias.AddAsync(transferencia);
        await _context.SaveChangesAsync();
    }
}
