using Lob.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lob.Mvvm.Tests.Common
{
    public class EntityEditableViewModel : EditableViewModel<Entity>
    {
        public bool MyProperty
        {
            get { return _myProperty; }
            set { SetProperty(ref _myProperty, value); }
        }
        private bool _myProperty;

        [Required]
        public string MyRequiredProperty
        {
            get { return _myRequiredProperty; }
            set { SetProperty(ref _myRequiredProperty, value); }
        }
        private string _myRequiredProperty;

        [IgnoreChange]
        public bool MyPropertyIgnored
        {
            get { return _myPropertyIgnored; }
            set { SetProperty(ref _myPropertyIgnored, value); }
        }
        private bool _myPropertyIgnored;
    }
}
