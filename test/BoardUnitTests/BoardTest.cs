using Chess.Application.Boards;
using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.MockLibrary;
using Xunit;

namespace Chess.Test.BoardTests;

public class BoardTest
{
    [Fact]
    public void BoardSetUpShouldHaveAllPiecesInCorrectPlaces()
    {
        var board = new Board(new PawnPromoter(typeof(Queen)));
        board.SetUpBoard();

        for (int column = 0; column < 8; column++)
        {
            Assert.IsType<Pawn>(board[(1, column)]);
            Assert.IsType<Pawn>(board[(6, column)]);
        }

        for (int row = 0; row < 8; row += 7)
        {
            Assert.IsType<Rook>(board[(row, 0)]);
            Assert.IsType<Knight>(board[(row, 1)]);
            Assert.IsType<Bishop>(board[(row, 2)]);
            Assert.IsType<Queen>(board[(row, 3)]);
            Assert.IsType<King>(board[(row, 4)]);
            Assert.IsType<Bishop>(board[(row, 5)]);
            Assert.IsType<Knight>(board[(row, 6)]);
            Assert.IsType<Rook>(board[(row, 7)]);
        }

        for (int row = 2; row < 6; row++)
        {
            for (int column = 0; column < 8; column++)
                Assert.Null(board[(row, column)]);
        }
    }

    [Fact]
    public void BoardSetUpPiecesShouldHaveCorrectColor()
    {
        var board = new Board(new PawnPromoter(typeof(Queen)));
        board.SetUpBoard();

        for (int column = 0; column < 8; column++)
        {
            Assert.True(board[(0, column)].Color == PieceColor.Black);
            Assert.True(board[(1, column)].Color == PieceColor.Black);
            Assert.True(board[(6, column)].Color == PieceColor.White);
            Assert.True(board[(7, column)].Color == PieceColor.White);
        }
    }

    [Fact]
    public void ColorIsSwitchedOnEachMove()
    {
        Board board = new(new PawnPromoter(typeof(Queen)));
        board.SetUpBoard();
        int counter = 0;

        Random random = new();
        PieceColor nextMove = PieceColor.White;

        while (board.Winner is null)
        {
            if (counter == 1_000)
                break;

            Assert.True(board.NextMove == nextMove);

            var piece = board.LivePieces[nextMove][random.Next(board.LivePieces[nextMove].Count)];

            var moves = piece.GetPossibleMoves();

            counter++;
            if (moves.Count == 0)
            {
                continue;
            }

            board.MovePiece(piece, moves[random.Next(moves.Count)]);

            nextMove = nextMove == PieceColor.White ? PieceColor.Black : PieceColor.White;
        }


    }
}
