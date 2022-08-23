using System.ComponentModel.DataAnnotations;

namespace NyTimesData.Entity
{
    public class Root
    {
        [Key]
        public long Id { get; set; }
        public string status { get; set; }
        public string copyright { get; set; }
        public string section { get; set; }
        public DateTime last_updated { get; set; }
        public int num_results { get; set; }
        public List<Result> results { get; set; }
    }
}
