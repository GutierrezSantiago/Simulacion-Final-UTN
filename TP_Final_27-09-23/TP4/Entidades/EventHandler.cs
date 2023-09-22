using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Final.Entidades
{
    internal class EventHandler
    {
        private static double[] AsignarAProximo(double[] proximo, double ev, double i)
        {
            proximo[0] = ev;
            proximo[1] = i;
            return proximo;
        }

        public static double[] ProximoEvento(double[] eventos)
        // Devuelve un arreglo indicando el tiempo del proximo evento y su posicion para determinar de que tipo es
        {
            double[] proximo = new double[2];

            for (int i = 0; i < eventos.Length; i++)
            {
                if (i == 0)
                {
                    AsignarAProximo(proximo, eventos[i], i);
                }
                else if (eventos[i] < proximo[0])
                {
                    AsignarAProximo(proximo, eventos[i], i);
                }
            }

            return proximo;
        }
    }
}

// [Llegada F, Llegada B ...]
// [3,5 , 0]