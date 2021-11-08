namespace Chess.Application.ChessAIs.Extensions;


/// <summary>
/// Extension methods for <see cref="Piece"/>.
/// </summary>
internal static class PieceExtensions
{
    /// <summary>
    /// Checks whether the <paramref name="piece"/> would be attacking <paramref name="whichSquare"/>
    /// if it were located on <paramref name="fromSquare"/>.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="fromSquare"></param>
    /// <param name="whichSquare"></param>
    /// <returns>True if the piece would be attacking a given square, otherwise false.</returns>
    internal static bool IsThreateningSquare(this Piece piece, Square fromSquare, Square whichSquare)
    {
        Square backup = piece.Square;
        piece.Square = fromSquare;
        bool result = piece.IsThreateningSquare(whichSquare);
        piece.Square = backup;
        return result;
    }
}
