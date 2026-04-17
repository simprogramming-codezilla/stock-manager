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


        // ── Métodos de apresentação ───────────────────────────────────────

        public void MostrarMenu()
        {
            // Apresenta o menu principal e recolhe a opção do utilizador
            Console.WriteLine();
            Console.WriteLine("=== Gestor de Stock Doméstico ===");
            Console.WriteLine("1 - Mostrar stock");
            Console.WriteLine("2 - Registar/atualizar produto");
            Console.WriteLine("3 - Remover quantidade");
            Console.WriteLine("4 - Lista de reposição");
            Console.WriteLine("9 - Sair");
            Console.Write("Opção: ");

            string opcao = Console.ReadLine() ?? string.Empty;

            // Notifica o Controller para processar a opção seleccionada
            OpcaoSelecionada?.Invoke(opcao.Trim());
        }

        public void MostrarStock()
        {
            // Pede ao Model a lista completa de produtos
            if (PrecisoDeProdutos == null)
            {
                MostrarErro("Serviço de produtos indisponível.");
                return;
            }

            List<Produto> lista = new List<Produto>();
            PrecisoDeProdutos(ref lista);

            Console.WriteLine();
            Console.WriteLine("=== Stock Atual ===");

            if (lista.Count == 0)
            {
                Console.WriteLine("Não existem produtos registados.");
                return;
            }

            // Apresenta cada produto com quantidades e unidade
            foreach (Produto p in lista)
            {
                Console.WriteLine($"{p.Nome} - {p.Quantidade} {p.Unidade} (mínimo: {p.QuantidadeMinima})");
            }
        }

        public void PedirDadosProduto()
        {
            // Recolhe dados para registar ou actualizar um produto
            Console.WriteLine();
            Console.WriteLine("=== Registo/Atualização de Produto ===");

            Console.Write("Nome do produto: ");
            string nome = Console.ReadLine() ?? string.Empty;

            int quantidade = LerInteiro("Quantidade: ", permitirZero: false);
            int quantidadeMinima = LerInteiro("Quantidade mínima: ", permitirZero: true);

            Console.Write("Unidade (ex: kg, un, l): ");
            string unidade = Console.ReadLine() ?? string.Empty;

            // Envia os dados para o Controller tratar
            DadosProdutoIntroduzidos?.Invoke(nome.Trim(), quantidade, quantidadeMinima, unidade.Trim());
        }

        public void PedirRemocaoQuantidade()
        {
            // Recolhe os dados para remover quantidade de um produto
            Console.WriteLine();
            Console.WriteLine("=== Remoção de Quantidade ===");

            Console.Write("Nome do produto: ");
            string nomeProduto = Console.ReadLine() ?? string.Empty;

            int quantidade = LerInteiro("Quantidade a remover: ", permitirZero: false);

            // Envia o pedido para o Controller tratar
            RemocaoSolicitada?.Invoke(nomeProduto.Trim(), quantidade);
        }

        public void MostrarListaReposicao()
        {
            // Pede ao Model a lista de produtos abaixo do mínimo
            if (PrecisoDeListaReposicao == null)
            {
                MostrarErro("Serviço de reposição indisponível.");
                return;
            }

            List<Produto> lista = new List<Produto>();
            PrecisoDeListaReposicao(ref lista);

            Console.WriteLine();
            Console.WriteLine("=== Lista de Reposição ===");

            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhum produto abaixo do mínimo.");
                return;
            }

            // Apresenta os produtos que precisam de reposição
            foreach (Produto p in lista)
            {
                Console.WriteLine($"{p.Nome} - {p.Quantidade} {p.Unidade} (mínimo: {p.QuantidadeMinima})");
            }
        }

        public void MostrarConfirmacao(string mensagem)
        {
            // Mostra confirmação de operações bem-sucedidas
            Console.WriteLine();
            Console.WriteLine($"[OK] {mensagem}");
        }

        public void MostrarErro(string mensagem)
        {
            // Mostra erro quando algo falha
            Console.WriteLine();
            Console.WriteLine($"[ERRO] {mensagem}");
        }

        public void MostrarMensagemFinal()
        {
            // Mensagem final antes de encerrar o programa
            Console.WriteLine();
            Console.WriteLine("Programa encerrado. Obrigado.");
        }

        // ── Método auxiliar interno da View ────────────────────────────────

        private int LerInteiro(string prompt, bool permitirZero)
        {
            // Garante leitura de um inteiro válido
            int valor;

            while (true)
            {
                Console.Write(prompt);
                string texto = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(texto, out valor))
                {
                    if (permitirZero && valor >= 0)
                    {
                        return valor;
                    }

                    if (!permitirZero && valor > 0)
                    {
                        return valor;
                    }
                }

                Console.WriteLine("Valor inválido. Tente novamente.");
            }
        }
    }
}