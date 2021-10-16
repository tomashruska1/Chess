using Chess.Application.Boards;
using Chess.Application.Enums;
using System.Collections.Generic;

namespace Chess.Application.Pieces
{
    /// <summary>
    /// Class that represents the Queen piece on the chessboard.
    /// </summary>
    public class Queen : Piece
    {
        /// <summary>
        /// Creates an instance of the Queen class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="square"></param>
        /// <param name="board"></param>
        public Queen(PieceColor color, Square square, IBoard board)
        {
            Color = color;
            Square = square;
            Board = board;
            PossibleMoves = new();

            if (color == PieceColor.White)
                UnicodeValue = '\u2655';
            else
                UnicodeValue = '\u265B';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
        public override List<Square> GetPossibleMoves()
        {
            if (PossibleMoves.Count > 0)
                return PossibleMoves;

            PossibleMoves.AddRange(Rook.RookPossibleMoves(this));
            PossibleMoves.AddRange(Bishop.BishopPossibleMoves(this));

            RemoveInvalidMoves();
            return PossibleMoves;
        }

        /// <summary>
        /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// </summary>
        /// <param name="otherSquare"></param>
        /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
        public override bool IsThreateningSquare(Square otherSquare)
        {
            return Rook.IsThreateningSquare(this, otherSquare)
                || Bishop.IsThreateningSquare(this, otherSquare);
        }
    }
}
