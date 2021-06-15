using Chess.Board;
using System.Collections.Generic;

namespace Chess.Pieces
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
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="board"></param>
        public Queen(PieceColor color, int rowIndex, int columnIndex, IBoard board)
        {
            Color = color;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Board = board;

            if (color == PieceColor.White)
                UnicodeValue = '\u2655';
            else
                UnicodeValue = '\u265B';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public override List<(int, int)> PossibleMoves()
        {
            List<(int, int)> moves = Rook.RookPossibleMoves(this);
            moves.AddRange(Bishop.BishopPossibleMoves(this));
            return moves;
        }

        /// <summary>
        /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// </summary>
        /// <param name="otherRowIndex"></param>
        /// <param name="otherColumnIndex"></param>
        /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
        public bool IsAttackingSquare(int otherRowIndex, int otherColumnIndex)
        {
            return IsAttackingSquare(otherRowIndex, otherColumnIndex, false);
        }

        /// <summary>
        /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// </summary>
        /// <param name="otherRowIndex"></param>
        /// <param name="otherColumnIndex"></param>
        /// <param name="threateningKing"></param>
        /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
        public override bool IsAttackingSquare(int otherRowIndex, int otherColumnIndex, bool threateningKing)
        {
            return Rook.IsAttackingSquare(this, otherRowIndex, otherColumnIndex, threateningKing) ||
                Bishop.IsAttackingSquare(this, otherRowIndex, otherColumnIndex, threateningKing);
        }
    }
}
