using LoanManagement.Models;
using LoanManagement.Repository.LoanRepository;
using LoanManagement.Services.LoanService;
using static LoanManagement.Exceptions.CustomExceptions;

namespace LoanManagement.LoanMenu
{
    internal class MainMenu
    {
        readonly ILoanService _loanService;
        public MainMenu() 
        {
            _loanService = new LoanService();
        }

        public void run()
        {
            Console.WriteLine("\t\t\t\t\tLoan Management System");
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("1. Apply for a Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan by ID");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. Exit");
                Console.Write("Please enter your choice (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ApplyForLoan();
                        break;
                    case "2":
                        GetAllLoans();
                        break;
                    case "3":
                        GetLoanById();
                        break;
                    case "4":
                        LoanRepayment();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Exiting the system. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                        break;
                }
            }
        }

        private void ApplyForLoan()
        {
            _loanService.GetCustomerNamesAndIds();
            Console.Write("\nEnter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            Console.Write("Enter Principal Amount: ");
            int principalAmount = int.Parse(Console.ReadLine());

            Console.Write("Enter Interest Rate: ");
            decimal interestRate = decimal.Parse(Console.ReadLine());

            Console.Write("Enter Loan Term (in months): ");
            int loanTerm = int.Parse(Console.ReadLine());

            Console.Write("Enter Loan Type (HomeLoan/CarLoan): ");
            string loanType = Console.ReadLine();

            Loan newLoan = new Loan
            {
                CustomerID = customerId,
                PrincipalAmount = principalAmount,
                InterestRate = interestRate,
                LoanTerm = loanTerm,
                LoanType = loanType,
                LoanStatus = "Pending"
            };

            _loanService.ApplyLoan(newLoan);
        }

        private void GetAllLoans()
        {
            _loanService.GetAllLoans();
        }

        private void GetLoanById()
        {
            Console.Write("Enter Loan ID: ");
            int loanId = int.Parse(Console.ReadLine());
            try
            {
                _loanService.GetLoanById(loanId);
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void LoanRepayment()
        {
            Console.Write("Enter Loan ID: ");
            int loanId = int.Parse(Console.ReadLine());
            try
            {
                _loanService.CalculateEMI(loanId);
                Console.Write("Enter Payment Amount: ");
                decimal paymentAmount = decimal.Parse(Console.ReadLine());

                _loanService.LoanRepayment(loanId, paymentAmount);
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine(ex.Message);

            }
 
        }
    }
}
