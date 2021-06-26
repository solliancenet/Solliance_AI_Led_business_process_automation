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
        public List<Claim> Claims { get; set; }

        [BindProperty]
        public List<FacetResult> Facets { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CategoryFilter { get; set; }

        public async Task OnGetAsync()
        {
            Transcriptions = new List<Transcription>();
            Claims = new List<Claim>();

            var azureSearchKey = Configuration["AzureSearchKey"];
            var azureSearchUrl = Configuration["AzureSearchUrl"];
            var claimsSearchIndexName = "claims-index";
            var audioTranscriptsSearchIndexName = "audio-index";

            AzureKeyCredential credential = new(azureSearchKey);

            SearchClient audioTranscriptsSearchclient = new(new Uri(azureSearchUrl), audioTranscriptsSearchIndexName, credential);

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

            //Searching for transcripts
            response = audioTranscriptsSearchclient.Search<Transcription>(indexSearch, options);
            await foreach (SearchResult<Transcription> result in response.GetResultsAsync())
            {
                Transcriptions.Add(result.Document);
            }

            //Searching for Claims Documents
            if (!string.IsNullOrEmpty(SearchString))
            {
                SearchClient claimsSearchclient = new(new Uri(azureSearchUrl), claimsSearchIndexName, credential);
                SearchOptions claimsSearchOptions;
                SearchResults<Claim> claimsSearchResponse;

                claimsSearchOptions = new SearchOptions()
                {
                    IncludeTotalCount = true,
                    Filter = "",
                    OrderBy = { "" }
                };

                claimsSearchOptions.Select.Add("PatientName");
                claimsSearchOptions.Select.Add("InsuredID");
                claimsSearchOptions.Select.Add("PatientBirthDate");
                claimsSearchOptions.Select.Add("DocumentDate");
                claimsSearchOptions.Select.Add("Diagnosis");
                claimsSearchOptions.Select.Add("FileName");

                claimsSearchResponse = claimsSearchclient.Search<Claim>(indexSearch, claimsSearchOptions);
                await foreach (SearchResult<Claim> result in claimsSearchResponse.GetResultsAsync())
                {
                    Claims.Add(result.Document);
                }
                foreach (Claim currentClaim in Claims)
                {
                    currentClaim.FileName = $"{Configuration["ContosoClaimsStorageContainerEndpoint"]}/{currentClaim.FileName}?{Configuration["ContosoClaimsStorageContainerSAS"]}";
                }
            }

            //Highlight keywords in HTML
            foreach (Transcription transcription in Transcriptions)
            {
                transcription.HTML = transcription.TranscribedText;
                transcription.AudioFileUrl = $"{Configuration["ContosoAudioStorageContainerEndpoint"]}/{transcription.FileName}?{Configuration["ContosoAudioStorageContainerSAS"]}" ;

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
