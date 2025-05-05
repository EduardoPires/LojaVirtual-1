using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes_Compartilhadas.Data;
using Classes_Compartilhadas.Models;
using Microsoft.EntityFrameworkCore;

namespace Classes_Compartilhadas.Services
{
    public class VendedorService
    {

        private readonly ApplicationDbContext _context;

        public VendedorService(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task InserirVendedor(Vendedor vendedor)
        {
            _context.Vendedor.Add(vendedor);
            await _context.SaveChangesAsync();

        }
    }
}
