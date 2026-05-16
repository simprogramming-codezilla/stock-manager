using GestorStockDomestico.Contracts;

namespace GestorStockDomestico
{
    // Classe de dados partilhada entre os componentes MVC
    // NOVO (T5.7): agora implementa a interface IProduto
    class Produto : IProduto
    {
        // NOVO (T5.7): propriedades tornam-se read-only (get apenas)
        public string Nome { get; }
        public int Quantidade { get; }
        public int QuantidadeMinima { get; }
        public string Unidade { get; }

        public Produto(string nome, int quantidade, int quantidadeMinima, string unidade)
        {
            Nome = nome;
            Quantidade = quantidade;
            QuantidadeMinima = quantidadeMinima;
            Unidade = unidade;
        }
    }
}
