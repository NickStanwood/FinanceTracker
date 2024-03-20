using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.WPF
{
    public class DateTimeDoublePoint : IGraphPoint<DateTime, double>
    {
        private DateTime _x;
        private double _y;

        public DateTimeDoublePoint(DateTime x, double y)
        {
            _x = x;
            _y = y;
        }

        public int CompareTo(object? obj)
        {
            IGraphPoint<DateTime, double>? other = obj as IGraphPoint<DateTime, double>;
            if(other == null)
                throw new ArgumentException(nameof(other));
            
            if(this.GetXValue() < other.GetXValue())
                return -1;

            if (this.GetXValue() > other.GetXValue())
                return 1;

            return 0;
        }

        public DateTime GetXValue()
        {
            return _x;
        }

        public double GetYValue()
        {
            return _y;
        }

        public double GetXPosition(DateTime minX, DateTime maxX)
        {
            if (minX == maxX)
                return 0.5;

            TimeSpan total = maxX - minX;
            return (_x - minX).TotalMilliseconds / total.TotalMilliseconds;
        }

        public double GetYPosition(double minY, double maxY)
        {
            if(minY == maxY)
                return 0.5;

            return -(_y - minY) / (maxY - minY) + 1.0;
        }

        public double GetXAxisPosition(double minY, double maxY)
        {
            //Axis needs to be where the y value = 0
            return (0 - minY) / (maxY - minY);
        }

        public double GetYAxisPosition(DateTime minX, DateTime maxX)
        {
            return double.NaN;
        }
    }

    public interface IGraphPoint<XType, YType> : IGraphPoint
        where XType : IComparable
        where YType : IComparable
    {
        public double GetXPosition(XType minX, XType maxX);
        public double GetYPosition(YType minY, YType maxY);
        public double GetXAxisPosition(YType minY, YType maxY);
        public double GetYAxisPosition(XType minX, XType maxX);
        new public XType GetXValue();
        new public YType GetYValue();

        #region IGraphPoint redirects
        double IGraphPoint.GetXPosition(object minX, object maxX)
        {
            return GetXPosition((XType)minX, (XType)maxX);
        }

        double IGraphPoint.GetYPosition(object minY, object maxY)
        {
            return GetYPosition((YType)minY, (YType)maxY);
        }

        double IGraphPoint.GetXAxisPosition(object minY, object maxY)
        {
            return GetXAxisPosition((YType)minY, (YType)maxY);
        }

        double IGraphPoint.GetYAxisPosition(object minX, object maxX)
        {
            return GetYAxisPosition((XType)minX, (XType)maxX);
        }

        IComparable IGraphPoint.GetXValue()
        {
            return GetXValue();
        }

        IComparable IGraphPoint.GetYValue()
        {
            return GetYValue();
        }
        #endregion
    }

    public interface IGraphPoint : IComparable
    {
        public double GetXPosition(object minX, object maxX);
        public double GetYPosition(object minY, object maxY);
        public double GetXAxisPosition(object minY, object maxY);
        public double GetYAxisPosition(object minX, object maxX);
        public IComparable GetXValue();
        public IComparable GetYValue();
    }
}
