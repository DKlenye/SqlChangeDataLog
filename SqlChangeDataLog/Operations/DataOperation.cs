namespace SqlChangeDataLog.Operations
{
    public abstract class DataOperation
    {
        protected DataOperation(string[] columns)
        {
            Columns = columns;
        }

        public string[] Columns { get; private set; }

        public abstract string Name { get; }
    }
}
