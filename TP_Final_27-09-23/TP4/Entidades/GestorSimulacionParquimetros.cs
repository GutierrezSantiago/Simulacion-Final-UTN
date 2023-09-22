using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace TP_Final.Entidades
{
    internal class GestorSimulacionParquimetros
    {
        public string Datos = @"./datos.csv";
        private Estado[] EstadosVehiculo = { new Estado("Estacionado"), new Estado("Siendo Multado"), new Estado("Finalizo") };
        private Estado[] EstadosParquimetro = { new Estado("Libre"), new Estado("Ocupado"), new Estado("En Infracción") };
        private Estado[] EstadosInspector = { new Estado("Ausente"), new Estado("Inspeccionando 1"), new Estado("Inspeccionando 2"), new Estado("Inspeccionando 3"), new Estado("Inspeccionando 4"), new Estado("Inspeccionando 5"), new Estado("Escribiendo Boleta 1"), new Estado("Escribiendo Boleta 2"), new Estado("Escribiendo Boleta 3"), new Estado("Escribiendo Boleta 4"), new Estado("Escribiendo Boleta 5") };

        private string[] Eventos = { "Llegada Vehiculo", "Llegada Inspector", "Fin de Estacionamiento 1", "Fin de Estacionamiento 2", "Fin de Estacionamiento 3", "Fin de Estacionamiento 4", "Fin de Estacionamiento 5", "Fin de Turno de Estacionamiento 1", "Fin de Turno de Estacionamiento 2", "Fin de Turno de Estacionamiento 3", "Fin de Turno de Estacionamiento 4", "Fin de Turno de Estacionamiento 5", "Fin de Inspeccion", "Fin de Escritura de Boleta", "Fin de Simulación" };
                                       // LV, LI, FE1, FE2, FE3, FE4, FE5, FT1, FT2, FT3, FT4, FT5, FI, FB, FinSim
        private double[] tDeEventos; // mismo orden que vector arriba
        #region Propiedades
        
        private double[] distVehiculo;
        private double distInspector;
        private double distFinTurnoEstacionamiento;
        private double[] distFinEstacionamiento;
        private double distFinInspeccion;
        private double distFinEscrituraBoleta;

        private double InicioImp;
        private int Iteraciones;

        private Parquimetro[] Parquimetros;
        private Inspector Inspector;
        private Vehiculo?[] VehiculosEnSistema;

        Random RndVehiculo1;
        Random RndVehiculo2;
        Random RndDemora;


        #endregion
        #region Constructor
        public GestorSimulacionParquimetros(double inicioImp, int Cantidad, double FinSim, double mediaVehiculo, double desviacionVehiculo)
        {
            tDeEventos = new double[15];
            tDeEventos[14] = FinSim;
            InicioImp = inicioImp;
            Iteraciones = Cantidad;

            //Generamos los parquimetros
            Parquimetros = new Parquimetro[5];

            Parquimetros[0] = new Parquimetro(EstadosParquimetro[0]);
            Parquimetros[1] = new Parquimetro(EstadosParquimetro[0]);
            Parquimetros[2] = new Parquimetro(EstadosParquimetro[0]);
            Parquimetros[3] = new Parquimetro(EstadosParquimetro[0]);
            Parquimetros[4] = new Parquimetro(EstadosParquimetro[0]);

            //Generamos el inspector
            Inspector = new Inspector(EstadosInspector[0]);

            //Generamos el espacio para vehiculos
            VehiculosEnSistema = new Vehiculo[5];

            //Generamos los RNDs
            RndVehiculo1 = new Random(Guid.NewGuid().GetHashCode());
            RndVehiculo2 = new Random(Guid.NewGuid().GetHashCode());
            RndDemora = new Random(Guid.NewGuid().GetHashCode());

            // Seteamos las distribuciones
            distVehiculo = new double[2] {mediaVehiculo, desviacionVehiculo};
            distInspector = 120;
            distFinTurnoEstacionamiento = 60;
            distFinEstacionamiento = new double[2] {60, 70};
            distFinInspeccion = 1;
            distFinEscrituraBoleta = 5;


        }
        #endregion
        #region Calculos
        // Seccion para calcular tiempos de eventos a partir de un rnd siguiendo su distribucion correspondiente
        public int getIndiceParquimetro(string nombreEstadoInspector) 
        {
            int indiceParquimetro = -1;
            for (int i = 0; i < EstadosInspector.Length; i++)
            {
                if (EstadosInspector[i].GetNombre() == nombreEstadoInspector)
                {
                    indiceParquimetro = i;
                    break;
                }
            }
            if (indiceParquimetro < 6)
            {
                indiceParquimetro = indiceParquimetro - 1;
            } else
            {
                indiceParquimetro = indiceParquimetro - 6;
            }
            return indiceParquimetro;
        }
        public double[] CalcularLlegadaVehiculo(double rnd1, double rnd2) {
            return GeneradorNros.Normal(distVehiculo[0], distVehiculo[1],rnd1, rnd2);
         }
        public double CalcularLlegadaInspector() {
            return distInspector;
         }
        public double CalcularFinTurnoEstacionamiento() {
            return distFinTurnoEstacionamiento;
         }

        public double CalcularFinEstacionamiento(double rnd)
        {
            if (rnd < 0.8)
            {
                return distFinEstacionamiento[0];
            } else
            {
                return distFinEstacionamiento[1];
            }
        }
        public string SeDemoraEstacionamiento(double rnd)
        {
            if (rnd < 0.8)
            {
                return "NO";
            }
            else
            {
                return "SI";
            }
        }
    

        public double CalcularFinInspeccion()
        {
            return distFinInspeccion;
        }
        public double CalcularFinEscrituraBoleta()
        {
            return distFinEscrituraBoleta;
        }
        #endregion

        #region Calculo de Estadisticas
        //QUITAR LUEGO
        #endregion

        #region Utilidades de Escritura
        private void EscribirVehiculosVectorEstado(string[] linea)
        {
            int puntero = 30;
            for(int i = 0; i<VehiculosEnSistema.Length; i++)
            {
                if (VehiculosEnSistema[i] == null)
                {
                    linea[puntero] = "";
                }
                else
                {
                    linea[puntero] = VehiculosEnSistema[i].getNombreEstado();
                }

                puntero += 1;
            }
        }

        private static void BorrarColumnasVector(string[] linea, int[] columnasABorrar) 
        {
            foreach(int i in columnasABorrar)
            {
                linea[i] = "";
            }
        }
        #endregion

        #region Llegada
        private void GenerarProximaLlegadaVehiculo(string[] linea, double[] relojYEvento)
        {
            double rnd1;
            double rnd2;
            double[] tEntreLlegadas;
            if (linea[5] == "" || linea[5] == null) 
            { 
                if (linea[4]== "" || linea[4] == null) // Llegada Vehiculo
                {
                    rnd1 = RndVehiculo1.NextDouble(); // Generamos rnds
                    rnd2 = RndVehiculo2.NextDouble();
                    tEntreLlegadas = CalcularLlegadaVehiculo(rnd1, rnd2); // Calculamos t entre Llegadas
                    tDeEventos[0] = tEntreLlegadas[0] + relojYEvento[0]; // calculamos t de proxima llegada

                    // asignamos los valores a la linea:
                    linea[2] = GeneradorNros.Truncar(rnd1).ToString();
                    linea[3] = GeneradorNros.Truncar(rnd2).ToString();
                    linea[4] = tEntreLlegadas[0].ToString();
                    linea[5] = tEntreLlegadas[1].ToString();
                    linea[6] = GeneradorNros.Truncar(tDeEventos[0]).ToString(); //usamos el primer resultado de boxmuller
                    return;
                }
                 //usamos el segundo resultado de boxmuller
            } else
            {   
                tDeEventos[0] = double.Parse(linea[5]) + relojYEvento[0];
                linea[6] = GeneradorNros.Truncar(tDeEventos[0]).ToString();
            }
             
        }

        private void GenerarProximaLlegadaInspector(string[] linea, double[] relojYEvento)
        {
            double tEntreLlegadas = CalcularLlegadaInspector();
            tDeEventos[1] = tEntreLlegadas + relojYEvento[0];
            linea[7] = tDeEventos[1].ToString();
        }

        private int HayParquimetroLibre()
        {
            for (int i = 0; i < Parquimetros.Length; i++)
            {
                if (Parquimetros[i].EstaLibre())
                {
                    return i;
                }
            }
            return -1;
        }

        private void AsignarVehiculoAVectorEstado(Vehiculo vec, int indiceParquimetro)
        {
            VehiculosEnSistema[indiceParquimetro] = vec;
        }

        private string[] LlegadaVehiculo(string[] lineaAnt, double[] relojYEvento)
        {
            string[] linea = lineaAnt;

            // seteamos la linea en vacio del t_entre_llegadas que se haya usado
            if (linea[4] == "" && linea[5] != "")
            {
                linea[5] = "";
            }
            if (linea[4] != "")
            {
                linea[4] = "";
            }
            
           
            // generamos proxima llegada y la anotamos en el vector estado 

            GenerarProximaLlegadaVehiculo(linea, relojYEvento);
            int indiceParquimetro = this.HayParquimetroLibre();
            if (indiceParquimetro == -1) // si hay menos de 5 en cola, creamos un nuevo deportista segun quien llego
            {
                int retirados = int.Parse(linea[28]) + 1;
                linea[28] = retirados.ToString();
                return linea;
            }
            else
            {
                Vehiculo vec = new Vehiculo(EstadosVehiculo[0]); // se crea un nuevo deportista con estado en espera


                GenerarEstacionamientoParquimetro(linea, relojYEvento[0], vec, indiceParquimetro); // generamos ocupacion y la cargamos al vector estado (cambiando estado de Dep a JUGANDO y de Cancha a OCUPADA
                

            }

            
            EscribirVehiculosVectorEstado(linea);
            return linea;
        }

        private string[] LlegadaInspector(string[] lineaAnt, double[] relojYEvento)
        {
            string[] linea = lineaAnt;

            GenerarProximaLlegadaInspector(linea, relojYEvento); // generamos proxima llegada y la anotamos en el vector estado
            for (int i = 0; i < Parquimetros.Length; i++)
            {
                if (!Parquimetros[i].EstaLibre())
                {
                    GenerarInspeccionVehiculo(linea, relojYEvento[0], i);
                    break; //Empieza la primera inspección, se corrobora el resto de parquimetros luego de finalizar la inspección del primero
                }
            }
            return linea;
        }
        #endregion

        #region Estacionamiento-Inspeccion-EscrituraBoleta
        private void GenerarEstacionamientoParquimetro(string[] linea, double reloj, Vehiculo vec, int indiceParquimetro)
        {
 
            // LV, LI, FE1, FE2, FE3, FE4, FE5, FT1, FT2, FT3, FT4, FT5, FI, FB, FinSim
            
            // generamos los datos de la ocupacion
            double rnd = GeneradorNros.Truncar(RndDemora.NextDouble());
            double tOcup = CalcularFinEstacionamiento(rnd);
            tDeEventos[2+indiceParquimetro] = GeneradorNros.Truncar(tOcup + reloj); // cargamos al vector de eventos (en el indice 2 comienzan los fin estacionamiento)
            tDeEventos[7 + indiceParquimetro] = CalcularFinTurnoEstacionamiento() + reloj;// cargamos al vector de eventos (en el indice 7 comienzan los fin estacionamiento)

            // cargamos al vector estado
            linea[8] = rnd.ToString();
            linea[9] = this.SeDemoraEstacionamiento(rnd);
            linea[10+indiceParquimetro] = tDeEventos[2 + indiceParquimetro].ToString();
            linea[15+indiceParquimetro] = tDeEventos[7 + indiceParquimetro].ToString();

            AsignarVehiculoAVectorEstado(vec, indiceParquimetro); // en cualquier caso, lo sumamos al vector estado
            Parquimetros[indiceParquimetro].SetEstado(EstadosParquimetro[1]); // cambiamos el estado de la cancha a ocupada
            linea[22+indiceParquimetro] = Parquimetros[indiceParquimetro].getNombreEstado(); // cambiamos la linea para mostrar estado ocupado
        }

        private void GenerarInspeccionVehiculo(string[] linea, double reloj, int indiceParquimetro)
        {
            // generamos los datos de la inspeccion
            double tInspecc = CalcularFinInspeccion();
            tDeEventos[12] = tInspecc + reloj;

            //cargamos el vector estado
            linea[20] = tDeEventos[12].ToString();

            //Cambiamos el estado de inspector
            Estado estado = EstadosInspector[1 + indiceParquimetro]; //Obtenemos el subestado de Inspeccionando (numero de parquimetro)
            Inspector.SetEstado(estado);

            //cargamos el vector estado
            linea[27] = Inspector.getNombreEstado();
            EscribirVehiculosVectorEstado(linea);
        }
        private void GenerarFinEscrituraBoleta(string[] linea, double reloj, int indiceParquimetro)
        {
            // generamos los datos de la escritra de boleta
            double tEscritura = CalcularFinEscrituraBoleta();
            tDeEventos[13] = tEscritura + reloj;

            //cargamos el vector estado
            linea[21] = tDeEventos[13].ToString();

            //Cambiamos el estado de inspector
            Estado estado = EstadosInspector[6 + indiceParquimetro]; //Obtenemos el subestado de Inspeccionando (numero de parquimetro)
            Inspector.SetEstado(estado);

            VehiculosEnSistema[indiceParquimetro].SetEstado(EstadosVehiculo[1]); // Estado a siendo multado

            //cargamos el vector estado
            linea[27] = Inspector.getNombreEstado();
            EscribirVehiculosVectorEstado(linea);
        }
        private string[] FinEstacionamiento(string[] lineaAnt, double reloj, int indiceParquimetro)
        {
            string[] linea = lineaAnt;

            //Corroboramos que no este siendo multado
            if (VehiculosEnSistema[indiceParquimetro].getNombreEstado() == EstadosVehiculo[0].GetNombre())
            {

                // limpiamos el vector estado y el tDeEventos de fin estacionamiento
                linea[10 + indiceParquimetro] = "";
                tDeEventos[2+indiceParquimetro] = (double)Int32.MaxValue;

                // limpiamos el vector estado y el tDeEventos de fin de turno de estacionamiento
                // en la proxima linea (flujo normal)

                VehiculosEnSistema[indiceParquimetro] = null;

                // cambiamos el parquimetro a libre
                Parquimetros[indiceParquimetro].SetEstado(EstadosParquimetro[0]);
                linea[22 + indiceParquimetro] = Parquimetros[indiceParquimetro].getNombreEstado();

                // SE VA MIENTRAS INSPECCION CASO 1 : SE MANEJA DESDE ACA,
                // CANCELAMOS FIN INSPECCION O LO ADELANTAMOS
                // Si se esta inspeccionando se cancela el finInspeccion

                // PARA CASO 2: SE ADELANTA LA FINALIZACION
                if (Inspector.getNombreEstado() == EstadosInspector[1+indiceParquimetro].GetNombre()) 
                { 
                    tDeEventos[12] = reloj + 0.000001;
                    linea[20] = tDeEventos[12].ToString();
                }


            } else  // Esta siendo multado
            {
                // SE ESTABLECE EL FIN DE ESTACIONAMIENTO APENAS TERMINA DE MULTAR
                tDeEventos[2 + indiceParquimetro] = tDeEventos[13] + 0.00000000001;
            }

            EscribirVehiculosVectorEstado(linea);
            return linea;
        }

        private string[] FinTurnoEstacionamiento(string[] lineaAnt, double reloj, int indiceParquimetro)
        {
            string[] linea = lineaAnt;

            //Corroboramos que no este siendo multado

            // limpiamos el vector estado y el tDeEventos de fin de turno de estacionamiento
            linea[15 + indiceParquimetro] = "";
            tDeEventos[7 + indiceParquimetro] = (double)Int32.MaxValue;

            // cambiamos el parquimetro a En infraccion
            if (linea[9] == "SI")
            {
                Parquimetros[indiceParquimetro].SetEstado(EstadosParquimetro[2]);
                linea[22 + indiceParquimetro] = Parquimetros[indiceParquimetro].getNombreEstado();
            }
            

            

            return linea;
        }

        // NO ESTA HECHO (IGULA A FIN ESTACIONAMIENTO Y FALTA SI ESTA DEMORADO GENERAR ESCRIBIR BOLETA
        private string[] FinInspeccion(string[] lineaAnt, double reloj, int indiceParquimetro)
        {
            string[] linea = lineaAnt;

            if (linea[10+indiceParquimetro] == "" || linea[10 + indiceParquimetro] == null)
            {
                //nos fijamos en el resto de parquimetros is hay algun auto estacionado
                for (int i = indiceParquimetro+1; i < Parquimetros.Length; i++)
                {
                    //nos fijamos en el resto de parquimetros is hay algun auto estacionado
                    if (!Parquimetros[i].EstaLibre())
                    {
                        GenerarInspeccionVehiculo(linea, reloj, i);
                        EscribirVehiculosVectorEstado(linea); // actualizamos el vector estado con el vehiculo
                        return linea;
                    }
                }
            }

            // SE VA ANTES DEL FIN INSPECCION, CASO 2: SE MANEJA DESDE ACA, IF NULL BLA BLA.
            if (VehiculosEnSistema[indiceParquimetro] == null)
            {
                for (int i = indiceParquimetro + 1; i < Parquimetros.Length; i++)
                {
                    //nos fijamos en el resto de parquimetros is hay algun auto estacionado
                    if (!Parquimetros[i].EstaLibre())
                    {
                        GenerarInspeccionVehiculo(linea, reloj, i);
                        EscribirVehiculosVectorEstado(linea); // actualizamos el vector estado con el vehiculo
                        return linea;
                    }
                }
                
            }

            //Corroboramos si hay que multarlo
            if (VehiculosEnSistema[indiceParquimetro] != null && Parquimetros[indiceParquimetro].getNombreEstado() == "En Infracción" )
            {
                VehiculosEnSistema[indiceParquimetro].SetEstado(EstadosVehiculo[1]); // Estado a siendo multado

                Inspector.SetEstado(EstadosInspector[6 + indiceParquimetro]); // Obtenemos el subestado de Inspeccionando (numero de parquimetro)

                linea[27] = Inspector.getNombreEstado(); // escribimos en el vector estado
                //ACA ME QUEDE

                // limpiamos el vector estado y el tDeEventos de fin inspeccion
                linea[20] = "";
                tDeEventos[12] = (double)Int32.MaxValue;

                GenerarFinEscrituraBoleta(linea, reloj, indiceParquimetro);
                EscribirVehiculosVectorEstado(linea); // actualizamos el vector estado con el vehiculo
                return linea;

            } else
            {
                //nos fijamos en el resto de parquimetros is hay algun auto estacionado
                for (int i = indiceParquimetro+1; i < Parquimetros.Length; i++)
                {
                    //nos fijamos en el resto de parquimetros is hay algun auto estacionado
                    if (!Parquimetros[i].EstaLibre())
                    {
                        GenerarInspeccionVehiculo(linea, reloj, i);
                        EscribirVehiculosVectorEstado(linea); // actualizamos el vector estado con el vehiculo
                        return linea;
                    }
                }
            }

            //se va el inspector
            Inspector.SetEstado(EstadosInspector[0]);
            linea[27] = Inspector.getNombreEstado();
            linea[20] = "";
            tDeEventos[12] = (double)Int32.MaxValue;

            //se pasa el auto a estacionado si no se retiro antes del fin inspecccion
            if (VehiculosEnSistema[indiceParquimetro] != null) { VehiculosEnSistema[indiceParquimetro].SetEstado(EstadosVehiculo[0]); }
            

            EscribirVehiculosVectorEstado(linea);// actualizamos el vector estado con el vehiculo

            return linea;

            
        }

        private string[] FinEscrituraBoleta(string[] lineaAnt, double reloj, int indiceParquimetro)
        {
            string[] linea = lineaAnt;

            // Aumentamos en 1 la cantidad de infracciones levantadas
            int cantidadInfracciones = Int32.Parse(linea[29]) + 1;
            linea[29] = cantidadInfracciones.ToString();

            //nos fijamos en el resto de parquimetros is hay algun auto estacionado
            for (int i = indiceParquimetro+1; i < Parquimetros.Length; i++)
            {
                if (!Parquimetros[i].EstaLibre())
                {
                    GenerarInspeccionVehiculo(linea, reloj, i);
                    return linea; // Continua con los parquimetros
                }
            }

            linea[21] = "";
            tDeEventos[13] = (double)Int32.MaxValue;
            //se va el inspector
            Inspector.SetEstado(EstadosInspector[0]);
            linea[27] = Inspector.getNombreEstado();

            //se pasa el auto a estacionado
            VehiculosEnSistema[indiceParquimetro].SetEstado(EstadosVehiculo[0]);
            EscribirVehiculosVectorEstado(linea);

            return linea;
        }
        #endregion

        public void Simular()
        {
            #region Inicializacion
            // Para escribir archivo CSV
            StreamWriter CSVWriter = new StreamWriter(Datos);
            string[] lineaAnt = new string[35];
            string[] linea = new string[35];

            // Reloj inicial
            double reloj = 0;
            lineaAnt[0] = "Inicializacion";
            lineaAnt[1] = reloj.ToString();

            // Llegada Vehiculo
            //double rnd1 = RndVehiculo1.NextDouble();
            //double rnd2 = RndVehiculo2.NextDouble();
            //lineaAnt[2] = GeneradorNros.Truncar(rnd1).ToString();
            //lineaAnt[3] = GeneradorNros.Truncar(rnd2).ToString();

            //double[] t_entre_llegadas = CalcularLlegadaVehiculo(rnd1, rnd2);
            //tDeEventos[0] = t_entre_llegadas[0];
            //lineaAnt[3] = tDeEventos[0].ToString();
            //lineaAnt[4] = t_entre_llegadas[1].ToString();
            GenerarProximaLlegadaVehiculo(lineaAnt, new double[] {0, 0});

            // Llegada Inspector
            tDeEventos[1] = CalcularLlegadaInspector();
            lineaAnt[7] = (tDeEventos[1] + reloj).ToString();

            // Fin Turno de Estacionamiento y Estacionamiento Parquimetros : Inicio no hay nada
            tDeEventos[2] = (double)Int32.MaxValue;
            tDeEventos[3] = (double)Int32.MaxValue;
            tDeEventos[4] = (double)Int32.MaxValue;
            tDeEventos[5] = (double)Int32.MaxValue;
            tDeEventos[6] = (double)Int32.MaxValue;
            tDeEventos[7] = (double)Int32.MaxValue;
            tDeEventos[8] = (double)Int32.MaxValue;
            tDeEventos[9] = (double)Int32.MaxValue;
            tDeEventos[10] = (double)Int32.MaxValue;
            tDeEventos[11] = (double)Int32.MaxValue;

            // Iniciar estado Parquimetros
            lineaAnt[22] = Parquimetros[0].getNombreEstado();
            lineaAnt[23] = Parquimetros[1].getNombreEstado();
            lineaAnt[24] = Parquimetros[2].getNombreEstado();
            lineaAnt[25] = Parquimetros[3].getNombreEstado();
            lineaAnt[26] = Parquimetros[4].getNombreEstado();

            //Iniciar estado Inspector
            lineaAnt[27] = Inspector.getNombreEstado();

            // Fin de Inspeccion : Inicio no esta inspeccionando
            tDeEventos[12] = (double)Int32.MaxValue;

            //Fin de escritura boleta : Inicio no esta escribiendo boleta
            tDeEventos[13] = (double)Int32.MaxValue;

            // Variables estadisticas

            // Contador Vehiculos Retirados
            double contVehiculosRetirados = 0;
            lineaAnt[28] = contVehiculosRetirados.ToString();

            // Contador Infracciones Levantadas por Inspector
            double contInfraccionesLevantadas = 0;
            lineaAnt[29] = contInfraccionesLevantadas.ToString();

            #endregion

            double[] relojYEvento;
            int contadorIteraciones = 0;
            string impresion;
            int nroIteraciones = 0;

            while (true)
            {
                relojYEvento = EventHandler.ProximoEvento(tDeEventos);

                if (relojYEvento[0] >= InicioImp && contadorIteraciones < Iteraciones)
                {
                    if (contadorIteraciones == 0)
                    {
                        lineaAnt[0] = lineaAnt[0] + " - I.I.";
                        lineaAnt[1] = InicioImp.ToString();
                    }
                    impresion = string.Join(";", lineaAnt);
                    CSVWriter.WriteLine(impresion);
                    contadorIteraciones++;
                }

                // AAA  ACORDATE DE MODIFICAR ESTO
                BorrarColumnasVector(linea, new int[] { 2, 3, 8 });
                BorrarColumnasVector(lineaAnt, new int[] { 2, 3, 8 });

                // LV, LI, FE1, FE2, FE3, FE4, FE5, FT1, FT2, FT3, FT4, FT5, FI, FB, FinSim
                if (relojYEvento[1] == 0) { linea = LlegadaVehiculo(lineaAnt, relojYEvento); }
                else if (relojYEvento[1] == 1) { linea = LlegadaInspector(lineaAnt, relojYEvento); }
                else if (relojYEvento[1] == 7) { linea = FinTurnoEstacionamiento(lineaAnt, relojYEvento[0], 0); }
                else if (relojYEvento[1] == 8) { linea = FinTurnoEstacionamiento(lineaAnt, relojYEvento[0], 1); }
                else if (relojYEvento[1] == 9) { linea = FinTurnoEstacionamiento(lineaAnt, relojYEvento[0], 2); }
                else if (relojYEvento[1] == 10) { linea = FinTurnoEstacionamiento(lineaAnt, relojYEvento[0], 3); }
                else if (relojYEvento[1] == 11) { linea = FinTurnoEstacionamiento(lineaAnt, relojYEvento[0], 4); }
                else if (relojYEvento[1] == 2) { linea = FinEstacionamiento(lineaAnt, relojYEvento[0], 0); }
                else if (relojYEvento[1] == 3) { linea = FinEstacionamiento(lineaAnt, relojYEvento[0], 1); }
                else if (relojYEvento[1] == 4) { linea = FinEstacionamiento(lineaAnt, relojYEvento[0], 2); }
                else if (relojYEvento[1] == 5) { linea = FinEstacionamiento(lineaAnt, relojYEvento[0], 3); }
                else if (relojYEvento[1] == 6) { linea = FinEstacionamiento(lineaAnt, relojYEvento[0], 4); }
                else if (relojYEvento[1] == 12) // SE HACE MAS QUE LLAMAR AL FIN PORQUE HAY QUE CONSEGUIR EL INDICE DE PARQUIMETRO
                {
                    int indiceParquimetro = getIndiceParquimetro(linea[27]);
                    linea = FinInspeccion(lineaAnt, relojYEvento[0], indiceParquimetro); 
                }
                else if (relojYEvento[1] == 13) // SE HACE MAS QUE LLAMAR AL FIN PORQUE HAY QUE CONSEGUIR EL INDICE DE PARQUIMETRO
                {
                    int indiceParquimetro = getIndiceParquimetro(linea[27]);
                    linea = FinEscrituraBoleta(lineaAnt, relojYEvento[0], indiceParquimetro); 
                }
                else 
                    
                { // FIN DE SIMULACION

                    string[] blank = new string[35];
                    impresion = string.Join(";", blank);
                    CSVWriter.WriteLine(impresion); // escribimos linea en blanco para mejor visualizacion
                    nroIteraciones++;
                    linea[0] = Eventos[(int)relojYEvento[1]] + " " + nroIteraciones.ToString();
                    linea[1] = tDeEventos[14].ToString();
                    impresion = string.Join(";", linea);
                    CSVWriter.WriteLine(impresion); // escribimos linea fin de simulacion
                    break;

                }

                // Escribimos datos identificatorios del evento actual
                nroIteraciones++;
                linea[0] = Eventos[(int)relojYEvento[1]] + " " + nroIteraciones.ToString();
                linea[1] = GeneradorNros.Truncar(relojYEvento[0]).ToString();
                
                lineaAnt = linea; // guardamos la linea anterior antes de la proxima iteracion
                
            }

            CSVWriter.Close();
        }
    }
}
