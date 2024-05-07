using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructurasDeDatos.Cola
{
    public class Cola
    {
        public NodoCola primero { get; set; }
        public NodoCola ultimo { get; set; }

        public Cola()
        {
            primero = null;
            ultimo = null;
        }

        public void pushCola(string cliente)
        {
            NodoCola nuevoNodo = new NodoCola(cliente);

            if (ultimo == null)
            {
                primero = nuevoNodo;
                ultimo = nuevoNodo;
            }
            else
            {
                ultimo.link = nuevoNodo;
                ultimo = nuevoNodo;
            }
        }

        public string deleteCola()
        {
            if (primero == null)
            {
                return "Vacia";
            }

            string valor = primero.nombre;
            primero = primero.link;

            if (primero == null)
            {
                ultimo = null;
            }

            return valor;
        }
    }
}
