//#define NEW_CAP

using OxyPlot;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WaveTheoryProject
{
    static class Settings
    {
        internal static class Init
        {
            public static double x0 = 0;
            public static double z0 = 0;
        }
        internal static class Canal
        {
            public static double delta = 3;
            public static double h = 1;
#if NEW_CAP
            public static double a0 = 0;
#else
            public static double a0 = 1.0 / 3.0;
#endif
        }
        internal static class WaveGroup
        {
            public static double kMultiplicator = 2;
        }
        internal static class DecayingWave
        {
            public static double Beta = 0.1;
        }
        public static double k = 0.5;
        public static double a = 0.2;
        public static double ro = 1000;
        public static double p0 = 101300;
        public static double InitTimeFrom = 0;
        public static double InitTimeTo = 10;
        public static double Time_h = 0.1;
        public static double InitX0From = -10;
        public static double InitX0To = 10;
        public static double X0_h = 0.05;
        public static ushort Precision { get { return _Precision.Decimals; } set { _Precision.Decimals = value; } }
        public static string Format => _Precision.Format;
        public static double Eps => _Precision.Eps;
        static class _Precision
        {
            private static ushort _d;
            public static double Eps { get; private set; }
            public static UInt16 Decimals
            {
                get
                {
                    return _d;
                }
                set
                {
                    _d = value;
                    Eps = Math.Pow(10, -_d);
                    Format = "0.";
                    for (int i = 1; i <= _d; i++)
                    {
                        Format += "#";
                    }
                }
            }
            public static string Format { get; private set; }
            static _Precision()
            {
                Decimals = 4;
            }
        }
        internal static ImageSource exitIcoSource= new BitmapImage(new Uri(@"Resources/exitIco.bmp", UriKind.Relative));
        internal static ImageSource exitIcoSelectedSource = new BitmapImage(new Uri(@"Resources/exitSelectedIco.bmp", UriKind.Relative));
    }
}
