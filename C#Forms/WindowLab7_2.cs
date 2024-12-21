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
using System.Diagnostics;
using Label = System.Windows.Forms.Label;

namespace C_Forms
{
    public partial class WindowLab7_2 : Form
    {
        private Menu mainForm;

        public WindowLab7_2(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += WindowLab7_2_FormClosed;
            Lab72();
        }

        private void WindowLab7_2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Показываем главное окно при закрытии текущего
            mainForm.Show();
        }

        void Lab72()
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

            resultBox.ScrollBars = ScrollBars.Vertical;

            Lab7_2_N1();
            
            Lab7_2_N2A();
            Lab7_2_N2B();
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
        void Lab7_2_N1()
        {
            double step = 0.1;
            double maxX = 2.0;

            var results = "Сравнение методов для вычисления erf(x):\r\n";

            double[] MethodEuler(double step, double maxX)
            { //пошаговый итерационный метод
                int n = (int)(maxX / step) + 1;
                double[] erfValues = new double[n];
                double x = 0.0;
                double y = 0.0;

                for (int i = 0; i < n; i++)
                {
                    erfValues[i] = y;
                    y += step * (2 / Math.Sqrt(Math.PI) * Math.Exp(-x * x));
                    x += step;
                }

                return erfValues;
            }

            // Время для метода через ОДУ
            Stopwatch stopwatch = Stopwatch.StartNew();
            double[] erfODE = MethodEuler(step, maxX);
            stopwatch.Stop();
            TimeSpan durationODE = stopwatch.Elapsed;

            // Добавляем заголовок для четвертой строки
            tableLayoutPanel1.Controls.Add(new Label { Text = "ODE (erf(x))", TextAlign = ContentAlignment.MiddleCenter }, 0, 3);

            // Заполняем четвёртую строку значениями erf(x)
            for (int col = 0; col <= 20; col++)
            {
                double x = col * step;
                Label label = new Label
                {
                    Text = erfODE[col].ToString("0.00000"), // Форматируем значение
                    TextAlign = ContentAlignment.MiddleCenter
                };
                tableLayoutPanel1.Controls.Add(label, col + 1, 3); // Добавляем в четвёртую строку
            }
            results += string.Format("Время выполнения метода ОДУ: {0}\r\n", durationODE);



            // Время для метода из ЛР7_1
            stopwatch.Restart();
            Lab7_1_N1();
            stopwatch.Stop();
            TimeSpan durationLR6 = stopwatch.Elapsed;
            results += string.Format("Время выполнения метода ЛР6: {0}\r\n", durationLR6);

            //Запись результата сравнения в label
            label1.Text = results;
        }
        void Lab7_2_N2A()
        {
            (double[] t, double[] r, double[] f) RungeKutta(double r0, double f0, double alpha, double dt, double tMax)
            {
                int n = (int)Math.Ceiling(tMax / dt);
                double[] t = new double[n];
                double[] r = new double[n];
                double[] f = new double[n];

                // Устанавливаем начальные условия
                r[0] = r0;
                f[0] = f0;
                t[0] = 0;

                for (int i = 1; i < n; i++)
                {
                    t[i] = t[i - 1] + dt;

                    // Промежуточные вычисления по Рунге-Кутте
                    (double dr1, double df1) = Derivatives(r[i - 1], f[i - 1], alpha);
                    (double dr2, double df2) = Derivatives(r[i - 1] + 0.5 * dt * dr1, f[i - 1] + 0.5 * dt * df1, alpha);
                    (double dr3, double df3) = Derivatives(r[i - 1] + 0.5 * dt * dr2, f[i - 1] + 0.5 * dt * df2, alpha);
                    (double dr4, double df4) = Derivatives(r[i - 1] + dt * dr3, f[i - 1] + dt * df3, alpha);

                    // Обновляем значения r и f
                    r[i] = r[i - 1] + dt * (dr1 + 2 * dr2 + 2 * dr3 + dr4) / 6;
                    f[i] = f[i - 1] + dt * (df1 + 2 * df2 + 2 * df3 + df4) / 6;
                }

                return (t, r, f);
            }

            (double dr, double df) Derivatives(double r, double f, double alpha)
            {
                double dr = 2 * r - alpha * r * f;
                double df = -f + alpha * r * f;
                return (dr, df);
            }



            // Параметры задачи
            double alpha = 0.01;
            double dt = 0.1;
            double tMax = 200.0;

            // Начальные условия
            var initialConditions = new[]
            {
                new { r0 = 50.0, f0 = 5.0 },
                new { r0 = 100.0, f0 = 10.0 },
                new { r0 = 200.0, f0 = 20.0 },
                new { r0 = 1000.0, f0 = 50.0 }
            };

            string results = "Динамика Лисов и Кроликов\r\n";

            foreach (var cond in initialConditions)
            {
                results += $"\n       Начальные значения: r0={cond.r0}, f0={cond.f0}\r\n";

                // Решение системы уравнений
                var (t, r, f) = RungeKutta(cond.r0, cond.f0, alpha, dt, tMax);

                // Вывод результатов
                for (int i = 0; i < t.Length; i += 50) // Печатаем каждую 50-ю точку для наглядности
                {
                    results += string.Format("t={0:F1}, Кролики={1:F2}, Лисы={2:F2}\r\n", t[i], r[i], f[i]);
                }
            }

            resultBox.AppendText(results);
            
        }
        void Lab7_2_N2B()
        {
            // Функция для вычисления изменений dr/dt и df/dt
            (double dr, double df) DerivativesStop(double r, double f, double alpha)
            {
                double dr = 2 * r - alpha * r * f;
                double df = -f + alpha * r * f;
                return (dr, df);
            }
            // Метод Рунге-Кутты 4-го порядка
            void RungeKuttaWithStop(double r0, double f0, double alpha, double dt, double tMax, string caseName)
            {
                int n = (int)Math.Ceiling(tMax / dt);
                double[] t = new double[n];
                double[] r = new double[n];
                double[] f = new double[n];

                // Устанавливаем начальные условия
                r[0] = r0;
                f[0] = f0;
                t[0] = 0;

                for (int i = 1; i < n; i++)
                {
                    t[i] = t[i - 1] + dt;

                    // Промежуточные вычисления по Рунге-Кутте
                    (double dr1, double df1) = DerivativesStop(r[i - 1], f[i - 1], alpha);
                    (double dr2, double df2) = DerivativesStop(r[i - 1] + 0.5 * dt * dr1, f[i - 1] + 0.5 * dt * df1, alpha);
                    (double dr3, double df3) = DerivativesStop(r[i - 1] + 0.5 * dt * dr2, f[i - 1] + 0.5 * dt * df2, alpha);
                    (double dr4, double df4) = DerivativesStop(r[i - 1] + dt * dr3, f[i - 1] + dt * df3, alpha);

                    // Обновляем значения r и f
                    r[i] = r[i - 1] + dt * (dr1 + 2 * dr2 + 2 * dr3 + dr4) / 6;
                    f[i] = f[i - 1] + dt * (df1 + 2 * df2 + 2 * df3 + df4) / 6;

                    // Проверка условий вымирания
                    if (r[i] < 1 && f[i] >= 1 && caseName == "Кролики")
                    {
                        resultBox.AppendText($"t={t[i]:F1}, Кролики вымерли! (Кролики={r[i]:F2}, Лисы={f[i]:F2})\r\n");
                        return;
                    }
                    if (f[i] < 1 && r[i] >= 1 && caseName == "Лисы")
                    {
                        resultBox.AppendText($"t={t[i]:F1}, Лисы вымерли! (Кролики={r[i]:F2}, Лисы={f[i]:F2})\r\n");
                        return;
                    }
                    if (r[i] < 1 && f[i] < 1 && caseName == "Оба вида")
                    {
                        resultBox.AppendText($"t={t[i]:F1}, Оба вида вымерли! (Кролики={r[i]:F2}, Лисы={f[i]:F2})\r\n");
                        return;
                    }
                }

                // Если вымирания не произошло
                resultBox.AppendText($"t={tMax:F1}, Итерации завершены без вымирания. (Кролики={r[n - 1]:F2}, Лисы={f[n - 1]:F2})\r\n");
            }


            bool RungeKuttaCheckExtinction(double r0, double alpha, double dt, double tMax)
            {
                int n = (int)Math.Ceiling(tMax / dt);
                double[] t = new double[n];
                double[] r = new double[n];
                double[] f = new double[n];

                // Initialize initial conditions
                r[0] = r0;
                f[0] = r0;
                t[0] = 0;

                for (int i = 1; i < n; i++)
                {
                    t[i] = t[i - 1] + dt;

                    // Intermediate Runge-Kutta calculations
                    (double dr1, double df1) = DerivativesStop(r[i - 1], f[i - 1], alpha);
                    (double dr2, double df2) = DerivativesStop(r[i - 1] + 0.5 * dt * dr1, f[i - 1] + 0.5 * dt * df1, alpha);
                    (double dr3, double df3) = DerivativesStop(r[i - 1] + 0.5 * dt * dr2, f[i - 1] + 0.5 * dt * df2, alpha);
                    (double dr4, double df4) = DerivativesStop(r[i - 1] + dt * dr3, f[i - 1] + dt * df3, alpha);

                    // Update values
                    r[i] = r[i - 1] + dt * (dr1 + 2 * dr2 + 2 * dr3 + dr4) / 6;
                    f[i] = f[i - 1] + dt * (df1 + 2 * df2 + 2 * df3 + df4) / 6;

                    // Check extinction condition
                    if (r[i] < 1 && f[i] < 1)
                    {
                        resultBox.AppendText($"Найдено вымирание: r0=f0={r0:F2} на t={t[i]:F1}\r\n");
                        return true;
                    }
                }

                return false;
            }


            // Parameters
            double alpha = 0.01;
            double dt = 0.01;
            double tMax = 1000.0;

            // Случай 1: Вымирание кроликов
            resultBox.AppendText("\r\nСлучай 1: Вымирание кроликов (r0=15, f0=22)\r\n");
            RungeKuttaWithStop(15, 22, alpha, dt, tMax, "Кролики");

            // Случай 2: Вымирание лис
            resultBox.AppendText("\r\nСлучай 2: Вымирание лис (r0=1000, f0=1)\r\n");
            RungeKuttaWithStop(1000, 1, alpha, dt, tMax, "Лисы");

            // Случай 3: Вымирание обоих видов
            resultBox.AppendText("\r\nСлучай 3: Вымирание обоих видов (r0=f0)\r\n");
            double r0 = 1.0;
            double r0Max = 1100.0; // Upper limit for search
            double step = 1.0;
            while (r0 <= r0Max)
            {
                if (RungeKuttaCheckExtinction(r0, alpha, dt, tMax))
                {
                    return; // Stop once the first condition is found
                }
                r0 += step;
            }

        }

    }
}
