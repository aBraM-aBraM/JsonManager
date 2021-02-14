using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Linq;

namespace JsonDB
{
    class Database
    {
        private Dictionary<string, Dictionary<string, string>> Data;
        public int Count
        {
            get
            {
                return Data.Count;
            }
        }
        public Dictionary<string,string> this[int index]
        {
            get
            {
                return Data[index.ToString()];
            }
        }
        public static Database instance = null;

        const string path = "values.val";
        const string config_path = "db_config.cfg";

        readonly string config_default = "[" + '"' + "name" + '"' + "," + '"' + "type" + '"' + "]";
        public string[] arguments {  get; private set; }


        public Database()
        {
            if (instance == null) instance = this;
            else throw new Exception("Singleton instance already exists");
            Load();
        }

        public void Load()
        {
            if (!File.Exists(path))
            {
                using (var stream = File.Open(path, FileMode.Create))
                {
                    AddText(stream, "{}");
                }
            }
            string jsonString = File.ReadAllText(path);
            Data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonString);


            if (!File.Exists(config_path))
            {
                using (var stream = File.Open(config_path, FileMode.Create))
                {
                    AddText(stream, config_default);
                }
            }
            string cfgJson = File.ReadAllText(config_path);
            arguments = JsonSerializer.Deserialize<string[]>(cfgJson);
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(Data);
            File.WriteAllText(path, jsonString);           
        }

        public void Add(Dictionary<string,string> item_data)
        {
            foreach(string arg in arguments)
            {
                if (!item_data.ContainsKey(arg)) throw new Exception("Argument " + arg + " not found in item's data");
            }
            string key = Data.Count.ToString();
            Console.WriteLine("Added " + ItemToString(item_data));
            Data[key] = item_data;
            Save();
        }

        public void Remove(int item_index)
        {
            Data.Remove(item_index.ToString());
        }


        public void Modify(int item_index, string key, string value)
        {
            if (Data.ContainsKey(item_index.ToString()))
            {
                if (Data[item_index.ToString()].ContainsKey(key))
                {
                    if (value != string.Empty)
                    {
                        Data[item_index.ToString()][key] = value;
                        Save();
                    }
                }
            }
        }

        public string GetDatabaseString()
        {
           var lines = Data.Select(kvp => kvp.Key + ": " + string.Join("/", kvp.Value.Select(_kvp => _kvp.Key + "," + _kvp.Value.ToString())));
           return string.Join(Environment.NewLine, lines);
        }

        public string GetItemString(int itemIndex)
        {
            var lines = Data[itemIndex.ToString()].Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            return string.Join(" ", lines);
        }

        public static string ItemToString(Dictionary<string,string> item)
        {
            var lines = item.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            return string.Join(" ", lines);
        }
    }
}
