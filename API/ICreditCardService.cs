using API;
using EstructurasDeDatos.ListaSimple;

public interface ICreditCardService
{
    void AddCreditCard(CreditCard card);
    List<CreditCard> GetAllCreditCards();
}

public class CreditCardService : ICreditCardService
{
    private readonly ListaS creditCardList = new ListaS();

    public void AddCreditCard(CreditCard card)
    {
        creditCardList.insertHeaderLista(card);
    }

    public List<CreditCard> GetAllCreditCards()
    {
        List<CreditCard> cards = new List<CreditCard>();
        NodoLista nodoActual = creditCardList.header;
        while (nodoActual != null)
        {
            if (nodoActual.Dato is CreditCard creditCard)
            {
                cards.Add(creditCard);
            }
            nodoActual = nodoActual.Link;
        }
        return cards;
    }
}
