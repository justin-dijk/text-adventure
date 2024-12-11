using System.Reflection.Metadata;

class Item
{
	// fields
	public string Name { get; }
	public string DoorDirection { get; }
	public int Weight { get; }
	public string Description { get; }
	public ItemActions Action { get; }
	public string UseText { get; }
	public Guid RoomId { get; }= Guid.Empty;
	// constructor
	public Item(string name, int weight, string description, string doorDirection, Guid roomId, ItemActions action = ItemActions.NoAction, string useText = "can't use")
	{
		Name = name;
		Weight = weight;
		Description = description;
		Action = action;
		UseText = useText;
		RoomId = roomId;
		DoorDirection = doorDirection;
	}

	
	public void use(string onItemName, Room currentRoom, string DoorDirection)
	{
		if (Action == ItemActions.SingleUse || Action == ItemActions.NoAction) {
			System.Console.WriteLine(UseText);
		} else if (Action == ItemActions.UseKey ) 
		{
			if (onItemName == "door")
			{
				if (currentRoom.Id ==  RoomId) 
				{
					currentRoom.OpenExits(DoorDirection);
					System.Console.WriteLine($"you open the door on the {DoorDirection} end of the room");
				}
				else
				{
					System.Console.WriteLine("you are in the wrong room for that");
				}
			}
			else if (onItemName == "rubble")
			{
				if (currentRoom.Id ==  RoomId) 
				{
					currentRoom.OpenExits(DoorDirection);
					System.Console.WriteLine($"you remove the rubble, creating a new door to {DoorDirection}");
				}
				else
				{
					System.Console.WriteLine("you are in the wrong room for that");
				}
			}
			else if (onItemName == "dirt")
			{
				if (currentRoom.Id ==  RoomId) 
				{
					currentRoom.OpenExits(DoorDirection);
					System.Console.WriteLine($"you dig away the dirt, revealing a way {DoorDirection}");
				}
				else
				{
					System.Console.WriteLine("you are in the wrong room for that");
				}
			}
			else if (onItemName == "pedestal")
			{
				if (currentRoom.Id ==  RoomId) 
				{
					currentRoom.OpenExits(DoorDirection);
					System.Console.WriteLine($"you put the statue on the pedestal opening the door to the {DoorDirection}");
				}
				else
				{
					System.Console.WriteLine("you are in the wrong room for that");
				}
			}
			}
			else
			{
				System.Console.WriteLine("use item on what?");
			}
		}
	}

