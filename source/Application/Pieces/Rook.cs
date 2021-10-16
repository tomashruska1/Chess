using Chess.Application.Boards;
using Chess.Application.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Application.Pieces
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
        /// <param name="square"></param>
        /// <param name="board"></param>
        public Rook(PieceColor color, Square square, IBoard board)
        {
            Color = color;
            Square = square;
            Board = board;
            HasMoved = false;
            PossibleMoves = new();

            if (color == PieceColor.White)
                UnicodeValue = '\u2656';
            else
                UnicodeValue = '\u265C';
        }

        /// <summary>
        /// Override method, calculates all possible moves for a given piece.
        /// </summary>
        /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
        public override List<Square> GetPossibleMoves()
        {
            if (PossibleMoves.Count > 0)
                return PossibleMoves;

            PossibleMoves.AddRange(RookPossibleMoves(this));

            RemoveInvalidMoves();
            return PossibleMoves;
        }

        /// <summary>
        /// Static method that calculates all moves a rook can make if put into the position of the piece given as an argument.
        /// Calculates the moves for Rook and Queen.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
        public static List<Square> RookPossibleMoves(Piece piece)
        {
            Square square = piece.Square;

            bool up = true;
            bool down = true;
            bool left = true;
            bool right = true;

            for (int offset = 1; offset < 8; offset++)
            {
                if (up && Square.IsValid(square.Row - offset, square.Column))
                {
                    up = piece.CanMoveToSquare(square + (-offset, 0));
                }
                if (down && Square.IsValid(square.Row + offset, square.Column))
                {
                    down = piece.CanMoveToSquare(square + (offset, 0));
                }
                if (left && Square.IsValid(square.Row, square.Column - offset))
                {
                    left = piece.CanMoveToSquare(square + (0, -offset));
                }
                if (right && Square.IsValid(square.Row, square.Column + offset))
                {
                    right = piece.CanMoveToSquare(square + (0, offset));
                }
            }

            return piece.PossibleMoves;
        }

        /// <summary>
        /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
        /// </summary>
        /// <param name="otherSquare"> </param>
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
            if (!vector.SquaresHaveDirectPath(MoveVectorDirection.Regular))
                return false;

            var squares = vector.GetSquares(MoveVectorDirection.Regular);

            return !squares.Where(square => piece.Board[square] is not null
                                            && !piece.Board[square].GetType().Equals(typeof(King)))
                            .Any();
        }
    }
}
