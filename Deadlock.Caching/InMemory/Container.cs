using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadlock.Caching.InMemory
{
    public class Container        
    {
        private ConcurrentDictionary<string, object> _Storage = new ConcurrentDictionary<string, object>();
        private ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _Links = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        public Container(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        #region Methods
        public object Get(string key, string linkName = null)
        {
            object value = null;
            if (!string.IsNullOrEmpty(linkName))
            {
                key = this.GetLinkedKey(linkName, key);
            }
            if(!string.IsNullOrEmpty(key))
                this._Storage.TryGetValue(key, out value);
            return value;
        }

        public void Set(string key, object value)
        {
            object currentValue = null;
            if(this._Storage.TryGetValue(key, out currentValue))
            {
                this._Storage.TryUpdate(key, value, currentValue);
            }
            else
            {
                this._Storage.TryAdd(key, value);
            }
        }

        public void Remove(string key, string linkName = null)
        {
            object value = null;
            if (!string.IsNullOrEmpty(linkName))
            {
                key = this.GetLinkedKey(linkName, key);
            }
            if (!string.IsNullOrEmpty(key))
                this._Storage.TryRemove(key, out value);            
        }

        public void SetLinkedKey(string linkName, string key, string newKey)
        {
            ConcurrentDictionary<string, string> link = this.GetLink(linkName);
            string currentKey = null;
            if(link.TryGetValue(newKey, out currentKey))
            {
                link.TryUpdate(newKey, key, currentKey);
            }
            else
            {
                link.TryAdd(newKey, key);
            }
        }

        public void RemoveLinkedKey(string linkName, string key)
        {
            ConcurrentDictionary<string, string> link = this.GetLink(linkName);
            string currentKey = null;
            link.TryRemove(key, out currentKey);
        }
        #endregion

        #region Privates
        private ConcurrentDictionary<string, string> GetLink(string linkName)
        {
            ConcurrentDictionary<string, string> link = null;
            if (!this._Links.TryGetValue(linkName, out link))
            {
                link = new ConcurrentDictionary<string,string>();
                this._Links.TryAdd(linkName, link);
            }
            return link;
        }

        private string GetLinkedKey(string linkName, string key)
        {
            ConcurrentDictionary<string, string> link = this.GetLink(linkName);
            string linkedKey = null;
            link.TryGetValue(key, out linkedKey);
            return linkedKey;
        }
        #endregion

    }
}
