using System.Collections.Generic;

namespace Chess.Application.ChessAIs.Weights
{
    /// <summary>
    /// Provides weights for move comparison.
    /// </summary>
    public static class ChessWeights
    {
        /// <summary>
        /// Provides the weight of the pieces used in the calculation of the moves.
        /// </summary>
        public static Dictionary<string, double> PieceWeights { get; } = new()
        {
            { "Pawn", 1 },
            { "Bishop", 3 },
            { "Knight", 3 },
            { "Rook", 5 },
            { "Queen", 9 },
            { "King", 100 }
        };

        /// <summary>
        /// Provides square weights for pawns.
        /// </summary>
        public static readonly double[,] PawnWeights = new double[,]
        {
            { 8.0, 8.0, 8.0, 8.0, 8.0, 8.0, 8.0, 8.0 },
            { 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6 },
            { 0.5, 0.5, 0.5, 0.6, 0.6, 0.5, 0.5, 0.5 },
            { 0.3, 0.4, 0.4, 0.6, 0.6, 0.4, 0.4, 0.3 },
            { 0.2, 0.3, 0.3, 0.8, 0.8, 0.3, 0.3, 0.2 },
            { 0.1, 0.2, 0.2, 0.0, 0.0, 0.2, 0.2, 0.1 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
        };

        /// <summary>
        /// Provides square weights for bishops.
        /// </summary>
        public static readonly double[,] BishopWeights = new double[,]
        {
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
        };

        /// <summary>
        /// Provides square weights for knights.
        /// </summary>
        public static readonly double[,] KnightWeights = new double[,]
        {
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
        };

        /// <summary>
        /// Provides square weights for rooks.
        /// </summary>
        public static readonly double[,] RookWeights = new double[,]
        {
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.6, 0.8, 0.8, 0.6, 0.0, 0.0 },
        };

        /// <summary>
        /// Provides square weights for queens.
        /// </summary>
        public static readonly double[,] QueenWeights = new double[,]
        {
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
        };

        /// <summary>
        /// Provides square weights for king.
        /// </summary>
        public static readonly double[,] KingWeights = new double[,]
        {
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
        };
    }
}
