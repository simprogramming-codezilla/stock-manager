using System;
using System.Collections.Generic;

namespace GestorStockDomestico
{
    // COMPONENTE: View
    // RESPONSÁVEL: Carlos
    // RESPONSABILIDADE:
    // - Captura input do utilizador
    // - Apresenta dados no ecrã
    // - NÃO contém lógica de negócio
    // - NÃO comunica diretamente com o Model
    // VARIANTE MVC: Curry & Grace — input entra na View

    class View
    {
        // ── Eventos (View → Controller) ──────────────────────────
        // A View apenas notifica ações do utilizador

        // Opção selecionada no menu principal
        public delegate void OpcaoSelecionadaHandler(string opcao);
        public event OpcaoSelecionadaHandler? OpcaoSelecionada;

        // Dados introduzidos para registo/atualização de produto
        public delegate void DadosProdutoHandler(string nome, int quantidade, int quantidadeMinima, string unidade);
        public event DadosProdutoHandler? DadosProdutoIntroduzidos;

        // Pedido de remoção de quantidade
        public delegate void RemocaoHandler(string nomeProduto, int quantidade);
        public event RemocaoHandler? RemocaoSolicitada;


        // ── Interface principal ───────────────────────────────────

        public void MostrarMenu()
        {
            Console.WriteLine("\n=== Gestor de Stock Doméstico ===");
            Console.WriteLine("1 - Mostrar stock");
            Console.WriteLine("2 - Registar/atualizar produto");
            Console.WriteLine("3 - Remover quantidade");
            Console.WriteLine("4 - Lista de reposição");
            Console.WriteLine("9 - Sair");

            Console.Write("Opção: ");
            string? opcao = Console.ReadLine();

            // Notifica o Controller
            OpcaoSelecionada?.Invoke(opcao ?? "");
        }


        // ── Apresentação de dados (Controller → View) ─────────────

        public void MostrarStock(List<Produto> lista)
        {
            Console.WriteLine("\n--- STOCK ---");

            if (lista.Count == 0)
            {
                Console.WriteLine("Sem produtos.");
                return;
            }

            foreach (var p in lista)
            {
                Console.WriteLine($"{p.Nome} - {p.Quantidade} {p.Unidade} (mín: {p.QuantidadeMinima})");
            }
        }

        public void MostrarListaReposicao(List<Produto> lista)
        {
            Console.WriteLine("\n--- REPOSIÇÃO ---");

            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhum produto abaixo do mínimo.");
                return;
            }

            foreach (var p in lista)
            {
                Console.WriteLine($"{p.Nome} - {p.Quantidade} {p.Unidade} (mín: {p.QuantidadeMinima})");
            }
        }


        // ── Captura de input ──────────────────────────────────────

        public void PedirDadosProduto()
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine() ?? "";

            int quantidade       = LerInteiro("Quantidade: ");
            int quantidadeMinima = LerInteiro("Quantidade mínima: ");

            Console.Write("Unidade: ");
            string unidade = Console.ReadLine() ?? "";

            // Envia dados para o Controller
            DadosProdutoIntroduzidos?.Invoke(nome, quantidade, quantidadeMinima, unidade);
        }

        public void PedirRemocaoQuantidade()
        {
            Console.Write("Nome do produto: ");
            string nome = Console.ReadLine() ?? "";

            int quantidade = LerInteiro("Quantidade a remover: ");

            // Envia pedido ao Controller
            RemocaoSolicitada?.Invoke(nome, quantidade);
        }


        // ── Método auxiliar interno da View ──────────────────────
        private int LerInteiro(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string texto = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(texto, out int valor))
                {
                    return valor;
                }

                Console.WriteLine("Valor inválido. Introduza um número inteiro.");
            }
        }


        // ── Feedback ao utilizador ────────────────────────────────

        public void MostrarConfirmacao(string mensagem)
        {
            Console.WriteLine($"✔ {mensagem}");
        }

        public void MostrarErro(string mensagem)
        {
            Console.WriteLine($"✖ {mensagem}");
        }

        public void MostrarMensagemFinal()
        {
            Console.WriteLine("\nPrograma encerrado. Obrigado.");
        }
    }
}