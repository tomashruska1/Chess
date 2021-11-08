namespace Chess.Application.Enums;


/// <summary>
/// Specifies the direction a vector can have, i.e. either diagonal, or horizontal/vertical, or both.
/// </summary>
[Flags]
public enum MoveVectorDirection
{
    /// <summary>
    /// Vector can have only horizontal or vertical direction.
    /// </summary>
    Regular = 1,
    /// <summary>
    /// Vector can have only diagonal direction.
    /// </summary>
    Diagonal = 2,
    /// <summary>
    /// Vector is not limited in which direction it can have.
    /// </summary>
    All = 3
}
