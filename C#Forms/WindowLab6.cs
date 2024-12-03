using OxyPlot.Series;
using OxyPlot;
using System.Data;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;
using Label = System.Windows.Forms.Label;
using OxyPlot.Annotations;

namespace C_Forms
{
    public partial class WindowLab6 : Form
    {
        private Menu mainForm;
        private TextBox NTextBox_1 = new TextBox();
        private Button calculateButton_1 = new Button();
        private Label answerLabel_1 = new Label();
        private Label absErrorLabel_1 = new Label();
        private Label relErrorLabel_1 = new Label();
        private static int n_1 = 14;
        private PlotModel plotModel_1 = new PlotModel { Title = "График" };
        private static double b_1 = 32, hx_1 = 22.4, a_1 = 16;

        public WindowLab6(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka ?? throw new ArgumentNullException(nameof(menushka)); // Проверяем, что menushka не null
            this.Size = new Size(1000, 600);
            this.FormClosed += WindowLab6_FormClosed;

            // Инициализируем объекты
            plotView = new PlotView { Model = plotModel_1, Dock = DockStyle.Top, Height = 300 };
        }

        private void WindowLab6_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        private void ClearPlotAndControls()
        {
            // Очистка графика
            plotModel_1.Axes.Clear();
            plotModel_1.Series.Clear();
            plotModel_1.Annotations.Clear();

            // Очистка элементов управления
            answerLabel_1.Text = string.Empty;
            absErrorLabel_1.Text = string.Empty;
            relErrorLabel_1.Text = string.Empty;

            // Очистка текстового поля NTextBox_1
            NTextBox_1.Clear();
        }

        private void WindowLab6_N1()
        {
            plotModel_1.Axes.Clear(); // Clear any previous axes
            plotModel_1.Series.Clear(); // Clear any previous series
            plotModel_1.Annotations.Clear(); // Clear previous annotations
            plotView.Dock = DockStyle.Top;
            plotView.Height = 300;
            plotModel_1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X" });
            plotModel_1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y" });

            var functionSeries = new LineSeries { Title = "f(x)" };
            for (double x = 0.0; x < hx_1; x += 0.2)
            {
                double y = 10 * x / n_1;
                functionSeries.Points.Add(new DataPoint(x, y));
            }
            for (double x = hx_1; x <= b_1; x += 0.2)
            {
                double y = 10 * (x - 20) / (n_1 - 20) + 20;
                functionSeries.Points.Add(new DataPoint(x, y));
            }
            plotModel_1.Series.Add(functionSeries);
            plotView.Model = plotModel_1;

            var inputPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 300
            };

            var NLabel = new Label { Text = "Введите количество точек N:", Location = new Point(10, 10), AutoSize = true };
            NTextBox_1.Location = new Point(250, 10);

            calculateButton_1.Text = "Рассчитать";
            calculateButton_1.Location = new Point(10, 50);
            calculateButton_1.Size = new Size(150, 30);
            calculateButton_1.Click += CalculateButton_Click_N1;

            answerLabel_1.Location = new Point(10, 80);
            answerLabel_1.AutoSize = true;

            absErrorLabel_1.Location = new Point(10, 110);
            absErrorLabel_1.AutoSize = true;

            relErrorLabel_1.Location = new Point(10, 140);
            relErrorLabel_1.AutoSize = true;

            inputPanel.Controls.Add(NLabel);
            inputPanel.Controls.Add(NTextBox_1);
            inputPanel.Controls.Add(calculateButton_1);
            inputPanel.Controls.Add(answerLabel_1);
            inputPanel.Controls.Add(absErrorLabel_1);
            inputPanel.Controls.Add(relErrorLabel_1);

            this.Controls.Add(inputPanel);
            this.Controls.Add(plotView);
        }

