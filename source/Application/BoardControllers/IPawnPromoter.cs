using Chess.Application.Enums;

namespace Chess.Application.BoardControllers
{
    /// <summary>
    /// Provides a method for resolving which piece should a pawn be promoted into.
    /// </summary>
    public interface IPawnPromoter
    {
        /// <summary>
        /// Resolves which piece should a pawn be promoted into.
        /// </summary>
        /// <returns></returns>
        public PromotedPiece GetPromotedPiece();
    }
}
