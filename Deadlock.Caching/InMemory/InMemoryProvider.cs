using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadlock.Caching.InMemory
{
    public class InMemoryProvider : Provider
    {
        #region Fields
        private static ConcurrentDictionary<string, DataBase> _DataBases = new ConcurrentDictionary<string, DataBase>();
        private DataBase _DataBase;
        #endregion

        public InMemoryProvider(string dataBase = "default")
            :base(dataBase)
        {
            this._DataBase = this.GetDataBase();
        }

        #region Methods
        public override TEntity Get<TEntity>(string key, string listName = "default", string linkName = null)            
        {
            Container container = this._DataBase.GetContainer(listName, true);
            object value = container.Get(key, linkName);
            return (TEntity)value;
        }

        public override void Set<TEntity>(string key, TEntity value, string listName = "default")
        {
            Container container = this._DataBase.GetContainer(listName, true);
            container.Set(key, value);
        }

        public override void Remove(string key, string listName = "default", string linkName = null)
        {
            Container container = this._DataBase.GetContainer(listName, true);
            container.Remove(key, linkName);
        }

        public override void SetLinkedKey(string linkName, string key, string newKey, string listName = "default")
        {
            Container container = this._DataBase.GetContainer(listName, true);
            container.SetLinkedKey(linkName, key, newKey);
        }

        public override void RemoveLinkedKey(string linkName, string key, string listName = "default")
        {
            Container container = this._DataBase.GetContainer(listName, true);
            container.RemoveLinkedKey(linkName, key);
        }
        #endregion

        #region Privates
        private DataBase GetDataBase()
        {
            DataBase dataBase = null;
            if (!_DataBases.TryGetValue(this.DataBase, out dataBase))
            {
                dataBase = new DataBase(this.DataBase);
                _DataBases.TryAdd(this.DataBase, dataBase);
            }
            return dataBase;
        }
        #endregion                
    }
}
