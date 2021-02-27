using System;
using System.Linq;
using MonopolyJr.BoardModel;
using MonopolyJr.PlayerModel;

namespace MonopolyJr.Engine
{
    public class MonopolyEngine
    {
        private Board _monopolyBoard { get; set; }

        public bool GameOver { get; set; }

        private int PlayerTurn { get; set; }

        public MonopolyEngine(Board monopolyBoard)
        {
            _monopolyBoard = monopolyBoard;
            GameOver = false;
        }

        public void TakeTurn()
        {
            Random r = new Random();
            _monopolyBoard.MovePlayer(PlayerTurn, r.Next(1, 6));
            _monopolyBoard.DoSpace(PlayerTurn);
            this.GameOver = _monopolyBoard.CheckGameOver();
            _monopolyBoard.PrintBoard();
            if(PlayerTurn == _monopolyBoard.TotalPlayer-1)
            {
                PlayerTurn = 0;
            }
            else
            {
                PlayerTurn = PlayerTurn + 1;
            }
        }

        public MonopolyPlayer GetWinner()
        {
            return _monopolyBoard.GetWinner();
        }

    }
}
