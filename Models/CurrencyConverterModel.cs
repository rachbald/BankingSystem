using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBankingSystem.Models
{
    public class CurrencyConverterModel
    {
        public Decimal Amount { get; set; }
        public Decimal Result { get; set; }

        public SelectList From { get; set; }
        public SelectList To { get; set; }
        

        public CurrencyConverterModel()
        {
            From = new SelectList(new CurrencyReference.CurrencesServiceClient().GetCurrencies(), "Code", "Currency");
            To = new SelectList(new CurrencyReference.CurrencesServiceClient().GetCurrencies(), "Code", "Currency");
        }
    }
}