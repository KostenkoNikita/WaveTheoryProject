using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    struct WavePoint
    {
        double x, z, vx, vz,_t;
        public double t => _t;
        public double X => x;
        public double Z => z;
        public double Vx => vx;
        public double Vz => vz;
        public WavePoint(double t, double X, double Z, double Vx, double Vz)
        {
            _t = Math.Round(t,Settings.Precision);
            x = Math.Round(X, Settings.Precision); 
            z = Math.Round(Z, Settings.Precision); 
            vx = Math.Round(Vx, Settings.Precision);
            vz = Math.Round(Vz, Settings.Precision);
        }
        public WavePoint(double t, Func<double, double> X, Func<double, double> Z, Func<double, double, double, double> Vx, Func<double, double, double, double> Vz)
        {
            _t = Math.Round(t, Settings.Precision);
            x = Math.Round(X(t),Settings.Precision);
            z = Math.Round(Z(t), Settings.Precision);
            vx = Math.Round(Vx(x, z, t),Settings.Precision); 
            vz = Math.Round(Vz(x, z, t), Settings.Precision);
        }
        public override bool Equals(object obj) => obj is WavePoint ? Equals((WavePoint)obj) : false;
        bool Equals(WavePoint other) => x == other.x && z == other.z && vx == other.vx && vz == other.vz && t == other.t;
        public override int GetHashCode() => t.GetHashCode() ^ x.GetHashCode() ^ z.GetHashCode() ^ vx.GetHashCode() ^ vz.GetHashCode();
        public override string ToString()
        {
            return string.Format("t: {0}; X: {1}; Z: {2}; Vx: {3}; Vz: {4}",
                t.ToString(Settings.Format), x.ToString(Settings.Format), z.ToString(Settings.Format),
                vx.ToString(Settings.Format), vz.ToString(Settings.Format));
        }
    }
}
