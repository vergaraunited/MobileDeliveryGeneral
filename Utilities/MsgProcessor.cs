using MobileDeliveryLogger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDeliveryGeneral.Data;
using MobileDeliveryGeneral.Interfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Utilities
{
    public delegate isaCommand RxMsg(isaCommand cmd);
    public delegate bool TxMsg(isaCommand cmd);
    public delegate void TxMsgSend(isaCommand cmd, Func<byte[],Task> cbsend);

    public class MsgProcessor
    {
        public MsgProcessor(SendMsgDelegate sm, isaReceiveMessageCallback cb) //, isaSendMessageCallback sm)
        {
            ReceivedMsg += cb.ReceiveMessage;
            SendMsg += new TxMsg(sm);
        }
        
        static public event RxMsg ReceivedMsg;
        static public event TxMsg SendMsg;

        public void ReceiveMessage(byte[] a)
        {
            //This callback should be referenced in the Client Server API
            ProcessMessage(a);
        }

        static public isaCommand ReceiveMessage(isaCommand cmd)
        {
            switch (cmd.command)
            {
                case eCommand.Ping:
                    Logger.Debug("Command Ping recevied!");
                    SendMsg(new Command { command = eCommand.Pong });
                    break;
                //case eCommand.LogOn:
                //    MobileDeliveryLogger.Logger.Debug("Command LogOn received!");
                //    break;
                //case eCommand.LogOff:
                //    MobileDeliveryLogger.Logger.Debug("Command LogOff received!");
                //    break;
                case eCommand.Pong:
                    Logger.Debug("Command Pong recevied!");
                    break;
                case eCommand.Manifest:
                case eCommand.OrdersUpload:
                case eCommand.OrdersLoad:
                case eCommand.OrderOptionsComplete:
                    //case eCommand.OrdersLoadComplete:
                    //case eCommand.Stops:
                    //case eCommand.StopsLoadComplete:
                    ReceivedMsg(cmd);
                    //MsgProcessor.ReceiveMessage(cmd);
                    //SendMsg(new Command { command = eCommand.Manifest});
                    break;
                case eCommand.Trucks:
                    trucks truck = (trucks)cmd;

                    break;
                default:
                    //msg.ReceiveMessage(cmd);
                    break;
            }
            return cmd;
        }

        //static Dictionary<long, List<long>> dStopOrders = new Dictionary<long, List<long>>();
        static Dictionary<short, Dictionary<byte,List<long>>> dTruckCodeToDealerNumbers = new Dictionary<short, Dictionary<byte,List<long>>>();  //TruckCode -> <Seq, List<Orders>>
        public static isaCommand CommandFactory(byte[] cmdBytes)
        {
            isaCommand cmd = new Command().FromArray(cmdBytes);
            switch (cmd.command)
            {
                case eCommand.Broadcast:
                    //Broadcast cmdType = Broadcast.FromArray(cmdBytes);
                    break;
                case eCommand.DeliveryComplete:
                    //Broadcast cmdType = Broadcast.FromArray(cmdBytes);
                    break;
                case eCommand.CheckManifest:
                case eCommand.CheckManifestComplete:
                    manifestMaster mamr = new manifestMaster();
                    cmd = mamr.FromArray(cmdBytes);
                    break;
                case eCommand.GenerateManifest:
                    manifestRequest mr = new manifestRequest();
                    cmd = mr.FromArray(cmdBytes);
                    if (cmd == null)
                    {
                        manifestMaster mmr = new manifestMaster();
                        cmd = mmr.FromArray(cmdBytes);
                    }
                    //Call ApplicationServer to return the data requested.
                    //UMDManifest.
                    break;
                case eCommand.OrdersLoad:
                    orders ords = new orders();
                    cmd = ords.FromArray(cmdBytes);
                    break;
                case eCommand.Trucks:
                    trucks trks = new trucks();
                    cmd = trks.FromArray(cmdBytes);
                    break;
                case eCommand.Stops:
                    stops stps = new stops();
                    cmd = stps.FromArray(cmdBytes);
                    break;
                case eCommand.OrderDetails:
                    orderDetails cmdType = new orderDetails();
                        cmd = cmdType.FromArray(cmdBytes);
                    break;
                case eCommand.Manifest:
                    manifestMaster mm = new manifestMaster();
                    cmd = mm.FromArray(cmdBytes);
                    break;
                case eCommand.ManifestDetails:
                    manifestDetails md = new manifestDetails();
                    cmd = md.FromArray(cmdBytes);
                    break;
                case eCommand.OrderModel:
                    orders o = new orders();
                    cmd = o.FromArray(cmdBytes);
                   // OrderModelData omd = new OrderModelData(o);
                    break;
                case eCommand.OrdersUpload:
                    orderMaster om = new orderMaster();
                    cmd = om.FromArray(cmdBytes);
                    //OrderMasterData omsd = new OrderMasterData(om);
                    
                    break;
                case eCommand.ScanFile:
                    scanFile sc = new scanFile();
                    cmd = sc.FromArray(cmdBytes);
                    //ScanFileData scd = new ScanFileData(sc);
                    break;
                case eCommand.OrderOptions:
                    orderOptions oo = new orderOptions();
                    cmd = oo.FromArray(cmdBytes);
                    OrderOptionsData ood = new OrderOptionsData(oo);
                    break;
                case eCommand.AccountReceivable:
                    accountReceivable ar = new accountReceivable();
                    cmd = ar.FromArray(cmdBytes);
                    // AccountsReceivableData acr = new AccountsReceivableData((accountReceivable)cmd);
                    break;
                case eCommand.ManifestLoadComplete:
                case eCommand.TrucksLoadComplete:
                case eCommand.StopsLoadComplete:
                case eCommand.OrdersLoadComplete:
                case eCommand.OrderUpdatesComplete:
                case eCommand.OrderDetailsComplete:
                case eCommand.OrderOptionsComplete:
                case eCommand.ManifestDetailsComplete:
                case eCommand.UploadManifestComplete:
                case eCommand.LoadFilesComplete:
                case eCommand.ScanFileComplete:
                    Logger.Info($"CommandFactory: {Enum.GetName(typeof(eCommand), cmd.command) + Environment.NewLine}");
                    manifestRequest req = new manifestRequest();
                    Logger.Info($"CommandFactory: {req.ToString()}");
                    cmd = req.FromArray(cmdBytes);
                    break;
                case eCommand.Ping:
                    //Broadcast cmdType = Broadcast.FromArray(cmdBytes);
                    //cmd = SendPong();
                    break;
                case eCommand.Pong:
                    //Broadcast cmdType = Broadcast.FromArray(cmdBytes);
                    //Broadcast cmdType = Broadcast.FromArray(cmdBytes);
                    break;
                case eCommand.RunQuery:
                    //Broadcast cmdType = Broadcast.FromArray(cmdBytes);
                    break;
                default:
                    break;
            }
            return cmd;
        }
        static public isaCommand OnMsgReceived(isaCommand cmd)
        {
            isaCommand retcmd = new Command { command = cmd.command };
            //this routes back out to the client callback
            if (ReceivedMsg != null)
                retcmd = (isaCommand)ReceivedMsg?.Invoke(cmd);
            return retcmd;
        }

        static public bool OnSndMsg(isaCommand cmd)
        {
            //this handles pings and comm level messages
            return (SendMsg?.Invoke(cmd) == true);
        }
        public isaCommand ProcessMessage(byte[] message)
        {
            Command cmd = new Command();
            return OnMsgReceived(cmd.FromArray(message));
        }
        
        static public bool SendMessage(isaCommand cmd)
        {
            return OnSndMsg(cmd);
        }
    }
}
