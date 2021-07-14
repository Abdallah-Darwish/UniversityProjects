using System;
using System.Collections.Generic;
using AITickTackToe.AI.Engine;

namespace AITickTackToe.TickTackToeGame.AI
{
    /// <summary>
    /// Expands TickToeGame playground.
    /// </summary>
    public class PlaygroundExpander : IDecisionNodeExpander<Playground>
    {
        private char _myChar = 'x';
        /// <summary>
        /// Which char to use when creating And nodes(expanding from Or node).
        public char MyChar
        {
            get => _myChar;
            init
            {
                _myChar = value;
                _opponentChar = MyChar == 'x' ? 'o' : 'x';
            }
        }
        private char _opponentChar = 'o';
        /// <summary>
        /// Computes possible states form the given playground.
        /// </summary>
        /// <param name="pg"> The playground to expand. </param>
        /// <param name="type"> Type of the new playground nodes, to determine which char to use in expansion. </param>
        public Memory<Playground> Expand(Playground pg, DecisionNodeType type)
        {
            char currentTurn = type == DecisionNodeType.And ? _myChar : _opponentChar;
            var newStates = new List<Playground>();
            if (pg.InMovingState(currentTurn))
            {
                //If I am in moving state then calculate possible cells to move.
                int nr, nc;
                for (int r = 0; r < Playground.Length; r++)
                {
                    for (int c = 0; c < Playground.Length; c++)
                    {
                        if (pg[r, c] != currentTurn) { continue; }
                        for (int m = 0; m < Playground.Movements.Length; m++)
                        {
                            nr = r + Playground.Movements[m][0];
                            nc = c + Playground.Movements[m][1];
                            if (nr < 0 || nr >= Playground.Length || nc < 0 || nc >= Playground.Length || pg[nr, nc] != Playground.Empty) { continue; }
                            newStates.Add(pg.Move(r, c, nr, nc));
                        }
                    }
                }
            }
            else
            {
                //If I am NOT in moving state then calculate possible cells to fill.
                for (int r = 0; r < Playground.Length; r++)
                {
                    for (int c = 0; c < Playground.Length; c++)
                    {
                        if (pg[r, c] != Playground.Empty) { continue; }
                        newStates.Add(pg.Set(r, c, currentTurn));
                    }
                }
            }
            return newStates.ToArray();
        }
    }
}