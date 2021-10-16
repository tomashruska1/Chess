using System;

namespace Chess.Application.Enums
{
    /// <summary>
    /// This is used to capture additional characteristics of a given move -
    /// whether a check was made, piece captured etc.
    /// </summary>
    [Flags]
    public enum MoveType
    {
        /// <summary>
        /// Nothing noteworthy has happened.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// A piece was captured during the move.
        /// </summary>
        Capture = 1,
        /// <summary>
        /// A pawn was captured by the En Passant move.
        /// </summary>
        EnPassantCapture = 2,
        /// <summary>
        /// King has castled to the Queen's side.
        /// </summary>
        QueensideCastling = 4,
        /// <summary>
        /// King has castled to the King's side.
        /// </summary>
        KingsideCastling = 8,
        /// <summary>
        /// The move resulted in a check.
        /// </summary>
        Check = 16,
        /// <summary>
        /// The move resulted in a checkmate.
        /// </summary>
        CheckMate = 32,
        /// <summary>
        /// The move resulted in a draw.
        /// </summary>
        Draw = 64,
        /// <summary>
        /// A pawn was promoted.
        /// </summary>
        PawnPromotion = 128
    }
}
