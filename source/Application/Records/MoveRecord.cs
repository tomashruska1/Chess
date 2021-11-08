namespace Chess.Application.Records;


/// <summary>
/// Struct used to capture data about each move made in a give game.
/// </summary>
public struct MoveRecord
{
    /// <summary>
    /// The piece taking this move.
    /// </summary>
    public Piece Piece { get; private set; }
    /// <summary>
    /// The starting row of the piece.
    /// </summary>
    public int StartRow { get => StartPoint.Row; }
    /// <summary>
    /// The starting column of the piece.
    /// </summary>
    public int StartColumn { get => StartPoint.Column; }
    /// <summary>
    /// Represents the starting coordinates of the move.
    /// </summary>
    public Square StartPoint { get; private set; }
    /// <summary>
    /// Represents the end coordinates of the move.
    /// </summary>
    public Square EndPoint { get; private set; }
    /// <summary>
    /// The end row of the piece.
    /// </summary>
    public int Row { get => EndPoint.Row; }
    /// <summary>
    /// The end column of the piece.
    /// </summary>
    public int Column { get => EndPoint.Column; }
    /// <summary>
    /// The <see cref="MoveType"/> enum that contains information about events during the piece's move.
    /// </summary>
    public MoveType Type { get; private set; }

    /// <summary>
    /// Creates an instance of the Move struct.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="startRow"></param>
    /// <param name="startColumn"></param>
    /// <param name="moveRow"></param>
    /// <param name="moveColumn"></param>
    /// <param name="moveType"></param>
    public MoveRecord(Piece piece, int startRow, int startColumn, int moveRow, int moveColumn, MoveType moveType)
    {
        StartPoint = new(startRow, startColumn);
        EndPoint = new(moveRow, moveColumn);
        Piece = piece;
        Type = moveType;
    }

    /// <summary>
    /// Creates an instance of the Move struct.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="moveType"></param>
    public MoveRecord(Piece piece, Square startPoint, Square endPoint, MoveType moveType)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        Piece = piece;
        Type = moveType;
    }
}
