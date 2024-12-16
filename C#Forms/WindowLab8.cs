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
using MathNet.Numerics;
using System.Numerics;
using Label = System.Windows.Forms.Label;

namespace C_Forms
{
    public partial class WindowLab8 : Form
    {
        private Menu mainForm;
        double[] alphaValues = { 1, 10, 112, 112345, 1e9, 1e12, 1e15, 1e16 };

        public WindowLab8(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += WindowLab8_FormClosed;
            Lab8();
        }
        private void WindowLab8_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }
        void Lab8()
        {
            Lab8_1();
            Lab8_2_N1();
            Lab8_2_N2();
        }

        void Lab8_1()
        {
            // Параметры для метода Ньютона
            double tolerance = 1e-9; //точность
            int maxIterations = 100; // максимальное число операций

            // Определение функции для вычисления численного интеграла методом трапеций (в лекции формула (***))
            double Integrate(Func<double, double> f, double a, double b, int n)
            {
                double h = (b - a) / n;
                double sum = 0.5 * (f(a) + f(b));

                for (int i = 1; i < n; i++)
                {
                    sum += f(a + i * h);
                }

                return sum * h;
            }

            // Функция для вычисления erf(x)
            double Erf(double x)
            {
                double sqrtPi = Math.Sqrt(Math.PI);
                return 2.0 / sqrtPi * Integrate(t => Math.Exp(-t * t), 0, x, 1000);
            }

            // Определяет уравнение и его производную
            double ErfEquation(double x)
            {
                return Erf(x) - 0.5;
            }
            double ErfDerivative(double x)
            {
                return (2 / Math.Sqrt(Math.PI)) * Math.Exp(-x * x);
            }

            // Метод Ньютона
            (double root, int iterations, string error) NewtonErf(double tolerance, int maxIterations)
            {
                // Начальное приближение
                double x = 1.2; // Точка старта, близкая к решению

                for (int i = 0; i < maxIterations; i++)
                {
                    double fx = ErfEquation(x);
                    double fpx = ErfDerivative(x);

                    // Проверка на слишком маленькую производную, чтобы избежать деления на ноль
                    if (Math.Abs(fpx) < 1e-15)
                    {
                        return (x, i, $"Ошибка: Производная слишком мала на итерации {i}");
                    }

                    // Следующее приближение для корня
                    double xNext = x - fx / fpx;

                    // Проверка на достижение желаемой точности
                    if (Math.Abs(xNext - x) < tolerance)
                    {
                        return (xNext, i + 1, null);
                    }

                    x = xNext;
                }

                // Если метод не сошелся
                return (x, maxIterations, "Ошибка: Метод не сошелся за максимальное количество итераций");
            }

            var result = NewtonErf(tolerance, maxIterations);

            if (result.error != null)
            {
                label2.Text = result.error;
            }
            else
            {
                label2.Text = $"Корень: {result.root:F13} найден за {result.iterations} итераций";
            }
        }

