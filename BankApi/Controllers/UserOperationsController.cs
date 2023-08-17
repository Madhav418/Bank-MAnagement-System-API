using BankApi.Models;
using BankApi.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace BankApi.Controllers
{
    public class UserOperationsController : ApiController
    {
        DBUSER d = new DBUSER();


        [HttpGet]
        [Route("api/UserOperations/GetDeposit")]
        public IHttpActionResult GetDeposit(int accountno, float amount)
        {
            string s = d.Deposit(accountno, amount);
            return Ok(s);
        }

        [HttpGet]
        [Route("api/UserOperations/GetWithdraw")]
        public IHttpActionResult GetWithdraw(int accountno, float amount)
        {
            string s = d.WithdrawOp(accountno, amount);
            return Ok(s);
        }
        [HttpGet]
        [Route("api/UserOperations/GetTransfer")]
        public IHttpActionResult GetTransfer(int saccountno, int accountno, string name, float amount)
        {
            string s = d.TransferAmount(saccountno, accountno, amount);
            return Ok(s);
        }
        [HttpGet]
        [Route("api/UserOperations/GetTransaction")]
        public IHttpActionResult GetTransaction(int saccountno)
        {
            // BankingEntities entities = new BankingEntities();
            List<Transaction> s = d.Transactions(saccountno);
            //var transactions = entities.Transactions.Where(t => t.AccountNo == saccountno).ToList();
            return Ok(s);
        }

        [HttpGet]
        [Route("api/UserOperations/GetCheckBalance")]
        public IHttpActionResult GetCheckBalance(int saccountno)
        {
            string Bal=d.checkBalance(saccountno);
            return Ok(Bal);
        }

        [HttpPost]
       
        public IHttpActionResult SubmitFeedBack([FromBody] Feedback F)
         {
            string s = d.Feedback(F);
            return Ok(s);
        }

        private bool IsValidEmail(string email)
        {
            // Use a regular expression to validate email format
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsValidMobileNumber(string mobileNumber)
        {
            // Use a regular expression to validate mobile number format
            string mobilePattern = @"^[6-9]{1}[0-9]{9}$";
            return Regex.IsMatch(mobileNumber, mobilePattern);
        }


    }

}

