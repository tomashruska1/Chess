using Chess.Application.Boards;
using Chess.Application.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Application.Pieces
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
        /// <param name="square"></param>
        /// <param name="board"></param>
        public Bishop(PieceColor color, Square square, IBoard board)
        {
            Color = color;
            Square = square;
            Board = board;
            PossibleMoves = new();

            if (color == PieceColor.White)
                UnicodeValue = '\u2657';
            else
                UnicodeValue = '\u265D';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
        public override List<Square> GetPossibleMoves()
        {
            if (PossibleMoves.Count > 0)
                return PossibleMoves;

            PossibleMoves.AddRange(BishopPossibleMoves(this));

            RemoveInvalidMoves();
            return PossibleMoves;
        }

        /// <summary>
        /// Static method that calculates all moves a bishop can make if put into the position of the piece given as an argument.
        /// Calculates the moves for Bishop and Queen.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
        public static List<Square> BishopPossibleMoves(Piece piece)
        {
            Square square = piece.Square;

            bool upleft = true;
            bool upright = true;
            bool downleft = true;
            bool downright = true;

            for (int offset = 1; offset < 8; offset++)
            {
                if (upleft && Square.IsValid(square.Row - offset, square.Column - offset))
                {
                    upleft = piece.CanMoveToSquare(square + (-offset, -offset));
                }
                if (upright && Square.IsValid(square.Row - offset, square.Column + offset))
                {
                    upright = piece.CanMoveToSquare(square + (-offset, offset));
                }
                if (downleft && Square.IsValid(square.Row + offset, square.Column - offset))
                {
                    downleft = piece.CanMoveToSquare(square + (offset, -offset));
                }
                if (downright && Square.IsValid(square.Row + offset, square.Column + offset))
                {
                    downright = piece.CanMoveToSquare(square + (offset, offset));
                }
            }

           return piece.PossibleMoves;
        }

        /// <summary>
        /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// </summary>
        /// <param name="otherSquare"></param>
        /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
        public override bool IsThreateningSquare(Square otherSquare)
        {
            return IsThreateningSquare(this, otherSquare);
        }

        /// <summary>
        /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="otherSquare"></param>
        /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
        public static bool IsThreateningSquare(Piece piece, Square otherSquare)
        {
            if (otherSquare == piece.Square)
                return false;

            MoveVector vector = piece.Square - otherSquare;
            if (!vector.SquaresHaveDirectPath(MoveVectorDirection.Diagonal))
                return false;

            var squares = vector.GetSquares(MoveVectorDirection.Diagonal);

            return !squares.Where(square => piece.Board[square] is not null 
                                            && !piece.Board[square].GetType().Equals(typeof(King)))
                           .Any();
        }
    }
}
