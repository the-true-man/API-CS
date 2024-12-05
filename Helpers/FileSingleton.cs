using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace registration.Classes
{
    public class FileSingleton
    {
        private static FileSingleton instance;
        public static string pathToFileWithUsers = "Data/allUsers.json";
        public static string pathToFileWithPizza = "PizzaProducts.json";
        public static string pathToFileWithBurgers = "BurgersProducts.json";
        public static string pathToFileWithDrinks = "DrinksProducts.json";

        private FileSingleton()
        { }

        public static FileSingleton getInstance()
        {
            if (instance == null) instance = new FileSingleton();
            return instance;
        }
        public static void serializeCollection<T>(string pathToFile, IEnumerable<T> collection)
        {
            if (!File.Exists(pathToFile))
            {
                File.Create(pathToFile);
            }
            string data = JsonConvert.SerializeObject(collection, Formatting.Indented);
            File.WriteAllText(pathToFile, data);
        }
        public static List<T> deserializeCoolection<T>(string pathToFile)
        {
            if (!File.Exists(pathToFile))
            {
                File.Create(pathToFile);
            }
            string data = File.ReadAllText(pathToFile);
            if(data.Length == 0)
            {
                return [];
            }
            return JsonConvert.DeserializeObject<List<T>>(data);
        }
    }
}
