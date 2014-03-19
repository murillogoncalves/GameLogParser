using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuakeLogParser;
using QuakeLogParser.Enum;
using QuakeLogParser.Model;

namespace UnitTest
{
    /// <summary>
    /// Testes unitários.
    /// </summary>
    [TestClass]
    public class QuakeLogParserUnitTest
    {
        /// <summary>
        /// Teste unitário do método "InitGame". 
        /// Verifica se um novo Game foi iniciado.
        /// </summary>
        [TestMethod]
        public void TestInitGame()
        {
            LogParser testParser = new LogParser();

            //Inicia um novo jogo.
            Game expected = new Game();
            Game actual = testParser.InitGame();

            Assert.IsTrue(expected.GetType() == actual.GetType(), "[InitGame] Não apresentou o comportamento esperado.");

            //Assert.AreEqual<Game>(expected, actual, "[InitGame] Não apresentou o comportamento esperado.");
        }

        /// <summary>
        /// Teste unitário do método "ClientConnect". 
        /// Verifica se um jogador é incluso na lista de jogadores do jogo, e se ele não é repetido.
        /// </summary>
        [TestMethod]
        public void TestClientConnect()
        {
            LogParser testParser = new LogParser();

            //Adiciona um jogador no jogo
            Game game = testParser.InitGame();
            testParser.ClientConnect(game, "20:38 ClientConnect: 2");
            testParser.ClientConnect(game, "20:39 ClientConnect: 2");

            int expected = 1;
            int actual = game.players.Count;

            Assert.AreEqual<int>(expected, actual, "[ClientConnect] Não apresentou o comportamento esperado.");
            
        }

        /// <summary>
        /// Teste unitário do método "ClientUserInfoChanged". 
        /// Verifica se o nome de um jogador é inserido e alterado.
        /// </summary>
        [TestMethod]
        public void TestClientUserInfoChanged()
        {
            LogParser testParser = new LogParser();

            //Adiciona um jogador no jogo
            Game game = testParser.InitGame();
            testParser.ClientConnect(game, "21:51 ClientConnect: 3");

            //Verifica se o jogador tem seu nome alterado.
            testParser.ClientUserInfoChanged(game, @"21:51 ClientUserinfoChanged: 3 n\Dono da Bola\t\0\model\sarge/krusade\hmodel\sarge/krusade\g_redteam\\g_blueteam\\c1\5\c2\5\hc\95\w\0\l\0\tt\0\tl\0");
            string expected = "Dono da Bola";
            string actual = game.players.Find(p => p.id == 3).nome;
            Assert.AreEqual<string>(expected, actual, "[ClientUserInfoChanged] Não apresentou o comportamento esperado ao setar o nome.");

            //Verifica se o mesmo jogador tem seu nome alterado novamente.
            testParser.ClientUserInfoChanged(game, @"21:53 ClientUserinfoChanged: 3 n\Mocinha\t\0\model\sarge\hmodel\sarge\g_redteam\\g_blueteam\\c1\4\c2\5\hc\95\w\0\l\0\tt\0\tl\0");
            expected = "Mocinha";
            actual = game.players.Find(p => p.id == 3).nome;
            Assert.AreEqual<string>(expected, actual, "[ClientUserInfoChanged] Não apresentou o comportamento esperado ao alterar o nome.");
        }

        /// <summary>
        /// Teste unitário do método "Kill". 
        /// Verifica se mortes sao incrementadas, decrementadas e agrupadas por modo.
        /// </summary>
        [TestMethod]
        public void TestKill()
        {
            LogParser testParser = new LogParser();

            //Adiciona um jogador no jogo
            Game game = testParser.InitGame();
            testParser.ClientConnect(game, "11:23 ClientConnect: 3");
            testParser.ClientConnect(game, "11:23 ClientConnect: 4");
            testParser.ClientConnect(game, "11:23 ClientConnect: 7");

            //Insere nome no jogador.
            testParser.ClientUserInfoChanged(game, @"11:23 ClientUserinfoChanged: 3 n\Isgalamido\t\0\model\uriel/zael\hmodel\uriel/zael\g_redteam\\g_blueteam\\c1\5\c2\5\hc\100\w\0\l\0\tt\0\tl\0");

            //Adiciona/decrementa mortes ao jogador
            testParser.Kill(game, @"11:48 Kill: 1022 3 19: <world> killed Isgalamido by MOD_FALLING");
            testParser.Kill(game, @"11:30 Kill: 3 4 6: Isgalamido killed Zeh by MOD_ROCKET");
            testParser.Kill(game, @"11:45 Kill: 3 7 7: Isgalamido killed Assasinu Credi by MOD_ROCKET_SPLASH");
            testParser.Kill(game, @"11:48 Kill: 1022 3 19: <world> killed Isgalamido by MOD_FALLING");

            int expected = 4;
            int actual = game.totalKills;
            Assert.AreEqual<int>(expected, actual, "[Kill] Não apresentou o comportamento esperado para o total de mortes do jogo.");

            expected = 1;
            actual = game.playersKills.Find(p => p.player.id == 3).kills;
            Assert.AreEqual<int>(expected, actual, "[Kill] Não apresentou o comportamento esperado para o total de mortes do jogador.");

            expected = 2;
            actual = game.killsByMeans.Find(k => k.mean == eMeansOfDeath.MOD_FALLING).numero;
            Assert.AreEqual<int>(expected, actual, "[Kill] Não apresentou o comportamento esperado para o total de mortes por modo.");
        }
    }
}
