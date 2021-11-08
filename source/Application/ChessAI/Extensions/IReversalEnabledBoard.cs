namespace Chess.Application.ChessAIs.Extensions;


/// <summary>
/// Extends the <see cref="IBoard"/> with the ability to reverse a given move.
/// </summary>
internal interface IReversalEnabledBoard : IBoard
{
    /// <summary>
    /// Reverses the most recent move.
    /// </summary>
    public void ReverseLastMove();
    /// <summary>
    /// Mirrors a given <see cref="IBoard"/> piece positions.
    /// </summary>
    /// <param name="board"></param>
    public void SetUpNonstandardBoard(IBoard board);
}
