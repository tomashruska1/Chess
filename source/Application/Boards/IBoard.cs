namespace Chess.Application.Boards;


/// <summary>
/// Represents the chessboard. Provides read access to pieces on the board and a collection of coordinates that are under attack per piece.
/// </summary>
public interface IBoard
{
    /// <summary>
    /// Provides a read-only access to the item on the given coordinates.
    /// </summary>
    /// <param name="square"></param>
    /// <returns>The piece on the given coordinates.</returns>
    public Piece this[Square square] { get; }
    /// <summary>
    /// Keeps a reference for all pieces attacking the king of the opposite color and the paths they attack the king on.
    /// </summary>
    public Dictionary<Piece, List<Square>> LineOfAttack { get; set; }
    /// <summary>
    /// Sets up the provided board.
    /// </summary>
    public void SetUpBoard();
    /// <summary>
    /// Represents the color that is currently on the move.
    /// </summary>
    public PieceColor NextMove { get; }
    /// <summary>
    /// Represents the winner of the game.
    /// </summary>
    public GameResultEnum? Winner { get; }
    /// <summary>
    /// Keeps a reference for all the pieces on the board by color.
    /// </summary>
    public Dictionary<PieceColor, List<Piece>> LivePieces { get; }
    /// <summary>
    /// Moves the given piece to the given square. Handles the encompassing aspects.
    /// </summary>
    /// <param name="fromSquare"></param>
    /// <param name="toSquare"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public MoveType MovePiece(Square fromSquare, Square toSquare);
    /// <summary>
    /// Moves the given piece to the given square. Handles the encompassing aspects.
    /// </summary>
    /// <param name="fromSquare"></param>
    /// <param name="toSquare"></param>
    /// <param name="moveSpecial"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public MoveType MovePiece(Square fromSquare, Square toSquare, Action<Square, Square> moveSpecial);
    /// <summary>
    /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public MoveType MovePiece(Piece piece, Square square);
    /// <summary>
    /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
    /// Calls the <paramref name="moveFromTo"/> delegate for <see cref="Rook"/> movement during <see cref="King"/> castling.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    /// <param name="moveFromTo"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public MoveType MovePiece(Piece piece, Square square, Action<Square, Square> moveFromTo);
    /// <summary>
    /// Checks if the given piece is protecting its king against a single attacker and returns the path it would take.
    /// Returns false if there is none, or more than one attacker.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="squares"></param>
    /// <returns>True if the piece is protecting its king, otherwise false.</returns>
    public bool IsProtectingKing(Piece piece, out List<Square> squares);
    /// <summary>
    /// Calculates all the legal moves for the piece on the given coordinates.
    /// </summary>
    /// <param name="square"></param>
    /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
    public List<Square> ValidMovesForPiece(Square square);
    /// <summary>
    /// Calculates all the legal moves for a given piece.
    /// </summary>
    /// <param name="piece"></param>
    /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
    public List<Square> ValidMovesForPiece(Piece piece);
    /// <summary>
    /// Keeps a reference for both kings on the board by color.
    /// </summary>
    public Dictionary<PieceColor, King> Kings { get; }
    /// <summary>
    /// Checks whether the given square is being attacked.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="square"></param>
    /// <returns></returns>
    public bool IsSquareUnderAttack(PieceColor color, Square square);
    /// <summary>
    /// Checks whether the given square is being attacked.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    /// <returns></returns>
    public bool IsSquareUnderAttack(Piece piece, Square square);
    /// <summary>
    /// Checks whether the king of the given color is under attack.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool IsKingUnderAttack(PieceColor color);
    /// <summary>
    /// Checks whether the given pawn can do en passant capture.
    /// </summary>
    /// <param name="pawn"></param>
    /// <returns>Column offset determining to which side can the move be performed, 0 if it can't.</returns>
    public int CanDoEnPassant(Pawn pawn);
}
