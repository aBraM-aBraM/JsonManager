using System;
using System.Collections.Generic;
using System.Text;

namespace JsonDB
{
    class DatabaseUserInterface
    {
        Database db;
        readonly string[] LABELS = new string[] { "Items", "Add" };
        ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));

        int choiceIndex = 0;
        int itemIndex = 0;

        bool is_backwards = false;

        public void UI()
        {
            db = new Database();
            PrintMenu();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    // Change choice position
                    if (key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow)
                    {
                        choiceIndex = 1 - choiceIndex;
                        PrintMenu();
                    }
                    // Choose item's menu or create new item
                    if (key == ConsoleKey.Enter)
                    {
                        if (choiceIndex == 1) Create_New_Item();
                        else ItemsMenu();
                        PrintMenu();
                    }
                  
                }
            }
        }

        private void ModifyMenu()
        {
            int serial = choiceIndex;
            Dictionary<string, string> itemData = Database.instance[serial];
            choiceIndex = 0;

            string currentKey = "";

            currentKey = PrintModMenu(itemData);
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.RightArrow)
                    {
                        if (choiceIndex + 1 < itemData.Count)
                        {
                            choiceIndex++;
                            currentKey = PrintModMenu(itemData);
                        }
                    }
                    if (key == ConsoleKey.LeftArrow)
                    {
                        if (choiceIndex > 0)
                        {
                            choiceIndex--;
                            currentKey = PrintModMenu(itemData);
                        }
                    }
                    if (key == ConsoleKey.Enter)
                    {
                        while (true)
                        {
                            int[] consolePos = new int[] { Console.CursorLeft, Console.CursorTop };
                            Console.CursorLeft = consolePos[0];
                            string new_value = Console.ReadLine();
                            
                            if(new_value != string.Empty)
                            {
                                Database.instance.Modify(itemIndex, currentKey, new_value);
                                PrintModMenu(itemData);
                                break;
                            }
                            
                        }
                    }
                    if(key == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
            }
        }


        private void ItemsMenu()
        {
            choiceIndex = 0;
            int count = PrintItemsMenu();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey mov = Console.ReadKey().Key;
                    // changing choices
                    if(mov == ConsoleKey.UpArrow)
                    {
                        if(!is_backwards && choiceIndex > 0)
                        {
                            choiceIndex--;
                            PrintItemsMenu();
                        }
                        else if(is_backwards && choiceIndex + 1 < count)
                        {
                            choiceIndex++;
                            PrintItemsMenu();
                        }

                    }
                    else if (mov == ConsoleKey.DownArrow)
                    {
                        if(!is_backwards && choiceIndex + 1 < count)
                        {
                            choiceIndex++;
                            PrintItemsMenu();
                        }
                        else if(is_backwards && choiceIndex > 0)
                        {
                            choiceIndex--;
                            PrintItemsMenu();
                        }
                    }
                    // starting to modify
                    else if (mov == ConsoleKey.Enter)
                    {
                        itemIndex = choiceIndex;
                        ModifyMenu();
                    }
                    // returns back
                    else if (mov == ConsoleKey.Escape)
                    {
                        choiceIndex = 0;
                        return;
                    }
                    // switch to backwards mode
                    else if (mov == ConsoleKey.Tab)
                    {
                        is_backwards = !is_backwards;
                        choiceIndex = Database.instance.Count - choiceIndex;
                        PrintItemsMenu();
                    }
                }
            }
        }
        private void Create_New_Item()
        {
            Dictionary<string, string> newItem = new Dictionary<string, string>();
            string[] arguments = Database.instance.arguments;

            int arg_index = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(arguments[arg_index]);
                string arg_input = Console.ReadLine();
                if (arg_input != string.Empty)
                {
                    newItem[arguments[arg_index]] = arg_input;
                    arg_index++;
                }
                if (arg_index == arguments.Length) break;
            }
            db.Add(newItem);
            Console.WriteLine("Added new item: " + db.GetItemString(db.Count - 1));
            
        }


        private string PrintModMenu(Dictionary<string, string> itemData)
        {

            Console.Clear();
            // getting keys 
            List<string> keys = new List<string>();
            foreach (string key in itemData.Keys) keys.Add(key);
            // after having the keys we can have iteration yay :)
            for (int i = 0; i < keys.Count; i++)
            {
                Console.Write(CompleteToLength(keys[i], Math.Max(keys[i].Length, itemData[keys[i]].Length)));
                Console.Write("|");
            }
            Console.Write("\n");
            Console.Write("\n");
            for (int i = 0; i < keys.Count; i++)
            {
                Console.ResetColor();
                if (i == choiceIndex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(">");
                }
                Console.Write(CompleteToLength(itemData[keys[i]], Math.Max(keys[i].Length, itemData[keys[i]].Length)));
                Console.Write("|");
            }
            Console.ResetColor();
            return keys[choiceIndex];
            
        }
        private void PrintMenu()
        {
            string text = "";
            for (int i = 0; i < LABELS.Length; i++)
            {
                if (choiceIndex == i) text += "> ";
                text += LABELS[i];
                text += "\n";
            }
            Console.Clear();
            Console.WriteLine(text);
        }
        private int PrintItemsMenu()
        {
            // Dictionary is indexed with serial numbers
            Console.Clear();
            for (int i = is_backwards?Database.instance.Count - 1:0 ; is_backwards?(i > -1):(i < Database.instance.Count); i += is_backwards?-1:1)
            {
                if (i == choiceIndex) Console.Write("> ");
                Console.Write(i.ToString() + "| " + Database.instance.GetItemString(i));
                Console.Write("\n");
            }
            return Database.instance.Count;
        }

        private string CompleteToLength(string source, int length)
        {
            int difference = length - source.Length;
            if (length <= 0) return source;
            source += MultiplyString(" ", difference);
            return source;
        }
        private string MultiplyString(string source, int multiplier)
        {
            for (int i = 0; i < multiplier - 1; i++)
            {
                source += source;
            }
            return source;
        }
    }
}
