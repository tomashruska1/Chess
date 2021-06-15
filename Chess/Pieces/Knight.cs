using Chess.Board;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Pieces
{
    /// <summary>
    /// Class that represents the Knight piece on the chessboard. Provides movement typical for Knights.
    /// </summary>
    public class Knight : Piece
    {
        /// <summary>
        /// Creates an instance of the Knight class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="board"></param>
        public Knight(PieceColor color, int rowIndex, int columnIndex, IBoard board)
        {
            Color = color;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Board = board;

            if (color == PieceColor.White)
                UnicodeValue = '\u2658';
            else
                UnicodeValue = '\u265E';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public override List<(int, int)> PossibleMoves()
        {
            List<(int, int)> possibleMoves = new();

            List<(int, int)> coordinates = new();
            coordinates.Add((RowIndex + 2, ColumnIndex + 1));
            coordinates.Add((RowIndex + 2, ColumnIndex - 1));
            coordinates.Add((RowIndex - 2, ColumnIndex + 1));
            coordinates.Add((RowIndex - 2, ColumnIndex - 1));
            coordinates.Add((RowIndex + 1, ColumnIndex + 2));
            coordinates.Add((RowIndex + 1, ColumnIndex - 2));
            coordinates.Add((RowIndex - 1, ColumnIndex + 2));
            coordinates.Add((RowIndex - 1, ColumnIndex - 2));

            foreach (var tuple in coordinates)
            {
                if (IsInBounds(tuple))
                {
                    if (Board[tuple.Item1, tuple.Item2] is null || Board[tuple.Item1, tuple.Item2].Color != Color)
                    {
                        if (!Board.Kings[Color].IsUnderAttack || Board.LineOfAttack.Values.First().Contains(tuple))
                           possibleMoves.Add(tuple);
                    }
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
            if (Math.Abs(RowIndex - otherRowIndex) == 2 && Math.Abs(ColumnIndex - otherColumnIndex) == 1)
            {
                if (threateningKing)
                    Board.LineOfAttack.Add(this, new() { (RowIndex, ColumnIndex) });
                return true;
            }
            else if (Math.Abs(RowIndex - otherRowIndex) == 1 && Math.Abs(ColumnIndex - otherColumnIndex) == 2)
            {
                if (threateningKing)
                    Board.LineOfAttack.Add(this, new() { (RowIndex, ColumnIndex) });
                return true;
            }
            return false;
        }
    }
}
