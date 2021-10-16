namespace Chess.Application.Enums
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
}
