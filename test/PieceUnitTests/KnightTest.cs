using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.Mocks;
using Xunit;

namespace Chess.Test.PieceTests
{
    public class KnightTest
    {
        [Theory]
        [InlineData(3, 3, PieceColor.Black, 4, 5)]
        [InlineData(3, 3, PieceColor.Black, 5, 4)]
        [InlineData(2, 2, PieceColor.Black, 0, 3)]
        [InlineData(2, 2, PieceColor.Black, 3, 0)]
        public void KnightShouldBeAbleToReachSquare(int row, int column, PieceColor color, int targetRow, int targetColumn)
        {
            var _ = SetUpBoard(row, column, color, out Knight knight);

            Assert.Contains((targetRow, targetColumn), knight.GetPossibleMoves());
        }

        [Theory]
        [InlineData(3, 3, PieceColor.Black, 4, 4)]
        [InlineData(3, 3, PieceColor.Black, 5, 5)]
        [InlineData(3, 3, PieceColor.Black, 3, 3)]
        [InlineData(3, 3, PieceColor.Black, 7, 7)]
        public void KnightShouldNotBeAbleToReachSquare(int row, int column, PieceColor color, int targetRow, int targetColumn)
        {
            var _ = SetUpBoard(row, column, color, out Knight knight);

            Assert.DoesNotContain((targetRow, targetColumn), knight.GetPossibleMoves());
        }

        [Theory]
        [InlineData(3, 3, PieceColor.White, 3, 5, 4, 4, 4, 5)]
        [InlineData(3, 3, PieceColor.White, 3, 5, 4, 4, 5, 4)]
        public void KnightShouldBeAbleToGoThroughAnotherPiece(int row, int column, PieceColor color, int otherRow, int otherColumn,
            int otherRow2, int otherColumn2, int targetRow, int targetColumn)
        {
            var board = SetUpBoard(row, column, color, out Knight knight);

            Pawn pawn = new(color, (otherRow, otherColumn), board);
            board.SetSquareForPiece(pawn, pawn.Square);
            board.LivePieces[color].Add(pawn);

            Pawn pawn2 = new(color, (otherRow2, otherColumn2), board);
            board.SetSquareForPiece(pawn2, pawn2.Square);
            board.LivePieces[color].Add(pawn2);

            Assert.Contains((targetRow, targetColumn), knight.GetPossibleMoves());
        }

        [Theory]
        [InlineData(3, 0, PieceColor.White, 2, 2)]
        [InlineData(3, 0, PieceColor.White, 4, 2)]
        [InlineData(3, 0, PieceColor.Black, 5, 1)]
        public void KnightShouldBeAbletoCapturePiece(int row, int column, PieceColor color, int otherRow, int otherColumn)
        {
            var board = SetUpBoard(row, column, color, out Knight knight);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            Pawn pawn = new(oppositeColor, (otherRow, otherColumn), board);
            board.SetSquareForPiece(pawn, pawn.Square);
            board.LivePieces[oppositeColor].Add(pawn);

            Assert.Contains(pawn.Square, knight.GetPossibleMoves());
        }

        [Theory]
        [InlineData(3, 0, PieceColor.White, 3, 2)]
        [InlineData(3, 0, PieceColor.White, 3, 3)]
        [InlineData(3, 0, PieceColor.Black, 4, 4)]
        public void KnightShouldNotBeAbletoCapturePiece(int row, int column, PieceColor color, int otherRow, int otherColumn)
        {
            var board = SetUpBoard(row, column, color, out Knight knight);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            Pawn pawn = new(oppositeColor, (otherRow, otherColumn), board);
            board.SetSquareForPiece(pawn, pawn.Square);
            board.LivePieces[oppositeColor].Add(pawn);

            Assert.DoesNotContain(pawn.Square, knight.GetPossibleMoves());
        }

        [Theory]
        [InlineData(7, 5, PieceColor.White, 1, 4)]
        [InlineData(7, 1, PieceColor.White, 4, 1)]
        public void KnightShouldBeAbleToProtectKing(int row, int column, PieceColor color, int otherRow, int otherColumn)
        {
            var board = SetUpBoard(row, column, color, out Knight knight);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            board.SetNextMove(oppositeColor);

            Queen queen = new(oppositeColor, (2, 0), board);
            board.SetSquareForPiece(queen, queen.Square);
            board.LivePieces[oppositeColor].Add(queen);

            board.MovePiece(queen, (otherRow, otherColumn));

            Assert.NotEmpty(knight.GetPossibleMoves());
        }

        [Theory]
        [InlineData(7, 7, PieceColor.White, 1, 4)]
        [InlineData(6, 1, PieceColor.White, 4, 1)]
        public void KnightShouldNotBeAbleToProtectKing(int row, int column, PieceColor color, int otherRow, int otherColumn)
        {
            var board = SetUpBoard(row, column, color, out Knight knight);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            board.SetNextMove(oppositeColor);

            Queen queen = new(oppositeColor, (2, 0), board);
            board.SetSquareForPiece(queen, queen.Square);
            board.LivePieces[oppositeColor].Add(queen);

            board.MovePiece(queen, (otherRow, otherColumn));

            Assert.Empty(knight.GetPossibleMoves());
        }

        [Theory]
        [InlineData(7, 1, PieceColor.White, 5, 2)]
        [InlineData(4, 6, PieceColor.Black, 5, 4)]
        public void KnightShouldBeAbleToProtectKingByCapture(int row, int column, PieceColor color, int otherRow, int otherColumn)
        {
            var board = SetUpBoard(row, column, color, out Knight knight);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            board.SetNextMove(oppositeColor);

            Queen queen = new(oppositeColor, (2, 0), board);
            board.SetSquareForPiece(queen, queen.Square);
            board.LivePieces[oppositeColor].Add(queen);

            board.MovePiece(queen, (otherRow, otherColumn));

            Assert.Contains(queen.Square, knight.GetPossibleMoves());
        }

        private static BoardWithDirectPieceSet SetUpBoard(int row, int column, PieceColor color, out Knight knight)
        {
            var board = new BoardWithDirectPieceSet(null);
            board.SetNextMove(color);
            knight = new(color, (row, column), board);
            board.SetSquareForPiece(knight, knight.Square);
            board.LivePieces[color].Add(knight);
            return board;
        }
    }
}
