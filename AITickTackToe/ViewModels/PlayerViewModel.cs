using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AITickTackToe.AI;
using AITickTackToe.AI.Engine;
using AITickTackToe.Controls;
using AITickTackToe.TickTackToeGame;
using AITickTackToe.TickTackToeGame.AI;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AITickTackToe.ViewModels
{
    public class PlayerViewModel : ReactiveObject, IDisposable
    {
        private bool _disposedValue;
        /// <summary>
        /// The game playground or state this player will modify.
        /// It will be monitored by <see cref="MainWindowViewModel"/>.
        /// </summary>
        [Reactive]
        public Playground CurrentGame { get; set; }
        //Used by the view
        public char CapitalMyChar {get; private set;}
        private char _myChar;
        /// <summary>
        /// This player char to use in his label.
        /// </summary>
        public char MyChar
        {
            get => _myChar;
            init
            {
                _myChar = value;
                CapitalMyChar = char.ToUpperInvariant(value);
                Expander = new PlaygroundExpander { MyChar = value };
                Evaluator = new PlaygroundEvaluator { MyChar = value };
            }
        }
        /// <summary>
        /// <see cref="IBrush"/> used to render this player label char brush IN HIS TURN.
        /// </summary>
        public IBrush MyCharBrush { get; init; }
        /// <summary>
        /// <see cref="IBrush"/> used to render this player label char brush.
        /// It will be <see cref="MyCharBrush"/> in his turn and <see cref="Brushes.Black"> otherwise.
        /// </summary>
        public IBrush MyBrush => IsMyTurn ? MyCharBrush : Brushes.Black;
        [Reactive]
        public bool IsMyTurn { get; set; }
        /// <summary>
        /// Whether this player will use AI to determine his next move, or its a human player.
        /// </summary>
        [Reactive]
        public bool IsAutoPlayer { get; set; }
        /// <summary>
        /// Height of the AI tree this player will use to determine his best next move.
        /// </summary>
        /// <remarks> Must be >= 1. </remarks>
        [Reactive]
        public int AITreeHeight { get; set; }
        /// <summary>
        /// Delay before finding and applying the best move for this palyer using AI in case <see cref="IsAutoPlayer"/> is <see langword="true"/>.
        /// </summary>
        [Reactive]
        public int AIDelay { get; set; }
        public PlaygroundEvaluator Evaluator { get; private set;}
        public PlaygroundExpander Expander { get; private set;}
        /// <summary>
        /// Find and apply best move using AI.
        /// </summary>
        public void Play()
        {
            var dn = new DecisionNode<Playground>(CurrentGame, Evaluator.Evaluate(CurrentGame));
            dn.Expand(Expander, Evaluator, AITreeHeight);
            CurrentGame = (dn.BestSon ?? dn).Value;
        }
        /// <summary>
        /// Holds reference to observable subscriptions so GC won't dispose them.
        private readonly IDisposable[] _subs;
        

        public PlayerViewModel()
        {
            _subs = new IDisposable[]
            {
                this
                .WhenAny(x => x.IsAutoPlayer, x => x.IsMyTurn, (p1, p2) => p1.Value && p2.Value)
                .Where(x => x)
                .ObserveOn(AvaloniaScheduler.Instance)
                .ForEachAsync(async _ =>
                {
                    await Task.Delay(AIDelay);
                    Play();
                })
                .ToObservable()
                .Subscribe(),
                this
                .WhenChanged(x => x.IsMyTurn, (_, p) => p)
                .Subscribe(_ => this.RaisePropertyChanged(nameof(MyBrush)))
            };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                foreach (var sub in _subs)
                {
                    sub.Dispose();
                }
                _disposedValue = true;
            }
        }

        ~PlayerViewModel()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}