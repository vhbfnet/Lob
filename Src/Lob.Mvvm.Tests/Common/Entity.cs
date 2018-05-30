using Lob.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lob.Mvvm.Tests.Common
{
    public class Entity : EntityCore<Entity>
    {
        public int MyInt
        {
            get { return _myInt; }
            set { SetProperty(ref _myInt, value); }
        }
        private int _myInt;

        public string MyString
        {
            get { return _myString; }
            set { SetProperty(ref _myString, value); }
        }
        private string _myString;

        [Required]
        public string MyRequiredString
        {
            get { return _myRequiredString; }
            set { SetProperty(ref _myRequiredString, value); }
        }
        private string _myRequiredString;

        public DateTime MyDate
        {
            get { return _myDate; }
            set { SetProperty(ref _myDate, value); }
        }
        private DateTime _myDate;
    }
}