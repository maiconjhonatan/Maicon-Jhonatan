using System.Collections.Generic;
using MySqlConnector;

namespace ControleEstoque
{
    // ===================== CLASSE ESTOQUE =====================
    // Todas as operações agora gravam e leem do banco MySQL.
    public class Estoque
    {
        private Produto LerProduto(MySqlDataReader r)
        {
            return new Produto(
                r.GetInt32("id"),
                r.GetString("nome"),
                r.GetDecimal("preco"),
                r.GetInt32("quantidade"),
                r.GetInt32("estoque_minimo"));
        }

        private List<Produto> Consultar(string sql, string parametro, object valor)
        {
            List<Produto> lista = new List<Produto>();
            using (MySqlConnection con = Conexao.Abrir())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                if (parametro != null)
                {
                    cmd.Parameters.AddWithValue(parametro, valor);
                }
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        lista.Add(LerProduto(r));
                    }
                }
            }
            return lista;
        }

        public Produto Cadastrar(string nome, decimal preco, int quantidade, int estoqueMinimo)
        {
            using (MySqlConnection con = Conexao.Abrir())
            {
                string sql = "INSERT INTO produtos (nome, preco, quantidade, estoque_minimo) " +
                             "VALUES (@nome, @preco, @qtd, @min)";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@preco", preco);
                cmd.Parameters.AddWithValue("@qtd", quantidade);
                cmd.Parameters.AddWithValue("@min", estoqueMinimo);
                cmd.ExecuteNonQuery();
                int novoId = (int)cmd.LastInsertedId;
                return new Produto(novoId, nome, preco, quantidade, estoqueMinimo);
            }
        }

        public List<Produto> ListarTodos()
        {
            return Consultar("SELECT * FROM produtos ORDER BY id", null, null);
        }

        public Produto BuscarPorId(int id)
        {
            List<Produto> lista = Consultar("SELECT * FROM produtos WHERE id = @id", "@id", id);
            if (lista.Count == 0) return null;
            return lista[0];
        }

        public List<Produto> BuscarPorNome(string texto)
        {
            return Consultar("SELECT * FROM produtos WHERE nome LIKE @texto ORDER BY id", "@texto", "%" + texto + "%");
        }

        public bool DarEntrada(int id, int qtd)
        {
            Produto p = BuscarPorId(id);
            if (p == null) return false;
            p.AdicionarQuantidade(qtd);
            AtualizarQuantidade(p);
            return true;
        }

        // retorna: 0 = ok, 1 = produto não existe, 2 = estoque insuficiente
        public int DarSaida(int id, int qtd)
        {
            Produto p = BuscarPorId(id);
            if (p == null) return 1;
            if (!p.RemoverQuantidade(qtd)) return 2;
            AtualizarQuantidade(p);
            return 0;
        }

        public bool Remover(int id)
        {
            using (MySqlConnection con = Conexao.Abrir())
            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM produtos WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<Produto> ProdutosEmAlerta()
        {
            return Consultar("SELECT * FROM produtos WHERE quantidade <= estoque_minimo ORDER BY id", null, null);
        }

        private void AtualizarQuantidade(Produto p)
        {
            using (MySqlConnection con = Conexao.Abrir())
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE produtos SET quantidade = @qtd WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@qtd", p.Quantidade);
                cmd.Parameters.AddWithValue("@id", p.Id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
