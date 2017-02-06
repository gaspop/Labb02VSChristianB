
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

using Bookkeeper.Model;

namespace Bookkeeper
{
	[Activity(Label = "@string/activityLabelEntryAdd")]
	public class EntryActivity : Activity
	{

		BookkeeperManager manager;
		bool modeEdit;
		int entryEditId;

		Entry entry, entryEdit;
        EntryType entryType;
        DateTime entryDate;

        Button btnDate, btnEntry;
        RadioButton radSetIncome, radSetExpense;
        EditText etDescription, etTotalSum;
        TextView tvDescription, tvTotalSum;
        Spinner spinType, spinAccount, spinVAT;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.EntryActivity);
            SetupActivity();
        }

        private void SetupActivity()
        {
            manager = BookkeeperManager.Instance;
            modeEdit = Intent.GetBooleanExtra("EXTRA_EDIT", false);
            entryEditId = Intent.GetIntExtra("EXTRA_ID", 0);
            InstantiateViews();
            SetupViews();
        }

        private void InstantiateViews()
        {
            btnDate = FindViewById<Button>(Resource.Id.btnDate);
            btnEntry = FindViewById<Button>(Resource.Id.btnEntry);
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

        private void SetupViews()
        {
			SetupButtons();
			SetupRadioButtons();
			SetupTaxRateSpinner();
			SetupAccountSpinner(spinAccount, manager.MoneyAccounts);

			if (modeEdit)
				SetupEditMode();
			else
				SetModeIncome();

            if (entryDate == DateTime.MinValue)
                SetDate(DateTime.Now);

            etTotalSum.TextChanged += delegate { SetTotalSumVAT(); };
        }

		private void SetupButtons()
		{
			btnDate.Click += SelectDate;
			btnEntry.Click += delegate
			{
				entry = GenerateEntry();
				if (!(entry == null))
				{
					if (modeEdit)
						UpdateEntry();
					else
						AddEntry(entry);
				}
			};
		}

        private void SetupRadioButtons()
        {
            radSetIncome.Click += delegate{ SetModeIncome(); };
            radSetExpense.Click += delegate { SetModeExpense(); };
        }

        private void SetupAccountSpinner(Spinner spinner, List<Account> list)
        {
            ArrayAdapter adapter = new ArrayAdapter<Account>(this, Resource.Layout.EntrySpinnerItem, list);
            adapter.SetDropDownViewResource(Resource.Layout.EntrySpinnerDropdownItem);
            spinner.Adapter = adapter;
        }

        private void SetupTaxRateSpinner()
        {
            List<TaxRate> list = manager.TaxRates;
            ArrayAdapter adapter = new ArrayAdapter<TaxRate>(this, Resource.Layout.EntrySpinnerItem, list);
            adapter.SetDropDownViewResource(Resource.Layout.EntrySpinnerDropdownItem);
            spinVAT.Adapter = adapter;
			spinVAT.ItemSelected += delegate { SetTotalSumVAT(); };
        }

		private void SetupEditMode()
		{
			entryEdit = manager.Entries.Where(e => e.Id == entryEditId).ToList()[0];
			entryType = entryEdit.Type;
			if (entryType == EntryType.Income)
			{
				SetModeIncome();
				spinType.SetSelection
						(GetMatchingAccountNumberIndex(entryEdit.TypeAccountNumber, manager.IncomeAccounts));
			}
			else
			{
				SetModeExpense();
				spinType.SetSelection
						(GetMatchingAccountNumberIndex(entryEdit.TypeAccountNumber, manager.ExpenseAccounts));
			}
			tvDescription.Text = GetString(Resource.String.entryEditInstructions);
			etDescription.Text = entryEdit.Description;
			etTotalSum.Text = string.Format("{0}", entryEdit.SumTotal);
			spinAccount.SetSelection
					   (GetMatchingAccountNumberIndex(entryEdit.MoneyAccountNumber, manager.MoneyAccounts));
			spinVAT.SetSelection(GetMatchingTaxRateIndex());
			btnEntry.Text = GetString(Resource.String.entryUpdateEvent);
			SetTitle(Resource.String.activityLabelEntryEdit);
			SetDate(entryEdit.Date);
			SetTotalSumVAT();
		}

        private void SetModeIncome()
		{
			radSetIncome.Checked = true;
			entryType = EntryType.Income;
			SetupAccountSpinner(spinType, manager.IncomeAccounts);
		}

		private void SetModeExpense()
		{
			radSetExpense.Checked = true;
			entryType = EntryType.Expense;
			SetupAccountSpinner(spinType, manager.ExpenseAccounts);
		}

		private void SetTotalSumVAT()
		{
			TaxRate t = (TaxRate)spinVAT.SelectedItem;
			try
			{
				double sum = Convert.ToDouble(etTotalSum.Text);
				sum /= (1.0 + t.Rate);
				tvTotalSum.Text = String.Format("{0}", Math.Round(sum));
			}
			catch
			{
				tvTotalSum.Text = "-";
			}
		}

		private void SetDate(DateTime newDate)
        {
            entryDate = newDate;
            btnDate.Text = entryDate.ToString("yyyy-MM-dd");
        }

        void SelectDate(object sender, EventArgs eventArgs)
		{
			DatePickerFragment frag;
			frag = DatePickerFragment.NewInstance(delegate (DateTime date)
													{
														SetDate(date);
													});
			frag.Show(FragmentManager, DatePickerFragment.TAG);
		}

		private int GetMatchingAccountNumberIndex(int number, List<Account> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Number == number)
					return i;
			}
			return 0;
		}

		private int GetMatchingTaxRateIndex()
		{
			for (int i = 0; i < spinVAT.Count; i++)
			{
				if (((TaxRate)spinVAT.Adapter.GetItem(i)).Rate == entryEdit.Rate)
					return i;
			}
			return 0;
		}

        private int GetAccountFromSpinner(Spinner spin)
        {
			return ((Account) spin.SelectedItem).Number;
        }

        private float GetTaxRateFromSpinner()
        {
			return ((TaxRate) spinVAT.SelectedItem).Rate;
        }

        private bool ValidateData()
        {
			string message = "";
			bool valid = true;
			if (etDescription.Text.Length == 0)
			{
				message = GetString(Resource.String.entryToastErrorDescription);
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
					message = GetString(Resource.String.entryToastErrorSum);
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
				e.TypeAccountNumber = GetAccountFromSpinner(spinType);
				e.MoneyAccountNumber = GetAccountFromSpinner(spinAccount);
				e.SumTotal = Convert.ToInt32(etTotalSum.Text);
				e.Rate = GetTaxRateFromSpinner();

				return e;
			}
			return null;
		}

        private void AddEntry(Entry e)
        {
			manager.AddEntry(e);
			string message = GetString(Resource.String.entryToastEntryAdded);
			Toast.MakeText(this, message, ToastLength.Long).Show();
			Finish();
		}

		private void UpdateEntry()
		{
			entryEdit.Type = entry.Type;
			entryEdit.Date = entry.Date;
			entryEdit.Description = entry.Description;
			entryEdit.TypeAccountNumber = entry.TypeAccountNumber;
			entryEdit.MoneyAccountNumber = entry.MoneyAccountNumber;
			entryEdit.SumTotal = entry.SumTotal;
			entryEdit.Rate = entry.Rate;

			manager.UpdateEntry(entryEdit);

			string message = GetString(Resource.String.entryToastEntryUpdated);
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
            _dateSelectedHandler(selectedDate);
        }

    }

}
