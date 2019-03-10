using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public static String GetFullNewspaperText(String url)
        {
            //Console.WriteLine("parsing: " + url);

            String APIKey = "nGc0ya2J7z2aalFrGa8Gx3Q1o8grGFsn3cz58EJy";
            String APIServer = "https://mercury.postlight.com/parser?url=" + url;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(APIServer);
            //request.TransferEncoding = "windows-1251";

            request.Headers.Add("x-api-key", APIKey);
            request.ContentType = "application/html";

            HttpWebResponse response = null;
            //try
            //{
            response = (HttpWebResponse) request.GetResponse(); // иногда ломается с Базовое соединение закрыто: Непредвиденная ошибка при передаче.
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message == "Базовое соединение закрыто: Непредвиденная ошибка при передаче.")

            //    Console.WriteLine(ex.Message + " url: " + url);
            //    return "";
            //}

            //Console.WriteLine(response.CharacterSet);

            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

            String json = "";
            try
            {
                json = sr.ReadToEnd();
            }
            catch
            {
                return "";
            }

            MercuryAPIResponce ps = JsonConvert.DeserializeObject<MercuryAPIResponce>(json);

            ps.content = System.Net.WebUtility.HtmlDecode(ps.content);

            //Console.WriteLine("have parsed: " + url);

            return ps.content.Replace("'", "");
        }

        public static String ParseObsolete(String url)
        {
            try
            {
                //String url = "https://properm.ru/news/politic/167508/";


                String dirName = "temp";
                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                String input = dirName + "/" + Path.GetRandomFileName().Split('.')[0] + ".html";
                String output = input.Split('.')[0] + ".txt";

                Console.WriteLine($"parsing " + url + " filename: " + input);

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream stream = response.GetResponseStream();
                String encString = response.CharacterSet;
                //Console.WriteLine(encString);
                Encoding enc;
                if (encString.Length < 1)
                    enc = Encoding.UTF8;
                else
                    enc = Encoding.GetEncoding(encString);

                StreamReader sr = new StreamReader(stream, enc);

                String text = sr.ReadToEnd();
                //MessageBox.Show(text);
                File.WriteAllText(input, text, enc);
                //Console.WriteLine("exe info collecting");
                ProcessStartInfo startInfo = new ProcessStartInfo("extract.exe");
                startInfo.Arguments = input;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //Console.WriteLine("exe starting");
                Process.Start(startInfo);

                //Console.WriteLine("input file: " + input);
                //Console.WriteLine("output file: " + output);

                int i = 0; // если скрипт вдруг сломается
                while (!File.Exists(output))
                {
                    if (i > 70)
                    {
                        File.Delete(input);
                        Console.WriteLine("error: process stopped");
                        return "";
                    }

                    i++;
                    System.Threading.Thread.Sleep(50);
                    //Console.WriteLine("sleep");
                }

                System.Threading.Thread.Sleep(50);
                String result = File.ReadAllText(output);

                //Console.WriteLine("parsing done, clean temp files");

                File.Delete(input);
                File.Delete(output);

                //Console.WriteLine("cleaning done. \n");

                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "";
            }
        }

        public static void ParseNewsObsolete(Newspaper np)
        {
            try
            {
                String dirName = "temp";
                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                String input = dirName + "/" + Path.GetRandomFileName().Split('.')[0] + ".html";
                String output = input.Split('.')[0] + ".txt";

                //Console.WriteLine($"parsing " + np.s_link + " filename: " + input);

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(np.s_link);
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream stream = response.GetResponseStream();
                //String encString = response.CharacterSet;
                //Console.WriteLine(encString);

                //Encoding enc;
                //if (encString.Length < 1)
                //    enc = Encoding.UTF8;
                //else
                //    enc = Encoding.GetEncoding(encString);

                Encoding enc; // некоторые сайты не возвращают кодировку
                try
                {
                    enc = Encoding.GetEncoding(response.CharacterSet);
                    np.s_header += " (enc: " + enc.EncodingName + " charset: " + response.CharacterSet + ") ";
                    //Console.WriteLine("\nload full: enc: " + enc.EncodingName + " charset: " + response.CharacterSet + "\n");
                }
                catch (System.ArgumentException)
                {
                    //Console.WriteLine("\nload full: set default encoding: " + response.CharacterSet + " site: " + np.s_link + "\n");
                    enc = Encoding.UTF8;
                }

                StreamReader sr = new StreamReader(stream, enc);

                String text = sr.ReadToEnd();
                //MessageBox.Show(text);
                File.WriteAllText(input, text, enc);
                //Console.WriteLine("exe info collecting");
                ProcessStartInfo startInfo = new ProcessStartInfo("extract.exe");
                startInfo.Arguments = input;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //Console.WriteLine("exe starting");
                Process process = Process.Start(startInfo);

                //Console.WriteLine("input file: " + input);
                //Console.WriteLine("output file: " + output);

                int i = 0; // если скрипт вдруг сломается
                while (!File.Exists(output))
                {
                    if (i > 70)
                    {
                        File.Delete(input);
                        //Console.WriteLine("error: process stopped");
                        np.s_full_text = "";
                        return;
                    }

                    i++;
                    System.Threading.Thread.Sleep(50);
                    //Console.WriteLine("sleep");
                }

                String result = File.ReadAllText(output);

                //Console.WriteLine("parsing done, clean temp files");

                File.Delete(input);
                File.Delete(output);

                //Console.WriteLine("cleaning done. \n");

                np.s_full_text = result;
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("GLOBAL EXCEPTION: " + e.Message);
                np.s_full_text = "";
                return;
            }
        }

        public static void ParseNewsNewObsolete(Newspaper np)
        {
            try
            {
                //Console.WriteLine($"parsing " + np.s_link + " filename: " + input);

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(np.s_link);
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream stream = response.GetResponseStream();

                Encoding enc; // некоторые сайты не возвращают кодировку
                try
                {
                    enc = Encoding.GetEncoding(response.CharacterSet);
                    np.s_header += " (enc: " + enc.EncodingName + " charset: " + response.CharacterSet + ") ";
                    //Console.WriteLine("\nload full: enc: " + enc.EncodingName + " charset: " + response.CharacterSet + "\n");
                }
                catch (System.ArgumentException)
                {
                    //Console.WriteLine("\nload full: set default encoding: " + response.CharacterSet + " site: " + np.s_link + "\n");
                    enc = Encoding.UTF8;
                }

                StreamReader sr = new StreamReader(stream, enc);

                String parserInput = sr.ReadToEnd().Replace("\n", "").Replace("\r", "");


                //Console.OutputEncoding = System.Text.Encoding.UTF8;

                var psi = new ProcessStartInfo
                {
                    FileName = "extract.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                };
                psi.StandardOutputEncoding = Encoding.GetEncoding(866);

                var p = Process.Start(psi);

                p.StandardInput.WriteLine(
                    "dsdfgsdfgdsfgdfgsdfg sdfg sdfg sdfg sdfg df тест тест етствыа тфиывало ыорвлд лроывалорфлыодрва лдорфыв");
                p.StandardInput.Flush();



                Encoding cp866 = Encoding.GetEncoding(866);
                Encoding cp1251 = Encoding.Default;

                String t2 = p.StandardOutput.ReadToEnd();
                byte[] cp866Bytes = cp1251.GetBytes(t2);
                cp866Bytes = Encoding.Convert(cp866, cp1251, cp866Bytes);
                var t3 = cp1251.GetString(cp866Bytes);
                p.WaitForExit();

                File.WriteAllBytes("test.txt", cp866Bytes);

                MessageBox.Show(t3);


                System.Threading.Thread.Sleep(100000);

            }
            catch (Exception e)
            {
                Console.WriteLine("GLOBAL EXCEPTION: " + e.Message);
                np.s_full_text = "";
            }
        }
    }
}