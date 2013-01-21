using System;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace PhotoBeamerClone.Test
{
    class Program
    {
        private static IHubProxy _mainHub;

        static void Main(string[] args)
        {
            Console.WriteLine("Press enter when the webprojects are loaded");
            Console.ReadLine();
            Connect();
            Console.ReadLine();
        }

        public async static void Connect()
        {
            var connection = new HubConnection("http://pbclone.azurewebsites.net/");
            //var connection = new HubConnection("http://localhost:4341/");
            _mainHub = connection.CreateHubProxy("imghub");

            await connection.Start().ContinueWith(_ =>
                                                {
                                                    _mainHub.Invoke("Create", "test");
                                                    //_mainHub.Invoke("SendMsg", "test ok");
                                                    //_mainHub.Subscribe("receiveMsg").Data += tokens => Console.WriteLine(tokens[0]);
                                                    _mainHub.Subscribe("ReceiveImage").Data += tokens =>
                                                                                                   {
                                                                                                       byte[] convertedFromBase64 = Convert.FromBase64String(tokens[0].ToString());

                                                                                                       Console.WriteLine
                                                                                                           (convertedFromBase64.Length);
                                                                                                   };
                                                    _mainHub.Invoke("ShareImage", new object[] { new byte[] { 1, 2 }, "test" });
                                                });
        }
    }
}
