using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadlock.Caching
{
    public abstract class CacheContext : IDisposable
    {
        #region Fields
        private ConcurrentDictionary<Type, object> _Sets = new ConcurrentDictionary<Type, object>();
        #endregion

        #region Properties
        public Provider Provider { get; protected set; }
        #endregion

        #region Methods
        public CacheSet<TEntity> CacheSet<TEntity>()
            where TEntity : class
        {
            Type type = typeof(TEntity);
            if(!this._Sets.ContainsKey(type))
            {
                CacheSet<TEntity> set = new CacheSet<TEntity>(this);
                this._Sets.TryAdd(type, set);
            }
            return (CacheSet<TEntity>)this._Sets[type];
        }

        public virtual TEntity Get<TEntity>(string key, string linkName = null)
        {
            return this.Provider.Get<TEntity>(key, linkName: linkName);
        }

        public virtual void Set<TEntity>(string key, TEntity value)
        {
            this.Provider.Set<TEntity>(key, value);
        }

        public virtual void Remove(string key, string linkName = null)
        {
            this.Provider.Remove(key, linkName: linkName);
        }

        public virtual void SetLinkedKey(string linkName, string key, string newKey)
        {
            this.Provider.SetLinkedKey(linkName, key, newKey);
        }

        public virtual void RemoveLinkedKey(string linkName, string key)
        {
            this.Provider.RemoveLinkedKey(linkName, key);
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
