﻿using TrainingMVVM.DTOs;
using System.Net.Http.Json;

namespace TrainingMVVM.Models
{
    public class FetchDataModel
    {
        private readonly HttpClient _httpClient;
        public FetchDataModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<WeatherForecast[]> RetrieveForecast()
        {
            return await _httpClient.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
        }
    }
}
