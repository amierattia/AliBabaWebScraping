
using AmazonWebScraping.View;
using AmazonWebScraping.viewModels;
namespace AmazonWebScraping
{
    public partial class MainPage : ContentPage
    {
        public ViewModelsView oViewModelsView;
        public MainPage()
        {
            InitializeComponent();
           

        }
        private  void LoadImage()
        {
           
        }

        private async void Acc_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new accessoriesView());
        }

        private async void Game_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamesView());    
        }

        private async void Camera_photos_and_accessories_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new oCamera_photos_and_accessoriesView());
        }

        private async void smartWatch_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewSmartWatch());
        }
    }
}



