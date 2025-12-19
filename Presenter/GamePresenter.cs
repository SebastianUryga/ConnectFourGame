using System;
using System.Linq;
using GameConnectFour.Model;
using GameConnectFour.View;
using GameConnectFour.Model;
using System.Threading.Tasks;

namespace GameConnectFour.Presenter
{
    public class GamePresenter
    {
        private readonly IGameView _view;
        private readonly IGameModel _model; // Dependency on Interface

        public GamePresenter(IGameView view, IGameModel model)
        {
            _view = view;
            _model = model;

            // Subscribe to View events
            _view.ColumnClicked += OnColumnClicked;
            _view.RestartClicked += OnRestartClicked;

            // Initial Setup
            StartGame();
        }

        private void StartGame()
        {
            _model.InitializeBoard();
            _view.InitializeBoard(_model.Cells);
            UpdateViewStatus();
        }

        private void OnColumnClicked(object? sender, int column)
        {
            // Human clicked a column button
            MoveResult result = _model.MakeMove(column);

            if (result.Success)
            {
                // Incremental Update
                if (result.ChangedCellId.HasValue && result.PlacedSymbol != null)
                {
                    _view.UpdateCell(result.ChangedCellId.Value, result.PlacedSymbol); 
                }
                else
                {
                   _view.InitializeBoard(_model.Cells);
                }


                if (result.IsGameOver)
                {
                    HandleGameOver(result.WinnerSymbol);
                }
                else
                {
                    UpdateViewStatus();
                }
            }
        }

        private void HandleGameOver(string? winnerSymbol)
        {
            if (winnerSymbol != null)
            {
                string msg = $"Wygrał Gracz {winnerSymbol}!";
                _view.UpdateStatus(msg);
                _view.ShowMessage(msg);
            }
            else
            {
                string msg = "Remis!";
                _view.UpdateStatus(msg);
                _view.ShowMessage(msg);
            }

            // Save to Database
            SaveGameResult(winnerSymbol);
        }

        private void SaveGameResult(string? winner)
        {
             Task.Run(async () =>
             {
                 try
                 {
                     using (var context = new ConnectFourContext())
                     {
                         var history = new GameHistory
                         {
                             EndedAt = DateTime.Now,
                             Winner = winner ?? "Draw"
                         };
                         context.GameHistories.Add(history);
                         await context.SaveChangesAsync();
                     }
                 }
                 catch (Exception ex)
                 {
                     // Silently fail or log for now? User mainly wants to see it work.
                     // In real app, log this.
                     System.Diagnostics.Debug.WriteLine($"DB Error: {ex.Message}");
                 }
             });
        }

        private void UpdateViewStatus()
        {
            _view.UpdateStatus($"Ruch Gracza: {_model.CurrentPlayerSymbol}");
        }

        private void OnRestartClicked(object? sender, EventArgs e)
        {
            StartGame();
        }
    }
}
