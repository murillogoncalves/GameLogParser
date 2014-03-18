using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeLogParser.Enum
{
    /// <summary>
    /// Possíveis ações apresentadas no jogo
    /// </summary>
    public enum GameAction
    {
        InitGame,
        ShutdownGame,
        Exit,
        ClientConnect,
        ClientDisconnect,
        ClientBegin,
        ClientUserinfoChanged,
        Item,
        Kill,
        score
    }
}
