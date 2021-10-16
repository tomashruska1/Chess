using Chess.Application.BoardControllers;
using Chess.Application.Boards;
using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.Mocks;
using System;
using Xunit;

namespace Chess.Test.PieceTests
{
    public class PawnTest
    {
        [Theory]
        [InlineData(1, 3, PieceColor.Black)]
        [InlineData(1, 7, PieceColor.Black)]
        [InlineData(6, 0, PieceColor.White)]
        public void PawnShouldHaveTwoMoves(int row, int column, PieceColor color)
        {
            int expected = 2;

            var _ = SetUpBoard(row, column, color, out Pawn pawn);

            int moveCount = pawn.GetPossibleMoves().Count;

            Assert.Equal(expected, moveCount);
        }

        [Theory]
        [InlineData(1, 3, PieceColor.Black)]
        [InlineData(1, 7, PieceColor.Black)]
        [InlineData(6, 0, PieceColor.White)]
        public void PawnShouldHaveOneMove(int row, int column, PieceColor color)
        {
            int expected = 1;

            var board = SetUpBoard(row, column, color, out Pawn pawn);
            board.MovePiece(pawn, pawn.Square + ((int)pawn.Color, 0));

            int moveCount = pawn.GetPossibleMoves().Count;

            Assert.Equal(expected, moveCount);
        }

        [Theory]
        [InlineData(1, 3, PieceColor.Black)]
        [InlineData(1, 7, PieceColor.Black)]
        [InlineData(6, 0, PieceColor.White)]
        public void PawnShouldHaveNoMove(int row, int column, PieceColor color)
        {
            int expected = 0;
            var board = SetUpBoard(row, column, color, out Pawn pawn);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            board.SetNextMove(oppositeColor);

            Queen queen = new(oppositeColor, (3, 0), board);
            board.SetSquareForPiece(queen, queen.Square);
            board.LivePieces[oppositeColor].Add(queen);

            board.MovePiece(queen, (3, 4));

            int moveCount = pawn.GetPossibleMoves().Count;

            Assert.Equal(expected, moveCount);
        }

        [Theory]
        [InlineData(3, 5, 6, 1, PieceColor.White)]
        [InlineData(3, 5, 7, 0, PieceColor.White)]
        [InlineData(3, 5, 4, -1, PieceColor.White)]
        public void PawnCanDoEnPassant(int row, int column, int enemyColumn, int expected, PieceColor color)
        {
            var board = SetUpBoard(row, column, color, out Pawn pawn);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            board.SetNextMove(oppositeColor);

            Pawn enemyPawn = new(oppositeColor, (row + 2*(int)color, enemyColumn), board);
            board.SetSquareForPiece(enemyPawn, enemyPawn.Square);
            board.LivePieces[oppositeColor].Add(enemyPawn);

            board.MovePiece(enemyPawn, enemyPawn.Square + (2 * (int)enemyPawn.Color, 0));

            Assert.Equal(expected, board.CanDoEnPassant(pawn));
        }

        [Theory]
        [InlineData(3, 2, 3, PieceColor.White)]
        [InlineData(3, 4, 3, PieceColor.White)]
        public void PawnShouldBeRemovedAfterEnPassant(int row, int column, int enemyStartColumn, PieceColor color)
        {
            var board = SetUpBoard(row, column, color, out Pawn pawn);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            board.SetNextMove(oppositeColor);

            Pawn enemyPawn = new(oppositeColor, (row + 2 * (int)color, enemyStartColumn), board);
            board.SetSquareForPiece(enemyPawn, enemyPawn.Square);
            board.LivePieces[oppositeColor].Add(enemyPawn);

            board.MovePiece(enemyPawn, enemyPawn.Square + (2 * (int)enemyPawn.Color, 0));

            int canDoEnPassant = board.CanDoEnPassant(pawn);
            Assert.NotEqual(0, canDoEnPassant);

            board.MovePiece(pawn, pawn.Square + ((int)pawn.Color, canDoEnPassant));

            Assert.True(pawn.Square == (row + (int)pawn.Color, enemyPawn.Square.Column));
            Assert.True(board[enemyPawn.Square] is null);
            Assert.DoesNotContain(enemyPawn, board.LivePieces[enemyPawn.Color]);
        }

        [Theory]
        [InlineData(1, 3, PieceColor.White, typeof(Queen))]
        [InlineData(6, 3, PieceColor.Black, typeof(Rook))]
        [InlineData(6, 3, PieceColor.Black, typeof(Bishop))]
        [InlineData(6, 3, PieceColor.Black, typeof(Knight))]
        public void PawnPromotion(int row, int column, PieceColor color, Type expected)
        {
            IPawnPromoter promoter = new PawnPromoter(expected);
            var board = SetUpBoard(row, column, color, out Pawn pawn, promoter);

            Square square = pawn.Square + ((int)pawn.Color, 0);

            board.MovePiece(pawn, square);

            Assert.True(board[square].GetType().Equals(expected));
            Assert.DoesNotContain(pawn, board.LivePieces[pawn.Color]);
            Assert.Contains(board[square], board.LivePieces[color]);
        }

