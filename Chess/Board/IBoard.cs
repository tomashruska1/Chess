using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Board
{
    /// <summary>
    /// Interface that specifies the methods a board class needs to implement.
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// Keeps a reference for all the moves made during the game, may export the data to a text file as standard notation.
        /// </summary>
        public Moves Moves { get; }
        /// <summary>
        /// Keeps a reference for all the pieces on the board by color.
        /// </summary>
        public Dictionary<PieceColor, List<Piece>> LivePieces { get; }
        /// <summary>
        /// Keeps a reference for both kings on the board by color.
        /// </summary>
        public Dictionary<PieceColor, King> Kings { get; }
        /// <summary>
        /// Keeps a reference for all pieces attacking the king of the opposite color and the paths they attack the king on.
        /// </summary>
        public Dictionary<Piece, List<(int, int)>> LineOfAttack { get; set; }
        /// <summary>
        /// Represents the color that is currently on the move.
        /// </summary>
        public PieceColor NextMove { get; }
        /// <summary>
        /// Evaluates whether the move has ended the game and how - checkmate or draw.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Returns the type of move - a bit flag enum.</returns>
        public MoveType EvaluateMove(MoveType type);
        /// <summary>
        /// Calculates all the legal moves for a given piece.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public List<(int, int)> LegalMovesForPiece(Piece piece);
        /// <summary>
        /// Calculates all the legal moves for the piece on the given coordinates.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public List<(int, int)> LegalMovesForPiece(int row, int column);
        /// <summary>
        /// Checks if the given piece is protecting its king.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>True if the piece is protecting its king, otherwise false/.</returns>
        public bool IsProtectingKing(Piece piece);
        /// <summary>
        /// Checks if the given piece is protecting its king.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool IsProtectingKing(Piece piece, int row, int column);
        /// <summary>
        /// Moves the piece on the board, removes the piece of opposing color if it is on the square and calls <see cref="EvaluateMove(MoveType)"/>
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void MovePiece(Piece piece, int row, int column);
        /// <summary>
        /// Provides a readonly access to the item on the given coordinates (row, column).
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>The piece on the given coordinates.</returns>
        public Piece this[int row, int column] { get; }
    }
}
