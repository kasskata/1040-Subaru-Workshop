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
            FeedViewModel model = new FeedViewModel();
            ReturnFeedModel(ref model);

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

        private void ReturnFeedModel(ref FeedViewModel model)
        {
            FacebookClient client =
                new FacebookClient("EAABrDhzpuaMBAALZA9rR5agnQoktwov7B1QmN4ehI9IKSAYDI1xWQjmXyk3gMbCBGcH7VX63HoJBEycZBpIyMeHmmIIA63lNSRjdN27sSuYaeTTuIADD27vEWqZC2td5IGWD9g4frBwhCiBBYSdWH6xHdZAj7NJ32x1n2JaHBKHJFTW54XnOapYQIl8WbtvVdINtjGyk1gZDZD");
            dynamic data =
                client.Get(
                    "1040subaruworkshop/?fields=posts{id,message,created_time,comments{comment_count},picture.width(400),attachments{media{image{src}},subattachments{media{image{src}}}}}");

            //Posts
            JsonObject jsonObject = (data as JsonObject)["posts"] as JsonObject;
            JsonArray jsonArray = jsonObject["data"] as JsonArray;
            PostFbViewModel parsedPost;

            for (int i = 0; i < jsonArray.Count; i++)
            {
                JsonObject message = jsonArray[i] as JsonObject;

                if (!message.ContainsKey("message"))
                {
                    continue;
                }

                parsedPost = new PostFbViewModel
                {
                    Id = message["id"].ToString(),
                    Message = message["message"].ToString(),
                    CreatedTime = DateTime.Parse(message["created_time"].ToString()),
                    Images = new List<string>()
                };

                if (message.ContainsKey("comments"))
                {
                    var jsonCommentsArr = (message["comments"] as JsonObject)["data"] as JsonArray;
                    parsedPost.CommentsCount += jsonCommentsArr.Count;
                    for (int j = 0; j < jsonCommentsArr.Count; j++)
                    {
                        parsedPost.CommentsCount +=
                            int.Parse((jsonCommentsArr[j] as JsonObject)["comment_count"].ToString());
                    }
                }

                if (message.ContainsKey("attachments"))
                {
                    JsonArray jsonAttachmentsArray = (message["attachments"] as JsonObject)["data"] as JsonArray;
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
                                    parsedPost.Images.Add(jsonObj["src"].ToString());
                                }

                                jsonAttachmentsArray.Clear();
                            }
                            else if (jsonObj.ContainsKey("media"))
                            {
                                jsonObj = jsonObj["media"] as JsonObject;
                                jsonObj = jsonObj["image"] as JsonObject;
                                parsedPost.Images.Add(jsonObj["src"].ToString());
                            }
                        }
                    }
                }

                model.Posts.Add(parsedPost);
            }
        }
    }
}