//#define NEW_CAP

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    struct CanalWaveParams
    {
        double _a, _k, _sigma;
        uint _n;
        public double a => _a;
        public double k => _k;
        public double sigma => _sigma;
        public uint n => _n;
        public CanalWaveParams(uint n)
        {
            if (n == 0) { throw new ArgumentException(); }
            _n = n;
            _k = Math.PI * _n / Settings.Canal.delta;
#if NEW_CAP
            _a = (2.0 / (Math.Sqrt(3) * Settings.Canal.delta * _k * _k)) * 
                (-1+Math.Cos(Settings.Canal.delta*_k)+ 0.5*Settings.Canal.delta*_k*Math.Sin(Settings.Canal.delta*_k));
#else
            _a = (2.0 / (3 * Settings.Canal.delta * Settings.Canal.delta * _k * _k)) *
                (-3 + 3 * Math.Cos(Settings.Canal.delta * _k) + 2 * Settings.Canal.delta * _k *
                Math.Sin(Settings.Canal.delta * _k));
#endif

            _sigma = Math.Sqrt(WaveController.g*_k*Math.Tanh(_k*Settings.Canal.h));
        }

        public override bool Equals(object obj)
        {
            return obj is CanalWaveParams ? Equals((CanalWaveParams)obj) : false;
        }
        bool Equals(CanalWaveParams other)
        {
            return _k == other._k &&
                _a == other._a &&
                _sigma == other._sigma &&
                _n == other._n;
        }
        public override int GetHashCode()
        {
            return _k.GetHashCode() ^
                _a.GetHashCode() ^
                _sigma.GetHashCode() ^
                _n.GetHashCode();
        }
        public override string ToString()
        {
            return $"n={_n}; k={_k}; a={_a}";
        }
    }
}
