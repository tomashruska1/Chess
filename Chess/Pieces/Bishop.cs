using Chess.Board;
using System;
using System.Collections.Generic;

namespace Chess.Pieces
{
    /// <summary>
    /// Class that represents the Bishop piece on the chessboard. Provides movement typical for Bishops.
    /// </summary>
    public class Bishop : Piece
    {
        /// <summary>
        /// Creates an instance of the Bishop class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="board"></param>
        public Bishop(PieceColor color, int rowIndex, int columnIndex, IBoard board)
        {
            Color = color;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Board = board;

            if (color == PieceColor.White)
                UnicodeValue = '\u2657';
            else
                UnicodeValue = '\u265D';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public override List<(int, int)> PossibleMoves()
        {
            return BishopPossibleMoves(this);
        }

        /// <summary>
        /// Static method that calculates all moves a bishop can make if put into the position of the piece given as an argument.
        /// Calculates the moves for Bishop and Queen.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        internal static List<(int, int)> BishopPossibleMoves(Piece piece)
        {
            int RowIndex = piece.RowIndex;
            int ColumnIndex = piece.ColumnIndex;
            List<(int, int)> possibleMoves = new();

            bool upleft = true;
            bool upright = true;
            bool downleft = true;
            bool downright = true;

            for (int n = 1; n < 8; n++)
            {
                if (upleft)
                {
                    upleft = piece.CheckCanMoveToSquare(RowIndex - n, ColumnIndex - n, possibleMoves);
                }
                if (upright)
                {
                    upright = piece.CheckCanMoveToSquare(RowIndex - n, ColumnIndex + n, possibleMoves);
                }
                if (downleft)
                {
                    downleft = piece.CheckCanMoveToSquare(RowIndex + n, ColumnIndex - n, possibleMoves);
                }
                if (downright)
                {
                    downright = piece.CheckCanMoveToSquare(RowIndex + n, ColumnIndex + n, possibleMoves);
                }
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
            return IsAttackingSquare(this, otherRowIndex, otherColumnIndex, threateningKing);
        }

        /// <summary>
        /// Static method that checks whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// This is used for calculating Bishop and Queen movements.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="otherRowIndex"></param>
        /// <param name="otherColumnIndex"></param>
        /// <param name="threateningKing"></param>
        /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
        internal static bool IsAttackingSquare(Piece piece, int otherRowIndex, int otherColumnIndex, bool threateningKing)
        {
            int RowIndex = piece.RowIndex;
            int ColumnIndex = piece.ColumnIndex;
            IBoard Board = piece.Board;
            List<(int, int)> lineOfAttack = new();

            if ((Math.Abs(RowIndex - otherRowIndex) != Math.Abs(ColumnIndex - otherColumnIndex)) ||
                (RowIndex == otherRowIndex && ColumnIndex == otherColumnIndex))
                return false;

            int rowOffset, columnOffset;

            rowOffset = otherRowIndex > RowIndex ? 1 : -1;
            columnOffset = otherColumnIndex > ColumnIndex ? 1 : -1;

            RowIndex += rowOffset;
            ColumnIndex += columnOffset;

            while (IsInBounds(RowIndex, ColumnIndex) && RowIndex != otherRowIndex && ColumnIndex != otherColumnIndex)
            {
                if (Board[RowIndex, ColumnIndex] is not null &&
                    !Board[RowIndex, ColumnIndex].GetType().Equals(typeof(King)))
                    return false;

                if (threateningKing)
                    lineOfAttack.Add((RowIndex, ColumnIndex));

                RowIndex += rowOffset;
                ColumnIndex += columnOffset;
            }

            if (threateningKing)
            {
                lineOfAttack.Add((piece.RowIndex, piece.ColumnIndex));
                piece.Board.LineOfAttack.Add(piece, lineOfAttack);
            }

            return true;
        }
    }
}
