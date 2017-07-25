using System;
using System.Web.Mvc;

namespace CarService.Controllers
{
    using System.Collections.Generic;
    using Facebook;
    using Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            this.ViewBag.Time = (DateTime.Now + TimeSpan.FromHours(3)).ToString("yyyy/MM/dd HH:mm");
            return this.View();
        }

        public ActionResult FeedFacebook()
        {
            FacebookClient client = new FacebookClient("EAACEdEose0cBADl3CgHJSduGmOdn7ZB7CHMrIt8qsz4vDdOCpAzjKqXPN6TY00BPOyfQo1PGBm80iOUdD0IlSDJRniaAaq3ZC6eZC4xKG8mFuGJ8LAtndcpqcSZAir8lZCNStbqt2Cg0uHjzywFQ2TruuhINRaqq1lGFwnuqugd9W2x3BFTYHShZBcHCbxuZCoZD");
            dynamic data = client.Get("1040SubaruWorkshop?fields=posts");

            FeedViewModel model = new FeedViewModel();
            JsonObject jsonObject = (data as JsonObject)["posts"] as JsonObject;
            JsonArray jsonArray = jsonObject["data"] as JsonArray;

            for (int i = 0; i < jsonArray.Count; i++)
            {
                var message = jsonArray[i] as JsonObject;

                if (message.ContainsKey("message"))
                {
                    var post = new PostFbViewModel
                    {
                        Id = message["id"].ToString(),
                        Message = message["message"].ToString(),
                        CreatedTime = DateTime.Parse(message["created_time"].ToString()),
                        Images = new List<string>()
                    };

                    data = client.Get(post.Id + "/attachments");
                    if ((data as JsonObject).ContainsKey("data"))
                    {
                        JsonArray jsonAttachmentsArray = data["data"] as JsonArray;
                        if (jsonAttachmentsArray != null && jsonAttachmentsArray.Count > 0)
                        {
                            for (int j = 0; j < jsonAttachmentsArray.Count; j++)
                            {
                                var jsonObj = jsonAttachmentsArray[j] as JsonObject;
                                if (jsonObj.ContainsKey("subattachments"))
                                {
                                    jsonObj = jsonObj["subattachments"] as JsonObject;
                                    jsonAttachmentsArray = jsonObj["data"] as JsonArray;

                                    for (int k = 0; k < jsonAttachmentsArray.Count; k++)
                                    {
                                        jsonObj = jsonAttachmentsArray[k] as JsonObject;
                                        jsonObj = jsonObj["media"] as JsonObject;
                                        jsonObj = jsonObj["image"] as JsonObject;
                                        post.Images.Add(jsonObj["src"].ToString());
                                    }

                                }
                                else if (jsonObj.ContainsKey("media"))
                                {
                                    jsonObj = jsonObj["media"] as JsonObject;
                                    jsonObj = jsonObj["image"] as JsonObject;
                                    post.Images.Add(jsonObj["src"].ToString());
                                }
                            }
                        }
                    }

                    model.Posts.Add(post);
                }
            }

            return this.View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Calendar()
        {
            throw new NotImplementedException();
        }
    }
}