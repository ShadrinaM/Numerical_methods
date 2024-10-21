namespace C_Forms
{
    partial class Menu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            StartTask4 = new Button();
            StartTask5_2 = new Button();
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe Print", 15.75F);
            button1.Location = new Point(36, 158);
            button1.Name = "button1";
            button1.Size = new Size(339, 54);
            button1.TabIndex = 0;
            button1.Text = "Лабораторная работа 1";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe Print", 15.75F);
            button2.Location = new Point(36, 218);
            button2.Name = "button2";
            button2.Size = new Size(339, 54);
            button2.TabIndex = 1;
            button2.Text = "Лабораторная работа 2";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Font = new Font("Segoe Print", 15.75F);
            button3.Location = new Point(36, 278);
            button3.Name = "button3";
            button3.Size = new Size(339, 54);
            button3.TabIndex = 2;
            button3.Text = "Лабораторная работа 3_1";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Font = new Font("Segoe Print", 15.75F);
            button4.Location = new Point(36, 338);
            button4.Name = "button4";
            button4.Size = new Size(339, 54);
            button4.TabIndex = 3;
            button4.Text = "Лабораторная работа 3_2";
            button4.UseVisualStyleBackColor = true;
            // 
            // StartTask4
            // 
            StartTask4.Font = new Font("Segoe Print", 15.75F);
            StartTask4.Location = new Point(36, 398);
            StartTask4.Name = "StartTask4";
            StartTask4.Size = new Size(339, 54);
            StartTask4.TabIndex = 4;
            StartTask4.Text = "Лабораторная работа 4";
            StartTask4.UseVisualStyleBackColor = true;
            StartTask4.Click += StartTask4_Click;
            // 
            // StartTask5_2
            // 
            StartTask5_2.Font = new Font("Segoe Print", 15.75F);
            StartTask5_2.Location = new Point(36, 458);
            StartTask5_2.Name = "StartTask5_2";
            StartTask5_2.Size = new Size(339, 54);
            StartTask5_2.TabIndex = 5;
            StartTask5_2.Text = "Лабораторная работа 5_2";
            StartTask5_2.UseVisualStyleBackColor = true;
            StartTask5_2.Click += StartTask5_2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe Print", 25F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(713, 118);
            label1.TabIndex = 6;
            label1.Text = "Лабораторные работы по предмету \r\n\"Методы вычислений\"";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe Print", 15.75F);
            label2.Location = new Point(700, 91);
            label2.Name = "label2";
            label2.Size = new Size(272, 36);
            label2.TabIndex = 7;
            label2.Text = "Шадрина Марина 35/1";
            // 
            // pictureBox1
            // 
            pictureBox1.AccessibleRole = AccessibleRole.None;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(381, 158);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(591, 354);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(StartTask5_2);
            Controls.Add(StartTask4);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Menu";
            Text = "Menu";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button StartTask4;
        private Button StartTask5_2;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
    }
}