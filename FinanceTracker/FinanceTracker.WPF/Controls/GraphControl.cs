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
            gpList.Sort();
            IComparable xMin = gpList[0].GetXValue();
            IComparable xMax = gpList[gpList.Count - 1].GetXValue();
            IComparable yMin = gpList[0].GetYValue();
            IComparable yMax = gpList[gpList.Count - 1].GetYValue();


            //get max and min values
            foreach (IGraphPoint gp in gpList)
            {
                IComparable x = gp.GetXValue();
                if (x.CompareTo(xMin) < 0)
                    xMin = x;
                if (x.CompareTo(xMax) > 0)
                    xMax = x;

                IComparable y = gp.GetYValue();
                if (y.CompareTo(yMin) < 0)
                    yMin = y;
                if (y.CompareTo(yMax) > 0)
                    yMax = y;
            }


            //add points to graph
            Point[] points = new Point[gpList.Count];
            int i = 0;
            foreach (IGraphPoint gp in gpList)
            {
                double x = gp.GetXPosition(xMin, xMax, yMin, yMax) * Width;
                double y = gp.GetYPosition(xMin, xMax, yMin, yMax) * Height;
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

            //set Xaxis
            double xAxisHeight = -gpList[0].GetXAxisPosition(xMin, xMax, yMin, yMax)*Height;
            Point xstart = new Point(0, xAxisHeight);
            Point xEnd = new Point(Width, xAxisHeight);
            List<LineSegment> seg = new List<LineSegment>();
            seg.Add(new LineSegment(xEnd, true));
            PathFigure xFig = new PathFigure(xstart, seg, false);
            XAxis = new PathGeometry();
            XAxis.Figures.Add(xFig);
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
        [Bindable(true)]
        [Category("Appearance")]
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

        static GraphControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphControl), new FrameworkPropertyMetadata(typeof(GraphControl)));
        }
    }
}
