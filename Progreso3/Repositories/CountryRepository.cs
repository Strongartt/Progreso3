using Newtonsoft.Json;
using Progreso3.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progreso3.Repositorios
{
    public class CountryRepository
    {
        private readonly string _dbPath;
        private SQLiteConnection conn;

        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Country>();
        }

        public CountryRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public List<Country> GetSavedCountries()
        {
            Init();
            return conn.Table<Country>().ToList();
        }

        public void SaveCountry(Country country)
        {
            Init();
            conn.Insert(country);
        }

        public async Task<List<Country>> GetCountriesFromApi()
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("https://restcountries.com/v3.1/all");
            var countries = JsonConvert.DeserializeObject<List<Country>>(response);
            return countries;
        }
    }
}