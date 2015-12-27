namespace SqlChangeDataLog.Tests.Dto
{
    public class EntityDto
    {
        public EntityDto(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; private set; }
    }
}
