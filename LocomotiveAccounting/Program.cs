using Microsoft.Data.Sqlite;
using System;

namespace LocomotiveAccounting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=C:\\Users\\Alex\\Desktop\\COD\\SQLite\\locomotives.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                while (true)
                {
                    Console.WriteLine("\n====== Учет локомотивов ======");

                    Console.WriteLine("1 - Показать локомотивы.");

                    Console.WriteLine("2 - Добавить локомотив.");

                    Console.WriteLine("3 - Найти локомотив по серии.");

                    Console.WriteLine("4 - Выход.");

                    Console.Write("Выберите действие: ");

                    if (!int.TryParse(Console.ReadLine(), out int choice))
                    {
                        Console.WriteLine("Неверный ввод!!");

                        continue;
                    }

                    if (choice == 1)
                    {
                        string sql = "SELECT Series, Number, Depot FROM Locomotives";

                        using (var command = new SqliteCommand(sql, connection))
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["Series"]}-{reader["Number"]}, депо: {reader["Depot"]}");
                            }
                        }
                    }

                    else if (choice == 2)
                    {
                        Console.Write("Серия: ");

                        string series = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(series))
                        {
                            Console.WriteLine("Серия не может быть пустой!");

                            continue;
                        }

                        Console.Write("Номер: ");

                        string numberInput = Console.ReadLine();

                        if (!int.TryParse(numberInput, out int number))
                        {
                            Console.WriteLine("Номер должен быть числом!");

                            continue;
                        }

                        Console.Write("Депо: ");

                        string depot = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(depot))
                        {
                            Console.WriteLine("Депо не может быть пустым!");

                            continue;
                        }

                        string insertSql = "INSERT INTO Locomotives (Series, Number, Depot) VALUES (@Series, @Number, @Depot)";

                        using (var insertCommand = new SqliteCommand(insertSql, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@Series", series);

                            insertCommand.Parameters.AddWithValue("@Number", number);

                            insertCommand.Parameters.AddWithValue("@Depot", depot);

                            insertCommand.ExecuteNonQuery();
                        }

                        Console.WriteLine("Локомотив добавлен!");
                    }

                    else if (choice == 3)
                    {
                        Console.Write("Введите серию для поиска: ");

                        string searchSeries = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(searchSeries))
                        {
                            Console.WriteLine("Серия не может быть пустой!");

                            continue;
                        }

                        string searchSql = "SELECT Series, Number, Depot FROM Locomotives WHERE Series = @Series";

                        using (var searchCommand = new SqliteCommand(searchSql, connection))
                        {
                            searchCommand.Parameters.AddWithValue("@Series", searchSeries);

                            using (var reader = searchCommand.ExecuteReader())
                            {
                                bool found = false;

                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["Series"]}-{reader["Number"]}, депо: {reader["Depot"]}");

                                    found = true;
                                }
                                if (!found)
                                {
                                    Console.WriteLine("Локомотивы с такой серией не найдены.");
                                }
                            }
                        }
                    }

                    else if (choice == 4)
                    {
                        Console.WriteLine("До свидания!");

                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор!");
                    }
                }
            }
        }
    }
}
