namespace SqlChangeDataLog.Triggers
{
    public abstract class SqlTemplates
    {
        public abstract string SelectXml();
        public abstract string ChangeType();
    }
}
