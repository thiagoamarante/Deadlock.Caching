using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadlock.Caching.InMemory
{
    public class DataBase 
    {
        private ConcurrentDictionary<string, Container> _Containers = new ConcurrentDictionary<string, Container>();

        public DataBase(string name)
        {
            this.Name = name;
            this._Containers.TryAdd("default", new Container("default"));
        }

        public string Name { get; private set; }

        public Container GetContainer(string name, bool createIfNotExists = false)            
        {
            Container container = null;
            if(!_Containers.TryGetValue(name, out container) && createIfNotExists)
            {
                container = new Container(name);
                _Containers.TryAdd(name, container);
            }
            return container;
        }
    }
}
