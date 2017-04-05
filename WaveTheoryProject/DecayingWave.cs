using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    class DecayingWaveController : WaveController
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

        public override double k
        {
            get
            {
                return base._k;
            }
            set
            {
                base._k = value;
                OnValueChanged?.Invoke(null, null, base._k, null, null, null);
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
                OnValueChanged?.Invoke(null, null, null, base._a, null, null);
            }
        }

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

        double dissipationCoeff;

        public double Beta
        {
            get
            {
                return dissipationCoeff;
            }
            set
            {
                Settings.DecayingWave.Beta = value;
                dissipationCoeff = value;
                Refresh();
            }
        }

        public override double sigma => Math.Sqrt(_k * g);

        public override event ValueChangedHandler OnValueChanged;

        public DecayingWaveController()
        {
            _x0fixed = Settings.Init.x0;
            _z0fixed = Settings.Init.z0;
            base._k = Settings.k;
            base._a = Settings.a;
            base._p0 = Settings.p0;
            base._ro = Settings.ro;
            dissipationCoeff = Settings.DecayingWave.Beta;
            OnValueChanged += ValueChangedMethod;
            FillWavePointsFixedX(x0fixed, z0fixed, Settings.InitTimeFrom, Settings.InitTimeTo, Settings.Time_h);
            FillWavePointsTimeline(z0fixed);
        }

        void ValueChangedMethod(double? x0, double? z0, double? k, double? a, double? p0, double? ro)
        {
            _x0fixed = Settings.Init.x0 = x0.HasValue ? (double)x0 : x0fixed;
            _z0fixed = Settings.Init.z0 = z0.HasValue ? (double)z0 : this.z0fixed;
            base._a = Settings.a = a.HasValue ? (double)a : base._a;
            base._k = Settings.k = k.HasValue ? (double)k : base._k;
            base._p0 = Settings.p0 = p0.HasValue ? (double)p0 : base._p0;
            base._ro = Settings.ro = ro.HasValue ? (double)ro : base._ro;
            FillWavePointsFixedX(x0fixed, this.z0fixed, Settings.InitTimeFrom, Settings.InitTimeTo, Settings.Time_h);
            if (z0.HasValue || k.HasValue || a.HasValue)
            {
                FillWavePointsTimeline(this.z0fixed);
            }
        }

        public override void Refresh()
        {
            OnValueChanged?.Invoke(null, null, null, null, null, null);
            FillWavePointsTimeline(this.z0fixed);
        }
        /// <summary>
        /// To be added
        /// </summary>
        public override double Vx(double x, double z, double t)
        {
            //return _a * sigma * Math.Exp(_k * z) * Math.Cos(sigma * t + _k * x);
            return 0;
        }
        /// <summary>
        /// To be added
        /// </summary>
        public override double Vz(double x, double z, double t)
        {
            //return _a * sigma * Math.Exp(_k * z) * Math.Sin(sigma * t + _k * x);
            return 0;
        }

        public override double X(double x0, double z0, double t)
        {
            return x0 + _a * Math.Exp(_k * z0) * Math.Sin(sigma * t + _k * x0)*Math.Exp(-dissipationCoeff*t);
        }

        public override double Z(double x0, double z0, double t)
        {
            return z0 - _a * Math.Exp(_k * z0) * Math.Cos(sigma * t + _k * x0) * Math.Exp(-dissipationCoeff*t);
        }
        /// <summary>
        /// To be added
        /// </summary>
        public override double P(double x, double z, double t)
        {
            //return _p0 - _ro * g * z - _a * g * _ro * Math.Exp(_k * z) * Math.Cos(sigma * t + _k * x);
            return 0;
        }
    }
}
