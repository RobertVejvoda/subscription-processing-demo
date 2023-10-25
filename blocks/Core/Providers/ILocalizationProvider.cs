using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Providers
{
    public interface ILocalizationProvider
    {
        string Localize(string message);
    }
}