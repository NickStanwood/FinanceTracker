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

        public double GetXPosition(DateTime minX, DateTime maxX, double minY, double maxY)
        {
            TimeSpan total = maxX - minX;
            return (_x - minX).TotalMilliseconds / total.TotalMilliseconds;
        }

        public double GetYPosition(DateTime minX, DateTime maxX, double minY, double maxY)
        {
            return (_y - minY) / (maxY - minY);
        }

        #region IGraphPoint implementation
        double IGraphPoint.GetXPosition(object minX, object maxX, object minY, object maxY)
        {
            return GetXPosition((DateTime)minX, (DateTime)maxX, (double)minY, (double)maxY);
        }

        double IGraphPoint.GetYPosition(object minX, object maxX, object minY, object maxY)
        {
            return GetYPosition((DateTime)minX, (DateTime)maxX, (double)minY, (double)maxY);
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
    public interface IGraphPoint<XType, YType> : IGraphPoint
        where XType : IComparable
        where YType : IComparable
    {
        public double GetXPosition(XType minX, XType maxX, YType minY, YType maxY);
        public double GetYPosition(XType minX, XType maxX, YType minY, YType maxY);
        new public XType GetXValue();
        new public YType GetYValue();
    }

    public interface IGraphPoint : IComparable
    {
        public double GetXPosition(object minX, object maxX, object minY, object maxY);
        public double GetYPosition(object minX, object maxX, object minY, object maxY);
        public IComparable GetXValue();
        public IComparable GetYValue();
    }
}
