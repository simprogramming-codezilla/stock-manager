namespace GestorStockDomestico.Contracts
{
    public interface IProduto
    {
        string Nome { get; }
        int Quantidade { get; }
        int QuantidadeMinima { get; }
        string Unidade { get; }
    }
}
