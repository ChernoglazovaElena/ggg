using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Newtonsoft.Json;

namespace WpfApp1
{
    internal class DSrealize
    {
        private static string _Path;
        public static void Serialize<T>(T list) /*where T: List<T>*/
        {

            File.WriteAllText(_Path, JsonConvert.SerializeObject(list));
        }
        public static T Deserialize<T>()
        {
            T Zametki = JsonConvert.DeserializeObject<T>(File.ReadAllText(_Path));
            return Zametki;
        }
        public static void SearchJsonFile()
        {
            string Path = Environment.CurrentDirectory;
            for(int i = Path.Length-1; i >= 0; i--)
            {
                if (Path[i].ToString() != @"\") Path = Path.Remove(i);
                else
                {
                    foreach (var element in Directory.GetDirectories(Path))
                    {
                        if(element == Path + "Json")
                        {
                            _Path = Path + "Json\\Zametka.json"; 
                            break;
                        }
                    }
                    if(_Path != null)
                    {
                        break;
                    }
                }
            }
        }
    }
}
