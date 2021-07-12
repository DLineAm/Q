using System;
using System.ComponentModel;

namespace Q.Services
{
    public class LengthErrorInfo : IDataErrorInfo
    {
        public string Error { get; }

        public string Login { get; set; }
        public string Password { get; set; }

        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;
                switch (columnName)
                {
                    case "Login":
                        if (Login.Length < 6 || Login.Length > 16)
                            error = "Логин должен иметь длинну не менее 6 и не более 16 символов";
                        break;
                }

                return error;
            }
        }
    }
}