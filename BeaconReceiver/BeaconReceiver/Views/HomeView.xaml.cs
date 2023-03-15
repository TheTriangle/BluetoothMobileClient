using System.Threading.Tasks;
using BeaconReceiver.ViewModels;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeaconReceiver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        private readonly HomeViewModel _viewModel;

        public HomeView()
        {
            InitializeComponent();

            _viewModel = new HomeViewModel();
            BindingContext = _viewModel;
        }

        public async Task Init()
        {
            await _viewModel.RequestPermissions();
        }
    }
}