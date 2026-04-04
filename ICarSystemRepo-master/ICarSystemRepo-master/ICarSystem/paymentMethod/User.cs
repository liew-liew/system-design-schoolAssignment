using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem.paymentMethod
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }

        // Constructor
        public User(int id, string fullName, string email, string phoneNum)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            PhoneNum = phoneNum;
        }
    }

}