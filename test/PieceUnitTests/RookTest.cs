using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.MockLibrary;
using Xunit;

namespace Chess.Test.PieceUnitTests;

public class RookTest
{
    [Theory]
    [InlineData(0, 6, PieceColor.White, 7, 6)]
    [InlineData(1, 6, PieceColor.White, 1, 1)]
    public void RookShouldBeAbleToReachSquare(int row, int column, PieceColor color, int targetRow, int targetColumn)
    {
        var _ = SetUpBoard(row, column, color, out Rook rook);

        Assert.Contains((targetRow, targetColumn), rook.GetPossibleMoves());
    }

    [Theory]
    [InlineData(0, 6, PieceColor.White, 4, 2)]
    [InlineData(0, 6, PieceColor.White, 7, 5)]
    [InlineData(0, 6, PieceColor.White, 0, 6)]
    public void RookShouldNotBeAbleToReachSquare(int row, int column, PieceColor color, int targetRow, int targetColumn)
    {
        var _ = SetUpBoard(row, column, color, out Rook rook);

        Assert.DoesNotContain((targetRow, targetColumn), rook.GetPossibleMoves());
    }

    [Theory]
    [InlineData(3, 0, PieceColor.White, 3, 2, 3, 7)]
    [InlineData(3, 0, PieceColor.White, 6, 0, 7, 0)]
    [InlineData(3, 0, PieceColor.White, 7, 0, 7, 0)]
    public void RookShouldNotBeAbleToGoThroughAnotherPiece(int row, int column, PieceColor color, int otherRow, int otherColumn, int targetRow, int targetColumn)
    {
        var board = SetUpBoard(row, column, color, out Rook rook);

        Pawn pawn = new(color, (otherRow, otherColumn), board);
        board.SetSquareForPiece(pawn, pawn.Square);
        board.LivePieces[color].Add(pawn);

        Assert.DoesNotContain((targetRow, targetColumn), rook.GetPossibleMoves());
    }

    [Theory]
    [InlineData(3, 0, PieceColor.White, 6, 0)]
    [InlineData(3, 0, PieceColor.White, 3, 7)]
    [InlineData(3, 0, PieceColor.Black, 6, 0)]
    public void RookShouldBeAbletoCapturePiece(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Rook rook);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

        Pawn pawn = new(oppositeColor, (otherRow, otherColumn), board);
        board.SetSquareForPiece(pawn, pawn.Square);
        board.LivePieces[oppositeColor].Add(pawn);

        Assert.Contains(pawn.Square, rook.GetPossibleMoves());
    }

    [Theory]
    [InlineData(3, 0, PieceColor.White, 6, 6)]
    [InlineData(3, 0, PieceColor.White, 4, 4)]
    [InlineData(3, 0, PieceColor.Black, 2, 6)]
    public void RookShouldNotBeAbletoCapturePiece(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Rook rook);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

        Pawn pawn = new(oppositeColor, (otherRow, otherColumn), board);
        board.SetSquareForPiece(pawn, pawn.Square);
        board.LivePieces[oppositeColor].Add(pawn);

        Assert.DoesNotContain(pawn.Square, rook.GetPossibleMoves());
    }

    [Theory]
    [InlineData(6, 0, PieceColor.White, 1, 4)]
    [InlineData(6, 0, PieceColor.White, 4, 1)]
    public void RookShouldBeAbleToProtectKing(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Rook rook);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen = new(oppositeColor, (3, 0), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[oppositeColor].Add(queen);

        board.MovePiece(queen, (otherRow, otherColumn));

        Assert.NotEmpty(rook.GetPossibleMoves());
    }

    [Theory]
    [InlineData(7, 0, PieceColor.White, 1, 4)]
    [InlineData(7, 0, PieceColor.White, 4, 1)]
    public void RookShouldNotBeAbleToProtectKing(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Rook rook);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen = new(oppositeColor, (3, 0), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[oppositeColor].Add(queen);

        board.MovePiece(queen, (otherRow, otherColumn));

        Assert.Empty(rook.GetPossibleMoves());
    }

    [Theory]
    [InlineData(7, 0, PieceColor.White, 3, 0)]
    [InlineData(1, 7, PieceColor.Black, 3, 7)]
    public void RookShouldBeAbleToProtectKingByCapture(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Rook rook);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen = new(oppositeColor, (2, 0), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[oppositeColor].Add(queen);

        board.MovePiece(queen, (otherRow, otherColumn));

        Assert.Contains(queen.Square, rook.GetPossibleMoves());
    }

    private static BoardWithDirectPieceSet SetUpBoard(int row, int column, PieceColor color, out Rook rook)
    {
        var board = new BoardWithDirectPieceSet(null);
        board.SetNextMove(color);
        rook = new(color, (row, column), board);
        board.SetSquareForPiece(rook, rook.Square);
        board.LivePieces[color].Add(rook);
        return board;
    }
}
