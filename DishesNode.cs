using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant;
internal class DishesNode
{
    public double Price { get; set; }
    public string Name { get; set; }
    public int AbsPrice { get; set; }

    public DishesNode Left;
    public DishesNode Right;
    public int Height;

    DishesNode(string name, double price)
    {
        Price = price;
        Name = name;
        AbsPrice = (int)Math.Floor(Price);
    }
}
