class Inventory
{
    // fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    // constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    // methods
    public bool Put(string itemName, Item item)
    {
        bool fits = false;
        if (FreeWeight() < item.Weight)
        {
            System.Console.WriteLine("I can't carry any more");
        }
        else if (items.ContainsKey(item.Name))
        {
            System.Console.WriteLine("this item exist already");
        }
        else
        {
            items.Add(item.Name, item);
            fits = true;
        }
        return fits;
    }

    public string Show()
    {
        string listOfItems = "";
        int i = 0;
        foreach (Item item in items.Values)
        {
            listOfItems += item.Name;
            i++;
            if (i < items.Values.Count)
            {
                listOfItems += ", ";
            }
        }
        return listOfItems;
    }

    public int TotalWeight()
    {
        int total = 0;
        foreach (Item item in items.Values)
        {
            total += item.Weight;
        }
        return total;
    }

    public int FreeWeight()
    {
        int freeWeight = maxWeight - TotalWeight();
        return freeWeight;
    }

    public Item Get(string itemName)
    {
        if (items.TryGetValue(itemName, out Item item))
        {
            items.Remove(itemName);
        }
        return item;
    }
}
