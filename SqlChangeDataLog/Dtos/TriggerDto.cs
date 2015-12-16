namespace SqlChangeDataLog.Dtos
{
    public class TriggerDto
    {
        public string TriggerName { get; set; }
        public string TableName { get; set; }

        public string GetOperation()
        {
            var nameArray = TriggerName.Split('_');
            var len = nameArray.Length;
            return nameArray[len-2];
        }
    }
}
