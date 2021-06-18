using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
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

        public void OnGet()
        {
            var azureSearchKey = Configuration["AzureSearchKey"];
            var azureSearchUrl = Configuration["AzureSearchUrl"];
            var searchIndexName = "cosmosdb-index";

            AzureKeyCredential credential = new(azureSearchKey);

            SearchClient searchclient = new(new Uri(azureSearchUrl), searchIndexName, credential);

        }
    }
}
