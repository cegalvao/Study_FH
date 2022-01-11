using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; //Get time execution

namespace Study_FH
{
    public partial class Form2 : Form
    {
        public static bool ShowMess = true;
        bool trescsv;
        public static bool HasInstance = false;
        public static string NameInstance;
        public int sizeChk = 25;
        public List<int> ChkIndex = new List<int>();
        public CheckBox[] PrincipalCheckBoxes = new CheckBox[25];
        public CheckBox[] SECheckBoxes = new CheckBox[25];
        public CheckBox[] RHSCheckBoxes = new CheckBox[25];
        public NumericUpDown[] RHSnUPd = new NumericUpDown[25];
        public bool anot = true;

        public Form2()
        {
            InitializeComponent();
            anot = false;
            BloqDesbloq(Txt_Path, false);
            BloqDesbloq(Txt_Name, false);
            BloqDesbloq(nUpD_PercentMaximalHarvestArea, false);
            BloqDesbloq(nUpD_PercentMinimalHarvestArea, false);
            ShowMess = chkMsgBox.Checked;
            for (int i = 2; i < 21; i++)
            {
                if (i != 16)
                {
                    ChkIndex.Add(i);
                }
            }
            PrincipalCheckBoxes[2] = chkR02;
            PrincipalCheckBoxes[3] = chkR03;
            PrincipalCheckBoxes[4] = chkR04;
            PrincipalCheckBoxes[5] = chkR05;
            PrincipalCheckBoxes[6] = chkR06;
            PrincipalCheckBoxes[7] = chkR07;
            PrincipalCheckBoxes[8] = chkR08;
            PrincipalCheckBoxes[9] = chkR09;
            PrincipalCheckBoxes[10] = chkR10;
            PrincipalCheckBoxes[11] = chkR11;
            PrincipalCheckBoxes[12] = chkR12;
            PrincipalCheckBoxes[13] = chkR13;
            PrincipalCheckBoxes[14] = chkR14;
            PrincipalCheckBoxes[15] = chkR15;
            PrincipalCheckBoxes[16] = chkR16;
            PrincipalCheckBoxes[17] = chkR17;
            PrincipalCheckBoxes[18] = chkR18;
            PrincipalCheckBoxes[19] = chkR19;
            PrincipalCheckBoxes[20] = chkR20;
            SECheckBoxes[2] = chk_SE_R02;
            SECheckBoxes[3] = chk_SE_R03;
            SECheckBoxes[4] = chk_SE_R04;
            SECheckBoxes[5] = chk_SE_R05;
            SECheckBoxes[6] = chk_SE_R06;
            SECheckBoxes[7] = chk_SE_R07;
            SECheckBoxes[8] = chk_SE_R08;
            SECheckBoxes[9] = chk_SE_R09;
            SECheckBoxes[10] = chk_SE_R10;
            SECheckBoxes[11] = chk_SE_R11;
            SECheckBoxes[12] = chk_SE_R12;
            SECheckBoxes[13] = chk_SE_R13;
            SECheckBoxes[14] = chk_SE_R14;
            SECheckBoxes[15] = chk_SE_R15;
            SECheckBoxes[17] = chk_SE_R17;
            SECheckBoxes[18] = chk_SE_R18;
            SECheckBoxes[19] = chk_SE_R19;
            SECheckBoxes[20] = chk_SE_R20;
            RHSnUPd[2] = nUpD_RHS_R02;
            RHSnUPd[3] = nUpD_RHS_R03;
            RHSnUPd[4] = nUpD_RHS_R04;
            RHSnUPd[5] = nUpD_RHS_R05;
            RHSnUPd[6] = nUpD_RHS_R06;
            RHSnUPd[7] = nUpD_RHS_R07;
            RHSnUPd[8] = nUpD_RHS_R08;
            RHSnUPd[9] = nUpD_RHS_R09;
            RHSnUPd[10] = nUpD_RHS_R10;
            RHSnUPd[11] = nUpD_RHS_R11;
            RHSnUPd[12] = nUpD_RHS_R12;
            RHSnUPd[13] = nUpD_RHS_R13;
            RHSnUPd[14] = nUpD_RHS_R14;
            RHSnUPd[15] = nUpD_RHS_R15;
            RHSnUPd[17] = nUpD_RHS_R17;
            RHSnUPd[18] = nUpD_RHS_R18;
            RHSnUPd[19] = nUpD_RHS_R19;
            RHSnUPd[20] = nUpD_RHS_R20;
            RHSCheckBoxes[2] = chk_RHS_R02;
            RHSCheckBoxes[3] = chk_RHS_R03;
            RHSCheckBoxes[4] = chk_RHS_R04;
            RHSCheckBoxes[5] = chk_RHS_R05;
            RHSCheckBoxes[6] = chk_RHS_R06;
            RHSCheckBoxes[7] = chk_RHS_R07;
            RHSCheckBoxes[8] = chk_RHS_R08;
            RHSCheckBoxes[9] = chk_RHS_R09;
            RHSCheckBoxes[10] = chk_RHS_R10;
            RHSCheckBoxes[11] = chk_RHS_R11;
            RHSCheckBoxes[12] = chk_RHS_R12;
            RHSCheckBoxes[13] = chk_RHS_R13;
            RHSCheckBoxes[14] = chk_RHS_R14;
            RHSCheckBoxes[15] = chk_RHS_R15;
            RHSCheckBoxes[17] = chk_RHS_R17;
            RHSCheckBoxes[18] = chk_RHS_R18;
            RHSCheckBoxes[19] = chk_RHS_R19;
            RHSCheckBoxes[20] = chk_RHS_R20;

            Change_AneqB(chkR04, chkR17);
            Change_AneqB(chkR07, chkR20);
            ChooseModel(rBtn_Original.Name);
            foreach (int i in ChkIndex)
            {
                if (i > 0)
                {
                    BloqDesbloq(RHSnUPd[i], RHSCheckBoxes[i].Checked);
                }
            }
            anot = true;
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void Change_AneqB(CheckBox A, CheckBox B)
        {
            if (A.CheckState == B.CheckState)
            {
                B.Checked = !(A.Checked);
            }
        }
        private void BloqDesbloq(NumericUpDown obj, bool enable)
        {
            obj.Enabled = enable;
            obj.ReadOnly = !(enable);
            if (enable)
            {
                obj.BackColor = System.Drawing.Color.White;
                obj.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                obj.BackColor = System.Drawing.Color.Silver;
                obj.ForeColor = System.Drawing.Color.DimGray;
            }
        }
        private void BloqDesbloq(TextBox obj, bool enable)
        {
            obj.Enabled = enable;
            obj.ReadOnly = !(enable);
            if (enable)
            {
                obj.BackColor = System.Drawing.Color.White;
                obj.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                obj.BackColor = System.Drawing.Color.Silver;
                obj.ForeColor = System.Drawing.Color.DimGray;
            }
        }
        private void BtnCalcCARPFH_Click(object sender, EventArgs e)
        {
            CalcCARPFH(true);
        }
        private void CalcCARPFH(bool clicked = false)
        {
            Stopwatch CalcCronometro = new Stopwatch();
            String Msg;
            List<string> logfile = new List<string>
            {
                "log_exe_" + Txt_Name.Text + "_" + Txt_Anot.Text + "_(" + ToolBox.GetNow() + ").log",
                "\n"
            };
            CalcCronometro.Start();
            this.Text = "Calculando " + Txt_Name.Text;
            lbl_Status.Text = "Calculando " + Txt_Name.Text;
            CARP_FH Problema = new CARP_FH
            {
                Instance = new FHInstance(Txt_Name.Text, Txt_Path.Text, Txt_Anot.Text, trescsv, ColetaPercents())
            };
            Msg = Problema.Instance.Name + " criado";
            lbl_Status.Text = Msg;
            if (ShowMess)
            {
                MessageBox.Show(Msg, "Instancia Criada");
            }
            logfile.Add(ToolBox.AddLogLine(("Cronômetro: " + CalcCronometro.Elapsed.ToString(), Msg)));

            bool[] ConstrCheck = new bool[sizeChk];
            bool[] SECheck = new bool[sizeChk];
            double[] RHSnum = new double[sizeChk];
            foreach (int i in ChkIndex)
            {
                if (i > 0)
                {
                    lbl_Status.Text = "Verificando " + PrincipalCheckBoxes[i].Text;
                    List<string> strchks = new List<string>();
                    ConstrCheck[i] = PrincipalCheckBoxes[i].Checked;
                    SECheck[i] = SECheckBoxes[i].Checked;
                    RHSnum[i] = Convert.ToDouble(RHSnUPd[i].Value);
                    strchks.Add("CheckBox: ");
                    strchks.Add(i.ToString());
                    strchks.Add(PrincipalCheckBoxes[i].Text + " - " + PrincipalCheckBoxes[i].CheckState.ToString());
                    strchks.Add(SECheckBoxes[i].Name + " - " + SECheckBoxes[i].CheckState.ToString());
                    strchks.Add(RHSCheckBoxes[i].Name + " - " + RHSCheckBoxes[i].CheckState.ToString());
                    strchks.Add("RHSValue = " + RHSnum[i].ToString());

                    logfile.Add(ToolBox.AddLogLine(strchks));
                    strchks.Clear();
                }
            }
            /*
            DialogResult versao = MessageBox.Show("Usar Versao Antiga?", "Versao", MessageBoxButtons.YesNo);

            bool vers = true;
            if (versao == DialogResult.No)
            {
                vers = false;
            }*/
            lbl_Status.Text = "Criando " + Problema.ToString();
            string lbltext;
            Problema.CreateModel(Txt_Path.Text, ConstrCheck, rBtn_ModeloA.Checked, chkR16.Checked, SECheck, RHSnum, chkMsgBox.Checked);
            List<string> finaldata = new List<string>
            {
                CalcCronometro.Elapsed.ToString(),
                Problema.resp.Item1.ToString(),
                Problema.resp.Item2
            };

            lbltext = ToolBox.AddLogLine(finaldata);
            lbl_Status.Text += lbltext;
            logfile.Add(lbltext);
            finaldata.Clear();

            finaldata.Add(CalcCronometro.Elapsed.ToString());
            finaldata.Add("Valor Objetivo: ");
            finaldata.Add(Problema.ModelObjVal.ToString());

            lbltext = ToolBox.AddLogLine(finaldata);
            lbl_Status.Text += lbltext;
            logfile.Add(lbltext);
            finaldata.Clear();

            finaldata.Add("Modelo Relaxado: ");
            finaldata.Add(Problema.resp_relax.Item1.ToString());
            finaldata.Add(Problema.resp_relax.Item2);

            lbltext = ToolBox.AddLogLine(finaldata);
            lbl_Status.Text += lbltext;
            logfile.Add(lbltext);
            finaldata.Clear();

            finaldata.Add(CalcCronometro.Elapsed.ToString());
            finaldata.Add("Valor Objetivo Relaxado: ");
            finaldata.Add(Problema.ModelObjVal_relax.ToString());

            lbltext = ToolBox.AddLogLine(finaldata);
            lbl_Status.Text += lbltext;
            logfile.Add(lbltext);
            finaldata.Clear();

            if (clicked && chkMsgBox.Checked)
            {
                MessageBox.Show("Ok", Txt_Path.Text);
            }
            CalcCronometro.Stop();
            finaldata.Add(CalcCronometro.Elapsed.ToString());
            finaldata.Add("Cron. encerrado.  ");

            lbltext = ToolBox.AddLogLine(finaldata);
            lbl_Status.Text += lbltext;
            logfile.Add(lbltext);
            finaldata.Clear();

            ToolBox.FileTxt(Txt_Path.Text + logfile[0], logfile, chkMsgBox.Checked);
            Problema.Dispose();
            this.Text = "CARP_FH";
            lbl_Status.Text = "";
        }
        private string ChangeAnot(bool ending)
        {
            StringBuilder TxtAnot = new StringBuilder();

            TxtAnot.Append("[");
            if (ending)
            {
                bool[,] chks = new bool[3, sizeChk];
                int[] counts = new int[3];
                string[] typs = new string[] { "", "s", "r" };
                string[] stris = new string[sizeChk];
                string[,] resps = new string[3, sizeChk];
                int j;
                int countgeral;
                List<int> indexgeral = new List<int> { 2, 3, 5, 6, 8, 9, 10, 11, 12, 13 };
                int[] indexsum = new int[] { 4, 7, 14, 15, 17, 18, 19, 20 };
                int testsum;
                int[] respadroes = new int[] {3,   7,   15,  18,  22,  30,
                                              35,  39,  47,  50,  54,  62,
                                              67,  71,  79,  82,  86,  94,
                                              129, 133, 141, 144, 148, 156,
                                              161, 165, 173, 176, 180, 188,
                                              193, 197, 205, 208, 212, 220 };
                string[] padroes = new string[] {"Org",       "OrgId",       "OrgR",
                                                 "Org-DV1",   "OrgId-DV1",   "OrgR-DV1",
                                                 "Org-DV2",   "OrgId-DV2",   "OrgR-DV2",
                                                 "Org-DV12",  "OrgId-DV12",  "OrgR-DV12",
                                                 "Org-DV3",   "OrgId-DV3",   "OrgR-DV3",
                                                 "Org-DV13",  "OrgId-DV13",  "OrgR-DV13",
                                                 "Org-DV4",   "OrgId-DV4",   "OrgR-DV4",
                                                 "Org-DV14",  "OrgId-DV14",  "OrgR-DV14",
                                                 "Org-DV24",  "OrgId-DV24",  "OrgR-DV24",
                                                 "Org-DV124", "OrgId-DV124", "OrgR-DV124",
                                                 "Org-DV34",  "OrgId-DV34",  "OrgR-DV34",
                                                 "Org-DV134", "OrgId-DV134", "OrgR-DV134" };

                foreach (int i in ChkIndex)
                {
                    if (i > 0)
                    {
                        j = 0;
                        stris[i] = (i < 10) ? "0" + (i).ToString() : (i).ToString();
                        chks[j, i] = PrincipalCheckBoxes[i] != null && PrincipalCheckBoxes[i].Checked;
                        if (chks[j, i])
                        {
                            counts[j]++;
                            resps[j, i] = "R" + stris[i];
                        }
                        j++;
                        chks[j, i] = SECheckBoxes[i] != null && SECheckBoxes[i].Checked;
                        if (chks[j, i])
                        {
                            counts[j]++;
                            resps[j, i] = stris[i];
                        }
                        j++;
                        chks[j, i] = RHSCheckBoxes[i] != null && RHSCheckBoxes[i].Checked;
                        if (chks[j, i])
                        {
                            counts[j]++;
                            resps[j, i] = stris[i];
                        }
                    }
                }
                for (int k = 0; k < 3; k++)
                {
                    countgeral = 0;
                    testsum = 0;
                    if (counts[k] > 0)
                    {
                        if (k > 0)
                        {
                            TxtAnot.Append("(");
                        }
                        TxtAnot.Append(typs[k]);
                        foreach (int ind in indexgeral)
                        {
                            countgeral += (chks[k, ind]) ? 0 : 1;
                        }
                        for (int s = 0; s < indexsum.Length; s++)
                        {
                            testsum += (chks[k, indexsum[s]]) ? Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(s))) : 0;
                        }
                        if (countgeral == 0 && respadroes.Contains(testsum))
                        {
                            int indresp = Array.FindIndex(respadroes, row => row == testsum);
                            if (TxtAnot.Length < 2)
                            {
                                TxtAnot.Append(padroes[indresp]);
                            }
                            else
                            {
                                TxtAnot.Append('_' + padroes[indresp]);
                            }
                        }
                        else
                        { //Nao estao todas as Rs dos padroes
                            foreach (int i in ChkIndex)
                            {
                                if (chks[k, i])
                                {
                                    TxtAnot.Append('_' + resps[k, i]);
                                }
                            }
                        }
                        if (k > 0)
                        {
                            TxtAnot.Append(")");
                        }
                    }
                }
            }

