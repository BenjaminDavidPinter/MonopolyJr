using System;
using System.Collections.Generic;
using System.Linq;
using MonopolyJr.PlayerModel;

namespace MonopolyJr.BoardModel
{
    public class Board
    {
        private int CURSOR_CONTROLLER;
        public BoardSpace[] spaces;
        public int TotalPlayer;
        public Board(int Players, int StartingMoney)
        {
            CURSOR_CONTROLLER = 0;
            TotalPlayer = Players;
            spaces = new BoardSpace[24];
            for (int i = 0; i < 24; i++)
            {
                spaces[i] = new BoardSpace();
                spaces[i].Players = new List<MonopolyPlayer>();
            }

            spaces[0].Name = "Go";
            spaces[0].Value = 0;
            spaces[0].SpaceColor = ConsoleColor.Green;
            spaces[0].OwnedBy = ConsoleColor.White;
            for (int i = 0; i < Players; i++)
            {
                spaces[0].Players.Add(new MonopolyPlayer()
                {
                    color = (ConsoleColor)i + 10,
                    Money = StartingMoney,
                    Spaces = new List<BoardSpace>(),
                    PlayerNumber = i
                });
            }

            spaces[6].Name = "Jail";
            spaces[6].Value = 0;
            spaces[6].SpaceColor = ConsoleColor.Red;
            spaces[6].OwnedBy = ConsoleColor.White;

            spaces[12].Name = "Free Parking";
            spaces[12].Value = 0;
            spaces[12].SpaceColor = ConsoleColor.Blue;
            spaces[12].OwnedBy = ConsoleColor.White;


            spaces[18].Name = "Go To Jail";
            spaces[18].Value = 0;
            spaces[18].SpaceColor = ConsoleColor.Red;
            spaces[18].OwnedBy = ConsoleColor.White;

            spaces[3].Name = "Chance";
            spaces[3].Value = 0;
            spaces[3].SpaceColor = ConsoleColor.Blue;
            spaces[3].OwnedBy = ConsoleColor.White;

            spaces[9].Name = "Chance";
            spaces[9].Value = 0;
            spaces[9].SpaceColor = ConsoleColor.Blue;
            spaces[9].OwnedBy = ConsoleColor.White;

            spaces[15].Name = "Chance";
            spaces[15].Value = 0;
            spaces[15].SpaceColor = ConsoleColor.Blue;
            spaces[15].OwnedBy = ConsoleColor.White;

            spaces[21].Name = "Chance";
            spaces[21].Value = 0;
            spaces[21].SpaceColor = ConsoleColor.Blue;
            spaces[21].OwnedBy = ConsoleColor.White;

            for (int i = 0; i < 24; i++)
            {
                if (i == 0 || i % 6 == 0 || i % 3 == 0)
                {
                    continue;
                } else if (i == 1 || i == 2)
                {
                    spaces[i].Value = 1;
                    spaces[i].Name = $"Brown-{Math.Abs(1-i)}";
                    spaces[i].SpaceColor = ConsoleColor.DarkYellow;
                }
                else if (i == 4 || i == 5)
                {
                    spaces[i].Value = 1;
                    spaces[i].Name = $"LightBlue-{Math.Abs(4 - i)}";
                    spaces[i].SpaceColor = ConsoleColor.Cyan;
                }
                else if (i == 7 || i == 8)
                {
                    spaces[i].Value = 2;
                    spaces[i].Name = $"Magenta-{Math.Abs(7 - i)}";
                    spaces[i].SpaceColor = ConsoleColor.Magenta;
                }
                else if (i == 10 || i == 11)
                {
                    spaces[i].Value = 2;
                    spaces[i].Name = $"Orange-{Math.Abs(10 - i)}";
                    spaces[i].SpaceColor = ConsoleColor.Red;
                }
                else if (i == 13 || i == 14)
                {
                    spaces[i].Value = 3;
                    spaces[i].Name = $"Red-{Math.Abs(13 - i)}";
                    spaces[i].SpaceColor = ConsoleColor.Red;
                }
                else if (i == 16 || i == 17)
                {
                    spaces[i].Value = 3;
                    spaces[i].Name = $"Yellow-{Math.Abs(16 - i)}";
                    spaces[i].SpaceColor = ConsoleColor.Yellow;
                }
                else if (i == 19 || i == 20)
                {
                    spaces[i].Value = 4;
                    spaces[i].Name = $"Green-{Math.Abs(19 - i)}";
                    spaces[i].SpaceColor = ConsoleColor.Green;
                }
                else if (i == 22 || i == 23)
                {
                    spaces[i].Value = 4;
                    spaces[i].Name = $"Blue-{Math.Abs(22 - i)}";
                    spaces[i].SpaceColor = ConsoleColor.Blue;
                }
            }
        }

        public int GetMaxBoardDepth()
        {
            return spaces.ToList().OrderByDescending(x => x.Players.Count).First().Players.Count();
        }

