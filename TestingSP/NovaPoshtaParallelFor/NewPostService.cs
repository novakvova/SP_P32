using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NovaPoshtaParallelFor.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NovaPoshtaParallelFor
{
    public class NewPostService
    {
        private bool _disposedValue;
        private readonly HttpClient _httpClient = new();
        private readonly string _newPostKey = "5ff369737ba5b868086725fe0c5e1ff1";
        private readonly string _newPostUrl = "https://api.novaposhta.ua/v2.0/json/";

        private async Task<IEnumerable<T>> GetNewPostData<T>(string modelName,
            string calledMethod,
            int page = 1,
            int limit = 200,
            string areaRef = ""
            , string region = "",
            string settlementRef = "")
        {
            NewPostRequestModel postModel = new(_newPostKey, modelName, calledMethod, page, limit, areaRef, region, settlementRef);
            string json = JsonConvert.SerializeObject(postModel);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_newPostUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var requestResult = await response.Content.ReadAsStringAsync();
                if (requestResult is not null)
                {
                    requestResult = requestResult.Trim('[', ']');
                    var result = JsonConvert.DeserializeObject<NewPostResponseModel<T>>(requestResult);
                    if (result != null && result.Data.Length > 0)
                    {
                        return result.Data;
                    }
                }
                return [];
            }
            else
            {
                throw new Exception("New post data update error");
            }
        }

        public async Task<IEnumerable<Area>> GetAreasDataAsync() => await GetNewPostData<Area>("Address", "getSettlementAreas");

        public async Task<IEnumerable<Region>> GetRegionsDataAsync(IEnumerable<string> areaRefs)
        {
            List<Region> result = [];
            foreach (var areaRef in areaRefs)
            {
                var regions = await GetNewPostData<Region>("Address", "getSettlementCountryRegion", areaRef: areaRef);
                if (regions.Any())
                {
                    regions.AsParallel().ForAll(region => region.AreaRef = areaRef);
                    result.AddRange(regions);
                }
            }
            return result.AsParallel()
                .GroupBy(x => x.Ref)
                .Select(z => z.First());
        }

        public async Task<IEnumerable<Settlement>> GetSettlementsDataAsync(IEnumerable<Region> regions)
        {
            List<Settlement> settlements = [];
            int page = 1;
            while (true)
            {
                var result = await GetNewPostData<Settlement>("Address", "getSettlements", page++, 500);
                if (result.Any())
                {
                    Console.WriteLine("Read page {0}", page);
                    settlements.AddRange(result);
                }
                else
                {
                    break;
                }
            }
            ;

            settlements.AsParallel().ForAll(settlement =>
            {
                if (System.String.IsNullOrWhiteSpace(settlement.Region))
                {
                    settlement.Region = regions.FirstOrDefault(region => region.AreasCenter == settlement.Ref)?.Ref;
                }
            });

            return settlements.AsParallel()
                .GroupBy(x => x.Ref)
                .Select(z => z.First());
        }

    }
}
