using System;
using OxyPlot;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    class PlaneWaveController : WaveController
    {
        public override bool IsDrawingAvaliable => base.reslist.IsAllThreadsCompleted();
        public override double x0fixed
        {
            get
            {
                return base._x0fixed;
            }
            set
            {
                base._x0fixed = value;
                OnValueChanged?.Invoke(value, null, null, null);
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
                OnValueChanged?.Invoke(null, value, null, null);
            }
        }
        public override double sigma
        {
            get
            {
                return base._sigma;
            }
            set
            {
                base._sigma = value;
                OnValueChanged?.Invoke(null, null, base._sigma, null);
            }
        }
        public override double a
        {
            get
            {
                return base._a;
            }
            set
            {
                base._a = value;
                OnValueChanged?.Invoke(null, null, null, base._a);
            }
        }
        public override double k => _sigma*_sigma/Settings.g;

        public override event ValueChangedHandler OnValueChanged;

        public PlaneWaveController()
        {
            _x0fixed = Settings.Init.x0;
            _z0fixed = Settings.Init.z0;
            OnValueChanged += ValueChangedMethod;
            base._sigma = Settings.sigma;
            base._a = Settings.a;
            FillWavePointsFixedX(x0fixed,z0fixed,Settings.InitTimeFrom, Settings.InitTimeTo, Settings.Time_h);
            FillWavePointsTimeline(z0fixed);
        }

        void ValueChangedMethod(double? x0, double? z0, double? sigma, double? a)
        {
            _x0fixed = x0.HasValue ? (double)x0 : x0fixed ;
            _z0fixed = Settings.Init.z0 = z0.HasValue ? (double)z0 : this.z0fixed;
            base._a = Settings.a = a.HasValue ? (double)a : base._a;
            base._sigma = Settings.sigma = sigma.HasValue ? (double)sigma : base._sigma;
            base._a = Settings.a = a.HasValue ? (double)a : base._a;
            FillWavePointsFixedX(x0fixed, this.z0fixed, Settings.InitTimeFrom, Settings.InitTimeTo, Settings.Time_h);
            if (z0.HasValue || sigma.HasValue || a.HasValue)
            { 
                FillWavePointsTimeline(this.z0fixed);
            }
        }

        public override void Refresh()
        {
            OnValueChanged?.Invoke(null, null, null, null);
            FillWavePointsTimeline(this.z0fixed);
        }

        public override double Vx(double x, double z, double t)
        {
            return _a * _sigma * Math.Exp(k * z) * Math.Cos(k * x) * Math.Cos(_sigma * t);
        }

        public override double Vz(double x, double z, double t)
        {
            return _a * sigma * Math.Exp(k * z) * Math.Sin(k * x) * Math.Cos(_sigma * t);
        }

        public override double X(double x0, double z0, double t)
        {
            return x0 + _a * Math.Exp(k * z0) * Math.Cos(k * x0) * Math.Sin(_sigma * t);
        }

        public override double Z(double x0, double z0, double t)
        {
            return z0 + _a * Math.Exp(k * z0) * Math.Sin(k * x0) * Math.Sin(_sigma * t);
        }
    }
}
