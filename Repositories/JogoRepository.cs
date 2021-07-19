using APICatalagoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalagoJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
            {Guid.Parse("fab8ab3d-bf0c-4d47-826e-39a3a3d2fd26"), new Jogo{ Id = Guid.Parse("fab8ab3d-bf0c-4d47-826e-39a3a3d2fd26"), Nome = "FIFA 21", Produtora = "Eletronic Arts", Preco = 90 } },
            {Guid.Parse("6fb80df6-6b96-4995-95c8-1294a51ec48b"), new Jogo{ Id = Guid.Parse("6fb80df6-6b96-4995-95c8-1294a51ec48b"), Nome = "PES 2021", Produtora = "Konami", Preco = 70 } },
            {Guid.Parse("71b40d3a-73b3-4de9-a46d-598687fa0acf"), new Jogo{ Id = Guid.Parse("71b40d3a-73b3-4de9-a46d-598687fa0acf"), Nome = "Need for Speed Heat", Produtora = "Eletronic Arts", Preco = 100 } },
            {Guid.Parse("f1e1f7ec-3492-4f31-8bec-bd9c0c803427"), new Jogo{ Id = Guid.Parse("f1e1f7ec-3492-4f31-8bec-bd9c0c803427"), Nome = "Battlefield 2042", Produtora = "Eletronic Arts", Preco = 130 } },
            {Guid.Parse("8a9f40d3-dd22-4d58-b4bd-453a7774c861"), new Jogo{ Id = Guid.Parse("8a9f40d3-dd22-4d58-b4bd-453a7774c861"), Nome = "Battlefield V", Produtora = "Eletronic Arts", Preco = 70 } },
            {Guid.Parse("2f258e44-53d7-4d30-8eba-222897712908"), new Jogo{ Id = Guid.Parse("2f258e44-53d7-4d30-8eba-222897712908"), Nome = "FIFA 22", Produtora = "Eletronic Arts", Preco = 130 } },
        };

        public Task Atualizar(Jogo jogo)
        {
            jogos[jogo.Id] = jogo;
            return Task.CompletedTask;
        }

        public Task Inserir(Jogo jogo)
        {
            jogos.Add(jogo.Id, jogo);
            return Task.CompletedTask;
        }

        public Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(jogos.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Jogo> Obter(Guid id)
        {
            if (!jogos.ContainsKey(id))
                return null;

            return Task.FromResult(jogos[id]);
        }

        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            return Task.FromResult(jogos.Values.Where(jogo => jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora)).ToList());
        }

        public Task Remover(Guid id)
        {
            jogos.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Fechar conexão com o banco
        }
    }
}
