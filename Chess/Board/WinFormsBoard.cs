using Chess.Pieces;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Chess.Board
{
    /// <summary>
    /// Implements the <see cref="IBoard"/> interface, is used with a Windows Forms GUI.
    /// </summary>
    class WinFormsBoard : IBoard
    {
        /// <summary>
        /// An array holding the pieces on the chessboard.
        /// </summary>
        private Piece[,] Board { get; set; }
        /// <summary>
        /// Represents all the moves made during the game, may export the data to a text file as standard notation.
        /// </summary>
        public Moves Moves { get; private set; }
        /// <summary>
        /// Represents the <see cref="DataGridView"/> that is used t display the chessboard.
        /// </summary>
        private DataGridView Grid { get; set; }
        /// <summary>
        /// Represents the Form implementing the GUI, is used to access the GUI methods.
        /// </summary>
        private Chess Form { get; set; }
        /// <summary>
        /// Represents all the pieces on the board by color.
        /// </summary>
        public Dictionary<PieceColor, List<Piece>> LivePieces { get; private set; }
        /// <summary>
        /// Represents both kings on the board by color.
        /// </summary>
        public Dictionary<PieceColor, King> Kings { get; private set; }
        /// <summary>
        /// Represents all pieces attacking the king of the opposite color and the paths they attack the king on.
        /// </summary>
        public Dictionary<Piece, List<(int, int)>> LineOfAttack { get; set; }
        /// <summary>
        /// Represents the color that is currently on the move.
        /// </summary>
        public PieceColor NextMove { get; private set; }
        /// <summary>
        /// Represents the <see cref="DataGridView"/> object displaying the chessboard.
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="form"></param>
        public WinFormsBoard(DataGridView dataGridView, Chess form)
        {
            Board = new Piece[8, 8];
            Moves = new();
            Grid = dataGridView;
            Form = form;
            LivePieces = new();
            Kings = new();
            LineOfAttack = new();
            NextMove = PieceColor.White;
            LivePieces.Add(PieceColor.White, new());
            LivePieces.Add(PieceColor.Black, new());

            Board[0, 0] = new Rook(PieceColor.Black, 0, 0, this);
            Board[0, 1] = new Knight(PieceColor.Black, 0, 1, this);
            Board[0, 2] = new Bishop(PieceColor.Black, 0, 2, this);
            Board[0, 3] = new Queen(PieceColor.Black, 0, 3, this);
            Board[0, 4] = new King(PieceColor.Black, 0, 4, this);
            Board[0, 5] = new Bishop(PieceColor.Black, 0, 5, this);
            Board[0, 6] = new Knight(PieceColor.Black, 0, 6, this);
            Board[0, 7] = new Rook(PieceColor.Black, 0, 7, this);

            Board[7, 0] = new Rook(PieceColor.White, 7, 0, this);
            Board[7, 1] = new Knight(PieceColor.White, 7, 1, this);
            Board[7, 2] = new Bishop(PieceColor.White, 7, 2, this);
            Board[7, 3] = new Queen(PieceColor.White, 7, 3, this);
            Board[7, 4] = new King(PieceColor.White, 7, 4, this);
            Board[7, 5] = new Bishop(PieceColor.White, 7, 5, this);
            Board[7, 6] = new Knight(PieceColor.White, 7, 6, this);
            Board[7, 7] = new Rook(PieceColor.White, 7, 7, this);

            Kings.Add(PieceColor.Black, (King)Board[0, 4]);
            Kings.Add(PieceColor.White, (King)Board[7, 4]);
            
            for (int n = 0; n < 8; n++)
            {
                Board[1, n] = new Pawn(PieceColor.Black, 1, n, this);
                Board[6, n] = new Pawn(PieceColor.White, 6, n, this);

                LivePieces[PieceColor.Black].Add(Board[0, n]);
                LivePieces[PieceColor.Black].Add(Board[1, n]);
                LivePieces[PieceColor.White].Add(Board[6, n]);
                LivePieces[PieceColor.White].Add(Board[7, n]);
            }


            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (Board[row, column] is null)
                        continue;

                    Grid[column, row].Value = Board[row, column].UnicodeValue;
                }
            }
        }

        /// <summary>
        /// Evaluates whether the move has ended the game and how - checkmate or draw.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Returns the type of move - a bit flag enum.</returns>
        public MoveType EvaluateMove(MoveType type)
        {
            if (!Kings[NextMove].CheckIsUnderAttack())
            {
                LineOfAttack.Clear();

                if (PlayerHasPossibleMoves())
                    return type;
                else
                    return type | MoveType.Draw;
            }
            else
            {
                if (PlayerHasPossibleMoves())
                    return type | MoveType.Check;
                else
                    return type | MoveType.CheckMate;
            }
        }

        /// <summary>
        /// Chesck whether the player on the move has possible moves.
        /// </summary>
        /// <returns>True if the player on the move has possible moves, otherwise false.</returns>
        private bool PlayerHasPossibleMoves()
        {
            if (Kings[NextMove].PossibleMoves().Count > 0)
                return true;

            if (Kings[NextMove].IsUnderAttack)
            {
                if (LineOfAttack.Count == 1)
                {
                    foreach (Piece piece in LivePieces[NextMove])
                    {
                        foreach (var tuple in piece.PossibleMoves())
                        {
                            if (LineOfAttack.Values.First().Contains(tuple))
                                return true;
                        }
                    }
                }
            }
            else
            {
                foreach (Piece piece in LivePieces[NextMove])
                {
                    if (piece.PossibleMoves().Count > 0)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Calculates all the legal moves for a given piece.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public List<(int, int)> LegalMovesForPiece(Piece piece)
        {
            return piece.PossibleMoves();
        }

        /// <summary>
        /// Calculates all the legal moves for the piece on the given coordinates.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>A list of all possible moves as tuples (row, column).</returns>
        public List<(int, int)> LegalMovesForPiece(int row, int column)
        {
            if (Board[row, column] is null)
                return null;

            return Board[row, column].PossibleMoves();
        }

        /// <summary>
        /// Checks if the given piece is protecting its king.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>True if the piece is protecting its king from being attacked, otherwise false/.</returns>
        public bool IsProtectingKing(Piece piece)
        {
            Board[piece.RowIndex, piece.ColumnIndex] = null;

            bool result = Kings[piece.Color].IsBeingProtected();

            Board[piece.RowIndex, piece.ColumnIndex] = piece;
            
            return result;
        }

        /// <summary>
        /// Checks if the given piece is protecting its king.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>True if the given piece is protecting its king from being attacked, otherwise false.</returns>
        public bool IsProtectingKing(Piece piece, int row, int column)
        {
            bool result;
            Board[piece.RowIndex, piece.ColumnIndex] = null;

            if (Board[row, column] is null)
            {
                Board[row, column] = piece;
                result = Kings[piece.Color].IsBeingProtected();
                Board[row, column] = null;
            }
            else if (Board[row, column].Color != piece.Color)
            {
                Piece placeholder = Board[row, column];
                Board[row, column] = piece;
                result = Kings[piece.Color].IsBeingProtected(placeholder);
                Board[row, column] = placeholder;
            }
            else
                result = false;

            Board[piece.RowIndex, piece.ColumnIndex] = piece;

            return result;
        }

        /// <summary>
        /// Moves the piece passed as argument to specified square and handles the special movements (pawn En Passant, King's Castling).
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void MovePiece(Piece piece, int row, int column)
        {
            int oldRow = piece.RowIndex;
            int oldColumn = piece.ColumnIndex;

            MoveType type = MoveType.Normal;

            if (Board[row, column] is not null)
            {
                LivePieces[Board[row, column].Color].Remove(Board[row, column]);
                type |= MoveType.Capture;
            }

            Form.SetPiecePosition(oldRow, oldColumn, row, column);
            Board[oldRow, oldColumn] = null;
            Board[row, column] = piece;
            piece.Move(row, column);

            if (piece.GetType().Equals(typeof(Pawn)))
            {
                if (PawnEnPassant((Pawn)piece, row, column, oldRow, oldColumn))
                    type |= MoveType.EnPassantCapture;

                if (piece.RowIndex == 0 || piece.RowIndex == 7)
                {
                    if (PawnPromotion((Pawn)piece))
                    {
                        piece = Board[row, column];
                        type |= MoveType.PawnPromotion;
                    }
                }
            }
            else if (piece.GetType().Equals(typeof(King)) && ((King)piece).IsCastling)
            {
                ((King)piece).IsCastling = false;
                type = DoCastling(row, column, type);
            }

            PreventEnPassantInFutureMoves(NextMove);

            NextMove = (NextMove == PieceColor.White ? PieceColor.Black : PieceColor.White);

            type = EvaluateMove(type);

            Moves.Add(new(piece, oldRow, oldColumn, row, column, type));

            if ((type | MoveType.CheckMate) == type)
                Form.Winner(NextMove == PieceColor.White ? PieceColor.Black : PieceColor.White);
            else if ((type | MoveType.Draw) == type)
                Form.Draw();
        }

        /// <summary>
        /// Handles the En Passant movement by Pawn.
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="oldRow"></param>
        /// <param name="oldColumn"></param>
        /// <returns></returns>
        private bool PawnEnPassant(Pawn pawn, int row, int column, int oldRow, int oldColumn)
        {
            if (Math.Abs(oldRow - row) > 1)
                EnableEnPassant(pawn, row, column);
            else if (column - oldColumn == pawn.CanDoEnPassant)
            {
                Piece p = Board[row - (int)pawn.Color, column];

                if (p is not null && p.GetType().Equals(typeof(Pawn)) &&
                    p.Color != pawn.Color)
                {
                    LivePieces[p.Color].Remove(p);
                    Grid[p.ColumnIndex, p.RowIndex].Value = null;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Handles the promotion of pawns when they reach the final row.
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        private bool PawnPromotion(Pawn pawn)
        {
            PromotedPiece promotedPiece = Form.PromotePawn();

            Piece newPiece = promotedPiece switch
            {
                PromotedPiece.Queen => new Queen(pawn.Color, pawn.RowIndex, pawn.ColumnIndex, pawn.Board),
                PromotedPiece.Rook => new Rook(pawn.Color, pawn.RowIndex, pawn.ColumnIndex, pawn.Board),
                PromotedPiece.Bishop => new Bishop(pawn.Color, pawn.RowIndex, pawn.ColumnIndex, pawn.Board),
                PromotedPiece.Knight => new Knight(pawn.Color, pawn.RowIndex, pawn.ColumnIndex, pawn.Board),
                PromotedPiece.None => null,
                _ => null,
            };
            if (newPiece is null)
                return false;

            Board[pawn.RowIndex, pawn.ColumnIndex] = newPiece;
            Grid[pawn.ColumnIndex, pawn.RowIndex].Value = newPiece.UnicodeValue;

            LivePieces[pawn.Color].Remove(pawn);
            LivePieces[pawn.Color].Add(newPiece);
            return true;
        }

        /// <summary>
        /// Triggers the change of bool <see cref="Pawn.CanDoEnPassant"/> to true for the pawns that may take this move.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private void EnableEnPassant(Pawn piece, int row, int column)
        {
            if (column > 0 && Board[row, column - 1] is not null)
                if (Board[row, column - 1].GetType().Equals(typeof(Pawn)) &&
                    Board[row, column - 1].Color != piece.Color)
                {
                    ((Pawn)Board[row, column - 1]).CanDoEnPassant = 1;
                }

            if (column < 7 && Board[row, column + 1] is not null)
                if (Board[row, column + 1].GetType().Equals(typeof(Pawn)) &&
                    Board[row, column + 1].Color != piece.Color)
                {
                    ((Pawn)Board[row, column + 1]).CanDoEnPassant = -1;
                }
        }

        /// <summary>
        /// Handles the Castling move by King and one of its Rooks.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="type"></param>
        /// <returns><see cref="MoveType"/> enum that contains information about what has happened in the given round.</returns>
        private MoveType DoCastling(int row, int column, MoveType type)
        {
            if (column == 2)
            {
                Form.SetPiecePosition(row, 0, row, 3);
                Board[row, 3] = Board[row, 0];
                Board[row, 0].Move(row, 3);
                Board[row, 0] = null;
                return type | MoveType.KingsideCastling;
            }
            else if (column == 6)
            {
                Form.SetPiecePosition(row, 7, row, 5);
                Board[row, 5] = Board[row, 7];
                Board[row, 7].Move(row, 5);
                Board[row, 7] = null;
                return type | MoveType.KingsideCastling;
            }
            return type;
        }

        /// <summary>
        /// Changes the bool <see cref="Pawn.CanDoEnPassant"/> to false at the end of each turn.
        /// </summary>
        /// <param name="color"></param>
        private void PreventEnPassantInFutureMoves(PieceColor color)
        {
            foreach (var p in LivePieces[color])
            {
                if (p.GetType().Equals(typeof(Pawn)))
                    ((Pawn)p).CanDoEnPassant = 0;
            }
        }

        /// <summary>
        /// Returns the piece at the square with the given coordinates. Read-only.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Piece this[int row, int column]
        {
            get => Board[row, column];
        }
    }
}
