using Chess.Application.Boards;
using Chess.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Application.Pieces
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
        /// <param name="square"></param>
        /// <param name="board"></param>
        public Knight(PieceColor color, Square square, IBoard board)
        {
            Color = color;
            Square = square;
            Board = board;
            PossibleMoves = new();

            if (color == PieceColor.White)
                UnicodeValue = '\u2658';
            else
                UnicodeValue = '\u265E';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
        public override List<Square> GetPossibleMoves()
        {
            if (PossibleMoves.Count > 0)
                return PossibleMoves;

            List<(int, int)> coordinates = new();
            coordinates.Add((Square.Row + 2, Square.Column + 1));
            coordinates.Add((Square.Row + 2, Square.Column - 1));
            coordinates.Add((Square.Row - 2, Square.Column + 1));
            coordinates.Add((Square.Row - 2, Square.Column - 1));
            coordinates.Add((Square.Row + 1, Square.Column + 2));
            coordinates.Add((Square.Row + 1, Square.Column - 2));
            coordinates.Add((Square.Row - 1, Square.Column + 2));
            coordinates.Add((Square.Row - 1, Square.Column - 2));

            PossibleMoves.AddRange(coordinates
                                     .Where(tuple => Square.IsValid(tuple))
                                     .Select(tuple => (Square)tuple)
                                     .Where(square => Board[square] is null || Board[square].Color != Color)
                                     .Where(square => !Board.Kings[Color].IsUnderAttack
                                            || (Board.LineOfAttack.Count == 1 && Board.LineOfAttack.Values.First().Contains(square))));

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
            return (Math.Abs(Square.Row - otherSquare.Row) == 2 && Math.Abs(Square.Column - otherSquare.Column) == 1)
                || (Math.Abs(Square.Row - otherSquare.Row) == 1 && Math.Abs(Square.Column - otherSquare.Column) == 2);
        }
    }
}
