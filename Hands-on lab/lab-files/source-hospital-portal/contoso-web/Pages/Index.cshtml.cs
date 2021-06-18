using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using contoso_web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace contoso_web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration Configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        [BindProperty]
        public List<Transcription> Transcriptions { get; set; }

        [BindProperty]
        public List<FacetResult> Facets { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CategoryFilter { get; set; }

        public async Task OnGetAsync()
        {
            Transcriptions = new List<Transcription>();

            var azureSearchKey = Configuration["AzureSearchKey"];
            var azureSearchUrl = Configuration["AzureSearchUrl"];
            var searchIndexName = "cosmosdb-index2";

            AzureKeyCredential credential = new(azureSearchKey);

            SearchClient searchclient = new(new Uri(azureSearchUrl), searchIndexName, credential);

            string indexSearch = "*";
            if (!string.IsNullOrEmpty(SearchString))
            {
                indexSearch = SearchString;
            }

            string indexFilter = "";
            if (!string.IsNullOrEmpty(CategoryFilter))
            {
                indexFilter = $"HealthcareEntities/any(t: t/Category eq '{CategoryFilter}')";
            }

            SearchOptions options;
            SearchResults<Transcription> response;

            options = new SearchOptions()
            {
                IncludeTotalCount = true,
                Filter = indexFilter,
                OrderBy = { "" }
            };

            options.Select.Add("TranscribedText");
            options.Select.Add("FileName");
            options.Select.Add("DocumentDate");
            options.Select.Add("HealthcareEntities");
            options.Facets.Add("HealthcareEntities/Category,count:1000");

            response = searchclient.Search<Transcription>(indexSearch, options);
            await foreach (SearchResult<Transcription> result in response.GetResultsAsync())
            {
                Transcriptions.Add(result.Document);
            }

            //Highlight keywords in HTML
            foreach (Transcription transcription in Transcriptions)
            {
                transcription.HTML = transcription.TranscribedText;
                transcription.AudioFileUrl = $"{Configuration["ContosoStorageContainerEndpoint"]}/{transcription.FileName}?{Configuration["ContosoStorageContainerSAS"]}" ;

                if (!string.IsNullOrEmpty(SearchString))
                {
                    var uniqueKeywords = (from inc in transcription.HealthcareEntities select inc.Text).Distinct();

                    foreach (var healthcareKeywords in uniqueKeywords)
                    {
                        var category = (from inc in transcription.HealthcareEntities where inc.Text == healthcareKeywords select inc.Category).FirstOrDefault();
                        transcription.HTML = transcription.HTML.Replace(healthcareKeywords,
                        $"<span class='badge badge-success badge - pill' data-toggle='tooltip' data-placement='top' title='{category}'>{@healthcareKeywords}</span>");
                    }
                }
                else if (!string.IsNullOrEmpty(CategoryFilter))
                {
                    var uniqueEntities = (from inc in transcription.HealthcareEntities where inc.Category == CategoryFilter select inc).Distinct();

                    foreach (var healthcareEntity in uniqueEntities)
                    {
                        transcription.HTML = transcription.HTML.Replace(healthcareEntity.Text,
                        $"<span class='badge badge-success badge - pill' data-toggle='tooltip' data-placement='top' title='{healthcareEntity.Category}'>{healthcareEntity.Text}</span>");
                    }
                }

            }

            Facets = response.Facets.FirstOrDefault().Value.ToList();
        }
    }
}
