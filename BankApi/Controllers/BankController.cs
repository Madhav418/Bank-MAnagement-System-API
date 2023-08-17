using BankApi.Models;
using BankApi.Models.DAO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using System.Web.UI.WebControls;


namespace BankApi.Controllers

{
   
    public class BankController : ApiController
    {
        DBOperations db = new DBOperations();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        //[Authorize(Roles = "User")]
        public IHttpActionResult getName(int accountno)
        {
            
           
                return Ok(db.getname(accountno));
            

        }
       
        [HttpPost]
        public IHttpActionResult Register([FromBody] User e)
        {
            string s = db.RegisterUser(e);
            return Ok(s);

        }

        [HttpGet]
        public IHttpActionResult CheckLogin(string username,string password)
        {
            bool s = db.validateUser(username,password);
            //string st = "data found";
            string AccNo=db.getAccount(username);
           // string ss = "Invalid Credentials";
            if (s)
            {
                return Ok(AccNo);
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }



        }
        


       



    }
}
