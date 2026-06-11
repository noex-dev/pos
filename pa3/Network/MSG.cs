namespace Server
{
    public class BabynameDetail
    {
        public long Year { get; set; }
        public long Count { get; set; }
    }

    public class MSG
    {
        public enum MessageType {SEARCH, SEARCHRESULT, DETAIL, DETAILRESULT}
        public MessageType Type { get; set; }
        public String? Search {  get; set; }
        public String? Sex { get; set; }
        public List<String>? Names { get; set; }
        public String ? DetailRequest { get; set; }
        public List<BabynameDetail>? Details { get; set; }
        public List<String>? AlternativeNames { get; set; }
    }
}
