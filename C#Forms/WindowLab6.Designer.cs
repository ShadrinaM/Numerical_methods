﻿namespace C_Forms
{
    partial class WindowLab6
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
            Task1 = new Button();
            Task2 = new Button();
            Task3 = new Button();
            Task4 = new Button();
            SuspendLayout();
            // 
            // Task1
            // 
            Task1.Location = new Point(16, 12);
            Task1.Margin = new Padding(3, 2, 3, 2);
            Task1.Name = "Task1";
            Task1.Size = new Size(82, 22);
            Task1.TabIndex = 0;
            Task1.Text = "Задание 1";
            Task1.UseVisualStyleBackColor = true;
            Task1.Click += Task1_Click;
            // 
            // Task2
            // 
            Task2.Location = new Point(103, 12);
            Task2.Margin = new Padding(3, 2, 3, 2);
            Task2.Name = "Task2";
            Task2.Size = new Size(82, 22);
            Task2.TabIndex = 1;
            Task2.Text = "Задание 2";
            Task2.UseVisualStyleBackColor = true;
            Task2.Click += Task2_Click;
            // 
            // Task3
            // 
            Task3.Location = new Point(191, 12);
            Task3.Margin = new Padding(3, 2, 3, 2);
            Task3.Name = "Task3";
            Task3.Size = new Size(82, 22);
            Task3.TabIndex = 2;
            Task3.Text = "Задание 3";
            Task3.UseVisualStyleBackColor = true;
            Task3.Click += Task3_Click;
            // 
            // Task4
            // 
            Task4.Location = new Point(278, 12);
            Task4.Margin = new Padding(3, 2, 3, 2);
            Task4.Name = "Task4";
            Task4.Size = new Size(82, 22);
            Task4.TabIndex = 3;
            Task4.Text = "Задание 4";
            Task4.UseVisualStyleBackColor = true;
            Task4.Click += Task4_Click;
            // 
            // WindowLab6
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(Task4);
            Controls.Add(Task3);
            Controls.Add(Task2);
            Controls.Add(Task1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "WindowLab6";
            Text = "WindowLab6";
            ResumeLayout(false);
        }

        #endregion

        private OxyPlot.WindowsForms.PlotView plotView;
        private Button Task1;
        private Button Task2;
        private Button Task3;
        private Button Task4;
    }
}