using Chess.Application.Enums;
using System;
using System.Collections.Generic;

namespace Chess.Application.Boards
{
    /// <summary>
    /// Represents the direct path between two given <see cref="Square"/> instances.
    /// </summary>
    public struct MoveVector
    {
        /// <summary>
        /// Represents the starting <see cref="Square"/>.
        /// </summary>
        public Square Square1 { get; }
        /// <summary>
        /// Represents the end <see cref="Square"/>.
        /// </summary>
        public Square Square2 { get; }

        /// <summary>
        /// Creates an instance of <see cref="MoveVector"/>.
        /// </summary>
        /// <param name="square1"></param>
        /// <param name="square2"></param>
        public MoveVector(Square square1, Square square2)
        {
            Square1 = square1;
            Square2 = square2;
        }

        /// <summary>
        /// Reduces the given number to -1, 0, or 1 based on the <paramref name="number"/> value.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static int ReduceNumber(int number)
        {
            if (number > 0)
                return -1;
            else if (number < 0)
                return 1;
            return 0;
        }


        /// <summary>
        /// Returns a list of squares between the two instances of <see cref="Square"/> passed to constructor.
        /// </summary>
        /// <returns></returns>
        public List<Square> GetSquares()
        {
            return GetSquares(MoveVectorDirection.All);
        }

        /// <summary>
        /// Returns a list of squares between the two instances of <see cref="Square"/> passed to constructor in the given direction.
        /// </summary>
        /// <returns></returns>
        public List<Square> GetSquares(MoveVectorDirection direction)
        {
            List<Square> result = new();

            if (!SquaresHaveDirectPath(direction))
            {
                return result;
            }

            int rowOffset = ReduceNumber(Square1.Row - Square2.Row);
            int columnOffset = ReduceNumber(Square1.Column - Square2.Column);
            (int, int) offset = (rowOffset, columnOffset);

            Square intermediateSquare = Square1 + offset;

            while (intermediateSquare != Square2)
            {
                result.Add(intermediateSquare);
                intermediateSquare += offset;
            }

            return result;
        }

        /// <summary>
        /// Checks if the given squares are lying next to each other.
        /// </summary>
        /// <returns>True if the given squares are adjacent.</returns>
        public bool SquaresAreAdjacent()
        {
            return Math.Abs(Square1.Row - Square2.Row) < 2 && Math.Abs(Square1.Column - Square2.Column) < 2;
        }

        /// <summary>
        /// Checks if the given squares have a direct path.
        /// </summary>
        /// <returns>True if the given squares have a direct path.</returns>
        public bool SquaresHaveDirectPath() => SquaresHaveDirectPath(MoveVectorDirection.All);

        /// <summary>
        /// Checks if the given squares have a direct path in the given direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>True if the given squares have a direct path.</returns>
        public bool SquaresHaveDirectPath(MoveVectorDirection direction)
        {
            if (IsRegularDirection(direction) && (Square1.Row == Square2.Row || Square1.Column == Square2.Column))
                return true;

            if (IsDiagonalDirection(direction) && Math.Abs(Square1.Row - Square2.Row) == Math.Abs(Square1.Column - Square2.Column))
                return true;

            return false;
        }

        private static bool IsDiagonalDirection(MoveVectorDirection direction)
        {
            return (direction | MoveVectorDirection.Diagonal) == direction;
        }

        private static bool IsRegularDirection(MoveVectorDirection direction)
        {
            return (direction | MoveVectorDirection.Regular) == direction;
        }
    }
}
