using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace TP4.Entidades
{
    internal class GestorSimulacion
    {
        public string Datos = @"./datos.csv";
        private Estado[] EstadosDeportistas = { new Estado("En Espera"), new Estado("Jugando"), new Estado("Finalizo") };
        private Estado[] EstadosCancha = { new Estado("Libre"), new Estado("Ocupada"), new Estado("Limpiando") };
        private Disciplina[] Disciplinas = { new Disciplina("Basket", 0), new Disciplina("Futbol", 1), new Disciplina("Handball", 1) };

        private string[] Eventos = { "Llegada Basket", "Llegada Futbol", "Llegada Handball", "Fin Ocupacion", "Fin de Limpieza", "Fin de Simulacion" };
        private double[] tDeEventos; // mismo orden que vector arriba
        #region Propiedades
        private double LimsLlegFutbol;
        private double[] LimsLlegHandball;
        private double[] LimsLlegBasket;

        private double[] LimsOcupFutbol;
        private double[] LimsOcupHandball;
        private double[] LimsOcupBasket;

        private double TLimpieza;

        private double InicioImp;
        private int Iteraciones;

        private Cancha Cancha;
        private Deportista?[] DeportistasEnSistema;

        Random RndBasket;
        Random RndHandball;
        Random RndOcupacion;
        Random RndFutbol;


        #endregion
        #region Constructor
        public GestorSimulacion(double inicioImp, int Cantidad, double FinSim, double limsLlegFutbol, double[] limsLlegHandball, 
            double[] limsLlegBasket, double[] limsOcupFutbol, double[] limsOcupHandball, double[] limsOcupBasket, 
            double tLimpieza) 
        {
            tDeEventos = new double[6];
            tDeEventos[5] = FinSim;
            InicioImp = inicioImp;
            Iteraciones = Cantidad;

            LimsLlegFutbol = limsLlegFutbol;
            LimsLlegHandball = limsLlegHandball;
            LimsLlegBasket = limsLlegBasket;

            LimsOcupFutbol = limsOcupFutbol;
            LimsOcupHandball = limsOcupHandball;
            LimsOcupBasket = limsOcupBasket;

            TLimpieza = tLimpieza;

            Cancha = new Cancha(EstadosCancha[0], 0);
            DeportistasEnSistema = new Deportista[6];

            RndBasket = new Random(Guid.NewGuid().GetHashCode());
            RndHandball = new Random(Guid.NewGuid().GetHashCode());
            RndOcupacion = new Random(Guid.NewGuid().GetHashCode());
            RndFutbol = new Random(Guid.NewGuid().GetHashCode());

        }
        #endregion
        #region Calculos
        // Seccion para calcular tiempos de eventos a partir de un rnd siguiendo su distribucion correspondiente
        public double CalcularLlegadaFutbol(double rnd) {
            return GeneradorNros.Exponencial(LimsLlegFutbol, rnd);
         }
        public double CalcularLlegadaBasket(double rnd) {
            return GeneradorNros.Uniforme(LimsLlegBasket[0], LimsLlegBasket[1], rnd);
         }
        public double CalcularLlegadaHandball(double rnd) {
            return GeneradorNros.Uniforme(LimsLlegHandball[0], LimsLlegHandball[1], rnd);
         }

        public double CalcularFinOcupacionCancha(double rnd, Deportista d)
        {
            string disc = d.getNombreDisc();
            double generado = 0;
            switch (disc)
            {
                case "Futbol":
                    generado = GeneradorNros.Uniforme(LimsOcupFutbol[0], LimsOcupFutbol[1], rnd); break;
                case "Basket":
                    generado = GeneradorNros.Uniforme(LimsOcupBasket[0], LimsOcupBasket[1], rnd); break;
                case "Handball":
                    generado = GeneradorNros.Uniforme(LimsOcupHandball[0], LimsOcupHandball[1], rnd); break;
                default:
                    break;
            }
            return generado;
        }
        #endregion

        #region Calculo de Estadisticas
        private static void SumarAEsperaFinalizadaSegunTipoDeportista(string[] linea, Deportista dep, double reloj)
        {
            int contadorEsperaFin;
            double esperaFinAC;

            if (dep.getNombreDisc() == "Basket")
            {
                esperaFinAC = double.Parse(linea[19]) + (reloj - dep.getTiempoLleg());
                contadorEsperaFin = int.Parse(linea[22]) + 1;

                linea[19] = GeneradorNros.Truncar(esperaFinAC).ToString();
                linea[22] = contadorEsperaFin.ToString();
            }
            else if (dep.getNombreDisc() == "Futbol")
            {
                esperaFinAC = double.Parse(linea[20]) + (reloj - dep.getTiempoLleg());
                contadorEsperaFin = int.Parse(linea[23]) + 1;

                linea[20] = GeneradorNros.Truncar(esperaFinAC).ToString();
                linea[23] = contadorEsperaFin.ToString();
            }
            else if (dep.getNombreDisc() == "Handball")
            {
                esperaFinAC = double.Parse(linea[21]) + (reloj - dep.getTiempoLleg());
                contadorEsperaFin = int.Parse(linea[24]) + 1;

                linea[21] = GeneradorNros.Truncar(esperaFinAC).ToString();
                linea[24] = contadorEsperaFin.ToString();
            }
        }
        #endregion

        #region Utilidades de Escritura
        private void EscribirDeportistasVectorEstado(string[] linea)
        {
            int puntero = 27;
            for(int i = 0; i<DeportistasEnSistema.Length; i++)
            {
                if (DeportistasEnSistema[i] == null)
                {
                    linea[puntero] = "";
                    linea[puntero+1] = "";
                    linea[puntero+2] = "";
                }
                else
                {
                    linea[puntero] = DeportistasEnSistema[i].getNombreEstado();
                    linea[puntero + 1] = DeportistasEnSistema[i].getTiempoLleg().ToString();
                    linea[puntero + 2] = DeportistasEnSistema[i].getNombreDisc();
                }

                puntero += 3;
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
        private void GenerarProximaLlegadaSegunEventoLlegada(string[] linea, double[] relojYEvento)
        {
            double rnd;
            double tEntreLlegadas;

            if (relojYEvento[1] == 0) // Llegada Basket
            {
                rnd = RndBasket.NextDouble(); // Generamos rnd
                tEntreLlegadas = CalcularLlegadaBasket(rnd); // Calculamos t entre Llegadas
                tDeEventos[0] = tEntreLlegadas + relojYEvento[0]; // calculamos t de proxima llegada

                // asignamos los valores a la linea:
                linea[2] = GeneradorNros.Truncar(rnd).ToString();
                linea[3] = tEntreLlegadas.ToString();
                linea[4] = GeneradorNros.Truncar(tDeEventos[0]).ToString();
                return;
            }
            if (relojYEvento[1] == 1) // Llegada Futbol
            {
                rnd = RndFutbol.NextDouble(); // Generamos rnd
                tEntreLlegadas = CalcularLlegadaFutbol(rnd); // Calculamos t entre Llegadas
                tDeEventos[1] = tEntreLlegadas + relojYEvento[0]; // calculamos t de proxima llegada

                // asignamos los valores a la linea:
                linea[5] = GeneradorNros.Truncar(rnd).ToString();
                linea[6] = tEntreLlegadas.ToString();
                linea[7] = GeneradorNros.Truncar(tDeEventos[1]).ToString();
                return;
            }
            if (relojYEvento[1] == 2) // Llegada Handball
            {
                rnd = RndHandball.NextDouble(); // Generamos rnd
                tEntreLlegadas = CalcularLlegadaHandball(rnd); // Calculamos t entre Llegadas
                tDeEventos[2] = tEntreLlegadas + relojYEvento[0]; // calculamos t de proxima llegada

                // asignamos los valores a la linea:
                linea[8] = GeneradorNros.Truncar(rnd).ToString();
                linea[9] = tEntreLlegadas.ToString();
                linea[10] = GeneradorNros.Truncar(tDeEventos[2]).ToString();
                return;
            }
        }

        private Deportista CrearNuevoDeportistaSegunLlegada(double[] relojYEvento)
        {
            Deportista dep;
            double tLlegada = GeneradorNros.Truncar(relojYEvento[0]);

            if (relojYEvento[1] == 0) // Llego Basketball
            {
                dep = new Deportista(tLlegada, EstadosDeportistas[0], Disciplinas[0]);
            }
            else if (relojYEvento[1] == 1) // Llego Futbol
            {
                dep = new Deportista(tLlegada, EstadosDeportistas[0], Disciplinas[1]);
            }
            else // Llego Handball
            {
                dep = new Deportista(tLlegada, EstadosDeportistas[0], Disciplinas[2]);
            }
            return dep;
        }

        private void AsignarDeportistaAVectorEstado(Deportista dep)
        {
            for(int i = 0; i < DeportistasEnSistema.Length; i++)
            {
                if (DeportistasEnSistema[i] == null) // buscamos un hueco en el arreglo
                {
                    DeportistasEnSistema[i] = dep; // metemos al deportista ahi
                    break;
                }
            }
        }

        private string[] Llegada(string[] lineaAnt, double[] relojYEvento)
        {
            string[] linea = lineaAnt;
            int contadorLlegadas = int.Parse(linea[25]) + 1; // sumamos 1 al contador de llegadas
            linea[25] = contadorLlegadas.ToString(); // escribimos el nuevo nro

            GenerarProximaLlegadaSegunEventoLlegada(linea, relojYEvento); // generamos proxima llegada y la anotamos en el vector estado

            if (Cancha.HayMenosDe5EnCola()) // si hay menos de 5 en cola, creamos un nuevo deportista segun quien llego
            {
                Deportista dep = CrearNuevoDeportistaSegunLlegada(relojYEvento); // se crea un nuevo deportista con estado en espera
                
                if (Cancha.EstaLibre()) // Si la cancha esta libre, pasa directo a jugar y no se suma a la cola
                {
                    GenerarOcupacionCancha(linea, relojYEvento[0], dep); // generamos ocupacion y la cargamos al vector estado (cambiando estado de Dep a JUGANDO y de Cancha a OCUPADA
                }
                else
                {
                    Cancha.SumarACola(); // sumamos el equipo a la cola
                    linea[16] = Cancha.getTamCola().ToString(); // ponemos nuevo valor en el vector estado
                }

                AsignarDeportistaAVectorEstado(dep); // en cualquier caso, lo sumamos al vector estado
            }
            else
            {
                int retirados = int.Parse(linea[26]) + 1; // si hay +5 en cola, se retira el equipo que acaba de llegar
                linea[26] = retirados.ToString(); // escribimos el nuevo nro
            }

            EscribirDeportistasVectorEstado(linea);
            return linea;
        }
        #endregion

        #region Ocupacion
        private void GenerarOcupacionCancha(string[] linea, double reloj, Deportista dep)
        {
            // generamos los datos de la ocupacion
            double rnd = GeneradorNros.Truncar(RndOcupacion.NextDouble());
            double tOcup = CalcularFinOcupacionCancha(rnd, dep);
            tDeEventos[3] = GeneradorNros.Truncar(tOcup + reloj); // cargamos al vector de eventos

            // cargamos al vector estado
            linea[11] = dep.getNombreDisc();
            linea[12] = rnd.ToString();
            linea[13] = tOcup.ToString();
            linea[14] = tDeEventos[3].ToString();

            Cancha.SetEstado(EstadosCancha[1]); // cambiamos el estado de la cancha a ocupada
            linea[15] = Cancha.getNombreEstado(); // cambiamos la linea para mostrar estado ocupado

            dep.SetEstado(EstadosDeportistas[1]); // cambiamos estado de Deportista a jugando

            SumarAEsperaFinalizadaSegunTipoDeportista(linea, dep, reloj); // sumamos 1 a contador llegadas tipo, y x al tiempo espera AC tipo
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

        private string[] FinOcupacion(string[] lineaAnt, double reloj)
        {
            string[] linea = lineaAnt;

            int jugando = BuscarDeportistaJugando(linea); // Buscamos equipo Jugando
            DeportistasEnSistema[jugando].SetEstado(EstadosDeportistas[2]); // cambiamos su estado a FINALIZO
            
            if(Cancha.getTamCola() == 0) // si no hay nadie en la cola
            {
                // borramos la ocupacion anterior
                linea[14] = "";
                tDeEventos[3] = (double)Int32.MaxValue; // valor maximo para que no ocurra

                // Generamos una limpieza
                GenerarLimpiezaCancha(linea, reloj);

            }
            else // si hay gente en la cola
            {
                // generamos una ocupacion
                int proximoAJugar = BuscarProximoDeportistaAJugar(reloj);
                GenerarOcupacionCancha(linea, reloj, DeportistasEnSistema[proximoAJugar]);

                Cancha.RestarACola(); // Restamos 1 a la cola
                linea[16] = Cancha.getTamCola().ToString(); // actualizamos el tam cola en el vector estado
            }

            EscribirDeportistasVectorEstado(linea);
            DeportistasEnSistema[jugando] = null; // quitamos al deportista que estaba jugando del sistema

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
            string[] lineaAnt = new string[45];
            string[] linea = new string[45];

            // Reloj inicial
            double reloj = 0;
            lineaAnt[0] = "Inicializacion";
            lineaAnt[1] = reloj.ToString();

            // Llegadas
            double rnd = RndBasket.NextDouble();
            lineaAnt[2] = GeneradorNros.Truncar(rnd).ToString();
            tDeEventos[0] = CalcularLlegadaBasket(rnd);
            lineaAnt[3] = tDeEventos[0].ToString();
            lineaAnt[4] = lineaAnt[3];

            rnd = RndHandball.NextDouble();
            lineaAnt[5] = GeneradorNros.Truncar(rnd).ToString();
            tDeEventos[1] = CalcularLlegadaHandball(rnd);
            lineaAnt[6] = tDeEventos[1].ToString();
            lineaAnt[7] = lineaAnt[6];

            rnd = RndFutbol.NextDouble();
            lineaAnt[8] = GeneradorNros.Truncar(rnd).ToString();
            tDeEventos[2] = CalcularLlegadaFutbol(rnd);
            lineaAnt[9] = tDeEventos[2].ToString();
            lineaAnt[10] = lineaAnt[9];

            // Ocupacion Cancha : Inicio no hay nada
            tDeEventos[3] = (double)Int32.MaxValue;

            // Iniciar Cancha y Deportistas en sistema
            lineaAnt[15] = Cancha.getNombreEstado();
            lineaAnt[16] = Cancha.getTamCola().ToString();

            // Limpieza : Inicio no hay nada
            tDeEventos[4] = (double)Int32.MaxValue;

            // Variables estadisticas
            // t espera AC 
            double tACB = 0;
            lineaAnt[19] = tACB.ToString();
            double tACH = 0;
            lineaAnt[20] = tACH.ToString();
            double tACF = 0;
            lineaAnt[21] = tACF.ToString();

            // contador fin espera
            int cFEB = 0;
            lineaAnt[22] = cFEB.ToString();
            int cFEH = 0;
            lineaAnt[23] = cFEH.ToString();
            int cFEF = 0;
            lineaAnt[24] = cFEF.ToString();

            // contador llegadas y retirados sin jugar
            int llegadas = 0;
            lineaAnt[25] = llegadas.ToString();
            int rsj = 0;
            lineaAnt[26] = rsj.ToString();
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

                if (relojYEvento[1] == 0 || relojYEvento[1] == 1 || relojYEvento[1] == 2) { linea = Llegada(lineaAnt, relojYEvento); }
                else if (relojYEvento[1] == 3) { linea = FinOcupacion(lineaAnt, relojYEvento[0]); }
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
