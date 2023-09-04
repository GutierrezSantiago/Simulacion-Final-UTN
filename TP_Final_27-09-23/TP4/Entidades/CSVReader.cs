using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.Common;

namespace TP4.Entidades
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

                // Deportistas 2-10
                dt.Columns.Add("RND L Basket");
                dt.Columns.Add("TEL Basket");
                dt.Columns.Add("Prox L Basket");
                dt.Columns.Add("RND L Handball");
                dt.Columns.Add("TEL Handball");
                dt.Columns.Add("Prox L Handball");
                dt.Columns.Add("RND L Futbol");
                dt.Columns.Add("TEL Futbol");
                dt.Columns.Add("Prox L Futbol");

                // Servidor: Cancha
                dt.Columns.Add("Quien Juega");
                dt.Columns.Add("RND DuracionJugada");
                dt.Columns.Add("DuracionJugada");
                dt.Columns.Add("Fin de Jugada");
                dt.Columns.Add("Estado Cancha");
                dt.Columns.Add("Tam Cola");
                dt.Columns.Add("TE Limpieza");
                dt.Columns.Add("Fin Limpieza");

                // Estadísticas
                dt.Columns.Add("T Espera AC Basket");
                dt.Columns.Add("T Espera AC Futbol");
                dt.Columns.Add("T Espera AC Handball");
                dt.Columns.Add("Contador EsperaFinalizada Basket");
                dt.Columns.Add("Contador EsperaFinalizada Futbol");
                dt.Columns.Add("Contador EsperaFinalizada Handball");
                dt.Columns.Add("Cont Llegadas");
                dt.Columns.Add("Cont Retirados sin Jugar");

                // Grupos
                dt.Columns.Add("Estado G1");
                dt.Columns.Add("Tiempo de Llegada G1");
                dt.Columns.Add("Disciplina G1");
                dt.Columns.Add("Estado G2");
                dt.Columns.Add("Tiempo de Llegada G2");
                dt.Columns.Add("Disciplina G2");
                dt.Columns.Add("Estado G3");
                dt.Columns.Add("Tiempo de Llegada G3");
                dt.Columns.Add("Disciplina G3");
                dt.Columns.Add("Estado G4");
                dt.Columns.Add("Tiempo de Llegada G4");
                dt.Columns.Add("Disciplina G4");
                dt.Columns.Add("Estado G5");
                dt.Columns.Add("Tiempo de Llegada G5");
                dt.Columns.Add("Disciplina G5");
                dt.Columns.Add("Estado G6");
                dt.Columns.Add("Tiempo de Llegada G6");
                dt.Columns.Add("Disciplina G6");
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

                        lineRow["Nombre de Evento"] = lineArray[0];
                        lineRow["Reloj"] = lineArray[1];

                        lineRow["RND L Basket"] = lineArray[2];
                        lineRow["TEL Basket"] = lineArray[3];
                        lineRow["Prox L Basket"] = lineArray[4];
                        
                        lineRow["RND L Futbol"] = lineArray[5];
                        lineRow["TEL Futbol"] = lineArray[6];
                        lineRow["Prox L Futbol"] = lineArray[7];
                        
                        lineRow["RND L Handball"] = lineArray[8];
                        lineRow["TEL Handball"] = lineArray[9];
                        lineRow["Prox L Handball"] = lineArray[10];


                        lineRow["Quien Juega"] = lineArray[11];
                        lineRow["RND DuracionJugada"] = lineArray[12];
                        lineRow["DuracionJugada"] = lineArray[13];
                        lineRow["Fin de Jugada"] = lineArray[14];

                        lineRow["Estado Cancha"] = lineArray[15];
                        lineRow["Tam Cola"] = lineArray[16];
                        lineRow["TE Limpieza"] = lineArray[17];
                        lineRow["Fin Limpieza"] = lineArray[18];


                        lineRow["T Espera AC Basket"] = lineArray[19];
                        lineRow["T Espera AC Futbol"] = lineArray[20];
                        lineRow["T Espera AC Handball"] = lineArray[21];
                        lineRow["Contador EsperaFinalizada Basket"] = lineArray[22];
                        lineRow["Contador EsperaFinalizada Futbol"] = lineArray[23];
                        lineRow["Contador EsperaFinalizada Handball"] = lineArray[24];

                        lineRow["Cont Llegadas"] = lineArray[25];
                        lineRow["Cont Retirados sin Jugar"] = lineArray[26];


                        lineRow["Estado G1"] = lineArray[27];
                        lineRow["Tiempo de Llegada G1"] = lineArray[28];
                        lineRow["Disciplina G1"] = lineArray[29];
                        lineRow["Estado G2"] = lineArray[30];
                        lineRow["Tiempo de Llegada G2"] = lineArray[31];
                        lineRow["Disciplina G2"] = lineArray[32];
                        lineRow["Estado G3"] = lineArray[33];
                        lineRow["Tiempo de Llegada G3"] = lineArray[34];
                        lineRow["Disciplina G3"] = lineArray[35];
                        lineRow["Estado G4"] = lineArray[36];
                        lineRow["Tiempo de Llegada G4"] = lineArray[37];
                        lineRow["Disciplina G4"] = lineArray[38];
                        lineRow["Estado G5"] = lineArray[39];
                        lineRow["Tiempo de Llegada G5"] = lineArray[40];
                        lineRow["Disciplina G5"] = lineArray[41];
                        lineRow["Estado G6"] = lineArray[42];
                        lineRow["Tiempo de Llegada G6"] = lineArray[43];
                        lineRow["Disciplina G6"] = lineArray[44];

                        #endregion
                        dt.Rows.Add(lineRow);
                    }
                }

                // Se calcula tiempo promedio de espera promedio por cada disciplina deportiva
                double tEsperaB = double.Parse(lineArray[19]);
                double tEsperaF = double.Parse(lineArray[20]);
                double tEsperaH = double.Parse(lineArray[21]);

                double contEfB = double.Parse(lineArray[22]);
                double contEfF = double.Parse(lineArray[23]);
                double contEfH = double.Parse(lineArray[24]);

                double contLlegadas = double.Parse(lineArray[25]);
                double contRSJ = double.Parse(lineArray[26]);

                double promedioB = Truncar(tEsperaB / contEfB);
                double promedioF = Truncar(tEsperaF / contEfF);
                double promedioH = Truncar(tEsperaH / contEfH);
                double porcRSJ = Truncar((contRSJ/contLlegadas)*10000)/100;

                return new double[] { promedioB, promedioF, promedioH, porcRSJ };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fallo al cargar datos del CSV: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}