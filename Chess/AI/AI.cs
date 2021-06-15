using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;
using Chess.Board;
using Chess.Pieces;

namespace Chess.AI
{
    /// <summary>
    /// Class representing a chess AI that can play against the user.
    /// </summary>
    public class AI
    {
        /// <summary>
        /// Represents a single move with its score that is used for evaulation.
        /// </summary>
        private struct Move
        {
            /// <summary>
            /// Represents the starting row of the move.
            /// </summary>
            int StartRow { get; }
            /// <summary>
            /// Represents the starting column of the move.
            /// </summary>
            int StartColumn { get; }
            /// <summary>
            /// Represents the ending row of the move.
            /// </summary>
            int EndRow { get; }
            /// <summary>
            /// Represents the ending column of the move.
            /// </summary>
            int EndColumn { get; }
            /// <summary>
            /// Represents the numerical score of the move for evaluation.
            /// </summary>
            int Score { get; }

            /// <summary>
            /// Creates an instance of the <see cref="Move"/> struct.
            /// </summary>
            /// <param name="startRow"></param>
            /// <param name="startColumn"></param>
            /// <param name="endRow"></param>
            /// <param name="endColumn"></param>
            /// <param name="score"></param>
            public Move(int startRow, int startColumn, int endRow, int endColumn, int score)
            {
                StartRow = startRow;
                StartColumn = startColumn;
                EndRow = endRow;
                EndColumn = endColumn;
                Score = score;
            }
        }

        /// <summary>
        /// Provides a reference to the <see cref="IBoard"/> object that is used to run the game.
        /// </summary>
        private IBoard Board { get; set; }
        /// <summary>
        /// Own copy of the chessboard used to calculate the moves.
        /// </summary>
        private Piece[,] ChessboardCopy { get; set; }
        /// <summary>
        /// Represents the color the AI is playing as.
        /// </summary>
        public PieceColor Color { get; private set; }

        /// <summary>
        /// Provides the weight of the pieces used in the calculation of the moves.
        /// </summary>
        private static Dictionary<Type, int> PieceWeights { get; set; } = new()
        {
            { typeof(Pawn), 1 },
            { typeof(Bishop), 3 },
            { typeof(Knight), 3 },
            { typeof(Rook), 5 },
            { typeof(Queen), 9 }
        };

        /// <summary>
        /// Creates an instance of the <see cref="AI"/> class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="board"></param>
        public AI(PieceColor color, IBoard board)
        {
            Color = color;
            Board = board;

            ChessboardCopy = new Piece[8, 8];

            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    ChessboardCopy[row, column] = Board[row, column];
                }
            }

            Move move = new(1, 2, 3, 4, 5);
        }
    }
}