        /// <summary>
        /// Выводит результаты
        /// </summary>
        private void CalculateButton_Click_N1(object sender, EventArgs e)
        {
            if (!int.TryParse(NTextBox_1.Text, out int N) || N <= 0)
            {
                MessageBox.Show("Введите корректное значение N.");
                return;
            }

            var (area, randomPoints, innerPoints) = AreaCalculation_N1(N);
            answerLabel_1.Text = $"Площадь треугольника: {Math.Round(area, 4)}";

            double absError = Math.Abs(256.0 - area);
            absErrorLabel_1.Text = $"Абсолютная погрешность: {Math.Round(absError, 2)}";

            double relError = absError / 100.0;
            relErrorLabel_1.Text = $"Относительная погрешность: {Math.Round(relError, 4):P2}";

            var scatterSeriesInner = new ScatterSeries { Title = "Внутренние точки", MarkerType = MarkerType.Circle, MarkerFill = OxyColors.Orange, MarkerSize = 4 };
            foreach (var point in innerPoints)
            {
                scatterSeriesInner.Points.Add(new ScatterPoint(point.Item1, point.Item2));
            }

            var scatterSeriesOuter = new ScatterSeries { Title = "Внешние точки", MarkerType = MarkerType.Circle, MarkerFill = OxyColors.Red, MarkerSize = 4 };
            foreach (var point in randomPoints.Except(innerPoints))
            {
                scatterSeriesOuter.Points.Add(new ScatterPoint(point.Item1, point.Item2));
            }

            for (int i = plotModel_1.Series.Count - 1; i >= 0; i--)
            {
                if (plotModel_1.Series[i] is ScatterSeries)
                {
                    plotModel_1.Series.RemoveAt(i);
                }
            }
            plotModel_1.Series.Add(scatterSeriesInner);
            plotModel_1.Series.Add(scatterSeriesOuter);

            plotView.InvalidatePlot(true);
        }

        /// <summary>
        /// Заданная функция
        /// </summary>
        private double F_N1(double x)
        {
            if (x < hx_1)
                return 10.0 * x / n_1;
            else
                return 10.0 * (x - 20) / (n_1 - 20) + 20;
        }

        /// <summary>
        /// Вычисление площади треугольника
        /// </summary>
        /// <param name="N">Количество точек</param>
        /// <returns> Кортеж из площади, списка рандомных точек и списка внутренних точек</returns>
        private (double, List<(double, double)>, List<(double, double)>) AreaCalculation_N1(int N)
        {
            var randomPoints = new List<(double, double)>();
            var rand = new Random();

            for (int i = 0; i < N; i++)
            {
                double x = rand.NextDouble() * b_1;
                double y = rand.NextDouble() * a_1;
                randomPoints.Add((x, y));
            }

            var innerPoints = randomPoints.Where(point => point.Item2 < F_N1(point.Item1)).ToList();
            double area = (innerPoints.Count / (double)N) * a_1 * b_1;

            return (area, randomPoints, innerPoints);
        }

        private void WindowLab6_N2()
        {
            // Set up the axes
            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "X", Minimum = 0 };
            var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Y", Minimum = 0 };
            plotModel_1.Axes.Add(xAxis);
            plotModel_1.Axes.Add(yAxis);

            // Create a series for the function f(x)
            var functionSeries = new LineSeries { Title = "f(x)" };
            for (double x = 0.0; x <= 5.5; x += 0.1)
            {
                double y = F_N2(x);
                functionSeries.Points.Add(new DataPoint(x, y));
            }
            plotModel_1.Series.Add(functionSeries);

