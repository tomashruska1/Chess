using Chess.Application.BoardControllers;
using Chess.Application.Boards;
using Chess.Application.ChessAIs;
using Chess.Application.Enums;
using Chess.Application.Pieces;
using Chess.Test.MockLibrary;
using System.Reflection;


namespace Chess.Test.ChessAITest;

public static class AITestHelper
{
    public static Pawn GetPawn(PieceColor pieceColor, Square square, DummyBoardInterface chessInterface)
    {
        Pawn pawn = new(pieceColor, square, chessInterface.Controller.Board);

        FieldInfo fieldInfo = typeof(Piece).GetField("<HasMoved>k__BackingField", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        fieldInfo.SetValue(pawn, true);

        return pawn;
    }

    public static IChessAI GetChessAI(IBoardController Controller)
    {
        FieldInfo fieldInfo = Controller.GetType().GetField("<ChessAI>k__BackingField", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        return (IChessAI)fieldInfo.GetValue(Controller);
    }

    public static IBoard GetAIBoard(IChessAI chessAI)
    {
        FieldInfo fieldInfo = chessAI.GetType().GetField("<Board>k__BackingField", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        return (IBoard)fieldInfo.GetValue(chessAI);
    }

    public static void AddFourBishops(DummyBoardInterface chessInterface)
    {
        chessInterface.AddPiece(new Bishop(PieceColor.White, (7, 5), chessInterface.Controller.Board));
        chessInterface.AddPiece(new Bishop(PieceColor.White, (7, 2), chessInterface.Controller.Board));
        chessInterface.AddPiece(new Bishop(PieceColor.Black, (1, 5), chessInterface.Controller.Board));
        chessInterface.AddPiece(new Bishop(PieceColor.Black, (1, 2), chessInterface.Controller.Board));
    }
}
