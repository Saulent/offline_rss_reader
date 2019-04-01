using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace second_course
{
    public class ParseFullNewspaper
    {
        /// <summary>
        /// Получить выжимку из новости в виде основного текста и картинок в виде HTML используя API Mercury 
        /// </summary>
        /// <param name="url">Адрес сайта, который необходимо обработать</param>
        /// <returns>Выделенная информация в виде HTML</returns>
        public static String GetFullNewspaperText(String url, int newspaperId)
        {
            String APIKey = "nGc0ya2J7z2aalFrGa8Gx3Q1o8grGFsn3cz58EJy";
            String APIServer;
            if (Params.isLocalServerUsing)
            {
                APIServer = "http://127.0.0.1:3000/" + url;
            }
            else
            {
                APIServer = "https://mercury.postlight.com/parser?url=" + url;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(APIServer);
            request.Headers.Add("x-api-key", APIKey);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

            String json = "";
            try
            {
                json = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show("get json " + ex.Message);
                return "";
            }

            MercuryAPIResponce ps = JsonConvert.DeserializeObject<MercuryAPIResponce>(json);
            string decoded = System.Net.WebUtility.HtmlDecode(ps.content).Replace("'", "");
            SelectAndSaveImages(ps.content, newspaperId);
            string fullText = GetFullTextFromHTML(decoded);

            return fullText;
        }
        private static string GetFullTextFromHTML(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            string fullText = "";
            try
            {
                fullText = doc.DocumentNode.InnerText.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error parsing html: " + ex.Message);
            }

            return fullText;
        }
        private static void SelectAndSaveImages(string fullText, int newspaperId)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(fullText);

            HtmlNodeCollection images = doc.DocumentNode.SelectNodes("//img");

            if (images != null)
            {
                foreach (HtmlNode image in images)
                {
                    string url = image.Attributes.AttributesWithName("src").First().Value;
                    
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    string localPath = LoadImageGetLocalPath(url);
                    ImageDB img = new ImageDB();
                    img.i_newspaper_id = newspaperId;
                    img.s_url = url;
                    img.s_path = localPath;

                    DataBase.SaveImage(img);
                }
            }
        }
        private static string LoadImageGetLocalPath(string url)
        {
            if (!Directory.Exists("temp"))
            {
                Directory.CreateDirectory("temp");
            }

            string fileName = "temp/" + Path.GetRandomFileName().Split('.')[0] + ".jpg";
            using (var client = new WebClient())
            {
                client.DownloadFile(url, fileName);
            }

            return fileName;
        }
    }
}