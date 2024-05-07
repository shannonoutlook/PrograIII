using System;
namespace API
{
	public class PinChangeRequest
	{
        public long CreditCardNumber { get; set; }
        public string OldPin { get; set; }
        public string NewPin { get; set; }
    }
}

