using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Sadas_test.Models.DTOs;
using Sadas_test.Services.Interfaces;
using static Sadas_test.Exceptions.ServiceException;

namespace Sadas_test.Services.Implementations
{
    public class PriceSourcesService : IPriceSourcesService
    {
        private readonly HttpClient _httpClient;
        private readonly List<IApiSource> _sources = new();

        public PriceSourcesService(HttpClient httpClient, IOptions<List<ProductSourceDto>> config)
        {
            _httpClient = httpClient;

            foreach (var dto in config.Value)
            {
                _sources.Add(new PriceApiSource
                {
                    Name = dto.SourceName,
                    ApiUrl = dto.ApiUrl,
                    NumParameters = dto.NumParameters,
                    ParametersMagicStr = dto.ParametersMagicStr,
                });
            }
        }

        public Task<IEnumerable<IApiSource>> GetAllSourcesAsync()
        {
            return Task.FromResult<IEnumerable<IApiSource>>(_sources);
        }

        public Task<int> CountSourcesAsync()
        {
            return Task.FromResult(_sources.Count);
        }

        public async Task<(string SourceName, decimal Price)> CallAllSources(string ProductName)
        {
            var tasks = _sources.Select(async source =>
            {
                if (source.NumParameters != 1) {
                    throw new InvalidSourceConfigurationException("Only one parameter is supported at this time.");
                }
                string url = ReplaceFirst(source.ApiUrl, source.ParametersMagicStr, ProductName);

                try
                {
                    var response = await _httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode) return default;

                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement[0];

                    // try known paths (in real case should come from DTO)
                    decimal price = ExtractPrice(root);

                    return (source.Name, price);
                }
                catch
                {
                    return default;
                }
            });

            var allResults = await Task.WhenAll(tasks);
            var first = allResults.FirstOrDefault(r => r != default);

            return first == default ? ("None", -1m) : first;
        }

        private static decimal ExtractPrice(JsonElement root)
        {
            foreach (var property in root.EnumerateObject())
            {
                if (IsPriceField(property.Name))
                {
                    if (TryGetDecimal(property.Value, out var val))
                        return val;
                }

                // One-level nested check
                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    foreach (var subProperty in property.Value.EnumerateObject())
                    {
                        if (IsPriceField(subProperty.Name))
                        {
                            if (TryGetDecimal(subProperty.Value, out var subVal))
                                return subVal;
                        }
                    }
                }
            }

            throw new Exception("Price not found in any recognized fields.");
        }

        private static bool IsPriceField(string name)
        {
            return name.Equals("price", StringComparison.OrdinalIgnoreCase);
        }

        private static bool TryGetDecimal(JsonElement element, out decimal value)
        {
            value = 0;

            if (element.ValueKind == JsonValueKind.Number)
                return element.TryGetDecimal(out value);

            if (element.ValueKind == JsonValueKind.String &&
                decimal.TryParse(element.GetString(), out value))
                return true;

            return false;
        }

        public static string ReplaceFirst(string input, string magic, string replacement)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(magic))
                return input;

            string escapedMagic = Regex.Escape(magic);
            bool replaced = false;

            return Regex.Replace(input, escapedMagic, match =>
            {
                if (!replaced)
                {
                    replaced = true;
                    return replacement;
                }
                return match.Value;
            });
        }
    }
}
