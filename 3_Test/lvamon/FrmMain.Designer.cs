namespace lvamon
{
    partial class MainForm
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
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnTopology = new System.Windows.Forms.Button();
            this.btnExe = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.cbxTop = new System.Windows.Forms.CheckBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnDeactivate = new System.Windows.Forms.Button();
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnInstGet = new System.Windows.Forms.Button();
            this.btnInstSet = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtConsole.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtConsole.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.txtConsole.Location = new System.Drawing.Point(0, 971);
            this.txtConsole.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(1613, 460);
            this.txtConsole.TabIndex = 3;
            this.txtConsole.TextChanged += new System.EventHandler(this.txtConsole_TextChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Yellow;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Location = new System.Drawing.Point(2, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1604, 963);
            this.panel3.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkGray;
            this.panel2.Controls.Add(this.btnTopology);
            this.panel2.Controls.Add(this.btnExe);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.cmbMode);
            this.panel2.Controls.Add(this.cbxTop);
            this.panel2.Controls.Add(this.btnStop);
            this.panel2.Controls.Add(this.btnStart);
            this.panel2.Controls.Add(this.btnDeactivate);
            this.panel2.Controls.Add(this.btnActivate);
            this.panel2.Controls.Add(this.btnInstGet);
            this.panel2.Controls.Add(this.btnInstSet);
            this.panel2.Location = new System.Drawing.Point(1295, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(309, 960);
            this.panel2.TabIndex = 4;
            // 
            // btnTopology
            // 
            this.btnTopology.Location = new System.Drawing.Point(62, 318);
            this.btnTopology.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTopology.Name = "btnTopology";
            this.btnTopology.Size = new System.Drawing.Size(200, 45);
            this.btnTopology.TabIndex = 9;
            this.btnTopology.Text = "Topology Set";
            this.btnTopology.UseVisualStyleBackColor = true;
            this.btnTopology.Click += new System.EventHandler(this.btnTopology_Click);
            // 
            // btnExe
            // 
            this.btnExe.Location = new System.Drawing.Point(62, 702);
            this.btnExe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExe.Name = "btnExe";
            this.btnExe.Size = new System.Drawing.Size(200, 45);
            this.btnExe.TabIndex = 8;
            this.btnExe.Text = "Run FFPlay";
            this.btnExe.UseVisualStyleBackColor = true;
            this.btnExe.Click += new System.EventHandler(this.btnExe_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(62, 627);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(200, 45);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cmbMode
            // 
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.FormattingEnabled = true;
            this.cmbMode.Items.AddRange(new object[] {
            "Analog Guage",
            "Object Detection",
            "Helmet Detection"});
            this.cmbMode.Location = new System.Drawing.Point(62, 109);
            this.cmbMode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(200, 40);
            this.cmbMode.TabIndex = 1;
            this.cmbMode.SelectedIndexChanged += new System.EventHandler(this.cmbMode_SelectedIndexChanged);
            // 
            // cbxTop
            // 
            this.cbxTop.AutoSize = true;
            this.cbxTop.Location = new System.Drawing.Point(62, 27);
            this.cbxTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbxTop.Name = "cbxTop";
            this.cbxTop.Size = new System.Drawing.Size(179, 36);
            this.cbxTop.TabIndex = 0;
            this.cbxTop.Text = "Top Window";
            this.cbxTop.UseVisualStyleBackColor = true;
            this.cbxTop.CheckedChanged += new System.EventHandler(this.cbxTop_CheckedChanged);
            // 
            // btnStop
            // 
            this.btnStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnStop.Location = new System.Drawing.Point(62, 239);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(200, 45);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop Monitor";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.ForeColor = System.Drawing.Color.Blue;
            this.btnStart.Location = new System.Drawing.Point(62, 186);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(200, 45);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start Monitor";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnDeactivate
            // 
            this.btnDeactivate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDeactivate.Location = new System.Drawing.Point(62, 551);
            this.btnDeactivate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDeactivate.Name = "btnDeactivate";
            this.btnDeactivate.Size = new System.Drawing.Size(200, 45);
            this.btnDeactivate.TabIndex = 7;
            this.btnDeactivate.Text = "Deactivate";
            this.btnDeactivate.UseVisualStyleBackColor = true;
            this.btnDeactivate.Click += new System.EventHandler(this.btnDeactivate_Click);
            // 
            // btnActivate
            // 
            this.btnActivate.ForeColor = System.Drawing.Color.Blue;
            this.btnActivate.Location = new System.Drawing.Point(62, 498);
            this.btnActivate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(200, 45);
            this.btnActivate.TabIndex = 6;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnInstGet
            // 
            this.btnInstGet.Location = new System.Drawing.Point(62, 424);
            this.btnInstGet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInstGet.Name = "btnInstGet";
            this.btnInstGet.Size = new System.Drawing.Size(200, 45);
            this.btnInstGet.TabIndex = 5;
            this.btnInstGet.Text = "Instance Get";
            this.btnInstGet.UseVisualStyleBackColor = true;
            this.btnInstGet.Click += new System.EventHandler(this.btnInstGet_Click);
            // 
            // btnInstSet
            // 
            this.btnInstSet.Location = new System.Drawing.Point(62, 371);
            this.btnInstSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInstSet.Name = "btnInstSet";
            this.btnInstSet.Size = new System.Drawing.Size(200, 45);
            this.btnInstSet.TabIndex = 4;
            this.btnInstSet.Text = "Instance Set";
            this.btnInstSet.UseVisualStyleBackColor = true;
            this.btnInstSet.Click += new System.EventHandler(this.btnInstSet_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.ForeColor = System.Drawing.Color.Blue;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 960);
            this.panel1.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Yellow;
            this.ClientSize = new System.Drawing.Size(1613, 1431);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "LVA Monitor";
            this.TransparencyKey = System.Drawing.Color.Yellow;
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnTopology;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox cmbMode;
        private System.Windows.Forms.CheckBox cbxTop;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnDeactivate;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Button btnInstGet;
        private System.Windows.Forms.Button btnInstSet;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExe;
    }
}

