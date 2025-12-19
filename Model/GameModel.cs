using System;
using System.Collections.Generic;
using System.Linq;

namespace GameConnectFour.Model
{
    public class GameModel : IGameModel
    {
        public const int Rows = 8;
        public const int Cols = 8;
        public List<BoardCell> Cells { get; private set; }
        public string CurrentPlayerSymbol { get; private set; } = "X";
        
        // Private state, exposed only via MoveResult returns
        private bool _isGameOver = false;
        private string? _winnerSymbol = null;

        public GameModel()
        {
            InitializeBoard();
        }

        public void InitializeBoard()
        {
            Cells = new List<BoardCell>();
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Cells.Add(new BoardCell(r, c));
                }
            }
            CurrentPlayerSymbol = "X";
            _isGameOver = false;
            _winnerSymbol = null;
        }

        public MoveResult MakeMove(int column)
        {
            if (_isGameOver)
                return new MoveResult(false, _isGameOver, _winnerSymbol);

            // Gravity Logic: Find lowest empty row in column
            var cell = Cells
                .Where(c => c.Column == column && string.IsNullOrEmpty(c.PlayerSymbol))
                .OrderByDescending(c => c.Row) // Bottom-up
                .FirstOrDefault();

            if (cell == null)
            {
                // Column full
                return new MoveResult(false, _isGameOver, _winnerSymbol);
            }

            // Update State
            cell.PlayerSymbol = CurrentPlayerSymbol;

            CheckWinCondition();

            TogglePlayer();

            return new MoveResult(true, _isGameOver, _winnerSymbol, ChangedCellId: cell.Id, PlacedSymbol: cell.PlayerSymbol);
        }

        private void TogglePlayer()
        {
            CurrentPlayerSymbol = (CurrentPlayerSymbol == "X") ? "O" : "X";
        }

        private void CheckWinCondition()
        {
             string GetSymbol(int r, int c)
            {
                return Cells.FirstOrDefault(cell => cell.Row == r && cell.Column == c)?.PlayerSymbol ?? "";
            }

            bool CheckLine(int r, int c, int dr, int dc)
            {
                string first = GetSymbol(r, c);
                if (string.IsNullOrEmpty(first)) return false;

                for (int i = 1; i < 4; i++)
                {
                    if (GetSymbol(r + i * dr, c + i * dc) != first) return false;
                }
                
                _isGameOver = true;
                _winnerSymbol = first;
                return true;
            }

            // Check all cells as starting points
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    // Horizontal
                    if (c + 3 < Cols && CheckLine(r, c, 0, 1)) return;
                    // Vertical
                    if (r + 3 < Rows && CheckLine(r, c, 1, 0)) return;
                    // Diagonal Down-Right
                    if (r + 3 < Rows && c + 3 < Cols && CheckLine(r, c, 1, 1)) return;
                    // Diagonal Down-Left
                    if (r + 3 < Rows && c - 3 >= 0 && CheckLine(r, c, 1, -1)) return;
                }
            }

            // Check Draw (Board Full)
            if (Cells.All(c => !string.IsNullOrEmpty(c.PlayerSymbol)))
            {
                _isGameOver = true;
                _winnerSymbol = null; // Draw
            }
        }
    }
}
