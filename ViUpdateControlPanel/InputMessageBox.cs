using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SngMediaApp.Utils
{
    public partial class InputMessageBox : Form
    {
        private string _rezText;
        private InputMessageBox()
        {
            InitializeComponent();
        }

        private static readonly InputMessageBox Imb = new InputMessageBox();

        // Microsoft.VisualBasic.Interaction.InputBox("Новое имя:", "Переименовать", treeView1.SelectedNode.Text)

        public static string Show(string message,string caption,string defText)
        {
            Imb._rezText = null;
            Imb.label1.Text = message;
            Imb.Text = caption;
            Imb.textBox1.Text = defText;

            Imb.ShowDialog();
            return Imb._rezText;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            _rezText = textBox1.Text;
            Close();
        }

        private void btCan_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
