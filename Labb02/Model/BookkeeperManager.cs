﻿using System; using System.Collections.Generic; using System.Linq; using System.Text;  using Android.App; using Android.Content; using Android.Runtime; using Android.OS; using Android.Views; using Android.Widget; using Android.Util;  using SQLite;  namespace Labb02.Model {     public class BookkeeperManager     {         public static readonly string TAG = "Bookkeeper"; 		public static readonly string dbPath = System.Environment.GetFolderPath 		                                             (System.Environment.SpecialFolder.Personal) 		                                             + @"database.db"; 		         private static BookkeeperManager instance;          public static BookkeeperManager Instance         {             get             {                 if (instance == null)                 {                     instance = new BookkeeperManager();                 }                 return instance;             }         }  		private SQLiteConnection db;          private BookkeeperManager()         { 			db = new SQLiteConnection(dbPath); 			CreateTableData(); 		}  		private void CreateTableData() 		{ 			db.CreateTable<Entry>(); 			db.CreateTable<Account>(); 			if (db.Table<Account>().Count() == 0) 			{ 				Log.Debug(TAG, "Generating Accounts"); 				db.Insert(new Account() { Number = 3000, Name = "Försäljning", Type = AccountType.Income }); 				db.Insert(new Account() { Number = 3040, Name = "Tjänster", Type = AccountType.Income }); 				db.Insert(new Account() { Number = 3911, Name = "Hyresintäkter", Type = AccountType.Income });  				db.Insert(new Account() { Number = 4010, Name = "Varuinköp", Type = AccountType.Expense }); 				db.Insert(new Account() { Number = 5830, Name = "Kost och logi", Type = AccountType.Expense }); 				db.Insert(new Account() { Number = 6110, Name = "Kontorsmaterial", Type = AccountType.Expense });  				db.Insert(new Account() { Number = 1910, Name = "Kassa", Type = AccountType.Money }); 				db.Insert(new Account() { Number = 1920, Name = "Bank", Type = AccountType.Money }); 			}  			db.CreateTable<TaxRate>(); 			if (db.Table<TaxRate>().Count() == 0) 			{ 				Log.Debug(TAG, "Generating TaxRates"); 				db.Insert(new TaxRate() { Rate = 0.25f }); 				db.Insert(new TaxRate() { Rate = 0.12f }); 				db.Insert(new TaxRate() { Rate = 0.06f }); 			}   		}          public List<Entry> Entries         {             get  			{ 				return db.Table<Entry>().ToList();
			}         } 		public List<Account> Accounts 		{ 			get 			{ 				return db.Table<Account>().ToList(); 			} 		}          public List<Account> IncomeAccounts         {             get  			{
				return db.Table<Account>().Where(a => a.Type == AccountType.Income).ToList();  			}         }          public List<Account> ExpenseAccounts         {             get  			{ 				return db.Table<Account>().Where(a => a.Type == AccountType.Expense).ToList();
			}         }          public List<Account> MoneyAccounts         {             get  			{ 				return db.Table<Account>().Where(a => a.Type == AccountType.Money).ToList();
			}         }          public List<TaxRate> TaxRates         {             get  			{ 				return db.Table<TaxRate>().ToList();
			}         }

		public void AddEntry(Entry entry)
		{
			db.Insert(new Entry
			{ 				Type = entry.Type, 				Date = entry.Date, 				Description = entry.Description, 				AccountType = entry.AccountType, 				AccountTarget = entry.AccountTarget, 				SumTotal = entry.SumTotal, 				Rate = entry.Rate
			});         }  		public void UpdateEntry(Entry entry) 		{ 			db.Update(entry); 		}  		public string EntryToString() 		{ 			return string.Join("\n", Entries); 		}  		public string GetTaxReport() 		{ 			string report = "\n"; 			List<TaxRate> t = TaxRates; 			var result = Entries.OrderBy(e => e.Date).Select(e =>
							String.Format("{0}:\n{1}\nSkatt: {2}",  	                              e.Date.ToString("yyyy-MM-dd"),  	                              e.Description, 			                              (e.SumTotal - Math.Round(e.SumTotal / (1.0 + e.Rate))) 			                              * ((e.Type == EntryType.Income) ? 1 : (-1) ))); 			 			report += string.Join("\n\n", result) + "\n"; 			return report; 		}      } }