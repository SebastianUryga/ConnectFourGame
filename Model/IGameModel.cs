using System;
using System.Collections.Generic;

namespace GameConnectFour.Model
{
    public interface IGameModel
    {
        // Properties needed for initial setup or query
        List<BoardCell> Cells { get; }
        string CurrentPlayerSymbol { get; }

        // Main action - returns result instead of void/bool
        void InitializeBoard();
        MoveResult MakeMove(int column);
    }
}
