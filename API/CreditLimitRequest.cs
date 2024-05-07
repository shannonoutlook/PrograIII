using System;
namespace API
{
	public class CreditLimitRequest
	{
        public long CreditCardNumber { get; set; }
        public double NewCreditLimit { get; set; }
    }
}

