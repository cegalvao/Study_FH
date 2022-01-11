
namespace Study_FH
{
    partial class Form4
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
            this.picTela = new System.Windows.Forms.PictureBox();
            this.lbl_numFMU = new System.Windows.Forms.Label();
            this.nUpD_FMUs = new System.Windows.Forms.NumericUpDown();
            this.BtnDraw = new System.Windows.Forms.Button();
            this.BtnCleanPic = new System.Windows.Forms.Button();
            this.rBtn_PolRegular = new System.Windows.Forms.RadioButton();
            this.rBtn_Random = new System.Windows.Forms.RadioButton();
            this.lbl_Seed = new System.Windows.Forms.Label();
            this.nUpD_Seed = new System.Windows.Forms.NumericUpDown();
            this.BtnTSP = new System.Windows.Forms.Button();
            this.picTela2 = new System.Windows.Forms.PictureBox();
            this.lbl_Pts = new System.Windows.Forms.Label();
            this.txt_Pts = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picTela)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpD_FMUs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpD_Seed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTela2)).BeginInit();
            this.SuspendLayout();
            // 
            // picTela
            // 
            this.picTela.Location = new System.Drawing.Point(5, 5);
            this.picTela.Name = "picTela";
            this.picTela.Size = new System.Drawing.Size(500, 500);
            this.picTela.TabIndex = 4;
            this.picTela.TabStop = false;
            // 
            // lbl_numFMU
            // 
            this.lbl_numFMU.AutoSize = true;
            this.lbl_numFMU.Location = new System.Drawing.Point(515, 5);
            this.lbl_numFMU.Name = "lbl_numFMU";
            this.lbl_numFMU.Size = new System.Drawing.Size(60, 13);
            this.lbl_numFMU.TabIndex = 5;
            this.lbl_numFMU.Text = "Num FMUs";
            // 
            // nUpD_FMUs
            // 
            this.nUpD_FMUs.Location = new System.Drawing.Point(587, 5);
            this.nUpD_FMUs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nUpD_FMUs.Name = "nUpD_FMUs";
            this.nUpD_FMUs.Size = new System.Drawing.Size(34, 20);
            this.nUpD_FMUs.TabIndex = 0;
            this.nUpD_FMUs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // BtnDraw
            // 
            this.BtnDraw.Location = new System.Drawing.Point(631, 5);
            this.BtnDraw.Name = "BtnDraw";
            this.BtnDraw.Size = new System.Drawing.Size(100, 40);
            this.BtnDraw.TabIndex = 4;
            this.BtnDraw.Text = "Desenhar";
            this.BtnDraw.UseVisualStyleBackColor = true;
            this.BtnDraw.Click += new System.EventHandler(this.BtnDraw_Click);
            // 
            // BtnCleanPic
            // 
            this.BtnCleanPic.Location = new System.Drawing.Point(631, 57);
            this.BtnCleanPic.Name = "BtnCleanPic";
            this.BtnCleanPic.Size = new System.Drawing.Size(100, 40);
            this.BtnCleanPic.TabIndex = 5;
            this.BtnCleanPic.Text = "Limpar Tela";
            this.BtnCleanPic.UseVisualStyleBackColor = true;
            this.BtnCleanPic.Click += new System.EventHandler(this.BtnCleanPic_Click);
            // 
            // rBtn_PolRegular
            // 
            this.rBtn_PolRegular.AutoSize = true;
            this.rBtn_PolRegular.Checked = true;
            this.rBtn_PolRegular.Location = new System.Drawing.Point(518, 31);
            this.rBtn_PolRegular.Name = "rBtn_PolRegular";
            this.rBtn_PolRegular.Size = new System.Drawing.Size(103, 17);
            this.rBtn_PolRegular.TabIndex = 1;
            this.rBtn_PolRegular.TabStop = true;
            this.rBtn_PolRegular.Text = "Regular Polygon";
            this.rBtn_PolRegular.UseVisualStyleBackColor = true;
            this.rBtn_PolRegular.CheckedChanged += new System.EventHandler(this.RBtn_PolRegular_CheckedChanged);
            // 
            // rBtn_Random
            // 
            this.rBtn_Random.AutoSize = true;
            this.rBtn_Random.Location = new System.Drawing.Point(518, 54);
            this.rBtn_Random.Name = "rBtn_Random";
            this.rBtn_Random.Size = new System.Drawing.Size(97, 17);
            this.rBtn_Random.TabIndex = 2;
            this.rBtn_Random.TabStop = true;
            this.rBtn_Random.Text = "Random Points";
            this.rBtn_Random.UseVisualStyleBackColor = true;
            this.rBtn_Random.CheckedChanged += new System.EventHandler(this.RBtn_Random_CheckedChanged);
            // 
            // lbl_Seed
            // 
            this.lbl_Seed.AutoSize = true;
            this.lbl_Seed.Location = new System.Drawing.Point(515, 77);
            this.lbl_Seed.Name = "lbl_Seed";
            this.lbl_Seed.Size = new System.Drawing.Size(32, 13);
            this.lbl_Seed.TabIndex = 11;
            this.lbl_Seed.Text = "Seed";
            // 
            // nUpD_Seed
            // 
            this.nUpD_Seed.Location = new System.Drawing.Point(570, 77);
            this.nUpD_Seed.Name = "nUpD_Seed";
            this.nUpD_Seed.Size = new System.Drawing.Size(51, 20);
            this.nUpD_Seed.TabIndex = 3;
            // 
            // BtnTSP
            // 
            this.BtnTSP.Location = new System.Drawing.Point(737, 5);
            this.BtnTSP.Name = "BtnTSP";
            this.BtnTSP.Size = new System.Drawing.Size(100, 40);
            this.BtnTSP.TabIndex = 6;
            this.BtnTSP.Text = "TSP";
            this.BtnTSP.UseVisualStyleBackColor = true;
            this.BtnTSP.Click += new System.EventHandler(this.BtnTSP_Click);
            // 
            // picTela2
            // 
            this.picTela2.Location = new System.Drawing.Point(843, 5);
            this.picTela2.Name = "picTela2";
            this.picTela2.Size = new System.Drawing.Size(500, 500);
            this.picTela2.TabIndex = 12;
            this.picTela2.TabStop = false;
            // 
            // lbl_Pts
            // 
            this.lbl_Pts.AutoSize = true;
            this.lbl_Pts.Location = new System.Drawing.Point(515, 105);
            this.lbl_Pts.Name = "lbl_Pts";
            this.lbl_Pts.Size = new System.Drawing.Size(39, 13);
            this.lbl_Pts.TabIndex = 13;
            this.lbl_Pts.Text = "Points:";
            // 
            // txt_Pts
            // 
            this.txt_Pts.AcceptsReturn = true;
            this.txt_Pts.AllowDrop = true;
            this.txt_Pts.Location = new System.Drawing.Point(518, 121);
            this.txt_Pts.Multiline = true;
            this.txt_Pts.Name = "txt_Pts";
            this.txt_Pts.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_Pts.Size = new System.Drawing.Size(319, 384);
            this.txt_Pts.TabIndex = 14;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1352, 511);
            this.Controls.Add(this.txt_Pts);
            this.Controls.Add(this.lbl_Pts);
            this.Controls.Add(this.picTela2);
            this.Controls.Add(this.BtnTSP);
            this.Controls.Add(this.nUpD_Seed);
            this.Controls.Add(this.lbl_Seed);
            this.Controls.Add(this.rBtn_Random);
            this.Controls.Add(this.rBtn_PolRegular);
            this.Controls.Add(this.BtnCleanPic);
            this.Controls.Add(this.BtnDraw);
            this.Controls.Add(this.nUpD_FMUs);
            this.Controls.Add(this.lbl_numFMU);
            this.Controls.Add(this.picTela);
            this.Name = "Form4";
            this.Text = "Form4";
            ((System.ComponentModel.ISupportInitialize)(this.picTela)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpD_FMUs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpD_Seed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTela2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picTela;
        private System.Windows.Forms.Label lbl_numFMU;
        private System.Windows.Forms.NumericUpDown nUpD_FMUs;
        private System.Windows.Forms.Button BtnDraw;
        private System.Windows.Forms.Button BtnCleanPic;
        private System.Windows.Forms.RadioButton rBtn_PolRegular;
        private System.Windows.Forms.RadioButton rBtn_Random;
        private System.Windows.Forms.Label lbl_Seed;
        private System.Windows.Forms.NumericUpDown nUpD_Seed;
        private System.Windows.Forms.Button BtnTSP;
        private System.Windows.Forms.PictureBox picTela2;
        private System.Windows.Forms.Label lbl_Pts;
        private System.Windows.Forms.TextBox txt_Pts;
    }
}