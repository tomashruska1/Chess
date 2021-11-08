using Chess.Application.BoardControllers;
using Chess.Application.Boards;
using Chess.Application.Enums;
using Chess.Application.Pieces;

namespace Chess.Test.MockLibrary;

public class BoardWithDirectPieceSet : Board
{
    public BoardWithDirectPieceSet(IPawnPromoter pawnPromoter) : base(pawnPromoter)
    {
        Kings[PieceColor.Black] = new King(PieceColor.Black, (0, 4), this);
        Kings[PieceColor.White] = new King(PieceColor.White, (7, 4), this);

        LivePieces.Add(PieceColor.Black, new List<Piece> { Kings[PieceColor.Black] });
        LivePieces.Add(PieceColor.White, new List<Piece> { Kings[PieceColor.White] });

        this[Kings[PieceColor.Black].Square] = Kings[PieceColor.Black];
        this[Kings[PieceColor.White].Square] = Kings[PieceColor.White];
    }

    public void SetSquareForPiece(Piece piece, Square square)
    {
        this[piece.Square] = null;
        ChessBoard[square.Row, square.Column] = piece;
        piece.Square = square;
    }

    public void SetNextMove(PieceColor color)
    {
        NextMove = color;
    }
}
