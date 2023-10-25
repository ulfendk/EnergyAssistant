// using Microsoft.Data.Sqlite;
// using UlfenDk.EnergyAssistant.Config;
//
// namespace UlfenDk.EnergyAssistant.Repository;
//
// public class EnergyAssistantRepository
// {
//     private readonly string _dbFileName;
//
//     public EnergyAssistantRepository(string dbFileName)
//     {
//         _dbFileName = dbFileName ?? throw new ArgumentNullException(nameof(dbFileName));
//     }
//
//     public void Test()
//     {
//         using var connection = new SqliteConnection($"Data Source={_dbFileName}");
//         connection.Open();
//
//         var command = connection.CreateCommand();
//         command.CommandText =
//         @"
//             IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'energyassistant')
//               BEGIN
//                 CREATE DATABASE energyassistant
//               END;
//             IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'SpotPrices')
//               BEGIN
//                   CREATE TABLE [dbo].[SpotPrices] (
//                     [CustomerID] INT            IDENTITY (1, 1) NOT NULL,
//                     [FirstName]  NVARCHAR (MAX) NULL,
//                     [LastName]   NVARCHAR (MAX) NULL,
//                     [Email]      NVARCHAR (MAX) NULL,
//                     CONSTRAINT [PK_dbo.Customers] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
//               END;
// );
//         ";
//
//         using (var reader = command.ExecuteReader())
//         {
//             while (reader.Read())
//             {
//                 var name = reader.GetString(0);
//
//                 Console.WriteLine($"Hello, {name}!");
//             }
//         }
//     }
// }