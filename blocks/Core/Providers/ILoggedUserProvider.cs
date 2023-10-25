using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Providers
{
    public interface ILoggedUserProvider
    {
        int UserId { get; }
        int PersonId { get; }
        string Name { get; }
    }
}