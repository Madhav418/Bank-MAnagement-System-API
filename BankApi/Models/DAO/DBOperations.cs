using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankApi.Models.DAO
{
    public class DBOperations
    {
        BankingEntities entity=new BankingEntities();

        public string RegisterUser(User user)
        {
            var exist = entity.Users.FirstOrDefault(a => a.UserId == user.UserId);
            if (exist == null)
            {
                try
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    entity.Users.Add(user);
                    entity.SaveChanges();
                    return "Successfully Registered";
                }
                catch (DbUpdateException e)
                {
                    SqlException sql = e.GetBaseException() as SqlException;
                    if (sql.Message.Contains("pk_eno"))
                    {
                        return "cannot have duplicate values";
                    }
                    else if (sql.Message.Contains("fk_emp"))
                    {
                        return "the no such record";
                    }
                    else
                    {
                        return e.Message;
                    }
                }
            }
            else
            {
                return "UserId already exists,try another one";
            }
        }
        public bool validateUser(string username,string password)
        {
            bool flag = false;
            var user = (from e in entity.Users
                       where e.UserId == username && e.Status == "approve"
                       select e).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            else
            {
                return BCrypt.Net.BCrypt.Verify(password, user.Password);
            }
            return flag;
        }

        public string getAccount(string username)
        {

            var user = (from x in entity.Users
                       where x.UserId == username
                       select x.AccountNo).FirstOrDefault();
            return user.ToString();
           
        }




        public bool validateAdmin(string EmployeeId, string Password)
        {
            bool flag = false;
            var user = (from x in entity.Admins
                       where x.EmployeeId == EmployeeId && x.Password == Password
                       select x).FirstOrDefault();


            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
            return flag;
        }
        public List<User> getUsers()
        {
            entity.Configuration.ProxyCreationEnabled = false;
            var data= (from x in entity.Users
                      where x.Status == null
                      select x).ToList();
           


            return data;
        }


        public void UpdateUserStatus(string UserId, string status)
        {
             var userToUpdate = (from e in entity.Users
                                where e.UserId == UserId 
                                select e).FirstOrDefault();

            if (userToUpdate != null)
                {
                //Use a combination of random numbers and the current timestamp to create a unique account number
                Random random = new Random();
                // string accountNumberString = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString();

                int AccountNumber = random.Next(100000000, 999999999);
                // Parse the generated string as an integer
                //int accountNumber = int.Parse(accountNumberString);
                userToUpdate.Status = status;
          
                        entity.SaveChanges();
                var user = (from x in entity.Users
                            where x.Status == "approve"
                            select x).FirstOrDefault();
                var user1 = (from x in entity.Users
                             where x.Status == "reject"
                             select x).FirstOrDefault();
                if (user != null)
                {
                    userToUpdate.AccountNo = AccountNumber;
                   
                    entity.SaveChanges();


                }
                if(user1!=null)
                {
                    userToUpdate.AccountNo = null;
                    entity.SaveChanges();

                }

                User U = (from e in entity.Users
                          where e.UserId == UserId && e.Status == "approve"
                          select e).FirstOrDefault();
                if (U != null)
                {
                    Account A = new Account();
                    A.AccountNo = AccountNumber;
                    A.AccountHolder_Name = U.FirstName + U.LastName;
                    A.Balance = 0;
                    entity.Accounts.Add(A);

                    entity.SaveChanges();
                }

            }
            else
             {
    
             }
        }

        public List<Feedback> getFeddback()
        {
            entity.Configuration.ProxyCreationEnabled = false;
            var data = (from x in entity.Feedbacks
                       
                        select x).ToList();



            return data;
        }
      
        public string GetName(int accountno)
        {
            var account = entity.Accounts.FirstOrDefault(e => e.AccountNo == accountno);

            if (account == null)
            {
               return "Account number is incorrect.";
            }

            return account.AccountHolder_Name;
        }
        public string getname(int accountno) 
        {
            try
            {
                string accountHolderName = GetName(accountno);
                return accountHolderName;
                // Use the account holder name
            }
            catch (ArgumentException ex)
            {
                // Handle the incorrect account number exception
               return ex.Message; // or display a user-friendly message
            }
        }

    }

}