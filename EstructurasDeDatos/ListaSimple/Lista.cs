using EstructurasDeDatos.ListaSimple;

public class ListaS
{
    public NodoLista header { get; set; }

    public ListaS()
    {
        header = null;
    }

    public void insertHeaderLista(object objNodo)
    {
        NodoLista nuevoNodo = new NodoLista(objNodo);
        nuevoNodo.Link = header;
        header = nuevoNodo;
    }

    public void insertLast(object objNodo)
    {
        NodoLista nuevoNodo = new NodoLista(objNodo);
        if (header == null)
        {
            header = nuevoNodo;
        }
        else
        {
            NodoLista lastNode = getLastNode();
            lastNode.Link = nuevoNodo;
        }
    }

    private NodoLista getLastNode()
    {
        NodoLista temp = header;
        while (temp.Link != null)
        {
            temp = temp.Link;
        }
        return temp;
    }

    public bool deleteNode(object objNodo)
    {
        if (header == null)
            return false;

        if (header.Dato.Equals(objNodo))
        {
            header = header.Link;
            return true;
        }

        NodoLista current = header;
        while (current.Link != null)
        {
            if (current.Link.Dato.Equals(objNodo))
            {
                current.Link = current.Link.Link;
                return true;
            }
            current = current.Link;
        }
        return false;
    }

    public NodoLista findNode(object objNodo)
    {
        NodoLista current = header;
        while (current != null)
        {
            if (current.Dato.Equals(objNodo))
                return current;
            current = current.Link;
        }
        return null;
    }

    public void displayList()
    {
        NodoLista current = header;
        while (current != null)
        {
            Console.WriteLine(current.Dato);
            current = current.Link;
        }
    }

    public int readNodos()
    {
        int suma = 0;
        NodoLista nodoActual = header;
        while (nodoActual != null)
        {
            suma += int.Parse(nodoActual.Dato.ToString());
            nodoActual = nodoActual.Link;
        }
        return suma;
    }

}
