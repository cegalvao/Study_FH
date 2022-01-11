using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Study_FH
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void BtnDraw_Click(object sender, EventArgs e)
        {
            int NumPts = Convert.ToInt32(nUpD_FMUs.Value);
            int Seed = Convert.ToInt32(nUpD_Seed.Value);
            picTela.Image = null;
            picTela.Image = ToolBox.Draw2points(NumPts, rBtn_Random.Checked, Seed);
        }

        private void BtnCleanPic_Click(object sender, EventArgs e)
        {
            picTela.Image = null;
            picTela2.Image = null;
        }

        private void RadioButtons(RadioButton ThisRBtn, RadioButton AltRBtn)
        {
            if (ThisRBtn.Checked)
            {
                AltRBtn.Checked = false;
            }
            else
            {
                AltRBtn.Checked = true;
            }

        }

        private void RBtn_PolRegular_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtons(rBtn_PolRegular, rBtn_Random);
        }

        private void RBtn_Random_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtons(rBtn_Random, rBtn_PolRegular);
        }

        private void BtnTSP_Click(object sender, EventArgs e)
        {
            StringBuilder lbl = new StringBuilder();
            List<(double, double)> pts = new List<(double, double)>();
            List<(double, double)> sol = new List<(double, double)>();
            List<int> ind = new List<int>();
            int NumPts = Convert.ToInt32(nUpD_FMUs.Value);
            int Seed = Convert.ToInt32(nUpD_Seed.Value);
            if (rBtn_Random.Checked)
            {
                pts = ToolBox.CriarListaDePontos(NumPts, 10, 490, 10, 490, Seed);
            }
            else
            {
                (pts, _) = ToolBox.CriarPontosPoligonoRegular(NumPts);
            }
            sol = Tsp_cs_2d.DoTSP(pts); //Solve TSP
            foreach ((double, double) point in sol)
            {
                ind.Add(pts.IndexOf(point));
                lbl.Append(pts.IndexOf(point));
                lbl.Append('\t');
                lbl.Append(point.Item1);
                lbl.Append('\t');
                lbl.AppendLine(point.Item2.ToString());
            }
            picTela.Image = null;
            picTela2.Image = null;
            //picTela.Image = ToolBox.Draw2points(NumPts, rBtn_Random.Checked, Seed);
            txt_Pts.Text = lbl.ToString();
            picTela.Image = ToolBox.DrawListPoints(pts, ind, 500, 500, 0, 0, 100);
            picTela2.Image = ToolBox.DrawListPoints(sol, ind, 500, 500, 0, 0, 100);
        }
    }
}
