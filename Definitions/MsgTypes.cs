using Microsoft.IO;
using MobileDeliveryLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using MobileDeliveryGeneral.Data;
using MobileDeliveryGeneral.ExtMethods;
using static MobileDeliveryGeneral.Definitions.enums;
using System.Text;

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


        #endregion

        static RecyclableMemoryStreamManager manager = new RecyclableMemoryStreamManager();//4, 128, 64000, true);

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

            Orders,               // = (1 << 6),
            OrderUpdatesComplete,       // = (1 << 7),

            OrderDetails,               // = (1 << 8),
            OrderDetailsComplete,       // = (1 << 9),

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
            Trucks,
            StopsLoadComplete,
            TrucksLoadComplete,
            OrdersLoadComplete
            // Unknown                     // = (1 << 17)
        };

        public enum OrderStatus : byte { New = 0, Shipped = 1, Delivered = 2, BackOrderd = 3 };
      //  public static OrderStatus Status { get { return (OrderStatus); } }

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

        /// <summary>
        /// Manifest Load
        /// </summary>
        /// 
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
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
                var reader = new BinaryReader(new MemoryStream(bytes));


                try
                {
                    command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    long lDate = reader.ReadInt64();
                    try
                    {
                        if(lDate == 0)
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
                catch(Exception ex) { Logger.Error($"Error manifestRequest (FromArray) ex= {ex.Message}"); }
                return null;

            }

            public override string ToString()
            {
                string mm = "Null bData";
                string valst = "Null valist";
                string trkcode = "Null truckCode";
                if (bData != null && bData.Length > 0)
                    mm = new manifestMaster().FromArray(bData).ToString() + Environment.NewLine;
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
                        Logger.Debug($"manifestMaster FromArray reqIdBytes: {requestId}");
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
                ShipDate = td.SHIP_DTE.ToBinary();
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
                         $"\t\t{BillComplete + Environment.NewLine}";
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

                    return stream.ToArray();
                }
            }
            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    //var s = new manifestMaster();

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

                    return this;
                }
            }
        }

        public class orders : isaCommand
        {
            public eCommand command { get; set; } = eCommand.OrdersLoad;
            public byte[] requestId { get; set; }

            public long ManifestId;
            public int DSP_SEQ;
            public int CustomerId;
            public long DLR_NO;
            public long ORD_NO;
            public string CLR;
            public int MDL_CNT;
            public string MDL_NO;
            public int WIN_CNT;
            public string DESC;
            public string MODEL;
            public byte Status;

            public decimal WIDTH;
            public decimal HEIGHT;

            public orders() { }
            public orders(OrderData st)
            {
                command = st.Command;
                requestId = st.RequestId.ToByteArray();
                ManifestId = st.ManifestId;
                DSP_SEQ = st.DSP_SEQ;
                CustomerId = st.CustomerId;
                DLR_NO = st.DLR_NO;
                ORD_NO = st.ORD_NO;
                CLR = st.CLR;
                MDL_CNT = st.MDL_CNT;
                // MDL_NO = Int32.Parse(st.MDL_NO);
                MDL_NO = st.MDL_NO;
                WIN_CNT = st.WIN_CNT;
                Status = (byte)st.Status;
                HEIGHT = st.HEIGHT;
                WIDTH = st.WIDTH;
            }

            public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ManifestId + Environment.NewLine}" +
                     $"\t\t{DSP_SEQ + Environment.NewLine}" +
                      $"\t\t{CustomerId + Environment.NewLine}" +
                       $"\t\t{DLR_NO + Environment.NewLine}" +
                        $"\t\t{ORD_NO + Environment.NewLine}" +
                         $"\t\t{CLR + Environment.NewLine}" +
                         $"\t\t{MDL_CNT + Environment.NewLine}" +
                         $"\t\t{MDL_NO + Environment.NewLine}" +
                         $"\t\t{WIN_CNT + Environment.NewLine}" +
                         $"\t\t{WIDTH + Environment.NewLine}" +
                         $"\t\t{HEIGHT + Environment.NewLine}";
            }
            //public orders(OrderMaster st)
            //{
            //    command = st.Command;
            //    requestId = st.RequestId.ToByteArray();
            //    //ManifestId = st.;
            //   // DSP_SEQ = st.DS;
            //    CustomerId = st.;
            //    DLR_NO = st.DLR_NO;
            //    ORD_NO = st.ORD_NO;
            //    CLR = st.CLR;
            //    MDL_CNT = st.MDL_CNT;
            //    MDL_NO = st.MDL_NO;
            //    WIN_CNT = st.WIN_CNT;
            //    //Status = st.Status;
            //}
            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(ManifestId);

                    writer.Write(DSP_SEQ);
                    writer.Write(CustomerId);
                    writer.Write(DLR_NO);
                    writer.Write(ORD_NO);
                    writer.Write(CLR);
                    writer.Write(MDL_CNT);
                    writer.Write(MDL_NO);
                    writer.Write(WIN_CNT);
                    writer.Write(DESC);
                    writer.Write(Status);
                    writer.Write(WIDTH);
                    writer.Write(HEIGHT);

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
                    DSP_SEQ = reader.ReadInt32();
                    CustomerId = reader.ReadInt32();
                    DLR_NO = reader.ReadInt64();
                    ORD_NO = reader.ReadInt64();
                    CLR = reader.ReadString();
                    MDL_CNT = reader.ReadInt32();
                    MDL_NO = reader.ReadString();
                    WIN_CNT = reader.ReadInt32();
                    DESC = reader.ReadString();
                    Status = reader.ReadByte();
                    WIDTH = reader.ReadDecimal();
                    HEIGHT = reader.ReadDecimal();
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
            public short ManId;
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
                ManId = (short)mdd.ManId;
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
                    ManId = reader.ReadInt16();
                };
                return this;
            }

        }

        public struct orderDetails : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public long ORD_NO;                   //4
            public short MDL_CNT;                  //2
            public byte PAT_POS;
            public string MDL_NO;                   //4
            public string OPT_TYPE;                 //2
            public short OPT_NUM;                  //2
            public string STOCK_ID;                 //30
            public string CALL_SIZE;                //10
            public string CLR;                      //3
            public string DESC;                     //60
            public short QTY;                      //2
            public short SHP_QTY;                  //2
            public decimal WIDTH;                    //4
            public decimal HEIGHT;                   //4
            public string CMT1;                     //40
            public string CMT2;                     //40
            public long SHP_DTE;                  //4
            public byte[] TRUCK;
            public short SHP_SEQUENCE;             //2
            public short LINK_WIN_CNT;             //2
            public DateTime ScanTime;

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
                    $"\t\t{STOCK_ID + Environment.NewLine}" +
                    $"\t\t{CALL_SIZE + Environment.NewLine}" +
                    $"\t\t{CLR + Environment.NewLine}" +
                    $"\t\t{DESC + Environment.NewLine}" +
                    $"\t\t{QTY + Environment.NewLine}" +
                    $"\t\t{SHP_QTY + Environment.NewLine}" +
                    $"\t\t{WIDTH + Environment.NewLine}" +
                    $"\t\t{HEIGHT + Environment.NewLine}" +
                    $"\t\t{CMT1 + Environment.NewLine}" +
                    $"\t\t{CMT2 + Environment.NewLine}" +
                    $"\t\t{SHP_DTE + Environment.NewLine}" +
                    $"\t\t{TRUCK + Environment.NewLine}" +
                    $"\t\t{SHP_SEQUENCE + Environment.NewLine}" +
                    $"\t\t{LINK_WIN_CNT}";
            }

            public orderDetails(OrderDetailsData odd)
            {
                command = odd.Command;
                requestId = odd.RequestId.ToByteArray();
                ORD_NO = odd.ORD_NO;
                MDL_CNT = odd.MDL_CNT;
                PAT_POS = 0; // odd.PAT_POS;
                MDL_NO = odd.MDL_NO;
                OPT_TYPE = odd.OPT_TYPE; //new byte[fldsz_OPT_TYPE]; // odd.OPT_TYPE:
                OPT_NUM = odd.OPT_NUM;
                STOCK_ID = odd.STOCK_ID; //odd.STOCK_ID;
                CALL_SIZE = odd.CALL_SIZE; // odd.CALL_SIZE:
                CLR = odd.CLR;
                DESC = odd.DESC;
                QTY = odd.QTY; // new byte[fldsz_QTY]; //odd.QTY;
                SHP_QTY = odd.SHP_QTY; // new byte[fldsz_SHP_QTY];
                WIDTH = odd.WIDTH;
                HEIGHT = odd.HEIGHT;
                CMT1 = odd.CMT1;
                CMT2 = odd.CMT2;
                SHP_DTE = odd.SHP_DTE; 
                TRUCK = odd.RTE_CDE.GetBytes(fldsz_TRUCK);
                SHP_SEQUENCE = (short)odd.LineNumber;
                LINK_WIN_CNT = odd.WIN_CNT;
                ScanTime = odd.SCAN_DATE_TIME;
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
                    writer.Write(STOCK_ID); //30 bytes
                    writer.Write(CALL_SIZE);   //10 bytes
                    writer.Write(CLR);   //3 bytes
                    writer.Write(DESC);   //60 bytes
                    writer.Write(QTY);    //2 bytes
                    writer.Write(SHP_QTY);   //2 bytes
                    writer.Write(WIDTH);   //4 bytes
                    writer.Write(HEIGHT);   //4 bytes
                    writer.Write(CMT1);   //40 bytes
                    writer.Write(CMT2);   //40 bytes
                    writer.Write(SHP_DTE);   //4 bytes

                    if (TRUCK == null)
                        TRUCK = new byte[fldsz_TRUCK];
                    writer.Write(TRUCK);

                    writer.Write(SHP_SEQUENCE);   //2 bytes
                    writer.Write(LINK_WIN_CNT);   //2 bytes

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    ORD_NO = reader.ReadInt64();
                    MDL_CNT = reader.ReadInt16();
                    PAT_POS = reader.ReadByte();
                    MDL_NO = reader.ReadString();
                    OPT_TYPE = reader.ReadString();
                    OPT_NUM = reader.ReadInt16();
                    STOCK_ID = reader.ReadString();
                    CALL_SIZE = reader.ReadString();
                    CLR = reader.ReadString();
                    DESC = reader.ReadString();
                    QTY = reader.ReadInt16();
                    WIDTH = reader.ReadDecimal();
                    HEIGHT = reader.ReadDecimal();
                    CMT1 = reader.ReadString();
                    CMT2 = reader.ReadString();
                    SHP_DTE = reader.ReadInt64();
                    TRUCK = reader.ReadBytes(fldsz_TRUCK);
                    SHP_SEQUENCE = reader.ReadInt16();
                    //LINK_WIN_CNT = reader.ReadInt16();
                };
                return this;
            }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct orderOptions : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public long ORD_NO;                   //4
            public short MDL_CNT;                  //2
            public byte PAT_POS;
            public string MDL_NO;                   //4
            public string OPT_TYPE;                 //2
            public short OPT_NUM;                  //2
            public string CLR;                      //3
            public string DESC;                     //60

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
                    $"\t\t{CALL_SIZE + Environment.NewLine}" +
                    $"\t\t{CLR + Environment.NewLine}" +
                    $"\t\t{DESC}";
            }

            public orderOptions(OrderOptionsData ood)
            {
                command = ood.Command;
                requestId = ood.RequestId.ToByteArray();
                ORD_NO = ood.ORD_NO;
                MDL_CNT = ood.MDL_CNT;
                PAT_POS = ood.PAT_POS; // odd.PAT_POS;
                MDL_NO = ood.MDL_NO;
                OPT_TYPE = ood.OPT_TYPE;
                OPT_NUM = ood.OPT_NUM;
                CLR = ood.CLR;
                DESC = ood.DESC;
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
                    
                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    ORD_NO = reader.ReadInt64();
                    MDL_CNT = reader.ReadInt16();
                    PAT_POS = reader.ReadByte();
                    MDL_NO = reader.ReadString();
                    OPT_TYPE = reader.ReadString();
                    OPT_NUM = reader.ReadInt16();
                    CLR = reader.ReadString();
                    DESC = reader.ReadString();
                };
                return this;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct optionsDetail : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public long ORD_NO;                     //4
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
                    writer.Write((long)ORD_NO);
                    writer.Write((short)MDL_CNT);
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
                    s.ORD_NO = reader.ReadInt64();
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
        public class inventoryDetail : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }
            /*Inventory Detail TPS*
    inv_dtl.tps*/

            public long ORD_NO;
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
            /*12) 61/5 INVD:MAT_COST - Bcd
            13) 66/5 INVD:STD_COST - Bcd
            14) 71/5 INVD:OH_COST - Bcd
            15) 76/5 INVD:LBR_COST - Bcd
            16) 81/4 INVD:COS_ACCT - SignedLong
            17) 85/4 INVD:OTC_COS_ACCT - SignedLong
            18) 89/4 INVD:COS_CASC_ACCT - SignedLong
            19) 93/4 INVD:OTC_REV_ACCT - SignedLong
            20) 97/4 INVD:REV_CASC_ACCT - SignedLong*/
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

                    writer.Write((byte)eCommand.Orders);
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
                    ORD_NO = reader.ReadInt64();
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

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class orderMaster : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public long ORD_NO;
            public long DLR_NO;
            public int SHIP_DTE;        //Int32

            public string DLR_NME="";      //30
            public string DLR_ADDR = "";     //30
            public string DLR_ADDR2 = "";    //30
            public string DLR_TEL = "";      //12
            public short DLR_CT;       //short
            
            public string SHP_NME = "";      //30
            public string SHP_ADDR = "";     //30
            public string SHP_ADDR2 = "";    //30
            public string SHP_TEL = "";      //309/12 DLR:SHP_TEL - FixedLengthString
            public string SHP_ZIP = "";      //325/11 DLR:SHP_ZIP - FixedLengthString

            public string CUS_NME = "";
            public string RTE_CDE = "";      //4
            public short SHP_QTY;
            public OrderStatus Status;
            public long ManId;

            public decimal WIDTH { get; set; }
            public decimal HEIGHT { get; set; }
            public byte PAT_POS;
            public string OPT_TYPE;
            public DateTime SCAN_DATE_TIME { get; set; }


            #region disabled fields

            /*
            public byte[] DIVISION_ID;
            public byte[] FAX_ID;
            public byte[] FRAGGED;                    //
            public byte[] SYSGRP;                     //
            public byte SYSFLG;
            public byte[] SYSUSER;                    //
            public byte[] SYSDATE;                    //
            
            public byte[] SYSTIME;                    //
            public byte[] SHIP_DTE;                   //
            public byte[] CPY_GRP;                    //
            public short PARENT_NO;
            public byte[] STORE_NO;                   //
            public byte[] DLR_PO;                     //
            public byte[] ORD_DTE;                    //
            public byte[] O_SHP_DTE;                  //
            public byte FREEZE_SHIP;
            public byte OUTSIDE;                      //

            public byte[] ADD_DAYS;                   //
            public byte[] SM;                         //
            public byte[] COM_OVR;                    //
            public short COM;
            public byte[] CMT_GRP;                    //
            public byte[] CMNT1;                      //
            public byte[] CMNT2;                      //
            public byte[] DLR_GRP;                    //
            public byte[] DIR_GRP;
            public byte[] DIR_1;                      //4
            public byte[] DIR_2;                      //4
            public byte[] DIR_3;                      //40
            public byte[] DIR_4;                      //40
            public byte[] SHP_GRP;                    //4
            public byte[] SHP_CSZ;                      //30
            public short SHP_CT;                        //321/2 DLR:SHP_CT - SignedShort
            public byte[] CUS_ADDR;
            public byte[] CUS_CSZ;
            public byte[] CUS_TEL;
            public byte[] SHP_TYPE;                     //
            public byte[] CLASS;
            public byte[] ORD_TYPE;                   //
            public byte[] DEPOSIT;                    //
            public byte[] APP_TAX;                    //
            public byte[] TAX_PROFILE;                //
            public byte[] SALES_TAX_AMT;              //
        
            public byte[] ENT_BY;                     //
            public byte[] CON_PRT;
            public byte[] DTE_ENT;                    //
            public byte[] USER_SOR;                   //
            public byte[] WINSYSLITEORD;              //
            public byte[] HLD_GRP;                    //
            public byte[] HLD_FLG;                    //
            public byte[] HLD_BY;                     //
            public byte[] HLD_MSG;                    //
            public byte[] HLD_DTE;                    //

            public byte[] FRT_CHG;                    //
            public byte[] USE_COST_PLUS;
            public byte[] MARKUP_PCT;                 //
            public byte[] CRED_AUTH;                  //
            public byte[] ORD_AMT;                      //
            public byte[] ORD_GRS;                      //
            public byte[] CST_GRS;                    //
            public byte[] WIN_QTY;                    //
            public byte[] PAR_QTY;                    //
            public byte[] STK_QTY;                    //

            public byte[] CMT_QTY;                    //
            public byte[] SHP_AMT;                    //
            public byte[] LBR_GRP;                    //
            public byte[] LBR_INT;                    //
            public byte[] LBR_CHG;                    //
            public byte[] LBR_MAT;                    //
            public byte[] LBR_EXP;                    //
            public byte[] LBR_TOT;                    //
            public byte[] MISC_TEXT;                  //*/
