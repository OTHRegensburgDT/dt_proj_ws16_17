using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MotorXPGUIMVVM.Controls.LineChart
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GaugeAndGraph.Controls.LineChart"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GaugeAndGraph.Controls.LineChart;assembly=GaugeAndGraph.Controls.LineChart"
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
    ///     <MyNamespace:LineChart/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_GraphCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_ChartCanvas", Type = typeof(Canvas))]
    public class LineChart : Control, INotifyPropertyChanged
    {

        #region DependencyProperties

        /// <summary>
        /// Property changed callback for dependency properties.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="Exception">Property changed callback was called without a handling switch case.</exception>
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var asLineChart = dependencyObject as LineChart;
            if (asLineChart == null) return;
            switch (dependencyPropertyChangedEventArgs.Property.Name)
            {
                case nameof(IsShowingAll):
                case nameof(StrokeColor):
                case nameof(FillColor):
                    asLineChart.DrawGraph();
                    break;
                case nameof(SampleWindow):
                case nameof(WindowPosition):
                    asLineChart.DrawGraph();
                    // ReSharper disable once ExplicitCallerInfoArgument
                    asLineChart.OnPropertyChanged(nameof(FirstSample));
                    // ReSharper disable once ExplicitCallerInfoArgument
                    asLineChart.OnPropertyChanged(nameof(LastSample));
                    break;
                case nameof(AutoMinMax):
                    if ((bool)dependencyPropertyChangedEventArgs.NewValue) asLineChart.CalcMinMax();
                    break;
                case nameof(MaxValue):
                case nameof(MinValue):
                    // ReSharper disable once ExplicitCallerInfoArgument
                    asLineChart.OnPropertyChanged(nameof(MiddleValue));
                    asLineChart.DrawGraph();
                    asLineChart.DrawGrid();
                    break;
                case nameof(HorizontalLines):
                case nameof(VerticalLines):
                case nameof(GridLinesColor):
                case nameof(AxisColor):
                    asLineChart.DrawGrid();
                    break;
                case nameof(Values):

                    asLineChart.DrawGraph();
                    asLineChart.CalcMinMax();
                    if (asLineChart.Values != null)
                        asLineChart.Values.ListChanged += asLineChart.ValuesOnListChanged;
                    break;

                default:
                    throw new Exception("Property changed callback was called without a handling switch case.");
            }
        }

        private void ValuesOnListChanged(object sender, ListChangedEventArgs listChangedEventArgs)
        {
            CalcMinMax();
            DrawGraph();
        }


        public static readonly DependencyProperty IsShowingAllProperty = DependencyProperty.Register(
            "IsShowingAll", typeof(bool), typeof(LineChart), new PropertyMetadata(true, PropertyChangedCallback));


        /// <summary>
        /// Gets or sets a value indicating whether this instance is showing all. If true, SampleWindow is always the size of all samples.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is showing all; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowingAll
        {
            get { return (bool)GetValue(IsShowingAllProperty); }
            set { SetValue(IsShowingAllProperty, value); }
        }

        public static readonly DependencyProperty SampleWindowProperty = DependencyProperty.Register(
            "SampleWindow", typeof(int), typeof(LineChart), new PropertyMetadata(50, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the sample window. If IsShowingAll is false, this value decides how many samples are shown at once.
        /// </summary>
        /// <value>
        /// The sample window.
        /// </value>
        public int SampleWindow
        {
            get { return (int)GetValue(SampleWindowProperty); }
            set { SetValue(SampleWindowProperty, value); }
        }

        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register(
            "StrokeColor", typeof(Brush), typeof(LineChart), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 207, 29)), PropertyChangedCallback));

        public Brush StrokeColor
        {
            get { return (Brush)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register(
            "FillColor", typeof(Brush), typeof(LineChart), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(126, 120, 255, 139)), PropertyChangedCallback));

        public Brush FillColor
        {
            get { return (Brush)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        public static readonly DependencyProperty WindowPositionProperty = DependencyProperty.Register(
            "WindowPosition", typeof(double), typeof(LineChart), new PropertyMetadata(default(double), PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the window position of the displayed samples in percent. 1 = 100%
        /// </summary>
        /// <value>
        /// The window position.
        /// </value>
        public double WindowPosition
        {
            get { return (double)GetValue(WindowPositionProperty); }
            set { SetValue(WindowPositionProperty, value); }
        }

        public static readonly DependencyProperty AutoMinMaxProperty = DependencyProperty.Register(
            "AutoMinMax", typeof(bool), typeof(LineChart), new PropertyMetadata(default(bool), PropertyChangedCallback));

        /// <summary>
        /// Gets or sets a value indicating whether [minValue and maxValue is automatic].
        /// </summary>
        /// <value>
        /// <c>true</c> if [minValue and maxValue is automatically set]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoMinMax
        {
            get { return (bool)GetValue(AutoMinMaxProperty); }
            set { SetValue(AutoMinMaxProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", typeof(double), typeof(LineChart), new PropertyMetadata(default(double), PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the minimum value. If AutoMinMax is set to true, this value gets updated automatically.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", typeof(double), typeof(LineChart), new PropertyMetadata(50.0, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the maximum value. If AutoMinMax is set to true, this value gets updated automatically.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty ShowIndicatorsProperty = DependencyProperty.Register(
            "ShowIndicators", typeof(bool), typeof(LineChart), new PropertyMetadata(true));

        public bool ShowIndicators
        {
            get { return (bool)GetValue(ShowIndicatorsProperty); }
            set { SetValue(ShowIndicatorsProperty, value); }
        }

        public static readonly DependencyProperty VerticalLinesProperty = DependencyProperty.Register(
            "VerticalLines", typeof(int), typeof(LineChart), new PropertyMetadata(10, PropertyChangedCallback));

        public int VerticalLines
        {
            get { return (int)GetValue(VerticalLinesProperty); }
            set { SetValue(VerticalLinesProperty, value); }
        }

        public static readonly DependencyProperty HorizontalLinesProperty = DependencyProperty.Register(
            "HorizontalLines", typeof(int), typeof(LineChart), new PropertyMetadata(10, PropertyChangedCallback));

        public int HorizontalLines
        {
            get { return (int)GetValue(HorizontalLinesProperty); }
            set { SetValue(HorizontalLinesProperty, value); }
        }

        public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
            "Values", typeof(BindingList<double>), typeof(LineChart), new PropertyMetadata(new BindingList<double>(), PropertyChangedCallback));

        public BindingList<double> Values
        {
            get { return (BindingList<double>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        public static readonly DependencyProperty GridLinesColorProperty = DependencyProperty.Register(
            "GridLinesColor", typeof(Brush), typeof(LineChart), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(78, 0, 0, 0)), PropertyChangedCallback));

        public Brush GridLinesColor
        {
            get { return (Brush)GetValue(GridLinesColorProperty); }
            set { SetValue(GridLinesColorProperty, value); }
        }

        public static readonly DependencyProperty AxisColorProperty = DependencyProperty.Register(
            "AxisColor", typeof(Brush), typeof(LineChart), new PropertyMetadata(new SolidColorBrush(Colors.Black), PropertyChangedCallback));

        public Brush AxisColor
        {
            get { return (Brush)GetValue(AxisColorProperty); }
            set { SetValue(AxisColorProperty, value); }
        }

        #endregion

        #region Properties
        //Todo Error handling!!
        public double MiddleValue => MinValue + (MaxValue - MinValue) / 2;
        public int FirstSample => Convert.ToInt32(Convert.ToDouble(Values.Count - SampleWindow - 1) * WindowPosition);
        public int LastSample => FirstSample + SampleWindow;

        #endregion

        #region Fields

        private Canvas _gridCanvas;
        private Canvas _chartCanvas;
        //trigger for mouse over event
#pragma warning disable 414
        private bool _displayMouseInfo;
#pragma warning restore 414


        private static Random _rnd; // RNG for in design mode value generation. See OnLoaded
        #endregion

        static LineChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineChart), new FrameworkPropertyMetadata(typeof(LineChart)));
        }

        public LineChart()
        {
            Loaded += OnLoaded;
        }

        private void CalcMinMax()
        {
            foreach (var value in Values)
            {
                CheckMinMaxValue(value);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // if this control is displayed in the designer. Fill in sample values
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                if (_rnd == null) _rnd = new Random();
                var count = _rnd.Next(5, 20);
                var samples = new List<double>(count);
                for (var i = 0; i < count; i++)
                {
                    samples.Add(_rnd.NextDouble() * 100.0);
                }
                SetValues(samples);
            }
        }

        /// <summary>
        /// Sets a list of values to display at the graph.
        /// </summary>
        /// <param name="to">To.</param>
        public void SetValues(List<double> to)
        {
            Values = new BindingList<double>(to);
            if (AutoMinMax) CalcMinMax();
            DrawGraph();
        }

        /// <summary>
        /// Adds a value to the graph.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddValue(double value)
        {
            if (AutoMinMax) CheckMinMaxValue(value);
            Values.Add(value);
            DrawGraph();
            if (Values.Count == 1) DrawGrid(); // if the values were zero before (are now 1) Redraw the grid. (which has some problems with the max. width for some reason)
        }

        /// <summary>
        /// Checks if the given value is bigger or smaller then the min/max value fields and if so, updates the fields.
        /// </summary>
        /// <param name="forValue">For value.</param>
        private void CheckMinMaxValue(double forValue)
        {
            if (forValue < MinValue) MinValue = forValue;
            if (forValue > MaxValue) MaxValue = forValue;
        }

        /// <summary>
        /// Draws the grid.
        /// </summary>
        private void DrawGrid()
        {
            if (_gridCanvas == null) return;
            _gridCanvas.Children.Clear();

            var availableWidth = _chartCanvas.ActualWidth;
            var lowerY = 0.0;
            var upperY = _gridCanvas.ActualHeight;
            var lowerX = 0.0;
            var upperX = availableWidth;

            // axis
            #region Axis

            var line = new Line 
            {
                // Y axis
                X1 = lowerX,
                X2 = lowerX,
                Y1 = upperY,
                Y2 = lowerY,
                Stroke = AxisColor
            };
            _gridCanvas.Children.Add(line);
            line = new Line
            { 
                // X axis
                X1 = lowerX,
                X2 = upperX,
                Y1 = upperY,
                Y2 = upperY,
                Stroke = AxisColor
            };
            _gridCanvas.Children.Add(line);

            #endregion

            // horizontal lines
            // we dont want a line at the bottom and on the top. Therefore we start with i = 1. To still have the required amount HorizontalLines, we have to add one
            var horizontalLines = HorizontalLines + 1;
            for (var i = 1; i < horizontalLines; i++)
            {
                // only the y position of the lines change.
                var yPos = lowerY + (upperY - lowerY) / HorizontalLines * i;
                line = new Line()
                {
                    X1 = lowerX, // from left
                    X2 = upperX, // to right
                    Y1 = yPos,
                    Y2 = yPos,
                    Stroke = GridLinesColor
                };
                _gridCanvas.Children.Add(line);
            }
            // vertical lines
            var verticalLines = VerticalLines + 1;
            for (var i = 1; i < verticalLines; i++)
            {
                var xPos = lowerX + (upperX - lowerX) / VerticalLines * i;
                line = new Line()
                {
                    X1 = xPos,
                    X2 = xPos,
                    Y1 = lowerY,//from top
                    Y2 = upperY, // to bottom
                    Stroke = GridLinesColor
                };
                _gridCanvas.Children.Add(line);
            }
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        private void DrawGraph()
        {
            // check if OnApplyTemplate received a canvas, check if we have at least one sample
            if (_chartCanvas == null) return;
            if (Values == null) return;
            if (Values.Count == 0) return;
            // clear the canvas
            _chartCanvas.Children.Clear();

            var availableWidth = _chartCanvas.ActualWidth;
            var firstSample = 0;
            var lastSample = Values.Count - 1;

            var samples = lastSample - firstSample;

            // if we have more samples than we show in our window AND IsShowingAll is false, set the first sample and the amount
            if (samples > SampleWindow && !IsShowingAll)
            {
                firstSample = Convert.ToInt32(Convert.ToDouble(Values.Count - SampleWindow - 1) * WindowPosition); // if the slider is most to the right (WindowPosition = 1), we want to show the last SampleWindow samples. Therefore our biggest first sample is _values.Count - SampleWindow.
                lastSample = firstSample + SampleWindow;
                // check if last sample went out of bounds
                if (lastSample >= Values.Count) lastSample = Values.Count - 1;
                samples = SampleWindow;
            }

            // canvas bounds
            var lowerY = 0.0;
            var upperY = _gridCanvas.ActualHeight;
            var lowerX = 0.0;
            var upperX = availableWidth;

            // we use a polygon to draw the graph (so we can fill in the area under it)
            var poly = new Polygon();
            // draw lower rectangle border (so we can fill the area)
            var points = new PointCollection
            {
                new Point(upperX, upperY),
                new Point(lowerX, upperY)
            };

            var pixelPerSample = availableWidth / Convert.ToDouble(samples); // how many pixel one sample displays
            var samplePerPixel = Convert.ToDouble(samples) / availableWidth; // how many samples are merged into one pixel

            if (pixelPerSample < 1)
            {
                // we have more samples than pixel. One pixel = multiple sample
                for (var i = 0; i < availableWidth - 1; i += 2)
                { // we draw two points at a time. The minimum and maximum of a sample window which represents two pixel
                    // receive the biggest and the smallest number in the sample window for the current pixel (we don't middle the values, so the graph doesn't distort so much)
                    var values = GetBiggestAndSmallesValue(Convert.ToInt32(firstSample + samplePerPixel * i),
                        Convert.ToInt32(firstSample + samplePerPixel * (i + 1)));

                    // translate the local min and max values into percent
                    var minValueInPercent = 1.0 - (values[0] - MinValue) / (MaxValue - MinValue);
                    var maxValueInPercent = 1.0 - (values[1] - MinValue) / (MaxValue - MinValue);

                    // translate it to points on the canvas
                    points.Add(new Point(lowerX + i, lowerY + (upperY - lowerY) * minValueInPercent));
                    points.Add(new Point(lowerX + i + 1, lowerY + (upperY - lowerY) * maxValueInPercent));
                }
            }
            else
            {
                // we have more pixel than samples. One Sample = multiple pixel
                var sampleNo = 0; // count at which no of sample we currently are
                for (var sampleIndex = firstSample; sampleIndex <= lastSample; sampleIndex++)
                { // iterate through each sample we want to show

                    var value = Values[sampleIndex]; // get the value for the sample index
                    var valueInPercent = 1.0 - (value - MinValue) / (MaxValue - MinValue); // calculate it in percent

                    var xPos = sampleNo * pixelPerSample; // translate to the graph position (no of the sample * the amount of pixel per sample)
                    points.Add(new Point(lowerX + xPos, lowerY + (upperY - lowerY) * valueInPercent)); // add as point
                    sampleNo++; // increase the current sample number
                }

            }
            // set the strok and fill color, add the points to the polygon and add it to the canvas
            poly.Points = points;
            poly.Stroke = StrokeColor;
            poly.Fill = FillColor;

            _chartCanvas.Children.Add(poly);

        }

        /// <summary>
        /// Gets the biggest and smalles value of the given sample window.
        /// </summary>
        /// <param name="fromIndex">From index.</param>
        /// <param name="toIndex">To index.</param>
        /// <returns>List of points with the first being the smalles and the last being the biggest. Always has a size of 2.</returns>
        private List<double> GetBiggestAndSmallesValue(int fromIndex, int toIndex)
        {
            var localMinValue = Double.MaxValue;
            var localMaxValue = Double.MinValue;
            // track where the local min and max occured as index
            var minIndex = -1;
            var maxIndex = -1;

            for (var i = fromIndex; i <= toIndex; i++)
            {
                if (i < 0)
                {
                    i = 0;
                }
                if (i >= Values.Count)
                {
                    i = Values.Count-1;
                }
                else if (i < 0)
                {
                    i = 0;
                }
                var val = Values[i];
                if (val < localMinValue)
                {
                    localMinValue = val;
                    minIndex = i;
                }
                if (val > localMaxValue)
                {
                    localMaxValue = val;
                    maxIndex = i;
                }
            }
            // return either first the min then the max or vice versa. If we would always return the min (or max) first, we might distort our data. 
            // E.g. for a sample window like this: 10, 5, 2. Which would return 2 as min and 10 as max. However 2 came AFTER the 10. So a distortion would appear
            return minIndex < maxIndex ? new List<double> { localMinValue, localMaxValue } : new List<double> { localMaxValue, localMinValue };
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            _chartCanvas = GetTemplateChild("PART_ChartCanvas") as Canvas;
            _gridCanvas = GetTemplateChild("PART_GridCanvas") as Canvas;
            var dp = GetTemplateChild("PART_DockPanel") as DockPanel;

            if (dp != null)
            {
                dp.MouseWheel += ChartCanvasOnMouseWheel;
                dp.MouseEnter += DpOnMouseEnter;
                dp.MouseLeave += DpOnMouseLeave;
            }
            base.OnApplyTemplate();
        }

        private void DpOnMouseEnter(object sender, MouseEventArgs e)
        {
            _displayMouseInfo = true;
        }

        private void DpOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            _displayMouseInfo = false;
        }

        private void ChartCanvasOnMouseWheel(object sender, MouseWheelEventArgs mouseWheelEventArgs)
        {
            if (mouseWheelEventArgs.Delta < 0) SampleWindow += 10;
            else SampleWindow -= 10;
            if (SampleWindow < 5) SampleWindow = 5;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.SizeChanged" /> event, using the specified information as part of the eventual event data.
        /// </summary>
        /// <param name="sizeInfo">Details of the old and new size involved in the change.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            DrawGraph();
            DrawGrid();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
