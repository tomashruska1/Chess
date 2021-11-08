namespace Chess.ChessUI;


/// <summary>
/// Provides the GUI for the chess board, handles user input and passes it to underlying objects.
/// </summary>
public partial class WinFormsChessInterface : Form, IChessboardInterface
{
    /// <summary>
    /// Keeps a reference to the cell last clicked by the user.
    /// </summary>
    private DataGridViewCell ActiveCell { get; set; } = null;
    /// <summary>
    /// A list of coordinates for chessboard squares that are being highlighted showing possible moves for the pieces in the <see cref="ActiveCell"/>.
    /// </summary>
    private List<Square> HighlightedCells { get; set; }
    /// <summary>
    /// A dictionary storing the original color of each square on the chessboard.
    /// </summary>
    private Dictionary<Square, Color> Colors { get; set; }
    /// <summary>
    /// A reference to the object storing the pieces and making all calculations.
    /// </summary>
    private IBoardController Controller { get; set; }
    /// <summary>
    /// Boolean property used after the game has finished to prevent other methods from finishing their work due to changed circumstances.
    /// </summary>
    private bool Finished { get; set; } = false;

    /// <summary>
    /// Creates an instance of the Chess class.
    /// </summary>
    public WinFormsChessInterface()
    {
        InitializeComponent();
        startNewLocalGameToolStripMenuItem.Click += StartNewLocalGameToolStripMenuItem_Click;
        playAsWhiteToolStripMenuItem.Click += PlayAsWhiteToolStripMenuItem_Click;
        playAsBlackToolStripMenuItem.Click += PlayAsBlackToolStripMenuItem_Click;

        HighlightedCells = new();
        Colors = new();
        CreateBoard();
        chessboard.Rows[0].Cells[0].Selected = true;
        chessboard.Rows[0].Cells[0].Selected = false;
    }

    private void PlayAsBlackToolStripMenuItem_Click(object sender, EventArgs e)
    {
        NewGame(PieceColor.White);
    }

