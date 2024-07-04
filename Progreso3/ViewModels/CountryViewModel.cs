using Progreso3.Models;
using Progreso3.Repositorios;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Progreso3.ViewModels
{
    public class CountryViewModel : INotifyPropertyChanged
    {
        private readonly CountryRepository _countryRepository;

        public ObservableCollection<Country> Countries { get; set; }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetCountriesCommand { get; }
        public ICommand ShowFavoriteCharacterCommand { get; }

        public CountryViewModel(string dbPath)
        {
            _countryRepository = new CountryRepository(dbPath);
            Countries = new ObservableCollection<Country>();
            GetCountriesCommand = new Command(async () => await GetCountries());
            ShowFavoriteCharacterCommand = new Command(async () => await ShowFavoriteCharacter());
            LoadCountries();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task ShowFavoriteCharacter()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Views.FavoriteCharacterView());
        }

        private async Task GetCountries()
        {
            try
            {
                var countriesFromApi = await _countryRepository.GetCountriesFromApi();
                foreach (var country in countriesFromApi)
                {
                    if (!_countryRepository.CountryExists(country.Codigo))
                    {
                        _countryRepository.SaveCountry(country);
                    }
                }
                LoadCountries();
            }
            catch (Exception ex)
            {
                Message = $"Error retrieving countries: {ex.Message}";
            }
        }

        private void LoadCountries()
        {
            var savedCountries = _countryRepository.GetSavedCountries();
            Countries.Clear();
            foreach (var country in savedCountries)
            {
                Countries.Add(country);
            }
        }
    }
}
