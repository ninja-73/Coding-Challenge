using LoanManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Repository.LoanRepository
{
    internal interface ILoanRepository
    {
        void GetCustomerNamesAndIds();
        int ApplyLoan(Loan loan);
        decimal CalculateInterest(int id);
        string LoanStatus(int id);
        decimal CalculateEMI(int id);
        void LoanRepayment(int loanId, decimal paymentAmount);
        List<Loan> GetAllLoans();
        Loan GetLoanById(int loanId);
    }
}
