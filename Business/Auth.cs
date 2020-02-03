using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObject.Auth;
using DataTransferObject.Common;

namespace Business
{
    public class Auth
    {
        private readonly DataAccess.Query.AuthQ _repository;
        //-------------------------------------------------------------------------------------------------
        public Auth()
        {
            _repository = new DataAccess.Query.AuthQ();
        }
        //-------------------------------------------------------------------------------------------------
        public Result Login(UserForLoginDto user)
        {
            Result res;
            if (user == null)
            {
                res= new Result() { Content = "خطایی رخ داده است", Value = false };
            }
           else if (user.Username == null)
            {
                res = new Result() { Content ="لطفا نام کاربری خود را وارد کنید", Value =false};
            }
            else if (user.Password == null)
            {
                res = new Result() { Content = "لطفا کلمه‌ی عبور خود را وارد کنید", Value = false };
            }
            else 
            {
                res = _repository.Login(user);
            }
            return res;  
        }
        //-------------------------------------------------------------------------------------------------

        public Result Registration(UserForRegistrationDto user)
        {
            Result res;
            if (user == null)
            {
                res = new Result() { Content = "خطایی رخ داده است", Value = false };
            }
            else if (
                user.FirstName==null ||
                user.Email==null ||
                user.LastName==null ||
                user.Username==null ||
                user.password==null
                )
            {
                res = new Result() { Content = "یکی از فیلد های اجباری پرنشده است", Value = false };
            }
            else
            {
                res = _repository.Registration(user);
            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------

        public Result ForgetPassword(ForgetPasswordDto user)
        {
            return _repository.ForgetPassword(user);
        }
        //-------------------------------------------------------------------------------------------------

    }
}
