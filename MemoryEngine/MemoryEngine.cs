using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryEngine
{
    public class MemoryEngineService
    {
        //group name and group id
        private IDictionary<string, Group> mGroups;

        public virtual void Load()
        {
            if (mGroups == null)
                mGroups = new Dictionary<string, Group>();
            mGroups["g00001"] = new Group("g00001");
            mGroups["g00002"] = new Group("g00002");
        }
        public virtual void Save()
        {
            foreach (var item in mGroup.Keys)
            {
                mGroup[item]
            }
        }


        public virtual Group GetGroup(string id)
        {
            if (!mGroups.ContainsKey(id))
                throw new Exception("there is no group named by +" + id);

            return mGroups[id];
        }
    }

    public class NullMemoryEngineService: MemoryEngineService
    {
        public override Group GetGroup(string id)
        {
            return new Group(id);
        }
        public override void Load()
        {
            
        }
        public override void Save()
        {
            
        }
    }
}
