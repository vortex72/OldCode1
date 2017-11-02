namespace EPWI.Components.Models
{
    public abstract class Repository
    {
        private bool AsAdmin;
        protected EPWIDataContext Db;

        public Repository() : this(false)
        {
        }

        public Repository(bool asAdmin)
        {
            Db = EPWIDataContext.GetInstance(asAdmin);
        }

        public virtual void Save()
        {
            Db.SubmitChanges();
        }
    }
}