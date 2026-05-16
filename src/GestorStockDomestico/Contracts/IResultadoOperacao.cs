namespace GestorStockDomestico.Contracts
{
    public interface IResultadoOperacao
    {
        bool Sucesso { get; }
        string Mensagem { get; }
    }
}
