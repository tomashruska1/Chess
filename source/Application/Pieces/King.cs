namespace Chess.Application.Pieces;


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
    /// Creates an instance of the King class.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="square"></param>
    /// <param name="board"></param>
    public King(PieceColor color, Square square, IBoard board)
    {
        Color = color;
        Square = square;
        Board = board;
        IsUnderAttack = false;
        HasMoved = false;
        PossibleMoves = new();

        if (color == PieceColor.White)
            UnicodeValue = '\u2654';
        else
            UnicodeValue = '\u265A';
    }

    /// <summary>
    /// Override method, calculates all possible moves for a given piece.
    /// </summary>
    /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
    public override List<Square> GetPossibleMoves()
    {
        if (PossibleMoves.Count > 0)
            return PossibleMoves;

        var offsets = Enumerable.Range(-1, 3)
                                    .SelectMany(x => Enumerable.Range(-1, 3).Select(y => (x, y)))
                                    .Where(x => x != (0, 0));
        PossibleMoves.AddRange(offsets
                                .Where(tuple => Square.IsValid(Square.Row + tuple.x, Square.Column + tuple.y))
                                .Select(tuple => Square + tuple)
                                .Where(square => Board[square] is null || Board[square].Color != Color)
                                .Where(square => !Board.IsSquareUnderAttack(this, square)));
        if (!HasMoved && !IsUnderAttack)
            CheckForCastlingOpportunities();

        return PossibleMoves;
    }

    /// <summary>
    /// Checks whether the king can castle to either side
    /// </summary>
    private void CheckForCastlingOpportunities()
    {
        bool rightSideClear = !Enumerable.Range(1, 2).Where(x => !CastlingPossibleForSquare(Square + (0, x))).Any();

        if (rightSideClear && Board[Square + (0, 3)] is not null && Board[Square + (0, 3)].GetType().Equals(typeof(Rook)))
        {
            if (!Board[Square + (0, 3)].HasMoved)
            {
                PossibleMoves.Add(Square + (0, 2));
            }
        }

        bool leftSideClear = !Enumerable.Range(1, 3).Where(x => !CastlingPossibleForSquare(Square + (0, -x))).Any();

        if (leftSideClear && Board[Square + (0, -4)] is not null && Board[Square + (0, -4)].GetType().Equals(typeof(Rook)))
        {
            if (!Board[Square + (0, -4)].HasMoved)
            {
                PossibleMoves.Add(Square + (0, -2));
            }
        }
    }

    /// <summary>
    /// Checks if the king can castle through the given square.
    /// </summary>
    /// <param name="square"></param>
    /// <returns>True if can castle through the given square, otherwise false.</returns>
    private bool CastlingPossibleForSquare(Square square)
    {
        if (Board[square] is null && !IsSquareUnderAttack(square))
            return true;
        return false;
    }

    /// <summary>
    /// Checks whether the given square is being attacked
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    private bool IsSquareUnderAttack(Square square)
    {
        return Board.IsSquareUnderAttack(this, square);
    }

    /// <summary>
    /// Checks whether the king is being attacked and sets the <see cref="King.IsUnderAttack"/> to true if yes.
    /// </summary>
    /// <returns>True if the king is being attacked, otherwise false.</returns>
    public bool CheckIsUnderAttack()
    {
        if (IsSquareUnderAttack(Square))
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
    /// <param name="otherSquare"></param>
    /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
    public override bool IsThreateningSquare(Square otherSquare)
    {
        return Math.Abs(Square.Row - otherSquare.Row) < 2 && Math.Abs(Square.Column - otherSquare.Column) < 2;
    }
}