    private void PlayAsWhiteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        NewGame(PieceColor.Black);
    }

    private void StartNewLocalGameToolStripMenuItem_Click(object sender, System.EventArgs e)
    {
        NewGame();
    }

    /// <summary>
    /// Action taken when a game has ended with the result <paramref name="endOfGame"/>.
    /// </summary>
    /// <param name="endOfGame"></param>
    public void EndOfGame(GameResultEnum endOfGame)
    {
        ClearHighlighted();
        ActiveCell.Selected = false;
        ActiveCell = null;
        string message;

        if (endOfGame == GameResultEnum.Draw)
            message = "Draw!";
        else
            message = $"{endOfGame} wins! Congratulations!";

        AfterEndOfGameGUI messageBox = new(message);
        messageBox.StartPosition = FormStartPosition.CenterParent;
        messageBox.ShowDialog();
        Finished = true;

        if (messageBox.Action == AfterEndOfGameEnum.NewGame)
            NewGame();
        else if (messageBox.Action == AfterEndOfGameEnum.ExportMoves)
        {
            Controller.Moves.Export();

            DialogResult result = MessageBox.Show("Success! Play again?", "", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
                NewGame();
        }

        messageBox.Dispose();
    }

    /// <summary>
    /// Starts a new game of chess for two human players.
    /// </summary>
    public void NewGame()
    {
        ResetBoard();
        Controller = new BoardController(this);
        AdjustCellFontColor();

        chessboard.Rows[0].Cells[0].Selected = true;
        chessboard.Rows[0].Cells[0].Selected = false;

        UpdateChessboard();
    }

    private void ResetBoard()
    {
        foreach (DataGridViewRow row in chessboard.Rows)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Value = null;
            }
        }
    }

    private void UpdateChessboard()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int column = 0; column < 8; column++)
            {
                chessboard[column, row].Value = Controller[(row, column)]?.UnicodeValue;
            }
        }
    }

    /// <summary>
    /// Starts a new game of chess against an AI.
    /// </summary>
    /// <param name="aiColor"></param>
    public void NewGame(PieceColor aiColor)
    {
        ResetBoard();
        Controller = new BoardController(this, aiColor);
        AdjustCellFontColor();

        chessboard.Rows[0].Cells[0].Selected = true;
        chessboard.Rows[0].Cells[0].Selected = false;

        UpdateChessboard();
        Controller.StartGame();
    }

    /// <summary>
    /// Creates a visual representation of a chessboard.
    /// </summary>
    private void CreateBoard()
    {
        chessboard.Rows.Add(8);
        bool white = true;

        foreach (DataGridViewRow row in chessboard.Rows)
        {
            row.Height = 100;

            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.ValueType = typeof(string);

                if (white)
                {
                    cell.Style.BackColor = Color.White;
                    cell.Style.ForeColor = Color.Black;
                    Colors.Add((cell.RowIndex, cell.ColumnIndex), Color.White);
                    white = false;
                }
                else
                {
                    cell.Style.BackColor = Color.Black;
                    cell.Style.ForeColor = Color.White;
                    Colors.Add((cell.RowIndex, cell.ColumnIndex), Color.Black);
                    white = true;
                }
            }

            white = !white;
        }

        chessboard.CellClick += new DataGridViewCellEventHandler(ChessboardSquareClick);
        chessboard.CellDoubleClick += new DataGridViewCellEventHandler(ChessboardSquareClick);
    }

    /// <summary>
    /// Handles the user clicking on a square.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChessboardSquareClick(object sender, DataGridViewCellEventArgs e)
    {
        if (HighlightedCells.Contains((e.RowIndex, e.ColumnIndex)))
        {
            MovePiece((ActiveCell.RowIndex, ActiveCell.ColumnIndex), (e.RowIndex, e.ColumnIndex));
            return;
        }

        ClearHighlighted();
        if (chessboard[e.ColumnIndex, e.RowIndex].Value is null
            || ActiveCell?.RowIndex == e.RowIndex && ActiveCell?.ColumnIndex == e.ColumnIndex)
        {
            if (ActiveCell is not null)
                ActiveCell.Selected = false;
            ActiveCell = null;
            return;
        }
        if (ActiveCell is not null)
            ResetCell(ActiveCell);

        if (Controller[(e.RowIndex, e.ColumnIndex)]?.Color != Controller.NextMove)
        {
            chessboard[e.ColumnIndex, e.RowIndex].Selected = false;
            return;
        }

        ActiveCell = chessboard[e.ColumnIndex, e.RowIndex];

        HighlightedCells = Controller.ValidMovesForPiece((e.RowIndex, e.ColumnIndex)).ToList();

        if (highlightPossibleMovesToolStripMenuItem.Checked)
            HighlightPossibleMoves();
    }

    /// <summary>
    /// Changes color of the squares currently held piece can move to.
    /// </summary>
    private void HighlightPossibleMoves()
    {
        foreach (Square square in HighlightedCells)
        {
            if (chessboard[square.Column, square.Row].Value is not null)
            {
                chessboard[square.Column, square.Row].Style.BackColor = Color.Red;
            }
            else if (Colors[square] == Color.White)
            {
                chessboard[square.Column, square.Row].Style.BackColor = Color.LightGreen;
            }
            else
            {
                chessboard[square.Column, square.Row].Style.BackColor = Color.Green;
            }
        }
    }

    /// <summary>
    /// Passes the user input to <see cref="IBoardController"/> if it would result in a movement of a piece, adjusts the font color of the square if the piece is black to
    /// more easily distinguish between the colors of the pieces.
    /// </summary>
    /// <param name="fromSquare"></param>
    /// <param name="toSquare"></param>
    public void MovePiece(Square fromSquare, Square toSquare)
    {
        ClearHighlighted();
        Controller.MovePiece(Controller[fromSquare], toSquare);

        if (Finished)
        {
            Finished = false;
            return;
        }

        if (Colors[toSquare] == Color.Black)
        {
            chessboard[toSquare.Column, toSquare.Row].Style.ForeColor =
                Controller[toSquare].Color == PieceColor.Black ? Color.DimGray : Color.White;
        }
        chessboard[toSquare.Column, toSquare.Row].Selected = false;
        ActiveCell = null;
        UpdateChessboard();
    }

    /// <summary>
    /// Visually moves a piece on the chessboard.
    /// </summary>
    /// <param name="fromSquare"></param>
    /// <param name="toSquare"></param>
    public void SetPiecePosition(Square fromSquare, Square toSquare)
    {
        chessboard[toSquare.Column, toSquare.Row].Value = Controller[fromSquare].UnicodeValue;
        RemovePiece(fromSquare);
    }

    /// <summary>
    /// Visually moves a piece on the chessboard.
    /// </summary>
    /// <param name="fromSquare"></param>
    public void RemovePiece(Square fromSquare)
    {
        chessboard[fromSquare.Column, fromSquare.Row].Value = null;
    }

    /// <summary>
    /// Adjusts the font color for all squares to more easily distinguish between the colors of the pieces.
    /// </summary>
    private void AdjustCellFontColor()
    {
        foreach (DataGridViewRow row in chessboard.Rows)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value is null)
                    continue;

                if (Controller[(cell.RowIndex, cell.ColumnIndex)].Color == PieceColor.Black &&
                    Colors[(cell.RowIndex, cell.ColumnIndex)] == Color.Black)
                {
                    cell.Style.ForeColor = Color.DimGray;
                }
            }
        }
    }

    /// <summary>
    /// Handles the promotion of a pawn - creates a GUI where the user selects the desired piece and passes this information to <see cref="IBoardController"/>.
    /// </summary>
    /// <returns>The type of the piece selected by the user.</returns>
    public PromotedPiece PromotePawn()
    {
        PawnPromotionGUI form3 = new();
        form3.StartPosition = FormStartPosition.CenterParent;
        form3.ShowDialog();
        PromotedPiece result = form3.PromotedPiece;
        form3.Dispose();
        return result;
    }

    /// <summary>
    /// Restores the color of the highlighted cells to their original color.
    /// </summary>
    private void ClearHighlighted()
    {
        foreach (Square square in HighlightedCells)
        {
            ResetCell(chessboard[square.Column, square.Row]);
        }
        HighlightedCells.Clear();
    }

    /// <summary>
    /// Restores the color of a single cell to its original color.
    /// </summary>
    private void ResetCell(DataGridViewCell cell)
    {
        cell.Style.BackColor = Colors[(cell.RowIndex, cell.ColumnIndex)];
    }
}
