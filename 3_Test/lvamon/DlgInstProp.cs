using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lvamon
{
    public partial class frmInstProp : Form
    {
        public List<LvaScenario> scenarios;

        public frmInstProp()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmInstProp_Load(object sender, EventArgs e)
        {
            foreach (var s in scenarios)
            {
                cmbScenario.Items.Add(s.name);
            }
        }
        private void cmbScenario_SelectedValueChanged(object sender, EventArgs e)
        {
            int idx = cmbScenario.SelectedIndex;

            txtRTSPsource.Text = scenarios[idx].rtspSource;
            txtAIUrl.Text = scenarios[idx].aiUri;
            cmbScale.SelectedIndex = (scenarios[idx].scaleMode == "pad") ? 1 : 0; // only 2 mode
            txtWidth.Text = scenarios[idx].frameWidth;
            txtHeight.Text = scenarios[idx].frameHeight;
        }
    }
}
