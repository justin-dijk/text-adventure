using System.Net.NetworkInformation;
using System.Threading.Tasks.Dataflow;

class Player
{
    // auto property
    public Room CurrentRoom { get; set; }
    private int health;
    private Inventory Hands;

    // constructor
    public Player()
    {
        CurrentRoom = null;
        health = 100;
        Hands = new Inventory(2);
    }

    //methods
    public void Damage(int amount)
    {
        Console.WriteLine($"you take {amount} points of damage");
        health -= amount;
    } // player loses some health

    public void Heal(int amount)
    {
        Console.WriteLine($"you heal {amount} point of health");
        health += amount;
    } // player's health restores

    public void check()
    {
        Console.WriteLine($"your health is at {health} points");
        if (Hands.Show() == "")
        {
            System.Console.WriteLine("you have nothing in your inventory");
        }
        else
        {
            System.Console.WriteLine($"your inventory contains {Hands.Show()}");
        }
    }

    public bool IsAlive()
    {
        bool isAlive = true;
        if (health <= 0)
        {
            Console.WriteLine("you have died");
            isAlive = false;
        }
        return isAlive;
    } // checks whether the player is alive or not

    public bool TakeFromChest(string itemName)
    {
        bool ItemTaken = true;
        Item item = CurrentRoom.Chest.Get(itemName);
        if (item == null)
        {
            System.Console.WriteLine("this item does not exist in the current room");
            ItemTaken = false;
        }
        else if (!Hands.Put(item.Name, item))
        {
            CurrentRoom.Chest.Put(item.Name, item);
            System.Console.WriteLine($"you put {item.Name} back");
            ItemTaken = false;
        }
        else
        {
            System.Console.WriteLine($"you took {item.Name} and put it in your backpack");
        }
        return ItemTaken;
    }

    public bool DropToChest(string itemName)
    {
        bool ItemDropped = true;
        Item item = Hands.Get(itemName);
        if (item == null)
        {
            System.Console.WriteLine("you do not have this item in your backpack");
            ItemDropped = false;
        }
        else
        {
            CurrentRoom.Chest.Put(itemName, item);
            System.Console.WriteLine($"you dropped {item.Name}");
        }
        return ItemDropped;
    }

    public void usePotion(string itemName)
    {
        Heal(50);
        Hands.Get(itemName);
    }

    public void Use(string itemName, string onItemName = null)
    {
        Item item = Hands.Get(itemName);
        if (item == null)
        {
            System.Console.WriteLine("you don't have that");
            return;
        }
        if (
            itemName == "key"
            || itemName == "statue"
            || itemName == "shovel"
            || itemName == "pickaxe"
        )
        {
            if (onItemName == null)
            {
                System.Console.WriteLine("use on what?");
            }
        }
        if (itemName == "potion")
        {
            usePotion(itemName);
            return;
        }
        item.use(onItemName, CurrentRoom, item.DoorDirection);
        Hands.Put(itemName, item);
    }

    public bool wincondition()
    {
        return CurrentRoom.winsGame;
    }
}
