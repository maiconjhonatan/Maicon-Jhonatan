using System;
using MySqlConnector;

namespace ControleEstoque
{
    // ===================== CLASSE CONEXAO =====================
    // Cuida da conexÃ£o com o MySQL e cria o banco/tabela sozinha.
    // SE O SEU MYSQL TIVER SENHA, TROQUE AQUI EM BAIXO (const Senha).
    public static class Conexao
    {
        private const string Servidor = "127.0.0.1";
        private const string Porta = "3306";
        private const string Usuario = "root";
        private const string Senha = "root";
        private const string Banco = "estoque_db";

        private static string StringSemBanco
        {
            get { return "Server=" + Servidor + ";Port=" + Porta + ";User ID=" + Usuario + ";Password=" + Senha + ";"; }
        }

        private static string StringComBanco
        {
            get { return StringSemBanco + "Database=" + Banco + ";"; }
        }

        public static MySqlConnection Abrir()
        {
            MySqlConnection con = new MySqlConnection(StringComBanco);
            con.Open();
            return con;
        }

        // Cria o banco e a tabela (se ainda nÃ£o existirem) e pÃµe 3 produtos de exemplo.
        public static void Inicializar()
        {
            using (MySqlConnection con = new MySqlConnection(StringSemBanco))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "CREATE DATABASE IF NOT EXISTS " + Banco + " CHARACTER SET utf8mb4", con);
                cmd.ExecuteNonQuery();
            }

            using (MySqlConnection con = Abrir())
            {
                string criar =
                    "CREATE TABLE IF NOT EXISTS produtos (" +
                    "id INT AUTO_INCREMENT PRIMARY KEY, " +
                    "nome VARCHAR(100) NOT NULL, " +
                    "preco DECIMAL(10,2) NOT NULL, " +
                    "quantidade INT NOT NULL, " +
                    "estoque_minimo INT NOT NULL)";
                new MySqlCommand(criar, con).ExecuteNonQuery();

                long total = Convert.ToInt64(new MySqlCommand("SELECT COUNT(*) FROM produtos", con).ExecuteScalar());
                if (total == 0)
                {
                    string exemplos =
                        "INSERT INTO produtos (nome, preco, quantidade, estoque_minimo) VALUES " +
                        "('Arroz 5kg', 28.90, 20, 5), " +
                        "('FeijÃ£o 1kg', 8.50, 3, 5), " +
                        "('Ã“leo de soja', 7.20, 15, 4)";
                    new MySqlCommand(exemplos, con).ExecuteNonQuery();
                }
            }
        }
    }
}
