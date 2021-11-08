namespace Chess.Application.ChessAIs;


/// <summary>
/// Class representing a chess AI that can play against the user.
/// </summary>
public class ChessAI : IPawnPromoter, IChessAI
{
    /// <summary>
    /// Provides a reference to the <see cref="IBoardController"/> object that is used to run the game.
    /// </summary>
    private IBoardController Controller { get; set; }
    /// <summary>
    /// Own copy of the chessboard used to calculate the moves.
    /// </summary>
    private IReversalEnabledBoard Board { get; set; }
    /// <summary>
    /// Represents the color the AI is playing as.
    /// </summary>
    public PieceColor ColorOfAI { get; private set; }
    /// <summary>
    /// Represents the possible moves in the current situation.
    /// </summary>
    private List<MoveEvaluator> Root { get; set; }
    /// <summary>
    /// Represents how far ahead will the AI calculate the moves.
    /// </summary>
    private int CalculateMovesAhead { get; set; } = 3;

    /// <summary>
    /// Represents the weights of each piece.
    /// </summary>
    private static readonly Dictionary<Type, double> PieceWeights = new()
    {
        { typeof(Pawn), ChessWeights.PieceWeights["Pawn"] },
        { typeof(Bishop), ChessWeights.PieceWeights["Bishop"] },
        { typeof(Knight), ChessWeights.PieceWeights["Knight"] },
        { typeof(Rook), ChessWeights.PieceWeights["Rook"] },
        { typeof(Queen), ChessWeights.PieceWeights["Queen"] },
        { typeof(King), ChessWeights.PieceWeights["King"] }
    };

    /// <summary>
    /// Creates an instance of the <see cref="ChessAI"/> class.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="board"></param>
    public ChessAI(PieceColor color, IBoardController board)
    {
        ColorOfAI = color;
        Controller = board;
        Board = new ReversalEnabledBoard(this);
        Root = new();
        Board.SetUpBoard();
        BuildMovesTree();
    }

    /// <summary>
    /// Makes the next move by the <see cref="ChessAI"/>.
    /// </summary>
    /// <returns></returns>
    public void DoNextMove()
    {
        MoveEvaluator nextMove = GetBestMove();

        Root = nextMove.NextMoves;

        Board.MovePiece(nextMove.FromSquare, nextMove.ToSquare);
        Controller.MovePiece(nextMove.FromSquare, nextMove.ToSquare);

        FillMovesTree();
    }

    /// <summary>
    /// Returns color opposite to the one passed as argument.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static PieceColor OppositeColor(PieceColor color)
    {
        return color == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }

    /// <summary>
    /// Registers a move made by the opponent.
    /// </summary>
    /// <param name="fromSquare"></param>
    /// <param name="toSquare"></param>
    public void RegisterEnemyMove(Square fromSquare, Square toSquare)
    {
        Board.MovePiece(fromSquare, toSquare);
        var newHead = Root.Where(move => move.FromSquare == fromSquare && move.ToSquare == toSquare).ToList();

        if (newHead.Count != 1)
        {
            BuildMovesTree();
            return;
        }

        Root = newHead.First().NextMoves;
        FillMovesTree();
    }

    /// <summary>
    /// Returns the best possible move the AI has identified.
    /// </summary>
    /// <returns></returns>
    private MoveEvaluator GetBestMove()
    {
        return Root.MaxBy(move => move.GetScore(true));
    }

    /// <summary>
    /// Builds the moves tree as a list of <see cref="MoveEvaluator"/> instances.
    /// </summary>
    private void BuildMovesTree()
    {
        Root.Clear();
        BuildMovesTreeLevel(0, Root);
    }

