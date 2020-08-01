using Microsoft.IO;
using MobileDeliveryLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using MobileDeliveryGeneral.Data;
using MobileDeliveryGeneral.ExtMethods;
using static MobileDeliveryGeneral.Definitions.enums;

namespace MobileDeliveryGeneral.Definitions
{
    public class MsgTypes
    {
        #region fieldsizes
        public const ushort fldsz_Command = 1;

        public const ushort fldsz_ORD_NO = 4;
        public const ushort fldsz_LINK = 4;
        public const ushort fldsz_TRK_CDE = 1;
        public const ushort fldsz_TRK_CDE_Master = 2;

        public const ushort fldsz_Value = sizeof(short);
        public const ushort fldsz_DateTime = sizeof(long);

        public const ushort fldsz_Stop = 3;
        public const ushort fldsz_UserId = 12;
        public const ushort fldsz_DESC = 24;

        public const ushort fldsz_SHIP_DTE = 4;
        public const ushort fldsz_SHP_QTY = 2;
        public const ushort fldsz_QTY = 2;
        public const ushort fldsz_TRUCKISCLOSED = 1;

        public const ushort fldsz_GUID = 16;
        public const ushort fldsz_Message = 256;
        public const ushort fldsz_Attachment = 1056;

        public const ushort fldsz_LOADUNITS = 6;
        public const ushort fldsz_SHP_NME = 30;
        public const ushort fldsz_SHP_TEL = 12;
        public const ushort fldsz_DIR = 44;

        public const ushort CALL_SIZE = 4;
        public const ushort fldsz_OPT_TYPE = 2;
        public const ushort fldsz_OPT_NUM = 2;
        public const ushort fldsz_MDL_CNT = 2;

        public const ushort fldsz_STOCK_ID = 30;
        public const ushort fldsz_MODEL = 4;
        public const ushort fldsz_MDL_NO = 4;
        public const ushort fldsz_NOTES = 10;

        public const ushort fldsz_STOCK_CONFIGURATOR_GROUP = 45;
        public const ushort fldsz_CALL_SIZE = 10;
        public const ushort fldsz_CLR = 3;
        public const ushort fldsz_DESCOrd = 60;

        public const ushort fldsz_WIDTH = 4;
        public const ushort fldsz_HEIGHT = 4;
        public const ushort fldsz_CMT1 = 40;
        public const ushort fldsz_CMT2 = 40;
        public const ushort fldsz_SHP_DTE = 4;
        public const ushort fldsz_TRUCK = 2;
        public const ushort fldsz_FIRST_NAME = 32;
        public const ushort fldsz_LAST_NAME = 32;

        public const ushort fldsz_SHP_SEQUENCE = 2;
        public const ushort fldsz_LINK_WIN_CNT = 2;
        public const ushort fldsz_SHP_ZIP = 11;

        public const ushort fldsz_OPT_SRC = 5;

        public const ushort fldsz_RTE_CDE = 4;
        public const ushort fldsz_CUS_NME = 14;

        public const ushort fldsz_POD = 5064;

        #endregion

        static RecyclableMemoryStreamManager manager = new RecyclableMemoryStreamManager();//4, 128, 64000, true);

        [Flags]
        public enum eWalletCommand
        {
            GetBalance,
            CreateCustomerAccount,
            GetCustomerBalance,
            CreateOrder, //Escrow funds
            ConfirmDelivery, //Release funds
            Withdraw,
            SweepToCold,
            TransferToHot
        }

        [Flags]
        public enum eCommand //: byte
        {
            GenerateManifest,           // = (1 << 0),
            CheckManifest,
            CheckManifestComplete,
            Manifest,         // = (1 << 1),
            ManifestLoadComplete,       // = (1 << 2),
            UploadManifest,
            UploadManifestComplete,

            //GetOrders,                  // = (1 << 3),
            ManifestDetails,            // = (1 << 4),
            ManifestDetailsComplete,    // = (1 << 5),
            UploadManifestDetails,

            OrdersUpload,               // = (1 << 6),
            OrderUpdatesComplete,       // = (1 << 7),

            OrderDetails,               // = (1 << 8),
            OrderDetailsComplete,       // = (1 << 9),

            ScanFile,
            ScanFileComplete,

            OrderOptions,
            OrderOptionsComplete,

            DeliveryComplete,           // = (1 << 10),

            RunQuery,                   // = (1 << 11),

            Broadcast,                  // = (1 << 12),

            Ping,                       // = (1 << 13),
            Pong,                       // = (1 << 14),

            LoadFiles,                  // = (1 << 15),
            LoadFilesComplete,          // = (1 << 16),

            LoadSettings,

            Drivers,                   // = (1 << 16),
            DriversLoadComplete,

            OrdersLoad,
            Stops,
            CompleteStop,
            CompleteOrder,
            Trucks,
            StopsLoadComplete,
            TrucksLoadComplete,
            OrdersLoadComplete,
            OrderModelLoadComplete,

            WalletCommand,
            GetNewHeads,
            CreateCustomerAccount,
            GetCustomerBalance,
            CreateOrder,
            ConfirmDelivery,
            Withdraw,
            SweepToCold,
            TransferToHot,

            OrderModel,

            //Accounts Receivables / Invoicing
            AccountReceivable
            // Unknown                     // = (1 << 17)
        };

        public enum OrderStatus : byte { New = 0, Shipped = 1, Delivered = 2, BackOrderd = 3 };
        public enum TruckManifestStatus : byte {  New=0, Shipped, Delivered}
        //  public static OrderStatus Status { get { return (OrderStatus); } }
        //public interface isaWalletCommand
        //{
        //    eWalletCommand command { get; set; }
        //    byte[] ToArray();
        //    isaWalletCommand FromArray(byte[] bytes);
        //    byte[] requestId { get; set; }
        //}
        public interface isaCommand
        {
            eCommand command { get; set; }
            byte[] ToArray();
            isaCommand FromArray(byte[] bytes);
            byte[] requestId { get; set; }
        }
        
        /// <summary>
        /// Admin Struct for table descriptions
        /// </summary>
        /// 
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TableDescription
        {
            [MarshalAs(UnmanagedType.ByValArray)]
            public List<Field> Fields;
            public string Tablename;
        }

        public static Guid NewGuid(byte[] byteArr, bool bFlipit=false)
        {
            byte[] guidbytes = new byte[16];
            try
            {
                if (BitConverter.IsLittleEndian && bFlipit)
                    guidbytes = FlipGuid(byteArr);
                else
                {
                    Buffer.BlockCopy(byteArr, 0, guidbytes, 0, 4);
                    Buffer.BlockCopy(byteArr, 4, guidbytes, 4, 2);
                    Buffer.BlockCopy(byteArr, 6, guidbytes, 6, 2);

                    guidbytes[9] = byteArr[9];
                    guidbytes[10] = byteArr[10];
                    guidbytes[11] = byteArr[11];
                    guidbytes[12] = byteArr[12];
                    guidbytes[13] = byteArr[13];
                    guidbytes[14] = byteArr[14];
                    guidbytes[15] = byteArr[15];
                }
                return new Guid(guidbytes);
            }
            catch
            {
                return Guid.NewGuid();
            }
        }
        public static Guid NewGuid()
        {
            var newG = Guid.NewGuid();
            var byteArr = newG.ToByteArray();
            return NewGuid(byteArr, true);
        }

        public static byte[] FlipGuid(byte[] byteArr)
        {
            byte[] guidbytes = new byte[16];
            try
            {
                byte b0 = byteArr[0];
                byte b1 = byteArr[1];
                byte b2 = byteArr[2];
                byte b3 = byteArr[3];
                byte b4 = byteArr[4];
                byte b5 = byteArr[5];
                byte b6 = byteArr[6];
                byte b7 = byteArr[7];
                
                guidbytes[0] = b7;
                guidbytes[1] = b6;
                guidbytes[2] = b5;
                guidbytes[3] = b4;
                guidbytes[4] = b3;
                guidbytes[5] = b2;
                guidbytes[6] = b1;
                guidbytes[7] = b0;

                guidbytes[9] = byteArr[9];
                guidbytes[10] = byteArr[10];
                guidbytes[11] = byteArr[11];
                guidbytes[12] = byteArr[12];
                guidbytes[13] = byteArr[13];
                guidbytes[14] = byteArr[14];
                guidbytes[15] = byteArr[15];
                Logger.Debug($"Flipped Hexadecimal byte Array");

                return guidbytes;
            }
            catch
            {
                return byteArr;
            }
        }
        public struct Field
        {
            public String Name { get; set; }
            public int Index { get; set; }
            public String Type { get; set; }
            public int Offset { get; set; }
            public int RecCnt { get; set; }
        }


        //public class WalletCommand : isaWalletCommand
        //{
        //    public eWalletCommand command { get; set; }
        //    public byte[] requestId { get; set; }
        //    public string value { get; set; }

        //    public isaWalletCommand FromArray(byte[] bytes)
        //    {

        //        var s = new WalletCommand();
        //        try
        //        {
        //            using (var reader = new BinaryReader(new MemoryStream(bytes)))
        //            {
        //                if (bytes.Length >= 1)
        //                    s.command = (eWalletCommand)reader.ReadByte();

        //                s.requestId = reader.ReadBytes(fldsz_GUID);

        //                Logger.Debug($"isaWalletCommand::FromArrray {s.ToString()}");

        //                if (bytes.Length >= 2 && s.command == eWalletCommand.GetBalance)
        //                    s.value = reader.ReadString();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Error($"isaWalletCommand::FromArrray - Error Converting incoming socket message to iCommand./n {ex.Message}");
        //        }
        //        return s;
        //    }

        //    public byte[] ToArray()
        //    {
        //        using (var stream = manager.GetStream())
        //        using (var writer = new BinaryWriter(stream))
        //        {
        //            writer.Write((byte)command);

        //            if (requestId == null)
        //                requestId = Guid.Empty.ToByteArray();

        //            writer.Write(NewGuid(requestId).ToByteArray());
        //            if (value != null)
        //                writer.Write(value);

