﻿using System;
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

        public bool InsertRequestLog(string LogContent)
        {
            RequestLog newRequestLog = new RequestLog();
            newRequestLog.LogDate = DateTime.Now;
            newRequestLog.LogContent = LogContent;
            try
            {
                DB.RequestLog.Add(newRequestLog);
                DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetNLPURL(string NLPName)
        {
            var URL = DB.NLPInfo.AsQueryable()
                .Where(o => o.NLPName == NLPName)
                .Select(o => o.WebHookURL).FirstOrDefault();
            return URL;
        }

        public NLPInfo GetNLPInfo(string NLPName)
        {
            return DB.NLPInfo.AsQueryable().Where(o => o.NLPName == NLPName).FirstOrDefault();
        }

        /// 判斷User查詢內容
        /// <summary>
        /// 判斷User查詢內容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetResult(Microsoft.Cognitive.LUIS.LuisResult analysisResult)
        {
            string Default = "很抱歉,無法解析你所問的問題";
            if (analysisResult == null)
            {
                return Default;
            }

            var intent = analysisResult.TopScoringIntent;
            var enitiesDictinary = analysisResult.Entities;
            IList<Microsoft.Cognitive.LUIS.Entity> entitiesCollection;
            try
            {
                //TODO 要用設計模式比較好
                if (intent.Name == "匯率查詢")
                {
                    string CurName;
                    DateTime ExDate;
                    if (enitiesDictinary.TryGetValue("Currency", out entitiesCollection))
                    {
                        CurName = entitiesCollection.Select(o => o.Value).FirstOrDefault();
                    }
                    else
                    {
                        return Default;
                    }
                    if (enitiesDictinary.TryGetValue("ExDate", out entitiesCollection))
                    {
                        DateTime.TryParse(entitiesCollection.Select(o => o.Value).FirstOrDefault(), out ExDate);
                    }
                    else
                    {
                        ExDate = DateTime.Today;
                    }
                    return GetCurrencyExRate(CurName, ExDate);
                }
                else
                {
                    return Default;
                }
            }
            catch (Exception ex)
            {
                return Default;
            }
        }

        private string GetCurrencyExRate(string CurrencyName, DateTime ExDate)
        {
            string currency = DB.CurInfo.AsQueryable()
                              .Where(o => o.CurChtName == CurrencyName)
                              .Select(o=>o.CurAbName).FirstOrDefault();
            var ExRateObj = DB.ExRate.AsQueryable()
                       .Where(o=>o.Cur == currency)
                       .Select(o => new
                       {
                           BuyRate = o.ExBRate,
                           SellRate = o.ExSRate
                       }).FirstOrDefault();

            return string.Format("{0} {1}匯率 : {4}買進{2} 賣出{3}", 
                CurrencyName, 
                ExDate.ToString("yyyyMMdd"), 
                ExRateObj.BuyRate.ToString("#.####"), 
                ExRateObj.SellRate.ToString("#.####"),
                Environment.NewLine);
        }
    }
}