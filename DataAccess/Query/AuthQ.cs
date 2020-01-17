using DataAccess.Mapper;
using DataTransferObject.Auth;
using DataTransferObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DataAccess.Query
{
    public class AuthQ
    {
        private IdeaManagmentDatabaseEntities _db;
        //-------------------------------------------------------------------------------------------------

        public Result Registration(UserForRegistrationDto user)
        {
            Result res = new Result();
            var userFromDb = new UserQ().GetUser(user.Username.Trim());
            if (userFromDb != null)
            {
                res.Value = false;
                res.Content = "نام کاربری موجود است";
            }else using (_db = new IdeaManagmentDatabaseEntities())
            {
                    var newUser = new USER()
                    {
                        EMAIL =user.Email.Trim(),
                        FIRST_NAME =user.FirstName.Trim(),
                        LAST_NAME =user.LastName.Trim(),
                        USERNAME =user.Username.Trim(),
                        PASSWORD =user.password.Trim(),
                        SAVE_DATE = DateTime.Now
                        };
                    _db.USERS.Add(newUser);
                    _db.SaveChanges();
                    res.Value = true;
                    res.Content = "کاربر ایجاد شد";
                }

            return res;
        }
        //-------------------------------------------------------------------------------------------------

        public Result Login(UserForLoginDto user)
        {
            Result res = new Result();
            var userFromDb = new UserQ().GetUser(user.Username.Trim());
            if (userFromDb == null ||!userFromDb.Password.Equals(user.Password.Trim()))
            {
                res.Value = false;
                res.Content = "مشکل در احراز هوییت";
            }
            else
            {
                res.Value = true;
                res.Content = "ورود موفقیت آمیز بود";
            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------

        public Result ForgetPassword(ForgetPasswordDto user)
        {
            Result res = new Result();
        
                using (_db = new IdeaManagmentDatabaseEntities())
                {
                var findUser = _db.USERS.FirstOrDefault(x =>
                      x.EMAIL == user.Email.Trim() &&
                      x.FIRST_NAME == user.FirstName.Trim() &&
                      x.LAST_NAME == user.LastName.Trim() &&
                     x.USERNAME == user.Username.Trim());
                if (findUser != null)
                {
                    res.Value = true;
                    res.Content = ""+findUser.PASSWORD;
                }
                else
                {
                    res.Value = false;
                    res.Content = "اطلاعات مطابقت ندارد";
                }
             }

            return res;
        }
        //-------------------------------------------------------------------------------------------------


    }
}