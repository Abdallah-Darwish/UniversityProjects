using AITickTackToe.AI.Engine;

namespace AITickTackToe.TickTackToeGame.AI
{
    /// <summary>
    /// Evaluates playgrounds by calculating how many rows, columns and diagonals <see cref="MyChar"/> can fill and win
    /// </summary>
    public class PlaygroundEvaluator : IDecisionNodeEvaluator<Playground>
    {
        private char _myChar = 'x';

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

        private static int Evaluate(Playground pg, char z)
        {
            int weight = 0;
            //flag used to check if filled current row, col or diag
            bool f;
            //rows
            for (int r = 0; r < Playground.Length; r++)
            {
                f = true;
                for (int c = 0; c < Playground.Length; c++)
                {
                    f &= pg[r, c] == z || pg[r, c] == Playground.Empty;
                }
                if (f) { weight++; }
            }

            //cols
            for (int c = 0; c < Playground.Length; c++)
            {
                f = true;
                for (int r = 0; r < Playground.Length; r++)
                {
                    f &= pg[r, c] == z || pg[r, c] == Playground.Empty;
                }
                if (f) { weight++; }
            }

            //diag 1
            f = true;
            for (int r = 0, c = 0; r < Playground.Length; r++)
            {
                f &= pg[r, c] == z || pg[r, c] == Playground.Empty;
                c++;
            }
            if (f) { weight++; }

            //diag 2
            f = true;
            for (int r = 0, c = 2; r < Playground.Length; r++)
            {
                f &= pg[r, c] == z || pg[r, c] == Playground.Empty;
                c--;
            }
            if (f) { weight++; }
            return weight;
        }
        public EvaluationResult Evaluate(Playground pg)
        {
            if (pg.Winner == _myChar)
            {
                return new EvaluationResult
                {
                    Value = EvaluationResult.INF,
                    Comment = $"{_myChar} won"
                };
            }
            if (pg.Winner == _opponentChar)
            {
                return new EvaluationResult
                {
                    Value = -EvaluationResult.INF,
                    Comment = $"{_opponentChar} won"
                };
            }
            int lhs = Evaluate(pg, _myChar),
            rhs = Evaluate(pg, _opponentChar);
            return new EvaluationResult
            {
                Value = lhs - rhs,
                Comment = $"{lhs} - {rhs} = {lhs - rhs}"
            };
        }
    }
}