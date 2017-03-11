using System;
using OxyPlot;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    struct WavePointSingle
    {
        double x, z, vx, vz,_t, p;
        public double t => _t;
        public double X => x;
        public double Z => z;
        public double Vx => vx;
        public double Vz => vz;
        public double P => p;
        public WavePointSingle(double t, double x0, double z0, double X, double Z, double Vx, double Vz, double p)
        {
            _t = Math.Round(t,Settings.Precision);
            x = Math.Round(X, Settings.Precision); 
            z = Math.Round(Z, Settings.Precision); 
            vx = Math.Round(Vx, Settings.Precision);
            vz = Math.Round(Vz, Settings.Precision);
            this.p = Math.Round(p, Settings.Precision);
        }
        public WavePointSingle(double t, double x0, double z0, Func<double, double,double, double> X, Func<double,double, double, double> Z, Func<double, double, double, double> Vx, Func<double, double, double, double> Vz, Func<double, double, double, double> P)
        {
            _t = Math.Round(t, Settings.Precision);
            x = Math.Round(X(x0,z0,t),Settings.Precision);
            z = Math.Round(Z(x0,z0,t), Settings.Precision);
            vx = Math.Round(Vx(x, z, t),Settings.Precision); 
            vz = Math.Round(Vz(x, z, t), Settings.Precision);
            p = Math.Round(P(x, z, t), Settings.Precision);
        }
        public override bool Equals(object obj) => obj is WavePointSingle ? Equals((WavePointSingle)obj) : false;
        bool Equals(WavePointSingle other) => x == other.x && z == other.z && vx == other.vx && vz == other.vz && t == other.t;
        public override int GetHashCode() => t.GetHashCode() ^ x.GetHashCode() ^ z.GetHashCode() ^ vx.GetHashCode() ^ vz.GetHashCode();
        public override string ToString()
        {
            return string.Format("t: {0}; X: {1}; Z: {2}; Vx: {3}; Vz: {4}",
                t.ToString(Settings.Format), x.ToString(Settings.Format), z.ToString(Settings.Format),
                vx.ToString(Settings.Format), vz.ToString(Settings.Format));
        }
        public static implicit operator DataPoint(WavePointSingle p)
        {
            return new DataPoint(p.X, p.Z);
        }
    }
}
