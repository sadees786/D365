using System;
using FinanceDebtorLedger.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FinanceDebtorLedger.Startup))]
namespace FinanceDebtorLedger
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IDataVerseService>((s) =>
            {
                return new DataVerseService();
            });        
        }

    }
}