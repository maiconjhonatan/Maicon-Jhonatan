using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ControleEstoque
{
    // ===================== PROGRAMA PRINCIPAL =====================
    class Program
    {
        static Estoque estoque = new Estoque();
        static CultureInfo pt = new CultureInfo("pt-BR");

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // conecta no MySQL e cria banco/tabela se precisar
            try
            {
                Conexao.Inicializar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("NÃO FOI POSSÍVEL CONECTAR NO MYSQL!");
                Console.WriteLine("Confira se o MySQL está ligado (XAMPP/Workbench) e se usuário e senha");
                Console.WriteLine("estão certos no arquivo Conexao.cs.");
                Console.WriteLine();
              Console.WriteLine("Detalhe do erro: " + ex.ToString());
                Console.ReadLine();
                return;
            }

            int opcao = -1;
            while (opcao != 0)
            {
                MostrarMenu();
                opcao = LerInt("Escolha uma opção: ");
                Console.WriteLine();

                try
                {
                switch (opcao)
                {
                    case 1: CadastrarProduto(); break;
                    case 2: ListarProdutos(); break;
                    case 3: BuscarProduto(); break;
                    case 4: EntradaEstoque(); break;
                    case 5: SaidaEstoque(); break;
                    case 6: RemoverProduto(); break;
                    case 7: MostrarAlertas(); break;
                    case 0: Console.WriteLine("Saindo... Até logo!"); break;
                    default: Console.WriteLine("Opção inválida!"); break;
                }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro no banco de dados: " + ex.Message);
                }

                if (opcao != 0)
                {
                    Console.WriteLine();
                    Console.Write("Aperte ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=====================================");
            Console.WriteLine("      CONTROLE DE ESTOQUE SIMPLES    ");
            Console.WriteLine("=====================================");
            Console.WriteLine("1 - Cadastrar produto");
            Console.WriteLine("2 - Listar produtos");
            Console.WriteLine("3 - Buscar produto por nome");
            Console.WriteLine("4 - Entrada de estoque");
            Console.WriteLine("5 - Saída de estoque");
            Console.WriteLine("6 - Remover produto");
            Console.WriteLine("7 - Alertas de estoque baixo");
            Console.WriteLine("0 - Sair");
            Console.WriteLine("=====================================");
        }

        // ---------- funcionalidades ----------
        static void CadastrarProduto()
        {
            Console.WriteLine("--- CADASTRAR PRODUTO ---");
            string nome = LerTexto("Nome: ");
            decimal preco = LerDecimal("Preço (ex: 12,50): ");
            int qtd = LerInt("Quantidade inicial: ");
            int minimo = LerInt("Estoque mínimo: ");

            if (qtd < 0 || minimo < 0 || preco < 0)
            {
                Console.WriteLine("Valores não podem ser negativos!");
                return;
            }

            Produto p = estoque.Cadastrar(nome, preco, qtd, minimo);
            Console.WriteLine("Produto cadastrado com sucesso! Código: " + p.Id);
        }

        static void ListarProdutos()
        {
            Console.WriteLine("--- LISTA DE PRODUTOS ---");
            MostrarTabela(estoque.ListarTodos());
        }

        static void BuscarProduto()
        {
            Console.WriteLine("--- BUSCAR PRODUTO ---");
            string texto = LerTexto("Digite parte do nome: ");
            MostrarTabela(estoque.BuscarPorNome(texto));
        }

        static void EntradaEstoque()
        {
            Console.WriteLine("--- ENTRADA DE ESTOQUE ---");
            int id = LerInt("Código do produto: ");
            int qtd = LerInt("Quantidade que chegou: ");

            if (qtd <= 0)
            {
                Console.WriteLine("A quantidade deve ser maior que zero!");
                return;
            }

            if (estoque.DarEntrada(id, qtd))
                Console.WriteLine("Entrada registrada com sucesso!");
            else
                Console.WriteLine("Produto não encontrado!");
        }

        static void SaidaEstoque()
        {
            Console.WriteLine("--- SAÍDA DE ESTOQUE ---");
            int id = LerInt("Código do produto: ");
            int qtd = LerInt("Quantidade que saiu: ");

            if (qtd <= 0)
            {
                Console.WriteLine("A quantidade deve ser maior que zero!");
                return;
            }

            int resultado = estoque.DarSaida(id, qtd);
            if (resultado == 0)
            {
                Console.WriteLine("Saída registrada com sucesso!");
                Produto p = estoque.BuscarPorId(id);
                if (p.EstaAbaixoDoMinimo())
                {
                    Console.WriteLine("ATENÇÃO: '" + p.Nome + "' está com estoque baixo (" + p.Quantidade + ")!");
                }
            }
            else if (resultado == 1)
            {
                Console.WriteLine("Produto não encontrado!");
            }
            else
            {
                Console.WriteLine("Estoque insuficiente para essa saída!");
            }
        }

        static void RemoverProduto()
        {
            Console.WriteLine("--- REMOVER PRODUTO ---");
            int id = LerInt("Código do produto: ");

            if (estoque.Remover(id))
                Console.WriteLine("Produto removido com sucesso!");
            else
                Console.WriteLine("Produto não encontrado!");
        }

        static void MostrarAlertas()
        {
            Console.WriteLine("--- ALERTAS DE ESTOQUE BAIXO ---");
            List<Produto> lista = estoque.ProdutosEmAlerta();
            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhum produto com estoque baixo.");
                return;
            }
            MostrarTabela(lista);
        }

        // ---------- auxiliares ----------
        static void MostrarTabela(List<Produto> lista)
        {
            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhum produto encontrado.");
                return;
            }

            Console.WriteLine("{0,-5}{1,-20}{2,10}{3,8}{4,8}", "COD", "NOME", "PREÇO", "QTD", "MÍN");
            Console.WriteLine(new string('-', 51));
            foreach (Produto p in lista)
            {
                Console.WriteLine("{0,-5}{1,-20}{2,10}{3,8}{4,8}",
                    p.Id,
                    p.Nome.Length > 19 ? p.Nome.Substring(0, 19) : p.Nome,
                    p.Preco.ToString("C", pt),
                    p.Quantidade,
                    p.EstoqueMinimo);
            }
        }

        static string LerTexto(string mensagem)
        {
            string texto = "";
            while (string.IsNullOrWhiteSpace(texto))
            {
                Console.Write(mensagem);
                texto = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(texto))
                    Console.WriteLine("Não pode ficar vazio!");
            }
            return texto.Trim();
        }

        static int LerInt(string mensagem)
        {
            int valor;
            Console.Write(mensagem);
            while (!int.TryParse(Console.ReadLine(), out valor))
            {
                Console.Write("Número inválido! " + mensagem);
            }
            return valor;
        }

        static decimal LerDecimal(string mensagem)
        {
            decimal valor;
            Console.Write(mensagem);
            while (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, pt, out valor))
            {
                Console.Write("Valor inválido! " + mensagem);
            }
            return valor;
        }
    }
}
