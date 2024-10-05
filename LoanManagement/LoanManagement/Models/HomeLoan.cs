namespace LoanManagement.Models
{
    internal class HomeLoan:Loan
    {
        private string propertyAddress;  
        private int propertyValue;       

        public HomeLoan() { }

        public HomeLoan(int loanId, int customer, int principalAmount, decimal interestRate, int loanTerm, string loanStatus, string propertyAddress, int propertyValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, "HomeLoan", loanStatus) 
        {
            this.propertyAddress = propertyAddress;
            this.propertyValue = propertyValue;
        }

        public string PropertyAddress { get { return propertyAddress; } set { propertyAddress = value; } }
        public int PropertyValue { get { return propertyValue; } set { propertyValue = value; } }

        public override string ToString()
        {
            return base.ToString() + $",\nPropertyAddress: {PropertyAddress},\nPropertyValue: {PropertyValue}";
        }
    }
}
