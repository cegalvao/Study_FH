using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Study_FH
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            var filePath = string.Empty;

            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = ToolBox.Path("ForestInstances\\");
            openFileDialog.Filter = "MPS files (*.mps)|*.mps|LP files (*.lp)|*.lp|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
            }
            TxtFileNameMPS.Text = filePath;
        }

        private void OpenFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            var filePath = string.Empty;

            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = ToolBox.Path("ForestInstances\\");
            openFileDialog.Filter = "MST files (*.mst)|*.mst|txt files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
            }
            TxtFileNameMST.Text = filePath;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Study_FH.Form2.ShowMess = true;
        }

        private void btnChoiceFileMPS_Click(object sender, EventArgs e)
        {
            openFileDialog1_FileOk(sender, new CancelEventArgs());
        }

        private void btnChoiceFileMST_Click(object sender, EventArgs e)
        {
            OpenFileDialog2_FileOk(sender, new CancelEventArgs());
        }

        private void ChangeActiveTextBox(TextBox txtbx, bool active)
        {
            if (active)
            {
                txtbx.BackColor = System.Drawing.Color.White;
                txtbx.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                txtbx.BackColor = System.Drawing.Color.Silver;
                txtbx.ForeColor = System.Drawing.Color.DimGray;
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            //Form3.ActiveForm.BackColor = SystemColors.InactiveCaption;
            List<TextBox> txtbxs = new List<TextBox> { TxtFileNameMPS, TxtFileNameMST };
            foreach (TextBox txtbx in txtbxs)
            {
                ChangeActiveTextBox(txtbx, false);
            }

            if (TxtFileNameMPS.TextLength < 1)
            {
                btnChoiceFileMPS_Click(sender, e);
            }
            this.Text = "Calculando " + TxtFileNameMPS.Text.Split('\\').Last();

            CARP_FH Problema = new CARP_FH();
            Problema.SolveFromFile(chkShowMess.Checked, TxtFileNameMPS.Text);

            //Form3.ActiveForm.BackColor = SystemColors.Window;
            foreach (TextBox txtbx in txtbxs)
            {
                ChangeActiveTextBox(txtbx, true);
            }

            MessageBox.Show(Problema.ToString() + " feito!");
            Problema.Dispose();
        }

        private void FolderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                _ = fbd.SelectedPath;
            }

        }

        private void btnChoseFolder_Click(object sender, EventArgs e)
        {
            {
                using var dialog = new FolderBrowserDialog();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string[] files = Directory.GetFiles(dialog.SelectedPath);
                    listBox_Files.Items.Clear();
                    foreach (string it in files)
                    {
                        if (it.Contains(".mps"))
                        {
                            listBox_Files.Items.Add(it);
                        }
                    }
                }
            }
        }

        private void btnSolveSelecteds_Click(object sender, EventArgs e)
        {
            MessageBox.Show(listBox_Files.SelectedItems.Count.ToString() + " itens");
            foreach (var i in listBox_Files.SelectedItems)
            {
                MessageBox.Show(i.ToString());
                TxtFileNameMPS.Text = i.ToString();
                btnSolve_Click(sender, e);
            }
            MessageBox.Show("Todos os Selecionados foram executados!", "Solve Selected", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void btnSubstring_Click(object sender, EventArgs e)
        {
            ToolBox.Substring_lps(TxtFileNameMPS.Text, TxtFileNameMST.Text, chkShowMess.Checked);
        }
    }
}
