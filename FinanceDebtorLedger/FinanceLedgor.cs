using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FinanceDebtorLedger.Services;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using FinanceDebtorLedger.Model;
using System.Collections.Generic;

namespace FinanceDebtorLedger
{
    public  class FinanceLedger
    {
        private readonly IDataVerseService _dataVerseservice;
        
/// <summary>
/// 
/// </summary>
/// <param name="dataVerseservice"></param>
/// <param name="secretManager"></param>
        public FinanceLedger(IDataVerseService dataVerseservice)
        {
          _dataVerseservice = dataVerseservice;
         
        }

        [FunctionName("UpdateStatus")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
                 string secretValue = Environment.GetEnvironmentVariable("ConnectionString");
            List<Entity> completeBandsCollection = new List<Entity>();
            var bands = new List<InvoiceDebtor>();
            try
            {
                ServiceClient dataVerseConnection = new ServiceClient(secretValue);
                if (!dataVerseConnection.IsReady)
                {
                    throw new Exception("Authentication Failed!");
                }
                QueryExpression queryExp = new QueryExpression
                {
                    EntityName = "new_debtorsledger",
                    ColumnSet = new ColumnSet()
                };
                queryExp.ColumnSet.Columns.Add("new_invoicenumber");
                queryExp.ColumnSet.Columns.Add("cr431_providerid");
                queryExp.ColumnSet.Columns.Add("statuscode");
                queryExp.ColumnSet.Columns.Add("new_customernumber");
                queryExp.ColumnSet.Columns.Add("new_dunninglevel");
                completeBandsCollection = _dataVerseservice.CompleteBandsCollection(queryExp, dataVerseConnection);
            }
            catch(Exception ex)
            {
                log.LogError(ex,ex.Message,null);
            }
            foreach (var invoice in completeBandsCollection)
            {
                EntityReference entref = (EntityReference)invoice.Attributes["new_dunninglevel"];
                bands.Add(new InvoiceDebtor
                {
                    Externalid = invoice.Attributes["new_customernumber"].ToString(),
                    StatusCode = ((OptionSetValue)invoice.Attributes["statuscode"]).Value.ToString(),
                    Invoiceid = invoice.Attributes["new_invoicenumber"].ToString(),
                    Dunninglevel= entref.Name
            }); 
            }
            InvoiceDebtorList bandlist = new InvoiceDebtorList
            {
               InvoiceDebtors  = bands
            };
            // return new JsonResult(bandlist, Helper..CamelCase);
            return new OkObjectResult(bandlist);        
        }
    }
}
