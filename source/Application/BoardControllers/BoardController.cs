using Chess.Application.Pieces;
using System.Collections.Generic;
using Chess.Application.Enums;
using Chess.Application.Records;
using Chess.Application.Boards;
using Chess.Application.ChessAIs;

namespace Chess.Application.BoardControllers
{
    /// <summary>
    /// Implements the <see cref="IBoardController"/> interface, is used with a Windows Forms GUI.
    /// </summary>
    public class BoardController : IBoardController, IPawnPromoter
    {
        /// <summary>
        /// Represents the chessboard.
        /// </summary>
        public IBoard Board { get; set; }
        /// <summary>
        /// Represents the color that is currently on the move.
        /// </summary>
        public PieceColor NextMove { get => Board.NextMove; }
        /// <summary>
        /// Represents all the moves made during the game, may export the data to a text file as standard notation.
        /// </summary>
        public IMoveRecordExporter Moves { get; private set; }
        /// <summary>
        /// Represents the Form implementing the GUI, is used to access the GUI methods.
        /// </summary>
        private IChessboardInterface Form { get; set; }
        /// <summary>
        /// Represents the color for which is the AI playing.
        /// </summary>
        private PieceColor? AIColor { get; } = null;
        private IChessAI ChessAI { get; } = null;

        /// <summary>
        /// Creates an instance of <see cref="BoardController"/>.
        /// </summary>
        /// <param name="form"></param>
        public BoardController(IChessboardInterface form)
        {
            Moves = new MoveRecordExporter();
            Form = form;
            Board = new Board(this);
            Board.SetUpBoard();
        }

        /// <summary>
        /// Creates an instance of <see cref="BoardController"/> with AI playing as <paramref name="color"/>.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="color"></param>
        public BoardController(IChessboardInterface form, PieceColor color)
        {
            Moves = new MoveRecordExporter();
            Form = form;
            Board = new Board(this);
            AIColor = color;
            ChessAI = new ChessAI(color, this);
            Board.SetUpBoard();
        }

        /// <summary>
        /// Calculates all the legal moves for a given piece.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>A list of all possible moves as Square.</returns>
        public List<Square> ValidMovesForPiece(Piece piece)
        {
            return piece.GetPossibleMoves();
        }

        /// <summary>
        /// Calculates all the legal moves for the piece on the given coordinates.
        /// </summary>
        /// <param name="square"></param>
        /// <returns>A list of all possible moves as Square.</returns>
        public List<Square> ValidMovesForPiece(Square square)
        {
            if (Board[square] is null)
                return null;

            return Board[square].GetPossibleMoves();
        }

        /// <summary>
        /// Moves the piece passed as argument to specified square and handles the special movements (pawn En Passant, King's Castling).
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="square"></param>
        public void MovePiece(Piece piece, Square square)
        {
            Square originalPosition = piece.Square;

            Form.SetPiecePosition(piece.Square, square);

            MoveType moveType = Board.MovePiece(piece, square, Form.SetPiecePosition);
            UpdateChessboardAfterSpecialMoves(piece, moveType);

            Moves.Add(new(piece, originalPosition, square, moveType));

            if (Board.Winner is not null)
            {
                Form.EndOfGame((GameResultEnum)Board.Winner);
                return;
            }

            if (ChessAI is not null && piece.Color != AIColor)
            {
                ChessAI.RegisterEnemyMove(originalPosition, square);
                ChessAI.DoNextMove();
            }
        }

        /// <summary>
        /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
        /// </summary>
        /// <param name="fromSquare"></param>
        /// <param name="toSquare"></param>
        public void MovePiece(Square fromSquare, Square toSquare)
        {
            MovePiece(Board[fromSquare], toSquare);
        }

        private void UpdateChessboardAfterSpecialMoves(Piece piece, MoveType moveType)
        {
            if ((moveType | MoveType.EnPassantCapture) == moveType)
                Form.RemovePiece((piece.Square.Row - (int)piece.Color, piece.Square.Column));
        }

        /// <summary>
        /// Resolves which piece should a pawn be promoted into.
        /// </summary>
        /// <returns></returns>
        public PromotedPiece GetPromotedPiece()
        {
            return Form.PromotePawn();
        }

        /// <summary>
        /// Signals to the controller that the game may start. Only has effect if an AI is playing as white.
        /// </summary>
        public void StartGame()
        {
            if (ChessAI.ColorOfAI == PieceColor.White)
                ChessAI.DoNextMove();
        }

        /// <summary>
        /// Returns the piece at the square with the given coordinates. Read-only.
        /// </summary>
        /// <param name="square"></param>
        /// <returns>The piece on the given coordinates.</returns>
        public Piece this[Square square]
        {
            get => Board[square];
        }
    }
}
