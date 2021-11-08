namespace Chess.Application.Boards;


/// <summary>
/// Represents the Chessboard. Implements <see cref="IBoard"/>.
/// </summary>
public class Board : IBoard
{
    /// <summary>
    /// Returns the piece at the square with the given coordinates. Read-only.
    /// </summary>
    /// <param name="square"></param>
    /// <returns>The piece on the given coordinates.</returns>
    public Piece this[Square square]
    {
        get => ChessBoard[square.Row, square.Column];
        protected set => ChessBoard[square.Row, square.Column] = value;
    }

    /// <summary>
    /// Keeps a reference for all pieces attacking the king of the opposite color and the paths they attack the king on.
    /// </summary>
    public Dictionary<Piece, List<Square>> LineOfAttack { get; set; }

    /// <summary>
    /// Keeps a reference for all the pieces on the board by color.
    /// </summary>
    public Dictionary<PieceColor, List<Piece>> LivePieces { get; }

    /// <summary>
    /// Keeps a reference for both kings on the board by color.
    /// </summary>
    public Dictionary<PieceColor, King> Kings { get; }

    /// <summary>
    /// An array holding the pieces on the chessboard.
    /// </summary>
    protected Piece[,] ChessBoard { get; set; }

    /// <summary>
    /// Represents the color that is currently on the move.
    /// </summary>
    public PieceColor NextMove { get; protected set; }

    /// <summary>
    /// Represents the winner of the game.
    /// </summary>
    public GameResultEnum? Winner { get; private set; }

    /// <summary>
    /// Provides a method for resolving which piece should a pawn be promoted into.
    /// </summary>
    protected IPawnPromoter PawnPromoter { get; }

    /// <summary>
    /// Stores the most recent moves for each side.
    /// </summary>
    private protected Dictionary<PieceColor, MoveData?> RecentMoves { get; set; }

    /// <summary>
    /// Creates an instance of <see cref="Board"/>.
    /// </summary>
    public Board(IPawnPromoter pawnPromoter)
    {
        ChessBoard = new Piece[8, 8];
        LineOfAttack = new();
        LivePieces = new();
        Kings = new();
        PawnPromoter = pawnPromoter;
        NextMove = PieceColor.White;
        Winner = null;
        RecentMoves = new();
        ResetRecentMoves();
    }

    /// <summary>
    /// Sets <see cref="RecentMoves"/> to null for each color.
    /// </summary>
    private void ResetRecentMoves()
    {
        RecentMoves[PieceColor.White] = null;
        RecentMoves[PieceColor.Black] = null;
    }

    /// <summary>
    /// Sets up the chess board.
    /// </summary>
    public void SetUpBoard()
    {
        LivePieces.Clear();
        Kings.Clear();
        ResetRecentMoves();

        NextMove = PieceColor.White;
        for (int row = 0; row < 8; row++)
            for (int column = 0; column < 8; column++)
                ChessBoard[row, column] = null;

        LivePieces.Add(PieceColor.White, new());
        LivePieces.Add(PieceColor.Black, new());

        ChessBoard[0, 0] = new Rook(PieceColor.Black, (0, 0), this);
        ChessBoard[0, 1] = new Knight(PieceColor.Black, (0, 1), this);
        ChessBoard[0, 2] = new Bishop(PieceColor.Black, (0, 2), this);
        ChessBoard[0, 3] = new Queen(PieceColor.Black, (0, 3), this);
        ChessBoard[0, 4] = new King(PieceColor.Black, (0, 4), this);
        ChessBoard[0, 5] = new Bishop(PieceColor.Black, (0, 5), this);
        ChessBoard[0, 6] = new Knight(PieceColor.Black, (0, 6), this);
        ChessBoard[0, 7] = new Rook(PieceColor.Black, (0, 7), this);

        ChessBoard[7, 0] = new Rook(PieceColor.White, (7, 0), this);
        ChessBoard[7, 1] = new Knight(PieceColor.White, (7, 1), this);
        ChessBoard[7, 2] = new Bishop(PieceColor.White, (7, 2), this);
        ChessBoard[7, 3] = new Queen(PieceColor.White, (7, 3), this);
        ChessBoard[7, 4] = new King(PieceColor.White, (7, 4), this);
        ChessBoard[7, 5] = new Bishop(PieceColor.White, (7, 5), this);
        ChessBoard[7, 6] = new Knight(PieceColor.White, (7, 6), this);
        ChessBoard[7, 7] = new Rook(PieceColor.White, (7, 7), this);

        Kings.Add(PieceColor.Black, (King)ChessBoard[0, 4]);
        Kings.Add(PieceColor.White, (King)ChessBoard[7, 4]);

        for (int column = 0; column < 8; column++)
        {
            ChessBoard[1, column] = new Pawn(PieceColor.Black, (1, column), this);
            ChessBoard[6, column] = new Pawn(PieceColor.White, (6, column), this);

            LivePieces[PieceColor.Black].Add(ChessBoard[0, column]);
            LivePieces[PieceColor.Black].Add(ChessBoard[1, column]);
            LivePieces[PieceColor.White].Add(ChessBoard[6, column]);
            LivePieces[PieceColor.White].Add(ChessBoard[7, column]);
        }
    }

