using LoanManagement.Models;
using LoanManagement.Utility;
using System.Data.SqlClient;
using static LoanManagement.Exceptions.CustomExceptions;

namespace LoanManagement.Repository.LoanRepository
{
    internal class LoanRepository : ILoanRepository
    {
        SqlConnection sqlConnection = null;
        SqlCommand sqlCommand = null;

        public LoanRepository()
        {
            sqlConnection = new SqlConnection(DbConnUtil.GetConnString());
            sqlCommand = new SqlCommand();
        }

        public void GetCustomerNamesAndIds()
        {
            try
            {
                sqlCommand.CommandText = "SELECT CustomerID, Name FROM Customer";
                sqlCommand.Connection = sqlConnection;

                sqlConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    Console.WriteLine("\nCustomer ID\tCustomer Name");
                    Console.WriteLine("--------------------------------");

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int customerId = (int)reader["CustomerID"];
                            string customerName = reader["Name"].ToString();

                            Console.WriteLine($"{customerId}\t\t{customerName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No customers found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public int ApplyLoan(Loan loan)
        {
            sqlCommand.CommandText = "insert into Loan (CustomerID,PrincipalAmount,InterestRate,LoanTerm,LoanType,LoanStatus) values (@cusId,@amt,@rate,@term,@type,@status)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@cusid", loan.CustomerID);
            sqlCommand.Parameters.AddWithValue("@amt", loan.PrincipalAmount);
            sqlCommand.Parameters.AddWithValue("@rate", loan.InterestRate);
            sqlCommand.Parameters.AddWithValue("@term", loan.LoanTerm);
            sqlCommand.Parameters.AddWithValue("@type", loan.LoanType);
            sqlCommand.Parameters.AddWithValue("@status", loan.LoanStatus);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            int status = sqlCommand.ExecuteNonQuery();
            return status;
        }

        public decimal CalculateInterest(int id)
        {
            sqlCommand.CommandText = "select * from Loan where LoanID = @loanId";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@loanId", id);
            sqlCommand.Connection = sqlConnection;

            Loan loan = null;
            sqlConnection.Open();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        loan = new Loan()
                        {
                            PrincipalAmount = (int)reader["PrincipalAmount"],
                            InterestRate = (decimal)reader["InterestRate"],
                            LoanTerm = (int)reader["Loanterm"]
                        };
                    }
                }
            }
            sqlConnection.Close();
            decimal interestAmount = (loan.PrincipalAmount * loan.InterestRate * loan.LoanTerm) / 12;
            return interestAmount;
        }

