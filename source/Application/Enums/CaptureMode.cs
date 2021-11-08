namespace Chess.Application.Enums;


/// <summary>
/// This is used to ensure pawns are blocked when they should be, and require a piece of opposite color to capture diagonally.
/// </summary>
public enum CaptureMode
{
    /// <summary>
    /// Piece can move to a square if it is unoccupied or occupied by a piece of the opposing color.
    /// </summary>
    All,
    /// <summary>
    /// Piece can only move to an unoccupied square - used by pawns.
    /// </summary>
    NoCapture,
    /// <summary>
    /// Piece can only move to a square occupied by a piece of the opposing color - used by pawns.
    /// </summary>
    CaptureOnly
}
