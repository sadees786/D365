﻿using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;

namespace FinanceDebtorLedger.Services
{ 
 public interface IDataVerseService
{
    List<Entity> CompleteBandsCollection(QueryExpression queryExp, ServiceClient service);
}

public class DataVerseService : IDataVerseService
{

    public List<Entity> CompleteBandsCollection(QueryExpression query, ServiceClient service)
    {
        var pageNumber = 1;
        var pagingCookie = string.Empty;
        var result = new List<Entity>();
        EntityCollection resp;
        do
        {
            if (pageNumber != 1)
            {
                query.PageInfo.PageNumber = pageNumber;
                query.PageInfo.PagingCookie = pagingCookie;
            }
            resp = service.RetrieveMultiple(query);
            
            if (resp.MoreRecords)
            {
                pageNumber++;
                pagingCookie = resp.PagingCookie;
            }
            //Add the result from RetrieveMultiple to the List to be returned.
            result.AddRange(resp.Entities);
        }
        while (resp.MoreRecords);

        return result;
    }
}
}
