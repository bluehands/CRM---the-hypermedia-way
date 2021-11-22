namespace CRM.Domain
{
    public abstract record IdBase
    {
        public Guid Value { get; }

        protected IdBase(Guid value)
        {
            Value = value;
        }

        protected IdBase()
        {
            Value = Guid.NewGuid();
        }

    }
}