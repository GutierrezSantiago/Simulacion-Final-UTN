using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP4.Entidades;

namespace TP4
{
    public partial class PantallaVisualizacion : Form
    {
        DataTable CSV;
        CsvReader CSVReader;
        public PantallaVisualizacion(string _archivoCSV)
        {
            // Inicializa los componentes de la pantalla e instancia
            InitializeComponent();
            CSVReader = new CsvReader(_archivoCSV);
            CSV = new DataTable();
        }

        private void PantallaVisualizacion_Load(object sender, EventArgs e)
        {
            //Carga la grilla con los contenidos del CSV

            // aca metodo CSV reader supongo que devuelve los 4 valores a cargar (tPromedios por cada uno, porc que retira)
            double[] resultados = CSVReader.LoadCsvData(CSV);
            gdw_iteracionesSolicitadas.DataSource = CSV;

            // Estetico Columnas Tabla
            foreach (DataGridViewColumn columna in gdw_iteracionesSolicitadas.Columns)
            {
                columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // Cargamos la probabilidad

            txt_tiempoPromEsperaBB.Text = resultados[0].ToString();
            txt_tiempoPromEsperaF.Text = resultados[1].ToString();
            txt_tiempoPromEsperaHB.Text = resultados[2].ToString();
            txt_porcRetiraSinJugar.Text = resultados[3].ToString();

        }
    }
}
