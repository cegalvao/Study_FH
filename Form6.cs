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

namespace Study_FH
{
    public partial class Form6 : Form
    {
        Bitmap MyImage;
        string path = ToolBox.Path("ForestInstances\\cenarios\\sols\\");
        bool listdrawed = false;
        string AtualDados = "";
        string AnteriorDados = "";
        List<(string, string, string, int)> ErrosCores = new List<(string, string, string, int)>();
        List<(string, int)> cores = new List<(string, int)> {("Verde Escuro", -15842120),
                                                                 ("Verde Claro", -6917980),
                                                                 ("Cinza", -2816170),
                                                                 ("Vermelho", -1304650),
                                                                 ("Amarelo", -572580),
                                                                 ("Laranja", -382420)};

        public Form6()
        {
            InitializeComponent();
            cmbCorrecaoCor.Items.Clear();
            foreach ((string, int) cor in cores)
            {
                cmbCorrecaoCor.Items.Add(cor.Item1);
            }
        }

        private void btnDados_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Dados_Click");
            getdata();
            //nupdTxtCoordX
            //nupdTxtCoordY
            //TxtInd.Text
        }

        private (string, string) GetModelDV(ComboBox cmB)
        {
            Console.WriteLine("GetModelDV");
            string model = "";
            string dv = "";
            if (cmB.Text.Length > 0)
            {
                if (cmB.Text[34] == '_')
                {
                    model = cmB.Text.Substring(0, 34);
                    dv = cmB.Text.Substring(35, cmB.Text.Length - 39);
                }
                else
                {
                    model = cmB.Text.Substring(0, 33);
                    dv = cmB.Text.Substring(34, cmB.Text.Length - 38);
                }
            }
            return (model, dv);
        }

        private string GetCSVName(ComboBox cmB)
        {
            Console.WriteLine("GetCSVName");
            return $"Dados_{GetModelDV(cmB).Item1}.csv";

        }

        private string GetRGBName(ComboBox cmB)
        {
            Console.WriteLine("GetRGBName");
            (string, string) resp = GetModelDV(cmB);
            return $"RGB_{resp.Item1}_{resp.Item2}.csv";

        }
        private string GetColorErrorName(string cmB)
        {
            Console.WriteLine("GetColorErrorName");
            return $"ColorError_{cmB}.csv".Replace(".png.csv", ".csv");
        }

        private void getdata()
        {
            Console.WriteLine("GetData");
            string model;
            string dv;
            //string[] dvs = new string[] { "ORG", "DV1", "DV2", "DV3", "DV4", "DV12", "DV13", "DV14", "DV24", "DV34", "DV124", "DV134" };

            picF6.BackgroundImage = null;
            picF6.Image = null;

            if (cmBArquivo.Text != "")
            {
                (model, dv) = GetModelDV(cmBArquivo);

                Console.WriteLine($"Model {model} - DV = {dv}");
                MyImage = new Bitmap(Bitmap.FromFile(path + cmBArquivo.Text));
                picF6.BackgroundImage = MyImage;
            }
        }


        private double Flinear(double x, double a, double b)
        {
            Console.WriteLine("flinear");
            double y = a * x + b;
            //Console.WriteLine($"X - {x.ToString()}, A - {a.ToString()}, B - {b.ToString()}, Y - {y.ToString()}");
            return y;
        }

        private (double, double) Novo2orig(double coordX, double coordY)
        {
            Console.WriteLine("novo->orig");
            double ax = double.MaxValue;
            double ay = double.MaxValue;
            if (nupdXnovo1.Value != nupdXnovo2.Value)
            {
                ax = Convert.ToDouble((nupdXorig1.Value - nupdXorig2.Value) / (nupdXnovo1.Value - nupdXnovo2.Value));
            }
            if (nupdYnovo1.Value != nupdYnovo2.Value)
            {
                ay = Convert.ToDouble((nupdYorig1.Value - nupdYorig2.Value) / (nupdYnovo1.Value - nupdYnovo2.Value));
            }

            double bx = Convert.ToDouble(nupdXorig1.Value) - ax * Convert.ToDouble(nupdXnovo1.Value);
            double by = Convert.ToDouble(nupdYorig1.Value) - ay * Convert.ToDouble(nupdYnovo1.Value);
            return (Flinear(coordX, ax, bx), Flinear(coordY, ay, by));
        }

