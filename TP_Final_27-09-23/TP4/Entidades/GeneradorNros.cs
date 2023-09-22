using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Final.Entidades
{
    internal class GeneradorNros
    {
        public static double Truncar(double nro)
        {
            return Math.Truncate(nro * 10000) / 10000;
        }

        public static double Uniforme(double inf, double sup, double rnd)
        {
            return Truncar(inf + rnd * (sup - inf));
        }

        public static double Exponencial(double media, double rnd)
        {
            return Truncar(-media * Math.Log(1 - rnd));
        }

        public static double[] Normal(double media, double desviacion, double rnd1, double rnd2)
        {
            double b = Math.Sqrt(-2 * Math.Log(rnd1));
            double ang = 2 * Math.PI * rnd2;
            double normal1 = (b * Math.Cos(ang) * desviacion) + media;
            double normal2 = (b * Math.Sin(ang) * desviacion) + media;
            
            return new double[2] { Truncar(normal1), Truncar(normal2)};
        }

    }
}
