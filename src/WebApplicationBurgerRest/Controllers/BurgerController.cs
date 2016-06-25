using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace WebApplicationBurgerRest.Controllers
{
    [Route("api/[controller]")]
    public class BurgerController : Controller
    {
        public BurgerController(OrdersService orderService, CloudStorage cloudStorage)
        {
            OrderService = orderService;
            CloudStorage = cloudStorage;
        }

        [FromServices]
        private CloudStorage CloudStorage { get; }

        [FromServices]
        private OrdersService OrderService { get; }

        [HttpGet("Test/{message}")]
        [Produces(typeof(string))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(int))]
        public async Task<IActionResult> Test(string message)
        {
            var task = Task.Factory.StartNew(() =>
            {
                message += " + async test";
            }
                );
            await task.ConfigureAwait(false);
            return this.Ok(message);
        }
        [HttpPost("InsertMessage/{data}")]
        public async Task<IActionResult> InsertMessage(string data, [FromBody]MessageItem value)
        {
            try
            {
                if (value != null)
                {
                    await OrderService.SendMessage("order", data);
                    await CloudStorage.InsertMessage("chats", value);
                    var task = Task.Factory.StartNew(() =>
                    {

                        ///
                    }
                        );
                    await task.ConfigureAwait(false);
                }
                return this.Ok("success");
            }
            catch (Exception)
            {
                return this.HttpBadRequest("Fail!");
            }
        }

        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
