namespace CollabXAI.Domain.Exceptions;

// thrown when an email is already registered, the api turns it into a 409
public class DuplicateEmailException : Exception
{
    public DuplicateEmailException(string email)
        : base($"An SME with the email '{email}' already exists.")
    {
    }
}
