using System.Data;
using System;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

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

        void LabUCHP()
        {
            // Параметры
            double L = Math.PI / 2;
            double T = 1;
            int Nx = 10;
            int Nt = 2 * Nx * Nx;

            double dx = L / (Nx - 0.5);
            double dt = T / Nt;

            double r = dt / (dx * dx);

            double[] xValues = Enumerable.Range(0, Nx).Select(i => i * L / (Nx - 1)).ToArray();
            double[] tValues = Enumerable.Range(0, Nt).Select(i => i * T / (Nt - 1)).ToArray();

            double[,] u = new double[Nx, Nt];
            double[,] u1 = new double[Nx, Nt];

            // Начальные условия
            for (int i = 0; i < Nx; i++)
            {
                u[i, 0] = Math.Sin(xValues[i]);
                u1[i, 0] = Math.Sin(xValues[i]);
            }

            // Граничные условия
            for (int j = 0; j < Nt; j++)
            {
                u[0, j] = 0;
                u[Nx - 1, j] = Math.Exp(-tValues[j]);
                u1[0, j] = 0;
                u1[Nx - 1, j] = Math.Exp(-tValues[j]);
            }

            // Явная разностная схема
            for (int t = 0; t < Nt - 1; t++)
            {
                for (int x = 1; x < Nx - 1; x++)
                {
                    u[x, t + 1] = r * u[x - 1, t] + (1 - 2 * r) * u[x, t] + r * u[x + 1, t];
                }
            }

            // Неявная разностная схема
            for (int t = 0; t < Nt - 1; t++)
            {
                double[,] A = new double[Nx - 1, Nx - 1];
                double[] b = new double[Nx - 1];

                for (int i = 0; i < Nx - 1; i++)
                {
                    A[i, i] = 1 + 2 * r;
                    if (i > 0) A[i, i - 1] = -r;
                    if (i < Nx - 2) A[i, i + 1] = -r;
                }

                for (int i = 0; i < Nx - 1; i++)
                {
                    b[i] = u1[i + 1, t];
                    if (i == 0) b[i] += r * u1[0, t + 1];
                    if (i == Nx - 2) b[i] += r * u1[Nx - 1, t + 1];
                }

                double[] solution = SolveLinearSystem(A, b);
                for (int i = 0; i < Nx - 1; i++)
                {
                    u1[i + 1, t + 1] = solution[i];
                }
            }

            // Визуализация
            PlotSolution("Явная схема", xValues, tValues, u);
            PlotSolution("Неявная схема", xValues, tValues, u1);
        }

        double[] SolveLinearSystem(double[,] A, double[] b)
        {
            // Решение системы линейных уравнений методом Гаусса
            int n = b.Length;
            double[] x = new double[n];
            double[,] matrix = (double[,])A.Clone();
            double[] rhs = (double[])b.Clone();

            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    double factor = matrix[i, k] / matrix[k, k];
                    for (int j = k; j < n; j++)
                    {
                        matrix[i, j] -= factor * matrix[k, j];
                    }
                    rhs[i] -= factor * rhs[k];
                }
            }

            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = rhs[i];
                for (int j = i + 1; j < n; j++)
                {
                    x[i] -= matrix[i, j] * x[j];
                }
                x[i] /= matrix[i, i];
            }

            return x;
        }

        void PlotSolution(string title, double[] xValues, double[] tValues, double[,] u)
        {
            var graph = new ZedGraphControl();
            //graph.Dock = DockStyle.Fill;
            this.Controls.Add(graph);

            var pane = graph.GraphPane;
            pane.Title.Text = title;
            pane.XAxis.Title.Text = "X";
            pane.YAxis.Title.Text = "T";

            for (int i = 0; i < tValues.Length; i += tValues.Length / 10)
            {
                var curve = pane.AddCurve($"t = {tValues[i]:F2}",
                    new PointPairList(xValues, Enumerable.Range(0, xValues.Length).Select(x => u[x, i]).ToArray()),
                    System.Drawing.Color.Blue,
                    SymbolType.None);
            }

            graph.AxisChange();
            graph.Refresh();
        }
    }

}
