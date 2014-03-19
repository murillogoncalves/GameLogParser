using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuakeLogParser.Model;
using System.Text.RegularExpressions;
using QuakeLogParser.Enum;

namespace QuakeLogParser
{
    /// <summary>
    /// Classe que faz a leitura do arquivo e interpreta as informações.
    /// </summary>
    public class LogParser
    {
        /// <summary>
        /// Identificador o world, que está fixo no arquivo de log.
        /// </summary>
        private const int WORLD_ID = 1022;

        /// <summary>
        /// Inicia um novo jogo.
        /// </summary>
        /// <returns>Novo jogo</returns>
        public Game InitGame()
        {
            return new Game();
        }

        /// <summary>
        /// Adiciona um novo jogador na listagem do jogo.
        /// </summary>
        /// <param name="game">Jogo em que se deve adicionar o jogador</param>
        /// <param name="row">Linha do arquivo de log contendo as informações</param>
        public void ClientConnect(Game game, string row)
        {
            //Expressão regular para recuperar Id e Nome do jogador
            Regex regex = new Regex("/\\W\\W(?<id>[0-9]+)/");
            MatchCollection info = regex.Matches(row);

            //Recupera informações da linha
            int id = Int32.Parse(info[0].Value);

            //Adiciona um novo jogador, se já não existir
            if (!game.players.Any(p => p.id == id))
            {
                game.players.Add(new Player { id = id });
            }
        }

        /// <summary>
        /// Verifica informações alteradas dos jogadores.
        /// </summary>
        /// <param name="game">Jogo em que houve a alteração</param>
        /// <param name="row">Linha do arquivo de log contendo as informações</param>
        public void ClientUserInfoChanged(Game game, string row)
        {
            //Expressão regular para recuperar Id e Nome do jogador
            Regex regex = new Regex("/\\W\\W(?<id>[0-9]+) n\\\\(?<nome>[a-zA-Z\\s]+)/");
            MatchCollection info = regex.Matches(row);

            //Recupera informações da linha
            int id = Int32.Parse(info[0].Value);
            string nome = info[1].Value;

            //Seta o novo nome do jogador
            game.players.Find(p => p.id == id).nome = nome;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="row"></param>
        public void Kill(Game game, string row)
        {
            //Expressão regular para recuperar que matou, quem foi morto e o modo de morte
            Regex regex = new Regex("/\\W\\W([0-9]+)\\s([0-9]+)\\s([0-9]+)/");
            MatchCollection info = regex.Matches(row);

            //Recupera informações da linha
            Player killer = game.players.Find(p => p.id == Int32.Parse(info[0].Value));
            Player killed = game.players.Find(p => p.id == Int32.Parse(info[1].Value));
            eMeansOfDeath mean = (eMeansOfDeath)Int32.Parse(info[2].Value);

            //Atualiza total de mortes do jogo
            game.totalKills++;

            if (killer.id == WORLD_ID)
            {
                //Decrementa mortes de jogador que foi morto por <world>, se existir
                if (game.playersKills.Any(p => p.player.id == killed.id))
                {
                    game.playersKills.Find(k => k.player.id == killed.id).kills--;
                }
            }
            else
            {
                //Atualiza mortes dos jogadores
                if (game.playersKills.Any(p => p.player.id == killer.id))
                {
                    game.playersKills.Find(k => k.player.id == killer.id).kills++;
                }
                else
                {
                    game.playersKills.Add(new PlayerKill { player = killer, kills = 1 });
                }
            }

            //Atualiza mortes por modos
            if (game.killsByMeans.Any(k => k.mean == mean))
            {
                game.killsByMeans.Find(k => k.mean == mean).numero++;
            }
            else
            {
                game.killsByMeans.Add(new KillsByMean { mean = mean, numero = 1 });
            }
        }
    }
}
