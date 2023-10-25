using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Types
{
    public class Currency : Enumeration
    {
        public static Currency CZK = new Currency(1, "Czech Crown", nameof(CZK), "Kč");
        public static Currency EUR = new Currency(2, "Euro", nameof(EUR), "€");
        public static Currency USD = new Currency(3, "American Dollar", nameof(USD), "$");
        public static Currency AUD = new Currency(4, "Australian Dollar", nameof(AUD), "A$");
        public static Currency GBP = new Currency(5, "British Pound", nameof(GBP), "Ł");
        public static Currency CHF = new Currency(6, "Swiss Frank", nameof(CHF), "CHF");

        public Currency(int id, string name, string code, string symbol) : base(id, name)
        {
            Code = code;
            Symbol = symbol;
        }

        public string Code { get; }
        public string Symbol { get; }
    }
}