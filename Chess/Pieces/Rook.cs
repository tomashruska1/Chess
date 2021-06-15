using Chess.Board;
using System.Collections.Generic;

namespace Chess.Pieces
{
    /// <summary>
    /// Class that represents the Rook piece on the chessboard. Provides movement typical for Rooks.
    /// </summary>
    public class Rook : Piece
    {
        /// <summary>
        /// Creates an instance of the Rook class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="board"></param>
        public Rook(PieceColor color, int rowIndex, int columnIndex, IBoard board)
        {
            Color = color;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Board = board;
            HasMoved = false;

            if (color == PieceColor.White)
                UnicodeValue = '\u2656';
            else
                UnicodeValue = '\u265C';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public override List<(int, int)> PossibleMoves()
        {
            return RookPossibleMoves(this);
        }

        /// <summary>
        /// Static method that calculates all moves a rook can make if put into the position of the piece given as an argument.
        /// Calculates the moves for Rook and Queen.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        internal static List<(int, int)> RookPossibleMoves(Piece piece)
        {
            int RowIndex = piece.RowIndex;
            int ColumnIndex = piece.ColumnIndex;
            List<(int, int)> possibleMoves = new();

            bool up = true;
            bool down = true;
            bool left = true;
            bool right = true;

            for (int n = 1; n < 8; n++)
            {

                if (up)
                {
                    up = piece.CheckCanMoveToSquare(RowIndex - n, ColumnIndex, possibleMoves);
                }
                if (down)
                {
                    down = piece.CheckCanMoveToSquare(RowIndex + n, ColumnIndex, possibleMoves);
                }
                if (left)
                {
                    left = piece.CheckCanMoveToSquare(RowIndex, ColumnIndex - n, possibleMoves);
                }
                if (right)
                {
                    right = piece.CheckCanMoveToSquare(RowIndex, ColumnIndex + n, possibleMoves);
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
        /// This is used for calculating Rook and Queen movements.
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

            if (RowIndex != otherRowIndex && ColumnIndex != otherColumnIndex)
            {
                return false;
            }
            else if (RowIndex == otherRowIndex && ColumnIndex == otherColumnIndex)
            {
                return false;
            }
            else if (RowIndex == otherRowIndex)
            {
                if (ColumnIndex > otherColumnIndex)
                {
                    for (int n = otherColumnIndex + 1; n < ColumnIndex; n++)
                    {
                        if (Board[RowIndex, n] is not null &&
                            !Board[RowIndex, n].GetType().Equals(typeof(King)))
                            return false;

                        if (threateningKing)
                            lineOfAttack.Add((RowIndex, n));
                    }
                }
                else
                {
                    for (int n = otherColumnIndex - 1; n > ColumnIndex; n--)
                    {
                        if (Board[RowIndex, n] is not null &&
                            !Board[RowIndex, n].GetType().Equals(typeof(King)))
                            return false;

                        if (threateningKing)
                            lineOfAttack.Add((RowIndex, n));
                    }
                }
            }
            else if (ColumnIndex == otherColumnIndex)
            {
                if (RowIndex > otherRowIndex)
                {
                    for (int n = otherRowIndex + 1; n < RowIndex; n++)
                    {
                        if (Board[n, ColumnIndex] is not null &&
                            !Board[n, ColumnIndex].GetType().Equals(typeof(King)))
                            return false;

                        if (threateningKing)
                            lineOfAttack.Add((n, ColumnIndex));
                    }
                }
                else
                {
                    for (int n = otherRowIndex - 1; n > RowIndex; n--)
                    {
                        if (Board[n, ColumnIndex] is not null &&
                            !Board[n, ColumnIndex].GetType().Equals(typeof(King)))
                            return false;

                        if (threateningKing)
                            lineOfAttack.Add((n, ColumnIndex));
                    }
                }
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
