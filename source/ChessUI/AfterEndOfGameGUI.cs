namespace Chess.ChessUI;


/// <summary>
/// Provides a means of selecting an action after the end of each game.
/// </summary>
public partial class AfterEndOfGameGUI : Form
{
    internal AfterEndOfGameEnum Action { get; private set; }

    /// <summary>
    /// Creates an instance of <see cref="AfterEndOfGameGUI"/>
    /// </summary>
    /// <param name="message"></param>
    public AfterEndOfGameGUI(string message)
    {
        InitializeComponent();

        Action = AfterEndOfGameEnum.None;

        label1.Text = message;
        label1.Location = new Point((Width - label1.Width) / 2, label1.Location.Y);

        button1.Click += Button1_Click;
        button2.Click += Button2_Click;
    }

    private void Button2_Click(object sender, EventArgs e)
    {
        Action = AfterEndOfGameEnum.ExportMoves;
        Close();
    }

    private void Button1_Click(object sender, EventArgs e)
    {
        Action = AfterEndOfGameEnum.NewGame;
        Close();
    }
}
