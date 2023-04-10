USE [master]
GO

/****** Object:  Database [HobbyShop]    Script Date: 4/10/2023 6:17:40 PM ******/
CREATE DATABASE [HobbyShop]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HobbyShop', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\HobbyShop.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HobbyShop_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\HobbyShop_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HobbyShop].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [HobbyShop] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [HobbyShop] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [HobbyShop] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [HobbyShop] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [HobbyShop] SET ARITHABORT OFF 
GO

ALTER DATABASE [HobbyShop] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [HobbyShop] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [HobbyShop] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [HobbyShop] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [HobbyShop] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [HobbyShop] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [HobbyShop] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [HobbyShop] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [HobbyShop] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [HobbyShop] SET  ENABLE_BROKER 
GO

ALTER DATABASE [HobbyShop] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [HobbyShop] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [HobbyShop] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [HobbyShop] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [HobbyShop] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [HobbyShop] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [HobbyShop] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [HobbyShop] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [HobbyShop] SET  MULTI_USER 
GO

ALTER DATABASE [HobbyShop] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [HobbyShop] SET DB_CHAINING OFF 
GO

ALTER DATABASE [HobbyShop] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [HobbyShop] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [HobbyShop] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [HobbyShop] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [HobbyShop] SET QUERY_STORE = ON
GO

ALTER DATABASE [HobbyShop] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

ALTER DATABASE [HobbyShop] SET  READ_WRITE 
GO

