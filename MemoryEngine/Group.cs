using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryEngine
{
    public class Group
    {

        private readonly string mId;
        public string Id { get { return mId; } }
        IDictionary<string, Item> mItems;


        public Group(string id)
        {
            mId = id;
        }

        public virtual void Save()
        {
            foreach (var key in mItems.Keys)
            {
                //mItems[key];
            }
        }

        public virtual void Load()
        {
            if(mItems == null)
                mItems = new Dictionary<string,Item>();

            mItems.Add("i0001", new Item("i0001"));
            mItems.Add("i0002", new Item("i0002"));
            mItems.Add("i0003", new Item("i0003"));
        }

        public virtual Item[] GetItemsForReview()
        {
           return mItems.Where((item) => item.Value.Priority >= 100).Select((x)=> { return x.Value; }).ToArray();
        }

        public virtual void Update()
        {

        }
             
    }
}
