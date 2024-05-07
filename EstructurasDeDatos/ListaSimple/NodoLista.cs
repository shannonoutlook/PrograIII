using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructurasDeDatos.ListaSimple
{
    public class NodoLista
    {
        public object Dato { get; set; }
        public NodoLista Link { get; set; }

        public NodoLista(object dato)
        {
            Dato = dato;
            Link = null;
        }
    }
}
