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
        private TextBox NTextBox_1 = new TextBox();
        private Button calculateButton_1 = new Button();
        private Label answerLabel_1 = new Label();
        private Label absErrorLabel_1 = new Label();
        private Label relErrorLabel_1 = new Label();
        private static int n_1 = 14;
        private PlotModel plotModel_1 = new PlotModel { Title = "График" };
        //private static double b = (-6) * (n - 20) / 10 + 20, hx = (30 * n - n * n) / 10, a = 10 * hx / n;
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

            WindowLab6_N1();
        }


        private void WindowLab6_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        private void WindowLab6_N1()
        {
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
            calculateButton_1.Click += CalculateButton_Click;

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
        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(NTextBox_1.Text, out int N) || N <= 0)
            {
                MessageBox.Show("Введите корректное значение N.");
                return;
            }

            var (area, randomPoints, innerPoints) = AreaCalculation(N);
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
        private double F(double x)
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
        private (double, List<(double, double)>, List<(double, double)>) AreaCalculation(int N)
        {
            var randomPoints = new List<(double, double)>();
            var rand = new Random();

            for (int i = 0; i < N; i++)
            {
                double x = rand.NextDouble() * b_1;
                double y = rand.NextDouble() * a_1;
                randomPoints.Add((x, y));
            }

            var innerPoints = randomPoints.Where(point => point.Item2 < F(point.Item1)).ToList();
            double area = (innerPoints.Count / (double)N) * a_1 * b_1;

            return (area, randomPoints, innerPoints);
        }
    }

}