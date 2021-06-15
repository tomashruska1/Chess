﻿using System;
using System.IO;
using System.Collections.Generic;
using Chess.Pieces;

namespace Chess
{
    /// <summary>
    /// Provides a means of collecting data about all moves made in a given game,
    /// optionally may export this into a text file formatted as standard notation.
    /// </summary>
    public class Moves
    {
        private List<Move> _Moves { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="Moves"/>
        /// </summary>
        public Moves()
        {
            _Moves = new();
        }

        /// <summary>
        /// Add Move struct to a collection of moves.
        /// </summary>
        /// <remarks>
        /// See <see cref="Move"/>
        /// </remarks>
        /// <param name="move"></param>
        public void Add(Move move)
        {
            _Moves.Add(move);
        }

        /// <summary>
        /// Iterates through collection of moves, translates it to standard notation,
        /// and appends it to a text file "moves.txt".
        /// </summary>
        public void ExportToText()
        {
            int n = 0;
            int moveNo = 1;

            using StreamWriter file = new("moves.txt", true);

            file.WriteLine($"{DateTime.Now}");

            while (n < _Moves.Count)
            {
                file.Write($"{moveNo}.");

                for (int x = 0; x < 2; x++)
                {
                    if (n + x >= _Moves.Count)
                        continue;

                    file.Write(" ");

                    Move move = _Moves[n + x];

                    if ((move.Type | MoveType.KingsideCastling) == move.Type)
                    {
                        file.Write("0-0");
                        continue;
                    }
                    else if ((move.Type | MoveType.QueensideCastling) == move.Type)
                    {
                        file.Write("0-0-0");
                        continue;
                    }
                    else if (!move.Piece.GetType().Equals(typeof(Pawn)) && (move.Type | MoveType.PawnPromotion) != move.Type)
                    {
                        file.Write($"{move.Piece.GetType().Name[0]}");
                    }

                    if ((move.Type | MoveType.Capture) == move.Type)
                    {
                        if (move.Piece.GetType().Equals(typeof(Pawn)) || (move.Type | MoveType.PawnPromotion) == move.Type)
                        {
                            file.Write($"{(char)('a' + move.StartColumn)}");
                        }
                        file.Write("\u2a2f");
                    }

                    file.Write($"{(char)('a' + move.Column)}{8 - move.Row}");

                    if ((move.Type | MoveType.PawnPromotion) == move.Type)
                        file.Write($"{move.Piece.GetType().Name[0]}");

                    if ((move.Type | MoveType.EnPassantCapture) == move.Type)
                        file.Write(" e.p.");

                    if ((move.Type | MoveType.Check) == move.Type)
                        file.Write("+");

                    if ((move.Type | MoveType.CheckMate) == move.Type)
                        file.Write("#");
                }
                file.WriteLine("");
                moveNo += 1;
                n += 2;
            }
            file.WriteLine("");
        }
    }
}
