using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiApp1.Service
{
    internal class FavoriteService
    {
        public const string key = "favorites";
        public static List<string> GetAll()
        {
            if (!Preferences.ContainsKey(key))
                return new List<string>();
            string json = Preferences.Get(key, "[]");
            return JsonSerializer.Deserialize<List<string>>(json) ?? new();

        }
        public static void SaveAll(List<string> favorites)
        {
            string json = JsonSerializer.Serialize(favorites);
            Preferences.Set(key, json);
        }
        public static void Add(string city)
        {
            var list = GetAll();
            if (!list.Contains(city))
            {
                list.Add(city);
                SaveAll(list);
            }
        }
        public static void Remove(string city)
        {
            var list = GetAll();
            if (list.Contains(city))
            {
                list.Remove(city);
                SaveAll(list);
            }
        }
    }
}
