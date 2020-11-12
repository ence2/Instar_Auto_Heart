using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;

namespace InstarAutoHeart
{
    public class Config : JsonConfig<Config>
    {
        public List<string> priorityTags = new List<string>();
        public bool hideChrome = true;

        public string ID = "";
        public string PW = "";

        public string currentSeleniumHandle = "";

        public List<string> alreadySerached = new List<string>();
        public Dictionary<string, int> dailyFollowCount = new Dictionary<string, int>();
        public Dictionary<string, int> dailyHeartCount = new Dictionary<string, int>();
        public Dictionary<string, int> dailyLoginCount = new Dictionary<string, int>();
    }

    public class JsonConfig<T> : Singleton<JsonConfig<T>> where T : new()
    {
        public T Data;
        private string _filename;

        public Queue<string> kors = new Queue<string>();

        public bool Load(string filename)
        {
            if (!File.Exists(filename))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                Data = new T();
                string content = Util.ToJson(Data);
                File.WriteAllText(filename, content);
            }

            TextReader r = File.OpenText(filename);
            JsonReader reader = new JsonTextReader(r);
            JsonSerializer serializer = new JsonSerializer();
            Data = serializer.Deserialize<T>(reader);
            _filename = filename;
            r.Close();

            using (StreamReader rr = new StreamReader(@"Data\\kor.csv"))
            {
                var line = rr.ReadToEnd();
                var list = line.Split('\t').ToList();
                list.Shuffle<string>();
                foreach (var item in list)
                {
                    var temp = item;
                    if (temp.Length == 0)
                        continue;

                    if (item.Length != 1)
                        temp = item[0].ToString();

                    if (item == "\\r")
                        continue;

                    Config.Instance.kors.Enqueue(temp);
                }
            }

            return true;
        }



        public void Save()
        {
            string content = Util.ToJson(Data);
            File.WriteAllText(_filename, content);
        }
    }

    public class Singleton<T>
    {
        protected static T instance_;

        static public T Instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = System.Activator.CreateInstance<T>();

                return instance_;

            }
        }
    }
}
