namespace Finance_Manager_Backend.Exceptions;

public class TransactionIsNotExistException : Exception
{
    public TransactionIsNotExistException(string transactionId) 
        : base($"Transaction with {transactionId} isn't exist.")  { }
}
