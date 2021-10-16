using Chess.Application.BoardControllers;
using Chess.Application.Enums;
using Chess.Application.Pieces;
using System;

namespace Chess.Test.Mocks
{
    public class PawnPromoter : IPawnPromoter
    {
        private Type GiveType { get; set; }
        public PawnPromoter(Type type)
        {
            GiveType = type;
        }

        public PromotedPiece GetPromotedPiece()
        {
            if (GiveType.Equals(typeof(Queen)))
                return PromotedPiece.Queen;
            if (GiveType.Equals(typeof(Bishop)))
                return PromotedPiece.Bishop;
            if (GiveType.Equals(typeof(Knight)))
                return PromotedPiece.Knight;
            if (GiveType.Equals(typeof(Rook)))
                return PromotedPiece.Rook;
            else
                throw new ArgumentException($"{nameof(GiveType)} does not have an acceptable value: {GiveType}!");
        }
    }
}
