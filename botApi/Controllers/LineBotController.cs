using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using botApi.Models;

namespace botApi.Controllers
{
    public class LineBotController : ApiController
    {
        BotRepository db = new BotRepository();
               // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        public string Get(int id)
        {
            return "value";
        }

        public HttpResponseMessage Get(string message)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            response.Content = new StringContent(message, Encoding.Unicode,"text/html");
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(20)
            };
            return response;
        }

        //// POST api/values
        //public string PostLineBot([FromBody]string value)
        //{
        //    return value;
        //}

        //[HttpPost]
        //[ActionName("PostMessage")]
        public IHttpActionResult Post() 
        {
            string ChannelAccessToken = db.GetBotToken("Line");

            try
            {
                //取得 http Post RawData(should be JSON)
                string postData = Request.Content.ReadAsStringAsync().Result;
                //剖析JSON
                var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);
                //回覆訊息
                string Message;
                Message = "你說了:" + ReceivedMessage.events[0].message.text;
                //回覆用戶
                isRock.LineBot.Utility.ReplyMessage(ReceivedMessage.events[0].replyToken, Message, ChannelAccessToken);
                //回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {



        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
