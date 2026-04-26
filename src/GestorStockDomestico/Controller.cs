using System;
using System.Collections.Generic;

namespace GestorStockDomestico
{
    class Controller
    {
        private View view;
        private Model model;

        private bool _emExecucao = true;

        public Controller()
        {
            view = new View();
            model = new Model();

            model.CarregarDados();

            // View → Controller
            view.OpcaoSelecionada += ProcessarOpcao;
            view.DadosProdutoIntroduzidos += RegistarOuAtualizarProduto;
            view.RemocaoSolicitada += RemoverQuantidade;

            //NOVO: View → Controller (pedidos de dados)
            view.PrecisoDeProdutos += OnPedirProdutos;
            view.PrecisoDeListaReposicao += OnPedirReposicao;

            // Model → View
            model.OperacaoConcluida += view.MostrarConfirmacao;
            model.ErroStockInsuficiente += view.MostrarErro;
        }

        public void IniciarPrograma()
        {
            while (_emExecucao)
            {
                view.MostrarMenu();
            }
        }

        private void ProcessarOpcao(string opcao)
        {
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
                    return;
                default:
                    view.MostrarErro("Opção inválida.");
                    break;
            }
        }

        private void RegistarOuAtualizarProduto(string nome, int quantidade, int quantidadeMinima, string unidade)
        {
            if (string.IsNullOrWhiteSpace(nome) || quantidade <= 0 || quantidadeMinima < 0)
            {
                view.MostrarErro("Dados inválidos.");
                return;
            }

            model.RegistarOuAtualizarProduto(nome, quantidade, quantidadeMinima, unidade);
        }

        private void RemoverQuantidade(string nomeProduto, int quantidade)
        {
            if (string.IsNullOrWhiteSpace(nomeProduto) || quantidade <= 0)
            {
                view.MostrarErro("Dados inválidos.");
                return;
            }

            model.RemoverQuantidade(nomeProduto, quantidade);
        }

        //NOVO: Controller passa a intermediar pedidos

        private void OnPedirProdutos(ref List<Produto> lista)
        {
            model.SolicitarListaProdutos(ref lista);
        }

        private void OnPedirReposicao(ref List<Produto> lista)
        {
            model.SolicitarListaReposicao(ref lista);
        }

        private void Encerrar()
        {
            model.GuardarDados();
            view.MostrarMensagemFinal();
            Environment.Exit(0);
        }
    }
}
