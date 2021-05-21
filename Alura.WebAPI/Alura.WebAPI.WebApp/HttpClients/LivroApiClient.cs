﻿using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;

        public LivroApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
       
        }

        public async Task<byte[]> GetCapaLivroAsync(int id)
        {
           
            HttpResponseMessage resposta = await _httpClient.GetAsync($"livros/{id}/capa");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsByteArrayAsync();
        }

        public async Task<LivroApi> GetLivroAsync(int id)
        {
    
            HttpResponseMessage resposta = await _httpClient.GetAsync($"livros/{id}");
            resposta.EnsureSuccessStatusCode();

            return await resposta.Content.ReadAsAsync<LivroApi>();
        }

    }
}





//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Alura.ListaLeitura.Modelos;


//namespace Alura.WebAPI.WebApp.HttpClient
//{
//    public class LivroApiClient
//    {
//        public async Task<byte[]> GetCapaLivroAsync(int id)
//        {

//            HttpClient httpClient = new HttpClient();
//            httpClient.BaseAddress = new System.Uri("http://localhost:6000/api/");
//            HttpResponseMessage resposta = await httpClient.GetAsync($"livros/{id}/capa");
//            resposta.EnsureSuccessStatusCode();

//            return await resposta.Content.ReadAsByteArrayAsync();

//        }


//        public async Task<LivroApiClient> GetLivroAsync(int id)
//        {

//            HttpClient httpClient = new HttpClient();
//            httpClient.BaseAddress = new System.Uri("http://localhost:6000/api/");
//            HttpResponseMessage resposta = await httpClient.GetAsync($"livros/{id}/capa");
//            resposta.EnsureSuccessStatusCode();


//            return await resposta.Content.ReadAsAsync<LivroApi>();
//        }
//    }
//}
