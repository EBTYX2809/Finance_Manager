namespace Finance_Manager_Backend.Exceptions;

public class TransactionIsNotExistException : Exception
{
    public TransactionIsNotExistException() 
        : base("Transaction isn't exist.")  { }
}
