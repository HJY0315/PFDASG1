CREATE TABLE Users(
user_id INT NOT NULL,
user_name VARCHAR(50) NOT NULL,
access_code CHAR(7) NOT NULL,
phone_number VARCHAR(20) NOT NULL,
pin CHAR(6) NOT NULL
);

CREATE TABLE Account(
account_id INT NOT NULL,
account_type VARCHAR(20) NOT NULL,
account_balance DECIMAL(10,2) NOT NULL,
user_id INT NOT NULL
);

CREATE TABLE Transactions(
transaction_id INT NOT NULL,
user_id INT NOT NULL,
account_id INT NOT NULL,
amount DECIMAL(10,2) NOT NULL,
transaction_date DATETIME NOT NULL,
receiver_id INT NULL
);

