namespace SqlChangeDataLog.Operations
{
    public class InsertOperation:DataOperation
    {
        public InsertOperation(string[] columns) : base(columns)
        {
        }

        public override string Name
        {
            get { return "insert"; }
        }
    }
}
