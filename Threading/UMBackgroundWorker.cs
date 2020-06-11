using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MobileDeliveryGeneral.Data;
using MobileDeliveryGeneral.Definitions;
using MobileDeliveryGeneral.ExtMethods;
using MobileDeliveryGeneral.Interfaces;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using MobileDeliveryLogger;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Threading
{
    public class UMBackgroundWorker<O> where O : IMDMMessage
    {
        BackgroundWorker bgWorker { get; set; }
        class evData { public ManualResetEvent evnt; public int secReset; public string name; }

        Dictionary<Guid, evData> dDoneEvent = new Dictionary<Guid, evData>();

        SendMsgDelegate sm;
        ProcessMsgDelegateRXRaw pm;
        //Func<byte[], Task> cbsend;

        public delegate void ProgressChanged<O>(O inp, Func<byte[], Task> cbsend = null);
        ProgressChanged<O> pc;
        ReceiveMsgDelegate rcvMsg;
       
        public UMBackgroundWorker(ProgressChanged<O> pc, ReceiveMsgDelegate rcvMsg, SendMsgDelegate sm=null, ProcessMsgDelegateRXRaw pm=null )
        {
            this.pc = pc;
            this.rcvMsg = rcvMsg;
            this.sm = sm;
            this.pm = pm;
        }
        void Init()
        {
            bgWorker = new BackgroundWorker();

            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged +=
                new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.WorkerReportsProgress = true;
        }
        public void Reset(Guid guid)
        {
            try
            {
                if (dDoneEvent.ContainsKey(guid))
                    dDoneEvent[guid].evnt.Reset();
            }
            catch (Exception ex) { }
        }

        public void OnStartProcess(manifestRequest req, Request reqInfo, ProcessMsgDelegateRXRaw cbsend=null)
        {
            Init();
            Guid g = NewGuid();

            if (req.requestId != null)
                g = new Guid(req.requestId);
            else
                req.requestId = g.ToByteArray();

            Logger.Info($"OnStartProcess Starting the background process Request: {req.ToString()}");

            object[] paramArgs = new object[] {
            req, reqInfo, cbsend };
            int to = 0;
            if (!dDoneEvent.ContainsKey(g))
            {
                string name = Enum.GetName(typeof(eCommand), req.command);
                if (name.CompareTo("OrdersLoad") == 0)
                    to = 60000;
                dDoneEvent.Add(g, new evData() { evnt=new ManualResetEvent(false), secReset=to, name=name});
            }
            bgWorker.RunWorkerAsync(paramArgs);
        }
        
        public  void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] parameters = e.Argument as object[];
            manifestRequest req = (manifestRequest)parameters[0];
            try
            {
                Request reqInfo = (Request)parameters[1];
                Logger.Info($"bgWorker_DoWork {Enum.GetName(typeof(eCommand), req.command)} ");
                switch (req.command)
                {
                    case eCommand.Drivers:
                    case eCommand.Manifest:
                    case eCommand.GenerateManifest:
                    case eCommand.LoadFiles:
                        if (!sm(req))
                            throw new Exception("UMBackgroundWorker::connectionVM.SendMsgWinsys - Failed to send Winsys Server a message");
                        break;

                    case eCommand.ManifestDetails:
                    case eCommand.OrderDetails:
                    case eCommand.OrderOptions:
                    case eCommand.OrdersUpload:
                    case eCommand.OrdersLoad:
                    case eCommand.Trucks:
                    case eCommand.Stops:
                    case eCommand.UploadManifest:
                    case eCommand.UploadManifestComplete:
                    case eCommand.CompleteStop:
                    case eCommand.CompleteOrder:
                    case eCommand.AccountReceivable:
                        sm(req);
                        break;
                    case eCommand.TrucksLoadComplete:
                        CompleteBackgroundWorker(NewGuid(req.requestId));
                        break;
                    default:
                        throw new Exception("Command not found");
                }
            }
            catch (Exception ex) {
            }
            finally
            {
                if (dDoneEvent.ContainsKey(new Guid(req.requestId)))
                {
                    if (dDoneEvent[new Guid(req.requestId)].secReset > 0)
                        dDoneEvent[new Guid(req.requestId)].evnt.WaitOne(dDoneEvent[new Guid(req.requestId)].secReset);
                    else
                        dDoneEvent[new Guid(req.requestId)].evnt.WaitOne();
                }
            }
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            object[] parameters = e.UserState as object[];
            isaCommand cmd = (isaCommand )parameters[0];
            Func<byte[], Task> cbsend = null;
            if (parameters.Length == 3)
                cbsend = (Func<byte[], Task>)parameters[2];
            IMDMMessage mcmd = null;
            
            switch (cmd.command)
            {
                case eCommand.Manifest:
                    mcmd = new ManifestMasterData((manifestMaster)cmd, ((manifestMaster)cmd).id);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.ManifestLoadComplete:
                    manifestRequest mreq = (manifestRequest)cmd;
                    if (mreq.valist != null)
                    {
                        foreach (var it in mreq.valist)
                        {
                            mcmd = new ManifestMasterData()
                            {
                                RequestId = new Guid(cmd.requestId),
                                LINK = it,
                                SHIP_DTE = DateTime.Today,
                                Command = eCommand.ManifestLoadComplete
                            };
                            Sendback((O)mcmd, cbsend);
                        }
                    }
                    else
                    {
                        mcmd = new ManifestMasterData()
                        {
                            RequestId = new Guid(cmd.requestId),
                            LINK = -1,
                            SHIP_DTE = DateTime.Today,
                            Command = eCommand.ManifestLoadComplete
                        };
                        Sendback((O)mcmd, cbsend);
                    }
                    break;
                case eCommand.CheckManifestComplete:
                    mcmd = new ManifestMasterData((manifestMaster)cmd, ((manifestMaster)cmd).id, false);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.CheckManifest:
                    mcmd = new ManifestMasterData((manifestMaster)cmd, ((manifestMaster)cmd).id, false);
                    Sendback((O)mcmd, cbsend);                   
                    break;
                case eCommand.Drivers:
                    mcmd = new DriverData() { Command = cmd.command };
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.DriversLoadComplete:
                    mcmd = (DriverData)cmd;
                    Sendback((O)mcmd, cbsend);
                    CompleteBackgroundWorker(new Guid(cmd.requestId));
                    break;
                case eCommand.ManifestDetails:
                    mcmd = new ManifestDetailsData() { Command = cmd.command };
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.AccountReceivable:
                    mcmd = new AccountsReceivableData((accountReceivable)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.ManifestDetailsComplete:
                    mreq = (manifestRequest)cmd;

                    if (mreq.valist != null)
                    {
                        foreach (var it in mreq.valist)
                        {
                            mcmd = new ManifestDetailsData()
                            {
                                RequestId = new Guid(cmd.requestId),
                                DLR_NO = it,
                                Command = eCommand.ManifestDetailsComplete
                            };
                            Sendback((O)mcmd, cbsend);
                        }
                    }
                    else
                    {
                        mcmd = new ManifestMasterData()
                        {
                            RequestId = new Guid(cmd.requestId),
                            LINK = -1,
                            SHIP_DTE = DateTime.Today,
                            Command = eCommand.ManifestLoadComplete
                        };
                        Sendback((O)mcmd, cbsend);
                    }
                    //mcmd = new ManifestDetailsData((manifestDetails)cmd);
                    //Sendback((O)mcmd, cbsend);
                    //CompleteBackgroundWorker(new Guid(cmd.requestId));
                    break;
                //case eCommand.OrdersLoad:
                //    mcmd = new OrderMasterData() {
                //        Command = cmd.command,
                //        ORD_NO=((orderMaster)cmd).ORD_NO,
                //        Status = ((orderMaster)cmd).Status
                //    };
                //    Sendback((O)mcmd, cbsend);
                //    break;

                case eCommand.OrdersUpload:
                    mcmd = new OrderMasterData()
                    {
                        Command = cmd.command,
                        ORD_NO = ((orderMaster)cmd).ORD_NO,
                        Status = ((orderMaster)cmd).Status
                    };
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.OrderUpdatesComplete:
                    mreq = (manifestRequest)cmd;

                    if (mreq.valist != null)
                    {
                        foreach (var it in mreq.valist)
                        {
                            mcmd = new OrderMasterData()
                            {
                                RequestId = new Guid(cmd.requestId),
                                ORD_NO = (int)it,
                                Command = eCommand.ManifestDetailsComplete
                            };
                            Sendback((O)mcmd, cbsend);
                        }
                    }

                    break;
                case eCommand.OrderDetails:
                    mcmd = new OrderDetailsData((orderDetails)cmd);

                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.OrderDetailsComplete:
                    mreq = (manifestRequest)cmd;

                    if (mreq.valist != null)
                    {
                        foreach (var it in mreq.valist)
                        {
                            mcmd = new OrderDetailsData()
                            {
                                RequestId = new Guid(cmd.requestId),
                                ORD_NO = it,
                                LineNumber=mreq.Stop, // using as the count (number of OrderDetail Records Expected)
                                Command = eCommand.OrderDetailsComplete
                            };
                            Sendback((O)mcmd, cbsend);
                        }
                    }
                    break;
                case eCommand.OrderOptions:
                    mcmd = new OrderOptionsData((orderOptions)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.OrderOptionsComplete:
                    mreq = (manifestRequest)cmd;

                    if (mreq.valist != null)
                    {
                        foreach (var it in mreq.valist)
                        {
                            mcmd = new OrderOptionsData()
                            {
                                RequestId = new Guid(cmd.requestId),
                                ORD_NO = (int)it,
                                Count = mreq.Stop, // using as the count (number of OrderDetail Records Expected)
                                Command = eCommand.OrderOptionsComplete
                            };
                            Sendback((O)mcmd, cbsend);
                        }
                    }
                    break;
                case eCommand.ScanFile:
                    mcmd = new ScanFileData((scanFile)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.Trucks:
                    mcmd = new TruckData((trucks)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.TrucksLoadComplete:
                    mreq = (manifestRequest)cmd;
                    mcmd = new TruckData()
                    {
                        RequestId = new Guid(cmd.requestId),
                        ManifestId = mreq.id,
                        Command = eCommand.TrucksLoadComplete
                    };
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.Stops:
                    mcmd = new StopData((stops)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.StopsLoadComplete:
                    mcmd = new StopData() { Command = cmd.command, RequestId=new Guid(cmd.requestId) };
                    mreq = (manifestRequest)cmd;
                    if (mreq.valist != null)
                    {
                        foreach (var id in mreq.valist)
                        {
                            mcmd = new StopData()
                            {
                                Command = cmd.command,
                                //TruckCode = TRK_CDE,
                                RequestId = new Guid(cmd.requestId)
                            };
                            Sendback((O)mcmd, cbsend);
                        }
                    }
                    else
                    {
                        mcmd = new StopData()
                        {
                            Command = cmd.command,
                            //TruckCode = TRK_CDE,
                            RequestId = new Guid(cmd.requestId)
                        };
                        Sendback((O)mcmd, cbsend);
                    }
                    break;
                case eCommand.OrdersLoad:
                    mcmd = new OrderData((orders)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.OrdersLoadComplete:
                    mreq = (manifestRequest)cmd;
                    if (mreq.valist != null)
                    {
                        foreach (var id in mreq.valist)
                        {
                            mcmd = new OrderData()
                            {
                                Command = cmd.command,
                                DLR_NO = (int)id,
                                RequestId = new Guid(cmd.requestId)
                            };
                            Sendback((O)mcmd, cbsend);
                        }
                    }
                    else
                    {
                        Logger.Info($"OrdersLoadComplete: {cmd.ToString()}");
                        mcmd = new OrderData()
                        {
                            Command = cmd.command,
//                            DLR_NO = id,
                            RequestId = new Guid(cmd.requestId)
                        };
                        Sendback((O)mcmd, cbsend);
                    }
                        
                    break;
                case eCommand.UploadManifestComplete:
                    mcmd = new ManifestMasterData() { Command = cmd.command, RequestId = NewGuid(cmd.requestId) };
                    Sendback((O)mcmd, cbsend);
                    break;
                default:
                    Logger.Error($"Unhandled command Backgrtound worker handler {Enum.GetName(typeof(eCommand), cmd.command)}.");
                    break;
            }
        }
        public void CompleteBackgroundWorker(Guid gReqId)
        {
            if (gReqId != Guid.Empty)
                dDoneEvent[gReqId].evnt.Set();
        }

        void Sendback(O cmd, Func<byte[], Task> cbsend = null)
        {
            if (cbsend != null)
                pc?.Invoke(cmd, cbsend);
            else
                pc?.Invoke(cmd);
        }
        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        public void ReportProgress(int pct, object[] paramArgs)
        {
            bgWorker.ReportProgress(pct, paramArgs);
        }
    }
}
