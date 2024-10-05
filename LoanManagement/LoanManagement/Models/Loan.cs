using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Models
{
    internal class Loan
    {
        private int loanID;
        private int customerID;
        private int principalAmount;
        private decimal interestRate;
        private int loanTerm;
        private string loanType;
        private string loanStatus;

        public Loan() { }

        public Loan(int loanId, int customerId, int principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus)
        {
            this.loanID = loanId;
            this.customerID = customerId;
            this.principalAmount = principalAmount;
            this.interestRate = interestRate;
            this.loanTerm = loanTerm;
            this.loanType = loanType;
            this.loanStatus = loanStatus;
        }

        public int LoanID { get { return loanID; } set { loanID = value; } }
        public int CustomerID { get { return customerID; } set { customerID = value; } }
        public int PrincipalAmount { get { return principalAmount; } set { principalAmount = value; } }
        public decimal InterestRate { get { return interestRate; } set { interestRate = value; } }
        public int LoanTerm { get { return loanTerm; } set { loanTerm = value; } }
        public string LoanType { get { return loanType; } set { loanType = value; } }
        public string LoanStatus { get { return loanStatus; } set { loanStatus = value; } }

        public override string ToString()
        {
            return $"\nLoanID: {LoanID},\nCustomerID: {CustomerID},\nPrincipalAmount: {PrincipalAmount},\nInterestRate: {InterestRate},\nLoanTerm: {LoanTerm} months,\nLoanType: {LoanType},\nLoanStatus: {LoanStatus}";
        }
    }
}
