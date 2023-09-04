using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Final.Entidades
{
    internal class Parquimetro
    {
        private Estado Estado;
        public Parquimetro(Estado estadoParam)
        {
            this.Estado = estadoParam;
        }
        public void SetEstado(Estado estado) => Estado = estado;
        public string getNombreEstado()
        {
            return Estado.GetNombre();
        }

        public bool EstaLibre()
        {
            return (this.getNombreEstado() == "Libre");
        }

    }
}
