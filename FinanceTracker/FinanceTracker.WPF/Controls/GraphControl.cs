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


            if (newValue == null || newValue.Count() == 0)
            {
                GraphData = null;
                return;
            }

            List<IGraphPoint>? gpList = newValue as List<IGraphPoint>;
            if (gpList == null || newValue.Count() == 0)
            {
                GraphData = null;
                return;
            }

            IComparable xMin = gpList[0].GetXValue();
            IComparable xMax = gpList[0].GetXValue();
            IComparable yMin = gpList[0].GetYValue();
            IComparable yMax = gpList[0].GetYValue();

            gpList.Sort();

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
            GraphData = new Point[gpList.Count];
            int i = 0;
            foreach (IGraphPoint gp in gpList)
            {
                double x = gp.GetXPosition(xMin, xMin, yMin, yMax);
                double y = gp.GetXPosition(xMin, xMin, yMin, yMax);
                GraphData[i++] = new Point(x, y);
            }

        }

        void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }
        #endregion

        #region GraphData
        public static readonly DependencyProperty GraphDataProperty;

        //
        // Summary:
        //     Gets or sets a brush that describes the foreground color.
        //
        // Returns:
        //     The brush that paints the foreground of the control. The default value is the
        //     system dialog font color.
        [Bindable(false)]
        [Category("Appearance")]
        public Point[]? GraphData {get; set;}
        #endregion

        static GraphControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphControl), new FrameworkPropertyMetadata(typeof(GraphControl)));
        }
    }
}
