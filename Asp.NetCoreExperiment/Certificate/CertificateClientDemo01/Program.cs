﻿using System;
using System.IO;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace CertificateClientDemo01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("回车开始");
            while (true)
            {
                try
                {
                    Console.WriteLine("1、请求Https   2、请求Http");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            HttpsMethod();
                            break;
                        case "2":
                            HttpMethod();
                            break;
                    }
                    void HttpsMethod()
                    {
                        var handler = new HttpClientHandler();
                        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                        handler.SslProtocols = SslProtocols.Tls12;
                        try
                        {
                            handler.ClientCertificates.Add(new X509Certificate2(Directory.GetCurrentDirectory() + "/server.pfx", "111111", X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                        {
                            Console.WriteLine(message);
                            Console.WriteLine(errors.ToString());

                            return true;
                        };
                        var client = new HttpClient(handler);
                        var url = "https://127.0.0.1/api/values";
                        var response = client.GetAsync(url).Result;
                        Console.WriteLine(response.IsSuccessStatusCode);
                        var back = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(back);
                    }
                    void HttpMethod()
                    {
                        var client = new HttpClient();
                        var url = "http://127.0.0.1/api/values";
                        var response = client.GetAsync(url).Result;
                        Console.WriteLine(response.IsSuccessStatusCode);
                        var back = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(back);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.InnerException?.InnerException?.Message);
                }
            }

        }
    }
}
