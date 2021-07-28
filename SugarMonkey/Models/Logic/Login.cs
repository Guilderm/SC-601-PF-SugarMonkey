using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SugarMonkey.Models.View;

namespace SugarMonkey.Models.Logic
{
    public class Login
    {
        //function to check if User is valid or not
        public User IsValidUser(Login model)
        {
            using (GeneralPurposeDBEntities dataContext = new GeneralPurposeDBEntities())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                User user = dataContext.Users
                    .Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password))
                    .SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                return user;
            }
        }
    }
}