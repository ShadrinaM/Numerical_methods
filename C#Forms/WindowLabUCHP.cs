using System;
using System.Linq;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace C_Forms
{
    public partial class WindowLabUCHP : Form
    {
        private Menu mainForm;

        public WindowLabUCHP(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += WindowLabUCHP_FormClosed;
            LabUCHP();
        }

        private void WindowLabUCHP_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        private void LabUCHP()
        {
            double L = Math.PI / 2; // Длина области
            numericUpDown1.Value = 1;
            double T = (double)numericUpDown1.Value; // Время моделирования
            int Nx = 10; // Количество узлов по пространству
            int Nt = 2 * Nx * Nx; // Количество временных шагов, вычисляемое для соблюдения условия устойчивости
            CalculateЫolutions(L, T, Nx, Nt);
        }

        void CalculateЫolutions(double L, double T, int Nx, int Nt)
        {
            // Рассчёт явной схемы
            var uExplicit = ExplicitScheme(L, T, Nx, Nt);
            PlotResults(uExplicit, L, Nx, Nt, "Явная схема", plotView1);

            // Рассчёт неявной схемы
            var uImplicit = ImplicitScheme(L, T, Nx, Nt);
            PlotResults(uImplicit, L, Nx, Nt, "Неявная схема", plotView2);

            // Построение аналитического решения
            PlotAnalyticalSolution(L, T, Nx, Nt, plotView3);

            // Вычисление ошибок
            double errorExplicit = CalculateError(uExplicit, L, T, Nx, Nt);
            double errorImplicit = CalculateError(uImplicit, L, T, Nx, Nt);

            // Вывод текстовой информации
            textBox1.Text = "Среднняя ошибка для явной схемы: " + errorExplicit + "\r\n" +
                            "Среднняя ошибка для неявной схемы: " + errorImplicit;
        }

        // Реализация явной разностной схемы
        // Простая реализация, но требует соблюдения условия устойчивости r<=0.5
        private double[,] ExplicitScheme(double L, double T, int Nx, int Nt)
        {
            double dx = L / (Nx - 1); // Разбивается область Nx узлов с шагом dx
            double dt = T / Nt; // Разбивается временная область Nt временных шагов с шагом dt
            double r = dt / (dx * dx); // Параметр устойчивости, определяет устойчивость схемы. Если r > 0.5, схема неустойчива.

            if (r > 0.5)
            {
                // Предупреждение о нарушении условия стабильности
                textBox1.Text += $"\r\nВнимание: условие устойчивости нарушено (r = {r} > 0,5). Уменьшите dt или увеличьте Nx.";
            }

            double[,] u = new double[Nt, Nx]; // Двумерный массив решений

            // Задаем начальные условия
            for (int i = 0; i < Nx; i++)
            {
                u[0, i] = Math.Sin(i * dx);
            }

            // Граничные условия и явная схема
            for (int t = 0; t < Nt - 1; t++)
            {
                for (int x = 1; x < Nx - 1; x++)
                {
                    u[t + 1, x] = r * u[t, x - 1] + (1 - 2 * r) * u[t, x] + r * u[t, x + 1];
                }

                // Граничные условия
                u[t + 1, 0] = 0;
                u[t + 1, Nx - 1] = Math.Exp(-(t + 1) * dt);
            }

            return u;
        }

        // Реализация неявной разностной схемы
        // Устойчива для любых r, но требует решения системы линейных уравнений.
        private double[,] ImplicitScheme(double L, double T, int Nx, int Nt)
        {
            double dx = L / (Nx - 1);
            double dt = T / Nt;
            double r = dt / (dx * dx);

            double[,] u = new double[Nt, Nx];

            // Инициализация начального условия
            for (int i = 0; i < Nx; i++)
            {
                u[0, i] = Math.Sin(i * dx);
            }

            // Коэффициенты матрицы
            double[] a = Enumerable.Repeat(-r, Nx - 2).ToArray();
            double[] b = Enumerable.Repeat(1 + 2 * r, Nx - 2).ToArray();
            double[] c = Enumerable.Repeat(-r, Nx - 2).ToArray();
            double[] d = new double[Nx - 2];

            // Проход по времени
            for (int t = 0; t < Nt - 1; t++)
            {
                // Формирование вектора правой части
                for (int i = 0; i < Nx - 2; i++)
                {
                    d[i] = u[t, i + 1];
                }

                // Граничные условия
                d[0] += r * 0; // Левое граничное условие (u(0) = 0)
                d[Nx - 3] += r * Math.Exp(-(t + 1) * dt); // Правое граничное условие (u(L, t) = e^(-t))

                // Решение системы методом прогонки
                double[] solution = ThomasAlgorithm(a, b, c, d);

                // Заполнение нового временного слоя
                for (int i = 0; i < Nx - 2; i++)
                {
                    u[t + 1, i + 1] = solution[i];
                }

                // Установка граничных значений
                u[t + 1, 0] = 0; // Левое граничное условие
                u[t + 1, Nx - 1] = Math.Exp(-(t + 1) * dt); // Правое граничное условие
            }

            return u;
        }



        // Метод Томаса (прогонки) для решения трехдиагональной системы
        private double[] ThomasAlgorithm(double[] a, double[] b, double[] c, double[] d)
        {
            int n = b.Length;
            double[] cPrime = new double[n];
            double[] dPrime = new double[n];

            // Прямая прогонка
            cPrime[0] = c[0] / b[0];
            dPrime[0] = d[0] / b[0];

            for (int i = 1; i < n; i++)
            {
                double denominator = b[i] - a[i] * cPrime[i - 1];
                cPrime[i] = c[i] / denominator;
                dPrime[i] = (d[i] - a[i] * dPrime[i - 1]) / denominator;
            }

            // Обратная прогонка
            double[] x = new double[n];
            x[n - 1] = dPrime[n - 1];

            for (int i = n - 2; i >= 0; i--)
            {
                x[i] = dPrime[i] - cPrime[i] * x[i + 1];
            }

            return x;
        }

        // Вычисление средней ошибки между численным и аналитическим решениями
        private double CalculateError(double[,] u, double L, double T, int Nx, int Nt)
        {
            double dx = L / (Nx - 1);
            double dt = T / Nt;
            double error = 0.0;

            for (int t = 0; t < Nt; t++)
            {
                for (int x = 0; x < Nx; x++)
                {
                    double analytical = AnalyticalSolution(x * dx, t * dt);
                    error += Math.Abs(u[t, x] - analytical);
                }
            }

            return error / (Nx * Nt);
        }

        // Вычисление аналитического решения задачи
        private double AnalyticalSolution(double x, double t)
        {
            return Math.Exp(-t) * Math.Sin(x);
        }

        // Построение графиков для численных схем
        private void PlotResults(double[,] u, double L, int Nx, int Nt, string title, PlotView plotView)
        {
            var model = new PlotModel { Title = title };
            double dx = L / (Nx - 1);

            for (int t = 0; t < Nt; t += Nt / 5)
            {
                var series = new LineSeries { Title = $"t = {t}" };

                for (int x = 0; x < Nx; x++)
                {
                    series.Points.Add(new DataPoint(x * dx, u[t, x]));
                }

                model.Series.Add(series);
            }

            plotView.Model = model;
        }

        // Построение аналитического решения
        private void PlotAnalyticalSolution(double L, double T, int Nx, int Nt, PlotView plotView)
        {
            var model = new PlotModel { Title = "Аналит. реш." };
            double dx = L / (Nx - 1);
            double dt = T / Nt;

            for (int t = 0; t < Nt; t += Nt / 5)
            {
                var series = new LineSeries { Title = $"t = {t}" };

                for (int x = 0; x < Nx; x++)
                {
                    series.Points.Add(new DataPoint(x * dx, AnalyticalSolution(x * dx, t * dt)));
                }

                model.Series.Add(series);
            }

            plotView.Model = model;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double L = Math.PI / 2; // Длина области
            double T = (double)numericUpDown1.Value; // Время моделирования
            int Nx = 10; // Количество узлов по пространству
            int Nt = 2 * Nx * Nx; // Количество временных шагов, вычисляемое для соблюдения условия устойчивости
            CalculateЫolutions(L, T, Nx, Nt);
        }
    }
}
