using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.LLRP.LTK.LLRPV1;
using Org.LLRP.LTK.LLRPV1.DataType;

namespace FC_RFID_Broadcaster
{
    public class tester
    {
        public static void Delete_RoSpec(LLRPClient reader)
        {
            var msg = new MSG_DELETE_ROSPEC {ROSpecID = 0};
            MSG_ERROR_MESSAGE errorMessage;

            var response = reader.DELETE_ROSPEC(msg, out errorMessage, 2000);

            ResponseWriteLine(response, errorMessage);
        }

        public static void ResponseWriteLine(MSG_DELETE_ROSPEC_RESPONSE response, MSG_ERROR_MESSAGE errorMessage)
        {
            if (response != null)
            {
                Console.WriteLine(response.ToString());
            }
            else if (errorMessage != null)
            {
                Console.WriteLine(errorMessage.ToString());
            }
            else
            {
                Console.WriteLine("Timeout Error");
            }
        }


        public static void Add_RoSpec(LLRPClient reader)
        {
            MSG_ERROR_MESSAGE errorMessage;
            var msg = new MSG_ADD_ROSPEC {ROSpec = new PARAM_ROSpec()};
            msg.ROSpec.CurrentState = ENUM_ROSpecState.Disabled;
            msg.ROSpec.ROSpecID = 123;

            msg.ROSpec.ROBoundarySpec = new PARAM_ROBoundarySpec();
            msg.ROSpec.ROBoundarySpec.ROSpecStartTrigger = new PARAM_ROSpecStartTrigger();
            msg.ROSpec.ROBoundarySpec.ROSpecStartTrigger.ROSpecStartTriggerType = ENUM_ROSpecStartTriggerType.Immediate;
            msg.ROSpec.ROBoundarySpec.ROSpecStopTrigger = new PARAM_ROSpecStopTrigger();
            msg.ROSpec.ROBoundarySpec.ROSpecStopTrigger.ROSpecStopTriggerType = ENUM_ROSpecStopTriggerType.Null;

            msg.ROSpec.SpecParameter = new UNION_SpecParameter();
            var aiSpec = new PARAM_AISpec {AntennaIDs = new UInt16Array()};
            aiSpec.AntennaIDs = new UInt16Array();
            aiSpec.AntennaIDs.Add(0);
            aiSpec.AISpecStopTrigger = new PARAM_AISpecStopTrigger();
            aiSpec.AISpecStopTrigger.AISpecStopTriggerType = ENUM_AISpecStopTriggerType.Null;

            aiSpec.InventoryParameterSpec = new PARAM_InventoryParameterSpec[1];
            aiSpec.InventoryParameterSpec[0] = new PARAM_InventoryParameterSpec();
            aiSpec.InventoryParameterSpec[0].InventoryParameterSpecID = 1234;
            aiSpec.InventoryParameterSpec[0].ProtocolID = ENUM_AirProtocols.EPCGlobalClass1Gen2;
            msg.ROSpec.SpecParameter.Add(aiSpec);

            msg.ROSpec.ROReportSpec = new PARAM_ROReportSpec();
            msg.ROSpec.ROReportSpec.ROReportTrigger = ENUM_ROReportTriggerType.Upon_N_Tags_Or_End_Of_ROSpec;
            msg.ROSpec.ROReportSpec.N = 1;
            msg.ROSpec.ROReportSpec.TagReportContentSelector = new PARAM_TagReportContentSelector();

            var response = reader.ADD_ROSPEC(msg, out errorMessage, 2000);

            if (response != null)
            {
                Console.WriteLine(response.ToString());
            }
            else if (errorMessage != null)
            {
                Console.WriteLine(errorMessage.ToString());
            }
            else
            {
                Console.WriteLine("Timeout Error");
            }
        }

        public static void Enable_RoSpec(LLRPClient reader)
        {
            MSG_ERROR_MESSAGE errorMessage;
            var msg = new MSG_ENABLE_ROSPEC();
            msg.ROSpecID = 123;
            var response = reader.ENABLE_ROSPEC(msg, out errorMessage, 2000);

            if (response != null)
            {
                Console.WriteLine(response.ToString());
            }
            else if (errorMessage != null)
            {
                Console.WriteLine(errorMessage.ToString());
            }
            else
            {
                Console.WriteLine("Timeout Error");
            }
        }

        public static void OnReportEvent(MSG_RO_ACCESS_REPORT msg)
        {
            foreach (var tagReport in msg.TagReportData)
            {
                if (tagReport.EPCParameter.Count > 0)
                {
                    string epc;
                    
                    var epcParam = tagReport.EPCParameter[0];
                    if (epcParam.GetType() == typeof(PARAM_EPC_96))
                    {
                        epc = ((PARAM_EPC_96) epcParam).EPC.ToHexString();
                    }
                    else
                    {
                        epc = ((PARAM_EPCData) epcParam).EPC.ToHexString();
                    }
                    Console.WriteLine("epc = " + epc);
                }
            }
        }
    }
}
