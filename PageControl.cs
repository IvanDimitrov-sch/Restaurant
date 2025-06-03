using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant;
internal class PageControl
{
    PageControl() { }
    public static void MainPage()
    {
        string text =
                        @" ------  ---------------   --------   -----------------    ----------------
                          | Menu | Reserved Tables | Receipts | Restaurant Orders? | TakeAway Orders  | 
                           ------  ---------------   --------   -----------------    ----------------
                             ||           ||            ||             ||                   ||
                             \/           \/            \/             \/                   \/
                             #1           #2            #3             #4                   #5

                                                                                               
                             >>> Return Home <<< (Press R)
                            "; // Reserved Tables == Tables Occupation

    }
    public static void DisplayAsciiMenu()
    {
        string menu1page = @"
                ---------------------- RESTAURANT MENU ----------------------
                | Item                     | Price                          |
                -------------------------------------------------------------
                | 1) Margherita Pizza      | $10.99                         |
                | 2) Cheeseburger          | $6.49                          |
                | 3) Caesar Salad          | $5.99                          |
                | 4) Grilled Salmon        | $14.35                         |
                | 5) Spaghetti Bolognese   | $8.75                          |
                | 6) French Fries          | $3.50                          |
                -------------------------------------------------------------
            Press [P] for Previous Page | Press [Q] to Quit | Press Numbers to Order
            ";
        string menu2page = @"
                ---------------------- RESTAURANT MENU ----------------------
                | Item                     | Price                          |
                -------------------------------------------------------------
                | 7) Chocolate Cake        | $4.99                          |
                | 8) Ice Cream Sundae      | $5.40                          |
                | 9) Tomato Soup           | $5.99                          |
                | 10) Steak & Chips        | $14.25                         |
                | 11) Pasta Carbonara      | $5.25                          |
                | 12) Lemonade             | $3.50                          |
                | 13) Iced Tea             | $2.99                          |
                -------------------------------------------------------------
            Press [P] for Previous Page | Press [Q] to Quit | Press Numbers to Order
            ";
    }
    public static void DisplayTables()
    {

    }

}
