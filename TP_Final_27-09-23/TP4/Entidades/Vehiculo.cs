using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Final.Entidades
{
    internal class Vehiculo
    {
        private Estado Estado;
        public Vehiculo(Estado estadoParam)
        {
            this.Estado = estadoParam;
        }
        public Estado GetEstado()
        {
            return Estado;
        }
        public void SetEstado(Estado estado)
        {
            this.Estado = estado;
        }

        internal string getNombreEstado()
        {
            return Estado.GetNombre();
        }
    }
    }
    
