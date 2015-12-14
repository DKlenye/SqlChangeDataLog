namespace SqlChangeDataLog.Operations
{
    public class UpdateOperation:DataOperation
    {
        public UpdateOperation(string[] columns) : base(columns)
        {
        }

        public override string Name
        {
            get { return "update"; }
        }
    }
}
