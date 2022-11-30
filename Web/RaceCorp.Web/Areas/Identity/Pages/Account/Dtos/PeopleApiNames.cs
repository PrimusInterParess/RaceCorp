namespace RaceCorp.Web.Areas.Identity.Pages.Account.Dtos
{
    using System.Collections.Generic;

    public class PeopleApiNames
    {
        public string resourceName { get; set; }

        public string etag { get; set; }


        public List<Name> names { get; set; }

        public class Source
        {
            public string type { get; set; }

            public string id { get; set; }
        }

        public class Metadata
        {
            public bool primary { get; set; }

            public Source source { get; set; }
        }

        public class Name
        {
            public Metadata metadata { get; set; }

            public string firstName { get; set; }

            public string lastName { get; set; }
        }
    }
}
