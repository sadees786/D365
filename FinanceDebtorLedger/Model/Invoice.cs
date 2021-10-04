using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;


namespace FinanceDebtorLedger.Model
{
    public class InvoiceDebtor
    {
       public string Externalid { get; set; }
       public string StatusCode { get; set; }
       public string Invoiceid { get; set; }       
       public string Dunninglevel { get; set; }
    }
    public class InvoiceDebtorList
    {
        [JsonProperty(NamingStrategyType = typeof(DefaultNamingStrategy))]
        public List<InvoiceDebtor> InvoiceDebtors { get; set; }
    }
}
