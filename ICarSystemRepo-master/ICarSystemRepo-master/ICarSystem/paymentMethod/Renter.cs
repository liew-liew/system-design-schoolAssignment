using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICarSystem.paymentMethod;

namespace ICarSystem
{

    public class Renter : User
    {
        public int RenterId { get; set; }
        public string DriverLicenseInfo { get; set; }
        public DateTime Dob { get; set; }
        public decimal PenaltyAmount { get; set; }

        // Constructor
        public Renter(int id, string fullName, string email, string phoneNum, int renterId, string driverLicenseInfo, DateTime dob, decimal penaltyAmount)
            : base(id, fullName, email, phoneNum)
        {
            RenterId = renterId;
            DriverLicenseInfo = driverLicenseInfo;
            Dob = dob;
            PenaltyAmount = penaltyAmount;
        }
    }


}
