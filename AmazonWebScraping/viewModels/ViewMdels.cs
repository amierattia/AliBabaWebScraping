using System.Collections.ObjectModel;
using AmazonWebScraping.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace AmazonWebScraping.viewModels
{
    public partial class ViewModelsView : ObservableObject
    {
        [ObservableProperty]
        private string? image;

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string? price;

        public ObservableCollection<clsModels> DisplayData { get; set; } = new ObservableCollection<clsModels>();

        [RelayCommand]
        public async Task ViewDataAsync()
        {
            IsBusy = true;
            string url = "https://sale.alibaba.com/category/half_trust_lp/index.html?spm=a27aq.cp_1420.8851329790.2.5f4c2af2kJoPxj&wx_navbar_transparent=true&path=/category/half_trust_lp/index.html&topOfferIds=1601020516452&cardType=101002784&cardId=1420&categoryIds=1420";

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

                        clsModels oclsmodels = new clsModels
                        {
                            Image = imageUrl.StartsWith("https:") ? imageUrl : "https:" + imageUrl,
                            CatigoryName = name,
                        };

                        DisplayData.Add(oclsmodels);
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
