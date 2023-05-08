using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using MvcCubosExamenEnriqueGPB.Models;

namespace MvcCubosExamenEnriqueGPB.Services
{
    public class ServiceCubos
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiCubos;
        public ServiceCubos(IConfiguration configuration)
        {
            this.UrlApiCubos =
                configuration.GetValue<string>("ApiUrls:ApiOAuthCubos");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");

        }

        public async Task<UsuarioCubo> GetPerfilUsuarioAsync(string token)
        {
            string request = "/api/cubos/getperfilusuario";
            UsuarioCubo usuario =
                await this.CallApiAsync<UsuarioCubo>(request, token);
            return usuario;
        }

        //LO PRIMERO DE TODO ES RECUPERAR NUESTRO TOKEN LA APP MVC
        public async Task<string> GetTokenAsync(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/auth/login";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    Email = email,
                    Password = password
                };
                string jsonModel = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(jsonModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data =
                        await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(data);
                    string token =
                        jsonObject.GetValue("response").ToString();
                    return token;

                }
                else
                {
                    return null;
                }

            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Cubo>> GetCubosMarcaAsync(string marca)
        {
            string request = "/api/cubos/getcubos/" + marca;
            List<Cubo> cubos =
                await this.CallApiAsync<List<Cubo>>(request);
            return cubos;
        }
        public async Task<List<Cubo>> GetCubosAsync()
        {
            string request = "/api/cubos";
            List<Cubo> cubos =
                await this.CallApiAsync<List<Cubo>>(request);
            return cubos;
        }

        public async Task NewCuboAsync(Cubo cubo, string token)
        {

            using (HttpClient client = new HttpClient())
            {
                string request = "/api/cubos";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
              ("Authorization", "bearer " + token);


                Cubo cubonuevo = new Cubo
                {
                    IdCubo = cubo.IdCubo,
                    Nonbre = cubo.Nonbre,
                    Marca = cubo.Marca,
                    Imagen = cubo.Imagen,
                    Precio = cubo.Precio
                };
                string jsonCubo =
                    JsonConvert.SerializeObject(cubonuevo);
                StringContent content =
                    new StringContent(jsonCubo, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);

            }
        }

        public async Task<List<CuboCompra>> GetPedidosAsync(string token)
        {
            string request = "/api/cubos/getpedidosusuario";
            List<CuboCompra> pedidos =
                await this.CallApiAsync<List<CuboCompra>>(request, token);
            return pedidos;
        }

    }
}
