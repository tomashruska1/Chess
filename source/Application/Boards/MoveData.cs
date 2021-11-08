namespace Chess.Application.Boards;


/// <summary>
/// Represents a single move as a point of origin, target, and any special events, that have happened during.
/// </summary>
internal readonly struct MoveData
{
    /// <summary>
    /// Represents the point of origin.
    /// </summary>
    internal Square FromSquare { get; }
    /// <summary>
    /// Represents the target square of the move.
    /// </summary>
    internal Square ToSquare { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="MoveData"/>.
    /// </summary>
    /// <param name="fromSquare"></param>
    /// <param name="toSquare"></param>
    public MoveData(Square fromSquare, Square toSquare)
    {
        FromSquare = fromSquare;
        ToSquare = toSquare;
    }

    /// <summary>
    /// Implicit cast from (<see cref="Square"/>, <see cref="Square"/>.
    /// </summary>
    /// <param name="tuple"></param>
    public static implicit operator MoveData((Square fromSquare, Square toSquare) tuple)
    {
        return new(tuple.fromSquare, tuple.toSquare);
    }
}
