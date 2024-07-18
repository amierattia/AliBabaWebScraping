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
    public partial class smartWatch : ObservableObject
    {
        //requedt orequedt = new requedt();
        //[RelayCommand]
        //void getData()
        //{
        //   orequedt.ViewDataAsync(
        //}



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
            string? url = "https://arabic.alibaba.com/trade/search?spm=a27aq.cp_44.4746171840.7.501a3ccfEDazyn&categoryId=201936701&SearchText=LED+%D9%88+LCD+%D8%AA%D9%84%D9%81%D8%B2%D9%8A%D9%88%D9%86%D8%A7%D8%AA&indexArea=product_en&fsb=y&productId=1600913351788";

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
