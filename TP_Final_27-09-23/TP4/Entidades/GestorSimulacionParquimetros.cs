using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace TP_Final.Entidades
{
    internal class GestorSimulacion
    {
        public string Datos = @"./datos.csv";
        private Estado[] EstadosVehiculo = { new Estado("Estacionado"), new Estado("Siendo Multado"), new Estado("Finalizo") };
        private Estado[] EstadosParquimetro = { new Estado("Libre"), new Estado("Ocupado") };
        private Estado[] EstadosInspector = { new Estado("Ausente"), new Estado("Inspeccionando 1"), new Estado("Inspeccionando 2"), new Estado("Inspeccionando 3"), new Estado("Inspeccionando 4"), new Estado("Inspeccionando 5"), new Estado("Escribiendo Boleta 1"), new Estado("Escribiendo Boleta 2"), new Estado("Escribiendo Boleta 3"), new Estado("Escribiendo Boleta 4"), new Estado("Escribiendo Boleta 5") };

        private string[] Eventos = { "Llegada Vehiculo", "Llegada Inspector", "Fin de Estacionamiento 1", "Fin de Estacionamiento 2", "Fin de Estacionamiento 3", "Fin de Estacionamiento 4", "Fin de Estacionamiento 5", "Fin de Turno de Estacionamiento 1", "Fin de Turno de Estacionamiento 2", "Fin de Turno de Estacionamiento 3", "Fin de Turno de Estacionamiento 4", "Fin de Turno de Estacionamiento 5", "Fin de Inspeccion", "Fin de Escritura de Boleta" };
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
        public GestorSimulacion(double inicioImp, int Cantidad, double FinSim, double mediaVehiculo, double desviacionVehiculo)
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
            if (linea[4] == "") 
            { 
                if (linea[5]=="") // Llegada Vehiculo
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
                tDeEventos[0] = double.Parse(linea[5]) + relojYEvento[0];
                linea[6] = GeneradorNros.Truncar(tDeEventos[0]).ToString(); //usamos el segundo resultado de boxmuller
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
                if (Parquimetros[0].EstaLibre())
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

            GenerarProximaLlegadaVehiculo(linea, relojYEvento); // generamos proxima llegada y la anotamos en el vector estado
            int indiceParquimetro = this.HayParquimetroLibre();
            if (indiceParquimetro == -1) // si hay menos de 5 en cola, creamos un nuevo deportista segun quien llego
            {
                int retirados = int.Parse(linea[28]) + 1;
                linea[26] = retirados.ToString();

            }
            else
            {
                Vehiculo vec = new Vehiculo(EstadosVehiculo[0]); // se crea un nuevo deportista con estado en espera


                GenerarEstacionamientoParquimetro(linea, relojYEvento[0], vec, indiceParquimetro); // generamos ocupacion y la cargamos al vector estado (cambiando estado de Dep a JUGANDO y de Cancha a OCUPADA
                

                AsignarVehiculoAVectorEstado(vec, indiceParquimetro); // en cualquier caso, lo sumamos al vector estado
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

        #region Ocupacion
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
        }
        private int BuscarDeportistaJugando(string[] linea)
        {
            int jugando = -1;

            for(int i = 0; i < DeportistasEnSistema.Length; i++)
            {
                if (DeportistasEnSistema[i] == null) continue;
                else if (DeportistasEnSistema[i].estaJugando())
                {
                    jugando = i; 
                    break;
                }
            }

            return jugando;
        }

        private int BuscarProximoDeportistaAJugar(double reloj)
        {
            int proximo = -1;
            double t = reloj;

            for (int i = 0; i < DeportistasEnSistema.Length; i++)
            {
                if (DeportistasEnSistema[i] == null) continue;
                else if (DeportistasEnSistema[i].getTiempoLleg() < t && DeportistasEnSistema[i].getNombreEstado() == "En Espera")
                {
                    if (proximo == -1) 
                    {
                        proximo = i; // proximo = primero encontrado
                        t = DeportistasEnSistema[i].getTiempoLleg();
                    }
                    else if (DeportistasEnSistema[proximo].GetPrioridad() <= DeportistasEnSistema[i].GetPrioridad()) // si se encuentra otro, se compara prioridad
                    {
                        proximo = i;
                        t = DeportistasEnSistema[i].getTiempoLleg();
                    }
                }
            }

            return proximo;
        }
        
        private string[] FinEstacionamiento(string[] lineaAnt, double reloj, int indiceParquimetro)
        {
            string[] linea = lineaAnt;

            //Corroboramos que no este siendo multado
            if (VehiculosEnSistema[indiceParquimetro].GetEstado() == EstadosVehiculo[0])
            {

                // limpiamos el vector estado y el tDeEventos de fin estacionamiento
                linea[10 + indiceParquimetro] = "";
                tDeEventos[2+indiceParquimetro] = (double)Int32.MaxValue;

                // limpiamos el vector estado y el tDeEventos de fin de turno de estacionamiento
                linea[15 + indiceParquimetro] = "";
                tDeEventos[7 + indiceParquimetro] = (double)Int32.MaxValue;

                VehiculosEnSistema[indiceParquimetro] = null;

                // cambiamos el parquimetro a libre
                Parquimetros[indiceParquimetro].SetEstado(EstadosParquimetro[0]);
                linea[22 + indiceParquimetro] = Parquimetros[indiceParquimetro].getNombreEstado();

            } else  // Esta siendo multado
            {
                if (tDeEventos[13] == (double)Int32.MaxValue) // SI FINALIZO LA ESCRITUA DE BOLETA
                {
                    // SE VA EL VEHICULO

                    // limpiamos el vector estado y el tDeEventos de fin estacionamiento
                    linea[10 + indiceParquimetro] = "";
                    tDeEventos[2 + indiceParquimetro] = (double)Int32.MaxValue;

                    // limpiamos el vector estado y el tDeEventos de fin de turno de estacionamiento
                    linea[15 + indiceParquimetro] = "";
                    tDeEventos[7 + indiceParquimetro] = (double)Int32.MaxValue;

                    VehiculosEnSistema[indiceParquimetro] = null;

                    // cambiamos el parquimetro a libre
                    Parquimetros[indiceParquimetro].SetEstado(EstadosParquimetro[0]);
                    linea[22 + indiceParquimetro] = Parquimetros[indiceParquimetro].getNombreEstado();
                } else
                {
                    // SE ESTABLECE EL FIN DE ESTACIONAMIENTO APENAS TERMINA DE MULTAR
                    tDeEventos[2 + indiceParquimetro] = tDeEventos[13] + 0.00000000001;
                }
                
            }
            // METER ACA CUANDO ESTA SIENDO MULTADO

            EscribirVehiculosVectorEstado(linea);
            return linea;
        }

        // NO ESTA HECHO (IGULA A FIN ESTACIONAMIENTO Y FALTA SI ESTA DEMORADO GENERAR ESCRIBIR BOLETA
        private string[] FinInspeccion(string[] lineaAnt, double reloj, int indiceParquimetro)
        {
            string[] linea = lineaAnt;

            double finTurnoEstacionamiento = double.Parse(linea[15+indiceParquimetro]);

            //Corroboramos si hay que multarlo
            if (reloj > finTurnoEstacionamiento)
            {
                VehiculosEnSistema[indiceParquimetro].SetEstado(EstadosVehiculo[1]); // Estado a siendo multado

                Inspector.SetEstado(EstadosInspector[6 + indiceParquimetro]); // Obtenemos el subestado de Inspeccionando (numero de parquimetro)

                linea[27] = Inspector.getNombreEstado(); // escribimos en el vector estado
                //ACA ME QUEDE

                // limpiamos el vector estado y el tDeEventos de fin inspeccion
                linea[20] = "";
                tDeEventos[12] = (double)Int32.MaxValue;

                // ACA GENERAMOS EL FIN DE ESCRITURA BOLETA (ESCRIBIR METODO)
            }

            // EN UN ELSE EMPEZAR OTRA INSPECCION SI NO HAY QUE MULTAR, SINO EN FIN MULTA TAMBIEN SE CORROBORA ESTO Y SE HACE OTRA INSPECCION


            EscribirVehiculosVectorEstado(linea); // actualizamos el vector estado con el vehiculo
            return linea;
        }

        #endregion

        #region Limpieza
        private void GenerarLimpiezaCancha(string[] linea, double reloj)
        {
            tDeEventos[4] = TLimpieza + reloj; // Calculamos fin de Limpieza

            // agregamos datos al vector estado
            linea[17] = TLimpieza.ToString(); 
            linea[18] = (tDeEventos[4]).ToString();

            Cancha.SetEstado(EstadosCancha[2]); // cambiamos el estado de la cancha a LIMPIANDO
            linea[15] = Cancha.getNombreEstado(); // cambiamos la linea para mostrar estado LIMPIANDO
        }

        private string[] FinLimpieza(string[] lineaAnt, double reloj)
        {
            string[] linea = lineaAnt;

            // borramos la limpieza que ocurrio
            linea[18] = "";
            tDeEventos[4] = (double)Int32.MaxValue;

            if(Cancha.getTamCola() == 0) // si no hay nadie en cola
            {
                Cancha.SetEstado(EstadosCancha[0]); // cambiamos el estado de la cancha a LIBRE
                linea[15] = Cancha.getNombreEstado(); // actualizamos estado en vector estado
            }
            else // hay deportistas en cola
            {
                // generamos una ocupacion
                int proximoAJugar = BuscarProximoDeportistaAJugar(reloj);
                GenerarOcupacionCancha(linea, reloj, DeportistasEnSistema[proximoAJugar]);

                Cancha.RestarACola(); // Restamos 1 a la cola
                linea[16] = Cancha.getTamCola().ToString(); // actualizamos el tam cola en el vector estado
            }

            EscribirDeportistasVectorEstado(linea);
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
            double rnd1 = RndVehiculo1.NextDouble();
            double rnd2 = RndVehiculo2.NextDouble();
            lineaAnt[2] = GeneradorNros.Truncar(rnd1).ToString();
            lineaAnt[3] = GeneradorNros.Truncar(rnd2).ToString();

            double[] t_entre_llegadas = CalcularLlegadaVehiculo(rnd1, rnd2);
            tDeEventos[0] = t_entre_llegadas[0];
            lineaAnt[3] = tDeEventos[0].ToString();
            lineaAnt[4] = t_entre_llegadas[1].ToString();

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
            lineaAnt[30] = contInfraccionesLevantadas.ToString();

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

                BorrarColumnasVector(linea, new int[] { 2, 3, 5, 6, 8, 9, 11, 12, 13, 17 });
                BorrarColumnasVector(lineaAnt, new int[] { 2, 3, 5, 6, 8, 9, 11, 12, 13, 17 });

                // LV, LI, FE1, FE2, FE3, FE4, FE5, FT1, FT2, FT3, FT4, FT5, FI, FB, FinSim
                if (relojYEvento[1] == 0) { linea = LlegadaVehiculo(lineaAnt, relojYEvento); }
                else if (relojYEvento[1] == 1) { linea = LlegadaInspector(lineaAnt, relojYEvento); }
                else if (relojYEvento[1] == 3 ) { linea = FinEstacionamiento(lineaAnt, relojYEvento[0], 3); }
                else if (relojYEvento[1] == 4) { linea = FinLimpieza(lineaAnt, relojYEvento[0]); }
                else { // FIN DE SIMULACION
                    string[] blank = new string[45];
                    impresion = string.Join(";", blank);
                    CSVWriter.WriteLine(impresion); // escribimos linea en blanco para mejor visualizacion
                    nroIteraciones++;
                    linea[0] = Eventos[(int)relojYEvento[1]] + " " + nroIteraciones.ToString();
                    linea[1] = tDeEventos[5].ToString();
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
