using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonopolyJr.BoardModel;
using MonopolyJr.Engine;
using MonopolyJr.PlayerModel;
using System.Linq;

namespace MonopolyJr
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            List<MonopolyPlayer> winners = new List<MonopolyPlayer>();
            for (int i = 0; i < 1000000; i++)
            {
                MonopolyEngine engine = new MonopolyEngine(new Board(4, 16));
                while (!engine.GameOver)
                {
                    Task.Delay(250).GetAwaiter().GetResult();
                    engine.TakeTurn();
                }
                winners.Add(engine.GetWinner());
            }
            var swinners = winners.OrderByDescending(x => x.TotalBoardLoops);
        }
    }
}
