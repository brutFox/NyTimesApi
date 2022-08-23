using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NyTimesData.Entity
{
    public class Result
    {
        [Key]
        public long Id { get; set; }
        public long RootId { get; set; }
        public string section { get; set; }
        public string subsection { get; set; }
        public string title { get; set; }
        public string @abstract { get; set; }
        public string url { get; set; }
        public string uri { get; set; }
        public string byline { get; set; }
        public string item_type { get; set; }
        public DateTime updated_date { get; set; }
        public DateTime created_date { get; set; }
        public DateTime published_date { get; set; }
        public string material_type_facet { get; set; }
        public string kicker { get; set; }
        public string des_facet { get; set; }
        public string org_facet { get; set; }
        public string per_facet { get; set; }
        public string geo_facet { get; set; }
        public List<Multimedium> multimedia { get; set; }
        public string short_url { get; set; }
        public Root root { get; set; }
    }
}
