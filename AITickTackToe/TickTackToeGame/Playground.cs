using System;
using System.Collections.Generic;

namespace AITickTackToe
{
    /// <summary>
    /// Holds the state of a tick tack toe playground and some relevant information like who is the winner.
    /// Read only class.
    /// </summary>
    /// <remarks> Lower case chars are used. </remarks>
    public class Playground
    {
        /// <summary>
        /// Possible movements from one cell to its neighbours.
        /// </summary>
        public static readonly int[][] Movements = new int[4][]
        {
            new int[2] { -1, 0 },
            new int[2] { 1, 0 },
            new int[2] { 0, -1 },
            new int[2] { 0, 1 },
        };
        /// <summary>
        /// Number of rows or columns in playground
        public const int Length = 3;
        ///<summary>
        /// value used to fill empty cells.
        /// </summary>
        public const char Empty = default;
        /// <summary>
        /// Number of cells with x as their value.
        /// </summary>
        public int SetX { get; private set; }
        /// <summary>
        /// Number of cells with o as their value.
        /// </summary>
        public int SetO { get; private set; }
        /// <summary>
        /// Count cells filled with <paramref name="c"/>.
        /// </summary>
        public int Count(char c)
        {
            if (c == 'x') { return SetX; }
            if (c == 'o') { return SetO; }
            return _cells.Length - SetX - SetO;
        }
        private bool _calcedWinner = false;
        private char _winner;
        private ((int Row, int Col), (int Row, int Col)) _winningLine = ((-1, -1), (-1, -1));
        /// <summary>
        /// Char of the winner or <see cref="Empty"/> if no winner.
        /// </summary>
        public char Winner
        {
            get
            {
                CalcWinner();
                return _winner;
            }
        }
        /// <summary>
        /// Start and end of winning line.
        /// </summary>
        public ((int Row, int Col) Start, (int Row, int Col) End) WinningLine
        {
            get
            {
                CalcWinner();
                return _winningLine;
            }
        }
        /// <summary>
        /// Try to find if there is a winner in the current playground and find its winning line.
        /// </summary>
        private void CalcWinner()
        {
            if (_calcedWinner) { return; }
            _calcedWinner = true;
            _winner = Empty;
            if (SetX < 3 && SetO < 3) { return; }
            char z;
            bool f;
            //rows
            for (int r = 0; r < Length; r++)
            {
                z = this[r, 0];
                if (z == Empty) { continue; }
                f = true;
                for (int c = 1; c < Length; c++)
                {
                    f &= this[r, c] == z;
                }
                if (f)
                {
                    _winner = z;
                    _winningLine = ((r, 0), (r, 2));
                    return;
                }
            }

            //cols
            for (int c = 0; c < Length; c++)
            {
                z = this[0, c];
                if (z == Empty) { continue; }
                f = true;

                for (int r = 1; r < Length; r++)
                {
                    f &= this[r, c] == z;
                }
                if (f)
                {
                    _winner = z;
                    _winningLine = ((0, c), (2, c));
                    return;
                }
            }

            //diag 1
            f = true;
            z = this[0, 0];
            if (z != Empty)
            {
                for (int r = 1, c = 1; r < Length; r++)
                {
                    f &= this[r, c] == z;
                    c++;
                }
                if (f)
                {
                    _winner = z;
                    _winningLine = ((0, 0), (2, 2));
                    return;
                }
            }

            //diag 2
            f = true;
            z = this[0, 2];
            if (z != Empty)
            {
                for (int r = 1, c = 1; r < Length; r++)
                {
                    f &= this[r, c] == z;
                    c--;
                }
                if (f)
                {
                    _winner = z;
                    _winningLine = ((0, 2), (2, 0));
                }
            }
        }
        /// <summary>
        /// Checks whether a player with char <paramref name="c"/> can fill new cells or only move old cells.
        /// </summary>
        public bool InMovingState(char c)
        {
            if (c == 'x') { return SetX >= 3; }
            if (c == 'o') { return SetO >= 3; }
            return false;
        }
        private readonly char[] _cells = new char[9];
        public ReadOnlyMemory<char> Cells => new(_cells);

        public char this[int row, int col]
        {
            get { return _cells[row * Length + col]; }
            private set
            {
                ref char c = ref _cells[row * Length + col];
                if (c == value) { return; }
                if (c == 'x') { SetX--; }
                else if (c == 'o') { SetO--; }

                if (value == 'x') { SetX++; }
                else if (value == 'o') { SetO++; }
                c = value;
            }
        }
        /// <summary>
        /// returns a new <see cref="Playground"/> with the specified cell set to <paramref name="value"/>
        public Playground Set(int row, int col, char value)
        {
            var res = Clone();
            res[row, col] = value;
            return res;
        }
        /// <summary>
        /// Moves cell value to its neighbour.
        /// </summary>
        /// <param name="currentRow">Row of the cell you want to move.</param>
        /// <param name="currentCol">Column of the cell you want to move.</param>
        /// <param name="newRow">Row of the cell you want to move the value to, it should be a neighbour.</param>
        /// <param name="newCol">Column of the cell you want to move the value to, it should be a neighbour.</param>
        public Playground Move(int currentRow, int currentCol, int newRow, int newCol)
        {
            /*
            Faster to let the array indexer check only
            if (cr < 0 || cc < 0 || nr < 0 || nc < 0)
            {
                throw new ArgumentOutOfRangeException("", "One of the arguments is negative.");
            }
            if (cr >= Row || cr >= Row || nr >= Row || nc >= Row)
            {
                throw new ArgumentOutOfRangeException("", $"One of the arguments is >= {Row}.");
            }
            */

            if ((Math.Abs(newRow - currentRow) + Math.Abs(newCol - currentCol)) > 1)
            {
                throw new ArgumentException("The new cell is not neighbour to the old cell.");
            }
            if (this[currentRow, currentCol] == Empty)
            {
                throw new InvalidOperationException($"Current Cell(row: {newRow}, col: {newCol}) is empty.");
            }
            if (this[newRow, newCol] != Empty)
            {
                throw new InvalidOperationException($"New Cell(row: {newRow}, col: {newCol}) is not empty.");
            }

            var res = Clone();
            res[newRow, newCol] = res[currentRow, currentCol];
            res[currentRow, currentCol] = Empty;
            return res;
        }
        public Playground Clone()
        {
            var clone = new Playground
            {
                SetX = SetX,
                SetO = SetO
            };
            _cells.CopyTo(clone._cells, 0);
            return clone;
        }
    }
}