using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using botApi.Models;
using isRock.LineBot;
using System.Web.Http;
using System.Threading.Tasks;

namespace botApi.Controllers
{
    public class LineBotWithTemplateMessageController : ApiController
    {
        BotRepository db = new BotRepository();
        public async Task<IHttpActionResult> Post()
        {
            try
            {
                //Get Message
                string postData = Request.Content.ReadAsStringAsync().Result;
                var RequestBody = isRock.LineBot.Utility.Parsing(postData);
                string Message = RequestBody.events[0].message.text;
                string ChannelAccessToken = db.GetBotToken("Line");
                //取得LUIS的相關設定值
                NLPInfo LUISInfo = db.GetNLPInfo("LineBotNLP");

                //Send to Analysis
                Microsoft.Cognitive.LUIS.LuisClient lc
                    = new Microsoft.Cognitive.LUIS.LuisClient(LUISInfo.appid, LUISInfo.appKey, true);
                var AnalysisResult = await lc.Predict(Message);

                string replyMessage;
                //Get Reply Message
                bool isGreeting = db.GetResult(AnalysisResult, out replyMessage);
              
                if (isGreeting)
                {
                    CarouselTemplate ct = LineRepository.GetButtonTemplateMessage("c") as CarouselTemplate;
                    var result = Utility.PushTemplateMessage(RequestBody.events[0].source.userId
                                                            , ct
                                                            , ChannelAccessToken);
                }
                else
                {
                    //Response Message
                    isRock.LineBot.Utility.ReplyMessage(RequestBody.events[0].replyToken, replyMessage, ChannelAccessToken);
                }
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