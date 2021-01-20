using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class DlgTopology : Form
    {
        public string topology = "";

        public DlgTopology()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Open Topology File";
            openFileDialog.Filter = "JSON (*.json)|*.json|All Files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtTopology.Text = File.ReadAllText(openFileDialog.FileName);
            }

        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            topology = txtTopology.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            topology = txtTopology.Text;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