#endregion

        public override string ToString()
            {
                var sReqId = "nullReqId";
                if (requestId != null)
                    sReqId = NewGuid(requestId).ToString();
                return $"{Environment.NewLine}\tCommand:{Enum.GetName(typeof(eCommand), command)}{Environment.NewLine}" +
                    $"\t\t{sReqId + Environment.NewLine}" +
                    $"\t\t{ORD_NO + Environment.NewLine}" +
                    $"\t\t{DLR_NO + Environment.NewLine}" +
                    $"\t\t{SHIP_DTE + Environment.NewLine}" +
                    $"\t\t{DLR_NME + Environment.NewLine}" +
                    $"\t\t{DLR_ADDR + Environment.NewLine}" +
                    $"\t\t{DLR_ADDR2 + Environment.NewLine}" +
                    $"\t\t{DLR_TEL + Environment.NewLine}" +
                    $"\t\t{DLR_CT + Environment.NewLine}" +
                    $"\t\t{SHP_NME + Environment.NewLine}" +
                    $"\t\t{SHP_ADDR + Environment.NewLine}" +
                    $"\t\t{SHP_ADDR2 + Environment.NewLine}" +
                    $"\t\t{SHP_TEL + Environment.NewLine}" +
                    $"\t\t{SHP_ZIP + Environment.NewLine}" +
                    $"\t\t{CUS_NME + Environment.NewLine}" +
                    $"\t\t{RTE_CDE + Environment.NewLine}" +
                    $"\t\t{SHP_QTY + Environment.NewLine}" +
                    $"\t\t{Enum.GetName(typeof(OrderStatus), Status)}";
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
                   // SHIP_DTE = omd.SHP_DTE;
                    DLR_NME = omd.DLR_NME;
                    DLR_ADDR = omd.DLR_ADDR;
                    DLR_ADDR2 = omd.DLR_ADDR2;
                    DLR_TEL = omd.DLR_TEL;
                    DLR_CT=omd.DLR_CT;

                    SHP_NME = omd.SHP_NME;
                    SHP_ADDR = omd.SHP_ADDR;
                    SHP_ADDR2 = omd.SHP_ADDR2;
                    SHP_TEL = omd.SHP_TEL;
                    SHP_ZIP = omd.SHP_ZIP;

                    CUS_NME = omd.CUS_NME;
                    RTE_CDE = omd.RTE_CDE;
                    SHP_QTY = (short)omd.SHP_QTY;
                     Status = omd.Status;
                     ManId = omd.ManId;
                    WIDTH = omd.WIDTH;
                    HEIGHT = omd.HEIGHT;
                    PAT_POS = omd.PAT_POS;
                    OPT_TYPE = omd.OPT_TYPE;
                    SCAN_DATE_TIME = omd.SCAN_DATE_TIME;
                    ManId = omd.ManifestId;
                }
                catch (Exception ex) { Logger.Error($"maniFestMaster Error:  {ex.Message}"); }
            }
            public orderMaster(OrderData ord, long id)
            {
                command = ord.Command;
                requestId = ord.RequestId.ToByteArray();

                ORD_NO = ord.ORD_NO;
                DLR_NO=ord.DLR_NO;
                Status = ord.Status;
             //SHIP_DTE=ord.SH;        //Int32

                //DLR_NME;      //30
                //DLR_ADDR;     //30
                //DLR_ADDR2;    //30
                //DLR_TEL;      //12
                //DLR_CT=0;       //short

                //SHP_NME;      //30
                //SHP_ADDR;     //30
                //SHP_ADDR2;    //30
                //SHP_TEL;      //309/12 DLR:SHP_TEL - FixedLengthString
                //SHP_ZIP;      //325/11 DLR:SHP_ZIP - FixedLengthString

                //CUS_NME;
                //RTE_CDE;      //4
                //SHP_QTY;

                //ManifestId = ord.ManifestId;
                //DSP_SEQ = ord.DSP_SEQ;
                //CustomerId = ord.CustomerId;
                //CLR = ord.CLR;
                //MDL_CNT = ord.MDL_CNT;
                //MDL_NO = ord.MDL_NO;
                //WIN_CNT = ord.WIN_CNT;
                //DESC = ord.DESC;
                //IsSelected = isSelected;
            }
            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    var writer = new BinaryWriter(stream);

                    writer.Write((byte)eCommand.Orders);
                    writer.Write(requestId);
                    writer.Write(ORD_NO);
                    writer.Write(DLR_NO);
                    writer.Write(SHIP_DTE);
                    writer.Write(DLR_NME);
                    writer.Write(DLR_ADDR);
                    writer.Write(DLR_ADDR2);
                    writer.Write(DLR_TEL);
                    writer.Write(DLR_CT);
                    writer.Write(SHP_NME);
                    writer.Write(SHP_ADDR);
                    writer.Write(SHP_ADDR2);
                    writer.Write(SHP_TEL);
                    writer.Write(SHP_ZIP);
                    writer.Write(CUS_NME);
                    writer.Write(RTE_CDE);
                    writer.Write(SHP_QTY);
                    writer.Write((Int16)Status);
                    writer.Write(ManId);

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();

                    requestId = reader.ReadBytes(fldsz_GUID);
                    ORD_NO = reader.ReadInt64(); ;
                    DLR_NO = reader.ReadInt64();

                    SHIP_DTE = reader.ReadInt32();
                    DLR_NME = reader.ReadString();

                    DLR_ADDR = reader.ReadString();
                    DLR_ADDR2 = reader.ReadString();
                    DLR_TEL = reader.ReadString();
                    DLR_CT = reader.ReadInt16();
                    SHP_NME = reader.ReadString();
                    SHP_ADDR=reader.ReadString();
                    SHP_ADDR2 = reader.ReadString();
                    SHP_TEL = reader.ReadString();
                    SHP_ZIP = reader.ReadString();
                    CUS_NME = reader.ReadString();
                    RTE_CDE = reader.ReadString();
                    SHP_QTY = reader.ReadInt16();
                    Status = (OrderStatus)reader.ReadInt16();
                    ManId = reader.ReadInt64();
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
                var reader = new BinaryReader(new MemoryStream(bytes));
                var s = default(RunQuery);

                s.command = (eCommand)reader.ReadByte();
                requestId = reader.ReadBytes(fldsz_GUID);
                s.Query = reader.ReadString();
                s.SP = (SPCmds)Enum.Parse(typeof(SPCmds), reader.ReadString());

                return s;
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
                var stream = new MemoryStream();
                var writer = new BinaryWriter(stream);
                writer.Write((byte)command);
                writer.Write(requestId);
                writer.Write(Message);
                writer.Write(Attachment);
                return stream.ToArray();
            }

            public isaCommand FromArray(byte[] bytes)
            {
                var reader = new BinaryReader(new MemoryStream(bytes));
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

