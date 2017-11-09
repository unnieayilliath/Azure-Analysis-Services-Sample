using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleConsoleAppQueryingAzureAS
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadFromAzureAS().Wait();
            Console.WriteLine("****Complete***");
            Console.ReadKey();
        }

        private static async Task ReadFromAzureAS()
        {
            var clientId = "<client id of Azure AD app a.ka Service principal>";
            var clientSecret = "<client secret>";
            var domain = "yourdomain.onmicrosoft.com";
            var ssasUrl = "northeurope.asazure.windows.net";//get this from your Azure AS connectionString
            var token = await ADALTokenHelper.GetAppOnlyAccessToken(domain, $"https://{ssasUrl}", clientId, clientSecret);
            
            var connectionString = $"Provider=MSOLAP;Data Source=asazure://{ssasUrl}/myanalysisservice;Initial Catalog= adventureworks;User ID=;Password={token};Persist Security Info=True;Impersonation Level=Impersonate";
            var ssasConnection = new AdomdConnection(connectionString);
            ssasConnection.Open();
            var query = @"Evaluate TOPN(10,Customer,Customer[Customer Id],1)";
            var cmd = new AdomdCommand(query)
            {
                Connection = ssasConnection
            };
            using (var reader = cmd.ExecuteXmlReader())
            {
                string value = reader.ReadOuterXml();
                Console.WriteLine(value);
            }
        }
    }
}
