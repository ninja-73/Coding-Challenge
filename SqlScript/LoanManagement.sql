create database LoanManagement
use LoanManagement

-- Creating Tables
create table Customer (
CustomerID int identity primary key,
Name varchar(30) not null,
Email varchar(30),
PhoneNumber varchar(30),
Address varchar(50),
CreditScore int
)

create table Loan (
LoanID int identity primary key,
CustomerID int,
PrincipalAmount int,
InterestRate decimal(5,2),
LoanTerm int,
LoanType varchar(30),
LoanStatus varchar(30),
Foreign Key (CustomerID) references Customer(CustomerID)
)

create table HomeLoan (
HomeLoanID int identity primary key,
LoanID int,
PropertyAddress varchar(50),
PropertyValue int,
Foreign key (LoanID) references Loan(LoanID)
)

create table CarLoan (
CarLoanId int identity primary key,
LoanID int,
CarModel varchar(30),
CarValue int,
Foreign Key (LoanID) references Loan(LoanID)
)

-- Inserting sample data 
INSERT INTO Customer (Name, Email, PhoneNumber, Address, CreditScore) 
VALUES 
('John Doe', 'john.doe@email.com', '1234567890', '123 Maple St', 750),
('Jane Smith', 'jane.smith@email.com', '9876543210', '456 Oak St', 680),
('Sam Wilson', 'sam.wilson@email.com', '8765432109', '789 Pine St', 800),
('Emily Davis', 'emily.davis@email.com', '7654321098', '321 Cedar St', 720),
('Michael Lee', 'michael.lee@email.com', '6543210987', '654 Elm St', 640);

INSERT INTO Loan (CustomerID, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus) 
VALUES 
(1, 500000, 6.75, 120, 'HomeLoan', 'Approved'),
(2, 300000, 7.50, 60, 'CarLoan', 'Pending'),
(3, 800000, 6.00, 180, 'HomeLoan', 'Approved'),
(4, 200000, 8.25, 48, 'CarLoan', 'Approved'),
(5, 450000, 7.00, 72, 'CarLoan', 'Pending');

INSERT INTO HomeLoan (LoanID, PropertyAddress, PropertyValue) 
VALUES 
(1, '101 Park Ave', 600000),
(3, '202 Green St', 900000);

INSERT INTO CarLoan (LoanID, CarModel, CarValue) 
VALUES 
(2, 'Honda Accord', 250000),
(4, 'Toyota Camry', 220000),
(5, 'Ford Mustang', 280000);


select * from customer
select * from loan
select * from homeloan

drop table carloan
drop table homeloan
drop table loan
drop table customer