    /// <summary>
    /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
    /// </summary>
    /// <param name="fromSquare"></param>
    /// <param name="toSquare"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public MoveType MovePiece(Square fromSquare, Square toSquare)
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
    public MoveType MovePiece(Square fromSquare, Square toSquare, Action<Square, Square> moveSpecial)
    {
        return MovePiece(this[fromSquare], toSquare, moveSpecial);
    }

    /// <summary>
    /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public MoveType MovePiece(Piece piece, Square square)
    {
        return MovePiece(piece, square, null);
    }

    /// <summary>
    /// Moves the piece on the board, removes the piece of opposing color if it is on the square.
    /// Calls the <paramref name="moveSpecial"/> delegate for <see cref="King"/> castling.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    /// <param name="moveSpecial"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public MoveType MovePiece(Piece piece, Square square, Action<Square, Square> moveSpecial)
    {
        if (piece.Color != NextMove)
            throw new InvalidOperationException($"Unable to move piece, it's not {piece.Color} move yet! {piece} to {square}");

        RecentMoves[piece.Color] = (piece.Square, square);

        MoveType moveType = HandleAdditionalEvents(piece, square, MoveType.Normal, moveSpecial);

        if ((moveType | MoveType.PawnPromotion) != moveType)
            MovePieceRaw(piece, square);

        if (IsKingUnderAttack())
        {
            throw new InvalidOperationException($"Piece {piece} can't be moved to {square} because it leaves the king unprotected!");
        }

        SwitchNextMove();

        moveType = EvaluateMove(moveType);
        CheckWinner(piece.Color, moveType);

        ClearPossibleMoves();
        return moveType;
    }

    /// <summary>
    /// Handles the internals of moving the piece - relocating on the chess board, pawn- or king-specific moves.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    /// <param name="moveType"></param>
    /// <param name="moveSpecial"></param>
    /// <returns></returns>
    private MoveType HandleAdditionalEvents(Piece piece, Square square, MoveType moveType, Action<Square, Square> moveSpecial)
    {
        if (this[square] is not null)
        {
            LivePieces[this[square].Color].Remove(this[square]);
            moveType |= MoveType.Capture;
        }

        if (piece.GetType().Equals(typeof(Pawn)))
            moveType = DoPawnSpecialMoves((Pawn)piece, square, moveType);

        if (!piece.HasMoved && piece.GetType().Equals(typeof(King)))
            moveType = DoKingSpecialMoves((King)piece, square, moveType, moveSpecial);

        return moveType;
    }

