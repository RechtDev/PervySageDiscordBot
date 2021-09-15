using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PervySageBot.Content.Fetcher
{
    internal class WebsiteScraper
    {
        public IWebDriver WebDriver { get; set; }
        public string Query { get; set; }

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

        public void GetWebsite(out string[] imgUrls, out string RefererUrl)
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
                RefererUrl = SelectedAlbum.GetAttribute("href");
                WebDriver.Navigate().GoToUrl(RefererUrl);
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
            }
        }

        public async Task<Stream[]> DownloadAttachments(string RefererUrl, params string[] Attachements)
        {
            HttpClient httpClient = new HttpClient();
            Stream[] ImgDataStreams = new Stream[Attachements.Length];
            httpClient.DefaultRequestHeaders.Add("Referer", RefererUrl);
            for (int i = 0; i < Attachements.Length; i++)
            {
                var response = await httpClient.GetAsync(Attachements[i]);
                ImgDataStreams[i] = await response.Content.ReadAsStreamAsync();
            }
            WebDriver.Quit();
            return ImgDataStreams;
        }

        private int SelectRngAblum()
        {
            Random Rng = new Random();
            return Rng.Next(0, 35);
        }
    }
}