using BankApi.Models;
using BankApi.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankApi.Controllers
{
    public class BankAdminController : ApiController
    {
        DBOperations db=new DBOperations();
        [HttpGet]
        [Route("api/BankAdmin/UserData")]
        public IHttpActionResult UserData()

        {

            return Ok(db.getUsers());

        }
        [HttpGet]
        public IHttpActionResult CheckLogin(string EmployeeId, string Password)
        {
            bool s = db.validateAdmin(EmployeeId, Password);
            string st = "data found";
            string ss = "Invalid Credentials";
            if (s)
            {
                return Ok(st);
            }
            else
            {
                return BadRequest(ss);
            }

        }
        [HttpPost]
        public IHttpActionResult UpdateStatus(List<User> updatedData)
        {
            //string s = "data is invalid";
            try
            {
                foreach (var user in updatedData)
                {
                   db.UpdateUserStatus(user.UserId, user.Status);    
                }
                return Ok();
                //return Json(new { success = true, message = "Roles updated successfully!" });
            }
            catch (Exception ex)
            {
               return Ok(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/BankAdmin/feedback")]
        public IHttpActionResult feedback()
        {

            return Ok(db.getFeddback());

        }

    }
}
