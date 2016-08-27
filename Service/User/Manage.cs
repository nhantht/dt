using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.User
{
    public class Manage
    {
        dtEntities1 db = new dtEntities1();
        public object GetColumns(string tableName)
        {
            return from t in typeof(Data.User).GetFields() select t.Name;
        }
        public bool ValidatePassword(string pass)
        {
            bool result = true;
            result = pass.Any(char.IsUpper);
            if (result)
            {
                result = pass.Any(char.IsDigit);
            }

            return result;
        }
        public Data.User GetUserInfo(string phone)
        {
            Data.User user = db.Users.Where(s => s.Phone == phone).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            else
            {
                return new Data.User();
            }
        }
        public Data.User GetUserInfoBySession(string session)
        {
            return db.Users.Where(s => s.Session == session).FirstOrDefault();
        }
        public string ValidateAccount(string phone, string password, string session)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            Service.User.Manage manage = new Service.User.Manage();
            Data.User user = new Service.User.Login().GetUserByPhone(phone);
            if (user != null)
            {
                //if (phone != Lib.Common.Variables.AdminUser && user.Logined)
                //{
                //    return "InuseAccount";
                //}
                if (user.IsActive == false)
                {
                    if (user.InActiveFrom != null)
                    {
                        if (user.InActiveFrom <= DateTime.Now
                            && user.InActiveTo >= DateTime.Now
                        )
                        {
                            return "LockedAccount";
                        }
                        else
                        {
                            user.InActiveFrom = user.InActiveTo = null;
                            user.IsActive = true;
                            if (manage.UpdateUserInfo(user))
                            {
                                manage.CreateTransactionHistory(7, phone);
                            }
                        }
                    }
                    else
                    {
                        return "ActiveAccount";
                    }
                }
                switch (user.StatusId)
                {
                    case 1:
                        {
                            return "ActiveAccount";
                        }
                    case 3:
                        {
                            return "RenewPassword";
                        }
                    default:
                        {
                            if (user.IsActive)
                            {
                                if (user.Password == crypto.Compute(password, user.PasswordSalt))
                                {
                                    if (session != null)
                                    {
                                        user.Session = session;
                                        //Update the user
                                        manage.UpdateUserInfo(user, true);
                                    }

                                    return string.Empty;
                                }
                                else
                                {
                                    return "InvalidAccount";
                                }
                            }

                            break;
                        }
                }
            }
            else
            {
                return "InvalidAccount";
            }

            return string.Empty;
        }
        public bool CreateTransactionHistory(decimal taskId, string userId)
        {
            try
            {
                Data.TransactionHistory history = new TransactionHistory();
                Data.TransactionHistory lastHistory = db.TransactionHistories.OrderByDescending(s => s.HistoryId).FirstOrDefault();
                decimal newId = 0;
                if (lastHistory != null)
                {
                    newId = lastHistory.HistoryId;
                }
                newId++;
                history.HistoryId = newId;
                history.Time = DateTime.Now;
                history.TaskId = taskId;
                history.UserId = userId;

                db.TransactionHistories.Add(history);
                db.SaveChanges();

                return true;
            }
            catch { return false; }
        }
        public bool Delete(string id)
        {
            try
            {
                db.Users.Remove(db.Users.Where(s => s.Phone == id).First());
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<Data.User> GetAllUsers()
        {
            return from u in db.Users
                   orderby u.CreatedDate descending
                   select u;
        }
        public IEnumerable<Data.Status> GetAllStatuses()
        {
            return from u in db.Statuses
                   orderby u.StatusId
                   select u;
        }
        public bool UpdateUserInfo(Data.User inputUser, bool forLogin = false)
        {
            try
            {
                Data.User user = db.Users.Where(s => s.Phone == inputUser.Phone).FirstOrDefault();
                user.Email = inputUser.Email;
                user.IsActive = inputUser.IsActive;
                user.InActiveTo = inputUser.InActiveTo;
                user.InActiveFrom = inputUser.InActiveFrom;
                if (forLogin != null)
                {
                    user.Session = inputUser.Session;
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }
        public Data.User GetUserByEmail(string email)
        {
            return db.Users.Where(s => s.Email == email).FirstOrDefault();
        }
        public string[] GetPhoneNumber(string countryCode, string phoneNumber)
        {
            Service.Region.CountryObj countryObj = new Service.Region.CountryObj();
            Data.Country country = countryObj.GetAllCountries(0, countryCode).FirstOrDefault();
            string telephoneCode = string.Format("+{0}", country.TelephoneCode);

            //Trim phone number
            while (phoneNumber.StartsWith("+"))
            {
                phoneNumber = phoneNumber.Substring(1);
            }
            while (phoneNumber.StartsWith("0"))
            {
                phoneNumber = phoneNumber.Substring(1);
            }
            phoneNumber = string.Format("{0}{1}", telephoneCode, phoneNumber);
            //Clean
            countryObj = null;

            return new string[] { country.CountryId.ToString(), phoneNumber };
        }
        public Data.User GetUserByRegetpasswordCode(string regetPasswordCode)
        {
            return (from s in db.Users
                    where s.RegetPasswordCode == regetPasswordCode
                             & s.StatusId == 3
                    select s).FirstOrDefault();
        }
        public bool ChangePassword(string phone, string newPassword, string salt)
        {
            try
            {
                Data.User user = db.Users.Where(s => s.Phone == phone).FirstOrDefault();
                if (user != null)
                {
                    user.Password = newPassword;
                    user.PasswordSalt = salt;
                    user.StatusId = 4;
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string[] GetDisplayedCountryListData()
        {
            string patern = "sq_AL,hy_AM,eu_ES,be_BY,bg_BG,ca_ES,hr_HR,cs_CZ,da_DK,et_EE,fo_FO,fi_FI,gl_ES,ka_GE,el_GR,hu_HU,is_IS"
                 + ",hi_IN,id_ID,fa_IR,he_IL,it_IT,ja_JP,kk_KZ,sw_KE,ko_KR,lv_LV,lt_LT,de_LU,ms_MY,fr_MC,mn_MN,nn_NO,ur_PK,pl_PL"
                 + ",pt_PT,ro_RO,ru_RU,sk_SK,sl_SI,af_ZA,sv_SE,zh_TW,th_TH,nl_NL,tr_TR,uk_UA,es_VE,vi_VN,ar_YE,en_ZW,mk_MK";

            Service.Region.CountryObj country = new Service.Region.CountryObj();

            List<string> list = (from s in country.GetAvailableLanguages(patern)
                                 orderby s.Name
                                 select s.TelephoneCode
                                 ).ToList();

            string codes = string.Join(",", list);

            list = (from s in country.GetAvailableLanguages(patern)
                    orderby s.Name
                    select s.Name
                                 ).ToList();

            string countryNames = string.Join(",", list);

            return new string[] { codes, countryNames };
        }
    }
}
