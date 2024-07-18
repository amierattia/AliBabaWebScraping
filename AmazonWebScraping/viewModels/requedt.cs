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

namespace AmazonWebScraping.viewModels
{
    public partial class requedt : ObservableObject
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


        public async Task ViewDataAsync(string? url)
        {
            IsBusy = true;

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response);

                var productNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'ads-creative-img-mask-container')]");
                var getName = htmlDocument.DocumentNode.SelectNodes("//div[@class='card-info gallery-card-layout-info']");
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
