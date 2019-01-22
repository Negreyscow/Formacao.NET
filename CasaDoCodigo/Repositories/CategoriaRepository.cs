using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{

    public interface ICategoriaRepository
    {
        Task SaveNomeCategoria(string NomeCategoria);
        Categoria GetCategoria(string nome);
    }

    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public Categoria GetCategoria(string nome)
        {
            return dbSet
                .Where(c => c.NomeCategoria.Equals(nome, StringComparison.OrdinalIgnoreCase))
                .SingleOrDefault();
        }


        public async Task SaveNomeCategoria(string NomeCategoria)
        {
            Categoria categoria = dbSet.Where(c => c.NomeCategoria == NomeCategoria).SingleOrDefault();

            if (categoria == null)
            {
                dbSet.Add(new Categoria(NomeCategoria));
                await contexto.SaveChangesAsync();
            }

        }

    }
}
