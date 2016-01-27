namespace SqlChangeDataLog.Tests.Dto
{
    public class CompositeIdEntityDto
    {
        public CompositeIdEntityDto(int key1, string key2)
        {
            Key2 = key2;
            Key1 = key1;
        }

        public int Key1 { get; set; }
        public string Key2 { get; set; }

    }
}
