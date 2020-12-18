using System.Collections.Generic;
using System.Threading.Tasks;

namespace WichtelnWebapp.Server
{
    public interface ISqlController
    {
        string conStr_name { get; set; }

        Task<List<T>> LoadData<T, U>(string sql, U parameters);
        Task SaveData<T>(string sql, T parameters);
    }
}