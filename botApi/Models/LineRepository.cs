using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using isRock.LineBot;
using System.Data;
using System.Data.SqlClient;

namespace botApi.Models
{
    public class LineRepository
    {
        static BotEntities db = new BotEntities();

        public static TemplateMessageBase GetButtonTemplateMessage(string type)
        {
            switch (type.ToLower())
            {
                case "confirms":
                    return GenConfirmsTemplate();
                case "button":
                    return GenButtonsTemplate();
                case "carousel":
                    return GenCarouselTemplate();
                default:
                    return GenCarouselTemplate();
            }
        }

        internal static ConfirmTemplate GenConfirmsTemplate()
        {
            return new ConfirmTemplate();

        }

        internal static ButtonsTemplate GenButtonsTemplate()
        {
            //            var TemplateMessages = db.Database.SqlQuery<BotTemplateMessage>(@"
            //SELECT A.*
            //  FROM dbo.BotTemplateMessage A
            // INNER JOIN dbo.BotInfo B ON A.BotSeq = BotSeq
            // WHERE B.BotName = 'LINE' AND A.Type = '{0}'", Type).FirstOrDefault() ;

            //            if (TemplateMessages == null)
            //            {

            //            }

            //            var buttonTemplateMessage = (db.BotTemplateAction.AsQueryable()
            //             .Where(o => o.TemplateSeq == TemplateMessages.Seq)).FirstOrDefault();

            //            ButtonsTemplate bt = new ButtonsTemplate();
            //            bt.thumbnailImageUrl = new Uri(buttonTemplateMessage.url);
            //            bt.altText = "This is a button menu";
            //            bt.text = buttonTemplateMessage.text;
            //            //bt.title = buttonTemplateMessage.ti
            return new ButtonsTemplate();

        }

        internal static CarouselTemplate GenCarouselTemplate()
        {
            var TemplateMessages =  db.Database.SqlQuery<BotTemplateMessage>(@"
SELECT A.*
  FROM dbo.BotTemplateMessage A
 INNER JOIN dbo.BotData B 
    ON A.BotSeq = BotSeq
 WHERE B.BotName = 'LINE' 
   AND A.Type = 'Menu'" ,  "");

            CarouselTemplate ct = new CarouselTemplate();
            ct.altText = "This is a RBot Carousel Title";
            ct.columns = new List<Column>();
            foreach (var message in TemplateMessages)
            {
                Column c = new Column();
                c.text = message.Text;
                c.title = message.Title;
                c.thumbnailImageUrl = new Uri (message.TemplateURL);
                var actionData = db.BotTemplateAction.AsQueryable()
                                 .Where(o => o.TemplateSeq == message.Seq);

                foreach (var action in actionData)
                {
                    if (action.type == "postback")
                    {
                        PostbackActon postbackAction = new PostbackActon();
                        postbackAction.data = action.data;
                        postbackAction.label = action.label;
                        postbackAction.text = action.text;
                        c.actions.Add(postbackAction);
                    }
                    else if (action.type == "url")
                    {
                        UriActon uriAction = new UriActon();
                        uriAction.label = action.label;
                        uriAction.uri = new Uri(action.url);
                        c.actions.Add(uriAction);
                    }
                }
                ct.columns.Add(c);
            }
            return ct;
        }
    }

}