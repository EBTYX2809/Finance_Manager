namespace Finance_Manager_Backend.Exceptions;

public class SavingIsNotExistException : Exception
{
    public SavingIsNotExistException(string savingId)
        : base($"Saving with {savingId} isn't exist.") { }
}

