using LoanManagement.Models;
using LoanManagement.Repository.LoanRepository;

namespace LoanManagement.Services.LoanService
{
    internal class LoanService:ILoanService
    {
        readonly ILoanRepository _loanRepository;
        public LoanService() 
        { 
            _loanRepository = new LoanRepository();
        }

        public void GetCustomerNamesAndIds()
        {
            _loanRepository.GetCustomerNamesAndIds();
        }

        public void ApplyLoan(Loan loan)
        {
            int status = _loanRepository.ApplyLoan(loan);
            if (status > 0)
            {
                Console.WriteLine("Loan has been applied successfully!");
            }
            else
            {
                Console.WriteLine("Loan application failed!");
            }
        }

        public void CalculateInterest(int id)
        {
            decimal amount = _loanRepository.CalculateInterest(id);
            Console.WriteLine($"The interest amount is {amount}");
        }

        public void CalculateInterest(int principalAmount, decimal interestRate, int loanTerm)
        {
            decimal amount = (principalAmount * interestRate * loanTerm) / 12;
            Console.WriteLine($"The interest amount is {amount}");
        }

        public void LoanStatus(int id)
        {
            string loanStatus = _loanRepository.LoanStatus(id);
            Console.WriteLine($"Your loan is {loanStatus}");
        }

        public void CalculateEMI(int id)
        {
            Console.WriteLine($"The EMI amount is {_loanRepository.CalculateEMI(id)}");
        }

        public void LoanRepayment(int loanId, decimal paymentAmount)
        {
            _loanRepository.LoanRepayment(loanId, paymentAmount);
        }

        public void GetAllLoans()
        {
            List<Loan> loans = new List<Loan>();
            loans = _loanRepository.GetAllLoans();
            foreach (Loan loan in loans)
            {
                Console.WriteLine(loan);
            }
        }

        public void GetLoanById(int loanId)
        {
            Console.WriteLine(_loanRepository.GetLoanById(loanId));
        }

    }
}
