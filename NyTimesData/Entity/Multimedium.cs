using System.ComponentModel.DataAnnotations;

namespace NyTimesData.Entity
{
    public class Multimedium
    {
        [Key]
        public long Id { get; set; }
        public long ResultId { get; set; }
        public string url { get; set; }
        public string format { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public string caption { get; set; }
        public string copyright { get; set; }
        public Result result { get; set; }
    }
}
