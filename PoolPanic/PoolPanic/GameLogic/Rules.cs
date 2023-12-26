using System;
using System.Collections.Generic;

namespace PoolPanic
{
    public class Rules
    {
        public enum Result
        {
            Foul,
            GameBallSunken,
            Gameover,
            Ok            
        }

        public enum Player
        {
            Player1,
            Player2            
        }

        public enum TableState
        { 
            Open,
            Player1Full,
            Player2Full
        }
        
        private Player _player;        
        public event Action<Player, TableState> PlayerUpdated;
        public event Action<Player> WinnerUpdated;
        public event Action<string> InfoUpdated;
        

        string _lastFoul = "";

        TableState _tableState = TableState.Open;

        /// <summary>
        /// change the active player
        /// </summary>
        public Player ActivePlayer
        {
            get { return _player; }
            set
            {
                _player = value;
                if (PlayerUpdated != null)
                    PlayerUpdated(_player, _tableState);
            }
        }
     
        public void SwitchPlayer()
        {
            if (_player == Player.Player1)
                ActivePlayer = Player.Player2;
            else
                ActivePlayer = Player.Player1;
        }

        public void Reset()
        {
            _tableState = TableState.Open;
            ActivePlayer = Player.Player1;            
        }

        public String LastFoul
        {
            get { return _lastFoul; }
        }

