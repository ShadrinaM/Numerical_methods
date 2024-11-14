
using MathNet.Numerics.LinearAlgebra;
using OxyPlot;
using OxyPlot.Series;
using MathNet.Numerics.LinearAlgebra.Double;

namespace C_Forms
{
    public partial class WindowLab4 : Form
    {
        private Menu mainForm;
        public WindowLab4(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            // Сохраняем ссылку на главное окно
            this.mainForm = menushka;
            // Подписка на событие закрытия формы
            this.FormClosed += WindowLab4_FormClosed;
            Lab4();
        }

        void Lab4()
        {
            // Заданная матрица A и вектор b
            double[,] A = {
                { 12.14, 1.32, -0.78, -2.75 },
                { -0.89, 16.75, 1.88, -1.55 },
                { 2.65, -1.27, -15.64, -0.64 },
                { 2.44, 1.52, 1.93, -11.43 }
            };

            double[] b = { 14.78, -12.14, -11.65, 4.26 };

            // Преобразуем в матричные структуры
            var matrixA = DenseMatrix.OfArray(A);
            var vectorB = DenseVector.OfArray(b);

            // Задание начального приближения
            double[] initialGuess = { 0.0, 0.0, 0.0, 0.0 };

            double tolerance = 1e-6;  // точность
            int maxIterations = 1000; // максимальное количество итераций


            (var solutionJacobi, var residualJacobi) = JacobiMethod(matrixA, vectorB, initialGuess, tolerance, maxIterations);
            label1.Text = "Метод Якоби\n";
            label1.Text += PrintSolution(solutionJacobi);
            label3.Text = "Метод Якоби невязки\n";
            label3.Text += PrintResidual(residualJacobi);

            // Найдем итерацию и значение невязки, при котором достигнута точность для метода Якоби
            int iterationJacobi = residualJacobi.FindIndex(r => r < tolerance) + 1;
            if (iterationJacobi > 0)
            {
                label5.Text = $"Якоби: Невязка достигла {residualJacobi[iterationJacobi - 1]:F6} на итерации {iterationJacobi}";
            }

            (var solutionSeidel, var residualSeidel) = SeidelMethod(matrixA, vectorB, initialGuess, tolerance, maxIterations);
            label2.Text = "Метод Зейделя\n";
            label2.Text += PrintSolution(solutionSeidel);
            label4.Text = "Метод Зейделя невязки\n";
            label4.Text += PrintResidual(residualSeidel);

            // Найдем итерацию и значение невязки, при котором достигнута точность для метода Зейделя
            int iterationSeidel = residualSeidel.FindIndex(r => r < tolerance) + 1;
            if (iterationSeidel > 0)
            {
                label6.Text = $"Зейдель: Невязка достигла {residualSeidel[iterationSeidel - 1]:F6} на итерации {iterationSeidel}";
            }

            // Построение графика невязки
            plotView1.Model = PlotResiduals(residualJacobi, residualSeidel);
        }

        /// <summary>
        /// Метод Якоби для решения системы линейных уравнений
        /// </summary>
        /// <param name="A"> Матрица коэффициентов 𝐴 системы линейных уравнений</param>
        /// <param name="b"> Вектор правой части b </param>
        /// <param name="initialGuess"> начальное приближение для переменных х (массив значений) </param>
        /// <param name="tolerance"> Требуемая точность сходимости (критерий завершения) </param>
        /// <param name="maxIterations"> Максимальное количество итераций </param>
        /// <returns> Итоговый вектор решения х и список невязок (остатков), который показывает, как изменялась ошибка между итерациями. </returns>
        static (Vector<double>, List<double>) JacobiMethod(Matrix<double> A, Vector<double> b, double[] initialGuess, double tolerance, int maxIterations)
        {
            int n = A.RowCount;
            var x = DenseVector.OfArray(initialGuess); //значения приближения
            var xOld = DenseVector.OfArray(initialGuess); //значения приближения прошлой итерации
            List<double> residuals = new List<double>(); // невязки

            for (int iter = 0; iter < maxIterations; iter++)
            {
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            sum += A[i, j] * xOld[j];
                        }
                    }
                    x[i] = (b[i] - sum) / A[i, i];
                }

                double residual = (x - xOld).L2Norm();
                residuals.Add(residual);
                if (residual < tolerance)
                    break;

                xOld = (DenseVector)x.Clone();
            }

            return (x, residuals);
        }

        /// <summary>
        /// Метод Зейделя для решения системы линейных уравнений
        /// </summary>
        /// <param name="A"> Матрица коэффициентов 𝐴 системы линейных уравнений</param>
        /// <param name="b"> Вектор правой части b </param>
        /// <param name="initialGuess"> начальное приближение для переменных х (массив значений) </param>
        /// <param name="tolerance"> Требуемая точность сходимости (критерий завершения) </param>
        /// <param name="maxIterations"> Максимальное количество итераций </param>
        /// <returns> Итоговый вектор решения х и список невязок (остатков), который показывает, как изменялась ошибка между итерациями. </returns>
        static (Vector<double>, List<double>) SeidelMethod(Matrix<double> A, Vector<double> b, double[] initialGuess, double tolerance, int maxIterations)
        {
            int n = A.RowCount;
            var x = DenseVector.OfArray(initialGuess); //значения приближения
            List<double> residuals = new List<double>(); // невязки

            for (int iter = 0; iter < maxIterations; iter++)
            {
                var xOld = x.Clone();

                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            sum += A[i, j] * x[j];
                        }
                    }
                    x[i] = (b[i] - sum) / A[i, i];
                }

                double residual = (x - xOld).L2Norm();
                residuals.Add(residual);
                if (residual < tolerance)
                    break;
            }

            return (x, residuals);
        }

        // Формирование строки решения
        static string PrintSolution(Vector<double> solution)
        {
            string s = "";
            s += "Решение:\n";
            for (int i = 0; i < solution.Count; i++)
            {
                s += $"x{i + 1} = {solution[i]:F6}\n";
            }
            return s;
        }

        // Формирование строки  невязок
        static string PrintResidual(List<double> residual)
        {
            string s = "";
            for (int i = 0; i < residual.Count; i++)
            {
                s += $"x{i + 1} = {residual[i]:F6}\n";
            }
            return s;
        }

        // Построение графика
        static PlotModel PlotResiduals(List<double> residualJacobi, List<double> residualSeidel)
        {
            var plotModel = new PlotModel { Title = "Зависимость нормы невязки от номера итерации" };

            var jacobiSeries = new LineSeries { Title = "Якоби", MarkerType = MarkerType.Circle };
            for (int i = 0; i < residualJacobi.Count; i++)
            {
                jacobiSeries.Points.Add(new OxyPlot.DataPoint(i + 1, residualJacobi[i]));
            }

            var seidelSeries = new LineSeries { Title = "Зейдель", MarkerType = MarkerType.Square };
            for (int i = 0; i < residualSeidel.Count; i++)
            {
                seidelSeries.Points.Add(new OxyPlot.DataPoint(i + 1, residualSeidel[i]));
            }

            plotModel.Series.Add(jacobiSeries);
            plotModel.Series.Add(seidelSeries);

            return plotModel;
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

    }
}
