using Chess.Application.Enums;
using System;
using System.Windows.Forms;

namespace Chess.ChessUI
{
    /// <summary>
    /// Used to provide a means of selecting a new piece after pawn promotion.
    /// </summary>
    public partial class PawnPromotionGUI : Form
    {
        internal PromotedPiece PromotedPiece { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="PawnPromotionGUI"/>
        /// </summary>
        public PawnPromotionGUI()
        {
            InitializeComponent();
            PromotedPiece = PromotedPiece.None;

            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;
            button4.Click += Button4_Click;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            PromotedPiece = PromotedPiece.Knight;
            Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            PromotedPiece = PromotedPiece.Bishop;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            PromotedPiece = PromotedPiece.Rook;
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            PromotedPiece = PromotedPiece.Queen;
            Close();
        }
    }
}
