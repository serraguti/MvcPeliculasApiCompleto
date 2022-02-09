using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using MvcPeliculasApiCompleto.Models;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MvcPeliculasApiCompleto.Services
{
    public class ServiceApiPeliculas
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceApiPeliculas(string url)
        {
            this.UrlApi = url;
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }

        //METODO SIN SEGURIDAD
        private async Task<T> CallApi<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data =
                        await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> CallApi<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                client.DefaultRequestHeaders.Add("Authorization"
                    , "bearer " + token);

                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data =
                        await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<string> GetTokenAsync(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/auth/login";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel();
                model.UserName = username;
                model.Password = password;
                string json = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(this.UrlApi);
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data =
                        await response.Content.ReadAsStringAsync();
                    JObject jobject = JObject.Parse(data);
                    string token =
                        jobject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            string request = "/api/peliculas/generos";
            List<Genero> generos =
               await this.CallApi<List<Genero>>(request);
            return generos;
        }

        public async Task<List<Pelicula>> GetPeliculasGeneroAsync(int idgenero)
        {
            string request = "/api/peliculas/peliculasgenero/" + idgenero;
            List<Pelicula> peliculas =
                await this.CallApi<List<Pelicula>>(request);
            return peliculas;
        }

        public async Task<Pelicula> FindPeliculaAsync(int idpelicula)
        {
            string request = "/api/peliculas/" + idpelicula;
            Pelicula peli = await this.CallApi<Pelicula>(request);
            return peli;
        }

        public async Task<List<Pelicula>> GetCarritoPeliculasAsync
            (List<int> carrito)
        {
            string request = "/api/peliculas";
            List<Pelicula> peliculas =
                await this.CallApi<List<Pelicula>>(request);
            var consulta = from datos in peliculas
                           where carrito.Contains(datos.IdPelicula)
                           select datos;
            return consulta.ToList();
        }
    }
}
