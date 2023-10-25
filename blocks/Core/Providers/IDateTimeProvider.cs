using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Providers
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}