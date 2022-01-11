using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Study_FH
{
    public partial class Form1 : Form
    {
        public Button[] ListBtns = new Button[9];

        public Form1()
        {
            InitializeComponent();
            ListBtns[0] = BtnCleanPic;
            ListBtns[1] = Btn2pts;
            ListBtns[2] = BtnAction;
            ListBtns[3] = BtnWrite;
            ListBtns[4] = BtnWriteJSON;
            ListBtns[5] = BtnJsonToMy;
            ListBtns[6] = BtnRWJson;
            ListBtns[7] = BtnWriteModel;
            ListBtns[8] = btnRInstance;
        }

        private void Blockbuttons()
        {
            foreach (Button btn in ListBtns)
            {
                btn.BackColor = Color.Gray;
            }
        }

        private void Allowbuttons()
        {
            foreach (Button btn in ListBtns)
            {
                btn.BackColor = Button.DefaultBackColor;
            }
            /*BtnCleanPic.BackColor = Button.DefaultBackColor;
            Btn2pts.BackColor = Button.DefaultBackColor;
            BtnAction.BackColor = Button.DefaultBackColor;
            BtnWrite.BackColor = Button.DefaultBackColor;
            BtnWriteJSON.BackColor = Button.DefaultBackColor;
            BtnJsonToMy.BackColor = Button.DefaultBackColor;
            BtnRWJson.BackColor = Button.DefaultBackColor;
            BtnWriteModel.BackColor = Button.DefaultBackColor;*/
        }
        private void Btn2pts_Click(object sender, EventArgs e)
        {
            Blockbuttons();
            int NumPts = int.Parse(txtNumPts.Text);
            picTela.Image = null;
            picTela.Image = ToolBox.Draw2points(NumPts);
            Allowbuttons();
        }

        private CARP InitializeInstance(string name, string ext = "dat", string type = "gdb")
        {
            string InstanceAddress = ToolBox.Path(string.Format("CARP_Instances\\gdb\\{0}.{1}", name, ext));
            if (ext == "json")
            {
                type = "json";
            }
            return BeginInstance(InstanceAddress, type);
        }
        private ModelRotaFH InitializeInstanceRotaFH(string name="", string ext = "csv", string type = "csv")
        {
            string InstanceAddress = ToolBox.Path(string.Format("CARP_Instances\\gdb\\{0}.{1}", name, ext));
            if (ext == "json")
            {
                type = "json";
            }
            return BeginInstanceRotaFH();
        }

        private CARP BeginInstance(string Address, string type)
        {
            MessageBox.Show(Address, "Read from");
            CARP P = new CARP();
            GDBInstance Inst = new GDBInstance(Address, type);
            P.Instance = Inst;
            return P;
        }

        private ModelRotaFH BeginInstanceRotaFH(string Address=@"D:\OneDrive - ufpr.br\Visual Studio 2019\ForestInstances\Kitt4\", string type="csv")
        {
            MessageBox.Show(Address, "Read from");
            ModelRotaFH P = new ModelRotaFH();
            //R_Instance Inst = new R_Instance();
            //P.RotaInstance = Inst;
            return P;
        }

        private string WriteDat(CARP P, string filename)
        {
            string namefile = string.Format("{0}{1}_lido.dat", P.Pasta, filename);
            P.Instance.WriteInstance(namefile);
            return namefile;
        }

        private void BtnAction_Click(object sender, EventArgs e)
        {
            Blockbuttons();
            CARP Problema = InitializeInstance(txtNumPts.Text);
            picTela.Image = null;
            picTela.Image = Problema.Instance.DrawInstance();
            Allowbuttons();
        }

        private void BtnWrite_Click(object sender, EventArgs e)
        {
            Blockbuttons();
            CARP Problema = InitializeInstance(txtNumPts.Text);
            MessageBox.Show(WriteDat(Problema, txtNumPts.Text), "Read/Write", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Allowbuttons();
        }

        private void BtnCleanPic_Click(object sender, EventArgs e)
        {
            Blockbuttons();
            picTela.Image = null;
            Allowbuttons();
        }

        private void BtnWriteJSON_Click(object sender, EventArgs e)
        {
            Blockbuttons();
            CARP Problema = InitializeInstance(txtNumPts.Text);
            MessageBox.Show(ToolBox.ObjToJson(Problema, Problema.Pasta, txtNumPts.Text), "JSON", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Allowbuttons();
        }

        private void BtnRWJson_Click(object sender, EventArgs e)
        {
            Blockbuttons();
            CARP Problema = InitializeInstance(txtNumPts.Text, "json");
            MessageBox.Show(ToolBox.ObjToJson(Problema, Problema.Pasta, txtNumPts.Text), "RW - JSON", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Allowbuttons();
        }

        private void BtnJsonToMy_Click(object sender, EventArgs e)
        {
            Blockbuttons();
            CARP Problema = InitializeInstance(txtNumPts.Text, "json");
            string mess = WriteDat(Problema, txtNumPts.Text + "_json2dat");
            MessageBox.Show(mess, "Form Json to .dat", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Allowbuttons();
        }

        private void BtnWriteModel_Click(object sender, EventArgs e)
        {
            Blockbuttons();
            CARP Problema = InitializeInstance(txtNumPts.Text);
            Problema.SolveModel(txtNumPts.Text, true);
            MessageBox.Show("Resolvido", "Ler dat Resolver Modelo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Allowbuttons();

        }

        private void BtnForm2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void BtnForm3_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
        }

        private void BtnForm4_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.ShowDialog();
        }

        private void BtnForm5_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.ShowDialog();
        }

        private void BtnForm6_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.ShowDialog();
        }

        private void BtnForm7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ver");
        }

        private void BtnRInstance_Click(object sender, EventArgs e)
        {
            Blockbuttons();
            ModelRotaFH Problema = InitializeInstanceRotaFH(txtNumPts.Text);
            Problema.CreateModelHyb(txtNumPts.Text, 20, SlackExcessVars:false, log: false);
            MessageBox.Show("Resolvido", "R_Instance", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            
            Allowbuttons();
        }
    }
}
