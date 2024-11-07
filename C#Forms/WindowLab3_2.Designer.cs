namespace C_Forms
{
    partial class WindowLab3_2
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
            label1 = new Label();
            BackToMenu = new Button();
            button1 = new Button();
            button2 = new Button();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Nirmala UI", 13.8F, FontStyle.Bold);
            label1.Location = new Point(22, 20);
            label1.Name = "label1";
            label1.Size = new Size(79, 31);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // BackToMenu
            // 
            BackToMenu.Location = new Point(997, 703);
            BackToMenu.Name = "BackToMenu";
            BackToMenu.Size = new Size(116, 33);
            BackToMenu.TabIndex = 6;
            BackToMenu.Text = "BackToMenu";
            BackToMenu.UseVisualStyleBackColor = true;
            BackToMenu.Click += BackToMenu_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe Print", 9F, FontStyle.Bold);
            button1.Location = new Point(22, 95);
            button1.Name = "button1";
            button1.Size = new Size(125, 44);
            button1.TabIndex = 7;
            button1.Text = "Матрица 1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe Print", 9F, FontStyle.Bold);
            button2.Location = new Point(207, 95);
            button2.Name = "button2";
            button2.Size = new Size(125, 44);
            button2.TabIndex = 8;
            button2.Text = "Матрица 2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Nirmala UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(22, 162);
            label2.Name = "label2";
            label2.Size = new Size(63, 25);
            label2.TabIndex = 9;
            label2.Text = "label2";
            // 
            // WindowLab3_2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1125, 748);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(BackToMenu);
            Controls.Add(label1);
            Name = "WindowLab3_2";
            Text = "WindowLab3_2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Button BackToMenu;
        private Button button1;
        private Button button2;
        private Label label2;
    }
}