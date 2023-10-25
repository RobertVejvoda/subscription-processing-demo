using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Helpers;

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