    /// <summary>
    /// Switches the color representing the player on the move.
    /// </summary>
    protected internal void SwitchNextMove()
    {
        NextMove = OppositeColor(NextMove);
    }

    private static PieceColor OppositeColor(PieceColor color)
    {
        return color == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }

    private void ClearPossibleMoves()
    {
        LivePieces.Values.ToList().ForEach(collection => collection.ForEach(piece => piece.PossibleMoves.Clear()));
    }

    /// <summary>
    /// Checks whether the given pawn can do en passant capture.
    /// </summary>
    /// <param name="pawn"></param>
    /// <returns>Column offset determining to which side can the move be performed, 0 if it can't.</returns>
    public int CanDoEnPassant(Pawn pawn)
    {
        var oppositeColor = OppositeColor(pawn.Color);

        if (RecentMoves[oppositeColor] is null)
            return 0;

        MoveData moveData = (MoveData)RecentMoves[oppositeColor];

        if (this[moveData.ToSquare] is null || !this[moveData.ToSquare].GetType().Equals(typeof(Pawn)))
            return 0;

        if (!HasMovedTwoRowsAndIsAdjacentSquare(pawn, moveData))
            return 0;

        return moveData.FromSquare.Column - pawn.Square.Column;
    }

    private static bool HasMovedTwoRowsAndIsAdjacentSquare(Pawn pawn, MoveData moveData)
    {
        return Math.Abs(moveData.FromSquare.Row - moveData.ToSquare.Row) == 2 && Math.Abs(moveData.FromSquare.Column - pawn.Square.Column) == 1 && moveData.ToSquare.Row == pawn.Square.Row;
    }

    /// <summary>
    /// Evaluates current move and changes the associated <see cref="MoveType"/> value.
    /// </summary>
    /// <param name="moveType"></param>
    /// <returns></returns>
    private MoveType EvaluateMove(MoveType moveType)
    {
        if (IsKingUnderAttack())
        {
            if (PlayerHasPossibleMoves())
                moveType |= MoveType.Check;
            else
                moveType |= MoveType.CheckMate;
        }
        else
        {
            if (!PlayerHasPossibleMoves())
                moveType |= MoveType.Draw;
        }

        return moveType;
    }

    private void CheckWinner(PieceColor color, MoveType moveType)
    {
        if ((moveType | MoveType.CheckMate) == moveType)
            Winner = color == PieceColor.White ? GameResultEnum.White : GameResultEnum.Black;
        if ((moveType | MoveType.Draw) == moveType)
            Winner = GameResultEnum.Draw;
    }

    private bool IsKingUnderAttack()
    {
        if (Kings[NextMove].CheckIsUnderAttack())
        {
            return true;
        }

        LineOfAttack.Clear();
        return false;
    }

    private bool PlayerHasPossibleMoves()
    {
        if (LivePieces[NextMove].Where(piece => piece.PossibleMoves.Count > 0).Any())
            return true;

        if (Kings[NextMove].GetPossibleMoves().Count > 0)
            return true;

        if (Kings[NextMove].IsUnderAttack)
        {
            return AtLeastOnePieceCanProtectKing();
        }
        else
        {
            foreach (Piece piece in LivePieces[NextMove])
            {
                if (piece.GetPossibleMoves().Count > 0)
                    return true;
            }
        }
        return false;
    }

