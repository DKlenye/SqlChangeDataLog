namespace SqlChangeDataLog.Operations
{
    public class UpdateOperation:DataOperation
    {
        public UpdateOperation(string[] columns) : base(columns)
        {
        }

        protected override string Name
        {
            get { return "update"; }
        }
    }
}
