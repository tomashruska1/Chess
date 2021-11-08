using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.MockLibrary;
using Xunit;

namespace Chess.Test.PieceUnitTests;

public class KingTest
{
    [Theory]
    [InlineData(PieceColor.White, 7, 3)]
    [InlineData(PieceColor.White, 7, 5)]
    [InlineData(PieceColor.White, 6, 4)]
    [InlineData(PieceColor.White, 6, 5)]
    public void KingShouldBeAbleToReachSquare(PieceColor color, int targetRow, int targetColumn)
    {
        var board = SetUpBoard(color);

        King king = board.Kings[color];

        Assert.Contains((targetRow, targetColumn), king.GetPossibleMoves());
    }

    [Theory]
    [InlineData(PieceColor.White, 5, 2)]
    [InlineData(PieceColor.White, 3, 3)]
    [InlineData(PieceColor.White, 0, 0)]
    [InlineData(PieceColor.White, 1, 2)]
    public void KingShouldNotBeAbleToReachSquare(PieceColor color, int targetRow, int targetColumn)
    {
        var board = SetUpBoard(color);

        King king = board.Kings[color];

        Assert.DoesNotContain((targetRow, targetColumn), king.GetPossibleMoves());
    }

    [Theory]
    [InlineData(PieceColor.White, 7, 5, 2, 0)]
    [InlineData(PieceColor.White, 7, 3, 3, 7)]
    [InlineData(PieceColor.Black, 1, 3, 5, 7)]
    [InlineData(PieceColor.Black, 1, 4, 4, 7)]
    public void KingShouldNotBeAbleToEnterSquareUnderAttack(PieceColor color, int targetRow, int targetColumn, int bishopRow, int bishopColumn)
    {
        var board = SetUpBoard(color);
        King king = board.Kings[color];
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Bishop bishop = new(oppositeColor, (0, 0), board);
        board.SetSquareForPiece(bishop, bishop.Square);
        board.LivePieces[oppositeColor].Add(bishop);

        board.MovePiece(bishop, (bishopRow, bishopColumn));

        Assert.DoesNotContain((targetRow, targetColumn), king.GetPossibleMoves());
    }

