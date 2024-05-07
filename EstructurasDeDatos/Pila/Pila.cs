using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructurasDeDatos.Pila
{
    public class Pila
    {
        public NodoPila header { get; set; }

        public Pila()
        {
            header = null;
        }
        public Boolean pilaVacia()
        {
            return header == null;
        }
        public void insertPila(string elemento)
        {
            NodoPila nuevoElemento = new NodoPila(elemento);
            nuevoElemento.link = header;
            header = nuevoElemento;
        }
        public object deletePila()
        {
            object auxElemento = header.Element;
            header = header.link;
            return auxElemento;
        }
    }
}
