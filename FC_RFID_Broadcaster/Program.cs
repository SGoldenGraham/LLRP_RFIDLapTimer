using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using Org.LLRP.LTK.LLRPV1;


namespace FC_RFID_Broadcaster
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var reader = new LLRPClient();
                ENUM_ConnectionAttemptStatusType status;
                reader.Open("SpeedwayR-10-25-32", 2000, out status);

                if (status != ENUM_ConnectionAttemptStatusType.Success)
                {
                    Console.WriteLine(status.ToString());
                    return;
                }

                reader.OnRoAccessReportReceived += tester.OnReportEvent;

                tester.Delete_RoSpec(reader);
                tester.Add_RoSpec(reader);
                tester.Enable_RoSpec(reader);

                Console.ReadLine();

                tester.Delete_RoSpec(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
