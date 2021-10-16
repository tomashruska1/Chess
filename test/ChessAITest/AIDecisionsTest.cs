using Chess.Application.Boards;
using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.Mocks;
using System;
using System.Linq;
using Xunit;

namespace Chess.Test.ChessAITest
{
    public class AIDecisionsTest
    {
        [Fact]
        public void AIShouldMakeCheckMateInOne()
        {
            var chessInterface = new DummyBoardInterface();
            chessInterface.NewNonStandardGame(PieceColor.Black);

            chessInterface.AddPiece(new Rook(PieceColor.Black, (0, 0), chessInterface.Controller.Board));
            chessInterface.AddPiece(new Rook(PieceColor.Black, (6, 7), chessInterface.Controller.Board));

            var ai = AITestHelper.GetChessAI(chessInterface.Controller);
            ai.SetUpNonstandardBoard(chessInterface.Controller.Board);

            chessInterface.MovePiece((7, 4), (7, 3));
            Assert.True(chessInterface.Controller.Board.Winner == GameResultEnum.Black);
        }

        [Fact]
        public void AIShouldMakeCheckMateInTwo()
        {
            var chessInterface = new DummyBoardInterface();
            chessInterface.NewNonStandardGame(PieceColor.Black);

            chessInterface.AddPiece(new Rook(PieceColor.Black, (0, 0), chessInterface.Controller.Board));
            chessInterface.AddPiece(new Rook(PieceColor.Black, (0, 7), chessInterface.Controller.Board));

            var ai = AITestHelper.GetChessAI(chessInterface.Controller);
            ai.SetUpNonstandardBoard(chessInterface.Controller.Board);

            chessInterface.MovePiece((7, 4), (7, 3));
            King king = chessInterface.Controller.Board.Kings[PieceColor.White];
            chessInterface.MovePiece(king.Square, king.GetPossibleMoves().First());

            Assert.True(chessInterface.Controller.Board.Winner == GameResultEnum.Black);
        }
    }
}
