namespace Chess.Application.Records
{
    /// <summary>
    /// Specifies methods for objects used to export moves made during the game.
    /// </summary>
    public interface IMoveRecordExporter
    {
        /// <summary>
        /// Adds <paramref name="move"/> to the collection.
        /// </summary>
        /// <param name="move"></param>
        void Add(MoveRecord move);
        /// <summary>
        /// Exports moves in the collection.
        /// </summary>
        void Export();
    }
}