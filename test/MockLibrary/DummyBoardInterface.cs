using Chess.Application.BoardControllers;
using Chess.Application.Boards;
using Chess.Application.Enums;
using Chess.Application.Pieces;

namespace Chess.Test.MockLibrary;

public class DummyBoardInterface : IChessboardInterface
{
    public IBoardController Controller { get; set; }
    public bool GameEnded { get; set; } = false;

    public void EndOfGame(GameResultEnum endOfGame)
    {
        GameEnded = true;
    }

    public void MovePiece(Square fromSquare, Square toSquare)
    {
        Controller.MovePiece(Controller[fromSquare], toSquare);
    }

    public void NewNonStandardGame()
    {
        Controller = new BoardController(this)
        {
            Board = new BoardWithDirectPieceSet(new PawnPromoter(typeof(Queen)))
        };
    }

    public void NewGame()
    {
        Controller = new BoardController(this);
    }

    public void NewNonStandardGame(PieceColor aiColor)
    {
        Controller = new BoardController(this, aiColor)
        {
            Board = new BoardWithDirectPieceSet(new PawnPromoter(typeof(Queen)))
        };
        Controller.StartGame();
    }

    public void NewGame(PieceColor aiColor)
    {
        Controller = new BoardController(this, aiColor);
        Controller.StartGame();
    }

    public void AddPiece(Piece piece)
    {
        ((BoardWithDirectPieceSet)Controller.Board).SetSquareForPiece(piece, piece.Square);
        Controller.Board.LivePieces[piece.Color].Add(piece);
    }

    public PromotedPiece PromotePawn()
    {
        return PromotedPiece.Queen;
    }

    public void RemovePiece(Square fromSquare) { }

    public void SetPiecePosition(Square fromSquare, Square toSquare) { }
}
