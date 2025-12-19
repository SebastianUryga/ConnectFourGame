using System;
using System.Collections.Generic;
using System.Collections.Generic;
using GameConnectFour.Model;

namespace GameConnectFour.View
{
    public interface IGameView
    {
        // Event raised when user clicks a cell
        // Event raised when user clicks a column
        event EventHandler<int> ColumnClicked;
        event EventHandler RestartClicked;

        // Methods for Presenter to control View
        void InitializeBoard(List<BoardCell> cells);
        void UpdateCell(Guid cellId, string symbol); 
        void UpdateStatus(string message);
        void ShowMessage(string message);
        void ResetBoard();
    }
}
