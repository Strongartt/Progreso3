
using Microsoft.Maui.Controls;
using Progreso3.ViewModels;

namespace Progreso3.Views
{
    public partial class CountryView : ContentPage
    {
        public CountryView()
        {
            InitializeComponent();
            BindingContext = new CountryViewModel("countries.db3"); 
        }
    }
}
