using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BankApi.Models.DAO
{

    public class DBUSER
    {
        BankingEntities entity = new BankingEntities();
        //Deposit
        public string Deposit(int accountno, float amount)
        {
            if (amount <= 0)
            {
                return "Amount must be a positive value.";
            }
            Account A = entity.Accounts.FirstOrDefault(x => x.AccountNo == accountno);
            if (A != null )
            {
                try
                {

                    A.Balance = A.Balance + amount;

                    entity.SaveChanges();

                    Random random = new Random();
                    // Create and save the sender's transaction
                    Transaction senderTransaction = new Transaction();
                    senderTransaction.TransactionId = "T" + random.Next(1000, 9999).ToString();
                    senderTransaction.TransactionDate = DateTime.Now;
                    senderTransaction.TransactionType = "Deposit";
                    senderTransaction.Amount = amount;
                    senderTransaction.AccountNo = accountno;
                    entity.Transactions.Add(senderTransaction);

                    // Create and save the receiver's transaction


                    // Save the changes to the Transactions table
                    entity.SaveChanges();


                    return "Rs." + amount.ToString("0.00") + " Deposited Successfully." + checkBalance1(accountno);

                }

                catch (DbUpdateException ex)
                {
                    // Log or print the inner exception for debugging
                    return " please enter the amount ";
                    //throw; // Re-throw the exception or handle it accordingly
                }
            }
            else
            {
                return "Account number is incorrect";
            }
        }

        //Withdraw
        public string WithdrawOp(int accountno, float amount)
        {
            if (float.IsNaN(amount))
            {
                return "Enter the  Amount";
            }
            if (amount <= 0)
            {
                return "Amount must be a positive value.";
            }
            Account A = entity.Accounts.FirstOrDefault(x => x.AccountNo == accountno);
            if (A != null&& amount>0)
            {
                if (amount<A.Balance)
                {
                    A.Balance = A.Balance - amount;

                    entity.SaveChanges();
                    Random random = new Random();
                    // Create and save the sender's transaction
                    Transaction senderTransaction = new Transaction();
                    senderTransaction.TransactionId = "T" + random.Next(1000, 9999).ToString();
                    senderTransaction.TransactionDate = DateTime.Now;
                    senderTransaction.TransactionType = "Withdraw";
                    senderTransaction.Amount = amount;
                    senderTransaction.AccountNo = accountno;
                    entity.Transactions.Add(senderTransaction);

                    // Create and save the receiver's transaction


                    // Save the changes to the Transactions table
                    entity.SaveChanges();
                    return "Rs." + amount.ToString("0.00") + " Withdrawn Successfully." + checkBalance1(accountno); 
                }
                else
                {
                    return "Insufficient Funds";
                }
            }
            else
            {
                return "Account number is incorrect";
            }
        }

        //Transfer Amount from one account to another account
        public string TransferAmount(int saccountno, int accountno, float amount)
        {
            if (amount <= 0)
            {
                return "Amount must be a positive value.";
            }

            if (saccountno == null || accountno == null || float.IsNaN(amount))
            {
               return "Please check your details";
            }

            var A = entity.Accounts.FirstOrDefault(y => y.AccountNo == accountno);
            var S = entity.Accounts.FirstOrDefault(y => y.AccountNo == saccountno);

            if (S == null)
            {
                return "Source account number not found";
            }
            if (amount < S.Balance )
            {
                if (A != null)
                {
                    A.Balance = A.Balance + amount;
                    S.Balance = S.Balance - amount;

                    // Save the changes to the Accounts table first
                    entity.SaveChanges();
                    Random random = new Random();
                    // Create and save the sender's transaction
                    Transaction senderTransaction = new Transaction();
                    senderTransaction.TransactionId = "T" + random.Next(1000, 9999).ToString();
                    senderTransaction.TransactionDate = DateTime.Now;
                    senderTransaction.TransactionType = "Transfer";
                    senderTransaction.Amount = amount;
                    senderTransaction.AccountNo = saccountno;
                    entity.Transactions.Add(senderTransaction);

                    // Create and save the receiver's transaction
                 

                    // Save the changes to the Transactions table
                    entity.SaveChanges();

                    return "Rs." + amount.ToString("0.00") + " Transferred Successfully.\r\n" + checkBalance1(saccountno);
                }
                else
                {
                    return "Account number not found";
                }
            }
            else
            {
                return "Insufficient Balance";
            }
        }

        //For getting Transaction details
        public List<Transaction> Transactions(int saccountno)
        {
            var A = (from x in entity.Transactions
                     where x.AccountNo == saccountno
                     select x).ToList();

            List<Transaction> t = new List<Transaction>(); // Initialize the list before the loop

            for (int i = 0; i < A.Count; i++)
            {
                // Create a new Transaction object for each item in A
                Transaction transaction = new Transaction
                {
                    TransactionId = A[i].TransactionId,
                    Amount = A[i].Amount,
                    AccountNo = A[i].AccountNo,
                    TransactionType = A[i].TransactionType,
                    TransactionDate = A[i].TransactionDate
                };

                t.Add(transaction); // Add the new transaction object to the list
            }

            return t;
        }

        public string checkBalance(int accountno)
        {
            Account A = entity.Accounts.FirstOrDefault(x => x.AccountNo == accountno);
            string s = "Your Account Balance is:  ";
            return s + A.Balance;
            //return (float)A.Balance;
        }

        public string checkBalance1(int accountno)
        {
            Account A = entity.Accounts.FirstOrDefault(x => x.AccountNo == accountno);
            string s = "Your Current Balance is:  ";
            return s + A.Balance;
            //return (float)A.Balance;
        }

        public string Feedback(Feedback F)
        {
            if (F.Name == "" || F.Type == "" || F.EnquiryOn == "" || F.Email == "" || F.Contact == "" || F.Description == "")
            {
                return "Invalid input parameters. Please provide all required data.";
            }

            //Email validation using regular expression
            if (!IsValidEmail(F.Email))
            {
                return "Invalid email format.";
            }

            // Mobile number validation using regular expression
            if (!IsValidMobileNumber(F.Contact))
            {
                return "Invalid mobile number format.";
            }

            entity.Feedbacks.Add(F);
            entity.SaveChanges();
            return "Thanks for your feedback";
        }

        private bool IsValidMobileNumber( string contact)
        {
            // Regular expression pattern for a basic mobile number validation
            // This pattern allows for various formats like XXX-XXX-XXXX, XXX.XXX.XXXX, XXX XXX XXXX, etc.
            string pattern = @"^[6-9]{1}[0-9]{9}$";

            return Regex.IsMatch(contact, pattern);
        }

        private bool IsValidEmail(string email)
        {
            // Regular expression pattern for basic email validation
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, pattern);
        }

    }
}