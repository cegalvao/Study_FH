
namespace Study_FH
{
    partial class Form6
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmBArquivo = new System.Windows.Forms.ComboBox();
            this.btnDados = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCoordX = new System.Windows.Forms.Label();
            this.lblCoordY = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtInd = new System.Windows.Forms.TextBox();
            this.picF6 = new System.Windows.Forms.PictureBox();
            this.btnDrawCoords = new System.Windows.Forms.Button();
            this.lbl_CoordNovo = new System.Windows.Forms.Label();
            this.cmBCoords = new System.Windows.Forms.ComboBox();
            this.nupdTxtCoordX = new System.Windows.Forms.NumericUpDown();
            this.nupdTxtCoordY = new System.Windows.Forms.NumericUpDown();
            this.btnFileCoords = new System.Windows.Forms.Button();
            this.nupdXorig1 = new System.Windows.Forms.NumericUpDown();
            this.nupdYorig1 = new System.Windows.Forms.NumericUpDown();
            this.nupdXorig2 = new System.Windows.Forms.NumericUpDown();
            this.nupdYorig2 = new System.Windows.Forms.NumericUpDown();
            this.nupdXnovo1 = new System.Windows.Forms.NumericUpDown();
            this.nupdYnovo1 = new System.Windows.Forms.NumericUpDown();
            this.nupdXnovo2 = new System.Windows.Forms.NumericUpDown();
            this.nupdYnovo2 = new System.Windows.Forms.NumericUpDown();
            this.lblXorig1 = new System.Windows.Forms.Label();
            this.lblYorig1 = new System.Windows.Forms.Label();
            this.lblXorig2 = new System.Windows.Forms.Label();
            this.lblYorig2 = new System.Windows.Forms.Label();
            this.lblXnovo1 = new System.Windows.Forms.Label();
            this.lblYnovo1 = new System.Windows.Forms.Label();
            this.lblXnovo2 = new System.Windows.Forms.Label();
            this.lblYnovo2 = new System.Windows.Forms.Label();
            this.lblOrig1 = new System.Windows.Forms.Label();
            this.lblOrig2 = new System.Windows.Forms.Label();
            this.lblNovo1 = new System.Windows.Forms.Label();
            this.lblNovo2 = new System.Windows.Forms.Label();
            this.nupdRaioCric = new System.Windows.Forms.NumericUpDown();
            this.lbl_raiocric = new System.Windows.Forms.Label();
            this.btnRGB2csv = new System.Windows.Forms.Button();
            this.cmbCorrecaoCor = new System.Windows.Forms.ComboBox();
            this.btnCorrectCor = new System.Windows.Forms.Button();
            this.btnSaveErrors = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picF6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdTxtCoordX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdTxtCoordY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdXorig1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdYorig1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdXorig2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdYorig2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdXnovo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdYnovo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdXnovo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdYnovo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdRaioCric)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Arquivo";
            // 
            // cmBArquivo
            // 
            this.cmBArquivo.FormattingEnabled = true;
            this.cmBArquivo.Items.AddRange(new object[] {
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV1.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV2.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV3.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV4.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV12.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV13.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV14.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV24.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV34.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV124.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_DV134.png",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2_ORG.png"});
            this.cmBArquivo.Location = new System.Drawing.Point(62, 10);
            this.cmBArquivo.Name = "cmBArquivo";
            this.cmBArquivo.Size = new System.Drawing.Size(300, 21);
            this.cmBArquivo.TabIndex = 0;
            this.cmBArquivo.SelectedIndexChanged += new System.EventHandler(this.CmBArquivo_SelectedIndexChanged);
            // 
            // btnDados
            // 
            this.btnDados.Location = new System.Drawing.Point(387, 7);
            this.btnDados.Name = "btnDados";
            this.btnDados.Size = new System.Drawing.Size(300, 25);
            this.btnDados.TabIndex = 5;
            this.btnDados.Text = "Levantar Dados";
            this.btnDados.UseVisualStyleBackColor = true;
            this.btnDados.Click += new System.EventHandler(this.btnDados_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Coordenadas";
            // 
            // lblCoordX
            // 
            this.lblCoordX.AutoSize = true;
            this.lblCoordX.Location = new System.Drawing.Point(96, 81);
            this.lblCoordX.Name = "lblCoordX";
            this.lblCoordX.Size = new System.Drawing.Size(14, 13);
            this.lblCoordX.TabIndex = 4;
            this.lblCoordX.Text = "X";
            // 
            // lblCoordY
            // 
            this.lblCoordY.AutoSize = true;
            this.lblCoordY.Location = new System.Drawing.Point(180, 81);
            this.lblCoordY.Name = "lblCoordY";
            this.lblCoordY.Size = new System.Drawing.Size(14, 13);
            this.lblCoordY.TabIndex = 6;
            this.lblCoordY.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(264, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Indice";
            // 
            // TxtInd
            // 
            this.TxtInd.Location = new System.Drawing.Point(310, 74);
            this.TxtInd.Name = "TxtInd";
            this.TxtInd.Size = new System.Drawing.Size(50, 20);
            this.TxtInd.TabIndex = 4;
            // 
            // picF6
            // 
            this.picF6.Location = new System.Drawing.Point(16, 100);
            this.picF6.Name = "picF6";
            this.picF6.Size = new System.Drawing.Size(850, 850);
            this.picF6.TabIndex = 10;
            this.picF6.TabStop = false;
            // 
            // btnDrawCoords
            // 
            this.btnDrawCoords.Location = new System.Drawing.Point(387, 39);
            this.btnDrawCoords.Name = "btnDrawCoords";
            this.btnDrawCoords.Size = new System.Drawing.Size(300, 25);
            this.btnDrawCoords.TabIndex = 6;
            this.btnDrawCoords.Text = "Desenhar Coords.";
            this.btnDrawCoords.UseVisualStyleBackColor = true;
            this.btnDrawCoords.Click += new System.EventHandler(this.BtnDrawCoords_Click);
            // 
            // lbl_CoordNovo
            // 
            this.lbl_CoordNovo.AutoSize = true;
            this.lbl_CoordNovo.Location = new System.Drawing.Point(703, 7);
            this.lbl_CoordNovo.Name = "lbl_CoordNovo";
            this.lbl_CoordNovo.Size = new System.Drawing.Size(36, 13);
            this.lbl_CoordNovo.TabIndex = 12;
            this.lbl_CoordNovo.Text = "Novo:";
            // 
            // cmBCoords
            // 
            this.cmBCoords.FormattingEnabled = true;
            this.cmBCoords.Location = new System.Drawing.Point(89, 38);
            this.cmBCoords.Name = "cmBCoords";
            this.cmBCoords.Size = new System.Drawing.Size(272, 21);
            this.cmBCoords.TabIndex = 1;
            this.cmBCoords.SelectedIndexChanged += new System.EventHandler(this.CmBCoords_SelectedIndexChanged);
            // 
            // nupdTxtCoordX
            // 
            this.nupdTxtCoordX.DecimalPlaces = 2;
            this.nupdTxtCoordX.Location = new System.Drawing.Point(124, 74);
            this.nupdTxtCoordX.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdTxtCoordX.Name = "nupdTxtCoordX";
            this.nupdTxtCoordX.Size = new System.Drawing.Size(50, 20);
            this.nupdTxtCoordX.TabIndex = 2;
            this.nupdTxtCoordX.ValueChanged += new System.EventHandler(this.NupdTxtCoordX_ValueChanged);
            // 
            // nupdTxtCoordY
            // 
            this.nupdTxtCoordY.DecimalPlaces = 2;
            this.nupdTxtCoordY.Location = new System.Drawing.Point(200, 74);
            this.nupdTxtCoordY.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdTxtCoordY.Name = "nupdTxtCoordY";
            this.nupdTxtCoordY.Size = new System.Drawing.Size(50, 20);
            this.nupdTxtCoordY.TabIndex = 3;
            this.nupdTxtCoordY.ValueChanged += new System.EventHandler(this.nupdTxtCoordY_ValueChanged);
            // 
            // btnFileCoords
            // 
            this.btnFileCoords.Location = new System.Drawing.Point(387, 70);
            this.btnFileCoords.Name = "btnFileCoords";
            this.btnFileCoords.Size = new System.Drawing.Size(300, 25);
            this.btnFileCoords.TabIndex = 7;
            this.btnFileCoords.Text = "Todas as Coords do Arquivo";
            this.btnFileCoords.UseVisualStyleBackColor = true;
            this.btnFileCoords.Click += new System.EventHandler(this.btnFileCoords_Click);
            // 
            // nupdXorig1
            // 
            this.nupdXorig1.DecimalPlaces = 2;
            this.nupdXorig1.Location = new System.Drawing.Point(929, 116);
            this.nupdXorig1.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdXorig1.Name = "nupdXorig1";
            this.nupdXorig1.Size = new System.Drawing.Size(120, 20);
            this.nupdXorig1.TabIndex = 8;
            this.nupdXorig1.Value = new decimal(new int[] {
            146,
            0,
            0,
            0});
            this.nupdXorig1.ValueChanged += new System.EventHandler(this.nupdXorig1_ValueChanged);
            // 
            // nupdYorig1
            // 
            this.nupdYorig1.DecimalPlaces = 2;
            this.nupdYorig1.Location = new System.Drawing.Point(929, 143);
            this.nupdYorig1.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdYorig1.Name = "nupdYorig1";
            this.nupdYorig1.Size = new System.Drawing.Size(120, 20);
            this.nupdYorig1.TabIndex = 9;
            this.nupdYorig1.Value = new decimal(new int[] {
            139,
            0,
            0,
            0});
            this.nupdYorig1.ValueChanged += new System.EventHandler(this.nupdYorig1_ValueChanged);
            // 
            // nupdXorig2
            // 
            this.nupdXorig2.DecimalPlaces = 2;
            this.nupdXorig2.Location = new System.Drawing.Point(929, 194);
            this.nupdXorig2.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdXorig2.Name = "nupdXorig2";
            this.nupdXorig2.Size = new System.Drawing.Size(120, 20);
            this.nupdXorig2.TabIndex = 10;
            this.nupdXorig2.Value = new decimal(new int[] {
            620,
            0,
            0,
            0});
            this.nupdXorig2.ValueChanged += new System.EventHandler(this.nupdXorig2_ValueChanged);
            // 
            // nupdYorig2
            // 
            this.nupdYorig2.DecimalPlaces = 2;
            this.nupdYorig2.Location = new System.Drawing.Point(929, 221);
            this.nupdYorig2.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdYorig2.Name = "nupdYorig2";
            this.nupdYorig2.Size = new System.Drawing.Size(120, 20);
            this.nupdYorig2.TabIndex = 11;
            this.nupdYorig2.Value = new decimal(new int[] {
            797,
            0,
            0,
            0});
            this.nupdYorig2.ValueChanged += new System.EventHandler(this.nupdYorig2_ValueChanged);
            // 
            // nupdXnovo1
            // 
            this.nupdXnovo1.DecimalPlaces = 2;
            this.nupdXnovo1.Location = new System.Drawing.Point(929, 280);
            this.nupdXnovo1.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdXnovo1.Name = "nupdXnovo1";
            this.nupdXnovo1.Size = new System.Drawing.Size(120, 20);
            this.nupdXnovo1.TabIndex = 12;
            this.nupdXnovo1.Value = new decimal(new int[] {
            196,
            0,
            0,
            0});
            this.nupdXnovo1.ValueChanged += new System.EventHandler(this.nupdXnovo1_ValueChanged);
            // 
            // nupdYnovo1
            // 
            this.nupdYnovo1.DecimalPlaces = 2;
            this.nupdYnovo1.Location = new System.Drawing.Point(929, 307);
            this.nupdYnovo1.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdYnovo1.Name = "nupdYnovo1";
            this.nupdYnovo1.Size = new System.Drawing.Size(120, 20);
            this.nupdYnovo1.TabIndex = 13;
            this.nupdYnovo1.Value = new decimal(new int[] {
            57,
            0,
            0,
            0});
            this.nupdYnovo1.ValueChanged += new System.EventHandler(this.nupdYnovo1_ValueChanged);
            // 
            // nupdXnovo2
            // 
            this.nupdXnovo2.DecimalPlaces = 2;
            this.nupdXnovo2.Location = new System.Drawing.Point(929, 357);
            this.nupdXnovo2.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdXnovo2.Name = "nupdXnovo2";
            this.nupdXnovo2.Size = new System.Drawing.Size(120, 20);
            this.nupdXnovo2.TabIndex = 14;
            this.nupdXnovo2.Value = new decimal(new int[] {
            651,
            0,
            0,
            0});
            this.nupdXnovo2.ValueChanged += new System.EventHandler(this.nupdXnovo2_ValueChanged);
            // 
            // nupdYnovo2
            // 
            this.nupdYnovo2.DecimalPlaces = 2;
            this.nupdYnovo2.Location = new System.Drawing.Point(929, 384);
            this.nupdYnovo2.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.nupdYnovo2.Name = "nupdYnovo2";
            this.nupdYnovo2.Size = new System.Drawing.Size(120, 20);
            this.nupdYnovo2.TabIndex = 15;
            this.nupdYnovo2.Value = new decimal(new int[] {
            687,
            0,
            0,
            0});
            this.nupdYnovo2.ValueChanged += new System.EventHandler(this.nupdYnovo2_ValueChanged);
            // 
            // lblXorig1
            // 
            this.lblXorig1.AutoSize = true;
            this.lblXorig1.Location = new System.Drawing.Point(909, 120);
            this.lblXorig1.Name = "lblXorig1";
            this.lblXorig1.Size = new System.Drawing.Size(14, 13);
            this.lblXorig1.TabIndex = 25;
            this.lblXorig1.Text = "X";
            // 
            // lblYorig1
            // 
            this.lblYorig1.AutoSize = true;
            this.lblYorig1.Location = new System.Drawing.Point(909, 147);
            this.lblYorig1.Name = "lblYorig1";
            this.lblYorig1.Size = new System.Drawing.Size(14, 13);
            this.lblYorig1.TabIndex = 26;
            this.lblYorig1.Text = "Y";
            // 
            // lblXorig2
            // 
            this.lblXorig2.AutoSize = true;
            this.lblXorig2.Location = new System.Drawing.Point(909, 198);
            this.lblXorig2.Name = "lblXorig2";
            this.lblXorig2.Size = new System.Drawing.Size(14, 13);
            this.lblXorig2.TabIndex = 27;
            this.lblXorig2.Text = "X";
            // 
            // lblYorig2
            // 
            this.lblYorig2.AutoSize = true;
            this.lblYorig2.Location = new System.Drawing.Point(909, 225);
            this.lblYorig2.Name = "lblYorig2";
            this.lblYorig2.Size = new System.Drawing.Size(14, 13);
            this.lblYorig2.TabIndex = 28;
            this.lblYorig2.Text = "Y";
            // 
            // lblXnovo1
            // 
            this.lblXnovo1.AutoSize = true;
            this.lblXnovo1.Location = new System.Drawing.Point(909, 284);
            this.lblXnovo1.Name = "lblXnovo1";
            this.lblXnovo1.Size = new System.Drawing.Size(14, 13);
            this.lblXnovo1.TabIndex = 29;
            this.lblXnovo1.Text = "X";
            // 
            // lblYnovo1
            // 
            this.lblYnovo1.AutoSize = true;
            this.lblYnovo1.Location = new System.Drawing.Point(909, 311);
            this.lblYnovo1.Name = "lblYnovo1";
            this.lblYnovo1.Size = new System.Drawing.Size(14, 13);
            this.lblYnovo1.TabIndex = 30;
            this.lblYnovo1.Text = "Y";
            // 
            // lblXnovo2
            // 
            this.lblXnovo2.AutoSize = true;
            this.lblXnovo2.Location = new System.Drawing.Point(909, 361);
            this.lblXnovo2.Name = "lblXnovo2";
            this.lblXnovo2.Size = new System.Drawing.Size(14, 13);
            this.lblXnovo2.TabIndex = 31;
            this.lblXnovo2.Text = "X";
            // 
            // lblYnovo2
            // 
            this.lblYnovo2.AutoSize = true;
            this.lblYnovo2.Location = new System.Drawing.Point(909, 388);
            this.lblYnovo2.Name = "lblYnovo2";
            this.lblYnovo2.Size = new System.Drawing.Size(14, 13);
            this.lblYnovo2.TabIndex = 32;
            this.lblYnovo2.Text = "Y";
            // 
            // lblOrig1
            // 
            this.lblOrig1.AutoSize = true;
            this.lblOrig1.Location = new System.Drawing.Point(929, 100);
            this.lblOrig1.Name = "lblOrig1";
            this.lblOrig1.Size = new System.Drawing.Size(79, 13);
            this.lblOrig1.TabIndex = 33;
            this.lblOrig1.Text = "Figura - Ponto1";
            // 
            // lblOrig2
            // 
            this.lblOrig2.AutoSize = true;
            this.lblOrig2.Location = new System.Drawing.Point(929, 178);
            this.lblOrig2.Name = "lblOrig2";
            this.lblOrig2.Size = new System.Drawing.Size(79, 13);
            this.lblOrig2.TabIndex = 34;
            this.lblOrig2.Text = "Figura - Ponto2";
            // 
            // lblNovo1
            // 
            this.lblNovo1.AutoSize = true;
            this.lblNovo1.Location = new System.Drawing.Point(932, 261);
            this.lblNovo1.Name = "lblNovo1";
            this.lblNovo1.Size = new System.Drawing.Size(74, 13);
            this.lblNovo1.TabIndex = 35;
            this.lblNovo1.Text = "CSV - Ponto 1";
            // 
            // lblNovo2
            // 
            this.lblNovo2.AutoSize = true;
            this.lblNovo2.Location = new System.Drawing.Point(929, 338);
            this.lblNovo2.Name = "lblNovo2";
            this.lblNovo2.Size = new System.Drawing.Size(74, 13);
            this.lblNovo2.TabIndex = 36;
            this.lblNovo2.Text = "CSV - Ponto 2";
            // 
            // nupdRaioCric
            // 
            this.nupdRaioCric.Location = new System.Drawing.Point(929, 443);
            this.nupdRaioCric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nupdRaioCric.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nupdRaioCric.Name = "nupdRaioCric";
            this.nupdRaioCric.Size = new System.Drawing.Size(120, 20);
            this.nupdRaioCric.TabIndex = 16;
            this.nupdRaioCric.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nupdRaioCric.ValueChanged += new System.EventHandler(this.nupdRaioCric_ValueChanged);
            // 
            // lbl_raiocric
            // 
            this.lbl_raiocric.AutoSize = true;
            this.lbl_raiocric.Location = new System.Drawing.Point(929, 424);
            this.lbl_raiocric.Name = "lbl_raiocric";
            this.lbl_raiocric.Size = new System.Drawing.Size(61, 13);
            this.lbl_raiocric.TabIndex = 38;
            this.lbl_raiocric.Text = "Raio Pincel";
            // 
            // btnRGB2csv
            // 
            this.btnRGB2csv.Location = new System.Drawing.Point(912, 489);
            this.btnRGB2csv.Name = "btnRGB2csv";
            this.btnRGB2csv.Size = new System.Drawing.Size(137, 25);
            this.btnRGB2csv.TabIndex = 17;
            this.btnRGB2csv.Text = "RGBs arquivo";
            this.btnRGB2csv.UseVisualStyleBackColor = true;
            this.btnRGB2csv.Click += new System.EventHandler(this.btnRGB2csv_Click);
            // 
            // cmbCorrecaoCor
            // 
            this.cmbCorrecaoCor.FormattingEnabled = true;
            this.cmbCorrecaoCor.Location = new System.Drawing.Point(929, 532);
            this.cmbCorrecaoCor.Name = "cmbCorrecaoCor";
            this.cmbCorrecaoCor.Size = new System.Drawing.Size(121, 21);
            this.cmbCorrecaoCor.TabIndex = 39;
            // 
            // btnCorrectCor
            // 
            this.btnCorrectCor.Location = new System.Drawing.Point(912, 567);
            this.btnCorrectCor.Name = "btnCorrectCor";
            this.btnCorrectCor.Size = new System.Drawing.Size(137, 25);
            this.btnCorrectCor.TabIndex = 40;
            this.btnCorrectCor.Text = "Corrigir Cor";
            this.btnCorrectCor.UseVisualStyleBackColor = true;
            this.btnCorrectCor.Click += new System.EventHandler(this.BtnCorrectCor_Click);
            // 
            // btnSaveErrors
            // 
            this.btnSaveErrors.Location = new System.Drawing.Point(912, 599);
            this.btnSaveErrors.Name = "btnSaveErrors";
            this.btnSaveErrors.Size = new System.Drawing.Size(137, 25);
            this.btnSaveErrors.TabIndex = 41;
            this.btnSaveErrors.Text = "Gravar Erros";
            this.btnSaveErrors.UseVisualStyleBackColor = true;
            this.btnSaveErrors.Click += new System.EventHandler(this.BtnSaveErrors_Click);
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 971);
            this.Controls.Add(this.btnSaveErrors);
            this.Controls.Add(this.btnCorrectCor);
            this.Controls.Add(this.cmbCorrecaoCor);
            this.Controls.Add(this.btnRGB2csv);
            this.Controls.Add(this.lbl_raiocric);
            this.Controls.Add(this.nupdRaioCric);
            this.Controls.Add(this.lblNovo2);
            this.Controls.Add(this.lblNovo1);
            this.Controls.Add(this.lblOrig2);
            this.Controls.Add(this.lblOrig1);
            this.Controls.Add(this.lblYnovo2);
            this.Controls.Add(this.lblXnovo2);
            this.Controls.Add(this.lblYnovo1);
            this.Controls.Add(this.lblXnovo1);
            this.Controls.Add(this.lblYorig2);
            this.Controls.Add(this.lblXorig2);
            this.Controls.Add(this.lblYorig1);
            this.Controls.Add(this.lblXorig1);
            this.Controls.Add(this.nupdYnovo2);
            this.Controls.Add(this.nupdXnovo2);
            this.Controls.Add(this.nupdYnovo1);
            this.Controls.Add(this.nupdXnovo1);
            this.Controls.Add(this.nupdYorig2);
            this.Controls.Add(this.nupdXorig2);
            this.Controls.Add(this.nupdYorig1);
            this.Controls.Add(this.nupdXorig1);
            this.Controls.Add(this.btnFileCoords);
            this.Controls.Add(this.nupdTxtCoordY);
            this.Controls.Add(this.nupdTxtCoordX);
            this.Controls.Add(this.cmBCoords);
            this.Controls.Add(this.lbl_CoordNovo);
            this.Controls.Add(this.btnDrawCoords);
            this.Controls.Add(this.picF6);
            this.Controls.Add(this.TxtInd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblCoordY);
            this.Controls.Add(this.lblCoordX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDados);
            this.Controls.Add(this.cmBArquivo);
            this.Controls.Add(this.label1);
            this.Name = "Form6";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form6";
            ((System.ComponentModel.ISupportInitialize)(this.picF6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdTxtCoordX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdTxtCoordY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdXorig1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdYorig1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdXorig2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdYorig2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdXnovo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdYnovo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdXnovo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdYnovo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupdRaioCric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmBArquivo;
        private System.Windows.Forms.Button btnDados;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCoordX;
        private System.Windows.Forms.Label lblCoordY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtInd;
        private System.Windows.Forms.PictureBox picF6;
        private System.Windows.Forms.Button btnDrawCoords;
        private System.Windows.Forms.Label lbl_CoordNovo;
        private System.Windows.Forms.ComboBox cmBCoords;
        private System.Windows.Forms.NumericUpDown nupdTxtCoordX;
        private System.Windows.Forms.NumericUpDown nupdTxtCoordY;
        private System.Windows.Forms.Button btnFileCoords;
        private System.Windows.Forms.NumericUpDown nupdXorig1;
        private System.Windows.Forms.NumericUpDown nupdYorig1;
        private System.Windows.Forms.NumericUpDown nupdXorig2;
        private System.Windows.Forms.NumericUpDown nupdYorig2;
        private System.Windows.Forms.NumericUpDown nupdXnovo1;
        private System.Windows.Forms.NumericUpDown nupdYnovo1;
        private System.Windows.Forms.NumericUpDown nupdXnovo2;
        private System.Windows.Forms.NumericUpDown nupdYnovo2;
        private System.Windows.Forms.Label lblXorig1;
        private System.Windows.Forms.Label lblYorig1;
        private System.Windows.Forms.Label lblXorig2;
        private System.Windows.Forms.Label lblYorig2;
        private System.Windows.Forms.Label lblXnovo1;
        private System.Windows.Forms.Label lblYnovo1;
        private System.Windows.Forms.Label lblXnovo2;
        private System.Windows.Forms.Label lblYnovo2;
        private System.Windows.Forms.Label lblOrig1;
        private System.Windows.Forms.Label lblOrig2;
        private System.Windows.Forms.Label lblNovo1;
        private System.Windows.Forms.Label lblNovo2;
        private System.Windows.Forms.NumericUpDown nupdRaioCric;
        private System.Windows.Forms.Label lbl_raiocric;
        private System.Windows.Forms.Button btnRGB2csv;
        private System.Windows.Forms.ComboBox cmbCorrecaoCor;
        private System.Windows.Forms.Button btnCorrectCor;
        private System.Windows.Forms.Button btnSaveErrors;
    }
}