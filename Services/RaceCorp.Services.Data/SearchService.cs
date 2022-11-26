namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models.Enums;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;

    public class SearchService : ISearchService
    {

        public void GetSearchResults(SearchInputModel inputModel)
        {
            var typeCategory = (SearchCategory)Enum.Parse(typeof(SearchCategory), inputModel.Area);

            var assembly = $"RaceCorp.Data.Models.{typeCategory.ToString()}";
            var repositoryAssembly = $"RaceCorp.Data.Common.Repositories.IDeletableEntityRepsitory";

            Type typeRepository = Type.GetType(repositoryAssembly);

            Type type = Type.GetType(assembly);
            if (type != null)
            {
                var instace = Activator.CreateInstance(type);

                if (typeRepository != null)
                {
                    var repoInstance = Activator.CreateInstance(typeRepository);
                }

                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    typeRepository = asm.GetType(repositoryAssembly);

                    ;
                }
            }

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(assembly);
                if (type != null)
                {
                    var instace = Activator.CreateInstance(type);

                    typeRepository = Type.GetType(repositoryAssembly);

                    if (typeRepository != null)
                    {
                        var repoInstance = Activator.CreateInstance(typeRepository);
                    }

                    foreach (var asm1 in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        typeRepository = asm1.GetType(repositoryAssembly);

                        ;
                    }
                }
            }

            ;
        }

        //public List<T> GetSearchResults<T>(SearchInputModel inputModel)
        //{

        //}
    }
}
