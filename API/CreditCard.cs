namespace API;
using EstructurasDeDatos.Arbol;

public class CreditCard : Comparador
{
    public long NumeroTarjeta { get; set; }
    public string NombreTarjeta { get; set; }
    public double Saldo { get; set; }
    public double LimiteCredito { get; set; }
    public string FechaCorte { get; set; }
    public string FechaPago { get; set; }
    public int Puntos { get; set; }
    public string Pin { get; set; }
    public bool Bloqueado { get; set; }

    public bool igualQue(Object q)
    {
        return NumeroTarjeta == ((CreditCard)q).NumeroTarjeta;
    }

    public bool menorQue(Object q)
    {
        return NumeroTarjeta < ((CreditCard)q).NumeroTarjeta;
    }

    public bool menorIgualQue(Object q) => menorQue(q) || igualQue(q);
    public bool mayorQue(Object q)
    {
        return NumeroTarjeta > ((CreditCard)q).NumeroTarjeta;
    }

    public bool mayorIgualQue(Object q) => mayorQue(q) || igualQue(q);
}