        //            return stream.ToArray();
        //        }
        //    }
        //}
        /// <summary>
        /// Manifest Load
        /// </summary>
        public class Command : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }
            public string value { get; set; }
            public byte[] ToArray()
            {
                //reuse the memory streams

                using (var stream = manager.GetStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);

                    if (requestId == null)
                        requestId = Guid.Empty.ToByteArray();

                    writer.Write(NewGuid(requestId).ToByteArray());
                    if (value != null)
                        writer.Write(value);

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                var s = new Command();
                try
                {
                    using (var reader = new BinaryReader(new MemoryStream(bytes)))
                    {
                        if (bytes.Length >= 1)
                            s.command = (eCommand)reader.ReadByte();

                        s.requestId = reader.ReadBytes(fldsz_GUID);

                        Logger.Debug($"isaCommand::FromArrray {s.ToString()}");

                        if (bytes.Length >= 2 && s.command == eCommand.Broadcast)
                            s.value = reader.ReadString();
                    }
                }
                catch(Exception ex)
                {
                    Logger.Error($"isaCommand::FromArrray - Error Converting incoming socket message to iCommand./n {ex.Message}");
                }
                return s;
            }
            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{value + Environment.NewLine}";
            }
        }

        /// <summary>
        /// Manifest Load
        /// </summary>
        /// 
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct manifestRequest : isaCommand
        {
            static eCommand cmd = eCommand.GenerateManifest;

            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public string date;
            public long id;
            public long DATA;
            public byte[] bData;
            public byte[] TRK_CDE; //2 bytes = short = each byte has 8 possible unique id's, so 16 possible TruckCodes - fldsz_TruckCode
            public int Stop; // 3 bytes - 32 possible Stops - fldsz_Stop
            public List<long> valist;

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(NewGuid(requestId).ToByteArray());
                    DateTime dt;
                    if (date == null)
                        writer.Write(DateTime.Today.Date.ToBinary());
                    else if (DateTime.TryParse(date, out dt))
                        writer.Write(dt.Date.ToBinary());
                    else
                        writer.Write(DateTime.Today.Date.ToBinary());
                    writer.Write(id);
                    writer.Write(DATA);
                    if (bData != null && bData.Length > 0)
                    {
                        writer.Write(bData.Length);
                        writer.Write(bData);
                    }
                    else writer.Write(0);

                    if (TRK_CDE == null)
                        TRK_CDE = new byte[fldsz_TRK_CDE];
                    else
                        Logger.Debug("Size of TRK_CDE = " + TRK_CDE.Length);
                    writer.Write(TRK_CDE);

                    writer.Write(Stop);
                    if (valist != null)
                        writer.Write(valist.WriteList());

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {

                    try
                    {
                        command = (eCommand)reader.ReadByte();
                        requestId = reader.ReadBytes(fldsz_GUID);
                        long lDate = reader.ReadInt64();
                        try
                        {
                            if (lDate == 0)
                                date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                            else
                                date = DateTime.FromBinary(lDate).Date.ToString("yyyy-MM-dd");
                        }
                        catch (Exception ex)
                        {
                            date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                        }
                        id = reader.ReadInt64();
                        DATA = reader.ReadInt64();
                        int szData = reader.ReadInt32();
                        bData = reader.ReadBytes(szData);
                        TRK_CDE = reader.ReadBytes(fldsz_TRK_CDE);
                        Stop = reader.ReadInt32();

                        if (reader.BaseStream.Length > reader.BaseStream.Position)
                            valist = ExtensionMethods.BytesToList(reader.ReadBytes((int)(reader.BaseStream.Length - (reader.BaseStream.Position - 1))));

                        return this;

                    }
                    catch (Exception ex) { Logger.Error($"Error manifestRequest (FromArray) ex= {ex.Message}"); }
                }
                return this;
            }

            public override string ToString()
            {
                string mm = "Null bData";
                string valst = "Null valist";
                string trkcode = "Null truckCode";
                if (bData != null && bData.Length > 0)
                {
                    try
                    {
                        mm = new manifestMaster().FromArray(bData).ToString() + Environment.NewLine;
                    }
                    catch (Exception ex) { Logger.Error($"bData.FromArray, not a Command"); }
                }

                if (valist != null)
                    valst = string.Join(",", valist.ToArray());
                if (TRK_CDE != null)
                    trkcode = TRK_CDE.UMToString(fldsz_TRK_CDE);

                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}\t\tReqId:{ NewGuid(requestId).ToString()} {Environment.NewLine}" +
                    $"\t\tDate:{date}{Environment.NewLine}\t\tId:{id.ToString()}{Environment.NewLine}\t\t{DATA.ToString()}{Environment.NewLine}\t\t{mm}{Environment.NewLine}" +
                    $"\t\t{trkcode + Environment.NewLine}\t\t{Stop + Environment.NewLine + valst}";
            }
        }

        /// Manifest Load
        public class manifestMaster : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public long id;
            public int DriverId;
            public long LINK;
            public byte[] TRK_CDE; // Its 2 bytes in this master table, confusing!
            public string DESC;
            public string NOTES;
            public long SHIP_DTE;
            public short SHP_QTY;
            public byte TRUCKISCLOSED;

            public decimal TOT_MILES;//4
            public decimal CST_MILE; //4
            public decimal CST_UNIT; //4
            public TruckManifestStatus Status { get; set; }
            public manifestMaster() { }

            public manifestMaster(ManifestMasterData mmd)
            {
                try
                {
                    command = mmd.Command;

                    Logger.Debug($"{mmd.RequestId}");
                    requestId = mmd.RequestId.ToByteArray();

                    //if (BitConverter.IsLittleEndian)
                    //{
                    //    Logger.Info($"manifestMaster Requestid is Little Endian, Flip the bytes: {requestId}");
                    //    var bytes = mmd.RequestId.ToByteArray();
                    //    foreach (var byt in bytes)
                    //        Logger.Debug($"{byt:X2} ");
                    //    requestId = FlipGuid(bytes);
                    //    Logger.Info($"manifestMaster Requestid is Little Endian, flipped result: {requestId}");
                    //}
                    //else
                    //    Logger.Info($"manifestMaster Requestid is Big Endian, proceed: {requestId}");

                    id = mmd.ManifestId;
                    DriverId = Int32.Parse(mmd.Userid);
                    LINK = mmd.LINK;
                    TRK_CDE = mmd.TRK_CDE.GetBytes();
                    DESC = mmd.Desc;
                    NOTES = mmd.NOTES;

                    SHIP_DTE = (long)mmd.SHIP_DTE.FromGregorianToJulian();
                    SHP_QTY = mmd.SHP_QTY;
                    TRUCKISCLOSED = Convert.ToByte(mmd.TRUCKISCLOSED);
                    Status = mmd.Status;
                }
                catch (Exception ex) { Logger.Error($"maniFestMaster Error:  {ex.Message}"); }
            }
            public override string ToString()
            {
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{NewGuid(requestId).ToString() + Environment.NewLine}" +
                    $"\t\t{id.ToString() + Environment.NewLine}" +
                    $"\t\t{DriverId.ToString() + Environment.NewLine}" +
                    $"\t\t{LINK.ToString() + Environment.NewLine}" +
                    $"\t\t{TRK_CDE.UMToString(fldsz_TRK_CDE_Master) + Environment.NewLine}" +
                    $"\t\t{DESC.ToString() + Environment.NewLine}" +
                    $"\t\t{NOTES.ToString() + Environment.NewLine}" +
                    $"\t\t{SHIP_DTE.ToString() + Environment.NewLine}" +
                    $"\t\t{SHP_QTY.ToString() + Environment.NewLine}" +
                    $"\t\t{TRUCKISCLOSED.ToString() + Environment.NewLine}";
            }
            public byte[] ToArray()
            {
                try
                {
                    using (var stream = manager.GetStream())
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write((byte)command);
                        writer.Write(NewGuid(requestId).ToByteArray());

                        Logger.Debug($"manifestMaster ToArray reqIdBytes: {requestId}");
                        
                        //var bytes = requestId;
                        //foreach (var byt in bytes)
                        //    Logger.Debug($"{byt:X2} ");

                        Logger.Debug($"manifestMaster ToArray reqIdString: {NewGuid(requestId).ToString()}");
                        writer.Write(id);
                        writer.Write(DriverId);
                        writer.Write(LINK);

                        if (TRK_CDE == null)
                            TRK_CDE = new byte[fldsz_TRK_CDE_Master];
                        else
                            Logger.Debug("Size of TRK_CDE = " + TRK_CDE.Length);
                        writer.Write(TRK_CDE);

                        Logger.Debug("Size of DESC = " + DESC.Length);
                        writer.Write(DESC);

                        Logger.Debug("Size of Notes " + NOTES.Length);
                        writer.Write(NOTES);

                        writer.Write(SHIP_DTE);
                        writer.Write(SHP_QTY);
                        writer.Write(TRUCKISCLOSED);
                        writer.Write((Int16)Status);
                        return stream.ToArray();
                    }
                }catch(Exception ex){ Logger.Error($"manifestMaster ToArray {ex.Message}"); }
                return null;
            }

            public isaCommand FromArray(byte[] bytes)
            {
                try
                { 
                    using (var reader = new BinaryReader(new MemoryStream(bytes)))
                    {
                        if (bytes.Length >= fldsz_Command)
                            command = (eCommand)reader.ReadByte();
                        requestId = reader.ReadBytes(fldsz_GUID);
                        //Logger.Debug($"manifestMaster FromArray reqIdBytes: {requestId}");
                        Logger.Debug($"manifestMaster FromArray reqIdString: {NewGuid(requestId).ToString()}");
                        id = reader.ReadInt64();
                        DriverId = reader.ReadInt32();
                        LINK = reader.ReadInt64();
                        TRK_CDE = reader.ReadBytes(fldsz_TRK_CDE_Master);
                        DESC = reader.ReadString();
                        NOTES = reader.ReadString();
                        SHIP_DTE = reader.ReadInt64();
                        SHP_QTY = reader.ReadInt16();
                        TRUCKISCLOSED = reader.ReadByte();
                        Status = (TruckManifestStatus) reader.ReadInt16();
                     return this;
                    }
                }
                catch (Exception ex) { Logger.Error($"Error manifestMaster (FromArray) ex= {ex.Message}"); }
                return this;
            }

        }
        public class drivers : isaCommand
        {
            public eCommand command { get; set; } = eCommand.Drivers;
            public byte[] requestId { get; set; }

            public int DriverId;
            public string FirstName;
            public string LastName;
               
            public drivers() { }
            public drivers(DriverData mmd)
            {
                command = mmd.Command;
                requestId = mmd.RequestId.ToByteArray();
                DriverId = mmd.DriverId;
                FirstName = mmd.FirstName;
                LastName = mmd.LastName;
            }

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(DriverId);
                    writer.Write(FirstName);
                    writer.Write(LastName);
                    return stream.ToArray();
                }
            }
            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    if (bytes.Length >= fldsz_Command)
                        command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    DriverId = reader.ReadInt32();
                    FirstName = reader.ReadString();
                    LastName = reader.ReadString();
                  
                    return this;
                }
            }
            public override string ToString()
            {
                return $"{Environment.NewLine}\t{Enum.GetName(typeof(eCommand), command) + Environment.NewLine}"+
                    $"\t\t{NewGuid(requestId).ToString() + Environment.NewLine}" +
                    $"\t\t{DriverId.ToString() + Environment.NewLine}" +
                    $"\t\t{FirstName.ToString() + Environment.NewLine}" +
                    $"\t\t{LastName.ToString() + Environment.NewLine}";
            }
        }

        public class trucks : isaCommand
        {
            public eCommand command { get; set; } = eCommand.Trucks;
            public byte[] requestId { get; set; }

            public long ManifestId;
            public int DriverId;
            public string FirstName;
            public string LastName;
            public string TruckCode;
            public long ShipDate;
            public string Description;
            public string Notes;
            public bool IsClosed;

            public trucks() { }

            public trucks(TruckData td)
            {
                command = td.Command;
                requestId = td.RequestId.ToByteArray();
                ManifestId = td.ManifestId;
                DriverId = td.DriverId;
                FirstName = td.FirstName;
                LastName = td.LastName;
                TruckCode = td.TRK_CDE;
                ShipDate = td.SHIP_DTE;
                Description = td.Desc;
                Notes = td.NOTES;
                IsClosed = td.IsClosed;
            }
            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(ManifestId);
                    writer.Write(DriverId);
                    writer.Write(FirstName);
                    writer.Write(LastName);
                    writer.Write(TruckCode);
                    writer.Write(ShipDate);
                    writer.Write(Description);
                    writer.Write(Notes);
                    writer.Write(IsClosed);

                    return stream.ToArray();
                }
            }
            public override string ToString()
            {
                return $"{Environment.NewLine}\t{Enum.GetName(typeof(eCommand), command) + Environment.NewLine}" +
                    $"\t\t{NewGuid(requestId).ToString() + Environment.NewLine}" +
                    $"\t\t{ManifestId.ToString() + Environment.NewLine}" +
                    $"\t\t{DriverId.ToString() + Environment.NewLine}" +
                    $"\t\t{FirstName.ToString() + Environment.NewLine}" +
                    $"\t\t{LastName.ToString() + Environment.NewLine}" +
                    $"\t\t{TruckCode.ToString() + Environment.NewLine}" +
                    $"\t\t{ShipDate.ToString() + Environment.NewLine}" +
                    $"\t\t{Description.ToString() + Environment.NewLine}" +
                    $"\t\t{Notes.ToString() + Environment.NewLine}";
            }
            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    if (bytes.Length >= fldsz_Command)
                        command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    ManifestId = reader.ReadInt64();
                    DriverId = reader.ReadInt32();
                    FirstName = reader.ReadString();
                    LastName = reader.ReadString();
                    TruckCode = reader.ReadString();
                    ShipDate = reader.ReadInt64();
                    Description = reader.ReadString();
                    Notes = reader.ReadString();
                    IsClosed = reader.ReadBoolean();

                    return this;
                }
            }
        }

        public class stops : isaCommand
        {
            public eCommand command { get; set; } = eCommand.Stops;
            public byte[] requestId { get; set; }

            public long ManifestId;
            public int DisplaySeq;
            public long DealerNo;
            public string DealerName;
            public string CustomerName;

            public string Address;
            public string PhoneNumber;
            public string Description;
            public string Notes;
            public string TRK_CDE;
            public int CustomerId;
            public bool BillComplete;
            public DateTime ScanDateTime;

            public stops() { }
            public stops(StopData st)
            {
                command = st.Command;
                requestId = st.RequestId.ToByteArray();
                ManifestId = st.ManifestId;
                DisplaySeq = st.DisplaySeq;
                DealerNo = st.DealerNo;
                DealerName = st.DealerName;
                Address = st.Address;
                PhoneNumber = st.PhoneNumber;
                Description = st.Description;
                Notes = st.Notes;
                TRK_CDE = st.TruckCode;
                CustomerId = st.CustomerId;
                BillComplete = st.BillComplete;
                ScanDateTime = st.ScanDateTime;
            }
            public override string ToString()
            {
                var sReqId = "nullReqId";
                string sBillComplete = "False";
                if (BillComplete)
                    sBillComplete = "True";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();

                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ManifestId + Environment.NewLine}" +
                     $"\t\t{DisplaySeq + Environment.NewLine}" +
                      $"\t\t{DealerNo + Environment.NewLine}" +
                       $"\t\t{DealerName + Environment.NewLine}" +
                        $"\t\t{Address + Environment.NewLine}" +
                         $"\t\t{PhoneNumber + Environment.NewLine}" +
                         $"\t\t{Description + Environment.NewLine}" +
                         $"\t\t{Notes + Environment.NewLine}" +
                         $"\t\t{TRK_CDE + Environment.NewLine}" +
                         $"\t\t{CustomerId + Environment.NewLine}" +
                         $"\t\t{sBillComplete + Environment.NewLine}";
            }
            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(ManifestId);
                    writer.Write(DisplaySeq);
                    writer.Write(DealerNo);

                    if (DealerName == null)
                        DealerName = "";
                    writer.Write(DealerName);

                    if (Address == null)
                        Address = "";
                    writer.Write(Address);

                    if (PhoneNumber == null)
                        PhoneNumber = "";
                    writer.Write(PhoneNumber);

                    if (Description == null)
                        Description = "";
                    writer.Write(Description);

                    if (Notes == null)
                        Notes = "";
                    writer.Write(Notes);

                    if (TRK_CDE == null)
                        TRK_CDE = "";
                    writer.Write(TRK_CDE);

                    writer.Write(CustomerId);
                    writer.Write(BillComplete);
                    writer.Write(ScanDateTime.ToBinary());

                    return stream.ToArray();
                }
            }
            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    //var s = new manifestMaster();
                    try
                    {
                        if (bytes.Length >= fldsz_Command)
                        command = (eCommand)reader.ReadByte();
                        requestId = reader.ReadBytes(fldsz_GUID);
                        ManifestId = reader.ReadInt64();
                        DisplaySeq = reader.ReadInt32();
                        DealerNo = reader.ReadInt64();
                        DealerName = reader.ReadString();
                        Address = reader.ReadString();
                        PhoneNumber = reader.ReadString();
                        Description = reader.ReadString();
                        Notes = reader.ReadString();
                        TRK_CDE = reader.ReadString();
                        CustomerId = reader.ReadInt32();
                        BillComplete = reader.ReadBoolean();
                
                        ScanDateTime = DateTime.FromBinary(reader.ReadInt64());
                    }
                    catch (Exception ex) { }
                    return this;
                }
            }
        }

        public class stopOrders : isaCommand
        {
            public eCommand command { get; set; } = eCommand.OrdersLoad;
            public byte[] requestId { get; set; }
            public Int32 ORD_NO { get; set; }
            public Int32 DLR_NO { get; set; }

            public isaCommand FromArray(byte[] bytes)
            {
                throw new NotImplementedException();
            }

            public byte[] ToArray()
            {
                throw new NotImplementedException();
            }
        }
        public class orders : isaCommand
        {
            public eCommand command { get; set; } = eCommand.OrdersLoad;
            public byte[] requestId { get; set; }

            public Int32 ORD_NO { get; set; }
            public Int32 DLR_NO { get; set; }
            public String DLR_PO { get; set; }
            public Int32 ORD_DTE { get; set; }
            public Int32 SHP_DTE { get; set; }
            public Int32 SHIP_DTE { get; set; }
            public String CMNT1 { get; set; }
            public String CMNT2 { get; set; }
            public String DLR_NME { get; set; }
            public String DLR_ADDR { get; set; }
            public String DLR_ADDR2 { get; set; }
            public String DLR_CSZ { get; set; }
            public Int16 DLR_CT { get; set; }
            public String SHP_NME { get; set; }
            public String SHP_ADDR { get; set; }
            public String SHP_ADDR2 { get; set; }
            public String SHP_CSZ { get; set; }
            public String SHP_TEL { get; set; }
            public Int16 SHP_CT { get; set; }
            public String SHP_ZIP { get; set; }
            public String CUS_NME { get; set; }
            public String CUS_ADDR { get; set; }
            public String CUS_CSZ { get; set; }
            public String CUS_TEL { get; set; }
            public String RTE_CDE { get; set; }
            public String ORD_TYPE { get; set; }
            public Decimal DEPOSIT { get; set; }
            public String ENT_BY { get; set; }
            public String HLD_FLG { get; set; }
            public String HLD_BY { get; set; }
            public String HLD_MSG { get; set; }
            public Decimal ORD_AMT { get; set; }
            public Int16 WIN_QTY { get; set; }
            public Int16 PAR_QTY { get; set; }
            public Int16 STK_QTY { get; set; }
            public Int16 CMP_QTY { get; set; }
            public Int16 SHP_QTY { get; set; }
            public Decimal SHP_AMT { get; set; }
            public String MISC_TEXT { get; set; }

            public long ManifestId;
            public byte Status;

            public orderDisplayDetails ordDetails = new orderDisplayDetails();
            public orders() {
               // ordDetails = new orderDetails();
            }
            public orders(OrderData st)
            {
                command = st.Command;
                requestId = st.RequestId.ToByteArray();
                ManifestId = st.ManifestId;
                //DSP_SEQ = st.DSP_SEQ;
                //CustomerId = st.CustomerId;
                DLR_NO = st.DLR_NO;
                DLR_PO = st.DLR_PO;
                ORD_NO = st.ORD_NO;
                SHP_DTE = st.SHP_DTE;
                //CLR = st.CLR;
                //MDL_CNT = st.MDL_CNT;
                // MDL_NO = Int32.Parse(st.MDL_NO);
                //MDL_NO = st.MDL_NO;
                //WIN_CNT = st.WIN_CNT;
                Status = (byte)st.Status;
                //HEIGHT = st.HEIGHT;
                //WIDTH = st.WIDTH;
                //ordDetails =;

                //ordDetails.BIN = st.
            }

            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ManifestId + Environment.NewLine}" +
                     //$"\t\t{DSP_SEQ + Environment.NewLine}" +
                     // $"\t\t{CustomerId + Environment.NewLine}" +
                       $"\t\t{DLR_NO + Environment.NewLine}" +
                        $"\t\t{ORD_NO + Environment.NewLine} " +
                $"\t\t{SHP_DTE + Environment.NewLine} " ; 
            }
            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(ManifestId);

                    writer.Write(ORD_NO);
                    writer.Write(DLR_NO);
                    writer.Write(DLR_PO);
                    writer.Write(ORD_DTE);
                    writer.Write(SHP_DTE);
                    writer.Write(SHIP_DTE);
                    writer.Write(CMNT1.ToStringExNull());
                    writer.Write(CMNT2.ToStringExNull());
                    writer.Write(DLR_NME.ToStringExNull());
                    writer.Write(DLR_ADDR.ToStringExNull());
                    writer.Write(DLR_ADDR2.ToStringExNull());
                    writer.Write(DLR_CSZ.ToStringExNull());
                    writer.Write(DLR_CT);
                    writer.Write(SHP_NME.ToStringExNull());
                    writer.Write(SHP_ADDR.ToStringExNull());
                    writer.Write(SHP_ADDR2.ToStringExNull());
                    writer.Write(SHP_CSZ.ToStringExNull());
                    writer.Write(SHP_TEL.ToStringExNull());
                    writer.Write(SHP_CT);
                    writer.Write(SHP_ZIP.ToStringExNull());
                    writer.Write(CUS_NME.ToStringExNull());
                    writer.Write(CUS_ADDR.ToStringExNull());
                    writer.Write(CUS_CSZ.ToStringExNull());
                    writer.Write(CUS_TEL.ToStringExNull());
                    writer.Write(RTE_CDE.ToStringExNull());
                    // writer.Write(ORD_TYPE);
                    //writer.Write(DEPOSIT);
                    writer.Write(ENT_BY.ToStringExNull());
                    writer.Write(HLD_FLG);
                    writer.Write(HLD_BY);
                    writer.Write(HLD_MSG);
                    writer.Write(ORD_AMT);
                    writer.Write(WIN_QTY);
                    writer.Write(PAR_QTY);
                    writer.Write(STK_QTY);
                    writer.Write(CMP_QTY);
                    writer.Write(SHP_QTY);
                    writer.Write(SHP_AMT);
                    writer.Write(MISC_TEXT.ToStringExNull());

                    ordDetails.ToArray(writer);
                   

                    return stream.ToArray();
                }
            }
            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    if (bytes.Length >= fldsz_Command)
                        command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    ManifestId = reader.ReadInt64();
                    ORD_NO = reader.ReadInt32();
                    DLR_NO = reader.ReadInt32();
                    DLR_PO = reader.ReadString();
                    ORD_DTE = reader.ReadInt32();
                    SHP_DTE = reader.ReadInt32();
                    SHIP_DTE = reader.ReadInt32();
                    CMNT1 = reader.ReadString();
                    CMNT2 = reader.ReadString();
                    DLR_NME = reader.ReadString();
                    DLR_ADDR = reader.ReadString();
                    DLR_ADDR2 = reader.ReadString();
                    DLR_CSZ = reader.ReadString();
                    DLR_CT = reader.ReadInt16();
                    SHP_NME = reader.ReadString();
                    SHP_ADDR = reader.ReadString();
                    SHP_ADDR2 = reader.ReadString();
                    SHP_CSZ = reader.ReadString();
                    SHP_TEL = reader.ReadString();
                    SHP_CT = reader.ReadInt16();
                    SHP_ZIP = reader.ReadString();
                    CUS_NME = reader.ReadString();
                    CUS_ADDR = reader.ReadString();
                    CUS_CSZ = reader.ReadString();
                    CUS_TEL = reader.ReadString();
                    RTE_CDE = reader.ReadString();
                    //ORD_TYPE = reader.ReadString();
                    //DEPOSIT = reader.ReadDecimal();
                    ENT_BY = reader.ReadString();
                    HLD_FLG = reader.ReadString();
                    HLD_BY = reader.ReadString();
                    HLD_MSG = reader.ReadString();
                    ORD_AMT = reader.ReadDecimal();
                    WIN_QTY = reader.ReadInt16();
                    PAR_QTY = reader.ReadInt16();
                    STK_QTY = reader.ReadInt16();
                    CMP_QTY = reader.ReadInt16();
                    SHP_QTY = reader.ReadInt16();
                    SHP_AMT = reader.ReadDecimal();
                    MISC_TEXT = reader.ReadString();
                    ordDetails = (orderDisplayDetails)new orderDisplayDetails().FromArray(reader);

                    return this;
                }
            }
        }

        public class manifestDetails : isaCommand
        {
            public eCommand command { get; set; } = eCommand.ManifestDetails;
            public byte[] requestId { get; set; }

            public byte[] LINK;
            public byte DEL_SEQ;
            public byte DSP_SEQ;
            public byte EXTRA_STOP;
            public byte[] LOADUNITS;   //6 bytes
            public short UNITSONTRUCK; 
            public long DLR_NO;
            public string SHP_NME;  //30 bytes
            public string SHP_ADDR; //30
            public string SHP_ADDR2;//30
            public string SHP_CSZ;  //30
            public string SHP_TEL;  //12
            public string DIR_1;  //44  
            public string DIR_2;
            public string DIR_3;
            public string DIR_4;
            public long ManId;
            public manifestDetails() { }
            public manifestDetails(ManifestDetailsData mdd)
            {

                command=mdd.Command;
                requestId = mdd.RequestId.ToByteArray(); 

                LINK=mdd.LINK.GetBytes();
                LOADUNITS=mdd.LOADUNITS.GetBytes();   //6 bytes
                UNITSONTRUCK=mdd.UNITSONTRUCK;
                DLR_NO=mdd.DLR_NO;
                SHP_NME=mdd.SHP_NME;  //30 bytes
                SHP_ADDR=mdd.SHP_ADDR; //30
                SHP_ADDR2=mdd.SHP_ADDR2;//30
                SHP_CSZ=mdd.SHP_CSZ;  //30
                SHP_TEL=mdd.SHP_TEL;  //12
                DIR_1=mdd.DIR_1;  //44  
                DIR_2=mdd.DIR_2;
                DIR_3=mdd.DIR_3;
                DIR_4 = mdd.DIR_4;
                ManId = mdd.ManId;
            }

            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{LINK + Environment.NewLine}" +
                    $"\t\t{LOADUNITS + Environment.NewLine}" +
                    $"\t\t{UNITSONTRUCK + Environment.NewLine}" +
                    $"\t\t{DLR_NO + Environment.NewLine}" +
                    $"\t\t{SHP_NME + Environment.NewLine}" +
                    $"\t\t{SHP_ADDR + Environment.NewLine}" +
                    $"\t\t{SHP_ADDR2 + Environment.NewLine}" +
                    $"\t\t{SHP_CSZ + Environment.NewLine}" +
                    $"\t\t{SHP_TEL + Environment.NewLine}" +
                    $"\t\t{DIR_1 + Environment.NewLine}" +
                    $"\t\t{DIR_2 + Environment.NewLine}" +
                    $"\t\t{DIR_3 + Environment.NewLine}" +
                    $"\t\t{DIR_4 + Environment.NewLine}" +
                    $"\t\t{ManId + Environment.NewLine}";
            }
            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    var writer = new BinaryWriter(stream);
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(LINK);
                    writer.Write(DEL_SEQ);
                    writer.Write(DSP_SEQ);
                    writer.Write(EXTRA_STOP);

                    if (LOADUNITS == null)
                        LOADUNITS = new byte[fldsz_LOADUNITS];
                    writer.Write(LOADUNITS);   //6 bytes
                    writer.Write(UNITSONTRUCK);

                    //KEY <Group> (loc.15)
                    writer.Write(DLR_NO);
                    writer.Write(SHP_NME);  //30 bytes
                    writer.Write(SHP_ADDR); //30
                    writer.Write(SHP_ADDR2); //30
                    writer.Write(SHP_CSZ); //30
                    writer.Write(SHP_TEL); //12
                    //DIR_GRP <Group> (loc.151)
                    writer.Write(DIR_1);   //44
                    writer.Write(DIR_2);   //44
                    writer.Write(DIR_3);//44
                    writer.Write(DIR_4);  //44
                    writer.Write(ManId);

                    return stream.ToArray();
                }
            }


            public isaCommand FromArray(byte[] bytes)
            {
              //  var s = new manifestDetails();

                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    LINK = reader.ReadBytes(fldsz_LINK);
                    DEL_SEQ = reader.ReadByte();
                    DSP_SEQ = reader.ReadByte();
                    EXTRA_STOP = reader.ReadByte();
                    LOADUNITS = reader.ReadBytes(fldsz_LOADUNITS);   //6 bytes
                    UNITSONTRUCK = reader.ReadInt16();
                    //KEY <Group> (loc.15)
                    DLR_NO = reader.ReadInt64();
                    SHP_NME = reader.ReadString();  //30 bytes
                    SHP_ADDR = reader.ReadString(); //30
                    SHP_ADDR2 = reader.ReadString(); //30
                    SHP_CSZ = reader.ReadString();//30
                    SHP_TEL = reader.ReadString(); //12
                    //DIR_GRP <Group> (loc.151)
                    DIR_1 = reader.ReadString();   //44  
                    DIR_2 = reader.ReadString();   //44  
                    DIR_3 = reader.ReadString();   //44  
                    DIR_4 = reader.ReadString();    //44
                    ManId = reader.ReadInt64();
                };
                return this;
            }

        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class orderMaster : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }
            public Int32 ORD_NO { get; set; }
            public Int32 DLR_NO { get; set; }
            public String DLR_PO { get; set; }
            public Int32 ORD_DTE { get; set; }
            public Int32 SHIP_DTE { get; set; }
            public Int32 SHP_DTE { get; set; }
            public String CMNT1 { get; set; }
            public String CMNT2 { get; set; }
            public String DLR_NME { get; set; }
            public String DLR_ADDR { get; set; }
            public String DLR_ADDR2 { get; set; }
            public String DLR_CSZ { get; set; }
            public String DLR_TEL { get; set; }
            public Int16 DLR_CT { get; set; }
            public String DIR_1 { get; set; }
            public String DIR_2 { get; set; }
            public String DIR_3 { get; set; }
            public String DIR_4 { get; set; }
            public String SHP_NME { get; set; }
            public String SHP_ADDR { get; set; }
            public String SHP_ADDR2 { get; set; }
            public String SHP_CSZ { get; set; }
            public String SHP_TEL { get; set; }
            public Int16 SHP_CT { get; set; }
            public String SHP_ZIP { get; set; }
            public String CUS_NME { get; set; }
            public String CUS_ADDR { get; set; }
            public String CUS_CSZ { get; set; }
            public String CUS_TEL { get; set; }
            public String RTE_CDE { get; set; }
            public Decimal DEPOSIT { get; set; }
            public String ENT_BY { get; set; }
            public String HLD_FLG { get; set; }
            public String HLD_BY { get; set; }
            public String HLD_MSG { get; set; }
            public Int32 DTE_ENT { get; set; }
            public Decimal ORD_AMT { get; set; }
            public Int16 WIN_QTY { get; set; }
            public Int16 PAR_QTY { get; set; }
            public Int16 STK_QTY { get; set; }
            public Int16 CMP_QTY { get; set; }
            public Int16 SHP_QTY { get; set; }
            public Decimal SHP_AMT { get; set; }
            public String MISC_TEXT { get; set; }
            public OrderStatus Status;
            public long ManId;
            public DateTime SCAN_DATE_TIME { get; set; }

            public status status;

           // public orderOptions ordOpt=new orderOptions();
            #region disabled fields
            //public int ScanFileCount { get{ return ScanFile.Count; }  }
            //public List<scanFile> ScanFile { get; set; } = new List<scanFile>();
            #endregion
            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();

                return $"Command:{Enum.GetName(typeof(eCommand), command) + Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ORD_NO + Environment.NewLine}" +
                    $"\t\t{DLR_NO + Environment.NewLine}" +
                    $"\t\t{DLR_PO + Environment.NewLine}" +
                    $"\t\t{ORD_DTE + Environment.NewLine}" +
                    $"\t\t{SHIP_DTE + Environment.NewLine}" +
                    $"\t\t{CMNT1 + Environment.NewLine}" +
                    $"\t\t{CMNT2 + Environment.NewLine}" +
                    $"\t\t{DLR_NME + Environment.NewLine}" +
                    $"\t\t{DLR_ADDR + Environment.NewLine}" +
                    $"\t\t{DLR_ADDR2 + Environment.NewLine}" +
                    $"\t\t{SHP_NME + Environment.NewLine}" +
                    $"\t\t{SHP_ADDR + Environment.NewLine}" +
                    $"\t\t{SHP_ADDR2 + Environment.NewLine}" +
                    $"\t\t{SHP_CSZ + Environment.NewLine}" +
                    $"\t\t{SHP_TEL + Environment.NewLine}" +
                    $"\t\t{SHP_CT + Environment.NewLine}" +
                    $"\t\t{SHP_ZIP + Environment.NewLine}" +
                    $"\t\t{CUS_NME + Environment.NewLine}" +
                    $"\t\t{CUS_ADDR + Environment.NewLine}" +
                    $"\t\t{CUS_CSZ + Environment.NewLine}" +
                    $"\t\t{CUS_TEL + Environment.NewLine}" +
                    $"\t\t{RTE_CDE + Environment.NewLine}" +
                    $"\t\t{DEPOSIT + Environment.NewLine}" +
                    $"\t\t{ENT_BY + Environment.NewLine}" +
                    $"\t\t{HLD_FLG + Environment.NewLine}" +
                    $"\t\t{HLD_BY + Environment.NewLine}" +
                    $"\t\t{HLD_MSG + Environment.NewLine}" +
                    $"\t\t{ORD_AMT + Environment.NewLine}" +
                    $"\t\t{WIN_QTY + Environment.NewLine}" +
                    $"\t\t{PAR_QTY + Environment.NewLine}" +
                    $"\t\t{STK_QTY + Environment.NewLine}" +
                    $"\t\t{CMP_QTY + Environment.NewLine}" +
                    $"\t\t{SHP_QTY + Environment.NewLine}" +
                    $"\t\t{SHP_AMT + Environment.NewLine}" +
                    $"\t\t{MISC_TEXT + Environment.NewLine}" +
                    $"\t\t{Enum.GetName(typeof(OrderStatus), Status)}" +
                    $"\t\t{Enum.GetName(typeof(status), status)}" +
                    $"\t\t{ManId + Environment.NewLine}";
            }

            public orderMaster() { }
            public orderMaster(OrderMasterData omd)
            {
                try
                {
                    command = omd.Command;

                    Logger.Info($"{omd.RequestId}");
                    requestId = omd.RequestId.ToByteArray();

                    ORD_NO = omd.ORD_NO;
                    DLR_NO = omd.DLR_NO;
                    DLR_PO = omd.DLR_PO;
                    ORD_DTE = omd.ORD_DTE;
                    SHP_DTE = omd.SHP_DTE;
                    SHIP_DTE = omd.SHIP_DTE;
                    CMNT1 = omd.CMNT1;
                    CMNT2 = omd.CMNT2;
                    DLR_NME = omd.DLR_NME;
                    DLR_ADDR = omd.DLR_ADDR;
                    DLR_ADDR2 = omd.DLR_ADDR2;
                    SHP_NME = omd.SHP_NME;
                    SHP_ADDR = omd.SHP_ADDR;
                    SHP_ADDR2 = omd.SHP_ADDR2;
                    SHP_CSZ = omd.SHP_CSZ;
                    SHP_TEL = omd.SHP_TEL;
                    SHP_CT = omd.SHP_CT;
                    SHP_ZIP = omd.SHP_ZIP;
                    CUS_NME = omd.CUS_NME;
                    CUS_ADDR = omd.CUS_ADDR;
                    CUS_CSZ = omd.CUS_CSZ;
                    CUS_TEL = omd.CUS_TEL;
                    RTE_CDE = omd.RTE_CDE;
                    ENT_BY = omd.ENT_BY;
                    ORD_AMT = omd.ORD_AMT;
                    WIN_QTY = omd.WIN_QTY;
                    STK_QTY = omd.STK_QTY;
                    CMP_QTY = omd.CMP_QTY;
                    SHP_QTY = omd.SHP_QTY;
                    SHP_AMT = omd.SHP_AMT;
                    MISC_TEXT = omd.MISC_TEXT;
                    Status = omd.Status;
                    ManId = omd.ManId;
                    SCAN_DATE_TIME = omd.SCAN_DATE_TIME;
                    status = omd.status;
                }
                catch (Exception ex) { Logger.Error($"orderMaster Error:  {ex.Message}"); }
            }

            public orderMaster(OrderMasterData ord, long id)
            {
                command = ord.Command;
                requestId = ord.RequestId.ToByteArray();

                ORD_NO = ord.ORD_NO;
                DLR_NO = ord.DLR_NO;
                DLR_PO = ord.DLR_PO;
                ORD_DTE = ord.ORD_DTE;
                SHIP_DTE = ord.SHIP_DTE;
                CMNT1 = ord.CMNT1;
                CMNT2 = ord.CMNT2;
                DLR_NME = ord.DLR_NME;
                DLR_ADDR = ord.DLR_ADDR;
                DLR_ADDR2 = ord.DLR_ADDR2;
                //DLR_CSZ = ord.DLR_CSZ;
                //DLR_TEL = ord.DLR_TEL;
                //DLR_CT = ord.DLR_CT;
                SHP_NME = ord.SHP_NME;
                SHP_ADDR = ord.SHP_ADDR;
                SHP_ADDR2 = ord.SHP_ADDR2;
                SHP_CSZ = ord.SHP_CSZ;
                SHP_TEL = ord.SHP_TEL;
                SHP_CT = ord.SHP_CT;
                SHP_ZIP = ord.SHP_ZIP;
                CUS_NME = ord.CUS_NME;
                CUS_ADDR = ord.CUS_ADDR;
                CUS_CSZ = ord.CUS_CSZ;
                CUS_TEL = ord.CUS_TEL;
                RTE_CDE = ord.RTE_CDE;
                ORD_AMT = ord.ORD_AMT;
                WIN_QTY = ord.WIN_QTY;
                STK_QTY = ord.STK_QTY;
                CMP_QTY = ord.CMP_QTY;
                SHP_QTY = ord.SHP_QTY;
                SHP_AMT = ord.SHP_AMT;
                MISC_TEXT = ord.MISC_TEXT;
                Status = ord.Status;
                status = ord.status;
            }

            public orderMaster(OrderData ord, long id)
            {
                command = ord.Command;
                requestId = ord.RequestId.ToByteArray();

                ORD_NO = ord.ORD_NO;
                DLR_NO = ord.DLR_NO;
                DLR_PO = ord.DLR_PO;
                ORD_DTE = ord.ORD_DTE;
                SHIP_DTE = ord.SHIP_DTE;
                CMNT1 = ord.CMNT1;
                CMNT2 = ord.CMNT2;
                DLR_NME = ord.DLR_NME;
                DLR_ADDR = ord.DLR_ADDR;
                DLR_ADDR2 = ord.DLR_ADDR2;
                //DLR_CSZ = ord.DLR_CSZ;
                //DLR_TEL = ord.DLR_TEL;
                //DLR_CT = ord.DLR_CT;
                SHP_NME = ord.SHP_NME;
                SHP_ADDR = ord.SHP_ADDR;
                SHP_ADDR2 = ord.SHP_ADDR2;
                SHP_CSZ = ord.SHP_CSZ;
                SHP_TEL = ord.SHP_TEL;
                SHP_CT = ord.SHP_CT;
                SHP_ZIP = ord.SHP_ZIP;
                CUS_NME = ord.CUS_NME;
                CUS_ADDR = ord.CUS_ADDR;
                CUS_CSZ = ord.CUS_CSZ;
                CUS_TEL = ord.CUS_TEL;
                RTE_CDE = ord.RTE_CDE;
                ORD_AMT = ord.ORD_AMT;
                WIN_QTY = ord.WIN_QTY;
                PAR_QTY = ord.PAR_QTY;
                STK_QTY = ord.STK_QTY;
                CMP_QTY = ord.CMP_QTY;
                SHP_QTY = ord.SHP_QTY;
                SHP_AMT = ord.SHP_AMT;
                MISC_TEXT = ord.MISC_TEXT;
                ORD_NO = ord.ORD_NO;
                DLR_NO = ord.DLR_NO;
                Status = ord.Status;
                status = ord.status;
                //IsSelected = isSelected;
            }
            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    var writer = new BinaryWriter(stream);

                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(ORD_NO);
                    writer.Write(DLR_NO);
                    writer.Write(DLR_PO);
                    writer.Write(ORD_DTE);
                    writer.Write(SHP_DTE);
                    writer.Write(SHIP_DTE);
                    writer.Write(CMNT1.ToStringExNull());
                    writer.Write(CMNT2.ToStringExNull());
                    writer.Write(DLR_NME.ToStringExNull());
                    writer.Write(DLR_ADDR.ToStringExNull());
                    writer.Write(DLR_ADDR2.ToStringExNull());
                    writer.Write(DLR_CSZ.ToStringExNull());
                    writer.Write(DLR_TEL.ToStringExNull());
                    writer.Write(DLR_CT);
                    writer.Write(SHP_NME.ToStringExNull());
                    writer.Write(SHP_ADDR.ToStringExNull());
                    writer.Write(SHP_ADDR2.ToStringExNull());
                    writer.Write(SHP_CSZ.ToStringExNull());
                    writer.Write(SHP_TEL.ToStringExNull());
                    writer.Write(SHP_ZIP.ToStringExNull());
                    writer.Write(SHP_CT);
                    writer.Write(CUS_NME.ToStringExNull());
                    writer.Write(CUS_ADDR.ToStringExNull());
                    writer.Write(CUS_CSZ.ToStringExNull());
                    writer.Write(CUS_TEL.ToStringExNull());
                    writer.Write(RTE_CDE.ToStringExNull());
                    writer.Write(ENT_BY.ToStringExNull());
                    writer.Write(DTE_ENT);
                    writer.Write(ORD_AMT);
                    writer.Write(WIN_QTY);
                    writer.Write(STK_QTY);
                    writer.Write(CMP_QTY);
                    writer.Write(SHP_QTY);
                    writer.Write(SHP_AMT);
                    writer.Write(MISC_TEXT.ToStringExNull());
                    writer.Write((Int16)Status);
                    writer.Write(ManId);
                    writer.Write((Int16)status);
                    //ordOpt.Write
                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    ORD_NO = reader.ReadInt32();
                    DLR_NO = reader.ReadInt32();
                    DLR_PO = reader.ReadString();
                    ORD_DTE = reader.ReadInt32();
                    SHP_DTE = reader.ReadInt32();
                    SHIP_DTE = reader.ReadInt32();
                    CMNT1 = reader.ReadString();
                    CMNT2 = reader.ReadString();
                    DLR_NME = reader.ReadString();
                    DLR_ADDR = reader.ReadString();
                    DLR_ADDR2 = reader.ReadString();
                    DLR_CSZ = reader.ReadString();
                    DLR_TEL = reader.ReadString();
                    DLR_CT = reader.ReadInt16();
                    SHP_NME = reader.ReadString();
                    SHP_ADDR = reader.ReadString();
                    SHP_ADDR2 = reader.ReadString();
                    SHP_CSZ = reader.ReadString();
                    SHP_TEL = reader.ReadString();
                    SHP_ZIP = reader.ReadString();
                    SHP_CT = reader.ReadInt16();
                    CUS_NME = reader.ReadString();
                    CUS_ADDR = reader.ReadString();
                    CUS_CSZ = reader.ReadString();
                    CUS_TEL = reader.ReadString();
                    RTE_CDE = reader.ReadString();
                    ENT_BY = reader.ReadString();
                    DTE_ENT = reader.ReadInt32();
                    ORD_AMT = reader.ReadDecimal();
                    WIN_QTY = reader.ReadInt16();
                    STK_QTY = reader.ReadInt16();
                    CMP_QTY = reader.ReadInt16();
                    SHP_QTY = reader.ReadInt16();
                    SHP_AMT = reader.ReadDecimal();
                    MISC_TEXT = reader.ReadString();
                    Status = (OrderStatus)reader.ReadInt16();
                    ManId = reader.ReadInt64();
                    status = (status)reader.ReadInt16();
                };
                return this;
            }
            
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct orderDisplayDetails : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }


            public int Id { get; set; }
            public int ORD_NO { get; set; }

            public short MDL_CNT { get; set; }
            public string MDL_NO { get; set; }


            public string CLR { get; set; }
            public byte PAT_POS { get; set; }
            public DateTime SCAN_DATE_TIME { get; set; }
            public Int16 BIN_NO { get; set; }

            public string DESC { get; set; }
            public decimal WIDTH { get; set; }
            public decimal HEIGHT { get; set; }
            public string ScanTime { get; set; }
            public OrderStatus Status { get; set; }


            
            public string OPT_TYPE;                 //2
            public short OPT_NUM;                  //2
 

            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"" +
                    $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ORD_NO + Environment.NewLine}" +
                    $"\t\t{MDL_CNT + Environment.NewLine}" +
                    $"\t\t{PAT_POS + Environment.NewLine}" +
                    $"\t\t{BIN_NO + Environment.NewLine}" +
                    $"\t\t{WIDTH + Environment.NewLine}" +
                    $"\t\t{HEIGHT + Environment.NewLine}" +
                    $"\t\t{MDL_NO + Environment.NewLine}" +
                    $"\t\t{OPT_TYPE + Environment.NewLine}" +
                    $"\t\t{OPT_NUM + Environment.NewLine}" +
                    $"\t\t{CLR + Environment.NewLine}" +
                    $"\t\t{DESC}";
            }
            public byte[] ToArray(MemoryStream stream)
            {
                using (var writer = new BinaryWriter(stream))
                {
                    ToArray(writer);
                    return stream.ToArray();
                }
            }
            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    return ToArray(stream);
                }
            }
            public byte[] ToArray(BinaryWriter writer)
            {
                using (var stream = manager.GetStream())
                {
                    //var writer = new BinaryWriter(stream);
                    //  writer.Write((byte)command);
                    //   writer.Write(requestId);
                    writer.Write(ORD_NO);
                    writer.Write(WIDTH);
                    writer.Write(HEIGHT);
                    writer.Write(MDL_CNT);
                    writer.Write(PAT_POS);
                    writer.Write(MDL_NO);   //4 bytes
                    writer.Write(OPT_TYPE.ToStringExNull());
                    writer.Write(OPT_NUM);
                    writer.Write(CLR.ToStringExNull());   //3 bytes
                    writer.Write(DESC.ToStringExNull());   //60 bytes

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(BinaryReader reader)
            {
                //   command = (eCommand)reader.ReadByte();
                //   requestId = reader.ReadBytes(fldsz_GUID);
                ORD_NO = reader.ReadInt32();
                WIDTH = reader.ReadDecimal();
                HEIGHT = reader.ReadDecimal();
                MDL_CNT = reader.ReadInt16();
                PAT_POS = reader.ReadByte();
                MDL_NO = reader.ReadString();
                OPT_TYPE = reader.ReadString();
                OPT_NUM = reader.ReadInt16();
                CLR = reader.ReadString();
                DESC = reader.ReadString();

                return this;
            }
            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    FromArray(reader);
                };
                return this;
            }
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct orderDetails : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }
            public Int32 ORD_NO { get; set; }
            public Int16 MDL_CNT { get; set; }
            public Int16 PAT_POS { get; set; }
            public decimal WIDTH { get; set; }
            public decimal HEIGHT { get; set; }

            public string MDL_NO { get; set; }
            public string OPT_TYPE;                 //2
            public short OPT_NUM;                  //2
            public string CLR;                      //3
            public string DESC; //60
            public DateTime ScanTime;

            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"" +
                   // $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                   // $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ORD_NO + Environment.NewLine}" +
                    $"\t\t{MDL_CNT + Environment.NewLine}" +
                    $"\t\t{PAT_POS + Environment.NewLine}" +
                    $"\t\t{WIDTH + Environment.NewLine}" +
                    $"\t\t{HEIGHT + Environment.NewLine}" +
                    $"\t\t{MDL_NO + Environment.NewLine}" +
                    $"\t\t{OPT_TYPE + Environment.NewLine}" +
                    $"\t\t{OPT_NUM + Environment.NewLine}" +
                    $"\t\t{CLR + Environment.NewLine}" +
                    $"\t\t{DESC}";
            }
            public byte[] ToArray(MemoryStream stream)
            {
                using (var writer = new BinaryWriter(stream))
                {
                    ToArray(writer);
                    return stream.ToArray();
                }
            }
            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    return ToArray(stream);
                }
            }
            public byte[] ToArray(BinaryWriter writer)
            {
                using (var stream = manager.GetStream())
                {
                    //var writer = new BinaryWriter(stream);
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(ORD_NO);
                    writer.Write(WIDTH);
                    writer.Write(HEIGHT);
                    writer.Write(MDL_CNT);
                    writer.Write(PAT_POS);
                    writer.Write(MDL_NO);   //4 bytes
                    writer.Write(OPT_TYPE.ToStringExNull());
                    writer.Write(OPT_NUM);
                    writer.Write(CLR.ToStringExNull());   //3 bytes
                    writer.Write(DESC.ToStringExNull());   //60 bytes

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(BinaryReader reader)
            {
                command = (eCommand)reader.ReadByte();
                requestId = reader.ReadBytes(fldsz_GUID);
                ORD_NO = reader.ReadInt32();
                WIDTH = reader.ReadDecimal();
                HEIGHT = reader.ReadDecimal();
                MDL_CNT = reader.ReadInt16();
                PAT_POS = reader.ReadInt16();
                MDL_NO = reader.ReadString();
                OPT_TYPE = reader.ReadString();
                OPT_NUM = reader.ReadInt16();
                CLR = reader.ReadString();
                DESC = reader.ReadString();

                return this;
            }
            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    FromArray(reader);
                };
                return this;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct optionsDetail : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public int ORD_NO;                     //4
            public short MDL_CNT;                   //2
            public byte PAT_POS;                    //1
            public string MDL_NO;                   //4
            public byte[] OPT_TYPE;                 //2
            public short OPT_NUM;                   //2
            public byte[] CLR;                      //3
            public byte[] DESC;                     //60
            public byte[] OPT_SOURCE;               //5

            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ORD_NO + Environment.NewLine}" +
                    $"\t\t{MDL_CNT + Environment.NewLine}" +
                    $"\t\t{PAT_POS + Environment.NewLine}" +
                    $"\t\t{MDL_NO + Environment.NewLine}" +
                    $"\t\t{OPT_TYPE + Environment.NewLine}" +
                    $"\t\t{OPT_NUM + Environment.NewLine}" +
                    $"\t\t{CLR + Environment.NewLine}" +
                    $"\t\t{DESC + Environment.NewLine}" +
                    $"\t\t{OPT_SOURCE}";
            }

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    var writer = new BinaryWriter(stream);
                    writer.Write((byte)eCommand.OrderOptions);
                    writer.Write(requestId);
                    writer.Write(ORD_NO);
                    writer.Write(MDL_CNT);
                    writer.Write(PAT_POS);

                    writer.Write(MDL_NO);

                    if (OPT_TYPE == null)
                        OPT_TYPE = new byte[fldsz_OPT_TYPE];
                    writer.Write(OPT_TYPE);

                    writer.Write((short)OPT_NUM);

                    if (CLR == null)
                        CLR = new byte[fldsz_CLR];
                    writer.Write(CLR);   //4 bytes

                    if (DESC == null)
                        DESC = new byte[fldsz_DESCOrd];
                    writer.Write(DESC);   //4 bytes

                    if (OPT_SOURCE == null)
                        OPT_SOURCE = new byte[fldsz_OPT_SRC];
                    writer.Write(OPT_SOURCE); //5 bytes

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                var s = new optionsDetail();

                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    s.command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    s.ORD_NO = reader.ReadInt32();
                    s.MDL_CNT = reader.ReadInt16();
                    s.PAT_POS = reader.ReadByte();
                    s.MDL_NO = reader.ReadString();                   //4
                    s.OPT_TYPE = reader.ReadBytes(fldsz_OPT_TYPE);                 //2
                    s.OPT_NUM = reader.ReadInt16();
                    s.CLR = reader.ReadBytes(fldsz_CLR);
                    s.DESC = reader.ReadBytes(fldsz_DESCOrd);
                    s.OPT_SOURCE = reader.ReadBytes(fldsz_OPT_SRC);
                };
                return s;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct orderOptions : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }
            public Int32 ORD_NO { get; set; }
            public Int16 MDL_CNT { get; set; }
            public Byte PAT_POS { get; set; }
            public String MDL_NO { get; set; }
            public String OPT_TYPE { get; set; }
            public Int16 OPT_NUM { get; set; }
            public String STOCK_ID { get; set; }
            public String CALL_SIZE { get; set; }
            public String CLR { get; set; }
            public String DESC { get; set; }
            public Int16 QTY { get; set; }
            public Int16 CMP_QTY { get; set; }
            public Int16 PAT_ID { get; set; }
            public Decimal WIDTH { get; set; }
            public Decimal HEIGHT { get; set; }
            public Decimal COM { get; set; }
            public Decimal PRICE { get; set; }
            public Decimal DIS_PER { get; set; }
            public Decimal DIS_AMT { get; set; }
            public Decimal DIS_UNT { get; set; }
            public Decimal NET_AMT { get; set; }
            public String CMT1 { get; set; }
            public String CMT2 { get; set; }
            public String NOTES { get; set; }
            public String SHIPPING { get; set; }
            public Int32 SHP_DTE { get; set; }
            public String TRUCK { get; set; }
            public Int16 SHP_SEQUENCE { get; set; }
            public String TYPE { get; set; }
            public String PROD_DESC { get; set; }
            public Decimal EXP_SZE { get; set; }
            public Int32 LOT_NO { get; set; }
            public Int16 ORTIDX { get; set; }
            public Int32 LOT_DTE { get; set; }
            public Int16 LOT_SEQ { get; set; }
            public Int16 BIN { get; set; }
            public Int16 BGN_BIN { get; set; }
            public Int16 END_BIN { get; set; }
            public Byte EMAILED { get; set; }
            public Int16 ADD_DAYS { get; set; }
            public Int32 DTE_ADDED { get; set; }

       
            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ORD_NO + Environment.NewLine}" +
                    $"\t\t{MDL_CNT + Environment.NewLine}" +
                    $"\t\t{PAT_POS + Environment.NewLine}" +
                    $"\t\t{MDL_NO + Environment.NewLine}" +
                    //$"\t\t{Model + Environment.NewLine}" +
                    $"\t\t{OPT_TYPE + Environment.NewLine}" +
                    $"\t\t{OPT_NUM + Environment.NewLine}" +
                  //  $"\t\t{CALL_SIZE + Environment.NewLine}" +
                    $"\t\t{CLR + Environment.NewLine}" +
                    $"\t\t{DESC}";
            }

            public orderOptions(OrderOptionsData ood)
            {
                command = ood.Command;
                requestId = ood.RequestId.ToByteArray();
                ORD_NO = ood.ORD_NO;
                MDL_CNT = ood.MDL_CNT;
                PAT_POS = ood.PAT_POS;
                MDL_NO = ood.MDL_NO;
                OPT_TYPE = ood.OPT_TYPE;
                OPT_NUM = ood.OPT_NUM;
                STOCK_ID = ood.STOCK_ID;
                CALL_SIZE = ood.CALL_SIZE;
                CLR = ood.CLR;
                DESC = ood.DESC;
                QTY = ood.QTY;
                CMP_QTY = ood.CMP_QTY;
                PAT_ID = ood.PAT_ID;
                WIDTH = ood.WIDTH;
                HEIGHT = ood.HEIGHT;
                COM = ood.COM;
                PRICE = ood.PRICE;
                DIS_PER = ood.DIS_PER;
                DIS_AMT = ood.DIS_AMT;
                DIS_UNT = ood.DIS_UNT;
                NET_AMT = ood.NET_AMT;
                CMT1 = ood.CMT1;
                CMT2 = ood.CMT2;
                NOTES = ood.NOTES;
                SHIPPING = ood.SHIPPING;
                SHP_DTE = ood.SHP_DTE;
                TRUCK = ood.TRUCK;
                SHP_SEQUENCE = ood.SHP_SEQUENCE;
                TYPE = ood.TYPE;
                PROD_DESC = ood.PROD_DESC;
                EXP_SZE = ood.EXP_SZE;
                LOT_NO = ood.LOT_NO;
                ORTIDX = ood.ORTIDX;
                LOT_DTE = ood.LOT_DTE;
                LOT_SEQ = ood.LOT_SEQ;
                BIN = ood.BIN;
                BGN_BIN = ood.BGN_BIN;
                END_BIN = ood.END_BIN;
                EMAILED = ood.EMAILED;
                ADD_DAYS = ood.ADD_DAYS;
                DTE_ADDED = ood.DTE_ADDED;
                
            }

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    var writer = new BinaryWriter(stream);

                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(ORD_NO);
                    writer.Write(MDL_CNT);
                    writer.Write(PAT_POS);
                    writer.Write(MDL_NO);   //4 bytes
                    writer.Write(OPT_TYPE);
                    writer.Write(OPT_NUM);
                    writer.Write(CLR);   //3 bytes
                    writer.Write(DESC);   //60 bytes

                    writer.Write(CMT1);
                    writer.Write(CMT2);
                    writer.Write(PROD_DESC);
                    writer.Write(BIN);
                    writer.Write(WIDTH);
                    writer.Write(HEIGHT);

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    ORD_NO = reader.ReadInt32();
                    MDL_CNT = reader.ReadInt16();
                    PAT_POS = reader.ReadByte();
                    MDL_NO = reader.ReadString();
                    OPT_TYPE = reader.ReadString();
                    OPT_NUM = reader.ReadInt16();
                    CLR = reader.ReadString();
                    DESC = reader.ReadString();
                    CMT1 = reader.ReadString();
                    CMT2 = reader.ReadString();
                    PROD_DESC = reader.ReadString();
                    BIN = reader.ReadInt16();
                    WIDTH = reader.ReadDecimal();
                    HEIGHT = reader.ReadDecimal();
                }; 
                return this;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct completeOrder : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }
            public int ORD_NO { get; set; }
            public Int16 MDL_CNT { get; set; }
            public string MDL_NO { get; set; }
            public Int16 BIN_NO { get; set; }
            public string DESC { get; set; }
            public decimal WIDTH { get; set; }
            public decimal HEIGHT { get; set; }
            public string ScanTime { get; set; }
            public OrderStatus Status { get; set; }

            public completeOrder(OrderDetailsModelData odm)
            {
                command = odm.Command;
                requestId = odm.RequestId.ToByteArray();
                ORD_NO = odm.ORD_NO;
                MDL_CNT = odm.MDL_CNT;
                MDL_NO = odm.MDL_NO;
                BIN_NO = odm.BIN_NO;
                
                DESC = odm.DESC;
                WIDTH = odm.WIDTH;
                HEIGHT = odm.HEIGHT;
                ScanTime = odm.ScanTime;
                Status = odm.Status;
            }
            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ORD_NO + Environment.NewLine}" +
                    $"\t\t{MDL_CNT + Environment.NewLine}" +
                    $"\t\t{MDL_NO + Environment.NewLine}" +
                    $"\t\t{BIN_NO.ToString() + Environment.NewLine}" +
                    $"\t\t{DESC + Environment.NewLine}" +
                    $"\t\t{WIDTH + Environment.NewLine}" +
                    $"\t\t{HEIGHT + Environment.NewLine}" +
                    $"\t\t{ScanTime + Environment.NewLine}" +
                    $"\t\t{Enum.GetName(typeof(OrderStatus), Status)}";
            }

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    return ToArray(stream);
                }
            }

            public byte[] ToArray(MemoryStream stream)
            {
                using (var writer = new BinaryWriter(stream))
                {
                    ToArray(writer);
                    return stream.ToArray();
                }
            }


            public void ToArray(BinaryWriter writer)
            {
                writer.Write((byte)eCommand.CompleteOrder);
                // writer.Write(requestId);
                writer.Write(ORD_NO);
                writer.Write(MDL_CNT);
                writer.Write(MDL_NO.ToStringExNull());
                writer.Write(BIN_NO);
                writer.Write(DESC);
                writer.Write(WIDTH);
                writer.Write(HEIGHT);
                writer.Write(ScanTime);
                writer.Write((Int16)Status);
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    return FromArray(reader);
                }
            }

            public isaCommand FromArray(BinaryReader reader)
            {
                var co = new completeOrder();

                using (reader)
                {
                    co.command = (eCommand)reader.ReadByte();
                    //s.requestId = reader.ReadBytes(fldsz_GUID);
                    co.ORD_NO = reader.ReadInt32();
                    co.MDL_CNT = reader.ReadInt16();
                    //PAT_POS = reader.ReadInt16();
                    //LOT_NO = reader.ReadInt32();
                    co.BIN_NO = reader.ReadInt16();
                    co.MDL_NO = reader.ReadString();

                    co.Status = (OrderStatus)reader.ReadInt16();
                };
                return co;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct scanFile : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }
            public Int64 ORD_NO { get; set; }
            public Int16 MDL_CNT { get; set; }
            public Int16 PAT_POS { get; set; }
            public Int32 LOT_NO { get; set; }
            public Int16 BIN_NO { get; set; }
            public String MDL_NO { get; set; }
            public Byte INVOICE_FLAG { get; set; }
            public Int32 INVOICE_NO { get; set; }
            //public String TYPE { get; set; }
            public Byte DSP_SEQ { get; set; }
            public String TRK_CDE { get; set; }
            public Int32 TRK_DTE { get; set; }
            public Int16 WIN_CNT { get; set; }
            //public String DLR_PO { get; set; }
            public Int32 SHP_DTE { get; set; }
            public Int32 SHP_TME { get; set; }
            public String SHP_BY { get; set; }
            public String LOCATION { get; set; }
            public String REASON { get; set; }
            public Int64 MAN_ID { get; set; }
            public scanFile(scanFile s)
            {
                command= s.command;
                requestId = s.requestId;
                ORD_NO = s.ORD_NO;
                MDL_CNT = s.MDL_CNT;
                PAT_POS = s.PAT_POS;
                LOT_NO = s.LOT_NO;
                BIN_NO = s.BIN_NO;
                MDL_NO = s.MDL_NO;
                INVOICE_FLAG = s.INVOICE_FLAG;
                INVOICE_NO = s.INVOICE_NO;
                DSP_SEQ = s.DSP_SEQ;
                TRK_CDE = s.TRK_CDE;
                TRK_DTE = s.TRK_DTE;
                WIN_CNT = s.WIN_CNT;
               // DLR_PO = s.DLR_PO;
                SHP_DTE = s.SHP_DTE;
                SHP_TME = s.SHP_TME;
                SHP_BY = s.SHP_BY ;
                LOCATION = s.LOCATION;
                REASON = s.REASON;
                MAN_ID = s.MAN_ID;
            }
            public scanFile(ScanFileData s)
            {
                command = s.Command;
                requestId = s.RequestId.ToByteArray();
                ORD_NO = s.ORD_NO;
                MDL_CNT = s.MDL_CNT;
                PAT_POS = s.PAT_POS;
                LOT_NO = s.LOT_NO;
                BIN_NO = s.BIN_NO;
                MDL_NO = s.MDL_NO;
                INVOICE_FLAG = s.INVOICE_FLAG;
                INVOICE_NO = s.INVOICE_NO;
                DSP_SEQ = s.DSP_SEQ;
                TRK_CDE = s.TRK_CDE;
                TRK_DTE = s.TRK_DTE;
                WIN_CNT = s.WIN_CNT;
                // DLR_PO = s.DLR_PO;
                SHP_DTE = s.SHP_DTE;
                SHP_TME = s.SHP_TME;
                SHP_BY = s.SHP_BY;
                LOCATION = s.LOCATION;
                REASON = s.REASON;
                MAN_ID = s.MAN_ID;
            }
            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ORD_NO + Environment.NewLine}" +
                    $"\t\t{MDL_CNT + Environment.NewLine}" +
                    $"\t\t{PAT_POS + Environment.NewLine}" +
                    $"\t\t{MDL_NO + Environment.NewLine}" +
                    $"\t\t{BIN_NO + Environment.NewLine}" +
                    $"\t\t{MAN_ID + Environment.NewLine}";
            }

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    return ToArray(stream);
                }
            }

            public byte[] ToArray(MemoryStream stream)
            {
                using (var writer = new BinaryWriter(stream))
                {
                    ToArray(writer);
                    return stream.ToArray();
                }
            }


            public void ToArray(BinaryWriter writer)
            {
                writer.Write((byte)eCommand.ScanFile);
               // writer.Write(requestId);
                writer.Write(ORD_NO);
                writer.Write(MDL_CNT);
                writer.Write(PAT_POS);
                writer.Write(LOT_NO);
                writer.Write(BIN_NO);
                writer.Write(MDL_NO.ToStringExNull());
                writer.Write(INVOICE_FLAG);
                writer.Write(INVOICE_NO);
                writer.Write(DSP_SEQ);
                writer.Write(TRK_CDE.ToStringExNull());
                writer.Write(TRK_DTE);
                writer.Write(WIN_CNT);
                //writer.Write(DLR_PO.ToStringExNull());
                writer.Write(SHP_DTE);
                writer.Write(SHP_TME);
                writer.Write(SHP_BY.ToStringExNull());
                writer.Write(LOCATION.ToStringExNull());
                writer.Write(REASON.ToStringExNull());
                writer.Write(MAN_ID);
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    return FromArray(reader);
                }
            }

            public isaCommand FromArray(BinaryReader reader)
            {
                var s = new scanFile();

                using (reader)
                {
                    s.command = (eCommand)reader.ReadByte();
                    //s.requestId = reader.ReadBytes(fldsz_GUID);
                    s.ORD_NO = reader.ReadInt64();
                    s.MDL_CNT = reader.ReadInt16();
                    s.PAT_POS = reader.ReadInt16();
                    s.LOT_NO = reader.ReadInt32();
                    s.BIN_NO = reader.ReadInt16();
                    s.MDL_NO = reader.ReadString();
                    s.INVOICE_FLAG = reader.ReadByte();
                    s.INVOICE_NO = reader.ReadInt32();
                    s.DSP_SEQ = reader.ReadByte();
                    s.TRK_CDE = reader.ReadString();
                    s.TRK_DTE = reader.ReadInt32();
                    s.WIN_CNT = reader.ReadInt16();
                    //s.DLR_PO = reader.ReadString();
                    s.SHP_DTE = reader.ReadInt32();
                    s.SHP_TME = reader.ReadInt32();
                    s.SHP_BY = reader.ReadString();
                    s.LOCATION = reader.ReadString();
                    s.REASON = reader.ReadString();
                    s.MAN_ID = reader.ReadInt64();
                };
                return s;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class inventoryDetail : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }
            /*Inventory Detail TPS*
    inv_dtl.tps*/

            public int ORD_NO;
            public short MDL_CNT;
            public short PAT_POS;
            public short QUANTITY;
            public short ORIG_QTY;
            public byte BACKORDER;
            public long LOT_NO;
            public short LOT_IDX;
            public byte STOCK_FLAG;
            public string MDL_NO;
            public string STOCK_KEY;
            public short TOT_UI;
            public string EXP_CDE;
            public string BA_CDE;
            public string GL_CDE;
            public string SC_CDE;
            public string GR1_CDE;
            public string GR2_CDE;
            public string OT1_CDE;
            public string OT2_CDE;
            public string OT3_CDE;
            public string OT4_CDE;
            public string OT5_CDE;
            public byte POST_FLAG;

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    var writer = new BinaryWriter(stream);

                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(ORD_NO);
                    writer.Write(MDL_CNT);
                    writer.Write(PAT_POS);
                    writer.Write(QUANTITY);
                    writer.Write(ORIG_QTY);
                    writer.Write(BACKORDER);
                    writer.Write(LOT_NO);
                    writer.Write(LOT_IDX);
                    writer.Write(STOCK_FLAG);
                    writer.Write(MDL_NO);
                    writer.Write(STOCK_KEY);
                    writer.Write(TOT_UI);
                    writer.Write(EXP_CDE);
                    writer.Write(BA_CDE);
                    writer.Write(GL_CDE);
                    writer.Write(SC_CDE);
                    writer.Write(GR1_CDE);
                    writer.Write(GR2_CDE);
                    writer.Write(OT1_CDE);
                    writer.Write(OT2_CDE);
                    writer.Write(OT3_CDE);
                    writer.Write(OT4_CDE);
                    writer.Write(OT5_CDE);
                    writer.Write(POST_FLAG);
                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();

                    requestId = reader.ReadBytes(fldsz_GUID);
                    ORD_NO = reader.ReadInt32();
                    MDL_CNT = reader.ReadInt16();
                    PAT_POS = reader.ReadInt16();
                    QUANTITY = reader.ReadInt16();
                    ORIG_QTY = reader.ReadInt16();
                    BACKORDER = reader.ReadByte();
                    LOT_NO = reader.ReadInt64();
                    LOT_IDX = reader.ReadInt16();
                    STOCK_FLAG = reader.ReadByte();
                    MDL_NO = reader.ReadString();
                    STOCK_KEY= reader.ReadString();
                    TOT_UI = reader.ReadInt16();
                    EXP_CDE = reader.ReadString();
                    BA_CDE = reader.ReadString();
                    GL_CDE = reader.ReadString();
                    SC_CDE = reader.ReadString();
                    GR1_CDE = reader.ReadString();
                    GR2_CDE = reader.ReadString();
                    OT1_CDE = reader.ReadString();
                    OT2_CDE = reader.ReadString();
                    OT3_CDE = reader.ReadString();
                    OT4_CDE = reader.ReadString();
                    OT5_CDE = reader.ReadString();
                    POST_FLAG = reader.ReadByte();

                   // Status = (OrderStatus)reader.ReadInt16();
                };
                return this;
            }
        }

        /// <summary>
        /// Custom Query and Stored Procedures
        /// </summary>
        /// 
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RunQuery : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public string Query;
            public SPCmds SP;

            public byte[] ToArray()
            {
                var stream = new MemoryStream();
                var writer = new BinaryWriter(stream);
                writer.Write((byte)command);
                writer.Write(requestId);
                writer.Write(Query);
                writer.Write(SP.ToString());

                return stream.ToArray();
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    var s = default(RunQuery);

                    s.command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    s.Query = reader.ReadString();
                    s.SP = (SPCmds)Enum.Parse(typeof(SPCmds), reader.ReadString());

                    return s;
                }
            }
        }

        public class accountReceivable : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }
            public int ORD_NO;
            public int DLR_NO;
            public DateTime SHIP_DTE;        //Int32

            public long ManifestId;
            public int DSP_SEQ; 
            public byte Status_S; 
            public byte[] POD;
            public DateTime Timestamp;
            public string CUS_NME;
            public string DLR_NME;
            public string RTE_CDE;
            //public long CustomerId;
            public byte Status_OD;
            public DateTime ScanDateTime_OD;
            public int WIN_CNT;

            public accountReceivable()
            { }

            public accountReceivable(AccountsReceivableData ad)
            {
                this.command = ad.Command;
                this.requestId = ad.RequestId.ToByteArray();
                ORD_NO = ad.ORD_NO;
                DLR_NO = ad.DLR_NO;
                SHIP_DTE = ad.SHP_DTE;
                //ManifestId=ad.ManifestId;
               // DSP_SEQ = ad.;
                Status_S = (byte)ad.Status;
             POD=ad.Signature;
             Timestamp=ad.BillDateTime;

                if (ad.CustomerName == null)
                    CUS_NME = "";
                else
                    CUS_NME = ad.CustomerName;

                if (ad.DLR_NME == null)
                    DLR_NME = "";
                else
                    DLR_NME = ad.DLR_NME;

                if (ad.RTE_CDE == null)
                    RTE_CDE = "";
                else
                    RTE_CDE = ad.RTE_CDE;

                //CustomerId = ad.CustomerId;
             Status_OD=(byte)ad.status;
             ScanDateTime_OD= ad.DeliveryDateTime;
             WIN_CNT= ad.WIN_CNT;
        }
            public byte[] ToArray()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(ORD_NO);
                    writer.Write(DLR_NO);
                    writer.Write(SHIP_DTE.ToBinary());
                    writer.Write(Status_S);

                    int pl = 0;
                    if (POD != null)
                        pl = POD.Length;

                    writer.Write(pl);

                    if (pl > 0)
                        writer.Write(POD);

                    writer.Write(Timestamp.ToBinary());

                    writer.Write(CUS_NME);
                    writer.Write(DLR_NME);
                    writer.Write(RTE_CDE);
                    writer.Write(Status_OD);
                    writer.Write(ScanDateTime_OD.ToBinary());
                    writer.Write(WIN_CNT);

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    var s = new accountReceivable();
                    s.command = (eCommand)reader.ReadByte();
                    s.requestId = reader.ReadBytes(fldsz_GUID);
                    s.ORD_NO = reader.ReadInt32();
                    s.DLR_NO = reader.ReadInt32();
                    long shpdte = reader.ReadInt64();
                    if(shpdte > 0) 
                        s.SHIP_DTE = DateTime.FromBinary(shpdte);

                    s.Status_S = reader.ReadByte();

                    int podlen = reader.ReadInt32();
                    if (podlen > 0) {
                        s.POD = new byte[podlen];
                        s.POD = reader.ReadBytes(podlen);
                    }
                    long lTimestamp = reader.ReadInt64();
                    if(lTimestamp > 0)
                        s.Timestamp = DateTime.FromBinary(lTimestamp);

                    s.CUS_NME = reader.ReadString();
                    s.DLR_NME = reader.ReadString();
                    s.RTE_CDE = reader.ReadString();
                    s.Status_OD = reader.ReadByte();

                    lTimestamp = reader.ReadInt64();
                    if(lTimestamp > 0)
                        s.ScanDateTime_OD = DateTime.FromBinary(lTimestamp);

                    s.WIN_CNT = reader.ReadInt32();

                    return s;
                }
            }
        }
        /// <summary>
        /// Broadcast
        /// </summary>
        /// 
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Broadcast : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public byte[] Message; 
            public byte[] Attachment;

            public byte[] ToArray()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(Message);
                    writer.Write(Attachment);
                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    var s = default(Broadcast);
                    s.command = (eCommand)reader.ReadByte();
                    s.requestId = reader.ReadBytes(fldsz_GUID);
                    s.Message = reader.ReadBytes(fldsz_Message);
                    s.Attachment = reader.ReadBytes(fldsz_Attachment);
                    return s;
                }
            }
        }


    }
}