            // Add a vertical line at x = 5
            var verticalLine = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                X = 5,
                Color = OxyColors.Blue,
                LineStyle = LineStyle.Solid,
                StrokeThickness = 2
            };
            plotModel_1.Annotations.Add(verticalLine);

            // Add the PlotView to the form
            plotView.Model = plotModel_1;

            // Create the input panel with labels, textbox, and button
            var inputPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 300
            };

            var NLabel = new Label { Text = "Введите количество точек N:", Location = new Point(10, 10), AutoSize = true };
            NTextBox_1.Location = new Point(250, 10);

            calculateButton_1.Text = "Рассчитать";
            calculateButton_1.Location = new Point(10, 50);
            calculateButton_1.Size = new Size(150, 30);
            calculateButton_1.Click += CalculateButton_Click_N2;

            answerLabel_1.Location = new Point(10, 80);
            answerLabel_1.AutoSize = true;

            absErrorLabel_1.Location = new Point(10, 110);
            absErrorLabel_1.AutoSize = true;

            relErrorLabel_1.Location = new Point(10, 140);
            relErrorLabel_1.AutoSize = true;

            inputPanel.Controls.Add(NLabel);
            inputPanel.Controls.Add(NTextBox_1);
            inputPanel.Controls.Add(calculateButton_1);
            inputPanel.Controls.Add(answerLabel_1);
            inputPanel.Controls.Add(absErrorLabel_1);
            inputPanel.Controls.Add(relErrorLabel_1);

            this.Controls.Add(inputPanel);
            this.Controls.Add(plotView);
        }

        private double F_N2(double x)
        {
            return Math.Sqrt(29 - 14 * Math.Pow(Math.Cos(x), 2));
        }

        private Tuple<double, List<(double, double)>, List<(double, double)>> AreaCalculation_N2(int N)
        {
            double a = 5.38516, b = 5.0;

            List<(double, double)> random_points = new List<(double, double)>();
            Random rnd = new Random();

            // Generate random points
            for (int i = 0; i < N; i++)
            {
                double x = rnd.NextDouble() * b;
                double y = rnd.NextDouble() * a;
                random_points.Add((x, y));
            }

            List<(double, double)> inner_points = new List<(double, double)>();

            // Check which points are inside the function's curve
            foreach ((double, double) point in random_points)
            {
                if (point.Item2 < F_N2(point.Item1))
                {
                    inner_points.Add(point);
                }
            }

            double area = (inner_points.Count / (double)N) * a * b;
            return new Tuple<double, List<(double, double)>, List<(double, double)>>(area, random_points, inner_points);
        }

        private void CalculateButton_Click_N2(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NTextBox_1.Text))
            {
                // Remove any existing points
                var seriesToRemoveInner = plotModel_1.Series.FirstOrDefault(s => s.Title == "Внутренние точки") as ScatterSeries;
                var seriesToRemoveOuter = plotModel_1.Series.FirstOrDefault(s => s.Title == "Внешние точки") as ScatterSeries;

                if (seriesToRemoveInner != null && seriesToRemoveOuter != null)
                {
                    plotModel_1.Series.Remove(seriesToRemoveInner);
                    plotModel_1.Series.Remove(seriesToRemoveOuter);
                }

                // Parse the number of points
                int N = Int32.Parse(NTextBox_1.Text);

                // Calculate the area using Monte Carlo method
                var result = AreaCalculation_N2(N);
                answerLabel_1.Text = $"Интеграл: {Math.Round(result.Item1, 4)}";

                // Calculate and display errors
                double exactValue = 14.8598209229187;
                double absoluteError = Math.Abs(exactValue - result.Item1);
                absErrorLabel_1.Text = $"Абсолютная погрешность: {Math.Round(absoluteError, 4)}";

                double relativeError = absoluteError / exactValue;
                relErrorLabel_1.Text = $"Относительная погрешность: {Math.Round(relativeError, 4)}";

                // Get random points and inner points
                List<(double, double)> randomPoints = result.Item2;
                List<(double, double)> innerPoints = result.Item3;

                // Define the outer points (points that are not inside the function)
                List<(double, double)> outerPoints = randomPoints.Except(innerPoints).ToList();

                // Create ScatterSeries for inner and outer points
                var scatterSeriesInner = new ScatterSeries { Title = "Внутренние точки", MarkerType = MarkerType.Circle, MarkerFill = OxyColors.Orange, MarkerSize = 4 };
                foreach (var point in innerPoints)
                {
                    scatterSeriesInner.Points.Add(new ScatterPoint(point.Item1, point.Item2));
                }

                var scatterSeriesOuter = new ScatterSeries { Title = "Внешние точки", MarkerType = MarkerType.Circle, MarkerFill = OxyColors.Red, MarkerSize = 4 };
                foreach (var point in outerPoints)
                {
                    scatterSeriesOuter.Points.Add(new ScatterPoint(point.Item1, point.Item2));
                }

                // Add the scatter series to the plot
                plotModel_1.Series.Add(scatterSeriesInner);
                plotModel_1.Series.Add(scatterSeriesOuter);

                // Refresh the plot
                plotView.Model = plotModel_1;
                plotView.InvalidatePlot(true);
            }
        }


        private void WindowLab6_N3()
        {

        }

        private void WindowLab6_N4()
        {

        }

        private void Task1_Click(object sender, EventArgs e)
        {
            ClearPlotAndControls();  // Очищаем график и элементы управления
            WindowLab6_N1();
        }

        private void Task2_Click(object sender, EventArgs e)
        {
            ClearPlotAndControls();
            WindowLab6_N2();
        }

        private void Task3_Click(object sender, EventArgs e)
        {
            ClearPlotAndControls();
            WindowLab6_N3();
        }

        private void Task4_Click(object sender, EventArgs e)
        {
            ClearPlotAndControls();
            WindowLab6_N4();
        }
    }

}