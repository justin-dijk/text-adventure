using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room startCell = new Room("an old cell, it's very dirty");
		Room hallway = new Room("a hallway made of old stones, there is some rubble to the north");
		Room cell2 = new Room("another cell, it looks about as bad as the first");
		Room cell3 = new Room("another cell, there is a patch of dirt on the floor");
		Room secretBasement = new Room("a small hole, there's hardly any room to walk");
		Room office = new Room(
			"an office, it must have belonged to the warden, it's the nicest looking room here, there's a pedestal in front of a large door"
		);
		Room officeUpstairs = new Room("a small storage space above the office");
		Room supplyCloset = new Room("a supply closet, there's hardly anything useful here");
		Room endRoom = new Room("a room with a large door, could this be the exit?");

		// Initialise room exits
		startCell.AddClosedExit("west", hallway);

		hallway.AddExit("east", startCell);
		hallway.AddExit("west", cell3);
		hallway.AddExit("south", cell2);
		hallway.AddClosedExit("north", office);

		cell2.AddExit("north", hallway);

		cell3.AddExit("east", hallway);
		cell3.AddClosedExit("down", secretBasement);

		secretBasement.AddExit("up", cell3);

		office.AddExit("south", hallway);
		office.AddExit("up", officeUpstairs);
		office.AddClosedExit("west", endRoom);
		office.AddExit("north", supplyCloset);

		officeUpstairs.AddExit("down", office);

		supplyCloset.AddExit("south", office);

		endRoom.AddExit("east", office);
		
		endRoom.winsGame = true;

		// Create your Items here
		// ...
		Item key = new Item("key", 1, "a rusty key", "west", startCell.Id, ItemActions.UseKey);
		Item key2 = new Item(
			"pickaxe",
			1,
			"a wooden stick with a pointy piece of metal on the top",
			"north",
			hallway.Id,
			ItemActions.UseKey
		);
		Item key3 = new Item(
			"shovel",
			1,
			"a shovel, a bit broken but still usable",
			"down",
			cell3.Id,
			ItemActions.UseKey
		);
		Item key4 = new Item(
			"statue",
			1,
			"a weird statue, it must be important",
			"west",
			office.Id,
			ItemActions.UseKey
		);
		Item potion = new Item(
			"potion",
			1,
			"a small vial filled with a glowing green liquid",
			"null",
			startCell.Id,
			ItemActions.usePotion
		);

		// And add them to the Rooms
		// ...
		supplyCloset.Chest.Put("potion", potion);
		startCell.Chest.Put("key", key);

		cell2.Chest.Put("pickaxe", key2);

		secretBasement.Chest.Put("statue", key4);

		officeUpstairs.Chest.Put("shovel", key3);
		// Start game outside
		player.CurrentRoom = startCell;
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
			if (player.wincondition())
			{
				System.Console.WriteLine("                               _         _ _ _ ");
				System.Console.WriteLine("                              (_)       | | | |");
				System.Console.WriteLine("  _   _  ___  _   _  __      ___ _ __   | | | |");
				System.Console.WriteLine(@" | | | |/ _ \| | | | \ \ /\ / / | '_ \  | | | |");
				System.Console.WriteLine(@" | |_| | (_) | |_| |  \ V  V /| | | | | |_|_|_|");
				System.Console.WriteLine(@"  \__, |\___/ \__,_|   \_/\_/ |_|_| |_| (_|_|_)");
				System.Console.WriteLine("   __/ |                                       ");
				System.Console.WriteLine("  |___/                                        ");
				finished = true;
			}
			if (!player.IsAlive())
			{
				finished = true;
			}
			;
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to a  mostly original text adventure by Justin Dijk!");
		Console.WriteLine(
			"Good luck, try to reach the end, commands can be a maximum of 3 words long"
		);
		Console.WriteLine("be sure to type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine();
		Console.WriteLine();
		Console.WriteLine(
			"You wake up in an unfamiliar place, you look around and notice you are in an old prison cell"
		);
		Console.WriteLine("the cell seems to be abandoned, there is a locked door to the west");
		Console.WriteLine(
			"I feel myself becoming weaker, I should make sure to not move around too much..."
		);
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				Look(command);
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				Use(command);
				break;
		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("commands are formulated in a simple way.");
		Console.WriteLine(
			"to look at a room, say: 'look room'. to move, say: 'go west'. to use an item, say: 'use item door'."
		);
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if (!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to " + direction + "!");
			return;
		}

		player.CurrentRoom = nextRoom;
		player.Damage(5);
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		
	}

	private void Look(Command command)
	{
		if (!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("what do you want to look at?");
			return;
		}

		string target = command.SecondWord;

		// Try to go to the next room.

		if (target == "room")
		{
			Console.WriteLine(player.CurrentRoom.GetLongDescription());
			if (player.CurrentRoom.Chest.Show() == "")
			{
				System.Console.WriteLine("the room contains no useful items");
			}
			else
			{
				System.Console.WriteLine($"the room contains {player.CurrentRoom.Chest.Show()}.");
			}
		}
		else if (target == "self")
		{
			player.check();
		}
		else
		{
			Console.WriteLine("I did not understand that");
		}
	}

	private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			System.Console.WriteLine("what do you want to take?");
			return;
		}

		string target = command.SecondWord;

		player.TakeFromChest(target);
	}

	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			System.Console.WriteLine("what do you want to drop?");
			return;
		}

		string target = command.SecondWord;

		player.DropToChest(target);
	}

	private void Use(Command command)
	{
		if (!command.HasSecondWord())
		{
			System.Console.WriteLine("what do you want to use?");
			return;
		}

		string useTarget = command.SecondWord;

		if (command.hasThirdWord())
		{
			string useOnTarget = command.ThirdWord;
			player.Use(useTarget, useOnTarget);
		}
		else
		{
			player.Use(useTarget);
		}
	}
}
