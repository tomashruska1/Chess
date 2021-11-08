namespace Chess.ChessUI.Enums;


/// <summary>
/// Used with <see cref="AfterEndOfGameGUI"/>
/// </summary>
internal enum AfterEndOfGameEnum
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
