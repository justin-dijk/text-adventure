using System.Collections.Generic;

class Room
{
	// Private fields
	private string description;
	public Guid Id { get; }=Guid.NewGuid();
	public bool winsGame;

	private Inventory chest;
	private Dictionary<string, Room> exits; // stores exits of this room.
	private Dictionary<string, Room> closedExits;

	public Inventory Chest
	{
		get { return chest; }
	}
	

	// Create a room described "description". Initially, it has no exits.
	// "description" is something like "in a kitchen" or "in a court yard".
	public Room(string desc)
	{
		description = desc;
		winsGame = false;
		exits = new Dictionary<string, Room>();
		closedExits = new Dictionary<string, Room>();
		chest = new Inventory(999999);
	}

	// Define an exit for this room.
	public void AddExit(string direction, Room neighbor)
	{
		exits.Add(direction, neighbor);
	}
		public void AddClosedExit(string direction, Room neighbor)
	{
		closedExits.Add(direction, neighbor);
	}
	public void OpenExits(string direction)
	{
		if (closedExits.TryGetValue(direction, out Room room))
		{
			closedExits.Remove(direction);
			exits.Add(direction, room);
		}
	}

	// Return the description of the room.
	public string GetShortDescription()
	{
		return description;
	}

	// Return a long description of this room, in the form:
	//     You are in the kitchen.
	//     Exits: north, west
	public string GetLongDescription()
	{
		string str = "You are in ";
		str += description;
		str += ".\n";
		str += GetExitString();
		return str;
	}

	// Return the room that is reached if we go from this room in direction
	// "direction". If there is no room in that direction, return null.
	public Room GetExit(string direction)
	{
		if (exits.ContainsKey(direction))
		{
			return exits[direction];
		}
		return null;
	}

	// Return a string describing the room's exits, for example
	// "Exits: north, west".
	public string GetExitString()
	{
		string str = "Exits:";

		// Build the string in a `foreach` loop.
		// We only need the keys.
		int countCommas = 0;
		foreach (string key in exits.Keys)
		{
			if (countCommas != 0)
			{
				str += ",";
			}
			str += " " + key;
			countCommas++;
		}

		return str;
	}
}
