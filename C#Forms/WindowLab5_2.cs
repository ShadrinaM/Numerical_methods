using MathNet.Numerics.LinearAlgebra;
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

namespace C_Forms
{
    public partial class WindowLab5_2 : Form
    {
        private Menu mainForm;
        double[] x = { 5, 15, 25, 35, 45, 55 };
        double[] y = { 2.2, 2.4, 2.6, 2.7, 2.8, 2.9 };
        public WindowLab5_2(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += WindowLab4_FormClosed;
            Lab52();
        }
        private void BackToMenu_Click(object sender, EventArgs e)
        {
            // Показываем главное окно
            mainForm.Show();
            // Закрываем текущее окно
            this.Close();
        }
        private void WindowLab4_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Показываем главное окно при закрытии текущего
            mainForm.Show();
        }

        void Lab52()
        {
            // Создаем объект для построения графиков
            var plotModel = new PlotModel { Title = "Аппроксимация методом наименьших квадратов" };

            // Нанесем экспериментальные точки
            var pointsSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = 4, MarkerFill = OxyColors.Red };
            for (int i = 0; i < x.Length; i++)
                pointsSeries.Points.Add(new ScatterPoint(x[i], y[i]));
            plotModel.Series.Add(pointsSeries);

            // Линейная аппроксимация
            var (aLin, bLin) = LinearApproximation();
            var linearSeries = CreateLineSeries(aLin, bLin, "Линейная: y = " + Math.Round(aLin, 2) + " + " + Math.Round(bLin, 2) + "x");
            plotModel.Series.Add(linearSeries);

            // Степенная аппроксимация
            var (aPow, bPow) = PowerApproximation();
            var powerSeries = CreatePowerSeries(aPow, bPow, "Степенная: y = " + Math.Round(aPow, 2) + "x^" + Math.Round(bPow, 2));
            plotModel.Series.Add(powerSeries);

            // Показательная аппроксимация
            var (aExp, bExp) = ExponentialApproximation();
            var expSeries = CreateExpSeries(aExp, bExp, "Показательная: y = " + Math.Round(aExp, 2) + "e^(" + Math.Round(bExp, 2) + "x)");
            plotModel.Series.Add(expSeries);

            // Квадратичная аппроксимация
            var (aQuad, bQuad, cQuad) = QuadraticApproximation();
            var quadSeries = CreateQuadraticSeries(aQuad, bQuad, cQuad, "Квадратичная: y = " + Math.Round(aQuad, 2) + " + " + Math.Round(bQuad, 2) + "x + " + Math.Round(cQuad, 2) + "x^2");
            plotModel.Series.Add(quadSeries);

