namespace Chess.Application.Boards;


/// <summary>
/// Exception that is thrown when an invalid value is received for pawn promotion.
/// </summary>
public class InvalidPieceException : Exception
{
    /// <summary>
    /// Throws a new <see cref="InvalidPieceException"/>.
    /// </summary>
    public InvalidPieceException() : base()
    {
    }

    /// <summary>
    /// Throws a new <see cref="InvalidPieceException"/> with <paramref name="message"/>.
    /// </summary>
    /// <param name="message"></param>
    public InvalidPieceException(string message) : base(message)
    {
    }

    /// <summary>
    /// Throws a new <see cref="InvalidPieceException"/> with <paramref name="message"/> and <paramref name="innerException"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public InvalidPieceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
