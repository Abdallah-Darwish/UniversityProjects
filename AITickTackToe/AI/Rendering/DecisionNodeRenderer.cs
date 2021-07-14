using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Direct2D1.Media;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Skia;
using SkiaSharp;
using AITickTackToe.AI.Engine;

namespace AITickTackToe.AI.Rendering
{
    /// <summary>
    /// Used to calculate hold necessary information to render a <see cref="DecisionNode"/>.
    /// </summary>
    /// <typeparam name="TValue">Type of node value.</typeparam>
    public class DecisionNodeRenderingContext<TValue>
    {
        /// <summary>
        /// Whether this node lies on the path from DecisionRoot to the best DecisionLeaf
        /// </summary>
        public bool IsBest { get; }
        /// <summary>
        /// Bounds of the value of this node
        /// </summary>
        public Rect NodeValueBounds { get; }
        /// <summary>
        /// Bounds of the value, evaluation result and icons
        /// </summary>
        public Rect NodeBounds { get; }
        /// <summary>
        /// Bounds of the sub tree rooted at this node.
        /// </summary>
        public Rect SubTreeBounds { get; }
        public DecisionNode<TValue> Node { get; }
        public ReadOnlyMemory<DecisionNodeRenderingContext<TValue>> Descendants { get; }
        public DecisionNodeRenderingContext<TValue>? Parent { get; }

