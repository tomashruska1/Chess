using Chess.Application.Boards;
using Chess.Application.Enums;

namespace Chess.Application.BoardControllers
{
    /// <summary>
    /// Specifies methods a user interface for the chess needs to implement.
    /// </summary>
    public interface IChessboardInterface
    {
        /// <summary>
        /// Starts a new game of chess for two human players.
        /// </summary>
        public void NewGame();
        /// <summary>
        /// Starts a new game of chess against an AI.
        /// </summary>
        /// <param name="aiColor"></param>
        public void NewGame(PieceColor aiColor);
        /// <summary>
        /// Moves a piece on <paramref name="fromSquare"/> to <paramref name="toSquare"/>.
        /// </summary>
        /// <param name="fromSquare"></param>
        /// <param name="toSquare"></param>
        public void MovePiece(Square fromSquare, Square toSquare);
        /// <summary>
        /// Promotes a pawn based on player's feedback.
        /// </summary>
        /// <returns></returns>
        public PromotedPiece PromotePawn();
        /// <summary>
        /// Action taken when a game has ended with the result <paramref name="endOfGame"/>.
        /// </summary>
        /// <param name="endOfGame"></param>
        public void EndOfGame(GameResultEnum endOfGame);
        /// <summary>
        /// Visually moves a piece on the chessboard.
        /// </summary>
        /// <param name="fromSquare"></param>
        /// <param name="toSquare"></param>
        public void SetPiecePosition(Square fromSquare, Square toSquare);
        /// <summary>
        /// Visually moves a piece on the chessboard.
        /// </summary>
        /// <param name="fromSquare"></param>
        public void RemovePiece(Square fromSquare);
    }
}
