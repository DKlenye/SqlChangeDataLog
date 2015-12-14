namespace SqlChangeDataLog.Operations
{
    public class DeleteOperation:DataOperation
    {
        public DeleteOperation(string[] columns) : base(columns)
        {
        }

        public override string Name
        {
            get { return "delete"; }
        }
    }
}
