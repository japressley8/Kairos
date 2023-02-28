using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;

namespace Kairos
{
    public class FileMan
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        public static void Save(Module module)
        {
            if (module != null)
            {
                string path = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings.Settings["ModuleDir"].Value;
                string fullPath = path + "\\module" + module.ID + ".kairos";
                while (File.Exists(fullPath))
                {
                    module.ID++;
                    fullPath = path + "\\module" + module.ID + ".kairos";
                }

                try
                {
                    string json = JsonConvert.SerializeObject(module, settings);
                    File.WriteAllText(fullPath, json);
                }
                catch
                {
                    MessageBox.Show("Error saving module to .kairos file: " + fullPath);
                }
            }
        }
        public static void Update(Module module)
        {
            string path = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings.Settings["ModuleDir"].Value;
            string fullPath = path + "\\module" + module.ID + ".kairos";

            try
            {
                string json = JsonConvert.SerializeObject(module, settings);
                File.WriteAllText(fullPath, json);
            }
            catch
            {
                MessageBox.Show("Error saving module to .kairos file: " + fullPath);
            }
        }
        public static List<Module> Load()
        {
            string path = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings.Settings["ModuleDir"].Value;
            List<Module> modules = new List<Module>();
            string[] files = Directory.GetFiles(path, "*.kairos", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                using (StreamReader r = new StreamReader(file))
                {
                    try
                    {
                        string json = r.ReadToEnd();
                        modules.Add(JsonConvert.DeserializeObject<Module>(json, settings));
                    }
                    catch
                    {
                        MessageBox.Show("Error reading .kairos file: " + file);
                    }
                }
            }
            return modules;
        }
        public static Module Load(string path)
        {
            Module module = new Module();
                using (StreamReader r = new StreamReader(path))
                {
                    try
                    {
                        string json = r.ReadToEnd();
                        return JsonConvert.DeserializeObject<Module>(json, settings);
                    }
                    catch
                    {
                        MessageBox.Show("Error reading .kairos file: " + path);
                    }
                }
            return new Module();
        }
        public static void Delete(Module module)
        {
            //delete .kairos file
            string path = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings.Settings["ModuleDir"].Value;
            string fullPath = path + "\\module" + module.ID + ".kairos";
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
