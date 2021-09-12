using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace PervySageBot.Content.Fetcher
{
    class WebsiteScraper
    {
        public IWebDriver WebDriver {get; set;}
        public string Query { get; set; }
        Uri SelectedAlbumUrl;
        public List<string> WebsiteSourcesUrl { get; set; } = new List<string>
        {
            @"https://www.erome.com/"
        };
        public WebsiteScraper(string query) 
        {
            Query = query;
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");
            WebDriver = new FirefoxDriver(options);
        }       
        ~WebsiteScraper()
        {
            WebDriver.Quit();
        }
        public void GetWebsite(out string[] imgUrls)
        {
            StringBuilder FullUrl = new StringBuilder();
            FullUrl.Append(WebsiteSourcesUrl[0]);
            Uri uri = new Uri(FullUrl.ToString());
            FullUrl.Append($@"search?q={Query}");
            try
            {
                WebDriver.Navigate().GoToUrl(FullUrl.ToString());
                var Albums = WebDriver.FindElements(By.ClassName("col-lg-2"));
                IWebElement SelectedAlbum = Albums[SelectRngAblum()].FindElement(By.TagName("a"));
                SelectedAlbumUrl = new Uri(SelectedAlbum.GetAttribute("href"));
                WebDriver.Navigate().GoToUrl(SelectedAlbumUrl.AbsoluteUri);
                var imgAttachments = WebDriver.FindElements(By.ClassName("img-front"));
                if (imgAttachments.Count == 1)
                {
                    imgUrls = new string[1];
                    for (int i = 0; i < 1; i++)
                    {
                        imgUrls[i] = imgAttachments[i].GetAttribute("src");
                    }
                }
                else if (imgAttachments.Count == 2)
                {
                    imgUrls = new string[2];
                    for (int i = 0; i < 2; i++)
                    {
                        imgUrls[i] = imgAttachments[i].GetAttribute("src");
                    }
                }
                else if (imgAttachments.Count == 3)
                {
                    imgUrls = new string[3];
                    for (int i = 0; i < 3; i++)
                    {
                        imgUrls[i] = imgAttachments[i].GetAttribute("src");
                    }
                }   
                else
                {
                    imgUrls = new string[3];
                    for (int i = 0; i < 3; i++)
                    {
                        imgUrls[i] = imgAttachments[i].GetAttribute("src");
                    }
                }   
            }
            finally
            {
                //WebDriver.Quit();
            }         
        }

        public CookieContainer GetSesstionCookies()
        {
            //creates container for cookies
            var Cookies = new CookieContainer();
            //converts Selenium.Cookie to Net.Cookie
            foreach (var cookie in WebDriver.Manage().Cookies.AllCookies)
            {
                System.Net.Cookie netcookie = new System.Net.Cookie()
                {
                    Domain = cookie.Domain,
                    HttpOnly = cookie.IsHttpOnly,
                    Name = cookie.Name,
                    Path = cookie.Path,
                    Secure = cookie.Secure,
                    Value = cookie.Value,
                };
                if (cookie.Expiry.HasValue)
                    netcookie.Expires = cookie.Expiry.Value;
                Cookies.Add(netcookie);
            }
            return Cookies;
        }   

         public async Task DownloadaAttachments(CookieContainer cookies, params string[] Attachements)
         {
            //Just takeing one src url and setting it to a Uri instance for testing
            Uri uri = new Uri(Attachements[0]);
            //passing the cookies from WebDriver
            var httpClientHandler = new HttpClientHandler() { CookieContainer = cookies};
            var client = new HttpClient(httpClientHandler) { BaseAddress = uri};
            //calling GetStream and waiting for response
            var response = await client.GetStreamAsync(uri);
         } 

        private int SelectRngAblum()
        {
            Random Rng = new Random();
            return Rng.Next(0, 35);
        }
    }
}
