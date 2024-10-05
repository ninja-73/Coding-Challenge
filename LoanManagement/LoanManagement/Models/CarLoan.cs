namespace LoanManagement.Models
{
    internal class CarLoan:Loan
    {
        private string carModel;   
        private int carValue;

        public CarLoan() { }

        public CarLoan(int loanId, int customer, int principalAmount, decimal interestRate, int loanTerm, string loanStatus, string carModel, int carValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, "CarLoan", loanStatus)  // Call to base constructor
        {
            this.carModel = carModel;
            this.carValue = carValue;
        }

        public string CarModel { get { return carModel; } set { carModel = value; } }
        public int CarValue { get { return carValue; } set { carValue = value; } }

        public override string ToString()
        {
            return base.ToString() + $",\nCarModel: {CarModel},\nCarValue: {CarValue}";
        }
    }
}
