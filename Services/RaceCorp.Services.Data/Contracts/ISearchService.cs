namespace RaceCorp.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;

    using RaceCorp.Web.ViewModels.Common;

    public interface ISearchService
    {
       void GetSearchResults(SearchInputModel inputModel);
    }
}
