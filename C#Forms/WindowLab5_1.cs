using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot.WindowsForms;
using Label = System.Windows.Forms.Label;

namespace C_Forms
{
    public partial class WindowLab5_1 : Form
    {
        private Menu mainForm;
        public WindowLab5_1(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += WindowLab5_1_FormClosed;
            Lab51();
        }
        private void WindowLab5_1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }
        void Lab51()
        {
            numericUpDown1.Value = 10;

            // Значения x_i и y_i
            double[] xValues = { 2, 3, 5, 7 };
            double[] yValues = { 4, -2, 6, -3 };

            // Добавляем заголовки
            tableLayoutPanel1.Controls.Add(new Label { Text = "x_i", TextAlign = ContentAlignment.MiddleCenter }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label { Text = "y_i", TextAlign = ContentAlignment.MiddleCenter }, 0, 1);

            // Добавляем значения
            for (int i = 0; i < xValues.Length; i++)
            {
                tableLayoutPanel1.Controls.Add(new Label { Text = xValues[i].ToString(), TextAlign = ContentAlignment.MiddleCenter }, i + 1, 0);
                tableLayoutPanel1.Controls.Add(new Label { Text = yValues[i].ToString(), TextAlign = ContentAlignment.MiddleCenter }, i + 1, 1);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка числа узлов
            if (numericUpDown1.Value < 3)
            {
                MessageBox.Show("Недостаточно точек для построения кубического сплайна. Укажите хотя бы 3 узла.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PolynomialInterpolation();
        }
        void PolynomialInterpolation()
        {
            // Создаем модель графика
            var plotModel = new PlotModel { Title = "Интерполяция Лагранжа" };
            plotView1.Model = plotModel;

            // Добавляем график оригинальной функции
            var functionSeries = new LineSeries
            {
                MarkerType = MarkerType.None,
                Color = OxyColors.Blue
            };
            for (double x = -1; x <= 1; x += 0.01)
            {
                functionSeries.Points.Add(new DataPoint(x, FunctionNumber1(x)));
            }
            plotModel.Series.Add(functionSeries);

            // Добавляем график интерполяционного полинома Лагранжа
            int n = (int)numericUpDown1.Value - 1; // Количество отрезков
            double[] nodes = new double[n + 1];
            double[] values = new double[n + 1];

            // Равноотстоящие узлы
            nodes = EquidistantNodes(n + 1, -1, 1);
            for (int i = 0; i <= n; i++)
            {
                values[i] = FunctionNumber1(nodes[i]);
            }
            var lagrangeSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                Color = OxyColors.Red
            };
            for (double x = -1; x <= 1; x += 0.01)
            {
                double y = LagrangeInterpolation(nodes, values, x);
                lagrangeSeries.Points.Add(new DataPoint(x, y));
            }
            plotModel.Series.Add(lagrangeSeries);

            // Чебышевские узлы
            nodes = ChebyshevNodes(n + 1, -1, 1);
            for (int i = 0; i <= n; i++)
            {
                values[i] = FunctionNumber1(nodes[i]);
            }
            var chebyshevSeries = new LineSeries
            {
                MarkerType = MarkerType.Diamond,
                Color = OxyColors.Green
            };
            for (double x = -1; x <= 1; x += 0.01)
            {
                double y = LagrangeInterpolation(nodes, values, x);
                chebyshevSeries.Points.Add(new DataPoint(x, y));
            }
            plotModel.Series.Add(chebyshevSeries);

            // Обновляем график
            plotView1.InvalidatePlot(true);
        }
        // Генерация равноотстоящих узлов
        public static double[] EquidistantNodes(int n, double a, double b)
        {
            double[] nodes = new double[n];
            for (int i = 0; i < n; i++)
            {
                nodes[i] = a + (b - a) * i / (n - 1);
            }
            return nodes;
        }
        // Генерация узлов Чебышева
        public static double[] ChebyshevNodes(int n, double a, double b)
        {
            double[] nodes = new double[n];
            for (int i = 0; i < n; i++)
            {
                nodes[i] = 0.5 * ((b - a) * Math.Cos(Math.PI * (2 * i + 1) / (2 * n)) + (a + b));
            }
            return nodes;
        }
        //Функция задания 1
        double FunctionNumber1(double x)
        {
            return 1 / (1 + 25 * x * x);
        }
        // Полиномиальная интерполяция
        double LagrangeInterpolation(double[] xNodes, double[] yNodes, double x)
        {
            int n = xNodes.Length - 1;
            double result = 0;
            for (int i = 0; i <= n; i++)
            {
                double term = yNodes[i];
                for (int j = 0; j <= n; j++)
                {
                    if (j != i)
                    {
                        term = term * (x - xNodes[j]) / (xNodes[i] - xNodes[j]);
                    }
                }
                result += term;
            }
            return result;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Проверка числа узлов
            if (numericUpDown1.Value < 3)
            {
                MessageBox.Show("Недостаточно точек для построения кубического сплайна. Укажите хотя бы 3 узла.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            InterpolationCubicSplines();
        }
        void InterpolationCubicSplines()
        {
            // Создаем модель графика
            var plotModel = new PlotModel { Title = "Кубическая сплайн-интерполяция" };
            plotView1.Model = plotModel;

            // Добавляем график оригинальной функции
            var functionSeries = new LineSeries
            {
                MarkerType = MarkerType.None,
                Color = OxyColors.Blue
            };
            for (double x = -1; x <= 1; x += 0.01)
            {
                functionSeries.Points.Add(new DataPoint(x, FunctionNumber1(x)));
            }
            plotModel.Series.Add(functionSeries);

            int n = (int)numericUpDown1.Value; // Количество узлов
            double[] nodes = EquidistantNodes(n, -1, 1);
            double[] values = new double[n];
            for (int i = 0; i < n; i++)
            {
                values[i] = FunctionNumber1(nodes[i]);
            }

            // Генерация точек для построения графика сплайна
            double[] xPoints = new double[1000];
            for (int i = 0; i < 1000; i++)
            {
                xPoints[i] = -1 + 2 * i / 999.0;
            }
            double[] ySpline = CubicSplineInterpolation(xPoints, nodes, values);

            var splineSeries = new LineSeries
            {
                MarkerType = MarkerType.None,
                Color = OxyColors.Red
            };
            for (int i = 0; i < xPoints.Length; i++)
            {
                splineSeries.Points.Add(new DataPoint(xPoints[i], ySpline[i]));
            }
            plotModel.Series.Add(splineSeries);

            // Обновляем график
            plotView1.InvalidatePlot(true);
        }
        // Построение кубического сплайна для заданной таблицы значений
        double[] CubicSplineInterpolation(double[] x, double[] xVals, double[] yVals)
        {
            int n = xVals.Length;
            if (n < 3)
            {
                throw new ArgumentException("Need at least three points for cubic spline interpolation");
            }

            double[] h = new double[n - 1];
            double[] alpha = new double[n - 1];
            for (int i = 0; i < n - 1; i++)
            {
                h[i] = xVals[i + 1] - xVals[i];
                if (i > 0)
                {
                    alpha[i] = (3 / h[i]) * (yVals[i + 1] - yVals[i]) - (3 / h[i - 1]) * (yVals[i] - yVals[i - 1]);
                }
            }

            double[] l = new double[n];
            double[] mu = new double[n];
            double[] z = new double[n];
            l[0] = 1;
            mu[0] = 0;
            z[0] = 0;

            for (int i = 1; i < n - 1; i++)
            {
                l[i] = 2 * (xVals[i + 1] - xVals[i - 1]) - h[i - 1] * mu[i - 1];
                mu[i] = h[i] / l[i];
                z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
            }

            l[n - 1] = 1;
            z[n - 1] = 0;
            double[] c = new double[n];
            double[] b = new double[n - 1];
            double[] d = new double[n - 1];
            double[] a = new double[n - 1];

            for (int j = n - 2; j >= 0; j--)
            {
                c[j] = z[j] - mu[j] * c[j + 1];
                b[j] = (yVals[j + 1] - yVals[j]) / h[j] - h[j] * (c[j + 1] + 2 * c[j]) / 3;
                d[j] = (c[j + 1] - c[j]) / (3 * h[j]);
                a[j] = yVals[j];
            }

            // Генерация точек для построения графика сплайна
            double[] result = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                // Найти нужный сегмент
                int segment = 0;
                for (int j = 0; j < n - 1; j++)
                {
                    if (x[i] >= xVals[j] && x[i] <= xVals[j + 1])
                    {
                        segment = j;
                        break;
                    }
                }
                double dx = x[i] - xVals[segment];
                result[i] = a[segment] + b[segment] * dx + c[segment] * dx * dx + d[segment] * dx * dx * dx;
            }

            return result;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            InterpolationCubicSplinesByTable();
        }
        void InterpolationCubicSplinesByTable()
        {
            // Считывание значений x_i и y_i из tableLayoutPanel1
            int numPoints = tableLayoutPanel1.ColumnCount - 1; // Число узлов
            double[] xVals = new double[numPoints];
            double[] yVals = new double[numPoints];

            for (int i = 0; i < numPoints; i++)
            {
                xVals[i] = double.Parse(tableLayoutPanel1.GetControlFromPosition(i + 1, 0).Text);
                yVals[i] = double.Parse(tableLayoutPanel1.GetControlFromPosition(i + 1, 1).Text);
            }

            // Проверяем: достаточно ли точек для интерполяции
            if (numPoints < 3)
            {
                MessageBox.Show("Недостаточно точек для кубической сплайн-интерполяции.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Генерация точек для построения графика сплайна
            double[] xPoints = new double[1000];
            for (int i = 0; i < 1000; i++)
            {
                xPoints[i] = xVals[0] + (xVals[numPoints - 1] - xVals[0]) * i / 999.0;
            }
            double[] ySpline = CubicSplineInterpolation(xPoints, xVals, yVals);

            // Проверка интерполирования в узловых точках
            for (int i = 0; i < numPoints; i++)
            {
                double splineValue = CubicSplineInterpolation(new double[] { xVals[i] }, xVals, yVals)[0];
                Console.WriteLine($"Узел x = {xVals[i]}, y = {yVals[i]}, интерполированное значение y = {splineValue}");
            }

            // Построение графика
            var plotModel = new PlotModel { Title = "Кубическая сплайн-интерполяция по таблице" };
            plotView1.Model = plotModel;

            // Добавляем график сплайна
            var splineSeries = new LineSeries
            {
                MarkerType = MarkerType.None,
                Color = OxyColors.Red
            };
            for (int i = 0; i < xPoints.Length; i++)
            {
                splineSeries.Points.Add(new DataPoint(xPoints[i], ySpline[i]));
            }
            plotModel.Series.Add(splineSeries);

            // Добавляем узловые точки
            var nodeSeries = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Blue,
                MarkerSize = 4
            };
            for (int i = 0; i < numPoints; i++)
            {
                nodeSeries.Points.Add(new ScatterPoint(xVals[i], yVals[i]));
            }
            plotModel.Series.Add(nodeSeries);

            // Обновляем график
            plotView1.InvalidatePlot(true);
        }

    }
}
