using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryEngine
{
    public class Item
    {
        private readonly string mId;
        public string Id { get { return mId; } }

        public bool IsNeverShow { get; set; }

        public bool IsFavorite { get; set; }

        public Item(string id)
        {
            mId = id;
        }

        private List<History> mHistories;

        public IList<History> Histories { get { return mHistories; } }

        public virtual void Save()
        {

        }

        public virtual void Load()
        {
            mHistories = new List<History>();
            
        }

        public void AddRecord(double extent)
        {
            mHistories.Add(new History() {Time = DateTime.Now,MemoryExtent = extent });
        }

        /// <summary>
        /// set this property by the situation of the content as if it is hard to remember
        /// </summary>
        public double RefFromOutSide { get; set; }

        public virtual double Priority
        {
            get
            {
                return CalculatePriority();
            }
        }

        protected virtual double CalculatePriority()
        {

            if (IsNeverShow)
                return double.MinValue;

            if (Histories.Count <= 0)
                return 100;

            var timeSpan = DateTime.Now.Subtract(GetLatest().Time);
            double val = timeSpan.Days * 24 * 60 + timeSpan.Hours * 60 + timeSpan.Minutes;
            return val / Histories.Count;
        }

        protected virtual History GetLatest()
        {
            if (mHistories.Count <= 0)
                return null;
            return mHistories[mHistories.Count - 1];
        }

        protected virtual History GetEarliest()
        {
            if (mHistories.Count <= 0)
                return null;
            return mHistories[0];
        }

        public class History
        {
            public DateTime Time;
            public double MemoryExtent;
        }
    }

    
}
