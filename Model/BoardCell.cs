using System;

namespace GameConnectFour.Model
{
    public class BoardCell
    {
        public Guid Id { get; } = Guid.NewGuid();
        public int Row { get; }
        public int Column { get; }
        public string PlayerSymbol { get; set; } = string.Empty; // "", "X", "O"

        public BoardCell(int row, int col)
        {
            Row = row;
            Column = col;
        }
    }
}
