namespace TP4
{
    partial class PantallaSimuladorParquimetros
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_tiempoSimulacion = new System.Windows.Forms.Label();
            this.txt_tiempoSimulacion = new System.Windows.Forms.TextBox();
            this.lbl_unidadTSim = new System.Windows.Forms.Label();
            this.txt_cantIteraciones = new System.Windows.Forms.TextBox();
            this.lbl_cantIteraciones = new System.Windows.Forms.Label();
            this.txt_horaDesde = new System.Windows.Forms.TextBox();
            this.lbl_horaDesde = new System.Windows.Forms.Label();
            this.btn_generar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_tiempoSimulacion
            // 
            this.lbl_tiempoSimulacion.AutoSize = true;
            this.lbl_tiempoSimulacion.Location = new System.Drawing.Point(12, 9);
            this.lbl_tiempoSimulacion.Name = "lbl_tiempoSimulacion";
            this.lbl_tiempoSimulacion.Size = new System.Drawing.Size(124, 15);
            this.lbl_tiempoSimulacion.TabIndex = 0;
            this.lbl_tiempoSimulacion.Text = "Tiempo de simulación";
            // 
            // txt_tiempoSimulacion
            // 
            this.txt_tiempoSimulacion.Location = new System.Drawing.Point(12, 27);
            this.txt_tiempoSimulacion.Name = "txt_tiempoSimulacion";
            this.txt_tiempoSimulacion.Size = new System.Drawing.Size(100, 23);
            this.txt_tiempoSimulacion.TabIndex = 1;
            // 
            // lbl_unidadTSim
            // 
            this.lbl_unidadTSim.AutoSize = true;
            this.lbl_unidadTSim.Location = new System.Drawing.Point(118, 30);
            this.lbl_unidadTSim.Name = "lbl_unidadTSim";
            this.lbl_unidadTSim.Size = new System.Drawing.Size(28, 15);
            this.lbl_unidadTSim.TabIndex = 2;
            this.lbl_unidadTSim.Text = "min";
            // 
            // txt_cantIteraciones
            // 
            this.txt_cantIteraciones.Location = new System.Drawing.Point(12, 80);
            this.txt_cantIteraciones.Name = "txt_cantIteraciones";
            this.txt_cantIteraciones.Size = new System.Drawing.Size(131, 23);
            this.txt_cantIteraciones.TabIndex = 4;
            // 
            // lbl_cantIteraciones
            // 
            this.lbl_cantIteraciones.AutoSize = true;
            this.lbl_cantIteraciones.Location = new System.Drawing.Point(12, 62);
            this.lbl_cantIteraciones.Name = "lbl_cantIteraciones";
            this.lbl_cantIteraciones.Size = new System.Drawing.Size(131, 15);
            this.lbl_cantIteraciones.TabIndex = 3;
            this.lbl_cantIteraciones.Text = "Cantidad de iteraciones";
            // 
            // txt_horaDesde
            // 
            this.txt_horaDesde.Location = new System.Drawing.Point(190, 80);
            this.txt_horaDesde.Name = "txt_horaDesde";
            this.txt_horaDesde.Size = new System.Drawing.Size(131, 23);
            this.txt_horaDesde.TabIndex = 6;
            // 
            // lbl_horaDesde
            // 
            this.lbl_horaDesde.AutoSize = true;
            this.lbl_horaDesde.Location = new System.Drawing.Point(190, 62);
            this.lbl_horaDesde.Name = "lbl_horaDesde";
            this.lbl_horaDesde.Size = new System.Drawing.Size(66, 15);
            this.lbl_horaDesde.TabIndex = 5;
            this.lbl_horaDesde.Text = "Desde hora";
            // 
            // btn_generar
            // 
            this.btn_generar.BackColor = System.Drawing.Color.Red;
            this.btn_generar.Font = new System.Drawing.Font("Snap ITC", 26.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.btn_generar.ForeColor = System.Drawing.Color.Cyan;
            this.btn_generar.Location = new System.Drawing.Point(12, 120);
            this.btn_generar.Name = "btn_generar";
            this.btn_generar.Size = new System.Drawing.Size(456, 94);
            this.btn_generar.TabIndex = 17;
            this.btn_generar.Text = "Generar";
            this.btn_generar.UseVisualStyleBackColor = false;
            // 
            // PantallaSimuladorPolideportivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 231);
            this.Controls.Add(this.btn_generar);
            this.Controls.Add(this.txt_horaDesde);
            this.Controls.Add(this.lbl_horaDesde);
            this.Controls.Add(this.txt_cantIteraciones);
            this.Controls.Add(this.lbl_cantIteraciones);
            this.Controls.Add(this.lbl_unidadTSim);
            this.Controls.Add(this.txt_tiempoSimulacion);
            this.Controls.Add(this.lbl_tiempoSimulacion);
            this.Name = "PantallaSimuladorPolideportivo";
            this.Text = "Simulador Parquímetros";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lbl_tiempoSimulacion;
        private TextBox txt_tiempoSimulacion;
        private Label lbl_unidadTSim;
        private TextBox txt_cantIteraciones;
        private Label lbl_cantIteraciones;
        private TextBox txt_horaDesde;
        private Label lbl_horaDesde;
        private Button btn_generar;
    }
}