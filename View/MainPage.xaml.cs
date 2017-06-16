using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF.AplicativoFIAP.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            App.ProfessorVM.Carregar();
            base.OnAppearing();
        }

        private void OnNovo(object sender, EventArgs e)
        {
            App.ProfessorVM.Selecionado = new Model.Professor();
            Navigation.PushAsync(new NovoProfessor() { BindingContext = App.ProfessorVM });
        }

        private void OnEditar(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NovoProfessor() { BindingContext = App.ProfessorVM });
        }

        private void OnSair(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}