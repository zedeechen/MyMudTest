namespace ZDMMO
{
    public abstract class IBaseItem
    {
        void_long_delegate dlgAmountChange;
        protected long MAmount
        {
            get;
            private set;
        }
        public virtual void Add(long amount)
        {
            MAmount += amount;
            if (dlgAmountChange != null)
                dlgAmountChange(MAmount);
        }
    }
}
