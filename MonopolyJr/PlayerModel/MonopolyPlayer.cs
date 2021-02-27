using System;
using System.Collections.Generic;
using MonopolyJr.BoardModel;

namespace MonopolyJr.PlayerModel
{
    public class MonopolyPlayer
    {
        public int Money { get; set; }
        public List<BoardSpace> Spaces { get; set; }
        public ConsoleColor color { get; set; }
        public int PlayerNumber { get; set; }
        public int TotalBoardLoops { get; set; }
        public MonopolyPlayer()
        { 
        }
    }
}
