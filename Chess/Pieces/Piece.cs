using Chess.Board;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Pieces
{
    /// <summary>
    /// Abstract base class for other pieces, provides shared members and methods.
    /// </summary>
    public abstract class Piece
    {
        /// <summary>
        /// Determines the color of a given piece.
        /// </summary>
        /// <remarks>
        /// See <see cref="PieceColor"/>
        /// </remarks>
        public PieceColor Color { get; protected set; }
        /// <summary>
        /// Provides a unicode character that should correspond visually with the piece and color it represents.
        /// </summary>
        public char UnicodeValue { get; protected set; }
        /// <summary>
        /// Provides an index of the row on which the piece is located.
        /// </summary>
        public int RowIndex { get; set; }
        /// <summary>
        /// Provides an index of the column on which the piece is located.
        /// </summary>
        public int ColumnIndex { get; set; }
        /// <summary>
        /// Provides a reference to the <see cref="IBoard"/> object that is used to run the game.
        /// </summary>
        public IBoard Board { get; protected set; }
        /// <summary>
        /// Provides a check whether the piece has moved - important for several pieces.
        /// </summary>
        public bool HasMoved { get; protected set; } = false;
        /// <summary>
        /// Abstract method, override should calculate all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as tuple(int, int).</returns>
        public abstract List<(int, int)> PossibleMoves();
        /// <summary>
        /// Abstract method, override should check whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// </summary>
        /// <param name="otherRowIndex"></param>
        /// <param name="otherColumnIndex"></param>
        /// <param name="threateningKing"></param>
        /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
        public abstract bool IsAttackingSquare(int otherRowIndex, int otherColumnIndex, bool threateningKing);

        /// <summary>
        /// Checks whether the piece is allowed to move to the given square - if is in bounds,
        /// unoccupied or occupied by the correct piece in the given context and adds the square coordinates
        /// to the list of possible moves for the piece.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="possibleMoves"></param>
        /// <param name="captureMode"></param>
        /// <returns>True if the square is a legal move, otherwise false.</returns>
        internal bool CheckCanMoveToSquare(int row, int column, List<(int, int)> possibleMoves, CaptureMode captureMode = CaptureMode.All)
        {
            if (!IsInBounds(row, column))
                return false;

            if (Board[row, column] is null)
            {
                if (captureMode == CaptureMode.CaptureOnly)
                    return false;

                if (IsProtectingKingInThisPosition(row, column))
                    possibleMoves.Add((row, column));
                return true;
            }
            else if (Board[row, column].Color != Color)
            {
                if (captureMode == CaptureMode.NoCapture)
                    return false;

                if (IsProtectingKingInThisPosition(row, column))
                    possibleMoves.Add((row, column));
            }
            return false;
        }

        /// <summary>
        /// Checks if the king would be protected should the piece be moved to this position.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>True if king would be protected, otherwise false.</returns>
        private bool IsProtectingKingInThisPosition(int row, int column)
        {
            if (!Board.Kings[Color].IsUnderAttack)
            {
                if (Board.IsProtectingKing(this, row, column))
                {
                    return true;
                }
            }
            else if(Board.LineOfAttack.Count == 1 && Board.LineOfAttack.Values.First().Contains((row, column)))
                return true;

            return false;
        }

        /// <summary>
        /// Handles the internals of moving the piece from the viewpoint of the piece - changing it's row and column
        /// and setting it's <see cref="HasMoved"/> property to true.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        internal virtual void Move(int row, int column)
        {
            RowIndex = row;
            ColumnIndex = column;
            HasMoved = true;
        }

        /// <summary>
        /// Converts the piece to it's unicode representation as string.
        /// </summary>
        /// <returns>Unicode representation of the piece as string.</returns>
        public override string ToString()
        {
            return UnicodeValue.ToString();
        }

        /// <summary>
        /// Checks whether the coordinates as tuple(int, int) are a part of the board.
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns>True if the coordinates are a part of the board.</returns>
        internal static bool IsInBounds((int, int) tuple)
        {
            return IsInBounds(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// Checks whether the coordinates as int, int are a part of the board.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>True if the coordinates are a part of the board.</returns>
        internal static bool IsInBounds(int row, int column)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7)
                return false;
            return true;
        }
    }
}
