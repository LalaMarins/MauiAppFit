using MauiAppFit.ViewModels;
namespace MauiAppFit.Views;

public partial class listaAtividades : ContentPage
{
	public listaAtividades()
	{
		BindingContext = new ListaAtividadesViewModel();

		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        var vm = (ListaAtividadesViewModel)BindingContext;
		vm.AtualizarLista.Execute(null);
    }
}