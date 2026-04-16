using System;

namespace GestorStockDomestico
{
    // COMPONENTE: Controller
    // RESPONSÁVEL: Pedro (Líder) + Kelvin (fluxo interno)
    // RESPONSABILIDADE: Único componente que conhece os restantes.
    //                   Faz todas as ligações entre View e Model no arranque.
    //                   Coordena o fluxo da aplicação (Curry & Grace: input na View).

    class Controller
    {
        private View view;
        private Model model;

        public Controller()
        {
            view  = new View();
            model = new Model();

            // ── Carregar dados persistidos ─────────────────────────────────
            model.CarregarDados();

            // ── Ligações: eventos de input da View → Controller ────────────
            // Quando o utilizador selecciona uma opção, o Controller processa
            view.OpcaoSelecionada         += ProcessarOpcao;
            view.DadosProdutoIntroduzidos += RegistarOuAtualizarProduto;
            view.RemocaoSolicitada        += RemoverQuantidade;

            // ── Ligações: pedidos de dados da View → Model (ref) ──────────
            // A View desconhece o Model — o Controller faz a ponte no arranque
            view.PrecisoDeProdutos        += model.SolicitarListaProdutos;
            view.PrecisoDeListaReposicao  += model.SolicitarListaReposicao;

            // ── Ligações: eventos de notificação do Model → View ──────────
            // O Model notifica resultado — o Controller encaminha para a View
            model.OperacaoConcluida       += view.MostrarConfirmacao;
            model.ErroStockInsuficiente   += view.MostrarErro;
        }

        public void IniciarPrograma()
        {
            // Inicia o ciclo principal — a View toma o controlo do input
            view.MostrarMenu();
        }


        // ── Handlers dos eventos da View ───────────────────────────────────

        private void ProcessarOpcao(string opcao)
        {
            // TODO (Kelvin): implementar lógica de navegação do menu
            switch (opcao)
            {
                case "1":
                    view.MostrarStock();
                    break;
                case "2":
                    view.PedirDadosProduto();
                    break;
                case "3":
                    view.PedirRemocaoQuantidade();
                    break;
                case "4":
                    view.MostrarListaReposicao();
                    break;
                case "9":
                    Encerrar();
                    break;
                default:
                    view.MostrarErro("Opção inválida.");
                    view.MostrarMenu();
                    break;
            }
        }

        private void RegistarOuAtualizarProduto(string nome, int quantidade, int quantidadeMinima, string unidade)
        {
            // TODO (Kelvin): validar dados se necessário antes de passar ao Model
            model.RegistarOuAtualizarProduto(nome, quantidade, quantidadeMinima, unidade);
        }

        private void RemoverQuantidade(string nomeProduto, int quantidade)
        {
            // TODO (Kelvin): validar dados se necessário antes de passar ao Model
            model.RemoverQuantidade(nomeProduto, quantidade);
        }

        private void Encerrar()
        {
            model.GuardarDados();
            view.MostrarMensagemFinal();
            Environment.Exit(0);
        }
    }
}
