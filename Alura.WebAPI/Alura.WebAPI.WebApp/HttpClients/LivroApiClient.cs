﻿using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.WebAPI.WebApp.HttpClients
{
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AuthApiClient _auth;

        public LivroApiClient(HttpClient httpClient, AuthApiClient auth)
        {
            _httpClient = httpClient;
            _auth = auth;
       
        }

        public async Task<Lista> GetListaLeituraAsync(TipoListaLeitura tipo)
        {
            var token = await _auth.PostLoginAssync(new LoginModel { Login = "admin", Password = "123" });
            
            _httpClient
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Bearer",token);
            
                    
            var resposta = await _httpClient.GetAsync($"listasleitura/{tipo}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<Lista>();

        }

        public async Task DeleteLivroAsync (int id)
        {
            var resposta = await _httpClient.DeleteAsync($"livros/{id}");
            resposta.EnsureSuccessStatusCode();

        }

        public async Task<byte[]> GetCapaLivroAsync(int id)
        {

            var token = await _auth.PostLoginAssync(new LoginModel { Login = "admin", Password = "123" });

            _httpClient
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Bearer", token);

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


        private string EnvolveComAspasDuplas(string valor)
        {
            return $"\"{valor}\"";
        }

        private HttpContent CreateMultipartFormDataContent(LivroUpload model)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.Titulo), EnvolveComAspasDuplas("titulo"));
            content.Add(new StringContent(model.Lista.ParaString()), EnvolveComAspasDuplas("lista"));

            if(!string.IsNullOrEmpty(model.Subtitulo))
            {
                content.Add(new StringContent(model.Subtitulo), EnvolveComAspasDuplas("subtitulo"));
            }

            if (!string.IsNullOrEmpty(model.Resumo))
            {
                content.Add(new StringContent(model.Resumo), EnvolveComAspasDuplas("resumo"));

            }

            if(!string.IsNullOrEmpty(model.Autor))
            {
                content.Add(new StringContent(model.Autor), EnvolveComAspasDuplas("autor"));

            }

            if (model.Id > 0)
            {
                content.Add(new StringContent(model.Id.ToString()), EnvolveComAspasDuplas("id"));
            }
            
            if(model.Capa != null)
            {
                var imagemContent = new ByteArrayContent(model.Capa.ConvertToBytes());
                imagemContent.Headers.Add("content-type", "image/png");
                content.Add(
                    imagemContent, 
                    EnvolveComAspasDuplas("capa"),
                    EnvolveComAspasDuplas("capa.png")              
              );
            }



            return content;
        }

        public async Task PostLivroAsync(LivroUpload model)
        {
            HttpContent content = CreateMultipartFormDataContent(model);
            var resposta = await _httpClient.PostAsync("livros", content);
            resposta.EnsureSuccessStatusCode();
        }

        public async Task PutLivroAsync(LivroUpload model)
        {
            HttpContent content = CreateMultipartFormDataContent(model);
            var resposta = await _httpClient.PutAsync("livros", content);
            resposta.EnsureSuccessStatusCode();

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
