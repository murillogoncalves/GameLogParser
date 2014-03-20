using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using QuakeLogParser.Model;
using System.Data.Linq;
using QuakeLogParser.Enum;
using System.Text.RegularExpressions;
using System.Reflection;

namespace QuakeLogParser
{
    public class HandleInfo
    {
        #region Attributes

        /// <summary>
        /// Caminho [fixo] onde está armazenda o arquivo de log do jogo.
        /// </summary>
        private static readonly string logFile = Environment.CurrentDirectory + @"\Assets\games.log";
        /// <summary>
        /// Caminho onde será salvo o arquivo com informações do jogo processadas.
        /// </summary>
        private static readonly string path = Environment.CurrentDirectory + @"\Assets\ParsedInfo\logParsed.txt";

        #endregion

        #region Methods

        /// <summary>
        /// Processa as informações do arquivo de log e as armazena em um objeto.
        /// </summary>
        /// <returns>Objeto contendo informações processadas</returns>
        public static Games ProcessInfo()
        {
            Games games = new Games();
            Game game = new Game();
            LogParser logParser = new LogParser();

            //string initGame = System.Enum.GetName(typeof(eGameAction), eGameAction.InitGame);
            //string clientConnect = System.Enum.GetName(typeof(eGameAction), eGameAction.ClientConnect);
            //string clientUserinfoChanged = System.Enum.GetName(typeof(eGameAction), eGameAction.ClientUserinfoChanged);
            //string kill = System.Enum.GetName(typeof(eGameAction), eGameAction.Kill);
            //Regex regex = new Regex("/(" + initGame + "|" + clientConnect + "|" + clientUserinfoChanged + "|" + kill + ")/");

            //Expressão regular para recuperar ações importantes para o nosso contexto
            Regex regex = new Regex("(InitGame|ClientConnect|ClientUserinfoChanged|Kill)");

            foreach (string row in File.ReadAllLines(logFile, Encoding.UTF8))
            {
                //Regex regex = new Regex("InitGame");
                Match action = regex.Match(row);

                switch (action.Value)
                {
                    //Novo jogo iniciado
                    case "InitGame":
                        game = logParser.InitGame();
                        games.games.Add(game);
                        break;
                    //Novo jogador
                    case "ClientConnect":
                        logParser.ClientConnect(game, row);
                        break;
                    //Jogador alterou informação (nome)
                    case "ClientUserinfoChanged":
                        logParser.ClientUserInfoChanged(game, row);
                        break;
                    //Ocorreu uma morte no jogo
                    case "Kill":
                        logParser.Kill(game, row);
                        break;
                    //Outras ações (não importantes para o nosso contexto)
                    default:
                        //Do nothing here
                        break;
                }

            }

            return games;
        }

        /// <summary>
        /// Escreve os dados processados do arquivo de log do jogo em um novo arquivo texto.
        /// </summary>
        /// <param name="games">Objeto Games com as informações processadas</param>
        public static void WritteProcessedInfo(Games games)
        {
            var json = new JavaScriptSerializer().Serialize(games);

            if (!File.Exists(path))
            {
                File.Create(path);
            }
            File.WriteAllText(path, json, Encoding.UTF8);
        }

        /// <summary>
        /// Converte os dados processados em uma string legível.
        /// </summary>
        /// <param name="games">bjeto Games com os dados processados</param>
        /// <returns>String contendo dados legíveis</returns>
        public static string ReadProcessedInfo(Games games)
        {
            StringBuilder sb = new StringBuilder("Games: {");
            sb.AppendLine();
            
            foreach (Game game in games.games)
            {
                //Jogos
                sb.AppendLine("\tGame " + game.id.ToString() + ": {");

                    //Total de mortes
                    sb.AppendLine("\t\ttotal_kills: " + game.totalKills.ToString() + ";");
                    
                    //Lista de jogadores
                    sb.AppendLine("\t\tplayers: [");
                    foreach (Player player in game.players.OrderBy(p => p.nome).ToList())
                    {
                        sb.AppendLine("\t\t\t" + player.nome + ",");
                    }
                    sb.Remove(sb.Length - 2, 1); //retirar vírgula final
                    sb.AppendLine("\t\t]"); //players close

                    //Lista de mortes por jogador
                    sb.AppendLine("\t\tkills: {");
                    foreach (PlayerKill playerKill in game.playersKills.OrderByDescending(p => p.kills))
                    {
                        sb.Append("\t\t\t" + playerKill.player.nome + ": ");
                        sb.AppendLine(playerKill.kills.ToString() + ",");
                    }
                    sb.Remove(sb.Length - 2, 1); //retirar vírgula final
                    sb.AppendLine("\t\t}"); //kills close

                    //Lista de mortes por modo
                    sb.AppendLine("\t\tkills_by_means: {");
                    foreach (KillsByMean killsByMean in game.killsByMeans.OrderByDescending(p => p.numero))
                    {
                        sb.Append("\t\t\t" + System.Enum.GetName(typeof(eMeansOfDeath), killsByMean.mean) + ": ");
                        sb.AppendLine(killsByMean.numero.ToString() + ",");
                    }
                    sb.Remove(sb.Length - 2, 1); //retirar vírgula final
                    sb.AppendLine("\t\t}"); //kills_by_means close

                sb.AppendLine("\t}"); //Game close
            }

            sb.AppendLine("}"); //Games close

            return sb.ToString();
        }

        /// <summary>
        /// Apaga o arquivo processado.
        /// </summary>
        public static void DeleteProcessedFile()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        #endregion
    }
}
