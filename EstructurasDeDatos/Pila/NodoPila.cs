using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructurasDeDatos.Pila
{
    public class NodoPila
    {
        public string Element { get; set; }
        public NodoPila? link { get; set; }

        public NodoPila(string element)
        {
            Element = element;
            link = null;
        }

    }
}
