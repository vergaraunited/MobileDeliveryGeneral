using System;
using System.Collections.Generic;
using UMDGeneral.Interfaces.DataInterfaces;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Data
{
   // public sealed class ProcessMessage
    //{
       // public enum EMsgType { LogOn, LogOff, GenerateManifest, LoadManifest, ManifestLoadComplete, DeliveryComplete, Unknown };
       /* public EMsgType msgType { get; set; }

        public class ProcessMessageHeader
        {
            public string token { get; set; }
            public EMsgType eMsg { get; set; }
        }

        public ProcessMessageHeader header { get; set; }
        */

        //public T value { get; set; }
   // }


    ///////////////////////////////Message types/////////////////////////////////

   

    // Order Detail
    //public sealed class OrderDetail : IMDMMessage
    //{
    //    public string ordernum { get; set; }
    //    public string modelnum { get; set; }
    //    public string modeldesc { get; set; }
    //    public string color { get; set; }
    //    public string width { get; set; }
    //    public string height { get; set; }
    //    public string linenum { get; set; }
    //    public string unitnum{ get; set; }
    //    public Int16 status { get; set; }
    //    public eCommand Command { get; set; }
    //    public Guid RequestId { get; set; }
    //}

    // Order Master
    public sealed class OrderMaster : IMDMMessage
    {
        public Guid RequestId { get; set; }
        public string ordernum { get; set; }
        public string customername { get; set; }
        public string accountnum { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string routecode { get; set; }
        public string phonenum { get; set; }
        public Int16 billcomeplete { get; set; }
        public string contact { get; set; }
        public eCommand Command { get; set; }
    }

  
    // Repair Images
    public sealed class RepairImage : IMDMMessage
    {
        public string ordernum { get; set; }
        public string linenum { get; set; }
        public string unitnum { get; set; }
        public string image { get; set; }
        public eCommand Command { get; set; }
        public Guid RequestId { get; set; }
    }

    // User
    public sealed class User : IMDMMessage
    {
        public string userid { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public eCommand Command { get; set; }
        public Guid RequestId { get; set; }
    }

    public enum eStatus : byte
    {
        InProgress,
        Success,
        PermissionDenied,
        Timeout,
        Failed
    }

    // FileCopy
    public sealed class FileCopy : IMDMMessage
    {
        public List<string> files { get; set; }
        public eStatus status {get; set;}
        public DateTime datetime { get; set; }
        public eCommand Command { get; set; }
        public Guid RequestId { get; set; }
    }
    // 
    /*   public sealed class MessageTypeConverter : JsonConverter
       {
           public override bool CanConvert(Type objectType)
           {
               return objectType == typeof(string);
           }

           public override object ReadJson(JsonReader reader, Type objectType,
                   object existingValue, JsonSerializer serializer)
           {
               var value = (string)reader.Value;

               switch (value)
               {
                   case "logon":
                       return EMsgType.LogOn;
                   case "logoff":
                       return EMsgType.LogOff;
                   case "deliverycomplete":
                       return EMsgType.DeliveryComplete;
                   case "generatemanifest":
                       return EMsgType.GenerateManifest;
                   case "loadmanifest":
                       return EMsgType.LoadManifest;
                   case "manifestloadcomplete":
                       return EMsgType.ManifestLoadComplete;
                   default:
                       return EMsgType.Unknown;
               }
           }

           public override void WriteJson(JsonWriter writer, object valuei, JsonSerializer serializer)
           {
               var value = (EMsgType)valuei;

               switch (value)
               {
                   case EMsgType.LogOn:
                       writer.WriteValue("logon");
                       break;
                   case EMsgType.LogOff:
                       writer.WriteValue("logoff");
                       break;
                   case EMsgType.DeliveryComplete:
                       writer.WriteValue("deliverycomplete");
                       break;
                   case EMsgType.GenerateManifest:
                       writer.WriteValue("generatemanifest");
                       break;
                   case EMsgType.LoadManifest:
                       writer.WriteValue("loadmanifest");
                       break;
                   case EMsgType.ManifestLoadComplete:
                       writer.WriteValue("manifestloadcomplete");
                       break;
                   default:
                       writer.WriteValue("unknown");
                       break;
               }
           }
       }
   }  */
}
