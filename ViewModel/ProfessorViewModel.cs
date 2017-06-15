using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XF.AplicativoFIAP.Model;

namespace XF.AplicativoFIAP.ViewModel
{
    public class ProfessorViewModel : INotifyPropertyChanged
    {
        #region Propriedades
        public string Nome { get { return App.ProfessorVM.Nome; } }

        private Professor selecionado;
        public Professor Selecionado
        {
            get { return selecionado; }
            set
            {
                selecionado = value as Professor;
                OnDeleteProfessorCMD.DeleteCanExecuteChanged();
                EventPropertyChanged();
            }
        }

        private string pesquisaPorNome;
        public string PesquisaPorNome
        {
            get { return pesquisaPorNome; }
            set
            {
                if (value == pesquisaPorNome) return;

                pesquisaPorNome = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PesquisaPorNome)));
                AplicarFiltro();
            }
        }

        public List<Professor> CopiaListaProfessor;
        public ObservableCollection<Professor> Professores { get; set; } = new ObservableCollection<Professor>();

        // UI Events

        public OnDeleteProfessorCMD OnDeleteProfessorCMD { get; }

        #endregion

        public void Carregar()
        {
            CopiaListaProfessor = ProfessorRepository.GetProfessoresSqlAzureAsync().Result;
            AplicarFiltro();
        }

        private void AplicarFiltro()
        {
            if (pesquisaPorNome == null)
                pesquisaPorNome = "";

            var resultado = CopiaListaProfessor.Where(n => n.Nome.ToLowerInvariant()
                                .Contains(PesquisaPorNome.ToLowerInvariant().Trim())).ToList();

            var removerDaLista = Professores.Except(resultado).ToList();
            foreach (var item in removerDaLista)
            {
                Professores.Remove(item);
            }

            for (int index = 0; index < resultado.Count; index++)
            {
                var item = resultado[index];
                if (index + 1 > Professores.Count || !Professores[index].Equals(item))
                    Professores.Insert(index, item);
            }
        }

        public async void Remover()
        {
            if (await App.Current.MainPage.DisplayAlert("Atenção?",
                string.Format("Tem certeza que deseja remover o {0}?", Selecionado.Nome), "Sim", "Não"))
            {
                string idstr = Selecionado.Id.ToString();
                
                if (await ProfessorRepository.DeleteProfessorSqlAzureAsync(idstr))
                {
                    CopiaListaProfessor.Remove(Selecionado);
                    AplicarFiltro();
                }
                else await App.Current.MainPage.DisplayAlert("Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class OnDeleteProfessorCMD : ICommand
    {
        private ProfessorViewModel professorVM;
        public OnDeleteProfessorCMD(ProfessorViewModel paramVM)
        {
            professorVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => professorVM.Selecionado != null;
        public void Execute(object parameter)
        {
            professorVM.Remover();
        }
    }

}