        /// <summary>
        /// Initialize fileds of this instance and creates descendants.
        /// </summary>
        /// <param name="node">Node this <see cref="DecisionNodeRenderingContext"/> will represent.</param>
        /// <param name="parent">Parent of this context.</param>
        /// <param name="topLeft">Coordinates of this node sub tree top left when rendreding.</param>
        /// <param name="isBest">Whether this node lies on the path from root to best leaf.</param>
        /// <param name="config">Rendering config.</param>
        private DecisionNodeRenderingContext(DecisionNode<TValue> node, DecisionNodeRenderingContext<TValue>? parent, Point topLeft, bool isBest, DecisionNodeRenderingConfig<TValue> config)
        {
            Node = node;
            Parent = parent;
            IsBest = isBest;

            var mySize = config.CalcNodeSize(node);

            var descendants = new DecisionNodeRenderingContext<TValue>[node.Descendants.Length];
            var nodeDes = node.Descendants.Span;
            var sonsTopLeft = topLeft.WithY(topLeft.Y + mySize.Height + config.SpacingBetweenLevels);
            var maxDepth = topLeft.Y + mySize.Height;
            for (int i = 0; i < descendants.Length; i++)
            {
                descendants[i] = new DecisionNodeRenderingContext<TValue>(nodeDes[i], this, sonsTopLeft, IsBest && nodeDes[i].IsBest, config);
                maxDepth = Math.Max(maxDepth, descendants[i].SubTreeBounds.Bottom);
                sonsTopLeft = descendants[i].SubTreeBounds.TopRight + new Point(config.SpacingBetweenNodes/* + (config.SpaceBetweenNodeAndEdge * 2)*/, 0);
            }
            Descendants = descendants;
            double stWidth = descendants.Length == 0 ? mySize.Width : (descendants.Last().SubTreeBounds.Right - topLeft.X);
            SubTreeBounds = new Rect(topLeft, new Size(stWidth, maxDepth - topLeft.Y));

            topLeft = topLeft.WithX(SubTreeBounds.X + (SubTreeBounds.Width - mySize.Width) / 2);

            NodeValueBounds = new Rect(topLeft, config.ValueConfig.CalcValueSize(node.Value));
            NodeBounds = new Rect(topLeft, mySize);
        }
        ///<summary>
        /// Use this construct root <see cref="DecisionNodeRenderingContext"/>.
        ///</summary>
        public DecisionNodeRenderingContext(DecisionNode<TValue> node, DecisionNodeRenderingConfig<TValue> config) : this(node, null, new Point(), true, config)
        { }
    }
    /// <summary>
    /// Instances are not thread safe
    /// </summary>
    /// <typeparam name="TValue">Decision node type.</typeparam>
    public class DecisionNodeRenderer<TValue>
    {
        public DecisionNodeRenderingConfig<TValue> Config { get; }
        public DecisionNodeRenderingContext<TValue> Root { get; }
        public IDecisionNodeValueRenderer<TValue> NodeValueRenderer { get; }
        private DrawingContext? _drawingCtx;
        //drawing ctx transformation will point to ROOT top left
        private Point Draw(DecisionNodeRenderingContext<TValue> nodeCtx)
        {
            if(_drawingCtx == null)
            {
                throw new InvalidOperationException($"Initialize {nameof(_drawingCtx)} first.");
            }
            {
                //1- Draw value
                using var _1 = _drawingCtx.PushPostTransform(Matrix.CreateTranslation(nodeCtx.NodeValueBounds.TopLeft));
                NodeValueRenderer.Draw(_drawingCtx, nodeCtx.Node.Value);

                //2- Draw text
                using var _2 = _drawingCtx.PushPostTransform(Matrix.CreateTranslation(Config.SpaceBetweenNodeValueAndInfo + nodeCtx.NodeValueBounds.Width, 0));
                var nodeInfoText = new FormattedText
                {
                    TextAlignment = TextAlignment.Left,
                    Wrapping = TextWrapping.NoWrap,
                    Text = nodeCtx.Node.Decision.Value.PrintableValue,
                    Typeface = Config.DecisionTypeface
                };
                _drawingCtx.DrawText(Config.DecisionValueBrush, new Point(0, 0), nodeInfoText);
                using var _3 = _drawingCtx.PushPostTransform(Matrix.CreateTranslation(0, nodeInfoText.Bounds.Height + Config.SpaceBetweenInfoSections));

                //3- Draw selection id
                if (nodeCtx.Node.IsSelected && Config.SelectedNodeIdentifier != null)
                {
                    var idBounds = new Rect(new Point(0, 0), Config.SelectedNodeIdentifier.Size);
                    _drawingCtx.DrawImage(Config.SelectedNodeIdentifier, 1.0, idBounds, idBounds);
                    using var _4 = _drawingCtx.PushPostTransform(Matrix.CreateTranslation(0, Config.SelectedNodeIdentifier.Size.Height + Config.SpaceBetweenInfoSections));
                }

                //4- Draw best id
                if (nodeCtx.Node.IsBest && Config.BestNodeIdentifier != null)
                {
                    var idBounds = new Rect(new Point(0, 0), Config.BestNodeIdentifier.Size);
                    _drawingCtx.DrawImage(Config.BestNodeIdentifier, 1.0, idBounds, idBounds);
                    using var _5 = _drawingCtx.PushPostTransform(Matrix.CreateTranslation(0, Config.BestNodeIdentifier.Size.Height + Config.SpaceBetweenInfoSections));
                }
            }

            //5- Draw sons
            Point edgesJoint = new(nodeCtx.NodeValueBounds.X + nodeCtx.NodeValueBounds.Width / 2, nodeCtx.NodeValueBounds.Bottom + Config.SpaceBetweenNodeAndEdge);
            var descendants = nodeCtx.Descendants.Span;
            var edgesHeads = new Point[descendants.Length];
            //Best descendant index to give it a different path color
            int bestDesIdx = -1;
            for (int i = 0; i < descendants.Length; i++)
            {
                edgesHeads[i] = Draw(descendants[i]);
                if (descendants[i].IsBest) { bestDesIdx = i; }
            }

            //6- Draw edges
            for (int i = 0; i < edgesHeads.Length; i++)
            {
                _drawingCtx.DrawLine(bestDesIdx == i ? Config.BestEdgesPen : Config.EdgesPen, edgesJoint, edgesHeads[i]);
            }

            //7- Draw "And" arc
            if (edgesHeads.Length > 1 && nodeCtx.Node.Type == DecisionNodeType.And)
            {
                /// <summary>
                /// Converts radians to degrees.
                /// </summary>
                static float ToDegress(double r)
                {
                    return (float)(360 * r / (2 * Math.PI));
                }
                /// <summary>
                /// Compute the angle between to vectors.
                /// <summary>
                /// <returns>
                /// Angle between to vectors in RADIANS.
                /// </returns>
                static double CalcAngle(Vector v1, Vector v2)
                {
                    var angleToV1 = Math.Atan2(v1.Y, v1.X);
                    var angleToV2 = Math.Atan2(v2.Y, v2.X);

                    var angle = angleToV2 - angleToV1;

                    if (angle < 0)
                    {
                        angle += 2 * Math.PI;
                    }
                    if (angle > Math.PI)
                    {
                        angle = 2 * Math.PI - angle;
                    }
                    return angle;
                }

                Vector firstEdge = edgesHeads[0] - edgesJoint, lastEdge = edgesHeads[^1] - edgesJoint;

                var circleRadius = (edgesHeads[0].Y - edgesJoint.Y) * 0.8;

                //and edge top should have an arc to identify it, so we will draw it here
                SKRect andCircle = new((float)(edgesJoint.X - circleRadius), (float)(edgesJoint.Y - circleRadius), (float)(edgesJoint.X + circleRadius), (float)(edgesJoint.Y + circleRadius));
                var vRadius = new Vector(andCircle.Right - edgesJoint.X, andCircle.MidY - edgesJoint.Y);
                var lastEdgeAngle = ToDegress(CalcAngle(vRadius, lastEdge));
                var angleBetweenEdges = ToDegress(CalcAngle(lastEdge, firstEdge));

                using SKPath arc = new();
                arc.MoveTo(edgesJoint.ToSKPoint());
                arc.AddArc(andCircle, lastEdgeAngle, angleBetweenEdges);

                var skContext = (_drawingCtx.PlatformImpl as ISkiaDrawingContextImpl)!;
                skContext.SkCanvas.DrawPath(arc, Config.AndArcPaint);
            }

            var nodeTopMid = new Point(nodeCtx.NodeValueBounds.X + nodeCtx.NodeValueBounds.Width / 2, nodeCtx.NodeValueBounds.Y - Config.SpaceBetweenNodeAndEdge);
            return nodeTopMid;
        }
        public void Draw(DrawingContext ctx)
        {
            _drawingCtx = ctx;
            Draw(Root);
            _drawingCtx = null;
        }

        public DecisionNodeRenderer(DecisionNode<TValue> root, IDecisionNodeValueRenderer<TValue> valueRenderer, DecisionNodeRenderingConfig<TValue> config)
        {
            Root = new DecisionNodeRenderingContext<TValue>(root, config);
            NodeValueRenderer = valueRenderer;
            Config = config;
        }
    }
}