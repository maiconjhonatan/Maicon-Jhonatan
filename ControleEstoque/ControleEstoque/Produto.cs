using System;

namespace ControleEstoque
{
    // ===================== CLASSE PRODUTO =====================
    public class Produto
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }
        public int Quantidade { get; private set; }
        public int EstoqueMinimo { get; private set; }

        public Produto(int id, string nome, decimal preco, int quantidade, int estoqueMinimo)
        {
            Id = id;
            Nome = nome;
            Preco = preco;
            Quantidade = quantidade;
            EstoqueMinimo = estoqueMinimo;
        }

        public void AdicionarQuantidade(int qtd)
        {
            Quantidade += qtd;
        }

        public bool RemoverQuantidade(int qtd)
        {
            if (qtd > Quantidade)
            {
                return false; // não pode ficar negativo
            }
            Quantidade -= qtd;
            return true;
        }

        public bool EstaAbaixoDoMinimo()
        {
            return Quantidade <= EstoqueMinimo;
        }
    }
}
