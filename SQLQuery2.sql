-- Create database (through Visual Studio Server Explorer)
-- Database name: ClubDB

-- Create table
CREATE TABLE ClubMembers (
    ID INT PRIMARY KEY,
    StudentId BIGINT,
    FirstName VARCHAR(50),
    MiddleName VARCHAR(50),
    LastName VARCHAR(50),
    Age INT,
    Gender VARCHAR(50),
    Program VARCHAR(150)
);