-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 16, 2025 at 05:42 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `finacore`
--

-- --------------------------------------------------------

--
-- Table structure for table `company_client`
--

CREATE TABLE `company_client` (
  `client_id` int(7) NOT NULL,
  `client_name` varchar(255) DEFAULT NULL,
  `client_code` int(7) DEFAULT NULL,
  `contact` varchar(15) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `address` text DEFAULT NULL,
  `status` varchar(12) DEFAULT NULL,
  `client_date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `company_client`
--

INSERT INTO `company_client` (`client_id`, `client_name`, `client_code`, `contact`, `email`, `address`, `status`, `client_date`) VALUES
(213990, 'Axa Life Corp', 6285147, '09730908910', 'axalifecorp@gmail.com', 'Twin Neo Towers Makati City Philippines', 'Active', '2025-04-16');

-- --------------------------------------------------------

--
-- Table structure for table `company_transactions`
--

CREATE TABLE `company_transactions` (
  `transaction_id` int(7) NOT NULL,
  `client_transaction_id` int(7) DEFAULT NULL,
  `transaction_code` int(7) DEFAULT NULL,
  `transaction_type` varchar(20) DEFAULT NULL,
  `transaction_amount` decimal(10,2) DEFAULT NULL,
  `transaction_date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `company_transactions`
--

INSERT INTO `company_transactions` (`transaction_id`, `client_transaction_id`, `transaction_code`, `transaction_type`, `transaction_amount`, `transaction_date`) VALUES
(3166625, 213990, 5659950, 'Loan', 31000.00, '2025-04-16');

-- --------------------------------------------------------

--
-- Table structure for table `item_purchased`
--

CREATE TABLE `item_purchased` (
  `transaction_purchased_id` int(7) NOT NULL,
  `item_code` int(7) DEFAULT NULL,
  `item_name` varchar(255) DEFAULT NULL,
  `item_type` varchar(20) DEFAULT NULL,
  `item_amount` decimal(10,2) DEFAULT NULL,
  `item_quantity` int(3) DEFAULT NULL,
  `purchased_date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `item_purchased`
--

INSERT INTO `item_purchased` (`transaction_purchased_id`, `item_code`, `item_name`, `item_type`, `item_amount`, `item_quantity`, `purchased_date`) VALUES
(3166625, 8342692, 'Dell Latitdue', 'Laptop', 31000.00, 1, '2025-04-16');

-- --------------------------------------------------------

--
-- Table structure for table `login_user`
--

CREATE TABLE `login_user` (
  `username` varchar(15) DEFAULT NULL,
  `password` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `login_user`
--

INSERT INTO `login_user` (`username`, `password`) VALUES
('FinaCore', 'Database01');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `company_client`
--
ALTER TABLE `company_client`
  ADD PRIMARY KEY (`client_id`);

--
-- Indexes for table `company_transactions`
--
ALTER TABLE `company_transactions`
  ADD PRIMARY KEY (`transaction_id`),
  ADD KEY `FK_client_transaction` (`client_transaction_id`);

--
-- Indexes for table `item_purchased`
--
ALTER TABLE `item_purchased`
  ADD PRIMARY KEY (`transaction_purchased_id`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `company_transactions`
--
ALTER TABLE `company_transactions`
  ADD CONSTRAINT `FK_client_transaction` FOREIGN KEY (`client_transaction_id`) REFERENCES `company_client` (`client_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `item_purchased`
--
ALTER TABLE `item_purchased`
  ADD CONSTRAINT `FK_transaction_item` FOREIGN KEY (`transaction_purchased_id`) REFERENCES `company_transactions` (`transaction_id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