        private (double, double) Orig2novo(double coordX, double coordY)
        {
            Console.WriteLine("orig->novo");
            double ax = double.MaxValue;
            double ay = double.MaxValue;
            if (nupdXorig1.Value != nupdXorig2.Value)
            {
                ax = Convert.ToDouble((nupdXnovo1.Value - nupdXnovo2.Value) / (nupdXorig1.Value - nupdXorig2.Value));
            }
            if (nupdYorig1.Value != nupdYorig2.Value)
            {
                ay = Convert.ToDouble((nupdYnovo1.Value - nupdYnovo2.Value) / (nupdYorig1.Value - nupdYorig2.Value));
            }
            double bx = Convert.ToDouble(nupdXorig1.Value) - ax * Convert.ToDouble(nupdXnovo1.Value);
            double by = Convert.ToDouble(nupdYorig1.Value) - ay * Convert.ToDouble(nupdYnovo1.Value);
            return (Flinear(coordX, ax, bx), Flinear(coordY, ay, by));
        }

        private string fromDataToString(int ind, double X, double Y)
        {
            return $"[{ind}]({X},{Y})";
        }
        private string fromDataToString(string ind, string X, string Y)
        {
            return $"[{ind}]({X},{Y})";
        }

        private (int, double, double) fromStringToData(string val)
        {
            string[] splitval = val.Split(new char[] { '[', ']', '(', ',', ')' });
            string[] resp = new string[3];
            int i = 0;
            foreach (string s in splitval)
            {
                if (s != "")
                {
                    resp[i] = s;
                    i++;
                }
            }

            return (int.Parse(resp[0]), Convert.ToDouble(resp[1]), Convert.ToDouble(resp[2]));
        }

        private string[] coordsfromcsv(string DatFile)
        {
            Console.WriteLine("CoordsFromCSV");
            //List<(int, double, double)> vals = new List<(int, double, double)>();
            string[] AllDatFile = File.ReadAllLines(path + DatFile);
            string[] vals = new string[AllDatFile.Length - 5];
            for (int cline = 5; cline < AllDatFile.Length; cline++)
            {
                string[] Line = AllDatFile[cline].Split(';');
                vals[cline - 5] = fromDataToString(Line[0], Line[1], Line[2]);//  $"[{}]({},{})";
            }
            return vals;
        }

        private void DrawCoords(List<(double, double)> pts, List<int> ind, ComboBox Dados, ComboBox Ponto)
        {
            Console.WriteLine("Draw_Click");
            lbl_CoordNovo.Text = "";
            for (int i = 0; i < pts.Count; i++)
            {
                int coordx = Convert.ToInt32(pts[i].Item1);
                int coordy = Convert.ToInt32(pts[i].Item2);

                if (coordx >= 0 && coordy >= 0)
                {
                    // Get the color of a pixel within myBitmap.
                    Color pixelColor = MyImage.GetPixel(coordx, coordy);

                    lbl_CoordNovo.Text += $"[{ind[i]}] Novo: \n X - {pts[i].Item1} " +
                                         $"\t Y - {pts[i].Item2} " +
                                         $"\n {ColorToString(pixelColor, Dados, Ponto, false)}";
                }
                else
                {
                    lbl_CoordNovo.Text += $"[{ind[i]}] - Coord negativas";
                }
            }
            picF6.Image = ToolBox.DrawListPoints(pts, ind, picF6.Width, picF6.Height, PointRadius: int.Parse(nupdRaioCric.Value.ToString()), transparent: true);

        }

