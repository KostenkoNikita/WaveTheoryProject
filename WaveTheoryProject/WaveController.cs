using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    abstract class WaveController
    {
        protected double _sigma, _a, _x0, _z0;
        public abstract double k { get; }
        public abstract double x0 { get; set; }
        public abstract double z0 { get; set; }
        public abstract double a { get; set; }
        public abstract double sigma { get; set; }
        public List<WavePoint> WavePoints;
        protected void FillWavePointsDictionary(double FromTime, double ToTime, double Time_h)
        {
            if (WavePoints == null) { WavePoints = new List<WavePoint>(); }
            for (; FromTime <= ToTime; FromTime += Time_h)
            {
                WavePoints.Add(new WavePoint(FromTime,X,Z,Vx,Vz));
            }
        }
        public delegate void ValueChangedHandler(double? x0, double? z0, double? sigma, double? a);
        public abstract event ValueChangedHandler OnValueChanged;
        public abstract double X(double t);
        public abstract double Z(double t);
        public abstract double Vx(double x, double z, double t);
        public abstract double Vz(double x, double z, double t);        
    }
}
