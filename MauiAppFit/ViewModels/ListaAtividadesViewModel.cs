using MauiAppFit.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiAppFit.ViewModels
{
    internal class ListaAtividadesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * Pegando oq foi digitado pelo usuario
         */

        public string ParametroBusca { get; set; }


        /**
         * Gerencia se mostra ao usuario o RefreshView
         */

        bool estaAtualizando = false;

        public bool EstaAtualizando
        {
            get => estaAtualizando;
            set
            {
                estaAtualizando = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Está Atualizando"));

            }
        }
        /**
         * Coleção que armazena as atividades do usuario
         */

        ObservableCollection<Atividade> listaAtividades = new ObservableCollection<Atividade>();

        public ObservableCollection<Atividade> ListaAtividades
        {
            get => listaAtividades;
            set => listaAtividades = value;
        }

        public ICommand AtualizarLista
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando) return;
                        EstaAtualizando = true;

                        List<Atividade> tmp = await App.Database.GetAllRows();
                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Ops", ex.Message, "Ok");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }//Fecha get
        }//Fecha AtualizarLista

        public ICommand Buscar
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando) return;
                        EstaAtualizando = true;

                        List<Atividade> tmp = await App.Database.Search(ParametroBusca);
                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Ops", ex.Message, "Ok");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }//Fecha o get
        }//Fecha Icommand Buscar

        public ICommand AbrirDetalhesAtividade
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    await Shell.Current.GoToAsync($"//CadastroAtividade?parametro_id={id}");
                });
            }//fecha get
        }//fecha ICommand AbrirDetalhes

        public ICommand Remover
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    try
                    {
                        bool conf = await Shell.Current.DisplayAlert("Tem certeza?", "Excluir", "Sim", "Não");

                        if (conf)
                        {
                            await App.Database.Delete(id);
                            AtualizarLista.Execute(null);
                        }
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Ops", ex.Message, "Ok");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }
    }
}