            // Устанавливаем модель графика в plotView1
            plotView1.Model = plotModel;
            // Заполнение TableLayoutPanel значениями
            FillTableLayoutPanel();
        }

        //Заполнене таблицы
        private void FillTableLayoutPanel()
        {
            tableLayoutPanel1.Controls.Clear(); // Очищаем старые значения, если есть

            // Установите количество строк и столбцов
            tableLayoutPanel1.ColumnCount = x.Length + 1; // +1 для столбца заголовков
            tableLayoutPanel1.RowCount = 2; // 2 строки: одна для x, одна для y

            // Установите одинаковую ширину для всех столбцов
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / tableLayoutPanel1.ColumnCount));
            }

            // Добавляем заголовки в первый столбец
            tableLayoutPanel1.Controls.Add(new System.Windows.Forms.Label { Text = "x", Anchor = AnchorStyles.None }, 0, 0);
            tableLayoutPanel1.Controls.Add(new System.Windows.Forms.Label { Text = "y", Anchor = AnchorStyles.None }, 0, 1);

            // Заполняем первую строку значениями x (сдвинуты на один столбец вправо)
            for (int i = 0; i < x.Length; i++)
            {
                tableLayoutPanel1.Controls.Add(new System.Windows.Forms.Label { Text = x[i].ToString("F2"), Anchor = AnchorStyles.None }, i + 1, 0);
            }

            // Заполняем вторую строку значениями y (сдвинуты на один столбец вправо)
            for (int i = 0; i < y.Length; i++)
            {
                tableLayoutPanel1.Controls.Add(new System.Windows.Forms.Label { Text = y[i].ToString("F2"), Anchor = AnchorStyles.None }, i + 1, 1);
            }
        }





        // Линейная аппроксимация для массивов x и y
        private (double a, double b) LinearApproximation(double[] x, double[] y)
        {
            double n = x.Length;
            double sumX = x.Sum();
            double sumY = y.Sum();
            double sumXY = x.Zip(y, (xi, yi) => xi * yi).Sum();
            double sumX2 = x.Sum(xi => xi * xi);

            double b = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double a = (sumY - b * sumX) / n;

            return (a, b);
        }


        // Линейная аппроксимация
        private (double a, double b) LinearApproximation()
        {
            double n = x.Length;
            double sumX = x.Sum();
            double sumY = y.Sum();
            double sumXY = x.Zip(y, (xi, yi) => xi * yi).Sum();
            double sumX2 = x.Sum(xi => xi * xi);

            double b = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double a = (sumY - b * sumX) / n;

            return (a, b);
        }

        // Степенная аппроксимация (логарифмируем данные)
        private (double a, double b) PowerApproximation()
        {
            var logX = x.Select(xi => Math.Log(xi)).ToArray();
            var logY = y.Select(yi => Math.Log(yi)).ToArray();

            var (aLog, bLog) = LinearApproximation(logX, logY);
            double a = Math.Exp(aLog);
            double b = bLog;
            return (a, b);
        }

        // Показательная аппроксимация (логарифмируем y)
        private (double a, double b) ExponentialApproximation()
        {
            var logY = y.Select(yi => Math.Log(yi)).ToArray();
            var (aLog, b) = LinearApproximation(x, logY);
            double a = Math.Exp(aLog);
            return (a, b);
        }

        // Квадратичная аппроксимация
        private (double a, double b, double c) QuadraticApproximation()
        {
            double n = x.Length;
            double sumX = x.Sum();
            double sumX2 = x.Sum(xi => xi * xi);
            double sumX3 = x.Sum(xi => xi * xi * xi);
            double sumX4 = x.Sum(xi => xi * xi * xi * xi);
            double sumY = y.Sum();
            double sumXY = x.Zip(y, (xi, yi) => xi * yi).Sum();
            double sumX2Y = x.Zip(y, (xi, yi) => xi * xi * yi).Sum();

            // Решение системы линейных уравнений для нахождения a, b, c
            var matrix = new double[,] {
                { n, sumX, sumX2 },
                { sumX, sumX2, sumX3 },
                { sumX2, sumX3, sumX4 }
            };
            var vector = new double[] { sumY, sumXY, sumX2Y };

            var result = SolveLinearSystem(matrix, vector);
            return (result[0], result[1], result[2]);
        }

        // Решение системы линейных уравнений методом Гаусса
        private double[] SolveLinearSystem(double[,] matrix, double[] vector)
        {
            int n = vector.Length;
            for (int i = 0; i < n; i++)
            {
                // Нормализация текущей строки
                double divisor = matrix[i, i];
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] /= divisor;
                }
                vector[i] /= divisor;

                // Вычитание текущей строки из остальных строк
                for (int k = 0; k < n; k++)
                {
                    if (k == i) continue;
                    double factor = matrix[k, i];
                    for (int j = 0; j < n; j++)
                    {
                        matrix[k, j] -= factor * matrix[i, j];
                    }
                    vector[k] -= factor * vector[i];
                }
            }
            return vector;
        }

        // Вспомогательные методы для создания серий
        private LineSeries CreateLineSeries(double a, double b, string title)
        {
            var lineSeries = new LineSeries { Title = title };
            for (double xi = x.Min(); xi <= x.Max(); xi += 0.1)
                lineSeries.Points.Add(new DataPoint(xi, a + b * xi));
            return lineSeries;
        }

        private LineSeries CreatePowerSeries(double a, double b, string title)
        {
            var lineSeries = new LineSeries { Title = title };
            for (double xi = x.Min(); xi <= x.Max(); xi += 0.1)
                lineSeries.Points.Add(new DataPoint(xi, a * Math.Pow(xi, b)));
            return lineSeries;
        }

        private LineSeries CreateExpSeries(double a, double b, string title)
        {
            var lineSeries = new LineSeries { Title = title };
            for (double xi = x.Min(); xi <= x.Max(); xi += 0.1)
                lineSeries.Points.Add(new DataPoint(xi, a * Math.Exp(b * xi)));
            return lineSeries;
        }

        private LineSeries CreateQuadraticSeries(double a, double b, double c, string title)
        {
            var lineSeries = new LineSeries { Title = title };
            for (double xi = x.Min(); xi <= x.Max(); xi += 0.1)
                lineSeries.Points.Add(new DataPoint(xi, a + b * xi + c * xi * xi));
            return lineSeries;
        }
    }
}
