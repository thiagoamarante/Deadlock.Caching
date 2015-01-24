using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadlock.Caching
{
    public class CacheSet<TEntity>
    {
        public CacheSet(CacheContext context)
        {
            this.Context = context;
        }

        private CacheContext Context { get; set; }

        public virtual TEntity Get(string key, string linkName = null)
        {
            return this.Context.Provider.Get<TEntity>(key, typeof(TEntity).Name, linkName);            
        }

        public virtual void Set(string key, TEntity value)
        {
            this.Context.Provider.Set<TEntity>(key, value, typeof(TEntity).Name);            
        }        

        public virtual void Remove(string key, string linkName = null)
        {
            this.Context.Provider.Remove(key, typeof(TEntity).Name, linkName);
        }

        public virtual void SetLinkedKey(string linkName, string key, string newKey)
        {
            this.Context.Provider.SetLinkedKey(linkName, key, newKey, typeof(TEntity).Name);
        }

        public virtual void RemoveLinkedKey(string linkName, string key)
        {
            this.Context.Provider.RemoveLinkedKey(linkName, key, typeof(TEntity).Name);
        }
    }
}
