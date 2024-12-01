namespace C_Forms
{
    partial class WindowLab5_2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BackToMenu = new Button();
            panel1 = new Panel();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            plotView1 = new OxyPlot.WindowsForms.PlotView();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // BackToMenu
            // 
            BackToMenu.Location = new Point(423, 684);
            BackToMenu.Margin = new Padding(3, 4, 3, 4);
            BackToMenu.Name = "BackToMenu";
            BackToMenu.Size = new Size(113, 31);
            BackToMenu.TabIndex = 0;
            BackToMenu.Text = "BackToMenu";
            BackToMenu.UseVisualStyleBackColor = true;
            BackToMenu.Click += BackToMenu_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Controls.Add(BackToMenu);
            panel1.Location = new Point(571, 13);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(539, 719);
            panel1.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label3.Location = new Point(11, 489);
            label3.Name = "label3";
            label3.Size = new Size(55, 23);
            label3.TabIndex = 4;
            label3.Text = "label3";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label2.Location = new Point(11, 315);
            label2.Name = "label2";
            label2.Size = new Size(55, 23);
            label2.TabIndex = 3;
            label2.Text = "label2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(11, 60);
            label1.Name = "label1";
            label1.Size = new Size(423, 35);
            label1.TabIndex = 2;
            label1.Text = "Исходная табличная зависимость:";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 7;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.990654F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57.009346F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 59F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 57F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 49F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 49F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            tableLayoutPanel1.Location = new Point(11, 120);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(389, 117);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // plotView1
            // 
            plotView1.Location = new Point(14, 13);
            plotView1.Margin = new Padding(3, 4, 3, 4);
            plotView1.Name = "plotView1";
            plotView1.PanCursor = Cursors.Hand;
            plotView1.Size = new Size(551, 719);
            plotView1.TabIndex = 2;
            plotView1.Text = "plotView1";
            plotView1.ZoomHorizontalCursor = Cursors.SizeWE;
            plotView1.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotView1.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // WindowLab5_2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1125, 748);
            Controls.Add(plotView1);
            Controls.Add(panel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "WindowLab5_2";
            Text = "WindowLab5_2";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button BackToMenu;
        private Panel panel1;
        private OxyPlot.WindowsForms.PlotView plotView1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}