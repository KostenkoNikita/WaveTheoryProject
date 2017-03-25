using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    abstract class WaveController
    {
        public const double g = 9.809;
        protected double _a, _x0fixed, _z0fixed, _p0, _ro, _k;
        public abstract double sigma { get; }
        public abstract double a { get; set; }
        public abstract double k { get; set; }
        public abstract double p0 { get; set; }
        public abstract double ro { get; set; }
        public abstract double x0fixed { get; set; }
        public abstract double z0fixed { get; set; }
        public abstract bool IsDrawingAvaliable { get; }
        public List<WavePointSingle> WavePointsFixedX = new List<WavePointSingle>();
        public List<WavePointsListAtTime> WavePointsListTimeline = new List<WavePointsListAtTime>();
        delegate void FillTimeLineDelegate(List<WavePointsListAtTime> timeline, double time, double z0);
        FillTimeLineDelegate fillingDelegate;
        protected List<IAsyncResult> reslist;
        protected void FillWavePointsFixedX(double x0, double z0, double FromTime, double ToTime, double Time_h)
        {
            WavePointsFixedX.Clear();
            for (; FromTime <= ToTime; FromTime += Time_h)
            {
                WavePointsFixedX.Add(new WavePointSingle(FromTime,x0,z0,X,Z,Vx,Vz,P));
            }
        }
        protected void FillWavePointsListAtTime(List<WavePointsListAtTime> timeline, double t, double z0)
        {
            timeline.Add(new WavePointsListAtTime(t, z0, X, Z, Vx, Vz,P));
        }
        protected void FillWavePointsTimeline(double z0)
        {
            WavePointsListTimeline.Clear();
            fillingDelegate = new FillTimeLineDelegate(FillWavePointsListAtTime);
            WavePointsListTimeline = new List<WavePointsListAtTime>();
            reslist = new List<IAsyncResult>();
            double tmptime = Settings.InitTimeFrom;
            for (; Settings.InitTimeFrom <= Settings.InitTimeTo; Settings.InitTimeFrom += Settings.Time_h)
            {
                reslist.Add(fillingDelegate.BeginInvoke(WavePointsListTimeline, Settings.InitTimeFrom,z0,null,null));
            }
            Settings.InitTimeFrom = tmptime;
        }
        public void DrawWaveAtTime(double time, PlotWindowModel g)
        {
            WavePointsListAtTime l = WavePointsListTimeline.FirstOrDefault((list) => { if (Math.Abs(list.Time - time) <= Settings.Eps) { return true; } else { return false; } });
            if (l != null)
            {
                g.DrawCurve(l);
            }
            if (this is WaveGroupController)
            {
                WaveGroupController ctmp = this as WaveGroupController;
                LineSeries lSpec = new LineSeries() { LineStyle = LineStyle.Dash, Color = OxyColors.Red, StrokeThickness = 2 };
                for (double x = Settings.InitX0From; x <= Settings.InitX0To; x += Settings.X0_h)
                {
                    lSpec.Points.Add(new DataPoint(x,ctmp.A(x,time)));
                }
                g.DrawCurve(lSpec);
            }
        }
        public WavePointsListAtTime DrawSingleWavePointsList(double t, double z0, double k, double a)
        {
            double tmp_k = _k;
            double tmp_a = _a;
            _k = k;
            _a = a;
            WavePointsListAtTime l = new WavePointsListAtTime(t, z0, X, Z, Vx, Vz, P);
            _k = tmp_k;
            _a = tmp_a;
            return l;
        }
        public delegate void ValueChangedHandler(double? x0, double? z0, double? sigma, double? a, double? p0, double? ro);
        public abstract event ValueChangedHandler OnValueChanged;
        public abstract double X(double x0, double z0,double t);
        public abstract double Z(double x0, double z0,double t);
        public abstract double Vx(double x, double z, double t);
        public abstract double Vz(double x, double z, double t);
        public abstract double P(double x, double z, double t);
        public abstract void Refresh();
    }

}
