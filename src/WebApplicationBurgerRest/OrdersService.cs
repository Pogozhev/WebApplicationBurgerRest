
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;


namespace WebApplicationBurgerRest
{
    public class OrdersService
    {
        public async Task SendMessage(string nameParameter, string text)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new System.Uri("http://193.111.63.216:8888/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var parameters = new System.Collections.Generic.Dictionary<string, string>();
                parameters["order"] = text;
                var response = await client.PostAsync("came_order", new FormUrlEncodedContent(parameters));
                //Task<HttpResponseMessage> response = client.PostAsync("came_order",
                //  new FormUrlEncodedContent(parameters));

                var contents = await response.Content.ReadAsStringAsync();

                
                //var created = await response.Content.ReadAsAsync<Post>();
            }
            //await table.ExecuteAsync(insertOperation);
        }
    }
}
