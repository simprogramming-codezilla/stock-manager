using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using GestorStockDomestico.Contracts;

namespace GestorStockDomestico
{
    // COMPONENTE: Model
    // RESPONSÁVEL: Alexandre
    // RESPONSABILIDADE: Lógica de negócio + persistência JSON (Json.NET)

    class Model
    {
        // Lista interna de produtos — estado do Model
        // NOVO (T5.7): agora usa IProduto (contrato)
        private List<IProduto> _listaProdutos = new List<IProduto>();

        // Ficheiro de persistência JSON
        private readonly string _ficheiroJson = "produtos.json";

        // ── Eventos de notificação (Model → Controller) ───────────────────
        public delegate void ErroStockHandler(string mensagem);
        public event ErroStockHandler? ErroStockInsuficiente;

        public delegate void ConfirmacaoHandler(string mensagem);
        public event ConfirmacaoHandler? OperacaoConcluida;


        // ── Métodos de resposta a pedidos ref ─────────────────────────────

        public void SolicitarListaProdutos(ref List<IProduto> lista)
        {
            // Deep copy — não expor estado interno
            lista.Clear();

            foreach (var p in _listaProdutos)
            {
                lista.Add(new Produto(
                    p.Nome,
                    p.Quantidade,
                    p.QuantidadeMinima,
                    p.Unidade
                ));
            }
        }

        public void SolicitarListaReposicao(ref List<IProduto> lista)
        {
            lista.Clear();

            foreach (var p in _listaProdutos)
            {
                if (p.Quantidade < p.QuantidadeMinima)
                {
                    lista.Add(new Produto(
                        p.Nome,
                        p.Quantidade,
                        p.QuantidadeMinima,
                        p.Unidade
                    ));
                }
            }
        }


        // ── Métodos de lógica de negócio ──────────────────────────────────

        public IResultadoOperacao RegistarOuAtualizarProduto(string nome, int quantidade, int quantidadeMinima, string unidade)
        {
            // Procurar produto existente
            var produtoExistente = _listaProdutos.Find(
                p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (produtoExistente == null)
            {
                // Criar novo produto (imutável)
                _listaProdutos.Add(new Produto(nome, quantidade, quantidadeMinima, unidade));
            }
            else
            {
                // Substituir instância (imutabilidade)
                _listaProdutos.Remove(produtoExistente);
                _listaProdutos.Add(new Produto(nome, quantidade, quantidadeMinima, unidade));
            }

            GuardarDados();
            OperacaoConcluida?.Invoke("Produto registado com sucesso.");

            return new ResultadoOperacao(true, "Produto registado com sucesso.");
        }

        public IResultadoOperacao RemoverQuantidade(string nomeProduto, int quantidade)
        {
            var produto = _listaProdutos.Find(
                p => p.Nome.Equals(nomeProduto, StringComparison.OrdinalIgnoreCase));

            if (produto == null || produto.Quantidade < quantidade)
            {
                ErroStockInsuficiente?.Invoke("Stock insuficiente.");
                return new ResultadoOperacao(false, "Stock insuficiente.");
            }

            // Criar nova instância com quantidade atualizada
            var novoProduto = new Produto(
                produto.Nome,
                produto.Quantidade - quantidade,
                produto.QuantidadeMinima,
                produto.Unidade
            );

            _listaProdutos.Remove(produto);
            _listaProdutos.Add(novoProduto);

            GuardarDados();
            OperacaoConcluida?.Invoke("Quantidade removida.");

            return new ResultadoOperacao(true, "Quantidade removida.");
        }


        // ── Persistência JSON ─────────────────────────────────────────────

        public void CarregarDados()
        {
            if (!File.Exists(_ficheiroJson))
            {
                _listaProdutos = new List<IProduto>();
                return;
            }

            try
            {
                string json = File.ReadAllText(_ficheiroJson);

                var lista = JsonConvert.DeserializeObject<List<Produto>>(json)
                            ?? new List<Produto>();

                _listaProdutos = new List<IProduto>(lista);
            }
            catch
            {
                _listaProdutos = new List<IProduto>();
            }
        }

        public void GuardarDados()
        {
            // Serializar como lista de Produto (classe concreta)
            var listaConcreta = new List<Produto>();

            foreach (var p in _listaProdutos)
            {
                listaConcreta.Add(new Produto(
                    p.Nome,
                    p.Quantidade,
                    p.QuantidadeMinima,
                    p.Unidade
                ));
            }

            string json = JsonConvert.SerializeObject(
                listaConcreta,
                Formatting.Indented
            );

            File.WriteAllText(_ficheiroJson, json);
        }
    }


    // ── Implementação concreta de IResultadoOperacao ─────────────────────

    class ResultadoOperacao : IResultadoOperacao
    {
        public bool Sucesso { get; }
        public string Mensagem { get; }

        public ResultadoOperacao(bool sucesso, string mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
    }
}
