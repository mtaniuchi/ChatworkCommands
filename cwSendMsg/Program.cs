using System;
using CSChatworkAPI;
using CSChatworkAPI.Models;

namespace cwSendMsg
{
    class Program
    {
        // ReSharper disable InconsistentNaming
        private const int EXIT_SUCCESS = 0;
        private const int EXIT_FAILURE = -1;
        // ReSharper restore InconsistentNaming

        static int Main(string[] args)
        {
            string messageBody;

            // MessageBody
            switch (args.Length)
            {
                case 2:
                    messageBody = Console.In.ReadToEnd();
                    break;
                case 3:
                    messageBody = args[2];
                    break;
                default:
                    PrintUsage();
                    return EXIT_FAILURE;
            }

            // APIToken
            var token = args[0];

            // RoomId
            int roomId;
            if (!int.TryParse(args[1], out roomId))
            {
                PrintUsage();
                return EXIT_FAILURE;
            }

            try
            {
                var client = new ChatworkClient(token);
                var resp = client.SendMessage(roomId, messageBody);
                if (resp == null || resp.message_id == 0)
                {
                    Console.Error.WriteLine("SendMessage failed.");
                    return EXIT_FAILURE;
                }
            }
            catch (TooManyRequestsException tmrex)
            {
                Console.Error.WriteLine("TooManyRequests.");
                Console.Error.WriteLine("Reset:{0}", tmrex.RateLimit.Reset);
                return EXIT_FAILURE;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("SendMessage failed.");
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                return EXIT_FAILURE;
            }

            
            return EXIT_SUCCESS;
        }

        static void PrintUsage()
        {
            Console.WriteLine("Send Message.");
            Console.WriteLine("[Usage1] cwSendMsg apiToken room_id messageBody");
            Console.WriteLine("\tapiToken:\tapiToken");
            Console.WriteLine("\troom_id:\troom id(chat group id)");
            Console.WriteLine("\tmessageBody:\tmessage text");
            Console.WriteLine("[Usage2] cwSendMsg apiToken room_id stdin");
            Console.WriteLine("\tapiToken:\tapiToken");
            Console.WriteLine("\troom_id:\troom id(chat group id)");
            Console.WriteLine("\tmessageBody:\tmessage with stdin");
        }
    }
}
