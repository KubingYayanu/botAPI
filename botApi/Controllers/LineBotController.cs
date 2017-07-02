using System;
using System.Collections.Generic;
using System.Web.Http;
using botApi.Models;

namespace botApi.Controllers
{
    public class LineBotController : ApiController
    {
        BotRepository db = new BotRepository();
              
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public IHttpActionResult Post() 
        {
            try
            {
                string ChannelAccessToken = db.GetBotToken("Line");
                string postData = Request.Content.ReadAsStringAsync().Result;
                var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);
                string Message = "你說了:" + ReceivedMessage.events[0].message.text;
                isRock.LineBot.Utility.ReplyMessage(ReceivedMessage.events[0].replyToken, Message, ChannelAccessToken);
                return Ok();
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("ErrorMessage:{0},ErrorStack:{1},InnerExcepton:",
                    ex.Message,
                    ex.StackTrace,
                    ex.InnerException);
                db.InsertRequestLog(errorMessage);
                return Ok(errorMessage);
            }
        }
    }
}
