using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Chess.Pieces;
using Chess.Board;

namespace Chess
{
    /// <summary>
    /// Provides the GUI for the chess board, handles user input and passes it to underlying objects.
    /// </summary>
    public partial class Chess : Form
    {
        /// <summary>
        /// Keeps a reference to the cell last clicked by the user.
        /// </summary>
        private DataGridViewCell ActiveCell { get; set; } = null;
        /// <summary>
        /// A list of coordinates for chessboard squares that are being highlighted showing possible moves for the piecen in the <see cref="ActiveCell"/>.
        /// </summary>
        private List<(int, int)> HighlightedCells { get; set; }
        /// <summary>
        /// A dictionary storing the original color of each square on the chessboard.
        /// </summary>
        private Dictionary<(int, int), Color> Colors { get; set; }
        /// <summary>
        /// A reference to the object storing the pieces and making all calculations.
        /// </summary>
        private IBoard Board { get; set; }
        /// <summary>
        /// Boolean property used after the game has finished to prevent other methods from finishing their work due to changed circumstances.
        /// </summary>
        private bool Finished { get; set; } = false;

        /// <summary>
        /// Creates an instance of the Chess class.
        /// </summary>
        public Chess()
        {
            InitializeComponent();
            startNewLocalGameToolStripMenuItem.Click += StartNewLocalGameToolStripMenuItem_Click;
            HighlightedCells = new();
            Colors = new();
            CreateBoard();
            dataGridView1.Rows[0].Cells[0].Selected = true;
            dataGridView1.Rows[0].Cells[0].Selected = false;
        }

        private void StartNewLocalGameToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SetUp();
        }

        /// <summary>
        /// Handles the end-of-game case when one player defeats the other.
        /// </summary>
        /// <param name="color"></param>
        public void Winner(PieceColor color)
        {
            EndOfGame(color);
        }

        /// <summary>
        /// Handles the end-of-game scenario when the game can no longer progress.
        /// </summary>
        public void Draw()
        {
            EndOfGame(null);
        }

