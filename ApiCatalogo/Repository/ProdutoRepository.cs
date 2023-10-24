using ApiCatalogo.Models;
using ApiCatalogo.Models.Context;

namespace ApiCatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(CatalogoContext context) : base(context) 
        {
        }
        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }

    }

}
