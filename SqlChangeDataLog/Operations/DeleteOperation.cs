namespace SqlChangeDataLog.Operations
{
    public class DeleteOperation:DataOperation
    {
        public DeleteOperation(string[] columns) : base(columns)
        {
        }

        protected override string Name
        {
            get { return "delete"; }
        }
    }
}
