using System.Collections.Generic;

namespace GestorStockDomestico
{
    // COMPONENTE: Model
    // RESPONSÁVEL: Alexandre
    // RESPONSABILIDADE: Lógica de negócio + persistência JSON (Json.NET)

    class Model
    {
        // Lista interna de produtos — estado do Model
        private List<Produto> _listaProdutos = new List<Produto>();

        // ── Eventos de notificação (Model → Controller) ───────────────────
        // Notifica que ocorreu um erro de stock insuficiente
        public delegate void ErroStockHandler(string mensagem);
        public event ErroStockHandler? ErroStockInsuficiente;

        // Notifica que uma operação foi concluída com sucesso
        public delegate void ConfirmacaoHandler(string mensagem);
        public event ConfirmacaoHandler? OperacaoConcluida;


        // ── Métodos de resposta a pedidos ref (a implementar por Alexandre) ──

        public void SolicitarListaProdutos(ref List<Produto> lista)
        {
            // TODO (Alexandre): copiar _listaProdutos para lista (deep copy)
            // Não fazer: lista = _listaProdutos; (expõe estado interno)
        }

        public void SolicitarListaReposicao(ref List<Produto> lista)
        {
            // TODO (Alexandre): filtrar produtos com Quantidade < QuantidadeMinima
            // e copiá-los para lista
        }


        // ── Métodos de lógica de negócio (a implementar por Alexandre) ────

        public void RegistarOuAtualizarProduto(string nome, int quantidade, int quantidadeMinima, string unidade)
        {
            // TODO (Alexandre): adicionar novo produto ou actualizar existente
            // Após sucesso: OperacaoConcluida?.Invoke("Produto registado com sucesso.");
            // Persistir em JSON via Json.NET
        }

        public void RemoverQuantidade(string nomeProduto, int quantidade)
        {
            // TODO (Alexandre): verificar se há stock suficiente
            // Se não: ErroStockInsuficiente?.Invoke("Stock insuficiente.");
            // Se sim: decrementar e persistir
            // Após sucesso: OperacaoConcluida?.Invoke("Quantidade removida.");
        }

        public void CarregarDados()
        {
            // TODO (Alexandre): carregar lista de produtos do ficheiro JSON via Json.NET
        }

        public void GuardarDados()
        {
            // TODO (Alexandre): guardar lista de produtos em ficheiro JSON via Json.NET
        }
    }
}