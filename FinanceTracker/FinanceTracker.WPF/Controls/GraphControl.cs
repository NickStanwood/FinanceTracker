using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceTracker.WPF
{
    public class GraphControl : Control
    {
        #region ItemsSource
        public IEnumerable<IGraphPoint> ItemsSource
        {
            get { return (IEnumerable<IGraphPoint>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<IGraphPoint>), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

        private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as GraphControl;
            if (control != null)
                control.OnItemsSourceChanged((IEnumerable<IGraphPoint>)e.OldValue, (IEnumerable<IGraphPoint>)e.NewValue);
        }

        private void OnItemsSourceChanged(IEnumerable<IGraphPoint> oldValue, IEnumerable<IGraphPoint> newValue)
        {
            // Remove handler for oldValue.CollectionChanged
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if (null != oldValueINotifyCollectionChanged)
            {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }
            // Add handler for newValue.CollectionChanged (if possible)
            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (null != newValueINotifyCollectionChanged)
            {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }


        }

        void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            List<IGraphPoint> gpList = ItemsSource.Cast<IGraphPoint>().ToList();
            UpdateGraphBounds(gpList);
            UpdateGridLines(gpList);
            UpdateGraphPlot(gpList);
            

        }
        #endregion

        #region GraphData
        [Bindable(false)]
        [Category("Appearance")]
        public PathGeometry GraphData
        {
            get { return (PathGeometry)GetValue(GraphDataProperty); }
            set { SetValue(GraphDataProperty, value); }
        }

        public static readonly DependencyProperty GraphDataProperty 
            = DependencyProperty.Register("GraphData", typeof(PathGeometry), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnGraphDataPropertyChanged)));
        private static void OnGraphDataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        #endregion

        #region XAxis
        [Bindable(false)]
        public PathGeometry XAxis
        {
            get { return (PathGeometry)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        public static readonly DependencyProperty XAxisProperty
            = DependencyProperty.Register("XAxis", typeof(PathGeometry), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnXAxisPropertyChanged)));
        private static void OnXAxisPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region YAxis
        [Bindable(false)]
        public PathGeometry YAxis
        {
            get { return (PathGeometry)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        public static readonly DependencyProperty YAxisProperty
            = DependencyProperty.Register("YAxis", typeof(PathGeometry), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnYAxisPropertyChanged)));
        private static void OnYAxisPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region XGridLines
        [Bindable(false)]
        public PathGeometry XGridLines
        {
            get { return (PathGeometry)GetValue(XGridLinesProperty); }
            set { SetValue(XGridLinesProperty, value); }
        }

        public static readonly DependencyProperty XGridLinesProperty
            = DependencyProperty.Register("XGridLines", typeof(PathGeometry), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnXGridLinesPropertyChanged)));
        private static void OnXGridLinesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region YGridLines
        [Bindable(false)]
        public PathGeometry YGridLines
        {
            get { return (PathGeometry)GetValue(YGridLinesProperty); }
            set { SetValue(YGridLinesProperty, value); }
        }

        public static readonly DependencyProperty YGridLinesProperty
            = DependencyProperty.Register("YGridLines", typeof(PathGeometry), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnYGridLinesPropertyChanged)));
        private static void OnYGridLinesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region MinY
        [Bindable(true)]
        [Category("Appearance")]
        public IGraphPoint MinY
        {
            get { return (IGraphPoint)GetValue(MinYProperty); }
            set { SetValue(MinYProperty, value); }
        }

        public static readonly DependencyProperty MinYProperty
            = DependencyProperty.Register("MinY", typeof(IGraphPoint), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnMinYPropertyChanged)));
        private static void OnMinYPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region MaxY
        [Bindable(true)]
        [Category("Appearance")]
        public IGraphPoint MaxY
        {
            get { return (IGraphPoint)GetValue(MaxYProperty); }
            set { SetValue(MaxYProperty, value); }
        }

        public static readonly DependencyProperty MaxYProperty
            = DependencyProperty.Register("MaxY", typeof(IGraphPoint), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnMaxYPropertyChanged)));
        private static void OnMaxYPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region XAxisTitle
        [Bindable(true)]
        [Category("Appearance")]
        public string XAxisTitle
        {
            get { return (string)GetValue(XAxisTitleProperty); }
            set { SetValue(XAxisTitleProperty, value); }
        }

        public static readonly DependencyProperty XAxisTitleProperty
            = DependencyProperty.Register("XAxisTitle", typeof(string), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnXAxisTitlePropertyChanged)));
        private static void OnXAxisTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region YAxisTitle
        [Bindable(true)]
        [Category("Appearance")]
        public string YAxisTitle
        {
            get { return (string)GetValue(YAxisTitleProperty); }
            set { SetValue(YAxisTitleProperty, value); }
        }

        public static readonly DependencyProperty YAxisTitleProperty
            = DependencyProperty.Register("YAxisTitle", typeof(string), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnYAxisTitlePropertyChanged)));
        private static void OnYAxisTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region GridHeight
        [Bindable(true)]
        [Category("Appearance")]
        public GridLength GridHeight
        {
            get { return (GridLength)GetValue(GridHeightProperty); }
            set { SetValue(GridHeightProperty, value); }
        }

        public static readonly DependencyProperty GridHeightProperty
            = DependencyProperty.Register("GridHeight", typeof(GridLength), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnGridHeightPropertyChanged)));
        private static void OnGridHeightPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region GridWidth
        [Bindable(true)]
        [Category("Appearance")]
        public GridLength GridWidth
        {
            get { return (GridLength)GetValue(GridWidthProperty); }
            set { SetValue(GridWidthProperty, value); }
        }

        public static readonly DependencyProperty GridWidthProperty
            = DependencyProperty.Register("GridWidth", typeof(GridLength), typeof(GraphControl), new PropertyMetadata(new PropertyChangedCallback(OnGridWidthPropertyChanged)));
        private static void OnGridWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        private double GraphWidth { get { return Width - 20;  } } //minus 18 for the size of the axis title rows & column. minus 2 for canvas margin
        private double GraphHeight{ get { return Height - 20; } }

        private IComparable _xMin;
        private IComparable _xMax;
        private IComparable _yMin;
        private IComparable _yMax;
        static GraphControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphControl), new FrameworkPropertyMetadata(typeof(GraphControl)));
        }

        private void UpdateGraphBounds(List<IGraphPoint> gpList)
        {
            gpList.Sort();
            _xMin = gpList[0].GetXValue();
            _xMax = gpList[gpList.Count - 1].GetXValue();

            _yMin = gpList[0].GetYValue();
            if (ReadLocalValue(MinYProperty) != DependencyProperty.UnsetValue)
                _yMin = MinY.GetYValue();

            _yMax = gpList[gpList.Count - 1].GetYValue();
            if (ReadLocalValue(MaxYProperty) != DependencyProperty.UnsetValue)
                _yMax = MaxY.GetYValue();

            //get max and min values
            foreach (IGraphPoint gp in gpList)
            {
                IComparable x = gp.GetXValue();
                if (x.CompareTo(_xMin) < 0)
                    _xMin = x;
                if (x.CompareTo(_xMax) > 0)
                    _xMax = x;

                IComparable y = gp.GetYValue();
                if (y.CompareTo(_yMin) < 0)
                    _yMin = y;
                if (y.CompareTo(_yMax) > 0)
                    _yMax = y;
            }
        }

        private void UpdateGridLines(List<IGraphPoint> gpList)
        {
            //set Xaxis
            double xAxisHeight = (1 - gpList[0].GetXAxisPosition(_yMin, _yMax)) * GraphHeight;
            Point xStart = new Point(0, xAxisHeight);
            Point xEnd = new Point(GraphWidth, xAxisHeight);
            List<LineSegment> xSeg = new List<LineSegment>();
            xSeg.Add(new LineSegment(xEnd, true));
            PathFigure xFig = new PathFigure(xStart, xSeg, false);
            XAxis = new PathGeometry();
            XAxis.Figures.Add(xFig);

            //set Xaxis grid lines
            if(ReadLocalValue(GridHeightProperty) != DependencyProperty.UnsetValue)
            {
                XGridLines = new PathGeometry();
                if (GridHeight.IsAbsolute && GridHeight.Value > 0.0)
                {
                    if (double.IsNaN(xAxisHeight))
                        xAxisHeight = GraphHeight;

                    double lineHeight = xAxisHeight + GridHeight.Value;
                    while (lineHeight < GraphHeight)
                    {
                        Point p0 = new Point(0, lineHeight);
                        Point p1 = new Point(GraphWidth, lineHeight);
                        List<LineSegment> seg = new List<LineSegment>();
                        seg.Add(new LineSegment(p1, true));
                        PathFigure fig = new PathFigure(p0, seg, false);
                        XGridLines.Figures.Add(fig);
                        lineHeight += GridHeight.Value;
                    }

                    lineHeight = xAxisHeight - GridHeight.Value;
                    while (lineHeight > 0)
                    {
                        Point p0 = new Point(0, lineHeight);
                        Point p1 = new Point(GraphWidth, lineHeight);
                        List<LineSegment> seg = new List<LineSegment>();
                        seg.Add(new LineSegment(p1, true));
                        PathFigure fig = new PathFigure(p0, seg, false);
                        XGridLines.Figures.Add(fig);
                        lineHeight -= GridHeight.Value;
                    }
                }
            }
            
            //set y axis
            double yAxisWidth = gpList[0].GetYAxisPosition(_xMin, _xMax) * GraphWidth;
            Point yStart = new Point(0, yAxisWidth);
            Point yEnd = new Point(GraphHeight, yAxisWidth);
            List<LineSegment> ySeg = new List<LineSegment>();
            ySeg.Add(new LineSegment(yEnd, true));
            PathFigure yFig = new PathFigure(yStart, ySeg, false);
            YAxis = new PathGeometry();
            YAxis.Figures.Add(yFig);

            //set Y axis grid lines
            if (ReadLocalValue(GridWidthProperty) != DependencyProperty.UnsetValue)
            {
                YGridLines = new PathGeometry();
                if (GridWidth.IsAbsolute && GridWidth.Value > 0.0)
                {
                    if (double.IsNaN(yAxisWidth))
                        yAxisWidth = 0;

                    double lineWidth = yAxisWidth + GridWidth.Value;
                    while (lineWidth < GraphWidth)
                    {
                        Point p0 = new Point(lineWidth, 0);
                        Point p1 = new Point(lineWidth, GraphHeight);
                        List<LineSegment> seg = new List<LineSegment>();
                        seg.Add(new LineSegment(p1, true));
                        PathFigure fig = new PathFigure(p0, seg, false);
                        YGridLines.Figures.Add(fig);
                        lineWidth += GridWidth.Value;
                    }

                    lineWidth = yAxisWidth - GridWidth.Value;
                    while (lineWidth > 0)
                    {
                        Point p0 = new Point(lineWidth, 0);
                        Point p1 = new Point(lineWidth, GraphHeight);
                        List<LineSegment> seg = new List<LineSegment>();
                        seg.Add(new LineSegment(p1, true));
                        PathFigure fig = new PathFigure(p0, seg, false);
                        YGridLines.Figures.Add(fig);
                        lineWidth -= GridWidth.Value;
                    }
                }
            }
        }

        private void UpdateGraphPlot(List<IGraphPoint> gpList)
        {
            //add points to graph
            Point[] points = new Point[gpList.Count];
            int i = 0;
            foreach (IGraphPoint gp in gpList)
            {
                double x = gp.GetXPosition(_xMin, _xMax) * GraphWidth;
                double y = (1 - gp.GetYPosition(_yMin, _yMax)) * GraphHeight;
                points[i++] = new Point(x, y);
            }

            if (points.Length > 0)
            {
                Point start = points[0];
                List<LineSegment> segments = new List<LineSegment>();
                for (i = 1; i < points.Length; i++)
                {
                    segments.Add(new LineSegment(points[i], true));
                }
                PathFigure figure = new PathFigure(start, segments, false); //true if closed
                GraphData = new PathGeometry();
                GraphData.Figures.Add(figure);
            }
            else
            {
                GraphData = null;
            }
        }
    }
}
