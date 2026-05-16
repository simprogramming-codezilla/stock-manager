using GestorStockDomestico.Contracts;

namespace GestorStockDomestico
{
    class Produto : IProduto
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadeMinima { get; set; }
        public string Unidade { get; set; }

        public Produto(string nome, int quantidade, int quantidadeMinima, string unidade)
        {
            Nome = nome;
            Quantidade = quantidade;
            QuantidadeMinima = quantidadeMinima;
            Unidade = unidade;
        }
    }
}
