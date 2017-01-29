﻿
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
	[Activity(Label = "@string/activityLabelEntryAdd")]
	public class EntryActivity : Activity
	{
        public static readonly string TAG = "EntryActivity";

		bool modeEdit;
		int entryEditId;
		Entry entryEdit;
		BookkeeperManager manager = BookkeeperManager.Instance;

		Entry entry;
        EntryType entryType;
        List<Account> entryTypeList;
        DateTime entryDate;

        Button btnDate, btnAddEntry;
        RadioButton radSetIncome, radSetExpense;
        EditText etDescription, etTotalSum;
        TextView tvDescription, tvTotalSum;
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
            SetupAccountSpinner(spinAccount, manager.MoneyAccounts);
            SetupTaxRateSpinner();

			// Run Edit Mode
			if (modeEdit = Intent.GetBooleanExtra("EXTRA_EDIT", false))
			{
				entryEditId = Intent.GetIntExtra("EXTRA_ID", 0);
				Log.Debug(TAG, "ID retrieved: " + Intent.GetIntExtra("EXTRA_ID", 0));
				SetupEditMode();
			}

        }

        private void InstantiateViews()
        {
            btnDate = FindViewById<Button>(Resource.Id.btnDate);
            btnAddEntry = FindViewById<Button>(Resource.Id.btnAddEvent);
            radSetIncome = FindViewById<RadioButton>(Resource.Id.radSetIncome);
            radSetExpense = FindViewById<RadioButton>(Resource.Id.radSetExpense);
			tvDescription = FindViewById<TextView>(Resource.Id.tvDescription);
			tvTotalSum = FindViewById<TextView>(Resource.Id.tvTotalSum);
			etDescription = FindViewById<EditText>(Resource.Id.etDescription);
            etTotalSum = FindViewById<EditText>(Resource.Id.etTotalSum);
            spinType = FindViewById<Spinner>(Resource.Id.spinType);
            spinAccount = FindViewById<Spinner>(Resource.Id.spinAccount);
            spinVAT = FindViewById<Spinner>(Resource.Id.spinVAT);
        }

		private int GetAccountNumberIndex(int number, List<Account> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Number == number)
					return i;
			}
			return 0;
		}

		private int GetTaxRateIndex()
		{
			Log.Debug(TAG, "GetTaxRateIndex:");
			List<TaxRate> list = manager.TaxRates;
			for (int i = 0; i < list.Count; i++)
			{
				Log.Debug(TAG, "List Rate: " + list[i].Rate + " vs. " + entryEdit.Rate);
				if (list[i].Rate == entryEdit.Rate)
					return i;
			}
			return 0;
		}

		private void SetupEditMode()
		{
			this.SetTitle(Resource.String.activityLabelEntryEdit);
			tvDescription.Text = GetString(Resource.String.eventEditInstructions);
			btnAddEntry.Text = GetString(Resource.String.eventUpdateEvent);
			entryEdit = manager.Entries.Where(e => e.Id == entryEditId).ToList()[0];
			entryType = entryEdit.Type;
			UpdateDate(entryEdit.Date);
			etDescription.Text = entryEdit.Description;
			if (entryType == EntryType.Income)
			{
				SetModeIncome();
				spinType.SetSelection(GetAccountNumberIndex(entryEdit.AccountType, manager.IncomeAccounts));
			}
			else
			{
				SetModeExpense();
				spinType.SetSelection(GetAccountNumberIndex(entryEdit.AccountType, manager.ExpenseAccounts));
			}
			spinAccount.SetSelection(GetAccountNumberIndex(entryEdit.AccountTarget, manager.MoneyAccounts));
			etTotalSum.Text = string.Format("{0}",entryEdit.SumTotal);
			spinVAT.SetSelection(GetTaxRateIndex());
			UpdateTotalSum();
		}

        private void SetupViews()
        {

            if (entryDate == DateTime.MinValue)
                UpdateDate(DateTime.Now);

            btnDate.Click += DateSelect_OnClick;

            btnAddEntry.Click += delegate {
				entry = GenerateEntry();
				if (!(entry == null))
				{
					if (modeEdit)
						UpdateEntry();
					else
						AddEntry(entry);
				}
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
            ArrayAdapter adapter = new ArrayAdapter<Account>(this, Resource.Layout.EntrySpinnerItem, list);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void SetupTaxRateSpinner()
        {
            List<TaxRate> list = manager.TaxRates;
            ArrayAdapter adapter = new ArrayAdapter<TaxRate>(this, Resource.Layout.EntrySpinnerItem, list);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinVAT.Adapter = adapter;
			spinVAT.ItemSelected += delegate { UpdateTotalSum(); };
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
            entryTypeList = manager.IncomeAccounts;
            SetupAccountSpinner(spinType, entryTypeList);
        }

        private void SetModeExpense()
        {
            radSetExpense.Checked = true;
            entryType = EntryType.Expense;
            entryTypeList = manager.ExpenseAccounts;
            SetupAccountSpinner(spinType, entryTypeList);
        }

        private void UpdateDate(DateTime newDate)
        {
            entryDate = newDate;
            btnDate.Text = entryDate.ToString("yyyy-MM-dd");
        }

        private void UpdateTotalSum()
        {
            TaxRate t = manager.TaxRates[spinVAT.SelectedItemPosition];
            try
            {
                double sum = Convert.ToDouble(etTotalSum.Text);
				sum /= (1.0 + t.Rate);
				tvTotalSum.Text = String.Format("{0}",Math.Round(sum));
            }
            catch
            {
                tvTotalSum.Text = "-";
            }
        }


        private int GetAccountFromSpinner(Spinner spin)
        {
            string text = spin.SelectedItem.ToString();
			int textIndex = text.IndexOf(':');
			string textData = text.Substring(0, text.Length - (text.Length - textIndex));
            try
            {
                int number = Convert.ToInt32(textData);
				Log.Debug(TAG, "GetAccountFromSpinner: Returning '" + number + "'");
				return number;
            }
            catch
            {
                Log.Debug(TAG, "GetAccountFromSpinner: Error!");
                return 0;
            }
        }

        private float GetTaxRateFromSpinner()
        {
			return manager.TaxRates[spinVAT.SelectedItemPosition].Rate;
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
				e.AccountType = GetAccountFromSpinner(spinType);
				e.AccountTarget = GetAccountFromSpinner(spinAccount);
				e.SumTotal = Convert.ToInt32(etTotalSum.Text);
				e.Rate = GetTaxRateFromSpinner();

				Log.Debug(TAG, "Generating Entry:\n" + e);
				return e;
			}
			return null;
		}

        private void AddEntry(Entry e)
        {
			manager.AddEntry(e);
			string message = GetString(Resource.String.eventToastEntryAdded);
			Toast.MakeText(this, message, ToastLength.Long).Show();
			Finish();
		}

		private void UpdateEntry()
		{
			entryEdit.Type = entry.Type;
			entryEdit.Date = entry.Date;
			entryEdit.Description = entry.Description;
			entryEdit.AccountType = entry.AccountType;
			entryEdit.AccountTarget = entry.AccountTarget;
			entryEdit.SumTotal = entry.SumTotal;
			entryEdit.Rate = entry.Rate;

			manager.UpdateEntry(entryEdit);

			string message = GetString(Resource.String.eventToastEntryUpdated);
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
