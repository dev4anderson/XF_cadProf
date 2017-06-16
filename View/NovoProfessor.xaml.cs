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
    public partial class NovoProfessor : ContentPage
    {
        public NovoProfessor()
        {
            InitializeComponent();
        }

        public void OnCancelar(object sender, EventArgs args)
        {
            Navigation.PopAsync();
        }
    }
}