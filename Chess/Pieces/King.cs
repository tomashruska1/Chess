using Chess.Board;
using System;
using System.Collections.Generic;

namespace Chess.Pieces
{
    /// <summary>
    /// Class that represents the Bishop piece on the chessboard. Provides movement typical for Kings.
    /// </summary>
    public class King : Piece
    {
        /// <summary>
        /// Provides a quick check whether the king is currently being attacked by pieces of opposing color.
        /// </summary>
        public bool IsUnderAttack { get; private set; }
        /// <summary>
        /// Provides a quick check whether the current move is castling.
        /// </summary>
        /// <remarks>This property needs to be set to false again in the method handling the piece movement on the board!</remarks>
        internal bool IsCastling { get; set; }

        /// <summary>
        /// Creates an instance of the King class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="board"></param>
        public King(PieceColor color, int rowIndex, int columnIndex, IBoard board)
        {
            Color = color;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Board = board;
            IsUnderAttack = false;
            HasMoved = false;
            IsCastling = false;

            if (color == PieceColor.White)
                UnicodeValue = '\u2654';
            else
                UnicodeValue = '\u265A';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public override List<(int, int)> PossibleMoves()
        {
            List<(int, int)> possibleMoves = new();

            for (int row = -1; row < 2; row++)
            {
                for (int column = -1; column < 2; column++)
                {
                    if ((row == 0 && column == 0) || !IsInBounds(RowIndex + row, ColumnIndex + column)
                       || Board[RowIndex + row, ColumnIndex + column]?.Color == Color)
                    {
                        continue;
                    }

                    if (!IsSquareUnderAttack(RowIndex + row, ColumnIndex + column))
                        possibleMoves.Add((RowIndex + row, ColumnIndex + column));
                }
            }

            if (!HasMoved && !IsUnderAttack)
                CheckForCastlingOpportunities(possibleMoves);

            return possibleMoves;
        }

        /// <summary>
        /// Checks whether the king can castle to either side
        /// </summary>
        /// <param name="possibleMoves"></param>
        private void CheckForCastlingOpportunities(List<(int, int)> possibleMoves)
        {
            if (CastlingPossibleForSquare(RowIndex, ColumnIndex + 1) && CastlingPossibleForSquare(RowIndex, ColumnIndex + 2))
            {
                if (Board[RowIndex, ColumnIndex + 3].GetType().Equals(typeof(Rook)))
                {
                    if (!Board[RowIndex, ColumnIndex + 3].HasMoved)
                    {
                        possibleMoves.Add((RowIndex, ColumnIndex + 2));
                    }
                }
            }

            if (CastlingPossibleForSquare(RowIndex, ColumnIndex - 1) && CastlingPossibleForSquare(RowIndex, ColumnIndex - 2)
                 && Board[RowIndex, ColumnIndex - 3] is null)
            {
                if (Board[RowIndex, ColumnIndex - 4].GetType().Equals(typeof(Rook)))
                {
                    if (!Board[RowIndex, ColumnIndex - 4].HasMoved)
                    {
                        possibleMoves.Add((RowIndex, ColumnIndex - 2));
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the king can castle through the given square.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>True if can castle through the given square, otherwise false.</returns>
        private bool CastlingPossibleForSquare(int row, int column)
        {
            if (Board[row, column] is null && !IsSquareUnderAttack(row, column))
                return true;
            return false;
        }

        /// <summary>
        /// Checks whether the king is protected in its current spot without triggering <see cref="IsUnderAttack"/> change.
        /// Used by other pieces to determine whether they may move to a certain position.
        /// </summary>
        /// <returns>True if the king is protected in its current position.</returns>
        public bool IsBeingProtected()
        {
            return !IsSquareUnderAttack(RowIndex, ColumnIndex);
        }

        /// <summary>
        /// Checks whether the king is protected in its current spot without triggering <see cref="IsUnderAttack"/> change.
        /// Used by other pieces to determine whether they may move to a certain position.
        /// </summary>
        /// <remarks>This overload returns true even if the king is being attacked by the piece given as argument.
        /// This is used by other pieces to check whether they may move to a different position even if the piece is protecting its king
        /// from being attacked - to capture the attacking piece for example.</remarks>
        /// <param name="ignoreThisPiece"></param>
        /// <returns>True if the king is protected in its current position.</returns>
        public bool IsBeingProtected(Piece ignoreThisPiece)
        {
            return !IsSquareUnderAttack(RowIndex, ColumnIndex, ignoreThisPiece: ignoreThisPiece);
        }

        /// <summary>
        /// Checks whether the given square is being attacked
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="occupied"></param>
        /// <param name="ignoreThisPiece"></param>
        /// <returns></returns>
        public bool IsSquareUnderAttack(int row, int column, bool occupied = false, Piece ignoreThisPiece = null)
        {
            foreach (Piece piece in Board.LivePieces[Color == PieceColor.White ? PieceColor.Black : PieceColor.White])
            {
                if (piece.Equals(ignoreThisPiece))
                    continue;

                if (piece.IsAttackingSquare(row, column, occupied))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether the king is being attacked and sets the <see cref="King.IsUnderAttack"/> to true if yes.
        /// </summary>
        /// <returns>True if the king is being attacked, otherwise false.</returns>
        public bool CheckIsUnderAttack()
        {
            if (IsSquareUnderAttack(RowIndex, ColumnIndex, true))
            {
                IsUnderAttack = true;
            }
            else
                IsUnderAttack = false;
            return IsUnderAttack;
        }

        /// <summary>
        /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// </summary>
        /// <param name="otherRowIndex"></param>
        /// <param name="otherColumnIndex"></param>
        /// <param name="_"></param>
        /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
        public override bool IsAttackingSquare(int otherRowIndex, int otherColumnIndex, bool _)
        {
            for (int row = -1; row < 2; row++)
            {
                for (int column = -1; column < 2; column++)
                {
                    if (RowIndex + row == otherRowIndex && ColumnIndex + column == otherColumnIndex)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Override method, sets the <see cref="King.IsCastling"/> property to true if conditions are met.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        internal override void Move(int row, int column)
        {
            if (!HasMoved && Math.Abs(ColumnIndex - column) > 1)
                IsCastling = true;
            base.Move(row, column);
        }
    }
}
