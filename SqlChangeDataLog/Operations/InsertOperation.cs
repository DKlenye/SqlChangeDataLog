namespace SqlChangeDataLog.Operations
{
    public class InsertOperation:DataOperation
    {
        public InsertOperation(string[] columns) : base(columns)
        {
        }

        protected override string Name
        {
            get { return "insert"; }
        }
    }
}
