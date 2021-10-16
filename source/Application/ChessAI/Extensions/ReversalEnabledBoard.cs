using Chess.Application.BoardControllers;
using Chess.Application.Boards;
using Chess.Application.Enums;
using Chess.Application.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chess.Application.ChessAIs.Extensions
{
    /// <summary>
    /// Extends the <see cref="Board"/> with the ability to reverse a given move.
    /// </summary>
    internal class ReversalEnabledBoard : Board, IReversalEnabledBoard
    {
        /// <summary>
        /// Creates an instance of <see cref="ReversalEnabledBoard"/> class.
        /// </summary>
        /// <param name="pawnPromoter"></param>
        internal ReversalEnabledBoard(IPawnPromoter pawnPromoter) : base(pawnPromoter)
        {
            MovesStack = new();
        }

        /// <summary>
        /// Stores recent moves in a stack, each moves is added on top of the stack,
        /// with each reversal the topmost one is removed and stored in the <see cref="Board.RecentMoves"/>.
        /// </summary>
        private protected Stack<(MoveData MoveData, Piece Piece, bool HasMoved, Piece BackupPiece, MoveData? PreviousMove)> MovesStack { get; set; }


        /// <summary>
        /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
        /// </summary>
        /// <param name="fromSquare"></param>
        /// <param name="toSquare"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public new MoveType MovePiece(Square fromSquare, Square toSquare)
        {
            return MovePiece(this[fromSquare], toSquare, null);
        }

        /// <summary>
        /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
        /// </summary>
        /// <param name="fromSquare"></param>
        /// <param name="toSquare"></param>
        /// <param name="moveSpecial"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public new MoveType MovePiece(Square fromSquare, Square toSquare, Action<Square, Square> moveSpecial)
        {
            return MovePiece(this[fromSquare], toSquare, moveSpecial);
        }

        /// <summary>
        /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="square"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public new MoveType MovePiece(Piece piece, Square square)
        {
            return MovePiece(piece, square, null);
        }

        /// <summary>
        /// Moves the piece on the board, removes the piece of opposing color if it is on the square. Calls the <paramref name="moveSpecial"/> delegate.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="square"></param>
        /// <param name="moveSpecial"></param>
        /// <returns></returns>
        public new MoveType MovePiece(Piece piece, Square square, Action<Square, Square> moveSpecial)
        {
            MoveData moveData = (piece.Square, square);
            Piece backup = GetBackup(moveData);
            MovesStack.Push((moveData, piece, piece.HasMoved, backup, RecentMoves[piece.Color]));

            return base.MovePiece(piece, square, moveSpecial);
        }

        /// <summary>
        /// Returns piece that will be captured during the current move.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private Piece GetBackup(MoveData move)
        {
            if (IsPawnDoingEnPassantCapture(move))
            {
                return this[move.ToSquare + (-(int)this[move.FromSquare].Color, 0)];
            }

            return this[move.ToSquare];
        }

        /// <summary>
        /// Checks if the current piece is a <see cref="Pawn"/> and is doing an en passant capture.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private bool IsPawnDoingEnPassantCapture(MoveData move)
        {
            if (this[move.FromSquare] is null || !this[move.FromSquare].GetType().Equals(typeof(Pawn)))
                return false;

            int canDoEnPassant = CanDoEnPassant((Pawn)this[move.FromSquare]);

            if (canDoEnPassant == 0)
                return false;

            return canDoEnPassant + ((Pawn)this[move.FromSquare]).Square.Column == move.ToSquare.Column;
        }

        /// <summary>
        /// Reverses the most recent move.
        /// </summary>
        public void ReverseLastMove()
        {
            if (MovesStack.Count == 0)
                return;

            var (moveData, piece, hasMoved, backupPiece, recentMove) = MovesStack.Pop();

            if (piece != this[moveData.ToSquare])
            {
                ReversePawnPromotion(moveData, piece);
            }

            MovePieceRaw(piece, moveData.FromSquare);

            if (!hasMoved)
                piece.ResetHasMoved();

            if (IsCastlingKing(moveData.FromSquare, moveData.ToSquare))
            {
                ReverseCastling(moveData.ToSquare);
            }

            ReinstateCapturedPiece(moveData, backupPiece);

            SwitchNextMove();

            RecentMoves[piece.Color] = recentMove;
        }

        private void ReinstateCapturedPiece(MoveData moveData, Piece backupPiece)
        {
            if (backupPiece is null)
            {
                this[moveData.ToSquare] = null;
                return;
            }

            this[backupPiece.Square] = backupPiece;
            LivePieces[backupPiece.Color].Add(backupPiece);
        }

        /// <summary>
        /// Reverses pawn promotion - removes promoted piece, reinstates pawn.
        /// Throws <see cref="InvalidOperationException"/> if <paramref name="piece"/> is not <see cref="Pawn"/>.
        /// </summary>
        /// <param name="moveData"></param>
        /// <param name="piece"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void ReversePawnPromotion(MoveData moveData, Piece piece)
        {
            if (!piece.GetType().Equals(typeof(Pawn)))
                throw new InvalidOperationException($"Piece of type {typeof(Pawn).Name} was expected, got {piece.GetType().Name} instead!");

            LivePieces[piece.Color].Remove(this[moveData.ToSquare]);
            LivePieces[piece.Color].Add(piece);
        }

        /// <summary>
        /// Moves appropriate rook back to its original square when reversing a castling move.
        /// </summary>
        /// <param name="toSquare"></param>
        private void ReverseCastling(Square toSquare)
        {
            if (toSquare.Column == 6)
            {
                Piece rook = this[(toSquare.Row, 5)];
                MovePieceRaw(rook, (toSquare.Row, 7));
                rook.ResetHasMoved();
            }
            else if (toSquare.Column == 2)
            {
                Piece rook = this[(toSquare.Row, 3)];
                MovePieceRaw(rook, (toSquare.Row, 0));
                rook.ResetHasMoved();
            }
        }

        /// <summary>
        /// Checks if the current piece is a <see cref="King"/> and is castling.
        /// </summary>
        /// <param name="fromSquare"></param>
        /// <param name="toSquare"></param>
        /// <returns></returns>
        private bool IsCastlingKing(Square fromSquare, Square toSquare)
        {
            return this[fromSquare].GetType().Equals(typeof(King))
                && Math.Abs(fromSquare.Column - toSquare.Column) > 1;
        }

        /// <summary>
        /// Mirrors a given <see cref="IBoard"/> piece positions.
        /// </summary>
        /// <param name="board"></param>
        public void SetUpNonstandardBoard(IBoard board)
        {
            var squares = Enumerable.Range(0, 8).SelectMany(x => Enumerable.Range(0, 8).Select(y => new Square(x, y)));
            LivePieces[PieceColor.Black].Clear();
            LivePieces[PieceColor.White].Clear();
            Kings[PieceColor.White] = null;
            Kings[PieceColor.Black] = null;
            RecentMoves[PieceColor.White] = null;
            RecentMoves[PieceColor.Black] = null;

            foreach (var square in squares)
            {
                ChessBoard[square.Row, square.Column] = null;
                
                if (board[square] is null)
                {
                    continue;
                }

                InjectPiece(board[square], square);
            }

            if (Kings[PieceColor.White] is null || Kings[PieceColor.Black] is null)
                throw new InvalidOperationException("There must be a king on both sides!");
        }

        /// <summary>
        /// Adds a piece of the same type as <paramref name="piece"/> to the given square.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="square"></param>
        private void InjectPiece(Piece piece, Square square)
        {
            Piece newPiece = GetPiece(piece, square);
            
            if (piece.GetType().Equals(typeof(King)))
            {
                ThrowIfKingIsAlreadyAssigned(piece);
                Kings[piece.Color] = (King)newPiece;
            }

            FieldInfo fieldInfo = typeof(Piece).GetField($"<{nameof(Piece.HasMoved)}>k__BackingField",
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            fieldInfo.SetValue(newPiece, piece.HasMoved);
            LivePieces[piece.Color].Add(newPiece);
            ChessBoard[square.Row, square.Column] = newPiece;
        }

        /// <summary>
        /// Makes sure that no more than one <see cref="King"/> is present on the chessboard.
        /// </summary>
        /// <param name="piece"></param>
        private void ThrowIfKingIsAlreadyAssigned(Piece piece)
        {
            if (Kings[piece.Color] is not null)
                throw new InvalidOperationException("There may not be more than one King!");
        }

        /// <summary>
        /// Returns a new instance of the same class as the given <paramref name="piece"/>.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="square"></param>
        /// <returns></returns>
        private Piece GetPiece(Piece piece, Square square)
        {
            return piece.GetType().Name switch
            {
                nameof(King) => new King(piece.Color, square, this),
                nameof(Pawn) => new Pawn(piece.Color, square, this),
                nameof(Queen) => new Queen(piece.Color, square, this),
                nameof(Bishop) => new Bishop(piece.Color, square, this),
                nameof(Rook) => new Rook(piece.Color, square, this),
                nameof(Knight) => new Knight(piece.Color, square, this),
                _ => throw new ArgumentException("Unknown piece type", piece.GetType().Name)
            };
        }
    }
}
