using System.Collections.Generic;
using System.IO;

namespace Networking.Serialization
{
    public class INIConfig
    {
        private Dictionary<string, string> configs;

        public INIConfig(string data)
        {
            configs = new Dictionary<string, string>();
            ReadConfigs(data);
        }

        public void ReadConfigs(string data)
        {
            // Read all lines from the file
            string[] lines = data.Split('\n');

            // Store each line as key-value pair in the dictionary
            foreach (string line in lines)
            {
                if (line.StartsWith("#") || line.Trim().Length == 0)
                    continue;

                string[] parts = line.Split('=');
                configs[parts[0].Trim()] = parts[1].Trim();
            }
        }

        public string WriteConfigs()
        {
            // Create a list of strings
            List<string> lines = new List<string>();

            // Write each key-value pair as a line
            foreach (KeyValuePair<string, string> config in configs)
                lines.Add(config.Key + "=" + config.Value);

            // Join the lines into a single string
            string data = string.Join("\n", lines);

            // Return the string
            return data;
        }

        public string this[string key]
        {
            get
            {
                if (!configs.ContainsKey(key))
                    return null;
                return configs[key];
            }
            set
            {
                configs[key] = value;
                WriteConfigs();
            }
        }
    }
}