            if (chkR16.Checked)
            {
                TxtAnot.Append("_GRBBin");
            }
            else
            {
                TxtAnot.Append("_GRBCont");
            }
            TxtAnot.Append("]");
            return TxtAnot.ToString();
        }
        private void ChangeChk(CheckBox Chk)
        {
            if (Chk.Checked)
            {
                Chk.CheckState = CheckState.Unchecked;
            }
            else
            {
                Chk.CheckState = CheckState.Checked;
            }
            chks_opostos(Chk);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chks_opostos(CheckBox Chk)
        {
            switch (Chk.Name)
            {
                case "chkR04":
                    Change_AneqB(chkR04, chkR17);
                    break;
                case "chkR07":
                    Change_AneqB(chkR07, chkR20);
                    break;
                case "chkR17":
                    Change_AneqB(chkR17, chkR04);
                    break;
                case "chkR18":
                    Change_AneqB(chkR18, chkR19);
                    break;
                case "chkR19":
                    Change_AneqB(chkR19, chkR18);
                    break;
                case "chkR20":
                    Change_AneqB(chkR20, chkR07);
                    break;
                case "chkDV02":
                    Change_AneqB(chkDV02, chkDV03);
                    break;
                case "chkDV03":
                    Change_AneqB(chkDV03, chkDV02);
                    break;
            }
        }
        private void ClearChks()
        {
            foreach (int i in ChkIndex)
            {
                if (i > 0)
                {
                    PrincipalCheckBoxes[i].Checked = true;
                    SECheckBoxes[i].Checked = false;
                    PrincipalCheckBoxes[i].Checked = false;
                    Checks(i);
                }
            }
            Txt_Anot.Text = ChangeAnot(true);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearChks();
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            foreach (int i in ChkIndex)
            {
                if (i > 0)
                {
                    ChangeChk(PrincipalCheckBoxes[i]);
                }
            }
            Txt_Anot.Text = ChangeAnot(true);
        }
        private void btnMarkAll_Click(object sender, EventArgs e)
        {
            foreach (int i in ChkIndex)
            {
                if (i > 0)
                {
                    PrincipalCheckBoxes[i].Checked = true;
                }
            }
            Txt_Anot.Text = ChangeAnot(true);
        }
        private decimal[] ColetaPercents()
        {
            decimal[] resp = new decimal[11];
            resp[0] = nUpD_PercentMinimalHarvestArea.Value;
            resp[1] = nUpD_PercentMaximalHarvestArea.Value;
            resp[2] = nUpD_DemandFactor.Value;
            resp[3] = nUpD_PercentVolumeVariation.Value;
            resp[4] = nUpD_NewBlockPercent.Value;
            resp[5] = nUpD_PercentMaxDistInBlock.Value;
            resp[6] = nUpD_PercentMaxDistOutBlockInPeriod.Value;
            resp[7] = nUpD_PercentMaxDistConsecutivePeriod.Value;
            resp[8] = nUpD_PercentMaxNumBlocks.Value;
            resp[9] = nUpD_MinimalHarvestAge10.Value;
            resp[10] = nUpD_FactorHarvestArea.Value;
            return resp;
        }
        private void btn_Roda_Click(object sender, EventArgs e)
        {
            anot = false;
            bool[,] rests = new bool[sizeChk, sizeChk];
            List<Tuple<string, bool>> names = new List<Tuple<string, bool>>();

            List<string> scen = new List<string>();

            foreach (string it in listBox_MultipleScenes.Items)
            {
                scen.Add(it);
            }

            string[] scenes = scen.ToArray();

            foreach (string sc in scenes)
            {
                names.Add(new Tuple<string, bool>(sc, true));
                names.Add(new Tuple<string, bool>(sc, false));
            }

            foreach (Tuple<string, bool> indx in names)
            {
                StringBuilder TxtAnot = new StringBuilder();
                bool[] ConstrCheck = new bool[sizeChk];
                TxtAnot.Append("[all]");
                bool[] SECheck = new bool[sizeChk];
                double[] RHSnum = new double[sizeChk];
                foreach (int i in ChkIndex)
                {
                    ConstrCheck[i] = rests[0, i];
                    SECheck[i] = true;
                    RHSnum[i] = 0;
                }
                //}
                Trocacenario(indx.Item1);
                CARP_FH Problema = new CARP_FH();
                FHInstance Inst = new FHInstance(indx.Item1, Txt_Path.Text, TxtAnot.ToString(), trescsv, ColetaPercents());
                Problema.Instance = Inst;
                if (ShowMess)
                {
                    MessageBox.Show(Inst.Name + " criado", "Instancia Criada");
                }

                Txt_Anot.Text = TxtAnot.ToString();

                Problema.CreateModel(Txt_Path.Text, ConstrCheck, indx.Item2, chkR16.Checked, SECheck, RHSnum, chkMsgBox.Checked);
                Problema.Dispose();
                Inst.Dispose();
                GC.Collect();
                MessageBox.Show(indx.Item1 + " - executado");
            }
            anot = true;
            MessageBox.Show("Ok");
        }
        private void RBtn_ModeloA_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtn_ModeloA.Checked)
            {
                rBtn_ModeloB.Checked = false;
            }
            else
            {
                rBtn_ModeloB.Checked = true;
            }
        }
        private void RBtn_ModeloB_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtn_ModeloB.Checked)
            {
                rBtn_ModeloA.Checked = false;
            }
            else
            {
                rBtn_ModeloA.Checked = true;
            }
        }
        private void ChkSlackExcess_CheckedChanged(object sender, EventArgs e)
        {
            foreach (int i in ChkIndex)
            {
                if (i > 0)
                {
                    if (PrincipalCheckBoxes[i].Checked)
                    {
                        SECheckBoxes[i].Checked = chkSlackExcess.Checked;
                    }
                    else
                    {
                        SECheckBoxes[i].Enabled = true;
                        SECheckBoxes[i].Checked = false;
                        SECheckBoxes[i].Enabled = false;
                    }
                }
            }
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void CBoxCenarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            Trocacenario(cBoxCenarios.Text);
        }
        private void Trocacenario(string valuetext)
        {
            trescsv = true;

            BloqDesbloq(Txt_Path, true);
            BloqDesbloq(Txt_Name, true);
            BloqDesbloq(nUpD_PercentMaximalHarvestArea, false);
            BloqDesbloq(nUpD_PercentMinimalHarvestArea, false);

            string path = "cenarios";

            Txt_Name.Text = valuetext;
            Txt_Path.Text = ToolBox.Path("ForestInstances\\" + path + '\\');

            switch (valuetext)
            {
                case "Kitt2":
                case "Kittaning0":
                case "Kittaning4":
                    trescsv = false;
                    path = "Kittaning4";
                    BloqDesbloq(nUpD_PercentMaximalHarvestArea, true);
                    BloqDesbloq(nUpD_PercentMinimalHarvestArea, true);
                    Txt_Name.Text = valuetext;
                    Txt_Path.Text = ToolBox.Path("ForestInstances\\" + path + '\\');
                    break;
            }

            BloqDesbloq(Txt_Path, false);
            BloqDesbloq(Txt_Name, false);
        }

        private void ChkMsgBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowMess = chkMsgBox.Checked;
        }
        private void BtnMultipleScenes_Click(object sender, EventArgs e)
        {
            foreach (var i in listBox_MultipleScenes.SelectedItems)
            {
                Trocacenario(i.ToString());
                CalcCARPFH();
            }
            MessageBox.Show("Ok", "MultipleScenes");
        }

        private void ChangingRHSChk(CheckBox chkRHS, NumericUpDown nupd)
        {
            if (chkRHS.CheckState == CheckState.Checked)
            {
                BloqDesbloq(nupd, true);
                nupd.Value = 0;
            }
            else
            {
                BloqDesbloq(nupd, false);
            }
        }
        private void ChangingConstrChk(CheckBox chkPrincipal, CheckBox chkSE,
                                       CheckBox chkRHS, NumericUpDown nupd)
        {
            if (chkPrincipal.CheckState == CheckState.Checked)
            {
                chkSE.Enabled = true;
                chkRHS.Enabled = true;
            }
            else
            {
                chkSE.Enabled = false;
                chkRHS.Enabled = false;
                BloqDesbloq(nupd, false);
            }
        }
        private void Checks(int num)
        {
            switch (num)
            {
                case 2:
                    ChangingConstrChk(chkR02, chk_SE_R02, chk_RHS_R02, nUpD_RHS_R02);
                    break;
                case 3:
                    ChangingConstrChk(chkR03, chk_SE_R03, chk_RHS_R03, nUpD_RHS_R03);
                    break;
                case 4:
                    ChangingConstrChk(chkR04, chk_SE_R04, chk_RHS_R04, nUpD_RHS_R04);
                    break;
                case 5:
                    ChangingConstrChk(chkR05, chk_SE_R05, chk_RHS_R05, nUpD_RHS_R05);
                    break;
                case 6:
                    ChangingConstrChk(chkR06, chk_SE_R06, chk_RHS_R06, nUpD_RHS_R06);
                    break;
                case 7:
                    ChangingConstrChk(chkR07, chk_SE_R07, chk_RHS_R07, nUpD_RHS_R07);
                    break;
                case 8:
                    ChangingConstrChk(chkR08, chk_SE_R08, chk_RHS_R08, nUpD_RHS_R08);
                    break;
                case 9:
                    ChangingConstrChk(chkR09, chk_SE_R09, chk_RHS_R09, nUpD_RHS_R09);
                    break;
                case 10:
                    ChangingConstrChk(chkR10, chk_SE_R10, chk_RHS_R10, nUpD_RHS_R10);
                    break;
                case 11:
                    ChangingConstrChk(chkR11, chk_SE_R11, chk_RHS_R11, nUpD_RHS_R11);
                    break;
                case 12:
                    ChangingConstrChk(chkR12, chk_SE_R12, chk_RHS_R12, nUpD_RHS_R12);
                    break;
                case 13:
                    ChangingConstrChk(chkR13, chk_SE_R13, chk_RHS_R13, nUpD_RHS_R13);
                    break;
                case 14:
                    ChangingConstrChk(chkR14, chk_SE_R14, chk_RHS_R14, nUpD_RHS_R14);
                    break;
                case 15:
                    ChangingConstrChk(chkR15, chk_SE_R15, chk_RHS_R15, nUpD_RHS_R15);
                    break;
                case 17:
                    ChangingConstrChk(chkR17, chk_SE_R17, chk_RHS_R17, nUpD_RHS_R17);
                    break;
                case 18:
                    ChangingConstrChk(chkR18, chk_SE_R18, chk_RHS_R18, nUpD_RHS_R18);
                    break;
                case 19:
                    ChangingConstrChk(chkR19, chk_SE_R19, chk_RHS_R19, nUpD_RHS_R19);
                    break;
                case 20:
                    ChangingConstrChk(chkR20, chk_SE_R20, chk_RHS_R20, nUpD_RHS_R20);
                    break;
            }

            if (chkSlackExcess.Checked)
            {
                if (ChkIndex.Contains(num))
                {
                    SECheckBoxes[num].Checked = true;
                }
            }
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void SetDVsConstr(CheckBox DV)
        {
            List<string> ops = new List<string> { "chkDV02", "chkDV03" };
            if (ops.Contains(DV.Name))
            {
                chks_opostos(DV);
            }

            string Name = rBtn_Original.Name;

            if (rBtn_Original_Id.Checked)
            {
                Name = rBtn_Original_Id.Name;
            }
            else if (rBtn_Original_R.Checked)
            {
                Name = rBtn_Original.Name;
            }

            ChooseModel(Name);
        }
        private void ChooseModel(string Name)
        {
            int[] trocas = new int[3];

            List<(CheckBox, int, int, int)> DVs = new List<(CheckBox, int, int, int)> { (chkDV01, 0, 17, 4), (chkDV04, 1, 20, 7) };

            foreach ((CheckBox, int, int, int) Cj in DVs)
            {
                trocas[Cj.Item2] = (Cj.Item1.Checked) ? Cj.Item3 : Cj.Item4;
            }

            if (chkDV02.Checked)
            {
                trocas[2] = 18;
            }
            else if (chkDV03.Checked)
            {
                trocas[2] = 19;
            }

            List<int> chks = new List<int> { 2, 3, trocas[0], 5, 6, trocas[1], 8, 9, 10, 11, 12, 13 };
            switch (Name)
            {
                case "rBtn_Original":
                    chks.Add(16);
                    break;
                case "rBtn_Original_Id":
                    chks.Add(14);
                    chks.Add(16);
                    break;
                case "rBtn_Original_R":
                    chks.Add(14);
                    chks.Add(15);
                    chks.Add(16);
                    break;
            }
            if (trocas[2] > 0)
            {
                chks.Add(trocas[2]);
            }

            foreach (int i in ChkIndex)
            {
                if (i > 0)
                {
                    PrincipalCheckBoxes[i].Checked = false;
                    Checks(i);
                }
            }

            foreach (int i in chks)
            {
                PrincipalCheckBoxes[i].Checked = true;
                Checks(i);
                chks_opostos(PrincipalCheckBoxes[i]);
            }

        }
        private void ChkR02_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR02, chk_SE_R02, chk_RHS_R02, nUpD_RHS_R02);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR03_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR03, chk_SE_R03, chk_RHS_R03, nUpD_RHS_R03);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR04_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR04, chk_SE_R04, chk_RHS_R04, nUpD_RHS_R04);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR05_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR05, chk_SE_R05, chk_RHS_R05, nUpD_RHS_R05);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR06_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR06, chk_SE_R06, chk_RHS_R06, nUpD_RHS_R06);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR07_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR07, chk_SE_R07, chk_RHS_R07, nUpD_RHS_R07);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR08_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR08, chk_SE_R08, chk_RHS_R08, nUpD_RHS_R08);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR09_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR09, chk_SE_R09, chk_RHS_R09, nUpD_RHS_R09);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR10_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR10, chk_SE_R10, chk_RHS_R10, nUpD_RHS_R10);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR11_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR11, chk_SE_R11, chk_RHS_R11, nUpD_RHS_R11);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR12_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR12, chk_SE_R12, chk_RHS_R12, nUpD_RHS_R12);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR13_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR13, chk_SE_R13, chk_RHS_R13, nUpD_RHS_R13);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR14_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR14, chk_SE_R14, chk_RHS_R14, nUpD_RHS_R14);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR15_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR15, chk_SE_R15, chk_RHS_R15, nUpD_RHS_R15);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR16_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR17_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR17, chk_SE_R17, chk_RHS_R17, nUpD_RHS_R17);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR18_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR18, chk_SE_R18, chk_RHS_R18, nUpD_RHS_R18);
            if (chkR18.Checked)
            {
                chkR19.Checked = false;
            }
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR19_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR19, chk_SE_R19, chk_RHS_R19, nUpD_RHS_R19);
            if (chkR19.Checked)
            {
                chkR18.Checked = false;
            }
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void ChkR20_CheckedChanged(object sender, EventArgs e)
        {
            ChangingConstrChk(chkR20, chk_SE_R20, chk_RHS_R20, nUpD_RHS_R20);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void Chk_RHS_R02_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R02, nUpD_RHS_R02);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void Chk_RHS_R03_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R03, nUpD_RHS_R03);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R04_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R04, nUpD_RHS_R04);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R05_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R05, nUpD_RHS_R05);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void Chk_RHS_R06_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R06, nUpD_RHS_R06);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void Chk_RHS_R07_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R07, nUpD_RHS_R07);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void Chk_RHS_R08_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R08, nUpD_RHS_R08);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void Chk_RHS_R09_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R09, nUpD_RHS_R09);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void Chk_RHS_R10_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R10, nUpD_RHS_R10);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R11_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R11, nUpD_RHS_R11);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R12_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R12, nUpD_RHS_R12);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R13_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R13, nUpD_RHS_R13);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R14_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R14, nUpD_RHS_R14);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R15_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R15, nUpD_RHS_R15);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R17_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R17, nUpD_RHS_R17);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R18_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R18, nUpD_RHS_R18);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R19_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R19, nUpD_RHS_R19);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_RHS_R20_CheckedChanged(object sender, EventArgs e)
        {
            ChangingRHSChk(chk_RHS_R20, nUpD_RHS_R20);
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R02_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R03_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R04_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R05_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R06_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R07_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R08_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R09_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R10_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R11_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R12_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R13_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R14_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R15_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R17_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R18_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void chk_SE_R19_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void Chk_SE_R20_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Anot.Text = ChangeAnot(anot);
        }
        private void RBtn_Original_CheckedChanged(object sender, EventArgs e)
        {
            ChooseModel(rBtn_Original.Name);
        }
        private void RBtn_Original_Id_CheckedChanged(object sender, EventArgs e)
        {
            ChooseModel(rBtn_Original_Id.Name);
        }
        private void RBtn_Original_R_CheckedChanged(object sender, EventArgs e)
        {
            ChooseModel(rBtn_Original_R.Name);
        }
        private void ChkDV01_CheckedChanged(object sender, EventArgs e)
        {
            SetDVsConstr(chkDV01);
        }
        private void chkDV02_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDV02.Checked)
            {
                SetDVsConstr(chkDV02);
                chkR18.Checked = true;
            }
            else
            {
                chkR18.Checked = false;
            }
        }
        private void chkDV03_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDV03.Checked)
            {
                SetDVsConstr(chkDV03);
                chkR19.Checked = true;
            }
            else
            {
                chkR19.Checked = false;
            }
        }
        private void chkDV04_CheckedChanged(object sender, EventArgs e)
        {
            SetDVsConstr(chkDV04);
        }
        private void lblScenes_Click(object sender, EventArgs e)
        {

        }

        private void btnSeqModels_Click(object sender, EventArgs e)
        {/*
            List<(string, int[])> ModelsScenes = new List<(string, int[])>
            {
                ("Org",       new int[] {  4,  7,  2, 3, 5, 6, 8, 9, 10, 11, 12, 13}),
                ("Org-DV1",   new int[] {  7, 17,  2, 3, 5, 6, 8, 9, 10, 11, 12, 13}),
                ("Org-DV2",   new int[] {  4,  7, 18, 2, 3, 5, 6, 8,  9, 10, 11, 12, 13}),
                ("Org-DV12",  new int[] {  7, 17, 18, 2, 3, 5, 6, 8,  9, 10, 11, 12, 13}),
                ("Org-DV3",   new int[] {  4,  7, 19, 2, 3, 5, 6, 8,  9, 10, 11, 12, 13}),
                ("Org-DV13",  new int[] {  7, 17, 19, 2, 3, 5, 6, 8,  9, 10, 11, 12, 13}),
                ("Org-DV4",   new int[] {  4, 20,  2, 3, 5, 6, 8, 9, 10, 11, 12, 13}),
                ("Org-DV14",  new int[] { 17, 20,  2, 3, 5, 6, 8, 9, 10, 11, 12, 13}),
                ("Org-DV24",  new int[] {  4, 18, 20, 2, 3, 5, 6, 8,  9, 10, 11, 12, 13}),
                ("Org-DV124", new int[] { 17, 18, 20, 2, 3, 5, 6, 8,  9, 10, 11, 12, 13}),
                ("Org-DV34",  new int[] {  4, 19, 20, 2, 3, 5, 6, 8,  9, 10, 11, 12, 13}),
                ("Org-DV134", new int[] { 17, 19, 20, 2, 3, 5, 6, 8,  9, 10, 11, 12, 13})
            };*/
            List<(string, bool[])> ModelsScenes = new List<(string, bool[])>
            {
                ("Org",       new bool[] { false, false, false, false}),
                ("Org-DV1",   new bool[] {  true, false, false, false}),
                ("Org-DV2",   new bool[] { false,  true, false, false}),
                ("Org-DV12",  new bool[] {  true,  true, false, false}),
                ("Org-DV3",   new bool[] { false, false,  true, false}),
                ("Org-DV13",  new bool[] {  true, false,  true, false}),
                ("Org-DV4",   new bool[] { false, false, false,  true}),
                ("Org-DV14",  new bool[] {  true, false, false,  true}),
                ("Org-DV24",  new bool[] { false,  true, false,  true}),
                ("Org-DV124", new bool[] {  true,  true, false,  true}),
                ("Org-DV34",  new bool[] { false, false,  true,  true}),
                ("Org-DV134", new bool[] {  true, false,  true,  true})
            };
            bool[] SE = new bool[] { false, true };
            CheckBox[] DVs = new CheckBox[]
            {
                chkDV01, chkDV02, chkDV03, chkDV04
            };

            foreach (bool varSE in SE)
            {
                //Empty All Chks
                foreach ((string, bool[]) Ms in ModelsScenes)
                {
                    ClearChks();

                    for (int qtd = 0; qtd < DVs.Length; qtd++)
                    {
                        DVs[qtd].Checked = Ms.Item2[qtd];
                    }
                    chkSlackExcess.Checked = false;
                    rBtn_Original.Checked = true;
                    ChooseModel("rBtn_Original");
                    chkSlackExcess.Checked = varSE;
                    //MessageBox.Show(Ms.Item1, "Verificar");
                    CalcCARPFH();
                    MessageBox.Show(Txt_Name.Text + "\t - \t" + Ms.Item1, "Calculado");
                }
            }
        }
    }
}