using System; using System.Collections.Generic; using System.Linq; using System.Text;  using Android.App; using Android.Content; using Android.Runtime; using Android.OS; using Android.Views; using Android.Widget; using Android.Util;  using SQLite;  namespace Bookkeeper.Model {     public class BookkeeperManager     { 		public static readonly string dbPath = System.Environment.GetFolderPath 		                                             (System.Environment.SpecialFolder.Personal) 		                                             + @"database.db"; 		         private static BookkeeperManager instance;          public static BookkeeperManager Instance         {             get             {                 if (instance == null)                 {                     instance = new BookkeeperManager();                 }                 return instance;             }         }  		private SQLiteConnection db;          private BookkeeperManager()         { 			db = new SQLiteConnection(dbPath); 			CreateTableData(); 		}  		private void CreateTableData() 		{ 			db.CreateTable<Entry>(); 			db.CreateTable<Account>();  			if (db.Table<Account>().Count() == 0) 			{ 				db.Insert(new Account() { Number = 3000, Name = "Försäljning", Type = AccountType.Income }); 				db.Insert(new Account() { Number = 3040, Name = "Tjänster", Type = AccountType.Income }); 				db.Insert(new Account() { Number = 3911, Name = "Hyresintäkter", Type = AccountType.Income });  				db.Insert(new Account() { Number = 4010, Name = "Varuinköp", Type = AccountType.Expense }); 				db.Insert(new Account() { Number = 5810, Name = "Biljetter", Type = AccountType.Expense }); 				db.Insert(new Account() { Number = 5830, Name = "Kost och logi", Type = AccountType.Expense }); 				db.Insert(new Account() { Number = 6110, Name = "Kontorsmaterial", Type = AccountType.Expense });  				db.Insert(new Account() { Number = 1910, Name = "Kassa", Type = AccountType.Money }); 				db.Insert(new Account() { Number = 1920, Name = "Bank", Type = AccountType.Money }); 				db.Insert(new Account() { Number = 2010, Name = "Eget kapital", Type = AccountType.Money }); 			}  			db.CreateTable<TaxRate>(); 			if (db.Table<TaxRate>().Count() == 0) 			{ 				db.Insert(new TaxRate() { Rate = 0.25f }); 				db.Insert(new TaxRate() { Rate = 0.12f }); 				db.Insert(new TaxRate() { Rate = 0.06f }); 			}  		}          public List<Entry> Entries         {             get { 				return db.Table<Entry>().ToList();
			}         } 		public List<Account> Accounts 		{ 			get 			{ 				return db.Table<Account>().ToList(); 			} 		}          public List<Account> IncomeAccounts         {             get  			{
				return db.Table<Account>().Where(a => a.Type == AccountType.Income).ToList();  			}         }          public List<Account> ExpenseAccounts         {             get  			{ 				return db.Table<Account>().Where(a => a.Type == AccountType.Expense).ToList();
			}         }          public List<Account> MoneyAccounts         {             get  			{ 				return db.Table<Account>().Where(a => a.Type == AccountType.Money).ToList();
			}         }          public List<TaxRate> TaxRates         {             get  			{ 				return db.Table<TaxRate>().ToList();
			}         }

		public void AddEntry(Entry entry)
		{
			db.Insert(new Entry
			{ 				Type = entry.Type, 				Date = entry.Date, 				Description = entry.Description, 				TypeAccountNumber = entry.TypeAccountNumber, 				MoneyAccountNumber = entry.MoneyAccountNumber, 				SumTotal = entry.SumTotal, 				Rate = entry.Rate
			});         }  		public void UpdateEntry(Entry entry) 		{ 			db.Update(entry); 		}  		public string GetTaxReport() 		{ 			var result = Entries.OrderBy(e => e.Date).Select(e =>
							String.Format("{0}\n{1}\n{2}",  	                              e.Date.ToString("yyyy-MM-dd"),  	                              e.Description, 			                              (e.SumTotal - Math.Round(e.SumTotal / (1.0 + e.Rate))) 			                              * ((e.Type == EntryType.Income) ? 1 : (-1) ))); 			 			return string.Join("\n\n", result); 		}


        /* Hjälpfunktion som genererar händelser för test.
        private void GenerateEntries()
        {
            List<Account> accIn = IncomeAccounts;
            List<Account> accEx = ExpenseAccounts;
            List<Account> accMoney = MoneyAccounts;
            List<TaxRate> VATs = TaxRates;

            string[] eventText = {  "Glass.",
                                    "Öl.",
                                    "Sprit.",
                                    "Kalle Anka-tidning.",
                                    "Böcker.",
                                    "Hyra.",
                                    "Ramlade på fyllan.",
                                    "Flytthjälp.",
                                    "Halkade på ett bananskal.",
                                    "\"Hjälpte\" en \"vän\" nedför en trappa." };

            Random rnd = new Random();
            DateTime start = new DateTime(2016, 1, 1);
            for (int i = 0; i < 40; i++)
            {
                int range = (DateTime.Today - start).Days;
                EntryType t = rnd.Next(2) == 0 ? EntryType.Income : EntryType.Expense;
                Entry e = new Entry();
                e.Type = t;
                e.Date = start.AddDays(rnd.Next(range));
                e.Description = eventText[rnd.Next(eventText.Length)];
                if (t == EntryType.Income)
                    e.TypeAccountNumber = accIn[rnd.Next(accIn.Count)].Number;
                else
                    e.TypeAccountNumber = accEx[rnd.Next(accEx.Count)].Number;
                e.MoneyAccountNumber = accMoney[rnd.Next(accMoney.Count)].Number;
                e.SumTotal = 25 + (rnd.Next(25) * 25) + (rnd.Next(2) * (rnd.Next(25) * 250));
                e.Rate = VATs[rnd.Next(VATs.Count)].Rate;
                AddEntry(e);
            }
        }         */
    } }