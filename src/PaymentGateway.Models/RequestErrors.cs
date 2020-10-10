namespace PaymentGateway.Api
{
    public static class RequestErrors
    {
        public const string Numeric = "Field must be numeric";
        public const string CardNumber = "Card number must be 15 or 16 digits.";
        public const string Month = "Expiry month must be 2 digits.";
        public const string Year = "Expiry year must be 2 digits.";
        public const string Cvv = "Cvv must be a number of 3 or 4 digits.";
        public const string Amount = "Amount must be in the format 00.00";
        
    }
}