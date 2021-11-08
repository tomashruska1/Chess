using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.MockLibrary;
using Xunit;

namespace Chess.Test.PieceUnitTests;

public class BishopTest
{
    [Theory]
    [InlineData(3, 3, PieceColor.Black, 6, 6)]
    [InlineData(3, 3, PieceColor.Black, 1, 1)]
    [InlineData(3, 3, PieceColor.Black, 0, 6)]
    [InlineData(3, 3, PieceColor.Black, 6, 0)]
    [InlineData(3, 3, PieceColor.Black, 7, 7)]
    public void BishopShouldBeAbleToReachSquare(int row, int column, PieceColor color, int targetRow, int targetColumn)
    {
        var _ = SetUpBoard(row, column, color, out Bishop bishop);

        Assert.Contains((targetRow, targetColumn), bishop.GetPossibleMoves());
    }


    [Theory]
    [InlineData(3, 3, PieceColor.White, 4, 3)]
    [InlineData(3, 3, PieceColor.White, 0, 7)]
    [InlineData(3, 3, PieceColor.White, 7, 3)]
    [InlineData(0, 6, PieceColor.White, 0, 6)]
    public void BishopShouldNotBeAbleToReachSquare(int row, int column, PieceColor color, int targetRow, int targetColumn)
    {
        var _ = SetUpBoard(row, column, color, out Bishop bishop);

        Assert.DoesNotContain((targetRow, targetColumn), bishop.GetPossibleMoves());
    }

    [Theory]
    [InlineData(3, 3, PieceColor.White, 5, 5, 7, 7)]
    [InlineData(3, 3, PieceColor.Black, 1, 5, 0, 6)]
    [InlineData(3, 3, PieceColor.Black, 5, 1, 6, 0)]
    public void BishopShouldNotBeAbleToGoThroughAnotherPiece(int row, int column, PieceColor color, int otherRow, int otherColumn, int targetRow, int targetColumn)
    {
        var board = SetUpBoard(row, column, color, out Bishop bishop);

        Pawn pawn = new(color, (otherRow, otherColumn), board);
        board.SetSquareForPiece(pawn, pawn.Square);
        board.LivePieces[color].Add(pawn);

        Assert.DoesNotContain((targetRow, targetColumn), bishop.GetPossibleMoves());
    }

    [Theory]
    [InlineData(3, 3, PieceColor.White, 5, 5)]
    [InlineData(3, 3, PieceColor.Black, 2, 4)]
    [InlineData(3, 3, PieceColor.Black, 5, 1)]
    public void BishopShouldBeAbleToCapturePiece(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Bishop bishop);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

        Pawn pawn = new(oppositeColor, (otherRow, otherColumn), board);
        board.SetSquareForPiece(pawn, pawn.Square);
        board.LivePieces[oppositeColor].Add(pawn);

        Assert.Contains((otherRow, otherColumn), bishop.GetPossibleMoves());
    }

    [Theory]
    [InlineData(3, 3, PieceColor.White, 3, 5)]
    [InlineData(3, 3, PieceColor.Black, 1, 3)]
    [InlineData(3, 3, PieceColor.Black, 6, 1)]
    public void BishopShouldNotBeAbleToCapturePiece(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Bishop bishop);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

        Pawn pawn = new(oppositeColor, (otherRow, otherColumn), board);
        board.SetSquareForPiece(pawn, pawn.Square);
        board.LivePieces[oppositeColor].Add(pawn);

        Assert.DoesNotContain((otherRow, otherColumn), bishop.GetPossibleMoves());
    }

    [Theory]
    [InlineData(6, 0, PieceColor.White, 1, 4)]
    [InlineData(6, 1, PieceColor.White, 4, 1)]
    public void BishopShouldBeAbleToProtectKing(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Bishop bishop);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen = new(oppositeColor, (3, 0), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[oppositeColor].Add(queen);

        board.MovePiece(queen, (otherRow, otherColumn));

        Assert.NotEmpty(bishop.GetPossibleMoves());
    }

    [Theory]
    [InlineData(5, 7, PieceColor.White, 3, 4)]
    [InlineData(7, 1, PieceColor.White, 4, 1)]
    public void BishopShouldNotBeAbleToProtectKing(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Bishop bishop);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen = new(oppositeColor, (3, 0), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[oppositeColor].Add(queen);

        board.MovePiece(queen, (otherRow, otherColumn));

        Assert.Empty(bishop.GetPossibleMoves());
    }

    [Theory]
    [InlineData(7, 0, PieceColor.White, 5, 2)]
    [InlineData(6, 7, PieceColor.Black, 3, 4)]
    public void BishopShouldBeAbleToProtectKingByCapture(int row, int column, PieceColor color, int otherRow, int otherColumn)
    {
        var board = SetUpBoard(row, column, color, out Bishop bishop);
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen = new(oppositeColor, (2, 0), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[oppositeColor].Add(queen);

        board.MovePiece(queen, (otherRow, otherColumn));

        Assert.Contains(queen.Square, bishop.GetPossibleMoves());
    }

    private static BoardWithDirectPieceSet SetUpBoard(int row, int column, PieceColor color, out Bishop bishop)
    {
        var board = new BoardWithDirectPieceSet(null);
        board.SetNextMove(color);
        bishop = new(color, (row, column), board);
        board.SetSquareForPiece(bishop, bishop.Square);
        board.LivePieces[color].Add(bishop);
        return board;
    }
}
