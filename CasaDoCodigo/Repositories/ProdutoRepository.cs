using CasaDoCodigo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private readonly ICategoriaRepository categoriaRepository;

        public ProdutoRepository(ApplicationContext contexto, ICategoriaRepository categoriaRepository) : base(contexto)
        {
            this.categoriaRepository = categoriaRepository;
        }

        public async Task SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (!dbSet.Where(p => p.Codigo == livro.Codigo).Any())
                {
                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco, ObterCategoria(livro.Categoria)));
                }
            }
            await contexto.SaveChangesAsync();
        }

        private Categoria ObterCategoria(string categoria)
        {
            if (string.IsNullOrEmpty(categoria))
                return null;
            categoriaRepository.SaveNomeCategoria(categoria).Wait();
            return categoriaRepository.GetCategoria(categoria);
        }


        public async Task<IList<Produto>> GetProdutos(string filtro)
        {
            IList<Produto> produtos = new List<Produto>();

            produtos = await dbSet
                    .Include(c => c.Categoria)
                    .Where(p => p.Nome.ToUpper().Contains(filtro.ToUpper()) ||
                           p.Categoria.NomeCategoria.ToUpper().Contains(filtro.ToUpper()))
                    .ToListAsync();

            if (produtos.Count <= 0)
            {
                produtos = await dbSet
                    .Include(c => c.Categoria)
                    .ToListAsync();
            }

            return produtos;
        }

        public IList<Produto> GetProdutos()
        {
            return dbSet
                .Include(c => c.Categoria)
                .ToList();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Preco { get; set; }
    }
}
