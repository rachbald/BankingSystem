using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using OnlineBankingSystem.AccReference;
using OnlineBankingSystem.UsersReference;
using OnlineBankingSystem.Models;
using OnlineBankingSystem.TransReference;
using OnlineBankingSystem.TransTypesReference;
using OnlineBankingSystem.AccReference;
using OnlineBankingSystem.AccTypesReference;
using OnlineBankingSystem.FixedRatesReference;
using OnlineBankingSystem.CurrencyReference;
using OnlineBankingSystem.LogsReference;
using OnlineBankingSystem.AppReference;

//using iTextSharp.text.pdf;
using System.Net;
using System.Text.RegularExpressions;
using System;

using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;
using System.Net.Mail;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.UI.WebControls;
using iTextSharp.text.html.simpleparser;
using OnlineBankingSystem.RolesReference;



namespace OnlineBankingSystem.Controllers
{
    public class AccountsController : Controller
    {

        public ActionResult Balances()
        {
            return View();
        }

        public JsonResult Balance()
        {
            string username = HttpContext.User.Identity.Name;

            List<OnlineBankingSystem.AccReference.Account> accounts = new AccountsServiceClient().GetAccounts();
            //List<OnlineBankingSystem.AccReference.Account> accounts2 = new AccountsServiceClient().GetUserAccounts(username);
            List<UserHomePageModel> summary = new List<UserHomePageModel>();
            UsersReference.User user = new UsersServiceClient().GetUserByUsername(username);
            int userId = user.Id;

            foreach (OnlineBankingSystem.AccReference.Account a in accounts)
            {
                if (a.User == userId)
                {
                    UserHomePageModel model = new UserHomePageModel();
                    model.AccountNo = a.AccountNo;
                    model.Currency = a.Currency;
                    model.Funds = a.Balance.ToString();


                    summary.Add(model);
                }
            }

            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckFixedAccounts()
        {
            string status = "";

            string username = HttpContext.User.Identity.Name;
            List<FixedAccountModel> models = new List<FixedAccountModel>();

            try
            {
                List<OnlineBankingSystem.AccReference.Account> accounts = new AccountsServiceClient().GetAccounts();

                
                foreach (OnlineBankingSystem.AccReference.Account a in accounts)
                {
                    int userId = new UsersServiceClient().GetUserByUsername(username).Id;
                    if (a.User.Equals(userId))
                    {
                        if (a.Type == 3)
                        {
                            //Rate Id
                            int temp = int.Parse(a.Rate.ToString());
                            //Get rate belonging to Id temp
                            FixedRatesReference.FixedRate rate = new FixedRatesServiceClient().GetRateById(temp);
                            //get rate months
                            int months = rate.Months;
                            //check whether account exceeded expiry date
                            int state = a.Creation.AddMonths(months).CompareTo(System.DateTime.Now);
                            if (state <= 0)
                            {
                                FixedAccountModel model = new FixedAccountModel();
                                model.Id = a.Id;
                                model.AccountNo = a.AccountNo;
                                model.Balance = a.Balance;
                                model.Duration = months + "months";
                                model.Rate = rate.Interest;
                                decimal interest = (model.Rate / 100) * a.Balance;
                                decimal monthlyInterest = interest / (12 / months);
                                decimal tax = monthlyInterest * (85 / 100);
                                model.Interest = decimal.Round(monthlyInterest, 2, MidpointRounding.AwayFromZero);
                                model.TaxReduced = decimal.Round(model.Interest, 2, MidpointRounding.AwayFromZero);
                                model.NewBalance = a.Balance + model.TaxReduced;

                                models.Add(model);

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json("System failed to load expired accounts", JsonRequestBehavior.AllowGet);
            }

            return Json(models, JsonRequestBehavior.AllowGet);

        }


        public JsonResult RenewAccount()
        {
            string status = "";
            string accountNo = HttpContext.Request.QueryString["id"];

            try
            {

                AccReference.Account a = new AccountsServiceClient().GetAccountByNo(accountNo);
                //Rate Id
                int temp = int.Parse(a.Rate.ToString());
                //Get rate belonging to Id temp
                FixedRatesReference.FixedRate rate = new FixedRatesServiceClient().GetRateById(temp);
                //get rate months
                int months = rate.Months;
                //check whether account exceeded expiry date
                int state = a.Creation.AddMonths(months).CompareTo(System.DateTime.Now);
                FixedAccountModel model = new FixedAccountModel();
                model.AccountNo = a.AccountNo;
                model.Balance = a.Balance;
                model.Duration = months + "months";
                model.Rate = rate.Interest;
                decimal interest = (model.Rate / 100) * a.Balance;
                decimal monthlyInterest = interest / (12 / months);
                decimal tax = monthlyInterest * (85 / 100);
                model.Interest = decimal.Round(monthlyInterest, 2, MidpointRounding.AwayFromZero);
                model.TaxReduced = decimal.Round(model.Interest, 2, MidpointRounding.AwayFromZero);
                model.NewBalance = a.Balance + model.TaxReduced;

                a.Balance = model.NewBalance;
                a.Creation = System.DateTime.Now.ToLocalTime();

                if (new AccountsServiceClient().UpdateAccount(a))
                {
                    status = "account extended";
                }else
                {
                    status = "system was unable to extend the account";
                }

            }
            catch (Exception e)
            {
                status = "system was unable to extend the account";
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TransferFixed()
        {
            string status = "";

            try {

                string accountNo = HttpContext.Request.QueryString["accountNo"];
                string external = HttpContext.Request.QueryString["external"];

            }
            catch (Exception e) { }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetAccountDetails()
        {
            string no = HttpContext.Request.QueryString["id"];


            List<AccountModel> models = new List<AccountModel>();
            try
            {
                AccReference.Account a = new AccountsServiceClient().GetAccountByNo(no);


                AccountModel model = new AccountModel();
                model.Id = a.Id;
                model.AccountNo = a.AccountNo;
                model.Name = a.Name;
                model.Type = new AccountTypesServiceClient().GetAccountTypeById(a.Type).Name;
                model.Currency = a.Currency;
                model.Creation = a.Creation;
                model.Balance = a.Balance;
                model.User = a.User;

                models.Add(model);
            }
            catch (Exception e)
            { }

            return Json(models, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public ActionResult Accounts(string accountNo)
        {
            string username = HttpContext.User.Identity.Name;
            List<OnlineBankingSystem.AccReference.Account> accounts = new AccountsServiceClient().GetAccounts();

            List<UserHomePageModel> summary = new List<UserHomePageModel>();



            foreach (OnlineBankingSystem.AccReference.Account a in accounts)
            {
                int userId = new UsersServiceClient().GetUserByUsername(username).Id;
                if (a.User.Equals(userId))
                {
                    UserHomePageModel model = new UserHomePageModel();
                    model.AccountNo = a.AccountNo;
                    model.Currency = a.Currency;
                    model.Funds = a.Balance.ToString();

                    summary.Add(model);
                }
            }


            return View("Accounts", accounts);
        }

        public ActionResult AccountTransactions()
        {
            return View();
        }

        [Authorize]
        public JsonResult AccountTransaction()
        {
            List<TransactionsModel> transactions = new List<TransactionsModel>();

            try
            {
                string accountNo = HttpContext.Request.QueryString["id"];


                List<OnlineBankingSystem.TransReference.Transaction> accountTrans = new TransactionsServiceClient().GetAccountTransactions(accountNo);



                foreach (OnlineBankingSystem.TransReference.Transaction t in accountTrans)
                {
                    TransactionsModel model = new TransactionsModel();
                    model.Date = t.Date.ToShortDateString();

                    AccReference.Account a = new AccountsServiceClient().GetAccountByNo(accountNo);
                    model.Currency = a.Currency;

                    if (t.Type == 1)
                    {
                        model.Details = "Transfer from Account " + t.SecondaryAccount;
                        model.Amount = "+" + t.Amount;
                    }
                    else if (t.Type == 2)
                    {
                        model.Details = "Transfer to Account " + t.SecondaryAccount;
                        model.Amount = "-" + t.Amount;
                    }

                    transactions.Add(model);
                }

            }
            catch (Exception e)
            { }

            return Json(transactions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterTransactions()
        {
            return View();
        }

        [Authorize]
        public JsonResult FilterTransaction()
        {
            List<TransactionsModel> transactions = new List<TransactionsModel>();
            try
            {
                string start = HttpContext.Request.QueryString["start"];
                string end = HttpContext.Request.QueryString["end"];

                string accountNo = HttpContext.Request.QueryString["id"];
                System.DateTime startd = System.DateTime.Parse(start);
                System.DateTime endd = System.DateTime.Parse(end);

                List<OnlineBankingSystem.TransReference.Transaction> accountTrans = new TransactionsServiceClient().FilterAccountTransactions(accountNo, startd, endd);


                foreach (OnlineBankingSystem.TransReference.Transaction t in accountTrans)
                {
                    TransactionsModel model = new TransactionsModel();
                    model.Date = t.Date.ToShortDateString();

                    AccReference.Account a = new AccountsServiceClient().GetAccountByNo(accountNo);
                    model.Currency = a.Currency;

                    if (t.Type == 1)
                    {
                        model.Details = "Transfer from Account " + t.SecondaryAccount;
                        model.Amount = "+" + t.Amount;
                    }
                    else if (t.Type == 2)
                    {
                        model.Details = "Transfer to Account " + t.SecondaryAccount;
                        model.Amount = "-" + t.Amount;
                    }

                    transactions.Add(model);
                }
            }
            catch (Exception e)
            { }

            return Json(transactions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRates()
        {
            List<OnlineBankingSystem.FixedRatesReference.FixedRate> rates = new FixedRatesServiceClient().GetFixedRates();
            List<System.Web.UI.WebControls.ListItem> rates2 = new List<System.Web.UI.WebControls.ListItem>();

            foreach (OnlineBankingSystem.FixedRatesReference.FixedRate t in rates)
            {
                rates2.Add(new System.Web.UI.WebControls.ListItem(t.Months.ToString() + "months - " + t.Interest + "% interest Rate", t.Id.ToString()));

            }

            return Json(rates2, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountType()
        {
            List<OnlineBankingSystem.AccTypesReference.AccountType> accountTypes = new AccountTypesServiceClient().GetAccountTypes();
            List<System.Web.UI.WebControls.ListItem> accountTypes2 = new List<System.Web.UI.WebControls.ListItem>();

            foreach (OnlineBankingSystem.AccTypesReference.AccountType t in accountTypes)
            {
                accountTypes2.Add(new System.Web.UI.WebControls.ListItem(t.Name.ToString(), t.Id.ToString()));

            }


            return Json(accountTypes2, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOtherAccount()
        {
            string secondaryAccount = HttpContext.Request.QueryString["other"];
            OnlineBankingSystem.AccReference.Account a = new AccountsServiceClient().GetAccountByNo(secondaryAccount);

            return Json(secondaryAccount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserAccounts()
        {
            List<System.Web.UI.WebControls.ListItem> summary = new List<System.Web.UI.WebControls.ListItem>();
            try
            {

                string username = HttpContext.User.Identity.Name;
                List<OnlineBankingSystem.AccReference.Account> accounts = new AccountsServiceClient().GetAccounts();




                foreach (OnlineBankingSystem.AccReference.Account a in accounts)
                {
                    int userId = new UsersServiceClient().GetUserByUsername(username).Id;
                    if (a.User.Equals(userId))
                    {
                        if (a.Type != 3)
                        {
                            summary.Add(new System.Web.UI.WebControls.ListItem(a.AccountNo + "\t\t\t-\t\t\t" + a.Name.ToString() + "\t" + a.Currency + " " + a.Balance, a.Id.ToString()));
                        }
                        else
                        {
                            int temp = int.Parse(a.Rate.ToString());
                            FixedRatesReference.FixedRate rate = new FixedRatesServiceClient().GetRateById(temp);
                            int months = rate.Months;
                            int state = a.Creation.AddMonths(months).CompareTo(System.DateTime.Now);
                            if (state <= 0)
                            {
                                summary.Add(new System.Web.UI.WebControls.ListItem(a.AccountNo + "\t\t\t-\t\t\t" + a.Name.ToString() + "\t" + a.Currency + " " + a.Balance, a.Id.ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            { }

            return Json(summary, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult CreateAccounts()
        {
            return View();
        }

        public JsonResult CreateAccount()
        {
            List<System.Web.UI.WebControls.ListItem> accountTypes2 = new List<System.Web.UI.WebControls.ListItem>();

            try
            {
                List<OnlineBankingSystem.AccTypesReference.AccountType> accountTypes = new AccountTypesServiceClient().GetAccountTypes();


                foreach (OnlineBankingSystem.AccTypesReference.AccountType t in accountTypes)
                {
                    accountTypes2.Add(new System.Web.UI.WebControls.ListItem(t.Name.ToString(), t.Id.ToString()));

                }

                new UsersServiceClient().UpdateToken(HttpContext.User.Identity.Name);
            }
            catch (Exception e)
            { }

            return Json(accountTypes2, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAccount()
        {

            string status = "";
            decimal balance = 0;

            try
            {
                string date = System.DateTime.Now.ToLocalTime().ToString();
                string date2 = date.Replace("/", "");
                string date3 = date2.Replace(":", "");
                string date4 = date3.Replace(" ", "");
                string date5 = date4.Substring(0, 4);

                string token = HttpContext.Request.QueryString["token"];
                string username = HttpContext.User.Identity.Name;
                string name = HttpContext.Request.QueryString["name"];

                if (token.Equals("") || token == null)
                {
                    status = "Please enter token to create the account";
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if ((date.Length == 0) || (username.Length == 0) || (name.Length == 0))
                    {
                        status = "Please enter all details";
                        return Json(status, JsonRequestBehavior.AllowGet);
                    }

                    if (new UsersReference.UsersServiceClient().Test(username, token))
                    {
                        FullAccountModel newAccount = new FullAccountModel();
                        newAccount.myAccount = new OnlineBankingSystem.AccReference.Account();
                        newAccount.myAccount.AccountNo = date5 + date4.Substring(8, 4);
                        newAccount.myAccount.Name = HttpContext.Request.QueryString["name"];
                        if (HttpContext.Request.QueryString["type"] == "")
                        {
                            status = "Please enter all details";
                            return Json(status, JsonRequestBehavior.AllowGet);
                        }
                        newAccount.myAccount.Type = int.Parse(HttpContext.Request.QueryString["type"]);



                        newAccount.myAccount.Currency = HttpContext.Request.QueryString["currency"];
                        newAccount.myAccount.Creation = System.DateTime.Now;
                        if (HttpContext.Request.QueryString["balance"] == "")
                        {
                            status = "Please enter all details";
                            return Json(status, JsonRequestBehavior.AllowGet);
                        }

                        if (HttpContext.Request.QueryString["balance"] == "")
                        {
                            status = "Please enter all details";
                            return Json(status, JsonRequestBehavior.AllowGet);
                        }

                        newAccount.myAccount.Balance = decimal.Parse(HttpContext.Request.QueryString["balance"]);
                        newAccount.myAccount.User = new UsersServiceClient().GetUserByUsername(HttpContext.User.Identity.Name).Id;


                        string type = new AccountTypesServiceClient().GetAccountTypeById(newAccount.myAccount.Type).Name;

                        if (type.Equals("Fixed"))
                        {
                            newAccount.myAccount.Rate = int.Parse(HttpContext.Request.QueryString["rate"]);
                        }

                        if (HttpContext.Request.QueryString["other"] == "")
                        {
                            status = "Please enter other account number be transfered from";
                            return Json(status, JsonRequestBehavior.AllowGet);
                        }
                        int secondaryAccount = int.Parse(HttpContext.Request.QueryString["other"]);
                        OnlineBankingSystem.AccReference.Account other = new AccountsServiceClient().GetAccountById(secondaryAccount);

                        if (HttpContext.Request.QueryString["currency"] == "")
                        {
                            status = "Please choose the currency";
                            return Json(status, JsonRequestBehavior.AllowGet);
                        }

                        newAccount.myAccount.Currency = HttpContext.Request.QueryString["currency"];

                        if (newAccount.myAccount.Currency != other.Currency)
                        {
                            balance = Conversion(newAccount.myAccount.Balance, newAccount.myAccount.Currency, other.Currency);
                        }

                        if (balance == 0)
                        {

                            status = "Please enter the balance to be transfered";
                            return Json(status, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {

                            decimal demand = Conversion(balance, newAccount.myAccount.Currency, "EUR");
                            decimal temp = Conversion(10, "EUR", newAccount.myAccount.Currency);

                            decimal userRequest = decimal.Round(temp, 2, MidpointRounding.AwayFromZero);

                            if (demand < 10)
                            {
                                status = "You must enter a value greater than EUR10 (" + newAccount.myAccount.Currency + " " + temp.ToString("F") + ")";
                            }
                            else if (other.Balance <= balance)
                            {
                                status = "There isn't enough balance in the account. Please try another secondary account";
                            }
                            else
                            {
                                decimal newBalance = other.Balance - balance;
                                other.Balance = newBalance;
                                new AccountsServiceClient().UpdateAccount(other);

                                string newNumber = new AccountsServiceClient().AddUserAccount(newAccount.myAccount);


                                TransReference.Transaction newA = new TransReference.Transaction();
                                newA.User = newAccount.myAccount.User;
                                newA.Type = 2;
                                newA.Amount = balance;
                                newA.CurrentAccount = newNumber;
                                newA.SecondaryAccount = other.AccountNo;
                                newA.Date = System.DateTime.Now;
                                newA.Currency = newAccount.myAccount.Currency;

                                TransReference.Transaction old = new TransReference.Transaction();
                                old.User = newAccount.myAccount.User;
                                old.Type = 1;
                                old.Amount = Conversion(balance, newA.Currency, other.Currency);
                                old.CurrentAccount = other.AccountNo;
                                old.SecondaryAccount = newNumber;
                                old.Date = System.DateTime.Now;
                                old.Currency = other.Currency;


                                new TransactionsServiceClient().ProcessTransaction(old);
                                new TransactionsServiceClient().ProcessTransaction(newA);


                                LogsReference.Log oldL = new LogsReference.Log();
                                oldL.Type = 1;
                                oldL.Amount = balance;
                                oldL.Account = new AccountsServiceClient().GetAccountByNo(other.AccountNo).Id;
                                oldL.Other = newNumber;
                                oldL.Date = System.DateTime.Now;
                                oldL.Currency = other.Currency;

                                new LogsServiceClient().AddLog(oldL);


                                status = "Account was successfully created.n/ your new account number is: " + newNumber;
                            }
                        }
                    }
                    else
                    {
                        status = "The token is invalid please try again";

                    }
                }
            }
            catch (HttpException e)
            {
                status = "System was unable to create your account, Please try again later";
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Transfers()
        {
            return View();
        }

        public JsonResult AccountTransfer()
        {
            //Continue here
            string status = "";

            try
            {
                string temp = System.DateTime.Now.ToLocalTime().ToString();

                string from = HttpContext.Request.QueryString["from"];
                string to = HttpContext.Request.QueryString["to"];
                string external = HttpContext.Request.QueryString["external"];
                string token = HttpContext.Request.QueryString["token"];

                decimal amount = 0;

                string username = HttpContext.User.Identity.Name;
                int userId = new UsersServiceClient().GetUserByUsername(username).Id;
                AccReference.Account accFrom = new AccountsServiceClient().GetAccountById(int.Parse(from));
                AccReference.Account accTo = new AccReference.Account();

                if (HttpContext.Request.QueryString["amount"] != "")
                {
                    amount = decimal.Parse(HttpContext.Request.QueryString["amount"]);
                }

                if ((accFrom.Balance - 10) <= amount)
                {
                    status = "There isn't enough balance in account ";
                }
                else
                {
                    if (new UsersReference.UsersServiceClient().IsAuthenticationValid(username, token) == true)
                    {
                        if (external.Length == 0)
                        {
                            accFrom.Balance -= amount;


                            accTo = new AccountsServiceClient().GetAccountById(int.Parse(to));
                            accTo.Balance += Conversion(amount, accFrom.Currency, accTo.Currency);

                            new AccountsServiceClient().UpdateAccount(accFrom);
                            new AccountsServiceClient().UpdateAccount(accTo);

                            TransReference.Transaction transFrom = new TransReference.Transaction();
                            transFrom.User = accFrom.User;
                            transFrom.Type = 1;
                            transFrom.Amount = Conversion(amount, accFrom.Currency, accTo.Currency);
                            transFrom.CurrentAccount = accFrom.AccountNo;
                            transFrom.SecondaryAccount = accTo.AccountNo;
                            transFrom.Date = System.DateTime.Now;
                            transFrom.Currency = accFrom.Currency;




                            TransReference.Transaction transTo = new TransReference.Transaction();
                            transTo.User = accFrom.User;
                            transTo.Type = 2;
                            transTo.Amount = amount;
                            transTo.CurrentAccount = accTo.AccountNo;
                            transTo.SecondaryAccount = accFrom.AccountNo;
                            transTo.Date = System.DateTime.Now;
                            transTo.Currency = accTo.Currency;

                            new TransactionsServiceClient().ProcessTransaction(transFrom);
                            new TransactionsServiceClient().ProcessTransaction(transTo);

                            LogsReference.Log transFromL = new LogsReference.Log();
                            transFromL.Type = 1;
                            transFromL.Amount = amount;
                            transFromL.Account = accFrom.Id;
                            transFromL.Other = accTo.AccountNo;
                            transFromL.Date = System.DateTime.Now;
                            transFromL.Currency = accFrom.Currency;

                            new LogsServiceClient().AddLog(transFromL);


                            status = "Transfer was successfully processed";


                        }
                        else if (to.Length == 0)
                        {
                            accFrom.Balance -= amount;


                            new AccountsServiceClient().UpdateAccount(accFrom);

                            TransReference.Transaction transFrom = new TransReference.Transaction();
                            transFrom.User = accFrom.User;
                            transFrom.Type = 1;
                            transFrom.Amount = amount;
                            transFrom.CurrentAccount = accFrom.AccountNo;
                            transFrom.SecondaryAccount = external;
                            transFrom.Date = System.DateTime.Now;
                            transFrom.Currency = accFrom.Currency;

                            new TransactionsServiceClient().ProcessTransaction(transFrom);

                            LogsReference.Log accFromL = new LogsReference.Log();
                            accFromL.Type = 1;
                            accFromL.Amount = amount;
                            accFromL.Account = accFrom.Id;
                            accFromL.Other = external;
                            accFromL.Date = System.DateTime.Now;
                            accFromL.Currency = accFrom.Currency;

                            new LogsServiceClient().AddLog(accFromL);

                            new UsersServiceClient().UpdateToken(username);

                            status = "Transfer was successfully processed";
                        }
                        else
                        {
                            status = "Please enter account to transfer to";
                        }

                    }
                    else
                    {
                        status = "Invalid token";
                    }
                }


            }
            catch (HttpException e)
            {
                status = "System was unable to process transfer. Please try again later";
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Convert()
        {
            string value = HttpContext.Request.QueryString["amount"];
            decimal amount = 0;
            decimal result = 0;

            try
            {

                if (!value.Contains("."))
                {
                    amount = decimal.Parse(value + ".00");
                }

                string from = HttpContext.Request.QueryString["from"];
                string to = HttpContext.Request.QueryString["to"];

                result = Conversion(amount, from, to);
            }
            catch (Exception e)
            { }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public decimal Conversion(decimal amount, string from, string to)
        {
            decimal temp = amount;
            double x = (double)temp;
            temp = (decimal)x;

            // decimal temp = decimal.Parse(amount.ToString                ());
            WebClient web = new WebClient();

            string url = string.Format("http://www.google.com/ig/calculator?hl=en&q={2}{0}%3D%3F{1}", from.ToUpper(), to.ToUpper(), temp);

            string response = web.DownloadString(url);

            Regex regex = new Regex("rhs: \\\"(\\d*.\\d*)");

            decimal rate = System.Convert.ToDecimal(regex.Match(response).Groups[1].Value);


            return rate;

        }

        public ActionResult CurrencyConverter()
        {
            return View(new CurrencyConverterModel());
        }

        public JsonResult Convertor()
        {
            List<System.Web.UI.WebControls.ListItem> currencyList = new List<System.Web.UI.WebControls.ListItem>();
            try
            {
                List<OnlineBankingSystem.CurrencyReference.Currency> currencies = new CurrencesServiceClient().GetCurrencies();


                foreach (OnlineBankingSystem.CurrencyReference.Currency c in currencies)
                {
                    currencyList.Add(new System.Web.UI.WebControls.ListItem(c.Currency1.ToString(), c.Code.ToString()));
                }
            }
            catch (Exception e)
            { }

            return Json(currencyList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SystemLogs()
        {
            UsersReference.User user = new UsersServiceClient().GetUserByUsername(HttpContext.User.Identity.Name);
            RolesReference.Role role = new RolesServiceClient().GetRoleById(user.Roles[0].Id);

            if (!role.Name.Equals("Admin"))
            {
                return RedirectToAction("Balances", "Accounts");
            }
            else
            {
                return View();
            }
        }

        public JsonResult SystemLog()
        {
            List<OnlineBankingSystem.LogsReference.Log> systemLogs = new LogsServiceClient().GetLogs();
            string logsStr = new LogsServiceClient().GetLogsString();
            List<LogsModel> logs = new List<LogsModel>();

            try
            {

                foreach (LogsReference.Log l in systemLogs)
                {

                    LogsModel model = new LogsModel();
                    model.Date = l.Date.ToShortDateString();
                    model.Account = new AccountsServiceClient().GetAccountById(l.Account).AccountNo;
                    model.Amount = l.Currency + l.Amount;
                    model.Client = l.Client.ToString();

                    if (l.Type == 1)
                    {
                        model.Details = "Transfer from Account " + l.Other;
                    }
                    else if (l.Type == 2)
                    {
                        model.Details = "Transfer to Account " + l.Other;
                    }

                    logs.Add(model);
                }
            }
            catch (Exception e)
            { }

            return Json(logs, JsonRequestBehavior.AllowGet);

        }

        public JsonResult FilterLogs()
        {
            List<LogsModel> logs = new List<LogsModel>();

            try
            {
                string start = HttpContext.Request.QueryString["start"];
                string end = HttpContext.Request.QueryString["end"];

                string client = HttpContext.Request.QueryString["clientId"];
                string account = HttpContext.Request.QueryString["accountId"];

                int accountId = 0;
                int clientId = 0;

                if (!client.Equals(""))
                {
                    clientId = int.Parse(client);
                }

                if (!account.Equals(""))
                {
                    accountId = int.Parse(account);
                }

                string temp = new LogsServiceClient().FilterTest(start, end, clientId, accountId);


                List<OnlineBankingSystem.LogsReference.Log> systemLogs = new LogsServiceClient().Filter(start, end, client, account);


                foreach (LogsReference.Log l in systemLogs)
                {

                    LogsModel model = new LogsModel();
                    model.Date = l.Date.ToShortDateString();
                    model.Account = new AccountsServiceClient().GetAccountById(l.Account).AccountNo;
                    model.Amount = l.Currency + l.Amount;
                    model.Client = l.Client.ToString();

                    if (l.Type == 1)
                    {
                        model.Details = "Transfer from Account " + l.Other;
                    }
                    else if (l.Type == 2)
                    {
                        model.Details = "Transfer to Account " + l.Other;
                    }

                    logs.Add(model);
                }
            }
            catch (Exception e)
            { }

            return Json(logs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AppointmentRequest()
        {
            return View();
        }

        public JsonResult AppointmentRequests()
        {
            string status = "";

            try
            {
                //  https://developers.google.com/google-apps/calendar/v1/developers_guide_dotnet#CreatingSingle



                string date = HttpContext.Request.QueryString["date"];
                string time = HttpContext.Request.QueryString["time"];
                string query = HttpContext.Request.QueryString["query"];

                UsersReference.User user = new UsersServiceClient().GetUserByUsername(HttpContext.User.Identity.Name);
                int userId = user.Id;
                System.DateTime appDate = System.DateTime.Parse(date);
                //System.DateTime endd = System.DateTime.Parse(time);


                /*
                GAuthSubRequestFactory authFactory = new GAuthSubRequestFactory("cl", "CalendarSampleApp");
                authFactory.Token = HttpContext.Session["sessionToken"].ToString(); 

                Service service = new Service("cl", authFactory.ApplicationName);
                service.RequestFactory = authFactory;

                 */



                OnlineBankingSystem.AppReference.Appointment appointment = new OnlineBankingSystem.AppReference.Appointment();
                //appointment.Date = new DateTime().;
                appointment.User = userId;
                appointment.Time = time;
                appointment.Date = DateTime.Parse(date);
                appointment.State = new AppStatusReference.AppointmentStatusServiceClient().GetAppointmentStatusId("Pending").Id;
                AppReference.Appointment appExists = new AppointmentsServiceClient().UserExistsCheck(appointment);


                if (((int)appointment.Date.DayOfWeek == 6) || (int)appointment.Date.DayOfWeek == 0)
                {
                    status = "Please choose appropriate weekday";
                }
                else if (new AppReference.AppointmentsServiceClient().AppointmentExists(appointment))
                {
                    status = "Appointment Slot already taken";
                }
                else
                {
                    if (new AppReference.AppointmentsServiceClient().AppointmentRequest(appointment))
                    {
                        MailMessage userM = new MailMessage();

                        userM.To.Add(new MailAddress(user.Email));
                        userM.From = new MailAddress("rayanne.baldacchino@gmail.com");

                        userM.Subject = "Appointment Request";
                        userM.Body = "Dear customer, We would like to let you know that you have just placed sent an appointment reuest for " + appointment.Date + " at " + appointment.Time + ". Thank you";

                        //to Admin
                        MailMessage adminM = new MailMessage();


                        adminM.To.Add(new MailAddress("rayanne.baldacchino@gmail.com"));
                        adminM.From = new MailAddress("rayanne.baldacchino@gmail.com");

                        adminM.Subject = "Appointment Request";
                        adminM.Body = "Client " + user.Username + " sent an apointment reuest for  " + appointment.Date + " at " + appointment.Time;



                        SmtpClient myClient = new SmtpClient();
                        myClient.Host = "smtp.gmail.com";
                        myClient.Port = 587;
                        myClient.EnableSsl = true;
                        myClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        myClient.UseDefaultCredentials = false;
                        myClient.Credentials = new NetworkCredential("rayanne.baldacchino@gmail.com", "1rayanne");


                        status = "Appointment Request successfully sent";
                    }
                    else
                    {
                        status = "Unable to create appointment";
                    }
                }
                /*
                try
                {

                    
            

                }
                catch (HttpException e)
                {
                    status = "event not added";
                }
                 */
            }
            catch (HttpException e)
            {
                status = "Unable to create appointment";
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AppointmentManagement()
        {
            UsersReference.User user = new UsersServiceClient().GetUserByUsername(HttpContext.User.Identity.Name);
            RolesReference.Role role = new RolesServiceClient().GetRoleById(user.Roles[0].Id);
            
            if (!role.Name.Equals("Admin"))
            {
                return RedirectToAction("AppointmentRequest", "Accounts");
            }
            else
            {
                return View();
            }
        }


        public JsonResult GetPendingList()
        {
            List<AppointmentModel> summary = new List<AppointmentModel>();

            try
            {
                List<OnlineBankingSystem.AppReference.Appointment> Appointments = new AppointmentsServiceClient().GetAppointments();



                foreach (OnlineBankingSystem.AppReference.Appointment a in Appointments)
                {
                    if (a.State == 3)
                    {
                        AppointmentModel model = new AppointmentModel();
                        model.Username = new UsersServiceClient().GetUserById(a.User).Username;
                        model.Time = a.Time;
                        model.Date = a.Date.ToShortDateString();
                        model.State = new AppStatusReference.AppointmentStatusServiceClient().GetAppointmentStatusById(a.State).State;
                        model.Id = a.Id;

                        summary.Add(model);
                    }
                }
            }
            catch (HttpException e)
            { }


            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetList()
        {
            List<AppointmentModel> summary = new List<AppointmentModel>();

            List<OnlineBankingSystem.AppReference.Appointment> Appointments = new AppointmentsServiceClient().GetAppointments();



            foreach (OnlineBankingSystem.AppReference.Appointment a in Appointments)
            {
                AppointmentModel model = new AppointmentModel();
                model.Username = new UsersServiceClient().GetUserById(a.User).Username;
                model.Time = a.Time;
                model.Date = a.Date.ToShortDateString();
                model.State = new AppStatusReference.AppointmentStatusServiceClient().GetAppointmentStatusById(a.State).State;
                model.Id = a.Id;
                summary.Add(model);
            }


            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AcceptAppointment()
        {
            string status = "";

            int id = int.Parse(HttpContext.Request.QueryString["id"]);

            try
            {
                AppReference.Appointment a = new AppointmentsServiceClient().GetAppointmentById(id);
                a.State = 2;
                if (new AppointmentsServiceClient().UpdateAppointment(a))
                {
                    status = "Appointment successfully accepted";
                }
                else
                {
                    status = "System was unable to reject appointment";
                }

                UsersReference.User user = new UsersServiceClient().GetUserById(a.User);

                //to User
                MailMessage userM = new MailMessage();


                userM.To.Add(new MailAddress(user.Email));
                userM.From = new MailAddress("rayanne.baldacchino@gmail.com");

                userM.Subject = "Appointment Request";
                userM.Body = "Your appointment request was accepted";



                SmtpClient myClient = new SmtpClient();
                myClient.Host = "smtp.gmail.com";
                myClient.Port = 587;
                myClient.EnableSsl = true;
                myClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                myClient.UseDefaultCredentials = false;
                myClient.Credentials = new NetworkCredential("rayanne.baldacchino@gmail.com", "1rayanne");

                myClient.Send(userM);

                ///Calendar
                //  GAuthSubRequestFactory authFactory = new GAuthSubRequestFactory("cl", "CalendarSampleApp");
                //  authFactory.Token = HttpContext.Session["sessionToken"].ToString();

                // Service service = new Service("cl", authFactory.ApplicationName);
                //  service.RequestFactory = authFactory;


                CalendarService myService = new CalendarService("exampleCo-exampleApp-1");
                myService.setUserCredentials("rayanne.baldacchino@gmail.com", "1rayanne");

                EventEntry entry = new EventEntry();

                // Set the title and content of the entry.
                entry.Title.Text = "Tennis with Beth";
                entry.Content.Content = "Meet for a quick lesson.";

                // Set a location for the event.
                Where eventLocation = new Where();
                eventLocation.ValueString = "South Tennis Courts";
                entry.Locations.Add(eventLocation);

                When eventTime = new When(DateTime.Now, DateTime.Now.AddMinutes(1));
                entry.Times.Add(eventTime);

                Uri postUri = new Uri("https://www.google.com/calendar/feeds/rayanne.baldacchino@gmail.com/private/full");

                // Send the request and receive the response:
                AtomEntry insertedEntry = myService.Insert(postUri, entry);
            }
            catch (HttpException e)
            {
                status = "System was unable to reject appointment";
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RejectAppointment()
        {
            string status = "";

            int id = int.Parse(HttpContext.Request.QueryString["id"]);

            try
            {
                AppReference.Appointment a = new AppointmentsServiceClient().GetAppointmentById(id);
                a.State = 1;
                if (new AppointmentsServiceClient().UpdateAppointment(a))
                {
                    status = "Appointment successfully rejected";
                }
                else
                {
                    status = "System was unable to reject appointment";
                }

                UsersReference.User user = new UsersServiceClient().GetUserById(a.User);

                //to User
                MailMessage userM = new MailMessage();


                userM.To.Add(new MailAddress(user.Email));
                userM.From = new MailAddress("rayanne.baldacchino@gmail.com");

                userM.Subject = "Appointment Request";
                userM.Body = "I'm sorry, your appointment request was rejected";



                SmtpClient myClient = new SmtpClient();
                myClient.Host = "smtp.gmail.com";
                myClient.Port = 587;
                myClient.EnableSsl = true;
                myClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                myClient.UseDefaultCredentials = false;
                myClient.Credentials = new NetworkCredential("rayanne.baldacchino@gmail.com", "1rayanne");

                myClient.Send(userM);


            }
            catch (HttpException e)
            {
                status = "System was unable to reject appointment";
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GeneratePDF(TransactionsModel c)
        {

            string accontNo = HttpContext.Request.QueryString["id"];
            string start = HttpContext.Request.QueryString["start"];
            string end = HttpContext.Request.QueryString["end"];

            System.DateTime startd = System.DateTime.Parse(start);
            System.DateTime endd = System.DateTime.Parse(end);

            UsersReference.User user = new UsersServiceClient().GetUserByUsername(HttpContext.User.Identity.Name);
            List<TransReference.Transaction> transactions = new TransactionsServiceClient().GetAccountTransactions(accontNo);
            AccReference.Account account = new AccountsServiceClient().GetAccountByNo(accontNo);
            // Create a Document object

            var output = new MemoryStream();


            try
            {
                /*
                
                
                PdfWriter.GetInstance(doc1, new FileStream(path + "/TransactionReport.pdf", FileMode.Create));
                doc1.SetPageSize(PageSize.A4);
                doc1.Open();
                 */

                var document = new iTextSharp.text.Document();
                string path = Server.MapPath("~/TransactionReports");
                FileStream fs = new FileStream(path + "/TransactionReport.pdf", FileMode.Create);
                PdfWriter.GetInstance(document, fs);

                document.SetPageSize(PageSize.A4);
                document.Open();
                /*string path = Server.MapPath("~/TransactionReports");
                FileStream fs = new FileStream("e:\\abcd.pdf", );
                PdfWriter.GetInstance(document, fs);
                document.Open();
                 */






                document.Add(new Paragraph("Transaction Report"));
                document.Add(Chunk.NEWLINE);

                var header = new PdfPTable(2);
                var logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Controllers/Images/BackgroundImage - Copy (2).jpeg"));
                header.AddCell(logo);
                header.AddCell("Date From: " + startd + " To: " + endd);
                document.Add(header);

                // Create the Account table - see http://www.mikesdotnetting.com/Article/86/iTextSharp-Introducing-Tables for more info
                var accountInfo = new PdfPTable(5);

                accountInfo.AddCell(new Phrase("Account No"));
                accountInfo.AddCell(new Phrase("Type of Account"));
                accountInfo.AddCell(new Phrase("Friendly Name"));
                accountInfo.AddCell(new Phrase("Available Balance"));
                accountInfo.AddCell(new Phrase("Currency"));
                accountInfo.AddCell(accontNo);
                accountInfo.AddCell(new AccountTypesServiceClient().GetAccountTypeById(account.Type).Name);
                accountInfo.AddCell(account.Name);
                accountInfo.AddCell(account.Balance.ToString());
                accountInfo.AddCell(new CurrencesServiceClient().GetCurrencyByCode(account.Currency).ToString());

                document.Add(accountInfo);
                document.Add(Chunk.NEWLINE);

                var transactionsInfo = new PdfPTable(4);

                transactionsInfo.AddCell(new Phrase("Date"));
                transactionsInfo.AddCell(new Phrase("Details"));
                transactionsInfo.AddCell(new Phrase("Currency"));
                transactionsInfo.AddCell(new Phrase("Amount"));

                foreach (TransReference.Transaction t in transactions)
                {
                    transactionsInfo.AddCell(t.Date.ToShortDateString());

                    string details = "";
                    string amount = "";

                    if (t.Type == 1)
                    {
                        details = "Transfer from Account " + t.SecondaryAccount;
                        amount = "+" + t.Amount;

                    }
                    else if (t.Type == 2)
                    {
                        details = "Transfer to Account " + t.SecondaryAccount;
                        amount = "+" + t.Amount;
                    }

                    transactionsInfo.AddCell(details);
                    transactionsInfo.AddCell(new CurrencesServiceClient().GetCurrencyByCode(t.Currency).Currency1);
                    transactionsInfo.AddCell(amount);
                }

                document.Add(transactionsInfo);





                Response.ContentType = "application/pdf";
                document.Close();
                fs.Close();

            }
            catch (Exception e)
            {

            }

            return Json(File("TransactionReport.pdf", "application/pdf", Server.HtmlEncode("TransactionReport.pdf")), JsonRequestBehavior.AllowGet);
        }
    }
}