using Progreso3.Models;
using Progreso3.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Progreso3.ViewModels
{
    public class PaisViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Pais> Paises { get; set; }
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

        public ICommand ObtenerPaisesCommand { get; }

        public PaisViewModel()
        {
            Paises = new ObservableCollection<Pais>();
            ObtenerPaisesCommand = new Command(async () => await ObtenerPaises());
            LoadItems();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task ObtenerPaises()
        {
            var paises = await App.PaisRepo.ObtenerPaisesDesdeApiAsync();
            foreach (var pais in paises)
            {
                App.PaisRepo.GuardarPais(pais);
                Paises.Add(pais);
            }
            Message = "Países guardados correctamente (" + Paises.Count + ")";
        }

        private void LoadItems()
        {
            Paises.Clear();
            var items = App.PaisRepo.DevuelveListadoPaises();
            foreach (var item in items)
            {
                Paises.Add(item);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
