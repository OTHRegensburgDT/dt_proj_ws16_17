using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MotorXPGUIMVVM.Annotations;

namespace MotorXPGUIMVVM.Controls.Gauge {
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GaugeAndGraph.Controls.Gauge"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GaugeAndGraph.Controls.Gauge;assembly=GaugeAndGraph.Controls.Gauge"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Gauge/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_BackgroundCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_NeedleCanvas", Type = typeof(Canvas))]
    public class Gauge : Control, INotifyPropertyChanged {



        #region DependencyProperties
        /// <summary>
        /// Property changed callback for dependency properties.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="Exception">Property changed callback was called without a handling switch case.</exception>
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
            var asGauge = dependencyObject as Gauge;
            if (asGauge == null) return;
            switch (dependencyPropertyChangedEventArgs.Property.Name) {
                case nameof(MinAngle):
                case nameof(MaxAngle):
                case nameof(InnerRadius):
                case nameof(GaugeFill):
                case nameof(StrokeLength):
                case nameof(Strokes):
                case nameof(StrokeColor):
                case nameof(GaugeBorderBrush):
                case nameof(CircleBackgroundBrush):
                case nameof(StrokeThickness):
                    asGauge.DrawBackground();
                    asGauge.RotateNeedle();
                    break;
                case nameof(NeedleColor):
                case nameof(NeedleWidth):
                case nameof(NeedleLength):
                case nameof(NeedleBackLength):
                case nameof(NeedleBorderBrush):
                    asGauge.DrawNeedle();
                    asGauge.RotateNeedle();
                    break;
                case nameof(MaxValue):
                case nameof(MinValue):
                case nameof(Value):
                    asGauge.RotateNeedle();
                    break;
                default:
                    throw new Exception("Property changed callback was called without a handling switch case.");
            }
        }

        #region GaugeBackground

