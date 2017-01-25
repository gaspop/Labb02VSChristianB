
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
	[Activity(Label = "EntryActivity")]
	public class EntryActivity : Activity
	{
        public static readonly string TAG = "EntryActivity";

		Entry entry;
        EntryType entryType;
        List<Account> entryTypeList;
        DateTime entryDate;

        Button btnDate, btnAddEntry;
        RadioButton radSetIncome, radSetExpense;
        EditText etDescription, etTotalSum;
        TextView tvTotalSum;
        Spinner spinType, spinAccount, spinVAT;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.EntryActivity);
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
            tvTotalSum = FindViewById<TextView>(Resource.Id.tvTotalSum);
            spinType = FindViewById<Spinner>(Resource.Id.spinType);
            spinAccount = FindViewById<Spinner>(Resource.Id.spinAccount);
            spinVAT = FindViewById<Spinner>(Resource.Id.spinVAT);
        }

        private void SetupViews()
        {

            if (entryDate == DateTime.MinValue)
                UpdateDate(DateTime.Now);

            btnDate.Click += DateSelect_OnClick;

            btnAddEntry.Click += delegate {
				entry = GenerateEntry();
				if (!(entry == null))
					AddEntry(entry); 
			};

            SetModeIncome();

            etTotalSum.TextChanged += delegate { UpdateTotalSum(); };
        }

        private void SetupRadioButtons()
        {
            radSetIncome.Click += delegate{ SetModeIncome(); };

            radSetExpense.Click += delegate { SetModeExpense(); };
        }

        private void SetupAccountSpinner(Spinner spinner, List<Account> list)
        {
			
            //spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            ArrayAdapter adapter = new ArrayAdapter<Account>(this, Resource.Layout.SpinnerItem, list);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void SetupTaxRateSpinner()
        {
            List<TaxRate> list = BookkeeperManager.Instance.TaxRates;
            ArrayAdapter adapter = new ArrayAdapter<TaxRate>(this, Resource.Layout.SpinnerItem, list);
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


        private void SetModeIncome()
        {
            radSetIncome.Checked = true;
            entryType = EntryType.Income;
            entryTypeList = BookkeeperManager.Instance.IncomeAccounts;
            SetupAccountSpinner(spinType, BookkeeperManager.Instance.IncomeAccounts);
        }

        private void SetModeExpense()
        {
            radSetExpense.Checked = true;
            entryType = EntryType.Expense;
            entryTypeList = BookkeeperManager.Instance.ExpenseAccounts;
            SetupAccountSpinner(spinType, BookkeeperManager.Instance.ExpenseAccounts);
        }

        private void UpdateDate(DateTime newDate)
        {
            Log.Debug(TAG, "Updating date: " + newDate.ToLongDateString());
            entryDate = newDate;
            btnDate.Text = entryDate.ToString("yyyy-MM-dd");
        }

        private void UpdateTotalSum()
        {
            TaxRate t = BookkeeperManager.Instance.TaxRates[spinVAT.SelectedItemPosition];
            try
            {
                double sum = Convert.ToDouble(etTotalSum.Text);
                sum *= 1.0 - ((double) t.VAT/100);
                tvTotalSum.Text = String.Format("{0} :-",sum);
            }
            catch
            {
                tvTotalSum.Text = "-";
            }
        }


        private Account GetAccountFromSpinner(Spinner spin, List<Account> list)
        {
            string text = spin.SelectedItem.ToString();
            string[] textData = text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                int dataId = Convert.ToInt32(textData[0]);
                var find = list.Where(a => a.Number == dataId).ToList<Account>();
                if (find.Count == 0)
                {
                    Log.Debug(TAG, "GetAccountFromSpinner: No match");
                    return null;
                }
                else
                {
                    Log.Debug(TAG, "GetAccountFromSpinner: Returning '" + find[0] +"'");
                    return find[0];
                }
            }
            catch
            {
                Log.Debug(TAG, "GetAccountFromSpinner: Error!");
                return null;
            }
        }

        private TaxRate GetTaxRateFromSpinner()
        {
            string text = spinVAT.SelectedItem.ToString();
            int i = text.IndexOf('%');
            string sub = text.Substring(0,text.Length - (text.Length - i));
            int data = Convert.ToInt32(sub);
            var find = BookkeeperManager.Instance.TaxRates.Where(t => t.VAT == data);

            Log.Debug(TAG, "GetTaxRateFromSpinner: Returning '" + find.ToList<TaxRate>()[0] + "'");
            return find.ToList<TaxRate>()[0];
        }

        private bool ValidateData()
        {
			string message = "";
			bool valid = true;
			if (etDescription.Text.Length == 0)
			{
				message = GetString(Resource.String.eventToastErrorDescription);
				valid = false;
			}
			else
			{
				try
				{
					Convert.ToInt32(etTotalSum.Text);
				}
				catch
				{
					message = GetString(Resource.String.eventToastErrorSum);
					valid = false;
				}
			}
			if (!valid)
			{
				Toast.MakeText(this, message, ToastLength.Long).Show();
			}
			return valid;
        }

		private Entry GenerateEntry()
		{
			if (ValidateData())
			{
				Entry e = new Entry();
				e.Type = entryType;
				e.Date = entryDate;
				e.Description = etDescription.Text;
				e.AccountType = GetAccountFromSpinner(spinType, entryTypeList);
				e.AccountTarget = GetAccountFromSpinner(spinAccount, BookkeeperManager.Instance.MoneyAccounts);
				e.SumTotal = Convert.ToInt32(etTotalSum.Text);
				e.VAT = GetTaxRateFromSpinner();

				Log.Debug(TAG, "Generating Entry:\n" + e);
				return e;
			}
			return null;
		}

        private void AddEntry(Entry e)
        {
			BookkeeperManager.Instance.AddEntry(e);
			string message = GetString(Resource.String.eventToastEntryAdded);
			Toast.MakeText(this, message, ToastLength.Long).Show();
			Finish();
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
