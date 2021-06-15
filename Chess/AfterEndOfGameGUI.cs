using System;
using System.Drawing;
using System.Windows.Forms;
using Chess.Pieces;

namespace Chess
{
    /// <summary>
    /// Provides a means of selecting an action after the end of each game.
    /// </summary>
    public partial class AfterEndOfGameGUI : Form
    {
        internal AfterEndOfGameAction Action { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="AfterEndOfGameGUI"/>
        /// </summary>
        /// <param name="message"></param>
        public AfterEndOfGameGUI(string message)
        {
            InitializeComponent();

            Action = AfterEndOfGameAction.None;

            label1.Text = message;
            label1.Location = new Point((Width - label1.Width) / 2, label1.Location.Y);

            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Action = AfterEndOfGameAction.ExportMoves;
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Action = AfterEndOfGameAction.NewGame;
            Close();
        }
    }

    /// <summary>
    /// Used with <see cref="AfterEndOfGameGUI"/>
    /// </summary>
    internal enum AfterEndOfGameAction
    {
        /// <summary>
        /// Indicates the user has chosen to play a new game.
        /// </summary>
        NewGame,
        /// <summary>
        /// Indicates the user wishes to export the moves to a text file.
        /// </summary>
        ExportMoves,
        /// <summary>
        /// Indicates the user has not yet selected what to do next.
        /// </summary>
        None
    }
}
