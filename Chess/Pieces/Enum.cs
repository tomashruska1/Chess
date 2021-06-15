
namespace Chess.Pieces
{
    /// <summary>
    /// Denotes Piece color, is also used to ensure pawns move only in the proper direction (up or down based on color).
    /// </summary>
    public enum PieceColor
    {
        /// <summary>
        /// White piece color, white pawns move down (row index decreases).
        /// </summary>
        White = -1,
        /// <summary>
        /// Black piece color, black pawns move down (row index increases).
        /// </summary>
        Black = 1
    }

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
}
