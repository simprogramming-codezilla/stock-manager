using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GestorStockDomestico
{
    // COMPONENTE: Model
    // RESPONSÁVEL: Alexandre
    // RESPONSABILIDADE: Lógica de negócio + persistência JSON (Json.NET)

    class Model
    {
        // Lista interna de produtos — estado do Model
        private List<Produto> _listaProdutos = new List<Produto>();

        // Ficheiro de persistência JSON
        private readonly string _ficheiroJson = "produtos.json";

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

        public void SolicitarListaReposicao(ref List<Produto> lista)
        {
            // TODO (Alexandre): filtrar produtos com Quantidade < QuantidadeMinima
            // e copiá-los para lista

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


        // ── Métodos de lógica de negócio (a implementar por Alexandre) ────

        public void RegistarOuAtualizarProduto(string nome, int quantidade, int quantidadeMinima, string unidade)
        {
            // TODO (Alexandre): adicionar novo produto ou actualizar existente
            // Após sucesso: OperacaoConcluida?.Invoke("Produto registado com sucesso.");
            // Persistir em JSON via Json.NET

            Produto? produto = _listaProdutos.Find(
                p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (produto == null)
            {
                _listaProdutos.Add(new Produto(nome, quantidade, quantidadeMinima, unidade));
            }
            else
            {
                produto.Quantidade = quantidade;
                produto.QuantidadeMinima = quantidadeMinima;
                produto.Unidade = unidade;
            }

            GuardarDados();
            OperacaoConcluida?.Invoke("Produto registado com sucesso.");
        }

        public void RemoverQuantidade(string nomeProduto, int quantidade)
        {
            // TODO (Alexandre): verificar se há stock suficiente
            // Se não: ErroStockInsuficiente?.Invoke("Stock insuficiente.");
            // Se sim: decrementar e persistir
            // Após sucesso: OperacaoConcluida?.Invoke("Quantidade removida.");

            Produto? produto = _listaProdutos.Find(
                p => p.Nome.Equals(nomeProduto, StringComparison.OrdinalIgnoreCase));

            if (produto == null || produto.Quantidade < quantidade)
            {
                ErroStockInsuficiente?.Invoke("Stock insuficiente.");
                return;
            }

            produto.Quantidade -= quantidade;
            GuardarDados();
            OperacaoConcluida?.Invoke("Quantidade removida.");
        }

        public void CarregarDados()
        {
            // TODO (Alexandre): carregar lista de produtos do ficheiro JSON via Json.NET

            if (!File.Exists(_ficheiroJson))
            {
                _listaProdutos = new List<Produto>();
                return;
            }

            string json = File.ReadAllText(_ficheiroJson);

            _listaProdutos = JsonConvert.DeserializeObject<List<Produto>>(json)
                            ?? new List<Produto>();
        }

        public void GuardarDados()
        {
            // TODO (Alexandre): guardar lista de produtos em ficheiro JSON via Json.NET

            string json = JsonConvert.SerializeObject(
                _listaProdutos,
                Formatting.Indented
            );

            File.WriteAllText(_ficheiroJson, json);
        }
    }
}
