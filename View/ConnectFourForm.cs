using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GameConnectFour.Model;

namespace GameConnectFour.View
{
    public partial class ConnectFourForm : Form, IGameView
    {
        public event EventHandler<int>? ColumnClicked;
        public event EventHandler? RestartClicked;

        private Dictionary<Guid, Label> _cellControls = new Dictionary<Guid, Label>();
        private TableLayoutPanel _boardLayout;
        private TableLayoutPanel _buttonPanel; // Changed to TableLayoutPanel
        private Label _statusLabel;
        private Button _restartButton;

        public ConnectFourForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Połącz Cztery";
            this.Size = new Size(700, 850); // Increased size

            // Main Container to organize layout vertically
            var mainContainer = new TableLayoutPanel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.RowCount = 3;
            mainContainer.ColumnCount = 1;
            // Row 0: Column Buttons (Fixed Height)
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
            // Row 1: Game Board (Remaining Space)
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            // Row 2: Status & Restart (Fixed Height)
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 100f));
            this.Controls.Add(mainContainer);

            // 1. Top Panel (Buttons)
            _buttonPanel = new TableLayoutPanel();
            _buttonPanel.Dock = DockStyle.Fill;
            _buttonPanel.RowCount = 1;
            _buttonPanel.ColumnCount = 8;
            for (int i = 0; i < 8; i++)
            {
                 _buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            }
            mainContainer.Controls.Add(_buttonPanel, 0, 0);

            // 2. Board Layout (Center)
            _boardLayout = new TableLayoutPanel();
            _boardLayout.Dock = DockStyle.Fill;
            _boardLayout.RowCount = 8;
            _boardLayout.ColumnCount = 8;
            for (int i = 0; i < 8; i++) 
            {
                _boardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5f));
                _boardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            }
            mainContainer.Controls.Add(_boardLayout, 0, 1);

            // 3. Bottom Panel (Status + Restart)
            var bottomPanel = new TableLayoutPanel(); // Sub-layout for bottom
            bottomPanel.Dock = DockStyle.Fill;
            bottomPanel.RowCount = 2;
            bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            mainContainer.Controls.Add(bottomPanel, 0, 2);

            _statusLabel = new Label();
            _statusLabel.Dock = DockStyle.Fill;
            _statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            _statusLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            bottomPanel.Controls.Add(_statusLabel, 0, 0);

            _restartButton = new Button();
            _restartButton.Text = "Nowa Gra";
            _restartButton.Dock = DockStyle.Fill;
            _restartButton.Font = new Font("Segoe UI", 12);
            _restartButton.Click += (s, e) => RestartClicked?.Invoke(this, EventArgs.Empty);
            bottomPanel.Controls.Add(_restartButton, 0, 1);

            // Create 8 Column Buttons
            for (int i = 0; i < 8; i++)
            {
                var btn = new Button();
                btn.Text = "↓";
                btn.Dock = DockStyle.Fill; // Fill the cell
                btn.Tag = i;
                btn.Click += ColumnButton_Click;
                _buttonPanel.Controls.Add(btn, i, 0); // Add to specific column
            }
        }

        private void ColumnButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is int colIndex)
            {
                ColumnClicked?.Invoke(this, colIndex);
            }
        }

        public void InitializeBoard(List<BoardCell> cells)
        {
            _boardLayout.Controls.Clear();
            _cellControls.Clear();

            // We need to map cells to (Row, Col) in TableLayoutPanel
            // Note: BoardCell Row 0 is often Top, assuming Model logic agrees.
            foreach (var cell in cells)
            {
                var label = new Label();
                label.Text = cell.PlayerSymbol;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Font = new Font("Arial", 24, FontStyle.Bold);
                label.Dock = DockStyle.Fill;
                label.BorderStyle = BorderStyle.FixedSingle;
                label.BackColor = Color.White;
                
                // Color coding
                if (cell.PlayerSymbol == "X") label.ForeColor = Color.Red;
                if (cell.PlayerSymbol == "O") label.ForeColor = Color.Blue;

                _boardLayout.Controls.Add(label, cell.Column, cell.Row);
                _cellControls[cell.Id] = label;
            }
        }

        public void UpdateCell(Guid cellId, string symbol)
        {
            if (_cellControls.TryGetValue(cellId, out var label))
            {
                label.Text = symbol;
                if (symbol == "X") label.ForeColor = Color.Red;
                if (symbol == "O") label.ForeColor = Color.Blue;
            }
        }

        public void UpdateStatus(string message)
        {
            _statusLabel.Text = message;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ResetBoard()
        {
           // Handled by InitializeBoard usually
        }
    }
}