        public void PrintBoard()
        {
            Console.Clear();
            IEnumerable<KeyValuePair<MonopolyPlayer, int>> playerLocations = GetPlayerLocations();
            for(int i = 0; i < 24; i++)
            {
                Console.ForegroundColor = spaces[i].SpaceColor;
                Console.BackgroundColor = spaces[i].OwnedBy;
                Console.Write($"{spaces[i].Name.Substring(0, 2)}\t");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.WriteLine();
            foreach (var pl in playerLocations)
            {
                for (int i = 0; i < pl.Value; i++)
                {
                    Console.Write("\t");
                }
                Console.BackgroundColor = pl.Key.color;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($" {pl.Key.TotalBoardLoops}${pl.Key.Money}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
            Console.SetCursorPosition(0, 0);

        }

        public IEnumerable<KeyValuePair<MonopolyPlayer, int>> GetPlayerLocations()
        {
            List<KeyValuePair<MonopolyPlayer, int>> playerLocations = new List<KeyValuePair<MonopolyPlayer, int>>();
            for (int i = 0; i < 24; i++)
            {
                if (spaces[i].Players.Count() > 0)
                {
                    foreach (var pl in spaces[i].Players)
                    {
                        playerLocations.Add(new KeyValuePair<MonopolyPlayer, int>(pl, i));
                    }
                }
            }

            return playerLocations.OrderBy(x => x.Key.PlayerNumber);
        }

        public void MovePlayer(int playerNumber, int numberOfSpaces)
        {
            var players = GetPlayerLocations();
            var thisPlayer = players.Where(x => x.Key.PlayerNumber == playerNumber).First();
            spaces[thisPlayer.Value].Players.Remove(thisPlayer.Key);
            if (thisPlayer.Value + numberOfSpaces > 23)
            {
                spaces[(thisPlayer.Value + numberOfSpaces) - 23].Players.Add(thisPlayer.Key);
                thisPlayer.Key.TotalBoardLoops = thisPlayer.Key.TotalBoardLoops + 1;
                thisPlayer.Key.Money = thisPlayer.Key.Money + 1;
            }
            else
            {
                spaces[thisPlayer.Value + numberOfSpaces].Players.Add(thisPlayer.Key);
            }
        }

        public MonopolyPlayer GetPlayer(int playerNumber)
        {
            var players = GetPlayerLocations();
            return players.Where(x => x.Key.PlayerNumber == playerNumber).First().Key;
        }

        public void DoSpace(int playerNumber)
        {
            MonopolyPlayer thisPlayer = GetPlayer(playerNumber);
            BoardSpace currentSpace = spaces.Where(x => x.Players.Any(y => y.PlayerNumber == playerNumber)).First();
            if (spaces[18].Players.Any(x => x.PlayerNumber == playerNumber))
            {
                spaces[18].Players.Remove(thisPlayer);
                spaces[6].Players.Add(thisPlayer);
                thisPlayer.Money = thisPlayer.Money - 1;
            }

            if(currentSpace.OwnedBy == ConsoleColor.Black)
            {
                currentSpace.OwnedBy = thisPlayer.color;
                thisPlayer.Money = thisPlayer.Money - currentSpace.Value;
            }
            else if (!(currentSpace.OwnedBy == ConsoleColor.White) && !(currentSpace.OwnedBy == thisPlayer.color))
            {
                if(spaces.Where(x => x.SpaceColor == currentSpace.SpaceColor).All(y => y.OwnedBy == currentSpace.OwnedBy))
                {
                    thisPlayer.Money = thisPlayer.Money - currentSpace.Value * 2;
                    var awardedPlayer = GetPlayerLocations().First(x => x.Key.color == currentSpace.OwnedBy);
                    awardedPlayer.Key.Money = awardedPlayer.Key.Money + currentSpace.Value * 2;
                }
                else
                {
                    thisPlayer.Money = thisPlayer.Money - currentSpace.Value;
                    var awardedPlayer = GetPlayerLocations().First(x => x.Key.color == currentSpace.OwnedBy);
                    awardedPlayer.Key.Money = awardedPlayer.Key.Money + currentSpace.Value;
                }
                
            }
            else if (currentSpace.Name == "Chance")
            {
                Random r = new Random();
                int card = r.Next(1, 8);
                switch (card)
                {
                    //Go to jail
                    case 1:
                        spaces[6].Players.Add(thisPlayer);
                        currentSpace.Players.Remove(thisPlayer);
                        thisPlayer.Money = thisPlayer.Money - 1;
                        break;
                    //It's your birthday
                    case 2:
                        thisPlayer.Money = thisPlayer.Money + 2;
                        break;
                    //Too many sweets
                    case 3:
                        thisPlayer.Money = thisPlayer.Money - 2;
                        break;
                    //It is your birthday
                    case 5:
                        var players = GetPlayerLocations();
                        foreach (var p in players)
                        {
                            p.Key.Money = p.Key.Money - 1;
                        }
                        thisPlayer.Money = thisPlayer.Money + TotalPlayer;
                        break;
                    //Advance to go
                    case 4:
                        spaces[0].Players.Add(thisPlayer);
                        currentSpace.Players.Remove(thisPlayer);
                        thisPlayer.Money = thisPlayer.Money + 2;
                        break;
                    //Move Forward up to 5 spaces
                    //If we're space 20;
                    //20 > 24 - 5 (19)
                    //Would make 21, 22, 23, 0 & 1 eligible moving squares
                    case 6:
                        List<BoardSpace> eligibleSpaces = new List<BoardSpace>();
                        //Get all spaces forward 5
                        //First check to ensure that forward 5 won't wrap the board
                        int thisSpaceIndex = Array.IndexOf(spaces, currentSpace) + 1;
                        if(thisSpaceIndex > (24 - 5))
                        {
                            //If we would wrap, get all the spaces UP to 23 (boardwalk)
                            for (int i = thisSpaceIndex; i < 24; i++)
                            {
                                eligibleSpaces.Add(spaces[i]);
                            }
                            //get the spaces off the rest of the board
                            for (int i = 0; i < thisSpaceIndex - (24-5); i++)
                            {
                                eligibleSpaces.Add(spaces[i]);
                            }
                        }
                        //Otherwise, just get the next 5 spaces
                        else
                        {
                            for (int j = thisSpaceIndex; j < thisSpaceIndex + 5; j++)
                            {
                                eligibleSpaces.Add(spaces[j]);
                            }
                        }

                        //Decide where to move
                        //Sort the array by most valuable, we want to try and get ground where we can.
                        var sortByMostValuable = eligibleSpaces.OrderByDescending(x => x.Value);

                        foreach (var space in sortByMostValuable)
                        {
                            //Can we afford this space?
                            if (thisPlayer.Money > space.Value)
                            {
                                //Would it break the bank
                                //Also make sure it's not already owned
                                if (!(thisPlayer.Money - space.Value <= 3) && space.OwnedBy == ConsoleColor.Black)
                                {
                                    MovePlayer(thisPlayer.PlayerNumber, Array.IndexOf(spaces, space) - Array.IndexOf(spaces, currentSpace)); //Use move and do here so that we get our money if we pass go.
                                    DoSpace(thisPlayer.PlayerNumber);
                                    return;
                                }
                            }
                        }

                        //If we still haven't picked a space, move to the best forward position
                        foreach (var space in sortByMostValuable) //Sorting by 'most valuable' here actually means check all the spaces in 'closest to go' order.
                        {
                            if(space.OwnedBy == thisPlayer.color)
                            {
                                MovePlayer(thisPlayer.PlayerNumber, Array.IndexOf(spaces, space) - Array.IndexOf(spaces, currentSpace));
                                DoSpace(thisPlayer.PlayerNumber);
                                return;
                            }
                        }

                        //If we don't have any properties in this line, and we still have to move, find a safe space;
                        var safeSpaces = eligibleSpaces.Where(x => x.Value == 0);
                        foreach (var space in safeSpaces)
                        {
                            if(space.Name != "Go To Jail")
                            {
                                MovePlayer(thisPlayer.PlayerNumber, Array.IndexOf(spaces, space) - Array.IndexOf(spaces, currentSpace));
                                DoSpace(thisPlayer.PlayerNumber);
                                return;
                            }
                        }

                        break;
                    //Advance to a light blue space
                    case 7:
                        List<BoardSpace> lightBlueSpaces = new List<BoardSpace>();
                        lightBlueSpaces.Add(spaces[4]);
                        lightBlueSpaces.Add(spaces[5]);

                        //if unowned, get it for free
                        foreach (var space in lightBlueSpaces)
                        {
                            if (space.OwnedBy == ConsoleColor.Black)
                            {
                                spaces[Array.IndexOf(spaces, space)].Players.Add(thisPlayer);
                                currentSpace.Players.Remove(thisPlayer);
                                //it's free, so credit the player with the value of the space
                                thisPlayer.Money = thisPlayer.Money + space.Value;
                                DoSpace(thisPlayer.PlayerNumber);
                                return;
                            }
                        }

                        //if both are owned, check to see if we own one
                        foreach (var space in lightBlueSpaces)
                        {
                            if (space.OwnedBy == thisPlayer.color)
                            {
                                spaces[Array.IndexOf(spaces, space)].Players.Add(thisPlayer);
                                currentSpace.Players.Remove(thisPlayer);
                                DoSpace(thisPlayer.PlayerNumber);
                                return;
                            }
                        }

                        //if we don't own one, and they're both owned, just pick the one closest to go
                        spaces[5].Players.Add(thisPlayer);
                        currentSpace.Players.Remove(thisPlayer);
                        DoSpace(thisPlayer.PlayerNumber);
                        return;

                }
            }
            
        }

        public bool CheckGameOver()
        {
            var players = GetPlayerLocations();
            if(players.Any(x => x.Key.Money <= 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public MonopolyPlayer GetWinner()
        {
            return GetPlayerLocations().OrderByDescending(x => x.Key.Money).First().Key;
        }
    }
}
