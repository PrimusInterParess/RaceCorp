using System.Collections.Generic;

namespace RaceCorp.Web.Areas.Identity.Pages.Account.Dtos
{
    public class PeopleApiPhotos
    {
        public string resourceName { get; set; }

        public string etag { get; set; }

        public List<Photo> photos { get; set; }

        public List<Name> names { get; set; }

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

        public class Photo
        {
            public Metadata metadata { get; set; }

            public string url { get; set; }
        }

        public class Name
        {
            public Metadata metadata { get; set; }

            public string firstName { get; set; }

            public string lastName { get; set; }
        }

        public class Gender
        {
            public Metadata metadata { get; set; }

            public string value { get; set; }
        }
    }
}
