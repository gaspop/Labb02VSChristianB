
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
using Android.Util;

using Labb02.Model;

namespace Labb02
{
	[Activity(Label = "EventActivity")]
	public class EventActivity : Activity
	{
        public static readonly string TAG = "EventActivity";

        Button btnDate;
        DateTime eventDate;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Event);
            // Create your application here

            // Run View Element setup
            InstantiateViews();
            SetupViews();
        }

        private void InstantiateViews()
        {
            btnDate = FindViewById<Button>(Resource.Id.btnDate);
        }

        private void SetupViews()
        {
            if (eventDate == DateTime.MinValue)
                UpdateDate(DateTime.Now);

            btnDate.Click += DateSelect_OnClick;
        }

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag;
            frag = DatePickerFragment.NewInstance(delegate (DateTime date) 
                                                    {
                                                        UpdateDate(date);
                                                    });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void UpdateDate(DateTime newDate)
        {
            Log.Debug(TAG, "Updating date: " + newDate.ToLongDateString());
            eventDate = newDate;
            btnDate.Text = eventDate.ToShortDateString();
        }

        private void AddEntry(Entry entry)
        {

        }

    }

    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        public static readonly string TAG = "EVENTDATEPICKER";

        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime current = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity, this,
                current.Year, current.Month -1, current.Day);

            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayofMonth)
        {
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayofMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }


    }

}
