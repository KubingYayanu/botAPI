using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace botApi.Models
{
    public class BotRepository
    {
        BotEntities DB = new BotEntities();

        public  string GetBotToken(string BotName)
        {
            var BotToken = DB.BotData.AsQueryable()
                        .Where(o => o.BotName == BotName)
                        .Select(o => o.Token).FirstOrDefault();
            return BotToken;

        }
    }
}