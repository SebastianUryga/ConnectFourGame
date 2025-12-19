namespace GameConnectFour.Model
{
    public record MoveResult(bool Success, bool IsGameOver, string? WinnerSymbol, Guid? ChangedCellId = null, string? PlacedSymbol = null);
}
