﻿#pragma warning disable 612

using System;
using System.IO;
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
            XAxis = new LinearAxis(AxisPosition.Bottom, -5, 5, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
            PlotModel.Axes.Add(XAxis);
            YAxis = new LinearAxis(AxisPosition.Left, -5, 5, "Z") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Y", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
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