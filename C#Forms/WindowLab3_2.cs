using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_Forms
{
    public partial class WindowLab3_2 : Form
    {
        private Menu mainForm;

        public static double[,] Matrix0no = {
            { 1, -1, -5, 0 },
            { 3, -2, -13, 0 },
            { 2, -1, -8, 0 },
        };

        public static double[,] Matrix0 = {
            { 2,-1, -3, -1 },
            { 3, 2, 1, 0 },
            { 1, 3, 4, 1 },
        };

        public static double[,] Matrix1 = {
        { 1.00, 0.80, 0.64, ERF(0.80) },
        { 1.00, 0.90, 0.81, ERF(0.90) },
        { 1.00, 1.10, 1.21, ERF(1.10) }
        };

        public static double[,] Matrix2or = {
        { 0.1, 0.2, 0.3, 0.1 },
        { 0.4, 0.5, 0.6, 0.3 },
        { 0.7, 0.8, 0.9, 0.5 },
        };
        public WindowLab3_2(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += WindowLab3_2_FormClosed;
            Lab32();

        }
        private void BackToMenu_Click(object sender, EventArgs e)
        {
            mainForm.Show();
            this.Close();
        }
        private void WindowLab3_2_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        private void Lab32()
        {
            label1.Text = "Выберите матрицу для решения (1, 2, enter для ввода):";

        }

        /// <summary>
        /// Считает функцию ERF(x)
        /// </summary>
        /// <param name="x">Аргумент функции</param>
        /// <returns> y=ERF(x) </returns>
        public static double ERF(double x)
        {
            double sum = 0, el, fact;
            int n = 0;
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
                el = 2 / Math.Sqrt(Math.PI) * (Math.Pow(-1, n) * Math.Pow(x, 2 * n + 1)) / (fact * (2 * n + 1));
                sum += el;
                n++;
            } while (Math.Abs(el) > Math.Pow(10, -16));
            return sum;
        }

        /// <summary> Применяет метод Гаусса к матрице </summary>
        /// <param name="selectedMatrix"> Расширенная матрица коэффициентов уравнений</param>
        /// <returns> Вектор решений </returns>
        double[] ApplicationOfGaussMethod(double[,] selectedMatrix)
        {
            double[,] originalSelectedMatrix = (double[,])selectedMatrix.Clone(); // клонирование исходной матрицы
            MatrixAndGaussianMethod gaussianElimination = new MatrixAndGaussianMethod(originalSelectedMatrix, selectedMatrix);
            label2.Text = gaussianElimination.printOriginalMatrix();

            // Получаем решение
            (double[] solution, string s) = gaussianElimination.GaussianEliminationWithPartialPivotSelection();

            // Проверка на наличие решения
            if (solution == null)
            {
                label2.Text += "Система имеет бесконечное количество решений. Невозможно вычислить невязки.";
                label2.Text += s;
                return null;
            }

            

            // Если решение существует, вычисляем невязки
            label2.Text += gaussianElimination.CalculateDiscrepanciesOriginalMatrix(solution);

            return solution;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedMatrix = (double[,])Matrix1.Clone();
            var solution1 = ApplicationOfGaussMethod(selectedMatrix);
            label2.Text += "Решение системы уравнений:\n";
            for (int i = 0; i < solution1.Length; i++)
            {
                label2.Text += $"x{i + 1} = {solution1[i]}\n";
            }
            label2.Text += "\n";
            double sum = 0;
            for (int i = 0; i < solution1.Length; i++)
            {
                sum += solution1[i];
            }
            label2.Text += $"x1 + x2 + x3 = {sum}\nERF(1.0) = {ERF(1.0)}\n|x1 + x2 + x3 - ERF(1.0)| = {sum - ERF(1.0)}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selectedMatrix = (double[,])Matrix2or.Clone();
            var solution2 = ApplicationOfGaussMethod(selectedMatrix);

        }
    }


    class MatrixAndGaussianMethod
    {
        /// <summary> 
        /// Оригинальная матрица 
        /// </summary>
        private double[,] originalMatrix;
        /// <summary> 
        /// Изменяемая матрица 
        /// </summary>
        private double[,] changMatrix;
        /// <summary> 
        /// Размерность системы 
        /// </summary>
        private int n;
        public MatrixAndGaussianMethod(double[,] originalMatrix, double[,] matrix)
        {
            this.originalMatrix = originalMatrix;
            this.changMatrix = matrix;
            this.n = matrix.GetLength(0);
        }

        /// <summary> 
        /// Вывод оригинальной матрицы 
        /// </summary>
        public string printOriginalMatrix()
        {
            string s = "Матрица исходная расширенная:\n";
            for (int i = 0; i < originalMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < originalMatrix.GetLength(1); j++)
                {
                    s += originalMatrix[i, j] + "   ";
                }
                s += "\n";
            }
            s += "\n";
            return s;
        }

        /// <summary> 
        /// Вывод верхнеугольной расширенной матрицы коэффициентов уравнения 
        /// </summary>
        public string printChangMatrix()
        {
            string s = "Матрица верхнеугольная расширенная:\n";
            for (int i = 0; i < changMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < changMatrix.GetLength(1); j++)
                {
                    s += changMatrix[i, j] + "\t";
                }
                s += "\n";
            }
            s += "\n";
            return s;
        }

        /// <summary> Гауссово исключение с частичным выбором ведущего элемента </summary>
        /// <returns> Массив решений если ранг = числу переменных, ФСР если меньше  </returns>
        public (double[],string) GaussianEliminationWithPartialPivotSelection()
        {
            double[] solution = new double[n];
            int rank = 0;

            // Прямой ход (приведение к треугольной форме)
            for (int i = 0; i < n; i++)
            {
                // Поиск максимального элемента в столбце для устойчивости
                int maxRow = i;
                for (int k = i + 1; k < n; k++)
                {
                    if (Math.Abs(changMatrix[k, i]) > Math.Abs(changMatrix[maxRow, i]))
                    {
                        maxRow = k;
                    }
                }

                // Обмен строк
                if (maxRow != i)
                {
                    for (int k = i; k < n + 1; k++)
                    {
                        double temp = changMatrix[maxRow, k];
                        changMatrix[maxRow, k] = changMatrix[i, k];
                        changMatrix[i, k] = temp;
                    }
                }

                // Приведение к треугольной форме
                if (Math.Abs(changMatrix[i, i]) < 1e-10)
                {
                    // Если ведущий элемент равен нулю, то ранг не увеличивается
                    continue;
                }

                rank++;
                for (int k = i + 1; k < n; k++)
                {
                    double factor = changMatrix[k, i] / changMatrix[i, i];
                    for (int j = i; j < n + 1; j++)
                    {
                        changMatrix[k, j] -= factor * changMatrix[i, j];
                    }
                }
            }

            if (rank < n)
            {
                // Если ранг матрицы меньше числа уравнений, строим фундаментальную систему решений
                string s=BuildFundamentalSystem(rank);
                return (null, s);
            }

            // Обратный ход (обратная подстановка)
            for (int i = n - 1; i >= 0; i--)
            {
                solution[i] = changMatrix[i, n] / changMatrix[i, i];
                for (int k = i - 1; k >= 0; k--)
                {
                    changMatrix[k, n] -= changMatrix[k, i] * solution[i];
                }
            }

            return (solution,"");
        }


        /// <summary>
        /// Строит фундаментальную систему решений для системы уравнений, если ранг меньше числа уравнений.
        /// </summary>
        /// <param name="rank">Ранг матрицы</param>

        private string BuildFundamentalSystem(int rank)
        {
            List<int> freeVariables = new List<int>(); // Список свободных переменных
            List<int> leadingVariables = new List<int>(); // Список базисных переменных

            // Определяем свободные и базисные переменные
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(changMatrix[i, i]) < 1e-10)
                {
                    freeVariables.Add(i); // Свободные переменные
                }
                else
                {
                    leadingVariables.Add(i); // Базисные переменные
                }
            }

            // Создаем массив для хранения частного решения
            double[] particularSolution = new double[n];

            // Находим частное решение (все свободные переменные = 0)
            for (int i = rank - 1; i >= 0; i--)
            {
                particularSolution[leadingVariables[i]] = changMatrix[i, n];
                for (int j = i + 1; j < n; j++)
                {
                    particularSolution[leadingVariables[i]] -= changMatrix[i, j] * particularSolution[j];
                }
                particularSolution[leadingVariables[i]] /= changMatrix[i, i];
            }

            // Массив для ФСР (фундаментальной системы решений)
            double[][] fundamentalSystem = new double[freeVariables.Count][];


            for (int i = 0; i < freeVariables.Count; i++)
            {
                double[] solution = new double[n];
                int freeVarIndex = freeVariables[i];

                // Присваиваем свободной переменной значение 1
                solution[freeVarIndex] = 1;

                // Выражаем базисные переменные через свободную переменную
                for (int j = rank - 1; j >= 0; j--)
                {
                    double value = 0; // значение для базисной переменной
                    for (int k = j + 1; k < n; k++)
                    {
                        value -= (changMatrix[j, k] * solution[k]) / changMatrix[j, j];
                    }
                    solution[leadingVariables[j]] = value;
                }

                // Добавляем базисный вектор в ФСР
                fundamentalSystem[i] = solution;
            }

            // Собираем строку с частным решением
            string s = "\n\nЧастное решение:\n";
            for (int i = 0; i < n; i++)
            {
                s+=$"x{i + 1} = {particularSolution[i]}\n";
            }

            // Собираем строку с фундаментальной системой решений
            s += "\nФундаментальная система решений (ФСР):\n(";
            for (int i = 0; i < n; i++)
            {           
                s += $"{particularSolution[i]}";
                if (i < n - 1)
                    s += "; ";
            }
            s += ")+";
            for (int i = 0; i < fundamentalSystem.Length; i++)
            {
                s += "(";
                for (int j = 0; j < n; j++)
                {
                    s += $"{fundamentalSystem[i][j]} ";
                    if (j < n - 1)
                        s += "; ";
                }
                s += ")";
                s += $"*c{i + 1}";
            }
            return s;
        }



        /// <summary>
        /// Подсчёт и вывод невязок по оригинальной матрице
        /// </summary>
        /// <param name="solution"> Массив решений </param>
        /// <returns> Массив невязок </returns>
        public string CalculateDiscrepanciesOriginalMatrix(double[] solution)
        {
            double[] residuals = new double[n];
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += originalMatrix[i, j] * solution[j];
                }
                residuals[i] = Math.Abs(originalMatrix[i, n] - sum);
            }
            // Сборка строки невязок по оригинальной матрице
            string s = "Невязки:\n";
            for (int i = 0; i < residuals.Length; i++)
            {
                s+=$"Невязка для уравнения {i + 1}: {residuals[i]}\n";
            }
            s += "\n";

            return s;
        }
    }
}
