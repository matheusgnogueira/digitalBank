using DigitalBank.Domain.Entities;
using DigitalBank.Domain.Interfaces;
using DigitalBank.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalBank.Infra.Data.Repositories;

public class ContaRepository : IContaRepository
{
    private readonly AppDbContext _context;

    public ContaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AtualizarAsync(Conta conta)
    {
        _context.Contas.Update(conta);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Conta>> BuscarAsync(string? nome, string? documento)
    {
        var query = _context.Contas
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(c => EF.Functions.Like(c.Nome.ToLower(), $"%{nome.ToLower()}%"));

        if (!string.IsNullOrWhiteSpace(documento))
            query = query.Where(c => c.Documento == documento);

        return await query.ToListAsync();
    }


    public async Task<Conta?> ObterPorDocumentoAsync(string documento)
    {
        return await _context.Contas
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Documento == documento);
    }

    public async Task<Conta?> ObterPorIdAsync(Guid id)
    {
        return await _context.Contas
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task InserirAsync(Conta conta)
    {
        await _context.Contas.AddAsync(conta);
        await _context.SaveChangesAsync();
    }
}
