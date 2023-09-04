using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4.Entidades
{
    internal class Disciplina
    {
        private string Nombre;
        private int Prioridad;

        public Disciplina(string nombre, int prio)
        {
            Nombre = nombre;
            Prioridad = prio;
        }

        public string GetNombre() { return Nombre; }

        public int GetPrioridad() { return Prioridad; }
    }
}
