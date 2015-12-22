using System;

namespace SqlChangeDataLog.Dtos
{
    public class ChangeLogDto
    {
        public int idChangeLog { get; set; }
        public DateTime date { get; set; }
        public string user { get; set; }
        public char changeType { get; set; }
        public string table { get; set; }
        public string idString { get; set; }
        public string description { get; set; }
    }
}