    /// <summary>
    /// Constructs moves tree levels recursively, up to the highest possible depth given by <see cref="CalculateMovesAhead"/>.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="moveEvals"></param>
    private void BuildMovesTreeLevel(int level, List<MoveEvaluator> moveEvals)
    {
        if (level >= CalculateMovesAhead)
            return;

        List<MoveData> moves = Board.LivePieces[Board.NextMove]
            .SelectMany(piece => piece.GetPossibleMoves().Select(toSquare => new MoveData(piece.Square, toSquare)))
            .ToList();

        foreach (var move in moves)
        {
            bool hasMoved = Board[move.FromSquare].HasMoved;
            MoveEvaluator current = ScoreMove(Board[move.FromSquare], move.ToSquare);
            moveEvals.Add(current);

            Piece currentPiece = Board[move.FromSquare];

            Board.MovePiece(currentPiece, move.ToSquare);

            if (IsCheckMate())
                current.SetScore(Board.NextMove == ColorOfAI ? -1_000_000 : 1_000_000);

            BuildMovesTreeLevel(level + 1, current.NextMoves);

            Board.ReverseLastMove();

            if (!hasMoved)
                Board[move.FromSquare].ResetHasMoved();
        }
    }

    /// <summary>
    /// Fills the tree after a move has been made.
    /// </summary>
    private void FillMovesTree()
    {
        FillMovesTreeLevel(0, Root);
    }

    /// <summary>
    /// Fills moves tree levels recursively, up to the highest possible depth given by <see cref="CalculateMovesAhead"/>.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="moveEvals"></param>
    private void FillMovesTreeLevel(int level, List<MoveEvaluator> moveEvals)
    {
        if (level >= CalculateMovesAhead)
            return;

        if (moveEvals.Count == 0)
        {
            BuildMovesTreeLevel(level + 1, moveEvals);
            return;
        }

        foreach (var move in moveEvals)
        {
            Board.MovePiece(move.FromSquare, move.ToSquare);
            FillMovesTreeLevel(level + 1, move.NextMoves);
            Board.ReverseLastMove();
        }
    }

    /// <summary>
    /// Evaluates a given move based on weights associated with the square, and type of piece currently occupying it, if any.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="square"></param>
    /// <returns></returns>
    private MoveEvaluator ScoreMove(Piece piece, Square square)
    {
        double colorMultiplier = piece.Color == ColorOfAI ? 1 : -1;

        // if the score is super bad, return null and stop calculating subsequent moves?
        // this would however exclude moves with short-term loss but bigger long-term gain
        // TODO add some metrics, and decide based on frequency of low score moves being chosen whether to calculate moves selectively

        double score = MaterialDifference(square);

        // TODO add remaining criteria - king security, queen development, center control, possibly other pieces' development

        return new(piece.Square, square, score * colorMultiplier);
    }

    /// <summary>
    /// Calculates the difference in material as <see cref="double"/>.
    /// </summary>
    private double MaterialDifference(Square square)
    {
        double SumOfPieceValuesForColor(PieceColor color, Square square) =>
            Board.LivePieces[color]
                .Where(piece => piece.Square != square)
                .Sum(piece => PieceWeights[piece.GetType()]);

        return SumOfPieceValuesForColor(ColorOfAI, square)
            - SumOfPieceValuesForColor(OppositeColor(ColorOfAI), square);
    }

    /// <summary>
    /// The move results in check mate - opponent's king is under attack and no piece can move in to protect it.
    /// </summary>
    /// <returns></returns>
    private bool IsCheckMate()
    {
        return Board.Winner is not null && Board.Winner != GameResultEnum.Draw;
    }

    /// <summary>
    /// Resolves which piece should a pawn be promoted into.
    /// </summary>
    /// <returns></returns>
    public PromotedPiece GetPromotedPiece()
    {
        // TODO add logic for selecting which piece should a pawn be promoted into
        return PromotedPiece.Queen;
    }

    /// <summary>
    /// Sets up a board with a non-standard piece layout, e.g. a specific positions. Copies the given <see cref="IBoard"/> instance's piece positions.
    /// </summary>
    public void SetUpNonstandardBoard(IBoard board)
    {
        Board.SetUpNonstandardBoard(board);
        BuildMovesTree();
    }
}
