using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    public interface ICacheManager
    {
        bool AddSet(string key, string value);
        bool AddString(string key, string value);
        bool AddString(string key, string value, TimeSpan expirey);
        bool AddObject<T>(string key, T value) where T : class;
        bool AddObject<T>(string key, T value, TimeSpan expirey) where T : class;
        bool Delete(string key);
        string GetString(string key);
        T GetObject<T>(string key) where T : class;
    }
}