    [Theory]
    [InlineData(3, 4, PieceColor.White)]
    [InlineData(3, 4, PieceColor.Black)]
    public void KingShouldBeUnderAttack(int row, int column, PieceColor color)
    {
        var board = SetUpBoard(color);
        King king = board.Kings[color];
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen = new(oppositeColor, (3, 0), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[oppositeColor].Add(queen);

        board.MovePiece(queen, (row, column));

        Assert.True(king.IsUnderAttack);
    }

    [Theory]
    [InlineData(3, 4, PieceColor.White)]
    [InlineData(3, 4, PieceColor.Black)]
    public void KingShouldBeAbleToRetreat(int row, int column, PieceColor color)
    {
        var board = SetUpBoard(color);
        King king = board.Kings[color];
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen = new(oppositeColor, (3, 0), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[oppositeColor].Add(queen);

        board.MovePiece(queen, (row, column));

        Assert.NotEmpty(king.GetPossibleMoves());
    }

    [Theory]
    [InlineData(6, 0, 7, 0, PieceColor.White)]
    [InlineData(1, 0, 0, 0, PieceColor.Black)]
    public void KingShouldNotBeAbleToRetreat(int row1, int column1, int row2, int column2, PieceColor color)
    {
        var board = SetUpBoard(color);
        King king = board.Kings[color];
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen1 = new(oppositeColor, (3, 0), board);
        board.SetSquareForPiece(queen1, queen1.Square);
        board.LivePieces[oppositeColor].Add(queen1);

        Queen queen2 = new(oppositeColor, (4, 0), board);
        board.SetSquareForPiece(queen2, queen2.Square);
        board.LivePieces[oppositeColor].Add(queen2);

        board.MovePiece(queen1, (row1, column1));
        board.SetNextMove(oppositeColor);
        board.MovePiece(queen2, (row2, column2));

        Assert.Empty(king.GetPossibleMoves());
    }

    [Theory]
    [InlineData(3, 0, 5, 2, PieceColor.White)]
    [InlineData(1, 4, 4, 4, PieceColor.White)]
    [InlineData(4, 4, 1, 4, PieceColor.Black)]
    public void KingShouldBeUnderAttackThenNot(int enemyRow, int enemyColumn, int friendlyRow, int friendlyColumn, PieceColor color)
    {
        var board = SetUpBoard(color);
        King king = board.Kings[color];
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen enemyQueen = new(oppositeColor, (1, 0), board);
        board.SetSquareForPiece(enemyQueen, enemyQueen.Square);
        board.LivePieces[oppositeColor].Add(enemyQueen);

        Queen friendlyQueen = new(color, (2, 0), board);
        board.SetSquareForPiece(friendlyQueen, friendlyQueen.Square);
        board.LivePieces[color].Add(friendlyQueen);

        board.MovePiece(enemyQueen, (enemyRow, enemyColumn));
        Assert.True(king.IsUnderAttack);

        board.MovePiece(friendlyQueen, (friendlyRow, friendlyColumn));
        Assert.False(king.IsUnderAttack);
    }

    [Theory]
    [InlineData(6, 5, PieceColor.White)]
    [InlineData(6, 4, PieceColor.White)]
    [InlineData(0, 3, PieceColor.Black)]
    public void KingShouldBeAbleToCaptureAttacker(int enemyRow, int enemyColumn, PieceColor color)
    {
        var board = SetUpBoard(color);
        King king = board.Kings[color];
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen = new(oppositeColor, (2, 0), board);
        board.SetSquareForPiece(queen, queen.Square);
        board.LivePieces[oppositeColor].Add(queen);

        Assert.False(king.IsUnderAttack);
        board.MovePiece(queen, (enemyRow, enemyColumn));

        Assert.True(king.IsUnderAttack);
        Assert.Contains(queen.Square, king.GetPossibleMoves());
    }

    [Theory]
    [InlineData(1, 5, 6, 5, PieceColor.White)]
    [InlineData(6, 0, 6, 4, PieceColor.White)]
    [InlineData(1, 1, 1, 4, PieceColor.Black)]
    public void KingShouldNotBeAbleToCaptureAttacker(int enemyRow1, int enemyColumn1, int enemyRow2, int enemyColumn2, PieceColor color)
    {
        var board = SetUpBoard(color);
        King king = board.Kings[color];
        PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        board.SetNextMove(oppositeColor);

        Queen queen1 = new(oppositeColor, (1, 0), board);
        board.SetSquareForPiece(queen1, queen1.Square);
        board.LivePieces[oppositeColor].Add(queen1);

        Queen queen2 = new(oppositeColor, (2, 0), board);
        board.SetSquareForPiece(queen2, queen2.Square);
        board.LivePieces[oppositeColor].Add(queen2);

        Assert.False(king.IsUnderAttack);
        board.MovePiece(queen1, (enemyRow1, enemyColumn1));
        board.SetNextMove(oppositeColor);
        board.MovePiece(queen2, (enemyRow2, enemyColumn2));

        Assert.True(king.IsUnderAttack);
        Assert.DoesNotContain(queen1.Square, king.GetPossibleMoves());
        Assert.DoesNotContain(queen2.Square, king.GetPossibleMoves());
    }

    [Theory]
    [InlineData(7, 7, PieceColor.White, 7, 6, 7, 5)]
    [InlineData(7, 0, PieceColor.White, 7, 2, 7, 3)]
    [InlineData(0, 7, PieceColor.Black, 0, 6, 0, 5)]
    public void KingCastlingShouldSucceed(int row, int column, PieceColor color, int kingRow, int kingColumn, int rookRow, int rookColumn)
    {
        var board = SetUpBoard(color);
        King king = board.Kings[color];

        Rook rook = new(color, (row, column), board);
        board.SetSquareForPiece(rook, (row, column));
        board.LivePieces[color].Add(rook);

        Assert.Contains((kingRow, kingColumn), king.GetPossibleMoves());

        board.MovePiece(king, (kingRow, kingColumn));

        Assert.Equal(king.Square, (kingRow, kingColumn));
        Assert.Equal(rook.Square, (rookRow, rookColumn));
        Assert.Equal(king.GetType(), board[king.Square].GetType());
        Assert.Equal(rook.GetType(), board[rook.Square].GetType());
    }

    private static BoardWithDirectPieceSet SetUpBoard(PieceColor color)
    {
        var board = new BoardWithDirectPieceSet(null);
        board.SetNextMove(color);
        return board;
    }
}
