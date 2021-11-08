namespace Chess.Application.BoardControllers;


/// <summary>
/// Interface that specifies the methods a board class needs to implement.
/// </summary>
public interface IBoardController
{
    /// <summary>
    /// Represents the chessboard.
    /// </summary>
    public IBoard Board { get; set; }
    /// <summary>
    /// Keeps a reference for all the moves made during the game, may export the data to a text file as standard notation.
    /// </summary>
    public IMoveRecordExporter Moves { get; }
    /// <summary>
    /// Represents the color that is currently on the move.
    /// </summary>
    public PieceColor NextMove { get; }
    /// <summary>
    /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    public void MovePiece(Piece piece, Square square);
    /// <summary>
    /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
    /// </summary>
    /// <param name="fromSquare"></param>
    /// <param name="toSquare"></param>
    public void MovePiece(Square fromSquare, Square toSquare);
    /// <summary>
    /// Provides a read-only access to the item on the given coordinates.
    /// </summary>
    /// <param name="square"></param>
    /// <returns>The piece on the given coordinates.</returns>
    public Piece this[Square square] { get; }
    /// <summary>
    /// Calculates all the legal moves for the piece on the given coordinates.
    /// </summary>
    /// <param name="square"></param>
    /// <returns>A list of all possible moves as Square.</returns>
    public List<Square> ValidMovesForPiece(Square square);
    /// <summary>
    /// Calculates all the legal moves for a given piece.
    /// </summary>
    /// <param name="piece"></param>
    /// <returns>A list of all possible moves as Square.</returns>
    public List<Square> ValidMovesForPiece(Piece piece);
    /// <summary>
    /// Signals to the controller that the game may start. Only has effect if an AI is playing as white.
    /// </summary>
    public void StartGame();
}
