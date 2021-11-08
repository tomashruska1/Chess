using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.MockLibrary;
using Xunit;

namespace Chess.Test.PieceUnitTests;

public class QueenTest
{
    [Theory]
    [InlineData(0, 6, PieceColor.White, 7, 6)]
    [InlineData(1, 6, PieceColor.White, 1, 1)]
    [InlineData(1, 6, PieceColor.White, 6, 1)]
    [InlineData(2, 2, PieceColor.White, 5, 5)]
    public void QueenShouldBeAbleToReachSquare(int row, int column, PieceColor color, int targetRow, int targetColumn)
    {
        var _ = SetUpBoard(row, column, color, out Queen queen);

        Assert.Contains((targetRow, targetColumn), queen.GetPossibleMoves());
    }

    [Theory]
    [InlineData(0, 6, PieceColor.White, 3, 4)]
    [InlineData(1, 6, PieceColor.White, 4, 1)]
    [InlineData(1, 6, PieceColor.White, 7, 7)]
    [InlineData(2, 2, PieceColor.White, 4, 3)]
    public void QueenShouldNotBeAbleToReachSquare(int row, int column, PieceColor color, int targetRow, int targetColumn)
    {
        var _ = SetUpBoard(row, column, color, out Queen queen);

        Assert.DoesNotContain((targetRow, targetColumn), queen.GetPossibleMoves());
    }

    [Theory]
    [InlineData(0, 6, PieceColor.White, 5, 6, 7, 6)]
    [InlineData(1, 6, PieceColor.White, 1, 3, 1, 1)]
    [InlineData(1, 6, PieceColor.White, 2, 5, 6, 1)]
    public void QueenShouldNotBeAbleToGoThroughAnotherPiece(int row, int column, PieceColor color, int otherRow, int otherColumn, int targetRow, int targetColumn)
    {
        var board = SetUpBoard(row, column, color, out Queen queen);

        Pawn pawn = new(color, (otherRow, otherColumn), board);
        board.SetSquareForPiece(pawn, pawn.Square);
        board.LivePieces[color].Add(pawn);

        Assert.DoesNotContain((targetRow, targetColumn), queen.GetPossibleMoves());
    }

    private static BoardWithDirectPieceSet SetUpBoard(int row, int column, PieceColor color, out Queen queen)
    {
        var board = new BoardWithDirectPieceSet(null);
        board.SetNextMove(color);
        queen = new(color, (row, column), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[color].Add(queen);
        return board;
    }
}
