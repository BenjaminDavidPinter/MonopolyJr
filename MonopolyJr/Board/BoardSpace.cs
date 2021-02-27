using System;
using System.Collections.Generic;
using MonopolyJr.PlayerModel;

namespace MonopolyJr.BoardModel
{
    public class BoardSpace
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public List<MonopolyPlayer> Players { get; set; }
        public ConsoleColor SpaceColor { get; set; }
        public ConsoleColor OwnedBy { get; set; }
        public BoardSpace()
        {
            OwnedBy = ConsoleColor.Black;
        }
    }
}
