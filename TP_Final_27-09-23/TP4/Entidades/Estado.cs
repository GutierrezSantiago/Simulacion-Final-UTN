using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Final.Entidades
{
    internal class Estado
    {
        private string Nombre;

        public Estado(string nombre)
        {
            Nombre = nombre;
        }

        public string GetNombre()
        {
            return Nombre;
        }
    }
}
