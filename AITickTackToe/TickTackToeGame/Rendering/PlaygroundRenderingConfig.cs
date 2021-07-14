using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using AITickTackToe.AI.Rendering;
namespace AITickTackToe.TickTackToeGame.Rendering
{
    public class PlaygroundRenderingConfig : IDecisionNodeValueConfig<Playground>
    {
        private Typeface _xoTypeface = Typeface.Default;
        private IPen _gridPen = new Pen(Brushes.Black);
        /// <summary>
        /// <see cref="IBrush"/> used to draw higlighted cells values.
        /// </summary>
        public IBrush HighlightBrush { get; init; } = Brushes.DarkCyan;
        /// <summary>
        /// <see cref="IBrush"/> used to draw NOT higlighted x cells values.
        /// </summary>
        public IBrush XBrush { get; init; } = Brushes.Blue;
        /// <summary>
        /// <see cref="IBrush"/> used to draw NOT higlighted o cells values.
        /// </summary>
        public IBrush OBrush { get; init; } = Brushes.Red;
        public Typeface XOTypeface
        {
            get => _xoTypeface;
            init
            {
                _xoTypeface = value;
                CalcGridSize();
            }
        }
        public IPen GridPen
        {
            get => _gridPen;
            init
            {
                _gridPen = value;
                CalcGridSize();
            }
        }
        public Size GridSize { get; private set; }
        public Size CellSize { get; private set; }
        private void CalcGridSize()
        {
            var txt = new FormattedText()
            {
                Text = "O",
                TextAlignment = TextAlignment.Center,
                Typeface = _xoTypeface
            };
            CellSize = txt.Bounds.Size;
            var gridWidth = (_gridPen.Thickness * 2) + (CellSize.Width * 3);
            var gridHeight = (_gridPen.Thickness * 2) + (CellSize.Height * 3);
            GridSize = new Size(gridWidth, gridHeight);
        }
        /// <summary>
        /// Computes start and end of vertical line that is used to seperate playground columns.
        /// </summary>
        public (Point Start, Point End) GetVerticalLine(int i, Point gridLocation = default)
        {
            Point p0 = new((_gridPen.Thickness + CellSize.Width) * i + CellSize.Width + _gridPen.Thickness / 2 + gridLocation.X, gridLocation.Y);
            return (p0, new Point(p0.X, p0.Y + GridSize.Height));
        }

        /// <summary>
        /// Computes start and end of horizontal line that is used to seperate playground rows.
        /// </summary>
        public (Point Start, Point End) GetHorizontalLine(int i, Point gridLocation = default)
        {
            Point p0 = new(gridLocation.X, (_gridPen.Thickness + CellSize.Height) * i + CellSize.Height + _gridPen.Thickness / 2 + gridLocation.Y);
            return (p0, new Point(p0.X + GridSize.Width, p0.Y));
        }
        /// <summary>
        /// Computes top left point of the cell at given coordinates.
        /// </summary>
        public Point GetCellLocation(int r, int c, Point gridLocation = default)
        {
            var p = new Point(gridLocation.X + (c * (_gridPen.Thickness + CellSize.Width)), gridLocation.Y + (r * (_gridPen.Thickness + CellSize.Height)));
            return p;
        }
        /// <inheritdoc/>
        public Size CalcValueSize(Playground node) => GridSize;

        public PlaygroundRenderingConfig()
        {
            CalcGridSize();
        }
    }
}