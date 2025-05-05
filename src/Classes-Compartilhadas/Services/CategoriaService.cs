using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes_Compartilhadas.Data;
using Classes_Compartilhadas.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore;

namespace Classes_Compartilhadas.Services
{
    public class CategoriaService
    {
        private readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }


        


        public async Task<IEnumerable<Categoria>> TodasCategorias()
        {
            return await _context.Categoria.ToListAsync();
        }

        public async Task<bool> CategoriaExiste(int id)
        {            
            return await _context.Categoria.AnyAsync(c => c.Id == id);
        }
        public async Task<bool> CategoriaPodeSerExcluida(int id)
        {
            return !await _context.Produto.AnyAsync(p => p.CategoriaId == id);
        }

        public async Task<bool> TemCategoria()
        {
            if (_context.Categoria == null)
                return false;

            return true;
        }

        public async Task<Categoria?> ObterCategoriaPorId(int? id)
        {
            return await _context.Categoria.FindAsync(id);
        }

        public async Task RemoverCategoria(Categoria categoria)
        {
            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();
        }
        public async Task InserirCategoria(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();

        }
        public async Task AtualizaCategoria(Categoria categoria)
        {
            _context.Categoria.Update(categoria);
            await _context.SaveChangesAsync();

        }




    }
}