        /// <summary>
        /// Provides the visual feedback to the user based on how the game ended.
        /// </summary>
        /// <param name="color"></param>
        private void EndOfGame(PieceColor? color)
        {
            ClearHighlighted();
            ActiveCell.Selected = false;
            ActiveCell = null;
            string message;

            if (color is null)
                message = "Draw!";
            else
                message = $"{color} wins! Congratulations!";

            AfterEndOfGameGUI messageBox = new(message);
            messageBox.StartPosition = FormStartPosition.CenterParent;
            messageBox.ShowDialog();
            Finished = true;

            if (messageBox.Action == AfterEndOfGameAction.NewGame)
                SetUp();
            else if (messageBox.Action == AfterEndOfGameAction.ExportMoves)
            {
                Board.Moves.ExportToText();

                DialogResult result = MessageBox.Show("Success! Play again?", "", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    SetUp();
            }

            messageBox.Dispose();
        }

        /// <summary>
        /// Creates an instance of a class implemeenting the <see cref="IBoard"/> interface, starting a game.
        /// </summary>
        private void SetUp()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Value = null;
                }
            }
            Board = new WinFormsBoard(dataGridView1, this);
            AdjustCellFontColor();

            dataGridView1.Rows[0].Cells[0].Selected = true;
            dataGridView1.Rows[0].Cells[0].Selected = false;
        }

        /// <summary>
        /// Creates a visual representation of a chessboard.
        /// </summary>
        private void CreateBoard()
        {

            dataGridView1.Rows.Add(8);
            bool white = true;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Height = 100;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.ValueType = typeof(string);

                    if (white)
                    {
                        cell.Style.BackColor = Color.White;
                        cell.Style.ForeColor = Color.Black;
                        Colors.Add((cell.ColumnIndex, cell.RowIndex), Color.White);
                        white = false;
                    }
                    else
                    {
                        cell.Style.BackColor = Color.Black;
                        cell.Style.ForeColor = Color.White;
                        Colors.Add((cell.ColumnIndex, cell.RowIndex), Color.Black);
                        white = true;
                    }
                }

                if (white)
                    white = false;
                else
                    white = true;

            }

            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
            dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }

        /// <summary>
        /// Handles the user clicking on a square.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (HighlightedCells.Contains((e.RowIndex, e.ColumnIndex)))
            {
                MovePiece(e);
                return;
            }

            if (ActiveCell is not null)
            {
                if (ActiveCell.RowIndex == e.RowIndex && ActiveCell.ColumnIndex == e.ColumnIndex)
                {
                    ClearHighlighted();
                    ActiveCell.Selected = false;
                    ActiveCell = null;
                    return;
                }
                ResetCell(ActiveCell);
            }

            ClearHighlighted();

            if (dataGridView1[e.ColumnIndex, e.RowIndex].Value is null)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Selected = false;
                ActiveCell = null;
                return;
            }

            if (Board[e.RowIndex, e.ColumnIndex]?.Color != Board.NextMove)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Selected = false;
                return;
            }


            ActiveCell = dataGridView1[e.ColumnIndex, e.RowIndex];

            HighlightedCells = Board.LegalMovesForPiece(e.RowIndex, e.ColumnIndex);

            if (highlightPossibleMovesToolStripMenuItem.Checked)
                foreach (var tuple in HighlightedCells)
                {
                    if (dataGridView1[tuple.Item2, tuple.Item1].Value is not null)
                    {
                        dataGridView1[tuple.Item2, tuple.Item1].Style.BackColor = Color.Red;
                    }
                    else if (Colors[(tuple.Item2, tuple.Item1)] == Color.White)
                    {
                        dataGridView1[tuple.Item2, tuple.Item1].Style.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        dataGridView1[tuple.Item2, tuple.Item1].Style.BackColor = Color.Green;
                    }
                }
        }

        /// <summary>
        /// Passes the user input to <see cref="IBoard"/> if it would result in a movement of a piece, adjusts the font color of the square if the piece is black to
        /// more easily distinguish between the colors of the pieces.
        /// </summary>
        /// <param name="e"></param>
        private void MovePiece(DataGridViewCellEventArgs e)
        {
            Board.MovePiece(Board[ActiveCell.RowIndex, ActiveCell.ColumnIndex], e.RowIndex, e.ColumnIndex);

            if (Finished)
            {
                Finished = false;
                return;
            }

            if (Colors[(e.ColumnIndex, e.RowIndex)] == Color.Black)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor =
                    Board[e.RowIndex, e.ColumnIndex].Color == PieceColor.Black ? Color.DimGray : Color.White;
            }

            ClearHighlighted();
            dataGridView1[e.ColumnIndex, e.RowIndex].Selected = false;
            ActiveCell = null;
            return;
        }

        /// <summary>
        /// Handles moving a piece on the chessboard.
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="endRow"></param>
        /// <param name="endColumn"></param>
        internal void SetPiecePosition(int startRow, int startColumn, int endRow, int endColumn)
        {
            dataGridView1[endColumn, endRow].Value = Board[startRow, startColumn].UnicodeValue;
            dataGridView1[startColumn, startRow].Value = null;
        }

        /// <summary>
        /// Adjusts the font color for all squares to more easily distinguis between the colors of the pieces.
        /// </summary>
        private void AdjustCellFontColor()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value is null)
                        continue;

                    if (Board[cell.RowIndex, cell.ColumnIndex].Color == PieceColor.Black &&
                        Colors[(cell.ColumnIndex, cell.RowIndex)] == Color.Black)
                    {
                        cell.Style.ForeColor = Color.DimGray;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the promotion of a pawn - creates a GUI where the user selects the desired piece and passes this information to <see cref="IBoard"/>.
        /// </summary>
        /// <returns>The type of the piece selected by the user.</returns>
        internal PromotedPiece PromotePawn()
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
            foreach (var tuple in HighlightedCells)
            {
                ResetCell(dataGridView1[tuple.Item2, tuple.Item1]);
            }
            HighlightedCells.Clear();
        }

        /// <summary>
        /// Restores the color of a single cell to its original color.
        /// </summary>
        private void ResetCell(DataGridViewCell cell)
        {
            cell.Style.BackColor = Colors[(cell.ColumnIndex, cell.RowIndex)];
        }
    }
}
