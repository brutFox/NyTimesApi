namespace NyTimesSharedModels.Models
{
    public class Result
    {
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
        public List<string> des_facet { get; set; }
        public List<string> org_facet { get; set; }
        public List<string> per_facet { get; set; }
        public List<string> geo_facet { get; set; }
        public List<Multimedium> multimedia { get; set; }
        public string short_url { get; set; }
        public Root root { get; set; }
    }
}
