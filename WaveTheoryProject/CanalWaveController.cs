using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    class CanalWaveController : WaveController
    {
        double d, _h;
        public double Delta {
            get => d;
            set { d = value; OnValueChanged?.Invoke(null, null, value, null, null, null); } }
        public double h {
            get => _h;
            set { _h = value; OnValueChanged?.Invoke(null, null, null, value, null, null); } }

        public override double sigma => throw new NotImplementedException();
        public override double a { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override double k { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override double p0
        {
            get
            {
                return base._p0;
            }
            set
            {
                base._p0 = value;
                OnValueChanged?.Invoke(null, null, null, null, _p0, null);
            }
        }
        public override double ro
        {
            get
            {
                return base._ro;
            }
            set
            {
                base._ro = value;
                OnValueChanged?.Invoke(null, null, null, null, null, base._ro);
            }
        }
        public override double x0fixed
        {
            get
            {
                return base._x0fixed;
            }
            set
            {
                base._x0fixed = value;
                OnValueChanged?.Invoke(value, null, null, null, null, null);
            }
        }
        public override double z0fixed
        {
            get
            {
                return base._z0fixed;
            }
            set
            {
                base._z0fixed = value;
                OnValueChanged?.Invoke(null, value, null, null, null, null);
            }
        }
        public override bool IsDrawingAvaliable => base.reslist.IsAllThreadsCompleted();
        CanalWaveParams cwp;

        public CanalWaveController()
        {
            _x0fixed = Settings.Init.x0;
            _z0fixed = Settings.Init.z0;
            d = Settings.Canal.delta;
            _h = Settings.Canal.h;
            base._p0 = Settings.p0;
            base._ro = Settings.ro;
            OnValueChanged += ValueChangedMethod;
            FillWavePointsFixedX(x0fixed, z0fixed, Settings.InitTimeFrom, Settings.InitTimeTo, Settings.Time_h);
            FillWavePointsTimeline(z0fixed);
        }

        void ValueChangedMethod(double? x0, double? z0, double? delta, double? h, double? p0, double? ro)
        {
            _x0fixed = Settings.Init.x0 = x0.HasValue ? (double)x0 : x0fixed;
            _z0fixed = Settings.Init.z0 = z0.HasValue ? (double)z0 : this.z0fixed;
            d = Settings.InitX0To = Settings.Canal.delta = delta.HasValue ? (double)delta : d;
            _h = Settings.Canal.h = h.HasValue ? (double)h : _h;
            base._p0 = Settings.p0 = p0.HasValue ? (double)p0 : base._p0;
            base._ro = Settings.ro = ro.HasValue ? (double)ro : base._ro;
            FillWavePointsFixedX(x0fixed, this.z0fixed, Settings.InitTimeFrom, Settings.InitTimeTo, Settings.Time_h);
            if (z0.HasValue || delta.HasValue || h.HasValue)
            {
                FillWavePointsTimeline(this.z0fixed);
            }
        }

        public override event ValueChangedHandler OnValueChanged;

        public override double P(double x, double z, double t)
        {
            return 0;
        }

        public override void Refresh()
        {
            OnValueChanged?.Invoke(null, null, null, null, null, null);
            FillWavePointsTimeline(this.z0fixed);
        }

        public override double Vx(double x, double z, double t)
        {
            return 0;
        }

        public override double Vz(double x, double z, double t)
        {
            return 0;
        }

        public override double X(double x0, double z0, double t)
        {
            double sum = 0, tmp=sum;
            for (uint n = 1; n<2 ; n++)
            {
                cwp = new CanalWaveParams(n);
                sum += GxnFunc(z0, t, ref cwp) * Math.Sin(cwp.k * x0);
                if (Math.Abs(sum - tmp) < Settings.Eps/10000.0) { break; }
                else { tmp = sum; }
            }
            return x0 + g * sum;
        }

        public override double Z(double x0, double z0, double t)
        {
            double sum = 0, tmp = sum;
            for (uint n = 1; n<2 ; n++)
            {
                cwp = new CanalWaveParams(n);
                sum += GznFunc(z0, t, ref cwp) * Math.Cos(cwp.k * x0);
                if (Math.Abs(sum - tmp) < Settings.Eps/10000.0) { break; }
                else { tmp = sum; }
            }
            return z0 - g * sum;
        }

        double FxnFunc(double z, double t, ref CanalWaveParams p)
        {
            return p.k * p.a * Math.Cosh(p.k * (z + Settings.Canal.h)) * Math.Sin(p.sigma * t) / (p.sigma * Math.Cosh(p.k * Settings.Canal.h));
        }
        double FznFunc(double z, double t, ref CanalWaveParams p)
        {
            return p.k * p.a * Math.Sinh(p.k * (z + Settings.Canal.h)) * Math.Sin(p.sigma * t) / (p.sigma * Math.Cosh(p.k * Settings.Canal.h));
        }
        double GxnFunc(double z0, double t, ref CanalWaveParams p)
        {
            return -p.k * p.a * Math.Cosh(p.k * (z0 + Settings.Canal.h)) * Math.Cos(p.sigma * t) / (p.sigma * p.sigma * Math.Cosh(p.k * Settings.Canal.h));
        }
        double GznFunc(double z0, double t, ref CanalWaveParams p)
        {
            return -p.k * p.a * Math.Sinh(p.k * (z0 + Settings.Canal.h)) * Math.Cos(p.sigma * t) / (p.sigma * p.sigma * Math.Cosh(p.k * Settings.Canal.h));
        }
    }
}
