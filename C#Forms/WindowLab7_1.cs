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
using Label = System.Windows.Forms.Label;

namespace C_Forms
{
    public partial class WindowLab7_1 : Form
    {
        private Menu mainForm;

        public WindowLab7_1(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += WindowLab7_1_FormClosed;
            Lab71();
        }
        private void WindowLab7_1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }
        void Lab71()
        {
            numericUpDown1.Value = 32;
            // Заполняем первую строку значениями
            tableLayoutPanel1.Controls.Add(new Label { Text = "x", TextAlign = ContentAlignment.MiddleCenter }, 0, 0);
            for (int col = 0; col <= 20; col++)
            {
                double value = col * 0.1;
                Label label = new Label
                {
                    Text = value.ToString("0.0"), // Форматируем значение
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                tableLayoutPanel1.Controls.Add(label, col + 1, 0); // Добавляем в первую строку
            }
            // Заполнение заголовков второй и третей строки
            tableLayoutPanel1.Controls.Add(new Label { Text = "erf(x)", TextAlign = ContentAlignment.MiddleCenter }, 0, 1);
            tableLayoutPanel1.Controls.Add(new Label { Text = "ERF(x)", TextAlign = ContentAlignment.MiddleCenter }, 0, 2);
            Lab7_1_N1();
            Lab7_1_N3();
        }
        void Lab7_1_N1()
        {
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

            // Заполнение второй строки значениями erf(x)
            for (int col = 0; col <= 20; col++)
            {
                double x = col * 0.1;
                double erfValue = Erf(x);
                Label label = new Label
                {
                    Text = erfValue.ToString("0.00000"), // Форматируем значение
                    TextAlign = ContentAlignment.MiddleCenter
                };
                tableLayoutPanel1.Controls.Add(label, col + 1, 1); // Добавляем во вторую строку
            }

            // Табличные значения ERF(x) для x = 0.0, 0.1, ..., 2.0
            double[] erfTable = {
                0.00000, 0.11246, 0.22270, 0.32863, 0.42839,
                0.52050, 0.60386, 0.67780, 0.74210, 0.79691,
                0.84270, 0.88021, 0.91031, 0.93401, 0.95229,
                0.96611, 0.97635, 0.98379, 0.98909, 0.99279, 0.99532 };

            // Заполнение третьей строки таблицы значениями ERF(x) из таблицы
            for (int col = 0; col <= 20; col++)
            {
                Label label = new Label
                {
                    Text = erfTable[col].ToString("0.00000"), // Форматируем значение
                    TextAlign = ContentAlignment.MiddleCenter
                };
                tableLayoutPanel1.Controls.Add(label, col + 1, 2); // Добавляем в третью строку
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Lab7_1_N2();
        }
        void Lab7_1_N2()
        {
            int n = (int)numericUpDown1.Value; // число разбиений интеграла

            // Функция для вычисления значения подынтегрального выражения
            double FunctionN2(double x)
            {
                return 4.0 / (1 + x * x);
            }

            // Метод трапеций
            double TrapezoidalRule(Func<double, double> f, double a, double b, int n)
            {
                double h = (b - a) / n;
                double sum = 0.5 * (f(a) + f(b));

                for (int i = 1; i < n; i++)
                {
                    sum += f(a + i * h);
                }

                return sum * h;
            }

            // Метод прямоугольников (средние точки)
            double MidpointRule(Func<double, double> f, double a, double b, int n)
            {
                double h = (b - a) / n;
                double sum = 0.0;

                for (int i = 0; i < n; i++)
                {
                    double xMid = a + (i + 0.5) * h;
                    sum += f(xMid);
                }

                return sum * h;
            }

            // Метод сплайн-квадратуры (реализация через кубические сплайны)
            double SplineQuadrature(Func<double, double> f, double a, double b, int n)
            {
                double h = (b - a) / n;
                double[] xValues = new double[n + 1];
                double[] yValues = new double[n + 1];

                for (int i = 0; i <= n; i++)
                {
                    double x = a + i * h;
                    xValues[i] = x;
                    yValues[i] = f(x);
                }

                double integral = 0.0;
                for (int i = 0; i < n; i++)
                {
                    h = xValues[i + 1] - xValues[i];
                    integral += (h / 6) * (yValues[i] + 4 * f((xValues[i] + xValues[i + 1]) / 2) + yValues[i + 1]);
                }

                return integral;
            }

            // Integration limits
            double a = 0.0;
            double b = 1.0;

            // Compute π using different methods
            double piTrapezoidal = TrapezoidalRule(FunctionN2, a, b, n);
            double piMidpoint = MidpointRule(FunctionN2, a, b, n);
            double piSpline = SplineQuadrature(FunctionN2, a, b, n);

            // True value of π
            double truePi = Math.PI;

            // Compute errors
            double errorTrapezoidal = Math.Abs(truePi - piTrapezoidal);
            double errorMidpoint = Math.Abs(truePi - piMidpoint);
            double errorSpline = Math.Abs(truePi - piSpline);

            // Display results with enlarged font
            Font font = new Font("Segoe UI", 12, FontStyle.Regular);
            Form messageBox = new Form()
            {
                Width = 600,
                Height = 300,
                Text = "Calculation Results"
            };

            Label label = new Label()
            {
                AutoSize = true,
                Font = font,
                Text = $"Приближённые значения числа π для {n} разбиений:\n" +
                $"Метод трапеций:\n {piTrapezoidal:F8} " +
                $"(ошибка: {errorTrapezoidal:F8}~h^2={Math.Pow(1.0 / n, 2):F8})\n" +
                $"Метод прямоугольников:\n {piMidpoint:F8} " +
                $"(ошибка: {errorMidpoint:F8}~h^2={Math.Pow(1.0 / n, 2):F8})\n" +
                $"Метод сплайн-квадратуры:\n {piSpline:F8} " +
                $"(ошибка: {errorSpline:F12}~h^4={Math.Pow(1.0 / n, 4):F12})",
                Dock = DockStyle.Fill
            };
            messageBox.StartPosition = FormStartPosition.CenterScreen;
            messageBox.Controls.Add(label);
            messageBox.ShowDialog();
        }
        void Lab7_1_N3()
        {
            // Function definition
            double FunctionN3(double x)
            {
                if (0 <= x && x <= 2)
                    return Math.Exp(x * x);
                if (2 < x && x <= 4)
                    return 1.0 / (4 - Math.Sin(16 * Math.PI * x));
                return 0; 
            }
            double SimpsonRule(Func<double, double> f, double a, double b, int n)
            {
                // n должно быть нечёт
                if (n % 2 != 0)
                {
                    n++;
                }

                double h = (b - a) / (double)n;
                double sum = f(a) + f(b);

                for (int i = 1; i < n; i++)
                {
                    double x = a + i * h;
                    if (i % 2 == 0)
                    {
                        sum += 2 * f(x);
                    }
                    else
                    {
                        sum += 4 * f(x);
                    }
                }

                return (h / 3) * sum;
            }
            // пределы интегрирования
            double a = 0.0;
            double b = 4.0;
            int n = 1000;

            otvet.Text = "Мой ответ: " + SimpsonRule(FunctionN3, a, b, n) + "\nОнлайн калькулятор: 16.452628 + 0.51639778 = "+ (16.452628 + 0.51639778);
        }
    }
}
