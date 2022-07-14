using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Values
{
    public static class AzureInfo
    {
        public static string ConnectionString { get; } = @"DefaultEndpointsProtocol=https;AccountName=realestateessayaccount;AccountKey=QJ0YNEpl61PDWc/WIzQqDebWY4BlXfeVgQyAIpTtVL19EeQ1Mhk4Z8O8pGkzWawQn10rXrskDTdQ9Ak19nAhjQ==;EndpointSuffix=core.windows.net";
        public static string ContainerName { get; } = "realestatesimage";
    }
}
