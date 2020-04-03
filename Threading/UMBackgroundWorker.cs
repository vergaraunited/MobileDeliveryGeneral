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
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Threading
{
    //public delegate bool SendMsgDelegate(isaCommand cmd);

    public class UMBackgroundWorker<O> where O : IMDMMessage
    {
        BackgroundWorker bgWorker { get; set; }
        Dictionary<Guid, ManualResetEvent> dDoneEvent = new Dictionary<Guid, ManualResetEvent>();

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
                dDoneEvent[guid].Reset();
            }
            catch (Exception ex) { }
        }

        public void OnStartProcess(manifestRequest req, Request reqInfo, ProcessMsgDelegateRXRaw cbsend=null)
        {
            Init();
            Guid g = Guid.NewGuid();

            if (req.requestId != null)
                g = new Guid(req.requestId);
            else
                req.requestId = g.ToByteArray();

            object[] paramArgs = new object[] {
            req, reqInfo, cbsend };

            dDoneEvent.Add(g, new ManualResetEvent(false));

            bgWorker.RunWorkerAsync(paramArgs);
        }
        
        public  void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] parameters = e.Argument as object[];
            manifestRequest req = (manifestRequest)parameters[0];
            //cbsend = (SendMsgDelegate)parameters[2];
            try
            {
                Request reqInfo = (Request)parameters[1];
                //bRunning = true;
                switch (req.command)
                {
                    case eCommand.Drivers:
                    case eCommand.Manifest:
                    case eCommand.GenerateManifest:
                        if (!sm(req))
                            throw new Exception("UMBackgroundWorker::connectionVM.SendMsgWinsys - Failed to send Winsys Server a message");
                        break;
        
                    case eCommand.ManifestDetails:
                    case eCommand.OrderDetails:
                    case eCommand.OrderOptions:
                    case eCommand.Orders:
                    case eCommand.OrdersLoad:
                    case eCommand.Trucks:
                    case eCommand.Stops:
                        sm(req);
                        break;
                    default:
                        throw new Exception("Command not found");
                }
            }
            finally
            {
                dDoneEvent[new Guid(req.requestId)].WaitOne();
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
                    if (cmd.GetType() == typeof(manifestMaster))
                    {
                        mcmd = new ManifestMasterData((manifestMaster)cmd, ((manifestMaster)cmd).id, false);
                        Sendback((O)mcmd, cbsend);
                        //CompleteBackgroundWorker(new Guid(cmd.requestId));
                    }
                    else if (cmd.GetType() == typeof(manifestRequest))
                    {
                        manifestRequest mreq = (manifestRequest)cmd;

                        mcmd = new ManifestMasterData()
                        {
                            RequestId = new Guid(cmd.requestId),
                            LINK = mreq.id,
                            Command = eCommand.ManifestLoadComplete
                        };
                        
                        Sendback((O)mcmd, cbsend);
                    }
                        
                    break;
                case eCommand.Drivers:
                    mcmd = (IMDMMessage)new DriverData() { Command = cmd.command };
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.DriversLoadComplete:
                    mcmd = (DriverData)cmd;
                    Sendback((O)mcmd, cbsend);
                    CompleteBackgroundWorker(new Guid(cmd.requestId));
                    break;
                case eCommand.ManifestDetails:
                    mcmd = (IMDMMessage)new ManifestDetailsData() { Command = cmd.command };
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.ManifestDetailsComplete:
                    mcmd = new ManifestDetailsData((manifestDetails)cmd);
                    Sendback((O)mcmd, cbsend);
                    CompleteBackgroundWorker(new Guid(cmd.requestId));
                    break;
                case eCommand.Orders:
               // case eCommand.OrderUpdatesComplete:
                    if (cmd is MsgTypes.Command)
                    {
                        mcmd = (IMDMMessage)new OrderMasterData() { Command = cmd.command };
                        Sendback((O)mcmd, cbsend);
                    }
                    else
                    {
                        mcmd = new OrderMasterData((orderMaster)cmd);
                        Sendback((O)mcmd, cbsend);
                        CompleteBackgroundWorker(new Guid(cmd.requestId));
                    }
                    
                    break;
                case eCommand.OrderDetails:
                case eCommand.OrderDetailsComplete:
                    if (cmd is MsgTypes.Command)
                    {
                        mcmd = (IMDMMessage)new OrderDetailsData() { Command = cmd.command };
                        Sendback((O)mcmd, cbsend);
                    }
                    else
                    {
                        mcmd = new OrderDetailsData((orderDetails)cmd);
                        Sendback((O)mcmd, cbsend);
                        CompleteBackgroundWorker(new Guid(cmd.requestId));
                    }
                    break;
                case eCommand.OrderOptions:
                case eCommand.OrderOptionsComplete:
                    if (cmd is MsgTypes.Command)
                        mcmd = (IMDMMessage)new OrderOptionsData() { Command = cmd.command };
                    else
                        mcmd = new OrderOptionsData((orderOptions)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.Trucks:
                    mcmd = (IMDMMessage)new TruckData((trucks)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.TrucksLoadComplete:

                    if (cmd.GetType() == typeof(trucks))
                    {
                        mcmd = new TruckData((trucks)cmd);
                        Sendback((O)mcmd, cbsend);
                    }
                    else // if (cmd.GetType() == typeof(manifestRequest))
                    {
                        manifestRequest mreq = (manifestRequest)cmd;

                        mcmd = new TruckData()
                        {
                            RequestId = new Guid(cmd.requestId),
                            ManifestId = mreq.id,
                            Command = eCommand.TrucksLoadComplete
                        };

                        Sendback((O)mcmd, cbsend);
                    }

                    break;
                case eCommand.Stops:
                    mcmd = (IMDMMessage)new StopData((stops)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.StopsLoadComplete:
                    mcmd = (IMDMMessage)new StopData() { Command = cmd.command, RequestId=new Guid(cmd.requestId) };
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.OrdersLoad:
                    mcmd = (IMDMMessage)new OrderData((orders)cmd);
                    Sendback((O)mcmd, cbsend);
                    break;
                case eCommand.OrdersLoadComplete:
                    if (cmd.GetType() == typeof(orders))
                    {
                        mcmd = new OrderData((orders)cmd);
                        Sendback((O)mcmd, cbsend);
                    }
                    else
                    {
                        manifestRequest mreq = (manifestRequest)cmd;

                        mcmd = new OrderData()
                        {
                            RequestId = new Guid(cmd.requestId),
                            ManifestId = mreq.id,
                            Command = eCommand.OrdersLoadComplete
                        };

                        Sendback((O)mcmd, cbsend);
                    }
                    break;
                case eCommand.OrderUpdatesComplete:
                    mcmd = (IMDMMessage)new OrderData() { Command = cmd.command };
                    Sendback((O)mcmd, cbsend);
                    CompleteBackgroundWorker(new Guid(cmd.requestId));
                    break;
            }
        }
        public void CompleteBackgroundWorker(Guid gReqId)
        {
            if (gReqId != Guid.Empty)
                dDoneEvent[gReqId].Set();
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
