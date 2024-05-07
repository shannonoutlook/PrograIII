using System;
namespace API
{
	public class PaymentRequest
	{
        public long CreditCardNumber { get; set; }
        public double Amount { get; set; }
    }
}