        [Theory]
        [InlineData(3, 3, PieceColor.White, typeof(Queen))]
        [InlineData(5, 3, PieceColor.Black, typeof(Rook))]
        [InlineData(5, 3, PieceColor.Black, typeof(Bishop))]
        [InlineData(5, 3, PieceColor.Black, typeof(Knight))]
        public void PawnPromotionShouldNotHappen(int row, int column, PieceColor color, Type expected)
        {
            IPawnPromoter promoter = new PawnPromoter(expected);
            var board = SetUpBoard(row, column, color, out Pawn pawn, promoter);

            Square square = pawn.Square + ((int)pawn.Color, 0);

            board.MovePiece(pawn, square);

            Assert.True(board[square].GetType().Equals(typeof(Pawn)));
            Assert.Contains(pawn, board.LivePieces[pawn.Color]);
        }

        [Theory]
        [InlineData(3, 0, PieceColor.White, 2, 1)]
        [InlineData(5, 5, PieceColor.White, 4, 6)]
        [InlineData(5, 5, PieceColor.White, 4, 4)]
        [InlineData(1, 2, PieceColor.Black, 2, 1)]
        [InlineData(1, 2, PieceColor.Black, 2, 3)]
        public void PawnShouldBeAbleToCapturePiece(int row, int column, PieceColor color, int otherRow, int otherColumn)
        {
            var board = SetUpBoard(row, column, color, out Pawn pawn);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            Pawn otherPawn = new(oppositeColor, (otherRow, otherColumn), board);
            board.SetSquareForPiece(otherPawn, otherPawn.Square);
            board.LivePieces[oppositeColor].Add(otherPawn);

            Assert.Contains(otherPawn.Square, pawn.GetPossibleMoves());
        }

        [Theory]
        [InlineData(1, 0, PieceColor.Black, 2, 0)]
        [InlineData(5, 5, PieceColor.Black, 4, 6)]
        [InlineData(5, 5, PieceColor.Black, 4, 4)]
        [InlineData(6, 2, PieceColor.White, 4, 2)]
        [InlineData(6, 2, PieceColor.White, 3, 2)]
        [InlineData(6, 2, PieceColor.White, 1, 2)]
        public void PawnShouldNotBeAbleToCapturePiece(int row, int column, PieceColor color, int otherRow, int otherColumn)
        {
            var board = SetUpBoard(row, column, color, out Pawn pawn);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            Pawn otherPawn = new(oppositeColor, (otherRow, otherColumn), board);
            board.SetSquareForPiece(otherPawn, otherPawn.Square);
            board.LivePieces[oppositeColor].Add(otherPawn);

            Assert.DoesNotContain(otherPawn.Square, pawn.GetPossibleMoves());
        }

        [Theory]
        [InlineData(6, 6, PieceColor.White, 4, 7, 5, 6)]
        public void PawnShouldBeAbleToProtectKing(int row, int column, PieceColor color, int qRow, int qCoumn, int iRow, int iColumn)
        {
            var board = SetUpBoard(row, column, color, out Pawn pawn);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            board.SetNextMove(oppositeColor);

            Queen queen = new(oppositeColor, (3, 0), board);
            board.SetSquareForPiece(queen, queen.Square);
            board.LivePieces[oppositeColor].Add(queen);

            board.MovePiece(queen, (qRow, qCoumn));

            var moves = pawn.GetPossibleMoves();

            Assert.True(moves.Contains((iRow, iColumn)) && moves.Count == 1);
        }

        [Theory]
        [InlineData(6, 5, PieceColor.White, 5, 4)]
        [InlineData(1, 5, PieceColor.Black, 2, 4)]
        public void PawnShouldBeAbleToProtectKingByCapture(int row, int column, PieceColor color, int qRow, int qCoumn)
        {
            var board = SetUpBoard(row, column, color, out Pawn pawn);
            PieceColor oppositeColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            board.SetNextMove(oppositeColor);

            Queen queen = new(oppositeColor, (3, 0), board);
            board.SetSquareForPiece(queen, queen.Square);
            board.LivePieces[oppositeColor].Add(queen);

            board.MovePiece(queen, (qRow, qCoumn));

            Assert.Contains(queen.Square, pawn.GetPossibleMoves());
        }

        private static BoardWithDirectPieceSet SetUpBoard(int row, int column, PieceColor color, out Pawn pawn)
        {
            return SetUpBoard(row, column, color, out pawn, null);
        }

        private static BoardWithDirectPieceSet SetUpBoard(int row, int column, PieceColor color, out Pawn pawn, IPawnPromoter promoter)
        {
            var board = new BoardWithDirectPieceSet(promoter);
            board.SetNextMove(color);
            pawn = new(color, (row, column), board);
            board.SetSquareForPiece(pawn, pawn.Square);
            board.LivePieces[color].Add(pawn);
            return board;
        }
    }
}
