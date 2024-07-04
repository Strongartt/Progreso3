using Newtonsoft.Json;
using Progreso3.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Progreso3.Repositories
{
    public class PaisesRepositories
    {
        public string _dbPath;
        private SQLiteConnection conn;

        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Pais>();
        }

        public PaisesRepositories(string dbPath)
        {
            _dbPath = dbPath;
        }

        public List<Pais> DevuelveListadoPaises()
        {
            Init();
            return conn.Table<Pais>().ToList();
        }

        public void GuardarPais(Pais pais)
        {
            Init();
            conn.Insert(pais);
        }

        public void ActualizarPais(Pais pais)
        {
            Init();
            conn.Update(pais);
        }

        public void EliminarPais(Pais pais)
        {
            Init();
            conn.Delete(pais);
        }

        public async Task<List<Pais>> ObtenerPaisesDesdeApiAsync()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("https://restcountries.com/v3.1/all");
            var paisesJson = JsonConvert.DeserializeObject<List<dynamic>>(response);

            var paises = new List<Pais>();
            var random = new Random();

            foreach (var paisJson in paisesJson)
            {
                var pais = new Pais
                {
                    Nombre = paisJson.name.official,
                    Region = paisJson.region,
                    Subregion = paisJson.subregion,
                    Status = paisJson.status,
                    Codigo = "RP" + random.Next(1000, 2000)
                };
                paises.Add(pais);
            }

            return paises;
        }
    }
}
