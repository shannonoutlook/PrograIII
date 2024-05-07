using EstructurasDeDatos.Cola;
using EstructurasDeDatos.ListaSimple;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json;
using EstructurasDeDatos.Pila;
using EstructurasDeDatos.Arbol;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreditCardController : ControllerBase
    {
        private static List<CreditCard> creditCards = new List<CreditCard>();
        private ListaS creditCardList = new ListaS();
        private static Cola paymentQueue = new Cola();
        private static Cola notificationQueue = new Cola();
        private static List<Notification> processedNotifications = new List<Notification>();
        private static Pila paymentStack = new Pila();
        private ArbolBinarioBusqueda creditCardTree;
        private static Pila limitIncreaseStack = new Pila();

        public CreditCardController(ArbolBinarioBusqueda creditCardTree, ListaS creditCardList)
        {
            this.creditCardTree = creditCardTree;
            this.creditCardList = creditCardList;
        }

        [HttpPost(Name = "AddCreditCards")]
        public IActionResult AddCreditCards([FromBody] List<CreditCard> newCreditCards)
        {
            if (newCreditCards == null || !newCreditCards.Any())
            {
                return BadRequest("No se ha proporcionado una lista válida de tarjetas de crédito.");
            }

            foreach (var creditCard in newCreditCards)
            {
                if (creditCard == null)
                {
                    return BadRequest("Una de las tarjetas de crédito proporcionadas no es válida.");
                }
                creditCards.Add(creditCard);
                creditCardList.insertHeaderLista(creditCard);
                creditCardTree.insertar(creditCard);
            }

            return Ok($"{newCreditCards.Count} tarjetas de crédito agregadas exitosamente");
        }

        [HttpGet(Name = "GetCreditCards")]
        public IEnumerable<CreditCard> GetCreditCards()
        {
            return creditCards;
        }

        [HttpGet("GetSaldo/{numeroTarjeta}")]
        public IActionResult GetSaldo(string numeroTarjeta)
        {
            NodoLista nodoActual = creditCardList.header;
            long numeroTarjetaLong = long.Parse(numeroTarjeta);

            while (nodoActual != null)
            {
                CreditCard tarjeta = nodoActual.Dato as CreditCard;
                if (tarjeta != null && tarjeta.NumeroTarjeta == numeroTarjetaLong)
                {
                    return Ok(tarjeta.Saldo);
                }
                nodoActual = nodoActual.Link;
            }

            return NotFound($"Tarjeta de crédito con número {numeroTarjeta} no encontrada.");
        }


        [HttpPost("makepayment")]
        public IActionResult MakePayment([FromBody] PaymentRequest paymentRequest)
        {
            if (paymentRequest == null || paymentRequest.Amount <= 0)
            {
                return BadRequest("Solicitud de pago no válida.");
            }

            paymentQueue.pushCola(JsonConvert.SerializeObject(paymentRequest));
            paymentStack.insertPila(JsonConvert.SerializeObject(paymentRequest));
            return Ok("Pago encolado exitosamente.");
        }

        [HttpPost("processpayments")]
        public IActionResult ProcessPayments()
        {
            while (paymentQueue.primero != null)
            {
                var paymentJson = paymentQueue.deleteCola();
                PaymentRequest payment = JsonConvert.DeserializeObject<PaymentRequest>(paymentJson);
                var creditCard = creditCards.FirstOrDefault(c => c.NumeroTarjeta == payment.CreditCardNumber);

                if (creditCard != null)
                {
                    creditCard.Saldo -= payment.Amount;
                    string message = $"Pago de {payment.Amount} procesado a la tarjeta {payment.CreditCardNumber}. Saldo actual: {creditCard.Saldo}.";
                    EnqueueNotification(message);
                    
                }
                else
                {
                    return NotFound($"Tarjeta de crédito con número {payment.CreditCardNumber} no encontrada.");
                }
            }

            return Ok("Todos los pagos han sido procesados correctamente.");
        }

        [HttpGet("GetEstadoCuenta/{numeroTarjeta}")]
        public IActionResult GetEstadoCuenta(string numeroTarjeta)
        {
            long numeroTarjetaLong = long.Parse(numeroTarjeta);
            CreditCard buscada = new CreditCard { NumeroTarjeta = numeroTarjetaLong };
            Nodo resultado = creditCardTree.buscarIterativo(buscada);

            if (resultado != null)
            {
                CreditCard tarjetaEncontrada = (CreditCard)resultado.valorNodo();
                var pagos = GetPaymentsForCard(tarjetaEncontrada.NumeroTarjeta);

                var estadoCuenta = new
                {
                    NumeroTarjeta = tarjetaEncontrada.NumeroTarjeta,
                    NombreTarjeta = tarjetaEncontrada.NombreTarjeta,
                    Saldo = tarjetaEncontrada.Saldo,
                    LimiteCredito = tarjetaEncontrada.LimiteCredito,
                    FechaCorte = tarjetaEncontrada.FechaCorte,
                    FechaPago = tarjetaEncontrada.FechaPago,
                    Puntos = tarjetaEncontrada.Puntos,
                    EstadoBloqueo = tarjetaEncontrada.Bloqueado,
                    Movimientos = pagos
                };
                return Ok(estadoCuenta);
            }
            return NotFound($"Tarjeta de crédito con número {numeroTarjeta} no encontrada.");
        }

        private List<PaymentRequest> GetPaymentsForCard(long numeroTarjeta)
        {
            List<PaymentRequest> payments = new List<PaymentRequest>();
            Pila tempStack = new Pila();
            while (!paymentStack.pilaVacia())
            {
                var paymentJson = paymentStack.deletePila().ToString();
                PaymentRequest payment = JsonConvert.DeserializeObject<PaymentRequest>(paymentJson);
                if (payment.CreditCardNumber == numeroTarjeta)
                {
                    payments.Add(payment);
                }
                tempStack.insertPila(paymentJson);
            }

            while (!tempStack.pilaVacia())
            {
                paymentStack.insertPila(tempStack.deletePila().ToString());
            }
            return payments;
        }

        private void EnqueueNotification(string message)
        {
            var notification = new Notification { Message = message };
            var notificationJson = JsonConvert.SerializeObject(notification);
            notificationQueue.pushCola(notificationJson);
        }

        [HttpPost("processnotifications")]
        public IActionResult ProcessNotifications()
        {
            while (notificationQueue.primero != null)
            {
                var notificationJson = notificationQueue.deleteCola();
                Notification notification = JsonConvert.DeserializeObject<Notification>(notificationJson);
                processedNotifications.Add(notification);
            }

            return Ok("Notificaciones procesadas exitosamente.");
        }

        [HttpGet("getnotifications")]
        public IActionResult GetNotifications()
        {
            return Ok(processedNotifications);
        }

        [HttpGet("getMovements")]
        public IActionResult GetPayments()
        {
            List<PaymentRequest> payments = new List<PaymentRequest>();

            Pila tempStack = new Pila();
            while (!paymentStack.pilaVacia())
            {
                var paymentJson = paymentStack.deletePila().ToString();
                PaymentRequest payment = JsonConvert.DeserializeObject<PaymentRequest>(paymentJson);
                payments.Add(payment);
                tempStack.insertPila(paymentJson);
            }

            while (!tempStack.pilaVacia())
            {
                paymentStack.insertPila(tempStack.deletePila().ToString());
            }

            return Ok(payments);
        }

        [HttpPost("ChangePin")]
        public IActionResult ChangePin([FromBody] PinChangeRequest request)
        {
            if (request == null || request.NewPin.Length != 4)
            {
                return BadRequest("Solicitud inválida o formato de PIN incorrecto.");
            }

            long numeroTarjetaLong = request.CreditCardNumber;
            NodoLista nodoActual = creditCardList.header;

            while (nodoActual != null)
            {
                CreditCard tarjeta = nodoActual.Dato as CreditCard;
                if (tarjeta != null && tarjeta.NumeroTarjeta == numeroTarjetaLong)
                {
                    if (tarjeta.Pin != request.OldPin)
                    {
                        return BadRequest("El PIN antiguo no coincide.");
                    }
                    tarjeta.Pin = request.NewPin;
                    EnqueueNotification($"PIN actualizado para la tarjeta {request.CreditCardNumber}.");
                    return Ok("PIN actualizado exitosamente.");
                }
                nodoActual = nodoActual.Link;
            }

            return NotFound("Tarjeta de crédito con número {request.CreditCardNumber} no encontrada.");
        }


        [HttpPost("BlockCard/{numeroTarjeta}")]
        public IActionResult BlockCard(string numeroTarjeta)
        {
            long numeroTarjetaLong = long.Parse(numeroTarjeta);
            CreditCard cardToBlock = new CreditCard { NumeroTarjeta = numeroTarjetaLong };
            Nodo cardNode = creditCardTree.buscarIterativo(cardToBlock);

            if (cardNode != null)
            {
                CreditCard foundCard = (CreditCard)cardNode.valorNodo();
                foundCard.Bloqueado = true;
                EnqueueNotification($"La tarjeta {numeroTarjeta} ha sido bloqueada temporalmente.");
                return Ok($"Tarjeta {numeroTarjeta} bloqueada temporalmente.");
            }
            return NotFound($"Tarjeta de crédito con número {numeroTarjeta} no encontrada.");
        }

        [HttpPost("requestcreditlimitincrease")]
        public IActionResult RequestCreditLimitIncrease([FromBody] CreditLimitRequest limitRequest)
        {
            if (limitRequest == null || limitRequest.NewCreditLimit <= 0)
            {
                return BadRequest("Solicitud de aumento de límite no válida.");
            }

            string limitRequestJson = JsonConvert.SerializeObject(limitRequest);
            limitIncreaseStack.insertPila(limitRequestJson);
            return Ok("Solicitud de aumento de límite apilada exitosamente.");
        }

        [HttpPost("processcreditlimitincreases")]
        public IActionResult ProcessCreditLimitIncreases()
        {
            List<CreditLimitRequest> processedRequests = new List<CreditLimitRequest>();
            while (!limitIncreaseStack.pilaVacia())
            {
                string limitRequestJson = limitIncreaseStack.deletePila().ToString();
                CreditLimitRequest limitRequest = JsonConvert.DeserializeObject<CreditLimitRequest>(limitRequestJson);
                var creditCard = creditCards.FirstOrDefault(c => c.NumeroTarjeta == limitRequest.CreditCardNumber);

                if (creditCard != null)
                {
                    creditCard.LimiteCredito = Math.Max(creditCard.LimiteCredito, limitRequest.NewCreditLimit);
                    processedRequests.Add(limitRequest);
                    EnqueueNotification($"Límite de crédito aumentado a {limitRequest.NewCreditLimit} para la tarjeta {limitRequest.CreditCardNumber}.");
                }
            }

            return Ok(processedRequests);
        }

    }
}

