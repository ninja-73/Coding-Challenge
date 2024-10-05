using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Models
{
    internal class Customer
    {
        private int customerID;
        private string name;
        private string email;
        private string phone;
        private string address;
        private int creditScore;

        public Customer() { }

        public Customer(int id, string name, string email, string phone, string address, int creditScore)
        {
            this.customerID = id;
            this.name = name;
            this.email = email;
            this.phone = phone;
            this.address = address;
            this.creditScore = creditScore;
        }

        public int CustomerID { get { return customerID; } set { customerID = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Phone { get { return phone; } set { phone = value; } }
        public string Address { get { return address; } set { address = value; } }
        public int CreditScore { get { return creditScore; } set { creditScore = value; } }

        public override string ToString()
        {
            return $"\nCustomerID: {CustomerID},\nName: {Name},\nEmail: {Email},\nPhoneNumber: {Phone},\nAddress: {Address},\nCreditScore: {CreditScore}";
        }
    }
}
