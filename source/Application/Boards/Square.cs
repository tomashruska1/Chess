using System;

namespace Chess.Application.Boards
{
    /// <summary>
    /// Represents a single coordinate on the chessboard. Allows for implicit conversion from (int row, int column) to <see cref="Square"/>.
    /// Disallows invalid row and column values.
    /// </summary>
    public readonly struct Square
    {
        /// <summary>
        /// Represents the row of the coordinate as index from the top.
        /// </summary>
        public int Row { get; }
        /// <summary>
        /// Represents the column of the coordinate as index from the left.
        /// </summary>
        public int Column { get; }
        /// <summary>
        /// Minimum acceptable value for <see cref="Square"/> properties.
        /// </summary>
        private static int MinValue { get; } = 0;
        /// <summary>
        /// Maximum acceptable value for <see cref="Square"/> properties.
        /// </summary>
        private static int MaxValue { get; } = 7;
        /// <summary>
        /// Creates an instance of the Square struct.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Square(int row, int column)
        {
            if (!IsValid(row, column))
            {
                throw new ArgumentException($"RowIndex and ColumnIndex must be between 0 and 7 inclusive! {nameof(Row)} {row}, {nameof(Column)} {column}");
            }

            Row = row;
            Column = column;
        }

        /// <summary>
        /// Implicit cast from (<see cref="Int32"/>, <see cref="Int32"/>) to <see cref="Square"/>.
        /// </summary>
        /// <param name="tuple"></param>
        public static implicit operator Square((int, int) tuple)
        {
            return new Square(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// Explicit cast from (<see cref="Int32"/>, <see cref="Int32"/>) to <see cref="Square"/>.
        /// </summary>
        /// <param name="square"></param>
        public static explicit operator (int, int)(Square square)
        {
            return (square.Row, square.Column);
        }

        /// <summary>
        /// Provides sum operation between two instances of <see cref="Square"/> or (<see cref="Int32"/>, <see cref="Int32"/>).
        /// </summary>
        /// <param name="square1"></param>
        /// <param name="tuple"></param>
        /// <returns></returns>
        public static Square operator +(Square square1, (int, int) tuple)
        {
            return new Square(square1.Row + tuple.Item1, square1.Column + tuple.Item2);
        }

        /// <summary>
        /// Provides sum operation between two instances of <see cref="Square"/> or (<see cref="Int32"/>, <see cref="Int32"/>) where applicable.
        /// </summary>
        /// <param name="square1"></param>
        /// <param name="square2"></param>
        /// <returns></returns>
        public static Square operator +(Square square1, Square square2)
        {
            return new Square(square1.Row + square2.Row, square1.Column + square2.Column);
        }

        /// <summary>
        /// Provides subtraction operation between two instances of <see cref="Square"/> or (<see cref="Int32"/>, <see cref="Int32"/>) where applicable.
        /// </summary>
        /// <param name="square1"></param>
        /// <param name="square2"></param>
        /// <returns><see cref="MoveVector"/> instance.</returns>
        public static MoveVector operator -(Square square1, Square square2)
        {
            return new MoveVector(square1, square2);
        }

        /// <summary>
        /// Provides equality check between two instances of <see cref="Square"/> or (<see cref="Int32"/>, <see cref="Int32"/>) where applicable.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static bool operator ==(Square point1, Square point2)
        {
            return point1.Row == point2.Row && point1.Column == point2.Column;
        }

        /// <summary>
        /// Provides inequality check between two instances of <see cref="Square"/> or (<see cref="Int32"/>, <see cref="Int32"/>) where applicable.
        /// </summary>
        /// <param name="square1"></param>
        /// <param name="square2"></param>
        /// <returns></returns>
        public static bool operator !=(Square square1, Square square2)
        {
            return !(square1 == square2);
        }

        /// <summary>
        /// Checks whether the coordinates as (<see cref="Int32"/>, <see cref="Int32"/>) are a part of the board.
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns>True if the coordinates are a part of the board.</returns>
        public static bool IsValid((int, int) tuple)
        {
            return IsValid(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// Checks whether the coordinates are a part of the board.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>True if the coordinates are a part of the board.</returns>
        public static bool IsValid(int row, int column)
        {
            return row >= MinValue && row <= MaxValue && column >= MinValue && column <= MaxValue;
        }

        /// <summary>
        /// Attempts to create a <see cref="Square"/> and returns it as an out <paramref name="square"/>. If parameters are not valid, returns it as null.
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="square"></param>
        /// <returns>True if parameters are valid, false if not.</returns>
        public static bool TryCreate((int, int) tuple, out Square? square)
        {
            if (IsValid(tuple))
            {
                square = new Square(tuple.Item1, tuple.Item2);
                return true;
            }
            square = null;
            return false;
        }

        /// <summary>
        /// Attempts to create a <see cref="Square"/> and returns it as an out <paramref name="square"/>. If parameters are not valid, returns it as null.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="square"></param>
        /// <returns>True if parameters are valid, false if not.</returns>
        public static bool TryCreate(int row, int column, out Square? square)
        {
            return TryCreate((row, column), out square);
        }

        /// <summary>
        /// Provides equality check between two instances of <see cref="Square"/> or (<see cref="Int32"/>, <see cref="Int32"/>).
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (other is null || !GetType().Equals(other.GetType()))
                return false;
            return this == (Square)other;
        }

        /// <summary>
        /// Overrides <see cref="Object.GetHashCode()"/> for <see cref="Square"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Row.GetHashCode() ^ Column.GetHashCode();
        }

        /// <summary>
        /// Overrides <see cref="Object.ToString()"/> for <see cref="Square"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Square)}({Row}, {Column})";
        }
    }
}
