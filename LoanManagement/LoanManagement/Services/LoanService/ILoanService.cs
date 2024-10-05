using LoanManagement.Models;

namespace LoanManagement.Services.LoanService
{
    internal interface ILoanService
    {
        void GetCustomerNamesAndIds();
        void ApplyLoan(Loan loan);
        void CalculateInterest(int id);
        void CalculateInterest(int principalAmount, decimal interestRate, int loanTerm);
        void LoanStatus(int id);
        void CalculateEMI(int id);
        void LoanRepayment(int loanId, decimal paymentAmount);
        void GetAllLoans();
        void GetLoanById(int loanId);
    }
}
