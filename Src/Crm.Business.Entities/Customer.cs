using Crm.Business.Entities.Properties;
using Lob.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Business.Entities
{
    public class Customer : EntityCore<Customer>
    {
        [DataMember]
        [Key]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        private int _id;

        [DataMember]
        [Required]
        [MinLength(5)]
        [Display(ResourceType = typeof(Resources), Name = "Customer_FirstName")]
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }
        private string _firstName;

        [DataMember]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Customer_LastName")]
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }
        private string _lastName;

        [DataMember]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Customer_Email")]
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        private string _email;
    }
}
