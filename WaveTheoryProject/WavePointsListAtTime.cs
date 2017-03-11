using System;
using System.Collections;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    class WavePointsListAtTime : IEnumerable, IList<WavePointSingle>
    {
        List<WavePointSingle> list;

        LineSeries ls;

        public LineSeries WaveLineSeries => ls;

        double time, z0;

        public WavePointSingle this[int index] { get => list[index]; set => list[index] = value; }

        public int Count => list.Count;

        public double Time => time;

        public double Z0 => z0;

        public bool IsReadOnly => false;

        public WavePointsListAtTime(double time, double z0, Func<double,double,double, double> X, Func<double, double, double, double> Z, Func<double, double, double, double> Vx, Func<double, double, double, double> Vz, Func<double, double, double, double> P)
        {
            this.time = time;
            this.z0 = z0;
            list = new List<WavePointSingle>();
            ls = new LineSeries();
            ls.Smooth = true;
            ls.Color = OxyColors.Blue;
            ls.StrokeThickness = 2;
            double tmpX0 = Settings.Init.x0;
            double tmpZ0 = Settings.Init.z0;
            for (tmpX0 = Settings.InitX0From- 5*Settings.X0_h; tmpX0 <= Settings.InitX0To+ 5*Settings.X0_h; tmpX0 += Settings.X0_h)
            {
                list.Add(new WavePointSingle(time, tmpX0, z0, X, Z, Vx, Vz,P));
                ls.Points.Add(list[list.Count-1]);
            }
        }

        public void Add(WavePointSingle item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(WavePointSingle item)
        {
            return list.Contains(item);
        }

        public void CopyTo(WavePointSingle[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(WavePointSingle item)
        {
            return IndexOf(item);
        }

        public void Insert(int index, WavePointSingle item)
        {
            list.Insert(index, item);
        }

        public bool Remove(WavePointSingle item)
        {
            return list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        IEnumerator<WavePointSingle> IEnumerable<WavePointSingle>.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void DrawListOn(PlotWindowModel m)
        {
            m.DrawCurve(this);
        }

        public static implicit operator List<DataPoint>(WavePointsListAtTime l)
        {
            List<DataPoint> res = new List<DataPoint>() { Capacity = l.Count };
            l.list.ForEach((p) => { res.Add(new DataPoint(p.X, p.Z)); });
            return res;
        }

        public override string ToString()
        {
            return $"t = {time}";
        }
    }
}
