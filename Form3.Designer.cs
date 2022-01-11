
namespace Study_FH
{
    partial class Form3
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnChoiceFileMPS = new System.Windows.Forms.Button();
            this.TxtFileNameMPS = new System.Windows.Forms.TextBox();
            this.btnSolve = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnChoseFolder = new System.Windows.Forms.Button();
            this.listBox_Files = new System.Windows.Forms.ListBox();
            this.btnSolveSelecteds = new System.Windows.Forms.Button();
            this.TxtFileNameMST = new System.Windows.Forms.TextBox();
            this.btnChoiceFileMST = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.chkShowMess = new System.Windows.Forms.CheckBox();
            this.btnSubstring = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // btnChoiceFileMPS
            // 
            this.btnChoiceFileMPS.Location = new System.Drawing.Point(29, 46);
            this.btnChoiceFileMPS.Margin = new System.Windows.Forms.Padding(2);
            this.btnChoiceFileMPS.Name = "btnChoiceFileMPS";
            this.btnChoiceFileMPS.Size = new System.Drawing.Size(132, 24);
            this.btnChoiceFileMPS.TabIndex = 0;
            this.btnChoiceFileMPS.Text = "Escolher Arquivo MPS";
            this.btnChoiceFileMPS.UseVisualStyleBackColor = true;
            this.btnChoiceFileMPS.Click += new System.EventHandler(this.btnChoiceFileMPS_Click);
            // 
            // TxtFileNameMPS
            // 
            this.TxtFileNameMPS.Location = new System.Drawing.Point(165, 49);
            this.TxtFileNameMPS.Margin = new System.Windows.Forms.Padding(2);
            this.TxtFileNameMPS.Name = "TxtFileNameMPS";
            this.TxtFileNameMPS.Size = new System.Drawing.Size(679, 20);
            this.TxtFileNameMPS.TabIndex = 1;
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(29, 107);
            this.btnSolve.Margin = new System.Windows.Forms.Padding(2);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(405, 25);
            this.btnSolve.TabIndex = 2;
            this.btnSolve.Text = "Solve From File MPS";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.FolderBrowserDialog1_HelpRequest);
            // 
            // btnChoseFolder
            // 
            this.btnChoseFolder.Location = new System.Drawing.Point(29, 151);
            this.btnChoseFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btnChoseFolder.Name = "btnChoseFolder";
            this.btnChoseFolder.Size = new System.Drawing.Size(100, 24);
            this.btnChoseFolder.TabIndex = 4;
            this.btnChoseFolder.Text = "Escolher Pastas";
            this.btnChoseFolder.UseVisualStyleBackColor = true;
            this.btnChoseFolder.Click += new System.EventHandler(this.btnChoseFolder_Click);
            // 
            // listBox_Files
            // 
            this.listBox_Files.FormattingEnabled = true;
            this.listBox_Files.HorizontalScrollbar = true;
            this.listBox_Files.Location = new System.Drawing.Point(29, 181);
            this.listBox_Files.Margin = new System.Windows.Forms.Padding(2);
            this.listBox_Files.Name = "listBox_Files";
            this.listBox_Files.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_Files.Size = new System.Drawing.Size(815, 147);
            this.listBox_Files.TabIndex = 5;
            // 
            // btnSolveSelecteds
            // 
            this.btnSolveSelecteds.Location = new System.Drawing.Point(29, 333);
            this.btnSolveSelecteds.Margin = new System.Windows.Forms.Padding(2);
            this.btnSolveSelecteds.Name = "btnSolveSelecteds";
            this.btnSolveSelecteds.Size = new System.Drawing.Size(814, 25);
            this.btnSolveSelecteds.TabIndex = 6;
            this.btnSolveSelecteds.Text = "Solve Selected MPS";
            this.btnSolveSelecteds.UseVisualStyleBackColor = true;
            this.btnSolveSelecteds.Click += new System.EventHandler(this.btnSolveSelecteds_Click);
            // 
            // TxtFileNameMST
            // 
            this.TxtFileNameMST.Location = new System.Drawing.Point(165, 77);
            this.TxtFileNameMST.Margin = new System.Windows.Forms.Padding(2);
            this.TxtFileNameMST.Name = "TxtFileNameMST";
            this.TxtFileNameMST.Size = new System.Drawing.Size(679, 20);
            this.TxtFileNameMST.TabIndex = 8;
            // 
            // btnChoiceFileMST
            // 
            this.btnChoiceFileMST.Location = new System.Drawing.Point(29, 74);
            this.btnChoiceFileMST.Margin = new System.Windows.Forms.Padding(2);
            this.btnChoiceFileMST.Name = "btnChoiceFileMST";
            this.btnChoiceFileMST.Size = new System.Drawing.Size(132, 24);
            this.btnChoiceFileMST.TabIndex = 7;
            this.btnChoiceFileMST.Text = "Escolher Arquivo MST";
            this.btnChoiceFileMST.UseVisualStyleBackColor = true;
            this.btnChoiceFileMST.Click += new System.EventHandler(this.btnChoiceFileMST_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // chkShowMess
            // 
            this.chkShowMess.AutoSize = true;
            this.chkShowMess.Location = new System.Drawing.Point(29, 24);
            this.chkShowMess.Name = "chkShowMess";
            this.chkShowMess.Size = new System.Drawing.Size(104, 17);
            this.chkShowMess.TabIndex = 9;
            this.chkShowMess.Text = "Show Messages";
            this.chkShowMess.UseVisualStyleBackColor = true;
            // 
            // btnSubstring
            // 
            this.btnSubstring.Location = new System.Drawing.Point(438, 107);
            this.btnSubstring.Name = "btnSubstring";
            this.btnSubstring.Size = new System.Drawing.Size(405, 25);
            this.btnSubstring.TabIndex = 10;
            this.btnSubstring.Text = "Substring";
            this.btnSubstring.UseVisualStyleBackColor = true;
            this.btnSubstring.Click += new System.EventHandler(this.btnSubstring_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 443);
            this.Controls.Add(this.btnSubstring);
            this.Controls.Add(this.chkShowMess);
            this.Controls.Add(this.TxtFileNameMST);
            this.Controls.Add(this.btnChoiceFileMST);
            this.Controls.Add(this.btnSolveSelecteds);
            this.Controls.Add(this.listBox_Files);
            this.Controls.Add(this.btnChoseFolder);
            this.Controls.Add(this.btnSolve);
            this.Controls.Add(this.TxtFileNameMPS);
            this.Controls.Add(this.btnChoiceFileMPS);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnChoiceFileMPS;
        private System.Windows.Forms.TextBox TxtFileNameMPS;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnChoseFolder;
        private System.Windows.Forms.ListBox listBox_Files;
        private System.Windows.Forms.Button btnSolveSelecteds;
        private System.Windows.Forms.TextBox TxtFileNameMST;
        private System.Windows.Forms.Button btnChoiceFileMST;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.CheckBox chkShowMess;
        private System.Windows.Forms.Button btnSubstring;
    }
}