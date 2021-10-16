using Chess.Application.Boards;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Application.ChessAIs.Moves
{
    /// <summary>
    /// Represents a single move with a list of possible moves that can follow it.
    /// Keeps a score for itself and subsequent moves that is used for evaluation.
    /// </summary>
    internal class MoveEvaluator
    {
        /// <summary>
        /// Represents the row of the coordinate as index from the top.
        /// </summary>
        internal Square FromSquare { get => MoveData.FromSquare; }
        /// <summary>
        /// Represents the column of the coordinate as index from the left.
        /// </summary>
        internal Square ToSquare { get => MoveData.ToSquare; }
        /// <summary>
        /// Represents the numerical score of the move for evaluation.
        /// </summary>
        private double Score { get; set; }
        /// <summary>
        /// Represents the starting square of the move, the end square, and events.
        /// </summary>
        internal MoveData MoveData { get; set; }
        /// <summary>
        /// Represents the possible moves that can follow this move.
        /// </summary>
        internal List<MoveEvaluator> NextMoves { get; set; }

        /// <summary>
        /// Creates an instance of the <see cref="MoveEvaluator"/>.
        /// </summary>
        /// <param name="moveData"></param>
        /// <param name="score"></param>
        internal MoveEvaluator(MoveData moveData, double score)
        {
            MoveData = moveData;
            Score = score;
            NextMoves = new();
        }

        /// <summary>
        /// Creates an instance of the <see cref="MoveEvaluator"/>.
        /// </summary>
        /// <param name="fromSquare"></param>
        /// <param name="toSquare"></param>
        /// <param name="score"></param>
        public MoveEvaluator(Square fromSquare, Square toSquare, double score) : this((fromSquare, toSquare), score)
        {
        }

        /// <summary>
        /// Changes the <see cref="Score"/> to <paramref name="newScore"/>.
        /// </summary>
        /// <param name="newScore"></param>
        internal void SetScore(double newScore)
        {
            Score = newScore;
        }

        /// <summary>
        /// Provides a hypothetical score for the move and all the possible best moves following it.
        /// </summary>
        /// <returns></returns>
        internal double GetScore(bool isAIColor)
        {
            if (NextMoves.Count == 0)
                return Score;
            
            if (isAIColor)
                return NextMoves.Select(x => x.GetScore(!isAIColor)).Max() + Score;

            return NextMoves.Select(x => x.GetScore(!isAIColor)).Min() + Score;
        }

        /// <summary>
        /// Registers a move following this one.
        /// </summary>
        /// <param name="startSquare"></param>
        /// <param name="endSquare"></param>
        /// <param name="score"></param>
        internal void AddMove(Square startSquare, Square endSquare, double score)
        {
            AddMove(new MoveEvaluator(startSquare, endSquare, score));
        }

        /// <summary>
        /// Registers a move following this one.
        /// </summary>
        /// <param name="move"></param>
        internal void AddMove(MoveEvaluator move)
        {
            NextMoves.Add(move);
        }
    }
}
