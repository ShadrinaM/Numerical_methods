using System;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace C_Forms
{
    public partial class WindowLab1 : Form
    {
        private Menu mainForm;
        private TextBox textBox1;

        public WindowLab1(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += LR1_FormClosed;
            Lab1();
        }

        private void LR1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        //void Lab1()
        //{

        //    // ЗДЕСЬ НЕОБХОДИМО ПЕРЕПИСАТЬ КОД
        //    textBox1.AppendText($"//таким образом весь текст выводимы программой должен быть выведен в окно\r\n");

        //}

        void Lab1()
        {
            textBox1.AppendText("Введите номер задания из ЛР1 для проверки (1-4):\r\n");

            // Для демонстрации выведем все задания последовательно
            textBox1.AppendText("\r\n=== Задание 1 ===\r\n");
            Number_1();

            textBox1.AppendText("\r\n=== Задание 2 ===\r\n");
            Number_2();

            textBox1.AppendText("\r\n=== Задание 3 ===\r\n");
            Number_3();

            textBox1.AppendText("\r\n=== Задание 4 ===\r\n");
            Number_4();
        }

        double ERF(double x)
        {
            double sum = 0, el, fact;
            int n = 0;
            const double M_PI = 3.14159265358979323846;
            do
            {
                fact = 1;
                if (n != 0)
                {
                    for (int i = 1; i <= n; i++)
                    {
                        fact *= i;
                    }
                }
                el = 2 / Math.Sqrt(M_PI) * (Math.Pow(-1, n) * Math.Pow(x, 2 * n + 1)) / (fact * (2 * n + 1));
                sum += el;
                n++;
            } while (Math.Abs(el) > Math.Pow(10, -16));
            return sum;
        }

        void Number_1()
        {
            double[] a = { 0.5, 1, 5, 10 };
            double[] res = { 0.52050, 0.84270, 1, 1 };
            for (int i = 0; i < a.Length; i++)
            {
                textBox1.AppendText($"При x = {a[i]}\r\n");
                textBox1.AppendText($"ERF(x)={ERF(a[i])}\r\n");
                textBox1.AppendText($"Табличное значение={res[i]}\r\n");
                textBox1.AppendText($"Ошибка={res[i] - ERF(a[i])}\r\n\r\n");
            }
        }

        void fi(double x)
        {
            double sum = 0, element;
            int k = 1, n = 0;
            do
            {
                element = (1.0 / (k * (k + x)));
                sum += element;
                n++; k++;
            } while (element > 0.5 * 1e-8);
            textBox1.AppendText($"fi(x) = {sum}  n = {n}\r\n");
        }

        void fidif(double x)
        {
            double sum = 0, element;
            int k = 1, n = 0;
            do
            {
                element = ((1.0 - x) / (k * (k + x) * (k + 1.0)));
                sum += element;
                n++; k++;
            } while (element > 0.5 * 1e-8);
            textBox1.AppendText($"fdif(x) = {sum}  n = {n}\r\n");
        }

        void Number_2()
        {
            textBox1.AppendText("Вычислим сумму исходного ряда E(1/k(k-1)):\r\n\r\n");
            for (double x = 0; x <= 1; x += 0.1)
            {
                textBox1.AppendText($"При x = {x}\r\n");
                fi(x);
                textBox1.AppendText("\r\n");
            }
            textBox1.AppendText("\r\n\r\n\r\nДокажем что fi(1)=1 \r\n E(1/k-1-(k+1))=1+1/2+1/3+...-1/2-1/3...=1\r\n\r\n\r\n\r\n\r\n");
            textBox1.AppendText("Представим разность fi(x)-fi(1) рядом E(1.0 - x) / (k * (k + x) * (k + 1.0)) который сходится быстрее\r\n");
            for (double x = 0; x <= 1; x += 0.1)
            {
                textBox1.AppendText($"При x = {x}\r\n");
                fidif(x);
                textBox1.AppendText("\r\n");
            }
        }

        void s1(double x)
        {
            double sum = 0, element;
            int k = 1, n = 0;
            do
            {
                element = 1 / (Math.Sqrt(Math.Pow(k, 3) + x));
                sum += element;
                n++;
                k++;
                if (n % 25000 == 0) textBox1.AppendText($"{element}\r\n");
            } while (element > 3e-8);
            textBox1.AppendText($"s1(x)={sum}  n={n} микросек={n * 500}\r\n");
        }

        void s2(double x)
        {
            double sum = 0, element;
            int k = 1, n = 0;
            do
            {
                element = 1 / (Math.Sqrt(Math.Pow(k, 3) - x));
                sum += element;
                n++;
                k++;
                if (n % 25000 == 0) textBox1.AppendText($"{element}\r\n");
            } while (element > 3e-8);
            textBox1.AppendText($"s2(x)={sum}  n={n} микросек={n * 500}\r\n");
        }

        void sMod(double x)
        {
            double sum = 0, element;
            int k = 1, n = 0;
            do
            {
                element = (Math.Sqrt(Math.Pow(k, 3) - x) - Math.Sqrt(Math.Pow(k, 3) + x)) / (Math.Sqrt(Math.Pow(k, 5) + Math.Pow(x, 2)));
                sum += element;
                n++;
                k++;
            } while (element > 3e-8);
            textBox1.AppendText($"sMod(x)={sum}  n={n} микросек={n * 500}\r\n");
        }

        void Number_3()
        {
            textBox1.AppendText("Проверим на сходимость ряды из s(x)\r\n");
            textBox1.AppendText("Для s1(x) =  1 / (sqrt(pow(k,3) + x))\r\n");
            for (double x = -0.9; x <= 0.9; x += 0.2)
            {
                textBox1.AppendText($"При x = {x}\r\n");
                s1(x);
                textBox1.AppendText("\r\n");
            }
            textBox1.AppendText("\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            textBox1.AppendText("Для s2(x) =  1 / (sqrt(pow(k,3) - x))\r\n");
            for (double x = -0.9; x <= 0.9; x += 0.2)
            {
                textBox1.AppendText($"При x = {x}\r\n");
                s2(x);
                textBox1.AppendText("\r\n");
            }
            textBox1.AppendText("\r\n\r\n\r\n\r\n\r\n\r\nЭлементы стремятся к нулю, значит ряды сходятся\r\n");
            textBox1.AppendText("Применяя модифицированное S(x)=(sqrt(pow(k, 3)-x)- sqrt(pow(k, 3) + x))/(sqrt(pow(k, 5) + pow(x,2)))  получим\r\n");
            for (double x = -0.9; x <= 0.9; x += 0.2)
            {
                textBox1.AppendText($"При x = {x}\r\n");
                sMod(x);
                textBox1.AppendText("\r\n");
            }
        }

        void f()
        {
            double sum = 0, element;
            int k = 0, n = 1;
            do
            {
                element = (1.0 / (n * n + 1));
                sum += element;
                n++; k++;
            } while (element > 0.0000000001);
            textBox1.AppendText($"E... = {sum} kol = {k}\r\n");
        }

        void ff()
        {
            double sum = 0, element;
            int k = 0, n = 1;
            const double M_PI = 3.14159265358979323846;
            do
            {
                element = (1.0 / (Math.Pow(n, 4) * (Math.Pow(n, 2) + 1)));
                sum += element;
                n++; k++;
            } while (element > 0.0000000001);
            sum += Math.Pow(M_PI, 2) / 6 - Math.Pow(M_PI, 4) / 90;
            textBox1.AppendText($"E... = {sum} kol = {k}\r\n");
        }

        void Number_4()
        {
            textBox1.AppendText("Вычислим сумму ряда E(1/n^2+1)):\r\n");
            f();
            textBox1.AppendText("\r\n\r\nВзяв во внимание E(1/n^2=pi^2/6), E(1/n^4=pi^4/90)\r\nПолучим pi^2/6-pi^4/90+E(1/n^4(n^2+1))\r\nСумма преобразованного ряда:\r\n");
            ff();
        }
    }
}
