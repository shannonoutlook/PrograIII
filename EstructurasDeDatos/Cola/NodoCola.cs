using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructurasDeDatos.Cola
{
    public class NodoCola
    {
        public string nombre { get; set; }
        public NodoCola link { get; set; }

        public NodoCola(string valor)
        {
            nombre = valor;
            link = null;
        }
    }
}
