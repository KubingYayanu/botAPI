using System;
using System.Web.Http;
using botApi.Models;
using System.Threading.Tasks;
namespace botApi.Controllers
{
    public class LineBotWithLUISController : ApiController
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

                //取得LUIS的相關設定值
                NLPInfo LUISInfo = db.GetNLPInfo("LineBotNLP");

                //Send to Analysis
                Microsoft.Cognitive.LUIS.LuisClient lc
                    = new Microsoft.Cognitive.LUIS.LuisClient(LUISInfo.appid, LUISInfo.appKey, true);
                var AnalysisResult = await lc.Predict(Message);

                //Get Reply Message
                string replyMessage = db.GetResult(AnalysisResult);

                //Response Message
                string ChannelAccessToken = db.GetBotToken("Line");
                isRock.LineBot.Utility.ReplyMessage(RequestBody.events[0].replyToken, replyMessage, ChannelAccessToken);
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
