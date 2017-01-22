
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

        DateTime eventDate;
        Button btnDate, btnAddEntry;
        RadioButton radSetIncome, radSetExpense;
        EditText etDescription, etTotalSum;
        Spinner spinType, spinAccount, spinVAT;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Event);
            // Create your application here

            // Run View Element setup
            InstantiateViews();
            SetupViews();
            SetupRadioButtons();
            SetupAccountSpinner(spinAccount, BookkeeperManager.Instance.MoneyAccounts);
            SetupTaxRateSpinner();
        }

        private void InstantiateViews()
        {
            btnDate = FindViewById<Button>(Resource.Id.btnDate);
            btnAddEntry = FindViewById<Button>(Resource.Id.btnAddEvent);
            radSetIncome = FindViewById<RadioButton>(Resource.Id.radSetIncome);
            radSetExpense = FindViewById<RadioButton>(Resource.Id.radSetExpense);
            etDescription = FindViewById<EditText>(Resource.Id.etDescription);
            etTotalSum = FindViewById<EditText>(Resource.Id.etTotalSum);
            spinType = FindViewById<Spinner>(Resource.Id.spinType);
            spinAccount = FindViewById<Spinner>(Resource.Id.spinAccount);
            spinVAT = FindViewById<Spinner>(Resource.Id.spinVAT);
        }

        private void SetupViews()
        {
            if (eventDate == DateTime.MinValue)
                UpdateDate(DateTime.Now);

            btnDate.Click += DateSelect_OnClick;
        }

        private void SetupRadioButtons()
        {
            radSetIncome.Click += delegate
            {
                SetupAccountSpinner(spinType, BookkeeperManager.Instance.IncomeAccounts);
            };

            radSetExpense.Click += delegate
            {
                SetupAccountSpinner(spinType, BookkeeperManager.Instance.ExpenseAccounts);
            };
        }

        private void SetupAccountSpinner(Spinner spinner, List<Account> list)
        {
            //spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, list);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void SetupTaxRateSpinner()
        {
            List<TaxRate> list = BookkeeperManager.Instance.TaxRates;
            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, list);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinVAT.Adapter = adapter;
        }

        /*
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Log.Debug(TAG, "Item selected!");
        }
        */

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
            btnDate.Text = eventDate.ToString("yyyy-MM-dd");
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