        private void BtnDrawCoords_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnDrawCoords_Click");
            listdrawed = false;
            NupdCoord_Changed();
        }

        private void CmBArquivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("ArquivoSelected");
            AnteriorDados = $"{AtualDados}";
            AtualDados = cmBArquivo.Text;
            string nome = $"{path}{GetColorErrorName(AtualDados)}";

            if (ErrosCores.Count > 0)
            {
                SaveFileColorErrors(AnteriorDados);
                ErrosCores.Clear();
            }

            try
            {
                string[] ErrosCoresLidos = File.ReadAllLines(nome);
                Console.WriteLine($"Aberto {nome}");
                foreach (string erroline in ErrosCoresLidos)
                {
                    string[] errolido = erroline.Split('\t');
                    string[] resp = new string[3];
                    int i = 0;
                    foreach (string s in errolido)
                    {
                        if (s != "")
                        {
                            resp[i] = s;
                            i++;
                        }
                    }
                    ErrosCores.Add((AtualDados, resp[0], resp[1], int.Parse(resp[2])));
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Não achado o arquivo {nome}");
            }

            cmBCoords.Items.Clear();

            foreach (string cmbitem in coordsfromcsv(GetCSVName(cmBArquivo)))
            {
                cmBCoords.Items.Add(cmbitem);
            }

        }

        private void CmBCoords_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("CoordsSelected");
            (int, double, double) datavals = fromStringToData(cmBCoords.SelectedItem.ToString());
            TxtInd.Text = datavals.Item1.ToString();
            nupdTxtCoordX.Value = Convert.ToDecimal(datavals.Item2);
            nupdTxtCoordY.Value = Convert.ToDecimal(datavals.Item3);

        }

        private (List<int>, List<(double, double)>) GetPts(bool normalized = true)
        {
            Console.WriteLine("GetPts");
            List<(double, double)> pts = new List<(double, double)> { };
            List<int> ind = new List<int> { };
            if (cmBArquivo.Text != "")
            {
                foreach (string cmbitem in coordsfromcsv(GetCSVName(cmBArquivo)))
                {
                    (int, double, double) vals = fromStringToData(cmbitem);
                    pts.Add(Orig2novo(vals.Item2, vals.Item3));
                    ind.Add(vals.Item1);
                }
            }
            return (ind, pts);
        }

        private void NupdCoord_Changed()
        {
            List<(double, double)> pts = new List<(double, double)> { };
            List<int> ind = new List<int> { };

            if (listdrawed)
            {
                (ind, pts) = GetPts();
                picF6.Image = ToolBox.DrawListPoints(pts, ind, picF6.Width, picF6.Height, drawlines: false, PointRadius: int.Parse(nupdRaioCric.Value.ToString()), transparent: true); ;
                lbl_CoordNovo.Text = "Desenhado do Arquivo CSV";
            }
            else
            {
                pts.Add(Orig2novo(Convert.ToDouble(nupdTxtCoordX.Value), Convert.ToDouble(nupdTxtCoordY.Value)));
                if (TxtInd.Text == "")
                {
                    ind.Add(-1);

                }
                else
                {
                    ind.Add(int.Parse(TxtInd.Text));
                }
                getdata();
                DrawCoords(pts, ind, cmBArquivo, cmBCoords);
            }

        }

        private void NupdTxtCoordX_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void nupdTxtCoordY_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void btnFileCoords_Click(object sender, EventArgs e)
        {
            listdrawed = true;
            NupdCoord_Changed();
        }

        private void nupdXorig1_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void nupdYorig1_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void nupdXorig2_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void nupdYorig2_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void nupdXnovo1_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void nupdYnovo1_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void nupdXnovo2_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void nupdYnovo2_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private void nupdRaioCric_ValueChanged(object sender, EventArgs e)
        {
            NupdCoord_Changed();
        }

        private Color GetAverageColor(int coordx, int coordy)
        {
            Console.WriteLine("GetAverageColor");
            double average = 0;
            int k = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (coordx - 1 - i < 0 || coordy - 1 - j < 0)
                    {
                        average += 0;
                    }
                    else
                    {
                        average += Convert.ToDouble(MyImage.GetPixel(coordx - 1 - i, coordy - 1 - j).ToArgb());
                        k++;
                    }
                }
            }
            Color pixelColor = Color.White;

            if (k != 0)
            {
                pixelColor = Color.FromArgb(Convert.ToInt32(average / k));
            }

            return pixelColor;
        }

        private string GetColorName(ComboBox Dados, ComboBox Ponto, int average)
        {
            Console.WriteLine("GetColorName");
            (bool, string) test = VerErroCor(Dados, Ponto);
            string resp = ColorNameFromAverage(average);
            if (test.Item1)
            {
                Console.WriteLine($"Resp - Item 2: {test.Item2}");
                resp = test.Item2;
            }
            else
            {
                Console.WriteLine($"Resp - Average: {average}");
            }
            return resp;
        }

        private string ColorNameFromAverage(int average)
        {
            int mincalc = int.MaxValue;
            string resp = "Não encontrado";
            foreach ((string, int) cor in cores)
            {
                int compare = Math.Abs(average - cor.Item2);
                if (compare < mincalc)
                {
                    mincalc = compare;
                    resp = cor.Item1;
                }
            }
            return resp;
        }

        private string ColorToString(Color C, ComboBox Dados, ComboBox Ponto, bool tabs = true)
        {
            Console.WriteLine("ColorToString");
            string resp = $"\n {C} \n\t ARGB: {C.ToArgb()} " +
                          $"\n\t KnowColor: {C.IsKnownColor} " +
                          $"\n\t Name: {GetColorName(Dados, Ponto, C.ToArgb())}";
            if (tabs)
            {
                resp = $"{C.R}\t{C.G}\t{C.B}" +
                       $"\t{C.ToArgb()}" +
                       $"\t{GetColorName(Dados, Ponto, C.ToArgb())}";
            }
            Console.WriteLine(resp);
            return resp;
        }

        private void btnRGB2csv_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnRGB->csv");
            if (listdrawed || cmBArquivo.Text != "")
            {
                List<(double, double)> pts;
                List<int> ind;

                (ind, pts) = GetPts();
                string newfile = $"{path}{GetRGBName(cmBArquivo)}";

                StringBuilder rgbs = new StringBuilder();
                rgbs.AppendLine("Indice\tX\tY\tR\tG\tB\tArgb\tNome");
                for (int i = 0; i < pts.Count; i++)
                {
                    int coordx = Convert.ToInt32(pts[i].Item1);
                    int coordy = Convert.ToInt32(pts[i].Item2);
                    string resp = $"{ind[i]}\t{pts[i].Item1}\t{pts[i].Item2}\t \t \t \t \t Coord negativas";

                    if (coordx >= 0 && coordy >= 0)
                    {
                        // Get the color of a pixel within myBitmap.
                        Color pixelColor = GetAverageColor(coordx, coordy);
                        resp = $"{ind[i]}\t{pts[i].Item1}\t{pts[i].Item2}\t{ColorToString(pixelColor, cmBArquivo, cmBCoords)}";
                        Console.WriteLine(resp);
                    }
                    Console.WriteLine(resp);
                    rgbs.AppendLine(resp);
                }

                using (StreamWriter writer = new StreamWriter(newfile))
                {
                    writer.Write(rgbs.ToString());
                }
                MessageBox.Show($"{newfile} gravado");
                Console.WriteLine($"{newfile} gravado");
            }
        }

        private int GetColorArgbFromList(string name)
        {
            Console.WriteLine("GetColorArgb");
            int resp = 0;
            foreach ((string, int) cor in cores)
            {
                if (name == cor.Item1)
                {
                    resp = cor.Item2;
                }
            }
            return resp;
        }

        private void InsertErroCor(ComboBox Dados, ComboBox Ponto, ComboBox CorCorreta)
        {
            Console.WriteLine("InsertErroCor");
            int ArgBCorCorreta = GetColorArgbFromList(CorCorreta.Text);
            if (!(ErrosCores.Contains((Dados.Text, Ponto.Text, CorCorreta.Text, ArgBCorCorreta))))
            {
                ErrosCores.Add((Dados.Text, Ponto.Text, CorCorreta.Text, ArgBCorCorreta));
            }
        }

        private (bool, string) VerErroCor(ComboBox Dados, ComboBox Ponto)
        {
            Console.WriteLine("VerErroCor");
            int k = 0;
            string resp = "";
            foreach ((string, int) cor in cores)
            {
                if (ErrosCores.Contains((Dados.Text, Ponto.Text, cor.Item1, cor.Item2)))
                {
                    resp = cor.Item1;
                    k++;
                }
            }
            return (k > 0, resp);
        }

        private void BtnCorrectCor_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnCorrectCor");
            if (cmbCorrecaoCor.Text != "")
            {
                InsertErroCor(cmBArquivo, cmBCoords, cmbCorrecaoCor);
            }
        }

        private void SaveFileColorErrors(string Dados)
        {
            Console.WriteLine("SaveFileColorErrors");
            string filename = $"{path}{GetColorErrorName(Dados)}";
            StringBuilder ColorErrors = new StringBuilder();

            foreach (var erro in ErrosCores)
            {
                if (erro.Item1 == Dados)
                {
                    ColorErrors.AppendLine($"{erro.Item2}\t{erro.Item3}\t{erro.Item4}");
                }
            }

            using StreamWriter writer = new StreamWriter(filename);
            writer.Write(ColorErrors.ToString());
        }

        private void BtnSaveErrors_Click(object sender, EventArgs e)
        {
            SaveFileColorErrors(cmBArquivo.Text);
        }
    }
}
