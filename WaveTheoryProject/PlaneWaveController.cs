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
        public override double x0
        {
            get
            {
                return base._x0;
            }
            set
            {
                base._x0 = value;
                OnValueChanged?.Invoke(base._x0, null, null, null);
            }
        }
        public override double z0
        {
            get
            {
                return base._z0;
            }
            set
            {
                base._z0 = value;
                OnValueChanged?.Invoke(null, base._z0, null, null);
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
        public override double k => sigma*sigma/Settings.g;

        public override event ValueChangedHandler OnValueChanged;

        public PlaneWaveController()
        {
            OnValueChanged += ValueChangedMethod;
            base._x0 = Settings.Init.x0;
            base._z0 = Settings.Init.z0;
            base._sigma = Settings.Init.sigma;
            base._a = Settings.Init.a;
            WavePoints = new List<WavePoint>();
            FillWavePointsDictionary(Settings.InitTimeFrom, Settings.InitTimeTo, Settings.Time_h);
        }

        void ValueChangedMethod(double? x0, double? z0, double? sigma, double? a)
        {
            base._x0 = x0.HasValue ? (double)x0 : base._x0;
            base._z0 = z0.HasValue ? (double)z0 : base._z0;
            base._sigma = sigma.HasValue ? (double)sigma : base._sigma;
            base._a = a.HasValue ? (double)a : base._a;
            WavePoints.Clear();
            FillWavePointsDictionary(Settings.InitTimeFrom, Settings.InitTimeTo, Settings.Time_h);
        }

        public override double Vx(double x, double z, double t)
        {
            return a * sigma * Math.Exp(k * z) * Math.Cos(k * x) * Math.Cos(sigma * t);
        }

        public override double Vz(double x, double z, double t)
        {
            return a * sigma * Math.Exp(k * z) * Math.Sin(k * x) * Math.Cos(sigma * t);
        }

        public override double X(double t)
        {
            return x0 + a * Math.Exp(k * z0) * Math.Cos(k * x0) * Math.Sin(sigma * t);
        }

        public override double Z(double t)
        {
            return z0 + a * Math.Exp(k * z0) * Math.Sin(k * x0) * Math.Sin(sigma * t);
        }
    }
}
