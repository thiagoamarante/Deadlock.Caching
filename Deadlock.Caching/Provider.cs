using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadlock.Caching
{
    public abstract class Provider
    {
        public Provider(string dataBase)
        {
            this.DataBase = dataBase;
        }

        public string DataBase { get; private set; }

        public abstract TEntity Get<TEntity>(string key, string listName = "default", string linkName = null);

        public abstract void Set<TEntity>(string key, TEntity value, string listName = "default");

        public abstract void Remove(string key, string listName = "default", string linkName = null);       

        public abstract void SetLinkedKey(string linkName, string key, string newKey, string listName = "default");

        public abstract void RemoveLinkedKey(string linkName, string key, string listName = "default");
    }
}
