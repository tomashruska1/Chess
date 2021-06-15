using Chess.Board;
using System.Collections.Generic;

namespace Chess.Pieces
{
    /// <summary>
    /// Class that represents the Pawn pieces on the chessboard. Provides movement typical for Pawns.
    /// </summary>
    public class Pawn : Piece
    {
        /// <summary>
        /// Stores whether the pawn is allowed to perform an En Passant capture against other pawns and in which direction the pawn may perform this move.
        /// </summary>
        internal int CanDoEnPassant { get; set; }

        /// <summary>
        /// Creates an instance of the Pawn class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="board"></param>
        public Pawn(PieceColor color, int rowIndex, int columnIndex, IBoard board)
        {
            Color = color;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Board = board;
            CanDoEnPassant = 0;

            if (color == PieceColor.White)
                UnicodeValue = '\u2659';
            else
                UnicodeValue = '\u265F';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public override List<(int, int)> PossibleMoves()
        {
            List<(int, int)> possibleMoves = new();

            int offset = (int)Color;

            if (IsInBounds(offset + RowIndex, ColumnIndex))
            {

                if (Board[offset + RowIndex, ColumnIndex] is null)
                {
                    CheckCanMoveToSquare(offset + RowIndex, ColumnIndex, possibleMoves, CaptureMode.NoCapture);
                    if (!HasMoved)
                        CheckCanMoveToSquare(2 * offset + RowIndex, ColumnIndex, possibleMoves, CaptureMode.NoCapture);
                }

                if (CanDoEnPassant == -1)
                    CheckCanMoveToSquare(offset + RowIndex, ColumnIndex - 1, possibleMoves);
                else
                    CheckCanMoveToSquare(offset + RowIndex, ColumnIndex - 1, possibleMoves, CaptureMode.CaptureOnly);

                if (CanDoEnPassant == 1)
                    CheckCanMoveToSquare(offset + RowIndex, ColumnIndex + 1, possibleMoves);
                else
                    CheckCanMoveToSquare(offset + RowIndex, ColumnIndex + 1, possibleMoves, CaptureMode.CaptureOnly);
            }

            return possibleMoves;
        }

        /// <summary>
        /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// </summary>
        /// <param name="otherRowIndex"></param>
        /// <param name="otherColumnIndex"></param>
        /// <param name="threateningKing"></param>
        /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
        public override bool IsAttackingSquare(int otherRowIndex, int otherColumnIndex, bool threateningKing = false)
        {
            int offset = (int)Color;

            if (otherRowIndex != RowIndex + offset)
                return false;

            if (otherColumnIndex == ColumnIndex + 1 || otherColumnIndex == ColumnIndex - 1)
            {
                if (threateningKing)
                    Board.LineOfAttack.Add(this, new() { (RowIndex, ColumnIndex) });
                return true;
            }

            return false;
        }
    }
}
