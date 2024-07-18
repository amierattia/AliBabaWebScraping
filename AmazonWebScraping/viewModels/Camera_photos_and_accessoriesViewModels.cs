using AmazonWebScraping.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace AmazonWebScraping.viewModels
{
    public partial class Camera_photos_and_accessoriesViewModels : ObservableObject
    {
        [ObservableProperty]
        private string? image;

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string? price;

        public ObservableCollection<Camera_photos_and_accessoriesModels> DisplayData { get; set; } = new ObservableCollection<Camera_photos_and_accessoriesModels>();

        [RelayCommand]
        public async Task ViewDataAsync()
        {
            IsBusy = true;
            string url = "https://sale.alibaba.com/category/theme/index.html?spm=a27aq.cp_44.8496621700.5.501a3ccfAqQsIE&wx_navbar_transparent=true&path=/category/theme/index.html&ncms_spm=a27aq.27059075&prefetchKey=met&cardType=3708203&cardId=3710202&topOfferIds=1600839762999&categoryIds=44&tracelog=fy23_all_categories_home_lp";
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response);

                var productNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'hugo4-product-picture')]");
                var getName = htmlDocument.DocumentNode.SelectNodes("//div[@class='hugo4-product-wrap-margin wrap-margin-left wrap-margin-pc hugo3-f']");
                if (productNodes != null && getName != null && productNodes.Count == getName.Count)
                {
                    for (int i = 0; i < productNodes.Count; i++)
                    {
                        var productNode = productNodes[i];
                        var nameNode = getName[i];

                        var imageNode = productNode.SelectSingleNode(".//img");
                        var imageUrl = imageNode?.GetAttributeValue("src", string.Empty) ?? string.Empty;

                        var name = nameNode?.InnerText.Trim() ?? string.Empty;

                        Camera_photos_and_accessoriesModels oCamera_photos_and_accessoriesModels = new Camera_photos_and_accessoriesModels
                        {
                            Image = imageUrl.StartsWith("https:") ? imageUrl : "https:" + imageUrl,
                            Name = name,
                        };

                        DisplayData.Add(oCamera_photos_and_accessoriesModels);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
