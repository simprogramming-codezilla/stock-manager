using System;
using System.Collections.Generic;

namespace GestorStockDomestico
{
    // COMPONENTE: View
    // RESPONSÁVEL: Carlos
    // RESPONSABILIDADE: Capturar input do utilizador e apresentar resultados
    // VARIANTE MVC: Curry & Grace — input entra na View

    class View
    {
        // ── Eventos de input (View → Controller) ──────────────────────────
        // O utilizador seleccionou uma opção do menu principal
        public delegate void OpcaoSelecionadaHandler(string opcao);
        public event OpcaoSelecionadaHandler? OpcaoSelecionada;

        // O utilizador introduziu dados de um produto para registo/actualização
        public delegate void DadosProdutoHandler(string nome, int quantidade, int quantidadeMinima, string unidade);
        public event DadosProdutoHandler? DadosProdutoIntroduzidos;

        // O utilizador pediu remoção de quantidade de um produto
        public delegate void RemocaoHandler(string nomeProduto, int quantidade);
        public event RemocaoHandler? RemocaoSolicitada;

        // ── Delegados para pedido de dados ao Model (ref) ─────────────────
        // Controller ligará este delegado ao Model no arranque
        public delegate void SolicitacaoListaProdutos(ref List<Produto> lista);
        public event SolicitacaoListaProdutos? PrecisoDeProdutos;

        public delegate void SolicitacaoListaReposicao(ref List<Produto> lista);
        public event SolicitacaoListaReposicao? PrecisoDeListaReposicao;


        // ── Métodos de apresentação (a implementar por Carlos) ────────────

        public void MostrarMenu()
        {
            // TODO (Carlos): apresentar menu principal e capturar opção
            // Após capturar, invocar: OpcaoSelecionada?.Invoke(opcao);
        }

        public void MostrarStock()
        {
            // TODO (Carlos): pedir lista ao Model via PrecisoDeProdutos(ref lista)
            // e apresentar cada produto no ecrã
        }

        public void PedirDadosProduto()
        {
            // TODO (Carlos): pedir nome, quantidade, mínimo e unidade ao utilizador
            // Após capturar, invocar: DadosProdutoIntroduzidos?.Invoke(...)
        }

        public void PedirRemocaoQuantidade()
        {
            // TODO (Carlos): pedir nome do produto e quantidade a remover
            // Após capturar, invocar: RemocaoSolicitada?.Invoke(...)
        }

        public void MostrarListaReposicao()
        {
            // TODO (Carlos): pedir lista via PrecisoDeListaReposicao(ref lista)
            // e apresentar produtos abaixo do mínimo
        }

        public void MostrarConfirmacao(string mensagem)
        {
            // TODO (Carlos): mostrar mensagem de confirmação
        }

        public void MostrarErro(string mensagem)
        {
            // TODO (Carlos): mostrar mensagem de erro
        }

        public void MostrarMensagemFinal()
        {
            // TODO (Carlos): apresentar mensagem de encerramento
        }
    }
}