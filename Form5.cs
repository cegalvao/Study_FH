using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Study_FH
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            ClearPicB01();
        }

        private void ClearPicB01()
        {
            picB01.Image = new Bitmap(picB01.Width, picB01.Height);
        }

        private void cBoxCenarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearPicB01();
            if (cBoxCenarios.SelectedIndex > -1)
            {
                string path = "cenarios";
                switch (cBoxCenarios.Text)
                {
                    case "Kitt2":
                    case "Kittaning0":
                    case "Kittaning4":
                        path = "Kittaning4";
                        break;
                }

                string file = ToolBox.Path("ForestInstances\\" + path + '\\' + "Dados_" + cBoxCenarios.Text + ".csv");
                string ptsfile = ToolBox.Path("ForestInstances\\cenarios\\sols\\PtsNormalizados_" + cBoxCenarios.Text + ".csv");
                string ImageFile = ToolBox.Path("ForestInstances\\" + path + '\\' + cBoxCenarios.Text + ".png");

                List<(double, double)> points = new List<(double, double)>();
                List<int> ind = new List<int>();

                string[] AllDatFile = File.ReadAllLines(file);
                for (int cline = 0; cline < AllDatFile.Length; cline++)
                {
                    string[] Line = AllDatFile[cline].Split(';');
                    switch (cline)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            break;
                        default:
                            ind.Add(cline - 5);
                            points.Add((ToolBox.ReadDouble(Line[1]), ToolBox.ReadDouble(Line[2])));
                            break;
                    }
                }
                List<(double, double)> Norm_points = ToolBox.NormalizePoints(points, (picB01.Width - 25, picB01.Height - 25), (25, 25));
                using (StreamWriter writer2 = new StreamWriter(ptsfile))
                {
                    for (int i = 0; i < Norm_points.Count; i++)
                    {
                        string name = $"{i};{Norm_points[i].Item1};{Norm_points[i].Item2}";
                        writer2.WriteLine(name);
                        Console.WriteLine(name);
                    }
                }
                picB01.Image = ToolBox.DrawListPoints(Norm_points, ind, picB01.Width, picB01.Height, PointRadius: 2, drawlines: false, fontsize: 12);
                /*picB01.Image = ToolBox.DrawSquareGuide(ToolBox.GetMinMaxCoordPoints(Norm_points),
                                                       ToolBox.GetMinMaxCoordPoints(Norm_points, false),
                                                       picB01.Width - 25, picB01.Height - 25);*/
                picB01.Image.Save(ImageFile, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

    }
}
