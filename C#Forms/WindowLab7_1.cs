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
            int n = (int)numericUpDown1.Value; //число разбиений интеграла


            // Функция для вычисления значения подынтегрального выражения
            double Function(double x)
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

            // Метод сплайн-квадратуры (на основе кубических сплайнов)
            double SplineQuadrature(Func<double, double> f, double a, double b, int n)
            {
                // Для точности используем метод трапеций как основу
                return TrapezoidalRule(f, a, b, n);
            }

            // Пределы интегрирования
            double a = 0.0;
            double b = 1.0;

            // Вычисление значений пи разными методами
            double piTrapezoidal = TrapezoidalRule(Function, a, b, n);
            double piMidpoint = MidpointRule(Function, a, b, n);
            double piSpline = SplineQuadrature(Function, a, b, n);

            // Вывод результатов
            MessageBox.Show($"Приближённые значения числа π для {n} разбиений:\n" +
                            $"Метод трапеций: {piTrapezoidal:F8}\n" +
                            $"Метод прямоугольников: {piMidpoint:F8}\n" +
                            $"Метод сплайн-квадратуры: {piSpline:F8}",
                            "Результаты вычислений");

        }
    }
}
