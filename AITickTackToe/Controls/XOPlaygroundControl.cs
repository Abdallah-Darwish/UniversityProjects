using AITickTackToe.AI;
using AITickTackToe.AI.Rendering;
using AITickTackToe.TickTackToeGame.Rendering;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITickTackToe.Controls
{
    //Can't and won't detect ties
    /// <summary>
    /// A control used to render game playground and handles user/player interaction.
    /// </summary>
    public class XOPlaygroundControl : Control
    {
        private int _version = -1;
        ///<summary>
        ///Used when both players are in moving state to determine who is turn to move.
        ///If its even then first player and second if odd.
        ///</summary>
        public int Version
        {
            get => _version;
            set
            {
                if (!SetAndRaise(VersionProperty, ref _version, value)) { return; }
                HighlightedCell = (-1, -1);
                CalcIsFirstPlayerTurn();
            }
        }
        public static readonly DirectProperty<XOPlaygroundControl, int> VersionProperty = AvaloniaProperty.RegisterDirect<XOPlaygroundControl, int>(nameof(Version), o => o.Version, (o, v) => o.Version = v);
        private Playground _value;
        /// <summary>
        /// The playground to render.
        /// </summary>
        public Playground Value
        {
            get => _value;
            set
            {
                if (!SetAndRaise(ValueProperty, ref _value, value)) { return; }
                IsGameDone = Value.Winner != Playground.Empty;
                Version++;
                CalcIsFirstPlayerTurn();
            }
        }
        private void CalcIsFirstPlayerTurn()
        {
            if (IsGameDone) { IsFirstPlayerTurn = false; }
            else if (Value.Count(FirstPlayerChar) < Value.Count(SecondPlayerChar))
            {
                IsFirstPlayerTurn = !Value.InMovingState(FirstPlayerChar);
            }
            else if (Value.Count(FirstPlayerChar) > Value.Count(SecondPlayerChar))
            {
                IsFirstPlayerTurn = Value.InMovingState(SecondPlayerChar);
            }
            else
            {
                IsFirstPlayerTurn = !Value.InMovingState(FirstPlayerChar) || !Value.InMovingState(SecondPlayerChar) || Version % 2 == 0;
            }
        }
        public static readonly DirectProperty<XOPlaygroundControl, Playground> ValueProperty =
        AvaloniaProperty.RegisterDirect<XOPlaygroundControl, Playground>(nameof(Value), o => o.Value, (o, v) => o.Value = v);
        private SimplePlaygroundRenderer _renderer;

        private PlaygroundRenderingConfig _renderingConfig;
        public PlaygroundRenderingConfig RenderingConfig
        {
            get => _renderingConfig;
            set
            {
                if (!SetAndRaise(RenderingConfigProperty, ref _renderingConfig, value)) { return; }
                _renderer = new SimplePlaygroundRenderer { Config = _renderingConfig };
                if (HighlightedCell.Row != -1)
                {
                    _renderer.IsHighlightedCell[_highlightedCell.Row][_highlightedCell.Col] = true;
                }
            }
        }
        public static readonly DirectProperty<XOPlaygroundControl, PlaygroundRenderingConfig> RenderingConfigProperty =
        AvaloniaProperty.RegisterDirect<XOPlaygroundControl, PlaygroundRenderingConfig>(nameof(Playground), o => o.RenderingConfig, (o, v) => o._renderingConfig = v);

        public Thickness Padding
        {
            get => GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        public static readonly StyledProperty<Thickness> PaddingProperty =
        AvaloniaProperty.Register<XOPlaygroundControl, Thickness>(nameof(Padding));
        /// <summary>
        /// <see cref="IPen"/> used to draw a line over winner 3 characters
        public IPen WinnerCrossLinePen
        {
            get => GetValue(WinnerCrossLinePenProperty);
            set => SetValue(WinnerCrossLinePenProperty, value);
        }
        public static readonly StyledProperty<IPen> WinnerCrossLinePenProperty = AvaloniaProperty.Register<XOPlaygroundControl, IPen>(nameof(WinnerCrossLinePen), new Pen(Brushes.Violet));

        private bool _isFirstPlayerTurn = true;
        /// <remarks>
        /// Can't be updated directly instead update <see cref="Version"/>.
        /// </remarks>
        public bool IsFirstPlayerTurn
        {
            get => _isFirstPlayerTurn;
            private set => SetAndRaise(IsFirstPlayerTurnProperty, ref _isFirstPlayerTurn, value);
        }
        public static readonly DirectProperty<XOPlaygroundControl, bool> IsFirstPlayerTurnProperty = AvaloniaProperty.RegisterDirect<XOPlaygroundControl, bool>
            (nameof(IsFirstPlayerTurn), o => o.IsFirstPlayerTurn);
        private char _firstPlayerChar = 'x';
        public char FirstPlayerChar
        {
            get => _firstPlayerChar;
            set
            {
                if(value != 'x' || value != 'o')
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Players chars can only be either 'x' or 'o'.");
                }
                if (SetAndRaise(FirstPlayerCharProperty, ref _firstPlayerChar, value))
                {
                    SecondPlayerChar = value == 'x' ? 'o' : 'x';
                    CalcIsFirstPlayerTurn();
                }
            }
        }

        public static readonly DirectProperty<XOPlaygroundControl, char> FirstPlayerCharProperty = AvaloniaProperty.RegisterDirect<XOPlaygroundControl, char>
                   (nameof(FirstPlayerChar), o => o.FirstPlayerChar, (o, v) => o.FirstPlayerChar = v);

        private char _secondPlayerChar = 'o';
        public char SecondPlayerChar
        {
            get => _secondPlayerChar;
            set
            {
                if(value != 'x' || value != 'o')
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Players chars can only be either 'x' or 'o'.");
                }
                if (SetAndRaise(SecondPlayerCharProperty, ref _secondPlayerChar, value))
                {
                    FirstPlayerChar = value == 'x' ? 'o' : 'x';
                }
            }
        }
        public static readonly DirectProperty<XOPlaygroundControl, char> SecondPlayerCharProperty = AvaloniaProperty.RegisterDirect<XOPlaygroundControl, char>
                   (nameof(SecondPlayerChar), o => o.SecondPlayerChar);
        public static readonly DirectProperty<XOPlaygroundControl, bool> IsGameDoneProperty = AvaloniaProperty.RegisterDirect<XOPlaygroundControl, bool>
                    (nameof(IsGameDone), o => o.IsGameDone);
        private bool _isGameDone = false;
        public bool IsGameDone
        {
            get => _isGameDone;
            private set => SetAndRaise(IsGameDoneProperty, ref _isGameDone, value);
        }
        /// <summary>
        /// Values used to scale a point on the grid to my space.
        /// </summary>
        private Size GetToMeScale() => new(Bounds.Width / _renderer!.Config.GridSize.Width, Bounds.Height / _renderer!.Config.GridSize.Height);
         /// <summary>
        /// Values used to scale a point on me to the grid or playground.
        /// </summary>
        private Size GetToPlaygroundScale() => new Size(_renderer!.Config.GridSize.Width / Bounds.Width, _renderer!.Config.GridSize.Height / Bounds.Height);
        /// <summary>
        /// Finds coordinates of the first cell <paramref name="p"/> lies inside.
        /// </summary>
        private (int Row, int Col) GetCellInPoint(Point p)
        {
            p = p.Scale(GetToPlaygroundScale());
            //cells location can be cached for better performance
            for (int r = 0; r < Playground.Length; r++)
            {
                for (int c = 0; c < Playground.Length; c++)
                {
                    Rect cellRect = new(_renderer!.Config.GetCellLocation(r, c), _renderer!.Config.CellSize);
                    if (cellRect.Contains(p)) { return (r, c); }
                }
            }
            return (-1, -1);
        }
        
        private (int Row, int Col) _highlightedCell = (-1, -1);
        /// <summary>
        /// Coordinates of the cell user have clicked.
        /// (-1, -1) if no cell is highlighted.
        /// If value Row or column is invalid then it will be set to (-1, -1) cell will be highlighted.
        /// </summary>
        private (int Row, int Col) HighlightedCell
        {
            get { return _highlightedCell; }
            set
            {
                if (_highlightedCell.Equals(value)) { return; }
                if (value.Row < 0 || value.Row >= Playground.Length || value.Col < 0 || value.Col >= Playground.Length) { value.Row = -1; }
                if (_highlightedCell.Row != -1 && _renderer != null)
                {
                    _renderer.IsHighlightedCell[_highlightedCell.Row][_highlightedCell.Col] = false;
                }
                if (value.Row != -1 && _renderer != null)
                {
                    _renderer.IsHighlightedCell[value.Row][value.Col] = true;
                }

                SetAndRaise(HighlightedCellProperty, ref _highlightedCell, value);
            }
        }
        private static readonly DirectProperty<XOPlaygroundControl, (int, int)> HighlightedCellProperty = AvaloniaProperty.RegisterDirect<XOPlaygroundControl, (int, int)>
                 (nameof(_highlightedCell), o => o._highlightedCell, (o, v) => o.HighlightedCell = v);

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (IsGameDone) { return; }
            var p = e.GetCurrentPoint(this);
            var cell = GetCellInPoint(p.Position);
            if (cell.Row == -1)
            {
                HighlightedCell = (-1, -1);
                return;
            }
            //current player char
            char c = IsFirstPlayerTurn ? FirstPlayerChar : SecondPlayerChar;
            if (Value[cell.Row, cell.Col] == Playground.Empty)
            {
                if (!Value.InMovingState(c))
                {
                    Value = Value.Set(cell.Row, cell.Col, c);
                }
                else if (HighlightedCell.Row != -1 && (Math.Abs(_highlightedCell.Row - cell.Row) + Math.Abs(_highlightedCell.Col - cell.Col)) <= 1)
                {
                    Value = Value.Move(HighlightedCell.Row, HighlightedCell.Col, cell.Row, cell.Col);
                }
                HighlightedCell = (-1, -1);
            }
            else if (Value[cell.Row, cell.Col] == c && Value.InMovingState(c))
            {
                HighlightedCell = cell;
            }
            else
            {
                HighlightedCell = (-1, -1);
            }
        }
        public override void Render(DrawingContext ctx)
        {
            base.Render(ctx);
            using var img = new RenderTargetBitmap(new PixelSize((int)RenderingConfig.GridSize.Width, (int)RenderingConfig.GridSize.Height));
            using (var imgCtx = new DrawingContext(img.CreateDrawingContext(null), true))
            {
                _renderer.Draw(imgCtx, Value);
                if (IsGameDone)
                {
                    //draw the winning cross line
                    Point offset = new(RenderingConfig.CellSize.Width / 2, RenderingConfig.CellSize.Height / 2);
                    var start = RenderingConfig.GetCellLocation(Value.WinningLine.Start.Row, Value.WinningLine.Start.Col) + offset;
                    var end = RenderingConfig.GetCellLocation(Value.WinningLine.End.Row, Value.WinningLine.End.Col) + offset;
                    imgCtx.DrawLine(WinnerCrossLinePen, start, end);
                }
            }
            var dstRect = Bounds.Deflate(Padding).WithX(0).WithY(0).Deflate(Padding);
            ctx.DrawImage(img, 1.0, new Rect(default, RenderingConfig.GridSize), dstRect);
        }
        static XOPlaygroundControl()
        {
            AffectsRender<XOPlaygroundControl>(RenderingConfigProperty, HighlightedCellProperty, PaddingProperty, FirstPlayerCharProperty, SecondPlayerCharProperty, WinnerCrossLinePenProperty, IsGameDoneProperty, ValueProperty);
        }
        public XOPlaygroundControl()
        {
            RenderingConfig = new PlaygroundRenderingConfig();
            Value = new Playground();
            Version = 0;
        }
    }
}