        public static readonly DependencyProperty CircleBackgroundBrushProperty = DependencyProperty.Register(
            "CircleBackgroundBrush", typeof(Brush), typeof(Gauge), new PropertyMetadata(default(Brush), PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the circle background brush.
        /// </summary>
        /// <value>
        /// The circle background brush.
        /// </value>
        public Brush CircleBackgroundBrush {
            get { return (Brush)GetValue(CircleBackgroundBrushProperty); }
            set { SetValue(CircleBackgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty MinAngleProperty = DependencyProperty.Register(
            "MinAngle", typeof(double), typeof(Gauge), new PropertyMetadata(-Math.PI / 3, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the minimum angle of the gauge background. 0 = Straight line up.
        /// </summary>
        /// <value>
        /// The minimum angle.
        /// </value>
        public double MinAngle {
            get { return (double)GetValue(MinAngleProperty); }
            set { SetValue(MinAngleProperty, value); }
        }

        public static readonly DependencyProperty MaxAngleProperty = DependencyProperty.Register(
            "MaxAngle", typeof(double), typeof(Gauge), new PropertyMetadata(Math.PI / 3, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the maximum angle of the gauge background. 0 = Straight line up.
        /// </summary>
        /// <value>
        /// The maximum angle.
        /// </value>
        public double MaxAngle {
            get { return (double)GetValue(MaxAngleProperty); }
            set { SetValue(MaxAngleProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            "InnerRadius", typeof(double), typeof(Gauge), new PropertyMetadata(0.6, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the inner radius of the gauge background in percentage. 1 = 100%.
        /// </summary>
        /// <value>
        /// The inner radius.
        /// </value>
        public double InnerRadius {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }


        public static readonly DependencyProperty GaugeBorderBrushProperty = DependencyProperty.Register(
            "GaugeBorderBrush", typeof(Brush), typeof(Gauge), new PropertyMetadata(new SolidColorBrush(Colors.Black), PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the gauge border brush.
        /// </summary>
        /// <value>
        /// The gauge border brush.
        /// </value>
        public Brush GaugeBorderBrush {
            get { return (Brush)GetValue(GaugeBorderBrushProperty); }
            set { SetValue(GaugeBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty GaugeFillProperty = DependencyProperty.Register(
            "GaugeFill", typeof(Brush), typeof(Gauge), new PropertyMetadata(new SolidColorBrush(Colors.Gray), PropertyChangedCallback));


        #endregion
        #region Strokes

        /// <summary>
        /// Gets or sets the gauge fill.
        /// </summary>
        /// <value>
        /// The gauge fill.
        /// </value>
        public Brush GaugeFill {
            get { return (Brush)GetValue(GaugeFillProperty); }
            set { SetValue(GaugeFillProperty, value); }
        }


        public static readonly DependencyProperty StrokesProperty = DependencyProperty.Register(
            "Strokes", typeof(int), typeof(Gauge), new PropertyMetadata(12, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the amount of strokes distributed along the gauge control.
        /// </summary>
        /// <value>
        /// The strokes.
        /// </value>
        public int Strokes {
            get { return (int)GetValue(StrokesProperty); }
            set { SetValue(StrokesProperty, value); }
        }

        public static readonly DependencyProperty StrokeLengthProperty = DependencyProperty.Register(
            "StrokeLength", typeof(double), typeof(Gauge), new PropertyMetadata(0.5, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the length of the strokes in percent. 1 = 100%.
        /// </summary>
        /// <value>
        /// The length of the stroke.
        /// </value>
        public double StrokeLength {
            get { return (double)GetValue(StrokeLengthProperty); }
            set { SetValue(StrokeLengthProperty, value); }
        }

        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register(
            "StrokeColor", typeof(Brush), typeof(Gauge), new PropertyMetadata(new SolidColorBrush(Colors.White), PropertyChangedCallback));


        #endregion
        #region Needle

        /// <summary>
        /// Gets or sets the color of the strokes.
        /// </summary>
        /// <value>
        /// The color of the stroke.
        /// </value>
        public Brush StrokeColor {
            get { return (Brush)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        public static readonly DependencyProperty NeedleWidthProperty = DependencyProperty.Register(
            "NeedleWidth", typeof(double), typeof(Gauge), new PropertyMetadata(10.0, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the width of the needle in pixel.
        /// </summary>
        /// <value>
        /// The width of the needle.
        /// </value>
        public double NeedleWidth {
            get { return (double)GetValue(NeedleWidthProperty); }
            set { SetValue(NeedleWidthProperty, value); }
        }

        public static readonly DependencyProperty NeedleColorProperty = DependencyProperty.Register(
            "NeedleColor", typeof(Brush), typeof(Gauge), new PropertyMetadata(new SolidColorBrush(Colors.Red), PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the color of the needle.
        /// </summary>
        /// <value>
        /// The color of the needle.
        /// </value>
        public Brush NeedleColor {
            get { return (Brush)GetValue(NeedleColorProperty); }
            set { SetValue(NeedleColorProperty, value); }
        }

        public static readonly DependencyProperty NeedleBorderBrushProperty = DependencyProperty.Register(
            "NeedleBorderBrush", typeof(Brush), typeof(Gauge), new PropertyMetadata(default(Brush), PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the needle border brush.
        /// </summary>
        /// <value>
        /// The needle border brush.
        /// </value>
        public Brush NeedleBorderBrush {
            get { return (Brush)GetValue(NeedleBorderBrushProperty); }
            set { SetValue(NeedleBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty NeedleLengthProperty = DependencyProperty.Register(
            "NeedleLength", typeof(double), typeof(Gauge), new PropertyMetadata(0.8, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the length of the needle in percent. 1 = 100%.
        /// </summary>
        /// <value>
        /// The length of the needle.
        /// </value>
        public double NeedleLength {
            get { return (double)GetValue(NeedleLengthProperty); }
            set { SetValue(NeedleLengthProperty, value); }
        }

        public static readonly DependencyProperty NeedleBackLengthProperty = DependencyProperty.Register(
            "NeedleBackLength", typeof(double), typeof(Gauge), new PropertyMetadata(5.0, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the length of the needle back in pixel.
        /// </summary>
        /// <value>
        /// The length of the needle back.
        /// </value>
        public double NeedleBackLength {
            get { return (double)GetValue(NeedleBackLengthProperty); }
            set { SetValue(NeedleBackLengthProperty, value); }
        }

        #endregion

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", typeof(double), typeof(Gauge), new PropertyMetadata(100.0, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the maximum value, which will be the most right point of the gauge.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public double MaxValue {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", typeof(double), typeof(Gauge), new PropertyMetadata(0.0, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the minimum value, which will be the most left point of the gauge.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public double MinValue {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinMaxValueIndicatorBrushProperty = DependencyProperty.Register(
            "MinMaxValueIndicatorBrush", typeof(Brush), typeof(Gauge), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// Gets or sets the min/max value indicator brush.
        /// </summary>
        /// <value>
        /// The minimum maximum value indicator brush.
        /// </value>
        public Brush MinMaxValueIndicatorBrush {
            get { return (Brush) GetValue(MinMaxValueIndicatorBrushProperty); }
            set { SetValue(MinMaxValueIndicatorBrushProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(Gauge), new PropertyMetadata(50.0, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueIndicatorSizeProperty = DependencyProperty.Register(
            "ValueIndicatorSize", typeof(int), typeof(Gauge), new PropertyMetadata(15));

        /// <summary>
        /// Gets or sets the size of the value indicator.
        /// </summary>
        /// <value>
        /// The size of the value indicator.
        /// </value>
        public int ValueIndicatorSize
        {
            get { return (int) GetValue(ValueIndicatorSizeProperty); }
            set { SetValue(ValueIndicatorSizeProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(Gauge), new PropertyMetadata(1.0, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the size of the gauge.
        /// </summary>
        /// <value>
        /// The size of the gauge.
        /// </value>
        public double GaugeSize
        {
            get { return _gaugeSize; }
            private set
            {
                _gaugeSize = value; 
                OnPropertyChanged();
            }
        }
        

        #endregion
        #region Fields

        private Canvas _backgroundCanvas;
        private Canvas _needleCanvas;
        private double _gaugeSize;

        #endregion

        static Gauge() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Gauge), new FrameworkPropertyMetadata(typeof(Gauge)));

        }


        private void DrawBackground() {
            if (_backgroundCanvas == null) return;
            // clear canvas
            _backgroundCanvas.Children.Clear();

            // draw circle
            // radius = the biggest we can fit into the canvas. So take width or height, whatever is smaller
            var outerRadius = (_backgroundCanvas.ActualWidth > _backgroundCanvas.ActualHeight
                                  ? _backgroundCanvas.ActualHeight
                                  : _backgroundCanvas.ActualWidth) / 2; // divide by two because we want the radius
            GaugeSize = outerRadius*2 - 2; // set GaugeSize for TextFields

            var circle = new Ellipse();
            circle.Width = outerRadius * 2;
            circle.Height = circle.Width;
            circle.Fill = CircleBackgroundBrush;
            Canvas.SetTop(circle, _backgroundCanvas.ActualHeight/ 2 - outerRadius);
            Canvas.SetLeft(circle, _backgroundCanvas.ActualWidth / 2 - outerRadius);

            _backgroundCanvas.Children.Add(circle);

            #region Background

            // draw half circle
            var steps = 10000;
            var poly = new Polygon();

            var bgPoints = new PointCollection();
            var origin = new Point(_backgroundCanvas.ActualWidth / 2, _backgroundCanvas.ActualHeight / 2);


            var innerRadius = outerRadius * InnerRadius;
            // move to inner radius origin
            bgPoints.Add(GetPoint(origin, innerRadius, MinAngle));
            for (var i = 0; i <= steps; i++) {
                bgPoints.Add(GetPoint(origin, outerRadius, MinAngle + (MaxAngle - MinAngle) / Convert.ToDouble(steps) * i));

            }
            for (var i = steps; i > 1; i--) {
                bgPoints.Add(GetPoint(origin, innerRadius, MinAngle + (MaxAngle - MinAngle) / Convert.ToDouble(steps) * i));
            }

            // set colors and points
            poly.Points = bgPoints;
            poly.Fill = GaugeFill;
            poly.Stroke = GaugeBorderBrush;
            // add to canvas
            _backgroundCanvas.Children.Add(poly);

            #endregion

            // we dont want a strok at the start and end of the gauge. Therefore we start with i = 1 and don't go to the total number. However since we do this, to achieve the correct stroke amount, we have to count to one more than the Strokes property
            var strokeAmount = Strokes + 1;
            for (var i = 1; i < strokeAmount; i++) {
                var line = new Line();
                line.Stroke = StrokeColor;
                line.StrokeThickness = StrokeThickness;

                // move to inner radius + stroke length
                var innerPoint = GetPoint(origin, innerRadius + (outerRadius - innerRadius) * (1.0-StrokeLength),
                    MinAngle + (MaxAngle - MinAngle) / Convert.ToDouble(strokeAmount) * i);

                var outerPoint = GetPoint(origin, outerRadius, MinAngle + (MaxAngle - MinAngle) / Convert.ToDouble(strokeAmount) * i);

                // set the X and Y
                line.X1 = innerPoint.X;
                line.X2 = outerPoint.X;
                line.Y1 = innerPoint.Y;
                line.Y2 = outerPoint.Y;

                // add to the canvas
                _backgroundCanvas.Children.Add(line);
            }


        }

        private void DrawNeedle() {
            if (_needleCanvas == null) return;

            _needleCanvas.Children.Clear();

            var poly = new Polygon();
            var points = new PointCollection();

            var centerY = _needleCanvas.ActualHeight / 2;
            var centerX = _needleCanvas.ActualWidth / 2;
            // radius = the biggest we can fit into the canvas. So take width or height, whatever is smaller
            var outerRadius = (_backgroundCanvas.ActualWidth > _backgroundCanvas.ActualHeight
                                  ? _backgroundCanvas.ActualHeight
                                  : _backgroundCanvas.ActualWidth) / 2; // divide by two because we want the radius

            GaugeSize = outerRadius * 2 - 2; // set GaugeSize for TextFields
            var needleLength = outerRadius * NeedleLength;

            // draw needle like this >
            points.Add(new Point(centerX, centerY - NeedleWidth / 2));
            // draw back
            points.Add(new Point(centerX - NeedleBackLength, centerY));
            // lower part of arrow
            points.Add(new Point(centerX, centerY + NeedleWidth / 2));
            // needle top
            points.Add(new Point(centerX + needleLength, centerY));

            poly.Points = points;
            poly.Fill = NeedleColor;
            poly.Stroke = NeedleBorderBrush;
            poly.StrokeThickness = 1;

            _needleCanvas.Children.Add(poly);
        }

        private void RotateNeedle() {
            if (_needleCanvas == null) return;

            var percent = ((Value - MinValue) / (MaxValue - MinValue));
            var angle = MinAngle + (MaxAngle - MinAngle) * percent;
            angle -= Math.PI / 2; // adjust by 90 degrees (since we did the same thing in GetPoint, so that 0 = straight up)

            var centerY = _needleCanvas.ActualHeight / 2;
            var centerX = _needleCanvas.ActualWidth / 2;
            var rotateTransform = new RotateTransform(angle / Math.PI * 180, centerX, centerY);

            _needleCanvas.RenderTransform = rotateTransform;
        }

        private Point GetPoint(Point origin, double radius, double angle) {
            angle -= Math.PI / 2;
            var x = origin.X + Math.Cos(angle) * radius;
            var y = origin.Y + Math.Sin(angle) * radius;

            return new Point(x, y);
        }

        public override void OnApplyTemplate() {
            _backgroundCanvas = GetTemplateChild("PART_BackgroundCanvas") as Canvas;
            _needleCanvas = GetTemplateChild("PART_NeedleCanvas") as Canvas;


            base.OnApplyTemplate();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {

            base.OnRenderSizeChanged(sizeInfo);
            DrawBackground();
            DrawNeedle();
            RotateNeedle();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
