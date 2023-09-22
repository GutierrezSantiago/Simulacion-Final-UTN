namespace TP_Final
{
    partial class PantallaVisualizacion
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
            this.gdw_iteracionesSolicitadas = new System.Windows.Forms.DataGridView();
            this.txt_vehiculosRetirados = new System.Windows.Forms.TextBox();
            this.lbl_vehiculosRetirados = new System.Windows.Forms.Label();
            this.lbl_infracciones = new System.Windows.Forms.Label();
            this.txt_infracciones = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gdw_iteracionesSolicitadas)).BeginInit();
            this.SuspendLayout();
            // 
            // gdw_iteracionesSolicitadas
            // 
            this.gdw_iteracionesSolicitadas.AllowUserToAddRows = false;
            this.gdw_iteracionesSolicitadas.AllowUserToDeleteRows = false;
            this.gdw_iteracionesSolicitadas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdw_iteracionesSolicitadas.Location = new System.Drawing.Point(9, 9);
            this.gdw_iteracionesSolicitadas.Name = "gdw_iteracionesSolicitadas";
            this.gdw_iteracionesSolicitadas.ReadOnly = true;
            this.gdw_iteracionesSolicitadas.RowHeadersWidth = 51;
            this.gdw_iteracionesSolicitadas.RowTemplate.Height = 25;
            this.gdw_iteracionesSolicitadas.Size = new System.Drawing.Size(1163, 444);
            this.gdw_iteracionesSolicitadas.TabIndex = 0;
            // 
            // txt_vehiculosRetirados
            // 
            this.txt_vehiculosRetirados.Location = new System.Drawing.Point(1072, 459);
            this.txt_vehiculosRetirados.Name = "txt_vehiculosRetirados";
            this.txt_vehiculosRetirados.Size = new System.Drawing.Size(100, 23);
            this.txt_vehiculosRetirados.TabIndex = 1;
            // 
            // lbl_vehiculosRetirados
            // 
            this.lbl_vehiculosRetirados.AutoSize = true;
            this.lbl_vehiculosRetirados.Location = new System.Drawing.Point(960, 462);
            this.lbl_vehiculosRetirados.Name = "lbl_vehiculosRetirados";
            this.lbl_vehiculosRetirados.Size = new System.Drawing.Size(106, 15);
            this.lbl_vehiculosRetirados.TabIndex = 2;
            this.lbl_vehiculosRetirados.Text = "Vehiculos retirados";
            // 
            // lbl_infracciones
            // 
            this.lbl_infracciones.AutoSize = true;
            this.lbl_infracciones.Location = new System.Drawing.Point(995, 491);
            this.lbl_infracciones.Name = "lbl_infracciones";
            this.lbl_infracciones.Size = new System.Drawing.Size(71, 15);
            this.lbl_infracciones.TabIndex = 4;
            this.lbl_infracciones.Text = "Infracciones";
            // 
            // txt_infracciones
            // 
            this.txt_infracciones.Location = new System.Drawing.Point(1072, 488);
            this.txt_infracciones.Name = "txt_infracciones";
            this.txt_infracciones.Size = new System.Drawing.Size(100, 23);
            this.txt_infracciones.TabIndex = 3;
            // 
            // PantallaVisualizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 520);
            this.Controls.Add(this.lbl_infracciones);
            this.Controls.Add(this.txt_infracciones);
            this.Controls.Add(this.lbl_vehiculosRetirados);
            this.Controls.Add(this.txt_vehiculosRetirados);
            this.Controls.Add(this.gdw_iteracionesSolicitadas);
            this.Name = "PantallaVisualizacion";
            this.Text = "Visualización de Simulación Parquímetros";
            ((System.ComponentModel.ISupportInitialize)(this.gdw_iteracionesSolicitadas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView gdw_iteracionesSolicitadas;
        private TextBox txt_vehiculosRetirados;
        private Label lbl_vehiculosRetirados;
        private Label lbl_infracciones;
        private TextBox txt_infracciones;
    }
}