using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace nat.Manager.Customers
{
    public class CreateOrUpdateCustomerRequest
    {
        private string _Insurancepolicy;
        //свойство управляющее полем _Insurancepolicy
        public string InsurancePolicy
        {
            get
            {
                return _Insurancepolicy;
            }
            set
            {
                //удаляем пробелы в начале и в конце строки
                value = value.Trim(' ');
                //заменяем все повторяющиеся пробелым одним
                _Insurancepolicy = Regex.Replace(value, @"\s+", " ");
            }
        }
        private string _SurName;
        //свойство управляющее полем _SurName
        public string SurName
        {
            get
            {
                return _SurName;
            }
            set
            {
                //удаляем пробелы в начале и в конце строки
                value = value.Trim(' ');
                //заменяем все повторяющиеся пробелым одним
                _SurName = Regex.Replace(value, @"\s+", " ");
            }
        }
        private string _Name;
        //свойство управляющее полем _Name
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                //удаляем пробелы в начале и в конце строки
                value = value.Trim(' ');
                //заменяем все повторяющиеся пробелым одним
                _Name = Regex.Replace(value, @"\s+", " ");
            }
        }
        private string _LastName;
        //свойство управляющее полем _LastName
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                //удаляем пробелы в начале и в конце строки
                _LastName = value.Trim(' ');
                //заменяем все повторяющиеся пробелым одним
                _LastName = Regex.Replace(_LastName, @"\s+", " ");
            }
        }
        public DateTime MyDay { get; private set; }
        //свойство управляющее полем MyDay
        public string Birthday
        {
            get
            {
                return MyDay.ToShortDateString();
            }
            set
            {
                MyDay = DateTime.Parse(value);
            }
        }
    }
}
