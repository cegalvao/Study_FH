
namespace Study_FH
{
    partial class Form5
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
            this.lblScenes = new System.Windows.Forms.Label();
            this.cBoxCenarios = new System.Windows.Forms.ComboBox();
            this.picB01 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picB01)).BeginInit();
            this.SuspendLayout();
            // 
            // lblScenes
            // 
            this.lblScenes.AutoSize = true;
            this.lblScenes.Location = new System.Drawing.Point(8, 17);
            this.lblScenes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblScenes.Name = "lblScenes";
            this.lblScenes.Size = new System.Drawing.Size(35, 13);
            this.lblScenes.TabIndex = 69;
            this.lblScenes.Text = "Scen:";
            // 
            // cBoxCenarios
            // 
            this.cBoxCenarios.FormattingEnabled = true;
            this.cBoxCenarios.Items.AddRange(new object[] {
            "t30_a3_dL800_aMax70_aMin10_tFv_s1",
            "t30_a3_dL800_aMax70_aMin10_tFv_s2",
            "t30_a3_dL800_aMax70_aMin10_tFv_s3",
            "t30_a3_dL800_aMax70_aMin10_tFv_s4",
            "t30_a3_dL800_aMax70_aMin10_tFv_s5",
            "t30_a3_dL800_aMax70_aMin10_tFv_s6",
            "t40_a3_dL800_aMax70_aMin10_tFv_s1",
            "t40_a3_dL800_aMax70_aMin10_tFv_s2",
            "t40_a3_dL800_aMax70_aMin10_tFv_s3",
            "t40_a3_dL800_aMax70_aMin10_tFv_s4",
            "t40_a3_dL800_aMax70_aMin10_tFv_s5",
            "t50_a3_dL800_aMax70_aMin10_tFa_s1",
            "t50_a3_dL800_aMax70_aMin10_tFa_s2",
            "t50_a3_dL800_aMax70_aMin10_tFn_s1",
            "t50_a3_dL800_aMax70_aMin10_tFn_s2",
            "t50_a3_dL800_aMax70_aMin10_tFv_s1",
            "t50_a3_dL800_aMax70_aMin10_tFv_s2",
            "t50_a5_dL800_aMax70_aMin10_tFa_s1",
            "t50_a5_dL800_aMax70_aMin10_tFa_s2",
            "t50_a5_dL800_aMax70_aMin10_tFn_s1",
            "t50_a5_dL800_aMax70_aMin10_tFn_s2",
            "t50_a5_dL800_aMax70_aMin10_tFv_s1",
            "t50_a5_dL800_aMax70_aMin10_tFv_s2",
            "t80_a5_dL800_aMax30_aMin10_tFa_s1",
            "t80_a5_dL800_aMax30_aMin10_tFa_s2",
            "t80_a5_dL800_aMax30_aMin10_tFn_s1",
            "t80_a5_dL800_aMax30_aMin10_tFn_s2",
            "t80_a5_dL800_aMax30_aMin10_tFv_s1",
            "t80_a5_dL800_aMax30_aMin10_tFv_s2",
            "t80_a5_dL800_aMax70_aMin10_tFa_s1",
            "t80_a5_dL800_aMax70_aMin10_tFa_s2",
            "t80_a5_dL800_aMax70_aMin10_tFn_s1",
            "t80_a5_dL800_aMax70_aMin10_tFn_s2",
            "t80_a5_dL800_aMax70_aMin10_tFv_s1",
            "t80_a5_dL800_aMax70_aMin10_tFv_s2",
            "t100_a5_dL800_aMax70_aMin10_tFa_s1",
            "t100_a5_dL800_aMax70_aMin10_tFa_s2",
            "t100_a5_dL800_aMax70_aMin10_tFn_s1",
            "t100_a5_dL800_aMax70_aMin10_tFn_s2",
            "t100_a5_dL800_aMax70_aMin10_tFv_s1",
            "t100_a5_dL800_aMax70_aMin10_tFv_s2",
            "Kittaning0",
            "Kitt2",
            "Kittaning4"});
            this.cBoxCenarios.Location = new System.Drawing.Point(49, 11);
            this.cBoxCenarios.Margin = new System.Windows.Forms.Padding(2);
            this.cBoxCenarios.Name = "cBoxCenarios";
            this.cBoxCenarios.Size = new System.Drawing.Size(293, 21);
            this.cBoxCenarios.TabIndex = 70;
            this.cBoxCenarios.SelectedIndexChanged += new System.EventHandler(this.cBoxCenarios_SelectedIndexChanged);
            // 
            // picB01
            // 
            this.picB01.Location = new System.Drawing.Point(49, 37);
            this.picB01.Name = "picB01";
            this.picB01.Size = new System.Drawing.Size(710, 710);
            this.picB01.TabIndex = 71;
            this.picB01.TabStop = false;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 761);
            this.Controls.Add(this.picB01);
            this.Controls.Add(this.lblScenes);
            this.Controls.Add(this.cBoxCenarios);
            this.Name = "Form5";
            this.Text = "Form5";
            ((System.ComponentModel.ISupportInitialize)(this.picB01)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblScenes;
        private System.Windows.Forms.ComboBox cBoxCenarios;
        private System.Windows.Forms.PictureBox picB01;
    }
}