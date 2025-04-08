namespace Finance_Manager_Backend.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userId) 
        : base($"User with ID {userId} not found.") { }
}