        public Result Check8Ball(List<Ball> sunkenBalls, List<Ball> tableBalls, Ball firstHit)
        {
            #region Sunken 8 Rules
            foreach (Ball b in sunkenBalls)
            {
                if (b.Number == 8)
                {
                    // game ball sunken while table is open -> immediate loss
                    if (_tableState == TableState.Open)
                    {
                        if (WinnerUpdated != null)
                            WinnerUpdated(_player == Player.Player1 ? Player.Player2 : Player.Player1);

                        return Result.Gameover;
                    }

                    // check if 8 sunken while there are wrong balls on the table
                    if (_player == Player.Player1)
                    {
                        if (_tableState == TableState.Player1Full && HasFull(tableBalls))
                        {
                            if (WinnerUpdated != null)
                                WinnerUpdated(Player.Player2);

                            return Result.Gameover;
                        }
                        if (_tableState == TableState.Player1Full && !HasFull(tableBalls))
                        {
                            if (WinnerUpdated != null)
                                WinnerUpdated(Player.Player1);

                            return Result.Gameover;
                        }
                        if (_tableState == TableState.Player2Full && HasHalf(tableBalls))
                        {
                            if (WinnerUpdated != null)
                                WinnerUpdated(Player.Player2);

                            return Result.Gameover;
                        }
                        if (_tableState == TableState.Player2Full && !HasHalf(tableBalls))
                        {
                            if (WinnerUpdated != null)
                                WinnerUpdated(Player.Player1);

                            return Result.Gameover;
                        }
                    }
                    else
                    {
                        if (_tableState == TableState.Player1Full && HasHalf(tableBalls))
                        {
                            if (WinnerUpdated != null)
                                WinnerUpdated(Player.Player1);

                            return Result.Gameover;
                        }
                        if (_tableState == TableState.Player1Full && !HasHalf(tableBalls))
                        {
                            if (WinnerUpdated != null)
                                WinnerUpdated(Player.Player2);

                            return Result.Gameover;
                        }
                        if (_tableState == TableState.Player2Full && HasFull(tableBalls))
                        {
                            if (WinnerUpdated != null)
                                WinnerUpdated(Player.Player1);

                            return Result.Gameover;
                        }
                        if (_tableState == TableState.Player2Full && !HasFull(tableBalls))
                        {
                            if (WinnerUpdated != null)
                                WinnerUpdated(Player.Player2);

                            return Result.Gameover;
                        }
                    }
                }
            }

            #endregion

            #region Game Ball Rules
            // check game ball sunken
            foreach (Ball b in sunkenBalls)
            {
                if (b.Number == 0)
                {
                    _lastFoul = "Game ball sunken";

                    if (InfoUpdated != null)
                        InfoUpdated("Game ball sunken");
                    
                    return Result.GameBallSunken;
                }
            }
            #endregion

            #region First Hit Rules
            if (firstHit == null)
            {
                // check no hit
                _lastFoul = "Nothing hit";


                if (InfoUpdated != null)
                    InfoUpdated("Nothing hit");

                return Result.Foul;
            }
            else
            {   
                //check first ball hit as player 1
                if (_player == Player.Player1)
                {
                    if (_tableState == TableState.Player1Full && firstHit.IsHalf)
                    {
                        _lastFoul = "Wrong color hit";

                        if (InfoUpdated != null)
                            InfoUpdated("Wrong color hit");

                        return Result.Foul;
                    }
                    if (_tableState == TableState.Player2Full && firstHit.IsFull)
                    {
                        _lastFoul = "Wrong color hit";

                        if (InfoUpdated != null)
                            InfoUpdated("Wrong color hit");

                        return Result.Foul;
                    }                                   
                }

                // check first ball hit as player 2
                if (_player == Player.Player2)
                {
                    if (_tableState == TableState.Player1Full && firstHit.IsFull)
                    {
                        _lastFoul = "Wrong color hit";


                        if (InfoUpdated != null)
                            InfoUpdated("Wrong color hit");

                        return Result.Foul;
                    }
                    if (_tableState == TableState.Player2Full && firstHit.IsHalf)
                    {
                        _lastFoul = "Wrong color hit";

                        if (InfoUpdated != null)
                            InfoUpdated("Wrong color hit");
                        return Result.Foul;
                    }
                }
            }
            #endregion
           
            #region First 8 Hit Rules
            if (firstHit != null)
            {
                // check if the 8 was hit first while there are balls of your color on the table
                if (firstHit.Number == 8)
                {
                    if ((_tableState == TableState.Player1Full && HasFull(tableBalls)) || _tableState == TableState.Player2Full && HasHalf(tableBalls))
                    {
                        _lastFoul = "8 hit";

                        if (InfoUpdated != null)
                            InfoUpdated("8 hit");

                        return Result.Foul;
                    }

                    if ((_tableState == TableState.Player2Full && HasFull(tableBalls)) || _tableState == TableState.Player1Full && HasHalf(tableBalls))
                    {
                        _lastFoul = "8 hit";

                        if (InfoUpdated != null)
                            InfoUpdated("8 hit");

                        return Result.Foul;
                    }
                }
            }
            #endregion

            #region Sunken Balls Rules
            // check no ball sunken
            if (sunkenBalls.Count == 0)
            {
                _lastFoul = "No ball sunken";

                if (InfoUpdated != null)
                    InfoUpdated("no ball sunken");

                return Result.Foul;
            }

            // check open table sunken balls -> assign color
            if (_tableState == TableState.Open)
            {
                foreach (Ball b in sunkenBalls)
                {
                    if (_player == Player.Player1)
                    {
                        if (b.IsFull)
                        {
                            _tableState = TableState.Player1Full;

                            if (InfoUpdated != null)
                                InfoUpdated("Player 1 plays full");                          
                        }
                        else
                        {
                            _tableState = TableState.Player2Full;

                            if (InfoUpdated != null)
                                InfoUpdated("Player 2 plays full");
                        }
                    }
                    else // player 2
                    {
                        if (b.IsFull)
                        {
                            _tableState = TableState.Player2Full;

                            if (InfoUpdated != null)
                                InfoUpdated("Player 2 plays full");
                        }
                        else
                        {
                            _tableState = TableState.Player1Full;

                            if (InfoUpdated != null)
                                InfoUpdated("Player 1 plays full");
                        }
                    }
                }

                // force setting of player status
                ActivePlayer = ActivePlayer;
            }

            // check locked table sunken balls
            if (_player == Player.Player1) // check player 1
            {
                bool foul = true;

                foreach (Ball b in sunkenBalls)
                {
                    if (_tableState == TableState.Player1Full)
                    {
                        if (b.IsFull)
                            foul = false;
                    }
                    else
                    {
                        if(b.IsHalf)
                            foul = false;
                    }                    
                }

                if (foul)
                {
                    _lastFoul = "Wrong color sunken";

                    if (InfoUpdated != null)
                        InfoUpdated("wrong color sunken");

                    return Result.Foul;
                }
            }
            else // check player 2
            {
                 bool foul = true;

                 foreach (Ball b in sunkenBalls)
                {
                    if (_tableState == TableState.Player2Full)
                    {
                        if (b.IsFull)
                            foul = false;
                    }
                    else
                    {
                        if(b.IsHalf)
                            foul = false;
                    }                    
                }

                 if (foul)
                 {
                     _lastFoul = "Wrong color sunken";

                     if (InfoUpdated != null)
                         InfoUpdated("wrong color sunken");

                     return Result.Foul;
                 }
            }
            #endregion

            return Result.Ok;
        }

        private bool HasFull(List<Ball> balls)
        {
            foreach(Ball b in balls)
            {
                if (b.IsFull)
                    return true;
            }
            return false;
        }

        private bool HasHalf(List<Ball> balls)
        {
            foreach(Ball b in balls)
            {
                if (b.IsHalf)
                    return true;
            }
            return false;
        }
    }
}
