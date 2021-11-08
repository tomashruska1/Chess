namespace Chess.Application.Pieces;


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
    public char UnicodeValue { get; protected init; }
    /// <summary>
    /// Provides an index of the row on which the piece is located.
    /// </summary>
    public Square Square { get; set; }
    /// <summary>
    /// Provides a reference to the <see cref="IBoard"/> instance that represents the chessboard.
    /// </summary>
    public IBoard Board { get; protected init; }
    /// <summary>
    /// Provides a check whether the piece has moved - important for several pieces.
    /// </summary>
    public bool HasMoved { get; protected set; } = false;
    /// <summary>
    /// Abstract method, override should calculate all possible moves for a given piece.
    /// </summary>
    /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
    public List<Square> PossibleMoves { get; protected set; }
    /// <summary>
    /// Abstract method, override should calculate all possible moves for a given piece.
    /// </summary>
    /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
    public abstract List<Square> GetPossibleMoves();
    /// <summary>
    /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
    /// </summary>
    /// <param name="otherSquare"></param>
    /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
    public abstract bool IsThreateningSquare(Square otherSquare);

    /// <summary>
    /// Checks if the piece can move to <paramref name="square"/> and adds the square to <see cref="PossibleMoves"/>.
    /// </summary>
    /// <param name="square"></param>
    /// <param name="captureMode"></param>
    /// <returns>True if the square is empty and piece can continue further in the direction.</returns>
    internal bool CanMoveToSquare(Square square, CaptureMode captureMode = CaptureMode.All)
    {
        if (CheckCanMoveToSquare(square, captureMode))
            PossibleMoves.Add(square);
        return Board[square] is null;
    }

    /// <summary>
    /// Checks if the piece can move to <paramref name="square"/> - if it is unoccupied, or occupied by a correct piece depending on the context.
    /// </summary>
    /// <param name="square"></param>
    /// <param name="captureMode"></param>
    /// <returns>True if the piece can move to the given square.</returns>
    private bool CheckCanMoveToSquare(Square square, CaptureMode captureMode = CaptureMode.All)
    {
        if (Board[square] is null && captureMode == CaptureMode.CaptureOnly)
            return false;

        if (Board[square] is null)
            return true;

        if (Board[square].Color == Color || captureMode == CaptureMode.NoCapture)
            return false;

        if (Board.Kings[Color].IsUnderAttack)
            return CanPreventKingFromBeingAttacked(square);

        return true;
    }

    /// <summary>
    /// Removes invalid moves from the list of available moves based on whether own king would remain safe for each given move.
    /// </summary>
    private protected void RemoveInvalidMoves()
    {
        if (Board.IsProtectingKing(this, out List<Square> lineOfAttack))
        {
            PossibleMoves = PossibleMoves.Where(square => lineOfAttack.Contains(square)).ToList();
        }
    }

    private bool CanPreventKingFromBeingAttacked(Square square)
    {
        return Board.LineOfAttack.Count == 1 && Board.LineOfAttack.Values.First().Contains(square);
    }

    /// <summary>
    /// Handles the internals of moving the piece from the viewpoint of the piece - changing it's row and column
    /// and setting its <see cref="HasMoved"/> property to true.
    /// </summary>
    /// <param name="square"></param>
    internal virtual void Move(Square square)
    {
        Square = square;
        HasMoved = true;
    }

    /// <summary>
    /// Converts the piece to its unicode representation as string.
    /// </summary>
    /// <returns>Unicode representation of the piece as string.</returns>
    public override string ToString()
    {
        return UnicodeValue.ToString();
    }

    /// <summary>
    /// Sets <see cref="HasMoved"/> to false.
    /// </summary>
    internal void ResetHasMoved()
    {
        HasMoved = false;
    }
}
