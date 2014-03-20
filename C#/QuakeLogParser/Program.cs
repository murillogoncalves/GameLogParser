using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuakeLogParser.Model;

namespace QuakeLogParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //Processa informações do arquivo e armazena em um objeto
                Games games = HandleInfo.ProcessInfo();

                //Escreve informações processadas em arquivo, com um formato mais "amigável"
                HandleInfo.WritteProcessedInfo(games);

                //Recupera informações processadas em uma string
                string info = HandleInfo.ReadProcessedInfo(games);

                //Imprime as informações na tela
                Console.WriteLine(info);

                //Evita o fechamento do console sem a interação do usuário
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao processar as informações do arquivo de log: " + ex.Message);

                //Evita o fechamento do console sem a interação do usuário
                Console.ReadKey();
            }
        }
    }
}
