
namespace Study_FH
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnAction = new System.Windows.Forms.Button();
            this.picTela = new System.Windows.Forms.PictureBox();
            this.Btn2pts = new System.Windows.Forms.Button();
            this.txtNumPts = new System.Windows.Forms.TextBox();
            this.BtnWrite = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnWriteJSON = new System.Windows.Forms.Button();
            this.BtnCleanPic = new System.Windows.Forms.Button();
            this.BtnRWJson = new System.Windows.Forms.Button();
            this.BtnJsonToMy = new System.Windows.Forms.Button();
            this.BtnWriteModel = new System.Windows.Forms.Button();
            this.BtnForm2 = new System.Windows.Forms.Button();
            this.BtnForm3 = new System.Windows.Forms.Button();
            this.BtnForm4 = new System.Windows.Forms.Button();
            this.BtnForm5 = new System.Windows.Forms.Button();
            this.BtnForm6 = new System.Windows.Forms.Button();
            this.BtnForm7 = new System.Windows.Forms.Button();
            this.btnRInstance = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picTela)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnAction
            // 
            this.BtnAction.Location = new System.Drawing.Point(736, 9);
            this.BtnAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnAction.Name = "BtnAction";
            this.BtnAction.Size = new System.Drawing.Size(103, 46);
            this.BtnAction.TabIndex = 3;
            this.BtnAction.Text = "Ler Instancia e Desenhar";
            this.BtnAction.UseVisualStyleBackColor = true;
            this.BtnAction.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // picTela
            // 
            this.picTela.Location = new System.Drawing.Point(18, 46);
            this.picTela.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picTela.Name = "picTela";
            this.picTela.Size = new System.Drawing.Size(700, 577);
            this.picTela.TabIndex = 3;
            this.picTela.TabStop = false;
            // 
            // Btn2pts
            // 
            this.Btn2pts.Location = new System.Drawing.Point(736, 117);
            this.Btn2pts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Btn2pts.Name = "Btn2pts";
            this.Btn2pts.Size = new System.Drawing.Size(103, 46);
            this.Btn2pts.TabIndex = 5;
            this.Btn2pts.Text = "Desenhar Pontos";
            this.Btn2pts.UseVisualStyleBackColor = true;
            this.Btn2pts.Click += new System.EventHandler(this.Btn2pts_Click);
            // 
            // txtNumPts
            // 
            this.txtNumPts.Location = new System.Drawing.Point(79, 10);
            this.txtNumPts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNumPts.Name = "txtNumPts";
            this.txtNumPts.Size = new System.Drawing.Size(638, 23);
            this.txtNumPts.TabIndex = 0;
            this.txtNumPts.Text = "gdb";
            // 
            // BtnWrite
            // 
            this.BtnWrite.Location = new System.Drawing.Point(736, 62);
            this.BtnWrite.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnWrite.Name = "BtnWrite";
            this.BtnWrite.Size = new System.Drawing.Size(103, 46);
            this.BtnWrite.TabIndex = 4;
            this.BtnWrite.Text = "Ler Inst. e Escrever .dat";
            this.BtnWrite.UseVisualStyleBackColor = true;
            this.BtnWrite.Click += new System.EventHandler(this.BtnWrite_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "parametro";
            // 
            // BtnWriteJSON
            // 
            this.BtnWriteJSON.Location = new System.Drawing.Point(736, 171);
            this.BtnWriteJSON.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnWriteJSON.Name = "BtnWriteJSON";
            this.BtnWriteJSON.Size = new System.Drawing.Size(103, 46);
            this.BtnWriteJSON.TabIndex = 6;
            this.BtnWriteJSON.Text = "Ler Inst e Escr. JSON";
            this.BtnWriteJSON.UseVisualStyleBackColor = true;
            this.BtnWriteJSON.Click += new System.EventHandler(this.BtnWriteJSON_Click);
            // 
            // BtnCleanPic
            // 
            this.BtnCleanPic.Location = new System.Drawing.Point(736, 332);
            this.BtnCleanPic.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnCleanPic.Name = "BtnCleanPic";
            this.BtnCleanPic.Size = new System.Drawing.Size(103, 46);
            this.BtnCleanPic.TabIndex = 10;
            this.BtnCleanPic.Text = "Limpar Tela";
            this.BtnCleanPic.UseVisualStyleBackColor = true;
            this.BtnCleanPic.Click += new System.EventHandler(this.BtnCleanPic_Click);
            // 
            // BtnRWJson
            // 
            this.BtnRWJson.Location = new System.Drawing.Point(736, 224);
            this.BtnRWJson.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnRWJson.Name = "BtnRWJson";
            this.BtnRWJson.Size = new System.Drawing.Size(103, 46);
            this.BtnRWJson.TabIndex = 7;
            this.BtnRWJson.Text = "Ler JSON e Escr JSON";
            this.BtnRWJson.UseVisualStyleBackColor = true;
            this.BtnRWJson.Click += new System.EventHandler(this.BtnRWJson_Click);
            // 
            // BtnJsonToMy
            // 
            this.BtnJsonToMy.Location = new System.Drawing.Point(736, 278);
            this.BtnJsonToMy.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnJsonToMy.Name = "BtnJsonToMy";
            this.BtnJsonToMy.Size = new System.Drawing.Size(103, 46);
            this.BtnJsonToMy.TabIndex = 8;
            this.BtnJsonToMy.Text = "Ler JSON e  Escr .dat";
            this.BtnJsonToMy.UseVisualStyleBackColor = true;
            this.BtnJsonToMy.Click += new System.EventHandler(this.BtnJsonToMy_Click);
            // 
            // BtnWriteModel
            // 
            this.BtnWriteModel.Location = new System.Drawing.Point(736, 387);
            this.BtnWriteModel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnWriteModel.Name = "BtnWriteModel";
            this.BtnWriteModel.Size = new System.Drawing.Size(103, 46);
            this.BtnWriteModel.TabIndex = 11;
            this.BtnWriteModel.Text = "Ler e Escrever Modelo";
            this.BtnWriteModel.UseVisualStyleBackColor = true;
            this.BtnWriteModel.Click += new System.EventHandler(this.BtnWriteModel_Click);
            // 
            // BtnForm2
            // 
            this.BtnForm2.Location = new System.Drawing.Point(846, 10);
            this.BtnForm2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnForm2.Name = "BtnForm2";
            this.BtnForm2.Size = new System.Drawing.Size(103, 46);
            this.BtnForm2.TabIndex = 12;
            this.BtnForm2.Text = "CARP FH";
            this.BtnForm2.UseVisualStyleBackColor = true;
            this.BtnForm2.Click += new System.EventHandler(this.BtnForm2_Click);
            // 
            // BtnForm3
            // 
            this.BtnForm3.Location = new System.Drawing.Point(846, 63);
            this.BtnForm3.Margin = new System.Windows.Forms.Padding(2);
            this.BtnForm3.Name = "BtnForm3";
            this.BtnForm3.Size = new System.Drawing.Size(103, 46);
            this.BtnForm3.TabIndex = 13;
            this.BtnForm3.Text = "Solve from .MPS";
            this.BtnForm3.UseVisualStyleBackColor = true;
            this.BtnForm3.Click += new System.EventHandler(this.BtnForm3_Click);
            // 
            // BtnForm4
            // 
            this.BtnForm4.Location = new System.Drawing.Point(846, 117);
            this.BtnForm4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnForm4.Name = "BtnForm4";
            this.BtnForm4.Size = new System.Drawing.Size(103, 46);
            this.BtnForm4.TabIndex = 14;
            this.BtnForm4.Text = "Form4 - Geom";
            this.BtnForm4.UseVisualStyleBackColor = true;
            this.BtnForm4.Click += new System.EventHandler(this.BtnForm4_Click);
            // 
            // BtnForm5
            // 
            this.BtnForm5.Location = new System.Drawing.Point(846, 171);
            this.BtnForm5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnForm5.Name = "BtnForm5";
            this.BtnForm5.Size = new System.Drawing.Size(103, 46);
            this.BtnForm5.TabIndex = 15;
            this.BtnForm5.Text = "Form5 - Des. cenarios";
            this.BtnForm5.UseVisualStyleBackColor = true;
            this.BtnForm5.Click += new System.EventHandler(this.BtnForm5_Click);
            // 
            // BtnForm6
            // 
            this.BtnForm6.Location = new System.Drawing.Point(846, 224);
            this.BtnForm6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnForm6.Name = "BtnForm6";
            this.BtnForm6.Size = new System.Drawing.Size(103, 46);
            this.BtnForm6.TabIndex = 16;
            this.BtnForm6.Text = "Form6 - Data picture";
            this.BtnForm6.UseVisualStyleBackColor = true;
            this.BtnForm6.Click += new System.EventHandler(this.BtnForm6_Click);
            // 
            // BtnForm7
            // 
            this.BtnForm7.Location = new System.Drawing.Point(846, 324);
            this.BtnForm7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnForm7.Name = "BtnForm7";
            this.BtnForm7.Size = new System.Drawing.Size(103, 46);
            this.BtnForm7.TabIndex = 99;
            this.BtnForm7.Text = "Form7 - ";
            this.BtnForm7.UseVisualStyleBackColor = true;
            this.BtnForm7.Click += new System.EventHandler(this.BtnForm7_Click);
            // 
            // btnRInstance
            // 
            this.btnRInstance.Location = new System.Drawing.Point(846, 278);
            this.btnRInstance.Name = "btnRInstance";
            this.btnRInstance.Size = new System.Drawing.Size(103, 46);
            this.btnRInstance.TabIndex = 17;
            this.btnRInstance.Text = "R Instance";
            this.btnRInstance.UseVisualStyleBackColor = true;
            this.btnRInstance.Click += new System.EventHandler(this.BtnRInstance_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 635);
            this.Controls.Add(this.btnRInstance);
            this.Controls.Add(this.BtnForm7);
            this.Controls.Add(this.BtnForm6);
            this.Controls.Add(this.BtnForm5);
            this.Controls.Add(this.BtnForm4);
            this.Controls.Add(this.BtnForm3);
            this.Controls.Add(this.BtnForm2);
            this.Controls.Add(this.BtnWriteModel);
            this.Controls.Add(this.BtnJsonToMy);
            this.Controls.Add(this.BtnRWJson);
            this.Controls.Add(this.BtnCleanPic);
            this.Controls.Add(this.BtnWriteJSON);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnWrite);
            this.Controls.Add(this.txtNumPts);
            this.Controls.Add(this.Btn2pts);
            this.Controls.Add(this.picTela);
            this.Controls.Add(this.BtnAction);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picTela)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnAction;
        private System.Windows.Forms.PictureBox picTela;
        private System.Windows.Forms.Button Btn2pts;
        private System.Windows.Forms.TextBox txtNumPts;
        private System.Windows.Forms.Button BtnWrite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnWriteJSON;
        private System.Windows.Forms.Button BtnCleanPic;
        private System.Windows.Forms.Button BtnRWJson;
        private System.Windows.Forms.Button BtnJsonToMy;
        private System.Windows.Forms.Button BtnWriteModel;
        private System.Windows.Forms.Button BtnForm2;
        private System.Windows.Forms.Button BtnForm3;
        private System.Windows.Forms.Button BtnForm4;
        private System.Windows.Forms.Button BtnForm5;
        private System.Windows.Forms.Button BtnForm6;
        private System.Windows.Forms.Button BtnForm7;
        private System.Windows.Forms.Button btnRInstance;
    }
}

