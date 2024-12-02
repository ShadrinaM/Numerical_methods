using OxyPlot.Series;
using OxyPlot;
using System.Data;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;
using Label = System.Windows.Forms.Label;

namespace C_Forms
{
    public partial class WindowLab6 : Form
    {
        private Menu mainForm;
        private TextBox NTextBox = new TextBox();
        private Button calculateButton = new Button();
        private Label answerLabel = new Label();
        private Label absErrorLabel = new Label();
        private Label relErrorLabel = new Label();
        private static int n = 14;
        private PlotModel plotModel = new PlotModel { Title = "График функции f(x)" };
        //private static double b = (-6) * (n - 20) / 10 + 20, hx = (30 * n - n * n) / 10, a = 10 * hx / n;
        private static double b = 32, hx = 22.4, a = 16;

        public WindowLab6(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka ?? throw new ArgumentNullException(nameof(menushka)); // Проверяем, что menushka не null
            this.FormClosed += WindowLab6_FormClosed;

            // Инициализируем объекты
            plotModel = new PlotModel { Title = "График функции f(x)" };
            plotView = new PlotView { Model = plotModel, Dock = DockStyle.Top, Height = 300 };

            WindowLab6_N1();
        }


        private void WindowLab6_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        private void WindowLab6_N1()
        {
            this.Size = new Size(1000, 600);

            plotView.Dock = DockStyle.Top;
            plotView.Height = 300;
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X" });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y" });

            var functionSeries = new LineSeries { Title = "f(x)" };
            for (double x = 0.0; x < hx; x += 0.2)
            {
                double y = 10 * x / n;
                functionSeries.Points.Add(new DataPoint(x, y));
            }
            for (double x = hx; x <= b; x += 0.2)
            {
                double y = 10 * (x - 20) / (n - 20) + 20;
                functionSeries.Points.Add(new DataPoint(x, y));
            }
            plotModel.Series.Add(functionSeries);
            plotView.Model = plotModel;

            var inputPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 300
            };

            var NLabel = new Label { Text = "Введите количество точек N:", Location = new Point(10, 10), AutoSize = true };
            NTextBox.Location = new Point(180, 10);

            calculateButton.Text = "Рассчитать";
            calculateButton.Location = new Point(10, 50);
            calculateButton.Size = new Size(150, 30);
            calculateButton.Click += CalculateButton_Click;

            answerLabel.Location = new Point(10, 80);
            answerLabel.AutoSize = true;

            absErrorLabel.Location = new Point(10, 110);
            absErrorLabel.AutoSize = true;

            relErrorLabel.Location = new Point(10, 140);
            relErrorLabel.AutoSize = true;

            inputPanel.Controls.Add(NLabel);
            inputPanel.Controls.Add(NTextBox);
            inputPanel.Controls.Add(calculateButton);
            inputPanel.Controls.Add(answerLabel);
            inputPanel.Controls.Add(absErrorLabel);
            inputPanel.Controls.Add(relErrorLabel);

            this.Controls.Add(inputPanel);
            this.Controls.Add(plotView);
        }

        /// <summary>
        /// Выводит результаты
        /// </summary>
        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(NTextBox.Text, out int N) || N <= 0)
            {
                MessageBox.Show("Введите корректное значение N.");
                return;
            }

            var (area, randomPoints, innerPoints) = AreaCalculation(N);
            answerLabel.Text = $"Площадь треугольника: {Math.Round(area, 4)}";

            double absError = Math.Abs(256.0 - area);
            absErrorLabel.Text = $"Абсолютная погрешность: {Math.Round(absError, 2)}";

            double relError = absError / 100.0;
            relErrorLabel.Text = $"Относительная погрешность: {Math.Round(relError, 4):P2}";

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

            for (int i = plotModel.Series.Count - 1; i >= 0; i--)
            {
                if (plotModel.Series[i] is ScatterSeries)
                {
                    plotModel.Series.RemoveAt(i);
                }
            }
            plotModel.Series.Add(scatterSeriesInner);
            plotModel.Series.Add(scatterSeriesOuter);

            plotView.InvalidatePlot(true);
        }

        /// <summary>
        /// Заданная функция
        /// </summary>
        private double F(double x)
        {
            if (x < hx)
                return 10.0 * x / n;
            else
                return 10.0 * (x - 20) / (n - 20)+20;
        }

        /// <summary>
        /// Вычисление площади треугольника
        /// </summary>
        /// <param name="N">Количество точек</param>
        /// <returns> Кортеж из площади, списка рандомных точек и списка внутренних точек</returns>
        private (double, List<(double, double)>, List<(double, double)>) AreaCalculation(int N)
        {      
            var randomPoints = new List<(double, double)>();
            var rand = new Random();

            for (int i = 0; i < N; i++)
            {
                double x = rand.NextDouble() * b;
                double y = rand.NextDouble() * a;
                randomPoints.Add((x, y));
            }

            var innerPoints = randomPoints.Where(point => point.Item2 < F(point.Item1)).ToList();
            double area = (innerPoints.Count / (double)N) * a * b;

            return (area, randomPoints, innerPoints);
        }
    }

}
