namespace Restaurant
{
    internal class Program
    {

        static CategoryNode root;
        static List<string> allOrderedItems = new List<string>();  // store all orders globally
        static Dictionary<string, double> allOrderedPrices = new Dictionary<string, double>();
        
        // Track reserved tables (table number -> reserver name)
        static Dictionary<int, string> reservedTables = new Dictionary<int, string>();

        // Track ordered items without duplicates
        static HashSet<string> orderedItemsSet = new HashSet<string>();

        // Track ordered items with prices for summary and total calculation
        static List<(string itemName, double price)> orderedItemsList = new List<(string, double)>();


        static void Main(string[] args)
        {
            root = AddClasses();
            while (true)
            {
                ShowStartingMenu();

                Console.Write("Select an option (1-4): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewOrders();
                        break;
                    case "2":
                        ViewReservedTables();
                        break;
                    case "3":
                        OrderPage(root);
                        break;
                    case "4":
                        Console.WriteLine("Exiting... Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
                Console.Clear();
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
                            if (orderedItemsSet.Add(itemName))  // HashSet prevents duplicates
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

                // Summary
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


        // Your existing AddClasses() method here...
        static CategoryNode AddClasses()
        {
            // Same as your previous implementation
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
                Console.Clear();
                Console.WriteLine("===== Reserved Tables Menu =====");
                Console.WriteLine("1. View Reserved Tables");
                Console.WriteLine("2. Reserve a Table");
                Console.WriteLine("3. Cancel a Reservation");
                Console.WriteLine("4. Return to Main Menu");
                Console.WriteLine("================================");
                Console.Write("Select an option (1-4): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowReservedTables();
                        break;
                    case "2":
                        ReserveTable();
                        break;
                    case "3":
                        CancelReservation();
                        break;
                    case "4":
                        return; // back to main menu
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

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
            }
            else
            {
                Console.WriteLine($"Table {tableNumber} was not reserved.");
            }
        }


    }
}

