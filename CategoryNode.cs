using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant;
internal class CategoryNode
{
    public string Category;
    public Dictionary<string, double> Items;
    public CategoryNode Left;
    public CategoryNode Right;

    public CategoryNode(string category)
    {
        Category = category;
        Items = new Dictionary<string, double>();
    }

    public void Insert(CategoryNode newNode)
    {
        if (string.Compare(newNode.Category, Category, StringComparison.OrdinalIgnoreCase) < 0)
        {
            if (Left == null)
                Left = newNode;
            else
                Left.Insert(newNode);
        }
        else
        {
            if (Right == null)
                Right = newNode;
            else
                Right.Insert(newNode);
        }
    }

    public CategoryNode Search(string categoryName)
    {
        if (Category.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
            return this;

        if (string.Compare(categoryName, Category, StringComparison.OrdinalIgnoreCase) < 0)
            return Left?.Search(categoryName);

        return Right?.Search(categoryName);
    }
}
