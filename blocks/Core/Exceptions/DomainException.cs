namespace Core.Exceptions
{
    public class DomainException : Exception
    {
        public string DomainObjectId { get; private set; }
        
        public DomainException(string message, string domainObjectId) : base(message)
        {
            DomainObjectId = domainObjectId;
        }
    }
}