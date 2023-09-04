using TP4.Entidades;

namespace TP4
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
            bool faltaTiempoLimpieza = estaVacio(txt_tiempoLimpieza.Text);
            bool faltaDistribucion = estaVacio(txt_distLlegF.Text) || estaVacio(txt_distLlegHBDesde.Text) || estaVacio(txt_distLlegHBHasta.Text) || estaVacio(txt_distLlegBBDesde.Text) || estaVacio(txt_distLlegBBHasta.Text);
            bool faltaOcupacion = estaVacio(txt_distOcupFDesde.Text) || estaVacio(txt_distOcupFHasta.Text) || estaVacio(txt_distOcupHBDesde.Text) || estaVacio(txt_distOcupHBHasta.Text) || estaVacio(txt_distOcupBBDesde.Text) || estaVacio(txt_distOcupBBHasta.Text);

            return (faltaObligatorio || faltaTiempoLimpieza || faltaDistribucion || faltaOcupacion);
        }

        private bool validarParamsGestor(double inicioImp, int cantidad, double finSim, double limsLlegFutbol, double[] limsLlegHandball, double[] limsLlegBasket, double[] limsOcupFutbol, double[] limsOcupHandball, double[] limsOcupBasket, double tLimpieza)
        {
            ValidadorParametros validadorParametros = new ValidadorParametros();
            bool todosSuperioresACero = validadorParametros.validarSuperiorACero(inicioImp);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(cantidad);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(finSim);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsLlegFutbol);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsLlegHandball[0]);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsLlegHandball[1]);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsLlegBasket[0]);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsLlegBasket[1]);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsOcupFutbol[0]);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsOcupFutbol[1]);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsOcupHandball[0]);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsOcupHandball[1]);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsOcupBasket[0]);
            todosSuperioresACero = todosSuperioresACero || validadorParametros.validarSuperiorACero(limsOcupBasket[1]);

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
            double limsLlegFutbol = Double.Parse(txt_distLlegF.Text) * 60;
            double[] limsLlegHandball = new double[] { Double.Parse(txt_distLlegHBDesde.Text) * 60, Double.Parse(txt_distLlegHBHasta.Text) * 60 };
            double[] limsLlegBasket = new double[] { Double.Parse(txt_distLlegBBDesde.Text) * 60, Double.Parse(txt_distLlegBBHasta.Text) * 60 };
            double[] limsOcupFutbol = new double[] { Double.Parse(txt_distOcupFDesde.Text), Double.Parse(txt_distOcupFHasta.Text) };
            double[] limsOcupHandball = new double[] { Double.Parse(txt_distOcupHBDesde.Text), Double.Parse(txt_distOcupBBHasta.Text) };
            double[] limsOcupBasket = new double[] { Double.Parse(txt_distOcupBBDesde.Text), Double.Parse(txt_distOcupBBHasta.Text) };
            double tLimpieza = Double.Parse(txt_tiempoLimpieza.Text);

            if (!validarParamsGestor(inicioImp, cantidad, finSim, limsLlegFutbol, limsLlegHandball, limsLlegBasket, limsOcupFutbol, limsOcupHandball, limsOcupBasket, tLimpieza))
            {
                MessageBox.Show("Los parametros deben ser superiores a cero!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GestorSimulacion gestorSimulacion = new GestorSimulacion(inicioImp, cantidad, finSim, limsLlegFutbol, limsLlegHandball, limsLlegBasket, limsOcupFutbol, limsOcupHandball, limsOcupBasket, tLimpieza);
            gestorSimulacion.Simular();
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