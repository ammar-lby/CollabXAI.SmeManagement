namespace CollabXAI.Domain.Exceptions;

// thrown by the service, the api later turns it into a 404
public class SmeNotFoundException : Exception
{
    public SmeNotFoundException(Guid id)
        : base($"SME with ID {id} was not found.")
    {
    }
}
