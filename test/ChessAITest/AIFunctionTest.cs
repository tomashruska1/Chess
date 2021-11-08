using Chess.Application.Boards;
using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.MockLibrary;
using Xunit;

namespace Chess.Test.ChessAITest;

public class AIFunctionTest
{
    [Fact]
    public void AICanHaveNonstandardBoardSetUp()
    {
        var chessInterface = new DummyBoardInterface();
        chessInterface.NewNonStandardGame(PieceColor.Black);
        AITestHelper.AddFourBishops(chessInterface);

        var ai = AITestHelper.GetChessAI(chessInterface.Controller);
        ai.SetUpNonstandardBoard(chessInterface.Controller.Board);

        var aiBoard = AITestHelper.GetAIBoard(ai);

        var squares = Enumerable.Range(0, 8).SelectMany(x => Enumerable.Range(0, 8).Select(y => new Square(x, y)));

        foreach (var square in squares)
        {
            if (chessInterface.Controller.Board[square] is null && aiBoard[square] is null)
                continue;
            Assert.Equal(chessInterface.Controller.Board[square].GetType(), aiBoard[square].GetType());
        }
    }

    [Fact]
    public void AIShouldBeAbleToPlayAsWhite()
    {
        var chessInterface = new DummyBoardInterface();
        chessInterface.NewGame(PieceColor.White);

        Assert.Equal(PieceColor.Black, chessInterface.Controller.Board.NextMove);
    }

    [Fact]
    public void AIShouldBeAbleToPlayAsBlack()
    {
        var chessInterface = new DummyBoardInterface();
        chessInterface.NewGame(PieceColor.Black);

        Piece piece = chessInterface.Controller.Board.LivePieces[PieceColor.White].First();
        chessInterface.Controller.MovePiece(piece, piece.GetPossibleMoves().First());

        Assert.Equal(PieceColor.White, chessInterface.Controller.Board.NextMove);
    }

    [Fact]
    public void AIShouldBeAbleToHandleCheck()
    {
        var chessInterface = new DummyBoardInterface();
        chessInterface.NewNonStandardGame(PieceColor.Black);

        AITestHelper.AddFourBishops(chessInterface);

        var ai = AITestHelper.GetChessAI(chessInterface.Controller);
        ai.SetUpNonstandardBoard(chessInterface.Controller.Board);

        Piece piece = chessInterface.Controller.Board.LivePieces[PieceColor.White].Where(piece => !piece.GetType().Equals(typeof(King))).First();
        chessInterface.Controller.MovePiece(piece, (4, 0));
        Assert.False(chessInterface.Controller.Board.Kings[PieceColor.Black].IsUnderAttack);
    }

    [Fact]
    public void AIShouldBeAbleToDoMultipleMoves()
    {
        Random random = new();
        var chessInterface = new DummyBoardInterface();
        chessInterface.NewGame(PieceColor.Black);
        int count = 0;

        while (!chessInterface.GameEnded && count < 20)
        {
            var pieces = chessInterface.Controller.Board.LivePieces[PieceColor.White];
            Piece piece = pieces[random.Next(pieces.Count)];

            var moves = piece.GetPossibleMoves();

            if (moves.Count == 0)
                continue;
            var move = moves[random.Next(moves.Count)];
            chessInterface.Controller.MovePiece(piece, move);
            count++;
        }
    }

    [Fact]
    public void AIShouldBeAbleToHandlePawnPromotion()
    {
        var chessInterface = new DummyBoardInterface();
        chessInterface.NewNonStandardGame(PieceColor.Black);

        Piece pawn = AITestHelper.GetPawn(PieceColor.White, (2, 0), chessInterface);
        chessInterface.AddPiece(pawn);

        Knight knight = new(PieceColor.Black, (0, 3), chessInterface.Controller.Board);
        chessInterface.AddPiece(knight);

        var ai = AITestHelper.GetChessAI(chessInterface.Controller);
        ai.SetUpNonstandardBoard(chessInterface.Controller.Board);
        var aiBoard = AITestHelper.GetAIBoard(ai);
        Piece aiPawn = aiBoard[pawn.Square];
        Assert.Contains(aiPawn, aiBoard.LivePieces[pawn.Color]);

        chessInterface.MovePiece(pawn.Square, (1, 0));
        chessInterface.MovePiece(pawn.Square, (0, 0));

        Assert.Equal(2, chessInterface.Controller.Board.LivePieces[pawn.Color].Count);
        Assert.DoesNotContain(pawn, chessInterface.Controller.Board.LivePieces[pawn.Color]);

        Assert.Equal(2, aiBoard.LivePieces[pawn.Color].Count);
        Assert.DoesNotContain(aiPawn, aiBoard.LivePieces[pawn.Color]);
    }
}
