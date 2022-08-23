using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NyTimesData;
using NyTimesServices.Contracts;
using NyTimesSharedModels.Models;
using System.Net.Http;
using static System.Collections.Specialized.BitVector32;

namespace NyTimesServices.Services
{
    public class NyTimesTopNewsServices : INyTimesTopNewsServices
    {
        private readonly NyTimesDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public NyTimesTopNewsServices(NyTimesDBContext context, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<Root> GetDataFromNyTimesAsync(string section)
        {
            var dbResult = await GetDataFromDBAsync(section);

            string baseUrl = string.Empty;
            string apiKey = _configuration.GetSection("NyTimesTopStoriesApi")["ApiKey"].ToString();

            switch (section.ToLower())
            {
                case "arts":
                    baseUrl = _configuration.GetSection("NyTimesTopStoriesApi")["ArtsURL"].ToString();
                    break;
                case "home":
                    baseUrl = _configuration.GetSection("NyTimesTopStoriesApi")["HomeURL"].ToString();
                    break;
                case "u.s. news":
                    baseUrl = _configuration.GetSection("NyTimesTopStoriesApi")["USURL"].ToString();
                    break;
                case "science":
                    baseUrl = _configuration.GetSection("NyTimesTopStoriesApi")["ScienceURL"].ToString();
                    break;
                case "world news":
                    baseUrl = _configuration.GetSection("NyTimesTopStoriesApi")["WorldURL"].ToString();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(baseUrl) && !string.IsNullOrWhiteSpace(apiKey))
            {
                string apiUrl = baseUrl + apiKey;

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                var apiResponse = await _httpClient.SendAsync(requestMessage);

                if (apiResponse.IsSuccessStatusCode)
                {
                    string responseString = await apiResponse.Content.ReadAsStringAsync();

                    var resultData = JsonConvert.DeserializeObject<Root>(responseString);

                    if(dbResult != null && dbResult.section == section && dbResult.last_updated != resultData.last_updated)
                    {
                        await SaveNyTimesDataSync(resultData);

                        return resultData;
                    }
                }
            }


            return dbResult;
        }

        public async Task SaveNyTimesDataSync(Root data)
        {
            if(data != null && await CheckDuplicateData(data))
            {

                using(var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var newRootData = new NyTimesData.Entity.Root
                        {
                            Id = 0,
                            copyright = data.copyright,
                            last_updated = data.last_updated,
                            num_results = data.num_results,
                            section = data.section,
                            status = data.status
                        };

                        await _context.AddAsync(newRootData);
                        await _context.SaveChangesAsync();

                        if(data.num_results > 0)
                        {
                            var newResulstData = data.results.Select(r => new NyTimesData.Entity.Result
                            {
                                Id = 0,
                                RootId = newRootData.Id,
                                @abstract = r.@abstract,
                                byline = r.byline,
                                created_date = r.created_date,
                                updated_date = r.updated_date,
                                kicker = r.kicker,
                                material_type_facet = r.material_type_facet,
                                item_type = r.item_type,
                                published_date = r.published_date,
                                section = r.section,
                                short_url = r.short_url,
                                subsection = r.subsection,
                                title = r.title,
                                uri = r.uri,
                                url = r.url,
                                des_facet = (r.des_facet != null && r.des_facet.Count > 0) ? r.des_facet.Aggregate((x , y) => x + "|" + y) : null,
                                geo_facet = (r.geo_facet != null && r.geo_facet.Count > 0) ? r.geo_facet.Aggregate((x, y) => x + "|" + y) : null,
                                org_facet = (r.org_facet != null && r.org_facet.Count > 0) ? r.org_facet.Aggregate((x, y) => x + "|" + y) : null,
                                per_facet = (r.per_facet != null && r.per_facet.Count > 0) ? r.per_facet.Aggregate((x, y) => x + "|" + y) : null,
                                multimedia = (r.multimedia != null && r.multimedia.Count > 0) ? r.multimedia.Select(m => new NyTimesData.Entity.Multimedium 
                                {
                                    copyright = m.copyright,
                                    caption = m.caption,
                                    format = m.format,
                                    height = m.height,
                                    subtype = m.subtype,
                                    type = m.type,
                                    url = m.url,
                                    width = m.width
                                }).ToList() : null
                            }).ToList();

                            await _context.AddRangeAsync(newResulstData);
                            await _context.SaveChangesAsync();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        private async Task<bool> CheckDuplicateData(Root data)
        {
            bool flag = true;

            var dbData = await _context.Roots.FirstOrDefaultAsync(r => r.section == data.section && r.last_updated == data.last_updated);

            if(dbData != null)
            {
                flag = false;
            }
            return flag;
        }

        private async Task<Root> GetDataFromDBAsync(string section)
        {
            var dbResult = await _context.Roots
                .Include(r => r.results).ThenInclude(res => res.multimedia)
                .Where(rt => rt.section == section)
                .OrderByDescending(rt => rt.last_updated)
                .FirstOrDefaultAsync();

            if(dbResult != null)
            {
                var rootResult = new Root
                {
                    Id = dbResult.Id,
                    copyright = dbResult.copyright,
                    last_updated = dbResult.last_updated,
                    num_results = dbResult.num_results,
                    section = dbResult.section,
                    status = dbResult.status,
                    results = dbResult.results.Count > 0 ? dbResult.results.Select(r => new Result
                    {
                        Id = r.Id,
                        RootId = r.RootId,
                        @abstract = r.@abstract,
                        byline = r.byline,
                        created_date = r.created_date,
                        item_type = r.item_type,
                        kicker = r.kicker,
                        material_type_facet = r.material_type_facet,
                        published_date = r.published_date,
                        section = r.section,
                        short_url = r.short_url,
                        subsection = r.subsection,
                        title = r.title,
                        updated_date = r.updated_date,
                        uri = r.uri,
                        url = r.url,
                        des_facet = (!string.IsNullOrWhiteSpace(r.des_facet) || r.des_facet != null) ? r.des_facet.Split('|').ToList() : null,
                        geo_facet = (!string.IsNullOrWhiteSpace(r.geo_facet) || r.geo_facet != null) ? r.geo_facet.Split('|').ToList() : null,
                        org_facet = (!string.IsNullOrWhiteSpace(r.org_facet) || r.org_facet != null) ? r.org_facet.Split('|').ToList() : null,
                        per_facet = (!string.IsNullOrWhiteSpace(r.per_facet) || r.per_facet != null) ? r.per_facet.Split('|').ToList() : null,
                        multimedia = (r.multimedia != null && r.multimedia.Count > 0) ? r.multimedia.Select(m => new Multimedium
                        {
                            copyright = m.copyright,
                            caption = m.caption,
                            format = m.format,
                            height = m.height,
                            subtype = m.subtype,
                            type = m.type,
                            url = m.url,
                            width = m.width
                        }).ToList() : null,
                    }).ToList() : null
                };

                return rootResult;
            }

            return null;
        }

    }
}
