using Chess.Application.Boards;
using Chess.Application.Enums;

namespace Chess.Application.ChessAIs
{
    /// <summary>
    /// Specifies methods a chess AI needs to implement.
    /// </summary>
    public interface IChessAI
    {
        /// <summary>
        /// Represents the color the AI is playing as.
        /// </summary>
        public PieceColor ColorOfAI { get; }
        /// <summary>
        /// Returns the next move by the <see cref="IChessAI"/>. 
        /// </summary>
        /// <returns></returns>
        public void DoNextMove();
        /// <summary>
        /// Registers a move made by the opponent.
        /// </summary>
        /// <param name="fromSquare"></param>
        /// <param name="toSquare"></param>
        public void RegisterEnemyMove(Square fromSquare, Square toSquare);
        /// <summary>
        /// Sets up a board with a non-standard piece layout, e.g. a specific positions. Copies the given <see cref="IBoard"/> instance's piece positions.
        /// </summary>
        public void SetUpNonstandardBoard(IBoard board);
    }
}
