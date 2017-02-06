using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Bookkeeper.Model
{
    class EntryAdapter: BaseAdapter<Entry>
    {
        BookkeeperManager manager;
        Activity activity;
        List<Entry> list;

        public EntryAdapter(Activity activity)
        {
            manager = BookkeeperManager.Instance;
            this.activity = activity;
			list = manager.Entries.OrderBy(e => e.Date).ToList();
        }

		public override Entry this[int position]
		{
			get { return list[position]; }
		}

		public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.EntryListItem, parent, false);
           
            TextView tvDate = view.FindViewById<TextView>(Resource.Id.entryItemDate);
            TextView tvDescription = view.FindViewById<TextView>(Resource.Id.entryItemDescription);
            TextView tvSum = view.FindViewById<TextView>(Resource.Id.entryItemSum);

            Entry entry = list[position];
            tvDate.Text = entry.Date.ToString("yyyy-MM-dd");
            tvDescription.Text = entry.Description;
            tvSum.Text = entry.SumTotal + " kr";
			tvSum.SetTextColor
			     (entry.Type == EntryType.Income ? Color.ForestGreen : Color.OrangeRed);

            return view;
		}

    }
}
