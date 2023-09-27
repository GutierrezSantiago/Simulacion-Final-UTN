using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.Common;

namespace TP_Final.Entidades
{
    internal class CsvReader
    {
        StreamReader streamReader;

        public CsvReader(string filePath)
        {
            // Instancia el objeto que nos permite escribir en el archivo CSV
            streamReader = new StreamReader(filePath);
        }

        private static double Truncar(double nro)
        {
            return Math.Truncate(nro * 10000) / 10000;
        }

        public double[]? LoadCsvData(DataTable dt)
        {
            // Carga con los numeros contenidos en el CSV una tabla, un array (frecObs) con las frecuencias
            // observadas de cada intervalo y otro array (arrayLimSup) con los limites superiores de cada intervalo

            try
            {
                // Crea la cabecera de la tabla
                #region Headers
                dt.Columns.Add("Nombre de Evento");
                dt.Columns.Add("Reloj");

                // Llegada Vehiculo
                dt.Columns.Add("RND 1 L Vehiculo");
                dt.Columns.Add("RND 2 L Vehiculo");
                dt.Columns.Add("TEL 1 Vehiculo");
                dt.Columns.Add("TEL 2 Vehiculo");
                dt.Columns.Add("Prox L Vehiculo");

                // Llegada Inspector
                dt.Columns.Add("Prox L Inspector");

                // Fin Estacionamiento
                dt.Columns.Add("RND SeDemora");
                dt.Columns.Add("SeDemora");
                dt.Columns.Add("FE Parquimetro 1");
                dt.Columns.Add("FE Parquimetro 2");
                dt.Columns.Add("FE Parquimetro 3");
                dt.Columns.Add("FE Parquimetro 4");
                dt.Columns.Add("FE Parquimetro 5");

                // Fin Turno Estacionamiento
                dt.Columns.Add("FTE Parquimetro 1");
                dt.Columns.Add("FTE Parquimetro 2");
                dt.Columns.Add("FTE Parquimetro 3");
                dt.Columns.Add("FTE Parquimetro 4");
                dt.Columns.Add("FTE Parquimetro 5");

                // Fin Inspeccion
                dt.Columns.Add("Fin Inspeccion");

                // Fin Escritura Boleta
                dt.Columns.Add("Fin Escritura Boleta");

                // Estados Parquimetros
                dt.Columns.Add("Estado Parquimetro 1");
                dt.Columns.Add("Estado Parquimetro 2");
                dt.Columns.Add("Estado Parquimetro 3");
                dt.Columns.Add("Estado Parquimetro 4");
                dt.Columns.Add("Estado Parquimetro 5");

                // Estado Inspector
                dt.Columns.Add("Estado Inspector");

                // Estadísticas
                dt.Columns.Add("Cont. Vehiculos No Encuentran Lugar");
                dt.Columns.Add("Cont. Infracciones Levantadas");

                // Vehiculos
                dt.Columns.Add("Estado Vehiculo 1");
                dt.Columns.Add("Estado Vehiculo 2");
                dt.Columns.Add("Estado Vehiculo 3");
                dt.Columns.Add("Estado Vehiculo 4");
                dt.Columns.Add("Estado Vehiculo 5");
                #endregion

                string[] lineArray = new string[45];

                // Inicializamos las estadísticas para evitar errores

                #region Valores de Calculo
                lineArray[19]= "0"; // T Espera AC Basket
                lineArray[20]= "0"; // T Espera AC Futbol
                lineArray[21] = "0"; // T Espera AC Handball
                lineArray[22] = "0"; // Contador EsperaFinalizada Basket
                lineArray[23] = "0"; // Contador EsperaFinalizada Futbol
                lineArray[24] = "0"; // Contador EsperaFinalizada Handball
                lineArray[25] = "0"; // Cont Llegadas
                lineArray[26] = "0"; // Cont Retirados sin Jugar
                #endregion

                using (streamReader)
                {
                    string? currentLine;
                    // Leer las líneas del archivo y cargar la tabla de datos
                    while ((currentLine = streamReader.ReadLine()) != null)
                    {
                        lineArray = currentLine.Split(';');
                        DataRow lineRow = dt.NewRow();
                        #region Cargar Valores a la Fila
                        // Reloj y Evento
                        lineRow["Nombre de Evento"] = lineArray[0];
                        lineRow["Reloj"] = lineArray[1];

                        // Llegada Vehiculo
                        lineRow["RND 1 L Vehiculo"] = lineArray[2];
                        lineRow["RND 2 L Vehiculo"] = lineArray[3];
                        lineRow["TEL 1 Vehiculo"] = lineArray[4];
                        lineRow["TEL 2 Vehiculo"] = lineArray[5];
                        lineRow["Prox L Vehiculo"] = lineArray[6];

                        // Llegada Inspector
                        lineRow["Prox L Inspector"] = lineArray[7];
                        
                        // Fin Estacionamiento
                        lineRow["RND SeDemora"] = lineArray[8];
                        lineRow["SeDemora"] = lineArray[9];
                        lineRow["FE Parquimetro 1"] = lineArray[10];
                        lineRow["FE Parquimetro 2"] = lineArray[11];
                        lineRow["FE Parquimetro 3"] = lineArray[12];
                        lineRow["FE Parquimetro 4"] = lineArray[13];
                        lineRow["FE Parquimetro 5"] = lineArray[14];

                        // Fin Turno Estacionamiento
                        lineRow["FTE Parquimetro 1"] = lineArray[15];
                        lineRow["FTE Parquimetro 2"] = lineArray[16];
                        lineRow["FTE Parquimetro 3"] = lineArray[17];
                        lineRow["FTE Parquimetro 4"] = lineArray[18];
                        lineRow["FTE Parquimetro 5"] = lineArray[19];

                        // Fin Inspeccion
                        lineRow["Fin Inspeccion"] = lineArray[20];

                        // Fin Escritura Boleta
                        lineRow["Fin Escritura Boleta"] = lineArray[21];

                        // Estados Parquimetros
                        lineRow["Estado Parquimetro 1"] = lineArray[22];
                        lineRow["Estado Parquimetro 2"] = lineArray[23];
                        lineRow["Estado Parquimetro 3"] = lineArray[24];
                        lineRow["Estado Parquimetro 4"] = lineArray[25];
                        lineRow["Estado Parquimetro 5"] = lineArray[26];

                        // Estado Inspector
                        lineRow["Estado Inspector"] = lineArray[27];

                        // Estadisticas
                        lineRow["Cont. Vehiculos No Encuentran Lugar"] = lineArray[28];
                        lineRow["Cont. Infracciones Levantadas"] = lineArray[29];

                        // Vehiculos
                        lineRow["Estado Vehiculo 1"] = lineArray[30];
                        lineRow["Estado Vehiculo 2"] = lineArray[31];
                        lineRow["Estado Vehiculo 3"] = lineArray[32];
                        lineRow["Estado Vehiculo 4"] = lineArray[33];
                        lineRow["Estado Vehiculo 5"] = lineArray[34];
                        #endregion
                        dt.Rows.Add(lineRow);
                    }
                }

                // Se calcula tiempo promedio de espera promedio por cada disciplina deportiva
                double contVehiculosRetirados = double.Parse(lineArray[28]);
                double contInfraccionesLevantadas = double.Parse(lineArray[29]);

                return new double[] { contVehiculosRetirados, contInfraccionesLevantadas };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fallo al cargar datos del CSV: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}