using System;
using Chess.Pieces;

namespace Chess
{
    /// <summary>
    /// Struct used to capture data about each move made in a give game.
    /// </summary>
    public struct Move
    {
        /// <summary>
        /// The piece taking this move.
        /// </summary>
        public Piece Piece{ get; private set; }
        /// <summary>
        /// The starting row of the piece.
        /// </summary>
        public int StartRow { get; private set; }
        /// <summary>
        /// The starting column of the piece.
        /// </summary>
        public int StartColumn { get; private set; }
        /// <summary>
        /// The end row of the piece.
        /// </summary>
        public int Row { get; private set; }
        /// <summary>
        /// The end column of the piece.
        /// </summary>
        public int Column { get; private set; }
        /// <summary>
        /// The <see cref="MoveType"/> enum that contains information about events during the piece's move.
        /// </summary>
        public MoveType Type { get; private set; }
        /// <summary>
        /// Creates an instance of the Move struct.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="moveRow"></param>
        /// <param name="moveColumn"></param>
        /// <param name="moveType"></param>
        public Move(Piece piece, int startRow, int startColumn, int moveRow, int moveColumn, MoveType moveType)
        {
            StartColumn = startColumn;
            StartRow = startRow;
            Column = moveColumn;
            Row = moveRow;
            Piece = piece;
            Type = moveType;
        }
    }

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
