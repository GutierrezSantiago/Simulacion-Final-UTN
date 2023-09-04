using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4.Entidades
{
    internal class ValidadorParametros
    {
        public bool validarSuperiorACero(int n)
        {
            // Valida que el parametro(int) ingresado sea superior a 0
            return n > 0;
        }
        public bool validarSuperiorACero(double n)
        {
            // Valida que el parametro(double) ingresado sea superior a 0
            return n > 0;
        }
        public bool validarDesdeHasta(int desde, int hasta)
        {
            // Valida que el valor desde no supere hasta

            if (desde > hasta) return false;
            return true;
        }

    }
}
