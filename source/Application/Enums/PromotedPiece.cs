namespace Chess.Application.Enums
{
    /// <summary>
    /// Used during pawn promotion to return the type of piece the user wants the pawn to be promoted into.
    /// </summary>
    public enum PromotedPiece
    {
        /// <summary>
        /// Indicates the user has selected the Queen.
        /// </summary>
        Queen,
        /// <summary>
        /// Indicates the user has selected the Rook.
        /// </summary>
        Rook,
        /// <summary>
        /// Indicates the user has selected the Bishop.
        /// </summary>
        Bishop,
        /// <summary>
        /// Indicates the user has selected the Knight.
        /// </summary>
        Knight,
        /// <summary>
        /// Indicates the user has not yet selected a piece.
        /// </summary>
        None
    }
}
