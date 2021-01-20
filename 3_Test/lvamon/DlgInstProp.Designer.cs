
namespace lvamon
{
    partial class frmInstProp
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
            this.txtAIUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbScale = new System.Windows.Forms.ComboBox();
            this.txtRTSPsource = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbScenario = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtAIUrl
            // 
            this.txtAIUrl.AcceptsReturn = true;
            this.txtAIUrl.Location = new System.Drawing.Point(192, 175);
            this.txtAIUrl.Name = "txtAIUrl";
            this.txtAIUrl.Size = new System.Drawing.Size(508, 31);
            this.txtAIUrl.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "RTSP Source:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "AI URL:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(177, 424);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(193, 60);
            this.btnOkay.TabIndex = 0;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(420, 424);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(193, 60);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 31);
            this.label3.TabIndex = 1;
            this.label3.Text = "Scale Mode:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWidth
            // 
            this.txtWidth.AcceptsReturn = true;
            this.txtWidth.Location = new System.Drawing.Point(192, 313);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(185, 31);
            this.txtWidth.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(24, 313);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 31);
            this.label4.TabIndex = 1;
            this.label4.Text = "Width:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtHeight
            // 
            this.txtHeight.AcceptsReturn = true;
            this.txtHeight.Location = new System.Drawing.Point(513, 313);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(187, 31);
            this.txtHeight.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(403, 313);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 31);
            this.label5.TabIndex = 1;
            this.label5.Text = "Height:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbScale
            // 
            this.cmbScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScale.FormattingEnabled = true;
            this.cmbScale.Items.AddRange(new object[] {
            "preserveAspectRatio",
            "pad"});
            this.cmbScale.Location = new System.Drawing.Point(192, 243);
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new System.Drawing.Size(508, 33);
            this.cmbScale.TabIndex = 4;
            // 
            // txtRTSPsource
            // 
            this.txtRTSPsource.Location = new System.Drawing.Point(192, 114);
            this.txtRTSPsource.Name = "txtRTSPsource";
            this.txtRTSPsource.Size = new System.Drawing.Size(508, 31);
            this.txtRTSPsource.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(24, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(162, 31);
            this.label6.TabIndex = 1;
            this.label6.Text = "Scenario:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbScenario
            // 
            this.cmbScenario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScenario.FormattingEnabled = true;
            this.cmbScenario.Location = new System.Drawing.Point(192, 56);
            this.cmbScenario.Name = "cmbScenario";
            this.cmbScenario.Size = new System.Drawing.Size(508, 33);
            this.cmbScenario.TabIndex = 1;
            this.cmbScenario.SelectedValueChanged += new System.EventHandler(this.cmbScenario_SelectedValueChanged);
            // 
            // frmInstProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 529);
            this.Controls.Add(this.cmbScenario);
            this.Controls.Add(this.cmbScale);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.txtAIUrl);
            this.Controls.Add(this.txtRTSPsource);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(826, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(826, 600);
            this.Name = "frmInstProp";
            this.Text = "Instrance Properties";
            this.Load += new System.EventHandler(this.frmInstProp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox txtAIUrl;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox cmbScale;
        public System.Windows.Forms.TextBox txtRTSPsource;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.ComboBox cmbScenario;
    }
}