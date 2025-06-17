namespace Restaurant
{
    internal class Program
    {

        static CategoryNode root;
        static List<string> allOrderedItems = new List<string>();
        static Dictionary<string, double> allOrderedPrices = new Dictionary<string, double>();

        static Dictionary<int, string> reservedTables = new Dictionary<int, string>();

        static HashSet<string> orderedItemsSet = new HashSet<string>();

        static List<(string itemName, double price)> orderedItemsList = new List<(string, double)>();

        static int ArrowMenu(string[] options)
        {
            int selected = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("===== Restaurant Main Menu =====\n");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"> {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {options[i]}");
                    }
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selected = (selected == 0) ? options.Length - 1 : selected - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selected = (selected == options.Length - 1) ? 0 : selected + 1;
                }

            } while (key != ConsoleKey.Enter);

            return selected;
        }

        const string ReservationFile = "reservations.txt";

        static void LoadReservationsFromFile()
        {
            if (!File.Exists(ReservationFile))
                return;

            var lines = File.ReadAllLines(ReservationFile);
            foreach (var line in lines)
            {
                var parts = line.Split(";", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && int.TryParse(parts[0], out int tableNum))
                {
                    reservedTables[tableNum] = parts[1];
                }
            }
        }

        static void SaveReservationsToFile()
        {
            var lines = reservedTables.Select(kvp => $"{kvp.Key};{kvp.Value}");
            File.WriteAllLines(ReservationFile, lines);
        }

        static void Main(string[] args)
        {
            root = AddClasses();
            LoadReservationsFromFile();
            while (true)
            {
                string[] menuOptions = { "View Orders", "View Reserved Tables", "Make an Order", "Exit" };
                int choice = ArrowMenu(menuOptions);

                switch (choice)
                {
                    case 0:
                        ViewOrders();
                        break;
                    case 1:
                        ViewReservedTables();
                        break;
                    case 2:
                        OrderPage(root);
                        break;
                    case 3:
                        Console.WriteLine("Exiting... Goodbye!");
                        return;
                }
            }

            static void ShowStartingMenu()
            {
                Console.WriteLine("===== Restaurant Main Menu =====");
                Console.WriteLine("1. View Orders");
                Console.WriteLine("2. View Reserved Tables");
                Console.WriteLine("3. Make an Order");
                Console.WriteLine("4. Exit");
                Console.WriteLine("================================");
            }

            static void ViewOrders()
            {
                Console.Clear();
                Console.WriteLine("===== Current Orders =====");
                if (allOrderedItems.Count == 0)
                {
                    Console.WriteLine("No orders yet.");
                }
                else
                {
                    double total = 0;
                    for (int i = 0; i < allOrderedItems.Count; i++)
                    {
                        string item = allOrderedItems[i];
                        double price = allOrderedPrices[item];
                        Console.WriteLine($"{i + 1}. {item} - ${price:F2}");
                        total += price;
                    }
                    Console.WriteLine("-------------------------");
                    Console.WriteLine($"Total: ${total:F2}");
                }
            }


            static void OrderPage(CategoryNode root)
            {
                orderedItemsSet.Clear();
                orderedItemsList.Clear();

                Console.WriteLine("Enter a category (e.g., Salads, Pizza, Sushi):");
                string categoryInput = Console.ReadLine();
                var category = root?.Search(categoryInput);

                if (category != null)
                {
                    Console.WriteLine($"\nItems in '{category.Category}':\n");
                    foreach (var item in category.Items)
                    {
                        Console.WriteLine($"- {item.Key}: ${item.Value:F2}");
                    }

                    Console.WriteLine("\nTo order, type the item name followed by ' O' (e.g., Tiramisu O)");
                    Console.WriteLine("Type 'Ready' when you're done.\n");

                    while (true)
                    {
                        Console.Write("Order: ");
                        string input = Console.ReadLine();

                        if (input.Trim().Equals("Ready", StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }

                        string[] parts = input.Split('O');
                        if (parts.Length == 2)
                        {
                            string itemName = parts[0].Trim();

                            if (category.Items.ContainsKey(itemName))
                            {
                                if (orderedItemsSet.Add(itemName))
                                {
                                    orderedItemsList.Add((itemName, category.Items[itemName]));
                                    Console.WriteLine($"✅ Added: {itemName} - ${category.Items[itemName]:F2}");
                                }
                                else
                                {
                                    Console.WriteLine("⚠️ Item already ordered.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("❌ Item not found in this category.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("❌ Invalid format. Use: ItemName O");
                        }
                    }

                    Console.WriteLine("\n🧾 Order Summary:");
                    double totalPrice = 0;
                    foreach (var order in orderedItemsList)
                    {
                        Console.WriteLine($"- {order.itemName} - ${order.price:F2}");
                        totalPrice += order.price;
                    }
                    Console.WriteLine($"Total: ${totalPrice:F2}");
                }
                else
                {
                    Console.WriteLine("❌ Category not found.");
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }



            static CategoryNode AddClasses()
            {

                CategoryNode root = null;

                CategoryNode AddCategory(string name, Dictionary<string, double> items)
                {
                    var node = new CategoryNode(name);
                    foreach (var kvp in items)
                    {
                        node.Items.Add(kvp.Key, kvp.Value);
                    }

                    if (root == null)
                        root = node;
                    else
                        root.Insert(node);

                    return node;
                }

                AddCategory("Salads", new Dictionary<string, double>
        {
            {"Caesar Salad", 9.50},
            {"Greek Salad", 10.00},
            {"Quinoa Avocado Bowl", 11.50},
            {"Caprese Salad", 9.75},
            {"Asian Chicken Salad", 12.00}
        });

                AddCategory("Burgers", new Dictionary<string, double>
        {
            {"Classic Cheeseburger", 11.00},
            {"BBQ Bacon Burger", 12.50},
            {"Veggie Black Bean Burger", 10.50}
        });

                AddCategory("Steaks", new Dictionary<string, double>
        {
            {"Ribeye", 27.50},
            {"Filet Mignon", 32.00},
            {"New York Strip", 25.00}
        });

                AddCategory("Pizza", new Dictionary<string, double>
        {
            {"Margherita Pizza", 14.00},
            {"Meat Lovers Pizza", 16.50}
        });

                AddCategory("Soup", new Dictionary<string, double>
        {
            {"Creamy Tomato Basil", 5.50},
            {"Chicken Tortilla Soup", 6.25}
        });

                AddCategory("Sushi", new Dictionary<string, double>
        {
            {"California Roll", 8.00},
            {"Spicy Tuna Roll", 9.00},
            {"Dragon Roll", 12.00}
        });

                AddCategory("Deserts", new Dictionary<string, double>
        {
            {"Chocolate Lava Cake", 7.00},
            {"New York Cheesecake", 6.50},
            {"Tiramisu", 6.75}
        });

                return root;
            }



            static void ViewReservedTables()
            {
                while (true)
                {
                    string[] subMenuOptions = {
                        "View Reserved Tables",
                        "Reserve a Table",
                        "Cancel a Reservation",
                        "Return to Main Menu"
                    };

                    int choice = ArrowMenu(subMenuOptions);

                    Console.Clear();

                    switch (choice)
                    {
                        case 0:
                            ShowReservedTables();
                            break;
                        case 1:
                            ReserveTable();
                            break;
                        case 2:
                            CancelReservation();
                            break;
                        case 3:
                            return;
                    }

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();


                    static void ShowReservedTables()
                    {
                        Console.Clear();
                        Console.WriteLine("===== Current Reserved Tables =====");
                        if (reservedTables.Count == 0)
                        {
                            Console.WriteLine("No tables reserved yet.");
                        }
                        else
                        {
                            foreach (var kvp in reservedTables)
                            {
                                Console.WriteLine($"Table {kvp.Key}: Reserved by {kvp.Value}");
                            }
                        }
                    }

                    static void ReserveTable()
                    {
                        Console.Clear();
                        Console.WriteLine("===== Reserve a Table =====");

                        Console.Write("Enter table number to reserve (e.g., 1-20): ");
                        if (!int.TryParse(Console.ReadLine(), out int tableNumber))
                        {
                            Console.WriteLine("Invalid table number.");
                            return;
                        }

                        if (reservedTables.ContainsKey(tableNumber))
                        {
                            Console.WriteLine($"Table {tableNumber} is already reserved by {reservedTables[tableNumber]}.");
                            return;
                        }

                        Console.Write("Enter your name: ");
                        string name = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(name))
                        {
                            Console.WriteLine("Name cannot be empty.");
                            return;
                        }

                        reservedTables[tableNumber] = name.Trim();
                        Console.WriteLine($"Table {tableNumber} successfully reserved for {name}.");
                        SaveReservationsToFile();
                    }

                    static void CancelReservation()
                    {
                        Console.Clear();
                        Console.WriteLine("===== Cancel a Reservation =====");

                        Console.Write("Enter table number to cancel reservation: ");
                        if (!int.TryParse(Console.ReadLine(), out int tableNumber))
                        {
                            Console.WriteLine("Invalid table number.");
                            return;
                        }

                        if (reservedTables.Remove(tableNumber))
                        {
                            Console.WriteLine($"Reservation for table {tableNumber} has been cancelled.");
                            SaveReservationsToFile();
                        }
                        else
                        {
                            Console.WriteLine($"Table {tableNumber} was not reserved.");
                        }
                    }
                }
            }
        }
    }
}