        void Lab8_2_N1()
        {
            double SolveCardanoOneRoot(double a, double b, double c)
            {
                double p = b - (a * a / 3);
                double q = c - (a * b) / 3 + 2 * Math.Pow(a / 3, 3);

                double pDiv3 = p / 3;
                double qDiv2 = q / 2;
                double innerRoot = Math.Sqrt(Math.Pow(pDiv3, 3) + Math.Pow(qDiv2, 2));
                double s = Math.Cbrt(-qDiv2 + innerRoot);
                double t = Math.Cbrt(-qDiv2 - innerRoot);

                double y1 = s + t;
                double x1 = y1 - a / 3;
                return x1;
            }

            (Complex, Complex) SolveComplexQuadratic(double a, double b, double c)
            {
                double discriminant = b * b - 4 * a * c;
                if (discriminant >= 0)
                {
                    double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                    double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                    return (new Complex(root1, 0), new Complex(root2, 0));
                }
                else
                {
                    double realPart = -b / (2 * a);
                    double imaginaryPart = Math.Sqrt(-discriminant) / (2 * a);
                    return (new Complex(realPart, imaginaryPart), new Complex(realPart, -imaginaryPart));
                }
            }

            //label4.Text = "\t=== Таблица корней и погрешностей для метода Кардано ===\n";
            //label4.Text += "Alpha\t\t\tx1 \tError (x1)\t\tКомплексные x2 и x3\t\t\tОшибка корнях x2 и x3\n";

            //foreach (var alpha in alphaValues)
            //{
            //    double b = alpha * alpha;
            //    double c = 3 * alpha * alpha;

            //    double a = 3.0;
            //    double x1 = SolveCardanoOneRoot(a, b, c); //поиск одного вещ корня
            //    double temp = Math.Pow(10, alpha);
            //    double x1Theoretical = x1 - 1 / temp;
            //    double x1Error = Math.Abs(x1 - x1Theoretical);

            //    //переход к квадратному уравнению с новыми коэффициентами
            //    double newB = 3 + x1;
            //    double newC = b + x1 * x1 + 3 * x1;

            //    var (x2, x3) = SolveComplexQuadratic(1, newB, newC); //поиск двух комплексных корней

            //    Complex x2Theoretical = new Complex(0, -alpha);
            //    Complex x3Theoretical = new Complex(0.0, -alpha);
            //    double x2Error = Complex.Abs(x2 - x2Theoretical);
            //    double x3Error = Complex.Abs(x3 - x3Theoretical);

            //    label4.Text += $"Alpha: {alpha:e}\t  {x1:F5}\t{x1Error:e}\t({x2.Real:F2}, {x2.Imaginary:F2}i) & ({x3.Real:F2}, {x3.Imaginary:F2}i)\t{x2Error:F2} & {x3Error:F2}\n";
            //}

            // Создаем TableLayoutPanel
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.ColumnCount = 6;
            tableLayoutPanel.RowCount = alphaValues.Length + 1;
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            Point point = label8.Location;
            tableLayoutPanel.Location = point;

            // Добавляем заголовки
            tableLayoutPanel.Controls.Add(new Label { Text = "Alpha", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 0, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "x1", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 1, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "Error (x1)", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 2, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "x2 (Complex)", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 3, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "x3 (Complex)", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 4, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "Errors x2 & x3", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 5, 0);

            int row = 1;

            foreach (var alpha in alphaValues)
            {
                double b = alpha * alpha;
                double c = 3 * alpha * alpha;

                double a = 3.0;
                double x1 = SolveCardanoOneRoot(a, b, c); // Поиск одного вещ корня
                double temp = Math.Pow(10, alpha);
                double x1Theoretical = x1 - 1 / temp;
                double x1Error = Math.Abs(x1 - x1Theoretical);

                // Переход к квадратному уравнению с новыми коэффициентами
                double newB = 3 + x1;
                double newC = b + x1 * x1 + 3 * x1;

                var (x2, x3) = SolveComplexQuadratic(1, newB, newC); // Поиск двух комплексных корней

                Complex x2Theoretical = new Complex(0, -alpha);
                Complex x3Theoretical = new Complex(0.0, -alpha);
                double x2Error = Complex.Abs(x2 - x2Theoretical);
                double x3Error = Complex.Abs(x3 - x3Theoretical);

                // Добавляем результаты в TableLayoutPanel
                tableLayoutPanel.Controls.Add(new Label { Text = string.Format("{0:e}", alpha), AutoSize = true }, 0, row);
                tableLayoutPanel.Controls.Add(new Label { Text = string.Format("{0:F5}", x1), AutoSize = true }, 1, row);
                tableLayoutPanel.Controls.Add(new Label { Text = string.Format("{0:e}", x1Error), AutoSize = true }, 2, row);
                tableLayoutPanel.Controls.Add(new Label { Text = string.Format("({0}, {1}i)", x2.Real, x2.Imaginary), AutoSize = true }, 3, row);
                tableLayoutPanel.Controls.Add(new Label { Text = string.Format("({0}, {1}i)", x3.Real, x3.Imaginary), AutoSize = true }, 4, row);
                tableLayoutPanel.Controls.Add(new Label { Text = string.Format("{0:F2} & {1:F2}", x2Error, x3Error), AutoSize = true }, 5, row);

                row++;
            }

            // Добавляем TableLayoutPanel в форму
            this.Controls.Add(tableLayoutPanel);

        }

        void Lab8_2_N2()
        {
            double CubicEquation(double x, double alpha)
            {
                return Math.Pow(x, 3) + 3 * Math.Pow(x, 2) + alpha * alpha * x + 3 * Math.Pow(alpha, 2);
            }

            double CubicDerivative(double x, double alpha)
            {
                return 3 * Math.Pow(x, 2) + 6 * x + Math.Pow(alpha, 2);
            }

            double CubicSecondDerivative(double x)
            {
                return 6 * x + 6;
            }

            double NewtonError(double x0, double alpha, double a, double b)
            {
                double m = double.PositiveInfinity;
                for (double x = a; x <= b; x += 0.01)
                {
                    double absDerivative = Math.Abs(CubicDerivative(x, alpha));
                    if (absDerivative < m)
                    {
                        m = absDerivative;
                    }
                }

                double fx0 = Math.Abs(CubicEquation(x0, alpha));
                return fx0 / m;
            }

            (double root, int iterations, string error) NewtonMethod(double alpha, double a, double b, double tolerance, int maxIterations)
            {
                double x0;
                if (CubicEquation(a, alpha) * CubicSecondDerivative(a) > 0)
                {
                    x0 = a;
                }
                else if (CubicEquation(b, alpha) * CubicSecondDerivative(b) > 0)
                {
                    x0 = b;
                }
                else
                {
                    return (0, 0, $"Подходящего начального приближения не найдено в [{a}, {b}]");
                }

                double x = x0;
                for (int i = 0; i < maxIterations; i++)
                {
                    double fx = CubicEquation(x, alpha);
                    double fpx = CubicDerivative(x, alpha);

                    if (Math.Abs(fpx) < 1e-15)
                    {
                        return (x, i, $"Производная слишком мала на итерации {i}");
                    }

                    double xNext = x - fx / fpx;

                    if (Math.Abs(xNext - x) < tolerance)
                    {
                        return (xNext, i + 1, null);
                    }

                    x = xNext;
                }

                return (x, maxIterations, "Метод не сходился за максимальное количество итераций");
            }

            double[] alphaValues = { 1, 10, 112, 112345, 1e9, 1e12, 1e15, 1e16 };
            double a = -10.0;
            double b = 10.0;
            double tolerance = 1e-9;
            int maxIterations = 100;

            // Создаем TableLayoutPanel
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.RowCount = alphaValues.Length + 1;
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            Point point = label9.Location;
            tableLayoutPanel.Location = point;

            // Добавляем заголовки
            tableLayoutPanel.Controls.Add(new Label { Text = "Alpha", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 0, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "Root (Num)", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 1, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "Iterations", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 2, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "Error Estimate", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) }, 3, 0);

            int row = 1;

            foreach (double alpha in alphaValues)
            {
                var (root, iterations, error) = NewtonMethod(alpha, a, b, tolerance, maxIterations);

                if (error != null)
                {
                    tableLayoutPanel.Controls.Add(new Label { Text = string.Format("{0:E2}", alpha), AutoSize = true }, 0, row);
                    tableLayoutPanel.Controls.Add(new Label { Text = "Error: " + error, AutoSize = true, ForeColor = Color.Red }, 1, row);
                    tableLayoutPanel.Controls.Add(new Label { Text = "-", AutoSize = true }, 2, row);
                    tableLayoutPanel.Controls.Add(new Label { Text = "-", AutoSize = true }, 3, row);
                }
                else
                {
                    double errorEstimate = NewtonError(root, alpha, a, b);
                    tableLayoutPanel.Controls.Add(new Label { Text = string.Format("{0:e}", alpha), AutoSize = true }, 0, row);
                    tableLayoutPanel.Controls.Add(new Label { Text = string.Format("{0:F5}", root), AutoSize = true }, 1, row);
                    tableLayoutPanel.Controls.Add(new Label { Text = iterations.ToString(), AutoSize = true }, 2, row);
                    tableLayoutPanel.Controls.Add(new Label { Text = string.Format("{0:E12}", errorEstimate), AutoSize = true }, 3, row);
                }

                row++;
            }

            // Добавляем TableLayoutPanel в форму
            this.Controls.Add(tableLayoutPanel);
        }
    }
}
