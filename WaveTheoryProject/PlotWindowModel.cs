#pragma warning disable 612
//#define NEW_CAP

using System;
using System.Collections.Generic;
using System.ComponentModel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace WaveTheoryProject
{
    class PlotWindowModel
    {
        private PlotModel p;

        private ArrowAnnotation arrow;

        private TextAnnotation arrowText;

        object locker = new object();

        public PlotModel PlotModel
        {
            get { return p; }
            set { p = value; OnPropertyChanged("PlotModel"); }
        }

        private Axis X_Axis
        {
            get { return PlotModel.Axes[0] as Axis; }
        }

        private Axis Y_Axis
        {
            get { return PlotModel.Axes[1] as Axis; }
        }

        public DataPoint GetDataPointCursorPositionOnPlot(ScreenPoint pos)
        {
            return OxyPlot.Axes.Axis.InverseTransform(pos, X_Axis, Y_Axis);
        }

        internal PlotWindowModel()
        {
            PlotModel = new PlotModel();
            SetUpModel();
        }

        void SetUpModel()
        {
            PlotModel.LegendTitle = null;
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;
            LinearAxis XAxis, YAxis;
            XAxis = new LinearAxis(AxisPosition.Bottom, -10, 10, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X", AbsoluteMaximum = 10, AbsoluteMinimum = -10, Font = "Times New Roman", FontSize = 15 };
            PlotModel.Axes.Add(XAxis);
            YAxis = new LinearAxis(AxisPosition.Left, -2, 2, "Z") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Z", AbsoluteMaximum = 10, AbsoluteMinimum = -10, Font = "Times New Roman", FontSize = 15 };
            PlotModel.Axes.Add(YAxis);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void DrawCurve(List<DataPoint> l)
        {
            lock (locker)
            {
                LineSeries ls = new LineSeries();
                ls.Smooth = true;
                ls.Color = OxyColors.Blue;
                ls.StrokeThickness = 2;
                foreach (DataPoint p in l) { ls.Points.Add(p); }
                PlotModel.Series.Add(ls);
            }
        }

        public void DrawCurve(WavePointsListAtTime l)
        {
            lock (locker)
            {
                PlotModel.Series.Clear();
                try
                {
                    PlotModel.Series.Add(l.WaveLineSeries);
                }
                catch
                {
                    PlotModel.Series.Remove(l.WaveLineSeries);
                    PlotModel.Series.Add(l.WaveLineSeries);
                }
            }
        }

        public void DrawCurve(LineSeries l)
        {
            lock (locker)
            {
                try
                {
                    PlotModel.Series.Add(l);
                }
                catch
                {
                    PlotModel.Series.Remove(l);
                    PlotModel.Series.Add(l);
                }
            }
        }

        public void DrawCanal()
        {
#if !NEW_CAP
            PolygonAnnotation Border = new PolygonAnnotation();
            Border.Fill = OxyColors.Transparent;
            Border.Points.Add(new DataPoint(0, 10));
            Border.Points.Add(new DataPoint(0, -Settings.Canal.h));
            Border.Points.Add(new DataPoint(Settings.Canal.delta, -Settings.Canal.h));
            Border.Points.Add(new DataPoint(Settings.Canal.delta, 10));
            Border.StrokeThickness = 2;
            Border.Stroke = OxyColors.Black;
            PlotModel.Annotations.Add(Border);
            PolygonAnnotation BorderDashPart = new PolygonAnnotation();
            BorderDashPart.Fill = OxyColors.Transparent;
            BorderDashPart.Points.Add(new DataPoint(0, -1.0/3.0));
            BorderDashPart.Points.Add(new DataPoint(Settings.Canal.delta, 2.0 / 3.0));
            BorderDashPart.StrokeThickness = 1;
            BorderDashPart.LineStyle = LineStyle.Dot;
            BorderDashPart.Stroke = OxyColors.Gray;
            PlotModel.Annotations.Add(BorderDashPart);
            TextAnnotation t = new TextAnnotation();
            t.Text = "x/δ - 1/3";
            t.TextPosition = new DataPoint(Settings.Canal.delta+1, 2.0 / 3.0);
            t.TextColor = OxyColors.Black;
            t.Background = OxyColors.Transparent;
            t.StrokeThickness = 0;
            t.FontSize = 20;
            PlotModel.Annotations.Add(t);
#else
            PolygonAnnotation Border = new PolygonAnnotation();
            Border.Fill = OxyColors.Transparent;
            Border.Points.Add(new DataPoint(0, 10));
            Border.Points.Add(new DataPoint(0, -Settings.Canal.h));
            Border.Points.Add(new DataPoint(Settings.Canal.delta, -Settings.Canal.h));
            Border.Points.Add(new DataPoint(Settings.Canal.delta, 10));
            Border.StrokeThickness = 2;
            Border.Stroke = OxyColors.Black;
            PlotModel.Annotations.Add(Border);
            PolygonAnnotation BorderDashPart = new PolygonAnnotation();
            BorderDashPart.Fill = OxyColors.Transparent;
            BorderDashPart.Points.Add(new DataPoint(0, -Settings.Canal.delta *Math.Sqrt(3) / 6.0));
            BorderDashPart.Points.Add(new DataPoint(Settings.Canal.delta, Settings.Canal.delta * Math.Sqrt(3) / 6.0));
            BorderDashPart.StrokeThickness = 1;
            BorderDashPart.LineStyle = LineStyle.Dot;
            BorderDashPart.Stroke = OxyColors.Gray;
            PlotModel.Annotations.Add(BorderDashPart);
            TextAnnotation t = new TextAnnotation();
            t.Text = "x*sqrt(3)/3 - δ*sqrt(3)/6";
            t.TextPosition = new DataPoint(Settings.Canal.delta - 4, Settings.Canal.delta * Math.Sqrt(3) / 6.0);
            t.TextColor = OxyColors.Black;
            t.Background = OxyColors.Transparent;
            t.StrokeThickness = 0;
            t.FontSize = 20;
            PlotModel.Annotations.Add(t);
#endif
        }

        public void DeleteCanale()
        {
            PlotModel.Annotations.Clear();
        }

        void CreateArrow()
        {
            arrow = new ArrowAnnotation()
            {
                Color = OxyColors.Black,
                StrokeThickness = 2,
                StartPoint = new DataPoint(0, 0),
                EndPoint = new DataPoint(0, 0),
            };
            arrowText = new TextAnnotation()
            {
                FontSize = 25,
                Background = OxyColors.White,
                Font = "Times New Roman",
                StrokeThickness = 2,
                Stroke = OxyColors.Black,
            };
            PlotModel.Annotations.Add(arrow);
            PlotModel.Annotations.Add(arrowText);
        }

        void DeleteArrow()
        {
            if (arrow != null)
            {
                PlotModel.Annotations.Remove(arrow);
            }
            if (arrowText != null)
            {
                PlotModel.Annotations.Remove(arrowText);
            }
        }

        bool HasArrow()
        {
            foreach (Annotation a in PlotModel.Annotations)
            {
                if (a is ArrowAnnotation) { return true; }
            }
            return false;
        }

        public void Clear()
        {
            PlotModel.Series.Clear();
            PlotModel.Annotations.Clear();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
