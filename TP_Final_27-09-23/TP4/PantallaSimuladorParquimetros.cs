using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP_Final.Entidades
{
    public partial class PantallaSimuladorParquimetros : Form
    {
        ValidadorParametros validadorParametros;
        public PantallaSimuladorParquimetros()
        {
            InitializeComponent();
            validadorParametros = new ValidadorParametros();
        }
        private bool estaVacio(string texto)
        {
            return texto == "";
        }

        private bool faltanParams()
        {
            bool faltaObligatorio = estaVacio(txt_tiempoSimulacion.Text) || estaVacio(txt_cantIteraciones.Text) || estaVacio(txt_horaDesde.Text);

            return (faltaObligatorio);
        }

        private bool validarParamsGestor(double inicioImp, int cantidad, double finSim)
        {
            ValidadorParametros validadorParametros = new ValidadorParametros();
            bool todosSuperioresACero = validadorParametros.validarSuperiorACero(inicioImp);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(cantidad);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(finSim);

            return todosSuperioresACero;
        }
        private void btn_generar_Click(object sender, EventArgs e)
        {
            if (faltanParams())
            {
                MessageBox.Show("Faltan parametros!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double inicioImp = Double.Parse(txt_horaDesde.Text) * 60;
            int cantidad = Int32.Parse(txt_cantIteraciones.Text);
            double finSim = Double.Parse(txt_tiempoSimulacion.Text);


            if (!validarParamsGestor(inicioImp, cantidad, finSim))
            {
                MessageBox.Show("Los parametros deben ser superiores a cero!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GestorSimulacionParquimetros gestorSimulacion = new GestorSimulacionParquimetros(inicioImp, cantidad, finSim, 7, 1);
            gestorSimulacion.Simular();
            MessageBox.Show("Listo rey");
            PantallaVisualizacion pantallaVisualizacion = new PantallaVisualizacion(gestorSimulacion.Datos);
            pantallaVisualizacion.ShowDialog();

        }

        private void txt_DoubleParam_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo digitos, caracteres de control y un solo punto decimal o signo menos
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != ',' || ((TextBox)sender).Text.Contains(",")) &&
                (e.KeyChar != '-' || ((TextBox)sender).Text.Length != 0))
            {
                e.Handled = true;
            }
        }

        private void txt_IntParam_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo digitos y caracteres de control 
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
