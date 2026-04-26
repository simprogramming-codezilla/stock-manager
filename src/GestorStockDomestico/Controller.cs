using System;
using System.Collections.Generic;

namespace GestorStockDomestico
{
    class Controller
    {
        private View view;
        private Model model;
        private bool _executando = true;

        public Controller()
        {
            view = new View();
            model = new Model();

            model.CarregarDados();

            // View → Controller
            view.OpcaoSelecionada         += ProcessarOpcao;
            view.DadosProdutoIntroduzidos += RegistarOuAtualizarProduto;
            view.RemocaoSolicitada        += RemoverQuantidade;

            // Model → View
            model.OperacaoConcluida      += view.MostrarConfirmacao;
            model.ErroStockInsuficiente  += view.MostrarErro;
        }

        public void IniciarPrograma()
        {
            while (_executando)
            {
                view.MostrarMenu();
            }
        }

        // =========================
        // FLUXO DO MENU
        // =========================

        private void ProcessarOpcao(string opcao)
        {
            switch (opcao)
            {
                case "1":
                    MostrarStock();
                    break;

                case "2":
                    view.PedirDadosProduto();
                    break;

                case "3":
                    view.PedirRemocaoQuantidade();
                    break;

                case "4":
                    MostrarListaReposicao();
                    break;

                case "9":
                    Encerrar();
                    break;

                default:
                    view.MostrarErro("Opção inválida.");
                    break;
            }
        }

        // =========================
        // FLUXO VIEW → CONTROLLER → MODEL → VIEW
        // =========================

        private void MostrarStock()
        {
            try
            {
                var lista = new List<Produto>();
                model.SolicitarListaProdutos(ref lista);
                view.MostrarStock(lista);
            }
            catch (Exception ex)
            {
                view.MostrarErro($"Erro ao obter stock: {ex.Message}");
            }
        }

        private void MostrarListaReposicao()
        {
            try
            {
                var lista = new List<Produto>();
                model.SolicitarListaReposicao(ref lista);
                view.MostrarListaReposicao(lista);
            }
            catch (Exception ex)
            {
                view.MostrarErro($"Erro ao obter lista de reposição: {ex.Message}");
            }
        }

        // =========================
        // INPUT → MODEL
        // =========================

        private void RegistarOuAtualizarProduto(string nome, int quantidade, int quantidadeMinima, string unidade)
        {
            try
            {
                model.RegistarOuAtualizarProduto(nome, quantidade, quantidadeMinima, unidade);
            }
            catch (Exception ex)
            {
                view.MostrarErro($"Erro ao registar produto: {ex.Message}");
            }
        }

        private void RemoverQuantidade(string nomeProduto, int quantidade)
        {
            try
            {
                model.RemoverQuantidade(nomeProduto, quantidade);
            }
            catch (Exception ex)
            {
                view.MostrarErro($"Erro ao remover quantidade: {ex.Message}");
            }
        }

        // =========================
        // ENCERRAR
        // =========================

        private void Encerrar()
        {
            try
            {
                model.GuardarDados();
                view.MostrarMensagemFinal();
            }
            catch (Exception ex)
            {
                view.MostrarErro($"Erro ao encerrar: {ex.Message}");
            }

            _executando = false;
        }
    }
}