        public string LoanStatus(int id)
        {
            sqlCommand.CommandText = "select * from Loan l join Customer c on l.CustomerID = c.CustomerID where LoanId = @loanId";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@loanId", id);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            int score = 0;
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        score = (int)reader["CreditScore"];
                    }
                }
            }

            string status;
            if (score > 650)
            {
                status = "Approved";
            }
            else
            {
                status = "Rejected";
            }

            sqlCommand.CommandText = "update loan set LoanStatus = @status where LoanID = @id";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@status", status);
            sqlCommand.Parameters.AddWithValue("@id", id);

            int updateStatus = sqlCommand.ExecuteNonQuery();
            if (updateStatus > 0)
            {
                Console.WriteLine("Updated successfully");
            }
            else
            {
                Console.WriteLine("Updation Failed");
            }
            return status;
        }

        public decimal CalculateEMI(int id)
        {
            Loan loan = null;
            sqlCommand.CommandText = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanID = @LoanID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@LoanID", id);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        loan = new Loan()
                        {
                            PrincipalAmount = (int)reader["PrincipalAmount"],
                            InterestRate = (decimal)reader["Interestrate"],
                            LoanTerm = (int)reader["LoanTerm"]
                        };
                    }
                }
            }
            sqlConnection.Close();
            if (loan == null)
            {
                throw new InvalidLoanException($"\nLoan with ID {id} not found.");
            }

            decimal monthlyInterestRate = loan.InterestRate / 12 / 100;
            int loanTenure = loan.LoanTerm;

            decimal emi = (loan.PrincipalAmount * monthlyInterestRate * (decimal)Math.Pow((double)(1 + monthlyInterestRate), loanTenure)) /
                           ((decimal)Math.Pow((double)(1 + monthlyInterestRate), loanTenure) - 1);
            decimal rounded = decimal.Round(emi, 3);
            return rounded;
        }
        public void LoanRepayment(int loanId, decimal paymentAmount)
        {
            decimal emi = CalculateEMI(loanId);
            // Log calculated EMI for debugging
            Console.WriteLine($"Calculated EMI for Loan ID {loanId}: {emi}");

            if (paymentAmount < emi)
            {
                Console.WriteLine("Payment amount is less than the EMI. Payment rejected.");
                return;
            }

            // Calculate the number of EMIs to be paid
            int noOfEmi = (int)(paymentAmount / emi);

            // Retrieve the current PrincipalAmount to validate
            sqlCommand.CommandText = "SELECT PrincipalAmount FROM Loan WHERE LoanID = @LoanID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@LoanID", loanId);
            sqlCommand.Connection = sqlConnection;

            sqlConnection.Open();
            decimal currentPrincipalAmount = 0;

            using (var reader = sqlCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    currentPrincipalAmount = (int)reader["PrincipalAmount"];
                }
            }

            sqlConnection.Close();


            // Ensure that we do not attempt to set the PrincipalAmount to less than or equal to zero
            decimal newPrincipalAmount = currentPrincipalAmount - (emi * noOfEmi);

            if (newPrincipalAmount < 0)
            {
                Console.WriteLine("Payment exceeds the outstanding principal amount. Payment rejected.");
                return;
            }          
        // Update the Loan record
        sqlCommand.CommandText = "UPDATE Loan SET PrincipalAmount = @newPrincipalAmount, LoanStatus = CASE WHEN @newPrincipalAmount <= 0 THEN 'Paid' ELSE LoanStatus END WHERE LoanID = @LoanID1";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@newPrincipalAmount", newPrincipalAmount);
        sqlCommand.Parameters.AddWithValue("@LoanID1", loanId);
        sqlCommand.Connection = sqlConnection;

        sqlConnection.Open();
        int affectedRows = sqlCommand.ExecuteNonQuery();
        sqlConnection.Close();

        // Calculate remaining amount to pay
        decimal remainingAmountToPay = newPrincipalAmount;

        if (affectedRows > 0)
        {
            Console.WriteLine($"Payment successful. {noOfEmi} EMIs have been paid.");
            Console.WriteLine($"Remaining amount to pay: {remainingAmountToPay}");
        }
        else
        {
            Console.WriteLine("No records were updated. Please check the loan ID or the EMI amount.");
        }
        }

        public List<Loan> GetAllLoans()
        {
            List<Loan> loans = new List<Loan>();

            sqlCommand.CommandText = "SELECT LoanID, CustomerID, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus FROM Loan";
            sqlCommand.Connection = sqlConnection;

            sqlConnection.Open();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Loan loan = new Loan()
                    {
                        LoanID = (int)reader["LoanID"],
                        CustomerID = (int)reader["CustomerID"], // Assuming you have a CustomerID property
                        PrincipalAmount = (int)reader["PrincipalAmount"],
                        InterestRate = (decimal)reader["InterestRate"],
                        LoanTerm = (int)reader["LoanTerm"],
                        LoanType = (string)reader["LoanType"],
                        LoanStatus = (string)reader["LoanStatus"]
                    };
                    loans.Add(loan);
                }
            }
            sqlConnection.Close();
            return loans;
        }

        public Loan GetLoanById(int loanId)
        {
            Loan loan = null;

            sqlCommand.CommandText = "SELECT LoanID, CustomerID, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus FROM Loan WHERE LoanID = @LoanID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@LoanID", loanId);
            sqlCommand.Connection = sqlConnection;

            sqlConnection.Open();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    loan = new Loan()
                    {
                        LoanID = (int)reader["LoanID"],
                        CustomerID = (int)reader["CustomerID"], 
                        PrincipalAmount = (int)reader["PrincipalAmount"],
                        InterestRate = (decimal)reader["InterestRate"],
                        LoanTerm = (int)reader["LoanTerm"],
                        LoanType = (string)reader["LoanType"],
                        LoanStatus = (string)reader["LoanStatus"]
                    };
                }
            }
            sqlConnection.Close();

            if (loan == null)
            {
                throw new InvalidLoanException($"\nLoan with ID {loanId} not found.");
            }

            return loan;
        }
    }
}
