namespace LogMe
{
    partial class frmLogMe
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbTask = new System.Windows.Forms.TextBox();
            this.btTask = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Task";
            // 
            // tbTask
            // 
            this.tbTask.Location = new System.Drawing.Point(89, 29);
            this.tbTask.Name = "tbTask";
            this.tbTask.Size = new System.Drawing.Size(326, 26);
            this.tbTask.TabIndex = 1;
            // 
            // btTask
            // 
            this.btTask.Location = new System.Drawing.Point(89, 76);
            this.btTask.Name = "btTask";
            this.btTask.Size = new System.Drawing.Size(105, 37);
            this.btTask.TabIndex = 2;
            this.btTask.Text = "Log This";
            this.btTask.UseVisualStyleBackColor = true;
            this.btTask.Click += new System.EventHandler(this.btTask_Click);
            // 
            // frmLogMe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 125);
            this.Controls.Add(this.btTask);
            this.Controls.Add(this.tbTask);
            this.Controls.Add(this.label1);
            this.Name = "frmLogMe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogMe";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTask;
        private System.Windows.Forms.Button btTask;
    }
}