    private bool AtLeastOnePieceCanProtectKing()
    {
        if (LineOfAttack.Count > 1)
            return false;

        var moves = LivePieces[NextMove].Select(piece => piece.GetPossibleMoves()).SelectMany(moves => moves);
        foreach (var move in moves)
        {
            if (LineOfAttack.Values.First().Contains(move))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Moves a piece onto the new square without doing any checks.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    private protected void MovePieceRaw(Piece piece, Square square)
    {
        this[piece.Square] = null;
        this[square] = piece;
        piece.Move(square);
    }

    /// <summary>
    /// Handles special king movements.
    /// </summary>
    /// <param name="king"></param>
    /// <param name="square"></param>
    /// <param name="moveType"></param>
    /// <param name="moveSpecial"></param>
    /// <returns></returns>
    private MoveType DoKingSpecialMoves(King king, Square square, MoveType moveType, Action<Square, Square> moveSpecial)
    {
        if (Math.Abs(king.Square.Column - square.Column) > 1)
            moveType |= KingCastlingMoveRook(square, moveType, moveSpecial);

        return moveType;
    }

    /// <summary>
    /// Handles the rook movement during king's castling move.
    /// </summary>
    /// <param name="square"></param>
    /// <param name="moveType"></param>
    /// <param name="moveSpecial"></param>
    /// <returns></returns>
    private MoveType KingCastlingMoveRook(Square square, MoveType moveType, Action<Square, Square> moveSpecial)
    {
        if (square.Column == 2)
        {
            if (moveSpecial is not null)
                moveSpecial((square.Row, 0), (square.Row, 3));
            MovePieceRaw(ChessBoard[square.Row, 0], (square.Row, 3));
            return moveType | MoveType.QueensideCastling;
        }
        else if (square.Column == 6)
        {
            if (moveSpecial is not null)
                moveSpecial((square.Row, 7), (square.Row, 5));
            MovePieceRaw(ChessBoard[square.Row, 7], (square.Row, 5));
            return moveType | MoveType.KingsideCastling;
        }
        return moveType;
    }

    /// <summary>
    /// Handles special pawn movements.
    /// </summary>
    /// <param name="pawn"></param>
    /// <param name="square"></param>
    /// <param name="moveType"></param>
    /// <returns></returns>
    private MoveType DoPawnSpecialMoves(Pawn pawn, Square square, MoveType moveType)
    {
        if (square.Row == 0 || square.Row == 7)
        {
            moveType = PromotePawn(pawn, square, moveType);
        }
        else if (square.Column != pawn.Square.Column)
        {
            moveType = DoPawnCapture(pawn, square, moveType);
        }

        return moveType;
    }

    /// <summary>
    /// Handles standard capture and en passant capture by the <see cref="Pawn"/>.
    /// </summary>
    /// <param name="pawn"></param>
    /// <param name="square"></param>
    /// <param name="moveType"></param>
    /// <returns></returns>
    private MoveType DoPawnCapture(Pawn pawn, Square square, MoveType moveType)
    {
        if (this[square] is null)
        {
            Pawn otherPawn = (Pawn)this[square + (-(int)pawn.Color, 0)];

            LivePieces[otherPawn.Color].Remove(otherPawn);
            this[otherPawn.Square] = null;

            return moveType | MoveType.EnPassantCapture;
        }

        Piece other = this[square];
        LivePieces[other.Color].Remove(other);

        return moveType | MoveType.Capture;
    }

    /// <summary>
    /// Handles pawn promotion logic.
    /// </summary>
    /// <param name="pawn"></param>
    /// <param name="square"></param>
    /// <param name="moveType"></param>
    /// <exception cref="ArgumentException"></exception>
    private MoveType PromotePawn(Pawn pawn, Square square, MoveType moveType)
    {
        PromotedPiece promotedPiece = PawnPromoter.GetPromotedPiece();

        Piece newPiece = promotedPiece switch
        {
            PromotedPiece.Queen => new Queen(pawn.Color, square, pawn.Board),
            PromotedPiece.Rook => new Rook(pawn.Color, square, pawn.Board),
            PromotedPiece.Bishop => new Bishop(pawn.Color, square, pawn.Board),
            PromotedPiece.Knight => new Knight(pawn.Color, square, pawn.Board),
            _ => throw new InvalidPieceException($"{nameof(PromotedPiece)} is invalid!")
        };

        LivePieces[pawn.Color].Remove(pawn);
        LivePieces[pawn.Color].Add(newPiece);
        this[pawn.Square] = null;
        this[square] = newPiece;

        return moveType | MoveType.PawnPromotion;
    }

    /// <summary>
    /// Checks if the given piece is protecting its king against a single attacker and returns the path it would take.
    /// Returns false if there is none, or more than one attacker.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="squares"></param>
    /// <returns>True if the piece is protecting its king, otherwise false.</returns>
    public bool IsProtectingKing(Piece piece, out List<Square> squares)
    {
        King king = Kings[piece.Color];
        squares = new();

        this[piece.Square] = null;
        PieceColor oppositeColor = piece.Color == PieceColor.White ? PieceColor.Black : PieceColor.White;

        var threateningKing = LivePieces[oppositeColor].Where(piece => piece.IsThreateningSquare(king.Square)).ToList();
        this[piece.Square] = piece;

        if (threateningKing.Count == 0)
            return false;
        else if (threateningKing.Count > 1)
        {
            piece.PossibleMoves.Clear();
            return false;
        }

        var threat = threateningKing.First();

        squares.AddRange((king.Square - threat.Square).GetSquares());
        squares.Add(threat.Square);

        return true;
    }

    /// <summary>
    /// Calculates all the legal moves for the piece on the given coordinates.
    /// </summary>
    /// <param name="square"></param>
    /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
    public List<Square> ValidMovesForPiece(Square square)
    {
        return ValidMovesForPiece(this[square]);
    }

    /// <summary>
    /// Calculates all the legal moves for a given piece.
    /// </summary>
    /// <param name="piece"></param>
    /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
    public List<Square> ValidMovesForPiece(Piece piece)
    {
        if (Kings[piece.Color].IsUnderAttack)
        {
            if (LineOfAttack.Count > 1)
            {
                piece.PossibleMoves.Clear();
                return piece.PossibleMoves;
            }

            List<Square> possibleMoves = piece.GetPossibleMoves().Where(move => LineOfAttack.Values.First().Contains(move)).ToList();
            return possibleMoves;
        }
        //else if (IsProtectingKing(piece))
        //{
        //    piece.PossibleMoves.Clear();
        //    return piece.PossibleMoves;
        //}
        return piece.GetPossibleMoves();
    }

    /// <summary>
    /// Checks whether the given square is being attacked.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    /// <returns></returns>
    public bool IsSquareUnderAttack(Piece piece, Square square)
    {
        return IsSquareUnderAttack(piece.Color, square);
    }

    /// <summary>
    /// Checks whether the given square is being attacked.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="square"></param>
    /// <returns></returns>
    public bool IsSquareUnderAttack(PieceColor color, Square square)
    {
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

        int counter = 0;
        var potentiallyAttacking = LivePieces[oppositeColor]
            .Where(piece => !piece.GetType().Equals(typeof(King)))
            .Where(piece => piece.IsThreateningSquare(square));

        if (this[square] is not null && this[square].GetType().Equals(typeof(King)))
        {
            foreach (var piece in potentiallyAttacking)
            {
                AddToLineOfAttack(square, piece);
                counter++;
            }
        }

        return counter > 0 || (Kings[oppositeColor].Square - square).SquaresAreAdjacent() || potentiallyAttacking.Any();
    }

    /// <summary>
    /// Adds squares between <paramref name="piece"/> and <paramref name="square"/> to <see cref="LineOfAttack"/>.
    /// </summary>
    /// <param name="square"></param>
    /// <param name="piece"></param>
    private void AddToLineOfAttack(Square square, Piece piece)
    {
        var moves = (piece.Square - square).GetSquares();
        moves.Add(piece.Square);

        if (LineOfAttack.ContainsKey(piece))
        {
            LineOfAttack[piece].AddRange(moves);
            return;
        }
        LineOfAttack.Add(piece, moves);
    }

    /// <summary>
    /// Checks whether the king of the given color is under attack.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool IsKingUnderAttack(PieceColor color)
    {
        King king = Kings[color];
        return IsSquareUnderAttack(king, king.Square);
    }
}
