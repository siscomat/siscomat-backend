using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Siscomat.Core.Entities;
using Siscomat.Core.Interfaces;

namespace Siscomat.Repositories
{
    public class GestorRepository : IGestorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Gestor> _dbSet;

        public GestorRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Gestor>();
        }

        async Task IGestorRepository.AddAsync(Gestor gestor)
        {
            await _dbSet.AddAsync(gestor);
            await _context.SaveChangesAsync();
        }

        async Task IGestorRepository.DeleteAsync(int id)
        {
            await _dbSet.Where(g => g.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();  
        }

        async Task<IEnumerable<Gestor>> IGestorRepository.GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        async Task<Gestor?> IGestorRepository.GetByCorreoAsync(string correo)
        {
            return await _dbSet.Where(g => g.Correo == correo).FirstOrDefaultAsync();
        }

        async Task<Gestor?> IGestorRepository.GetByIdAsync(int id)
        {
            return await _dbSet.Where(g => g.Id == id).FirstOrDefaultAsync();
        }

        async Task<int> IGestorRepository.SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }

        async Task IGestorRepository.UpdateAsync(Gestor gestor)
        {
            await _dbSet.Where(g => g.Id == gestor.Id).ExecuteUpdateAsync(s => s
                .SetProperty(g => g.Nombre, gestor.Nombre)
                .SetProperty(g => g.Apellido1, gestor.Apellido1)
                .SetProperty(g => g.Apellido2, gestor.Apellido2)
                .SetProperty(g => g.Correo, gestor.Correo)
                .SetProperty(g => g.PasswordHash, gestor.PasswordHash)
                .SetProperty(g => g.EsAdmin, gestor.EsAdmin));

            await _context.SaveChangesAsync();
        }
    }
}
