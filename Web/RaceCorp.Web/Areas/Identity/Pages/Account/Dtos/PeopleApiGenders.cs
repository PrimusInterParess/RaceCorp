using System.Collections.Generic;

namespace RaceCorp.Web.Areas.Identity.Pages.Account.Dtos
{
    public class PeopleApiGenders
    {
        public string resourceName { get; set; }

        public string etag { get; set; }

        public List<Gender> genders { get; set; }

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

        public class Gender
        {
            public Metadata metadata { get; set; }

            public string value { get; set; }
        }
    }
}
