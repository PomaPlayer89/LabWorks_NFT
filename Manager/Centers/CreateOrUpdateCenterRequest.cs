using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace nat.Manager.Centers
{
    public class CreateOrUpdateCenterRequest
    {
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
        private string _City;
        //свойство управляющее полем _City
        public string AdressCity
        {
            get 
            { 
                return _City;
            }
            set
            {
                //удаляем пробелы в начале и в конце строки
                value = value.Trim(' ');
                //заменяем все повторяющиеся пробелым одним
                _City = Regex.Replace(value, @"\s+", " ");
            }
        }
        private string _Street;
        //свойство управляющее полем _Street
        public string AdressStreet
        {
            get
            {
                return _Street;
            }
            set
            {
                //удаляем пробелы в начале и в конце строки
                value = value.Trim(' ');
                //заменяем все повторяющиеся пробелым одним
                _Street = Regex.Replace(value, @"\s+", " ");
            }
        }
        private string _NumberHouse;
        //свойство управляющее полем _NumberHouse
        public string AdressNumberHouse
        {
            get
            {
                //удаляем пробелы в начале и в конце строки
                return _NumberHouse;
            }
            set
            {
                //удаляем пробелы в начале и в конце строки
                value = value.Trim(' ');
                //заменяем все повторяющиеся пробелым одним
                _NumberHouse = Regex.Replace(value, @"\s+", " ");
            }
        }

        
    }
}
