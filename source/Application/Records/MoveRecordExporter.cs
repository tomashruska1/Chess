namespace Chess.Application.Records;


/// <summary>
/// Provides a means of collecting data about all moves made in a given game,
/// optionally may export this into a text file formatted as standard notation.
/// </summary>
public class MoveRecordExporter : IMoveRecordExporter
{
    private List<MoveRecord> Moves { get; set; }

    /// <summary>
    /// Creates an instance of <see cref="MoveRecordExporter"/>
    /// </summary>
    public MoveRecordExporter()
    {
        Moves = new();
    }

    /// <summary>
    /// Add Move struct to a collection of moves.
    /// </summary>
    /// <remarks>
    /// See <see cref="MoveRecord"/>
    /// </remarks>
    /// <param name="move"></param>
    public void Add(MoveRecord move) => Moves.Add(move);

    /// <summary>
    /// Iterates through collection of moves, translates it to standard notation,
    /// and appends it to a text file "moves.txt".
    /// </summary>
    public void Export()
    {
        // TODO proper refactoring
        int currentMove = 0;
        int moveNo = 1;

        using StreamWriter file = new("moves.txt", true);

        file.WriteLine($"{DateTime.Now}");

        while (currentMove < Moves.Count)
        {
            file.Write($"{moveNo}.");

            for (int whiteOrBlack = 0; whiteOrBlack < 2; whiteOrBlack++)
            {
                if (currentMove + whiteOrBlack >= Moves.Count)
                    continue;

                file.Write(" ");

                MoveRecord move = Moves[currentMove + whiteOrBlack];

                if (IsMoveType(move, MoveType.KingsideCastling))
                {
                    file.Write("0-0");
                    continue;
                }
                else if (IsMoveType(move, MoveType.QueensideCastling))
                {
                    file.Write("0-0-0");
                    continue;
                }
                else if (!IsTypeOfPiece<Pawn>(move) && IsMoveType(move, MoveType.PawnPromotion))
                {
                    file.Write($"{move.Piece.GetType().Name[0]}");
                }

                if (IsMoveType(move, MoveType.Capture))
                {
                    if (IsTypeOfPiece<Pawn>(move) || IsMoveType(move, MoveType.PawnPromotion))
                    {
                        file.Write($"{(char)('a' + move.StartColumn)}");
                    }
                    file.Write("\u2a2f");
                }

                file.Write($"{(char)('a' + move.Column)}{8 - move.Row}");

                if (IsMoveType(move, MoveType.PawnPromotion))
                    file.Write($"{move.Piece.GetType().Name[0]}");

                if (IsMoveType(move, MoveType.EnPassantCapture))
                    file.Write(" e.p.");

                if (IsMoveType(move, MoveType.Check))
                    file.Write("+");

                if (IsMoveType(move, MoveType.CheckMate))
                    file.Write("#");
            }
            file.WriteLine("");
            moveNo += 1;
            currentMove += 2;
        }
        file.WriteLine("");
    }

    private static bool IsTypeOfPiece<T>(MoveRecord move)
    {
        return move.Piece.GetType().Equals(typeof(T));
    }

    private static bool IsMoveType(MoveRecord move, MoveType moveType)
    {
        return (move.Type | moveType) == move.Type;
    }
}
