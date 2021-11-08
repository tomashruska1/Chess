namespace Chess.Application.Pieces;


/// <summary>
/// Class that represents the Pawn pieces on the chessboard. Provides movement typical for Pawns.
/// </summary>
public class Pawn : Piece
{
    /// <summary>
    /// Creates an instance of the Pawn class.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="square"></param>
    /// <param name="board"></param>
    public Pawn(PieceColor color, Square square, IBoard board)
    {
        Color = color;
        Square = square;
        Board = board;
        PossibleMoves = new();

        if (color == PieceColor.White)
            UnicodeValue = '\u2659';
        else
            UnicodeValue = '\u265F';
    }

    /// <summary>
    /// Override method, calculates all possible moves for a given piece.
    /// </summary>
    /// <returns>A list of all possible moves as <see cref="Square"/>.</returns>
    public override List<Square> GetPossibleMoves()
    {
        if (PossibleMoves.Count > 0)
            return PossibleMoves;

        int offset = (int)Color;

        if (Square.IsValid(Square.Row + offset, Square.Column))
        {
            Square target = new(Square.Row + offset, Square.Column);

            if (Board[target] is null)
            {
                CanMoveToSquare(target, CaptureMode.NoCapture);
                if (!HasMoved)
                    CanMoveToSquare(target + (offset, 0), CaptureMode.NoCapture);
            }

            int canDoEnPassant = Board.CanDoEnPassant(this);

            CheckCanMoveToTheSide(target, 1, canDoEnPassant);
            CheckCanMoveToTheSide(target, -1, canDoEnPassant);
        }

        RemoveInvalidMoves();
        return PossibleMoves;
    }

    private void CheckCanMoveToTheSide(Square square, int offset, int canDoEnPassant)
    {
        if (!Square.IsValid(square.Row, square.Column + offset))
            return;

        if (canDoEnPassant == offset)
            CanMoveToSquare(square + (0, offset));
        else
            CanMoveToSquare(square + (0, offset), CaptureMode.CaptureOnly);

    }

    /// <summary>
    /// Checks whether the piece is attacking a given square to put restraints on king and other piece movements.
    /// </summary>
    /// <param name="otherSquare"></param>
    /// <returns>True if the piece is attacking a given square, otherwise false.</returns>
    public override bool IsThreateningSquare(Square otherSquare)
    {
        return otherSquare.Row == Square.Row + (int)Color && Math.Abs(otherSquare.Column - Square.Column) == 1;
    }
}
