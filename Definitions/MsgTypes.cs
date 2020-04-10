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
        public const ushort fldsz_CALL_SIZE=10;
        public const ushort fldsz_CLR=3;
        public const ushort fldsz_DESCOrd=60;
        
        public const ushort fldsz_WIDTH=4;
        public const ushort fldsz_HEIGHT=4;
        public const ushort fldsz_CMT1=40;
        public const ushort fldsz_CMT2=40;
        public const ushort fldsz_SHP_DTE=4;
        public const ushort fldsz_TRUCK = 2;
        public const ushort fldsz_FIRST_NAME = 32;
        public const ushort fldsz_LAST_NAME = 32;

        public const ushort fldsz_SHP_SEQUENCE=2;
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
            Manifest,         // = (1 << 1),
            ManifestLoadComplete,       // = (1 << 2),
            UploadManifest,

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
            Trucks,
            StopsLoadComplete,
            TrucksLoadComplete,
            OrdersLoadComplete
            // Unknown                     // = (1 << 17)
        };

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

                    writer.Write(requestId);
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

                        requestId = reader.ReadBytes(fldsz_GUID);

                        //Logger.Info($"isaCommand::FromArrray {s.command} : {requestId}");

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
            public short TRK_CDE; //2 bytes = short = each byte has 8 possible unique id's, so 16 possible TruckCodes - fldsz_TruckCode
            public int Stop; // 3 bytes - 32 possible Stops - fldsz_Stop
            public List<long> valist;


            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(requestId);
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
                    TRK_CDE = reader.ReadInt16();
                    Stop = reader.ReadInt32();

                    if (reader.BaseStream.Length > reader.BaseStream.Position)
                        valist = ExtensionMethods.BytesToList(reader.ReadBytes((int)(reader.BaseStream.Length - (reader.BaseStream.Position - 1))));

                    return this;
                }
                catch(Exception ex) { Logger.Debug($"Error manifestRequest (FromArray) ex= {ex.Message}"); }
                return null;

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
            public byte[] TRK_CDE;

            public byte[] DESC; //24 bytes fixed  -- should be  22, fix me
            public byte[] NOTES; //24 bytes fixed -- should be 20 , fix me
            public long SHIP_DTE;
            public short SHP_QTY;
            public byte TRUCKISCLOSED;

            public Decimal TOT_MILES;//4
            public Decimal CST_MILE; //4
            public Decimal CST_UNIT; //4

            public manifestMaster() { }

            public manifestMaster(ManifestMasterData mmd)
            {
                //command = mmd.Command;
                requestId = mmd.RequestId.ToByteArray();
                id = mmd.ManifestId;
                DriverId = Int32.Parse(mmd.Userid);
                LINK = mmd.LINK;
                TRK_CDE = mmd.TRK_CDE.GetBytes();
                DESC = mmd.Desc.StringToByteArray(fldsz_DESC);
                NOTES = mmd.NOTES.StringToByteArray(fldsz_NOTES * sizeof(char));

                SHIP_DTE = mmd.SHIP_DTE.Ticks;
                SHP_QTY = mmd.SHP_QTY;
                TRUCKISCLOSED = Convert.ToByte(mmd.TRUCKISCLOSED);
            }

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    writer.Write(id);
                    writer.Write(DriverId);
                    writer.Write(LINK);

                    if (TRK_CDE == null)
                        TRK_CDE = new byte[fldsz_TRK_CDE];
                    else
                        MobileDeliveryLogger.Logger.Debug("Size of TRK_CDE = " + TRK_CDE.Length);
                    writer.Write(TRK_CDE);

                    if (DESC == null)
                        DESC = new byte[fldsz_DESC];
                    else
                        MobileDeliveryLogger.Logger.Debug("Size of DESC = " + DESC.Length);
                    writer.Write(DESC);

                    if (NOTES == null)
                        NOTES = new byte[fldsz_NOTES * sizeof(char)];
                    else
                        MobileDeliveryLogger.Logger.Debug("Sisze of Notes " + NOTES.Length);

                    writer.Write(NOTES);

                    writer.Write(SHIP_DTE);
                    writer.Write(SHP_QTY);
                    writer.Write(TRUCKISCLOSED);

                    //if (md != null)
                    //{
                    //    foreach (var it in md.Values)
                    //    {
                    //        byte[] sdzBuff = it.ToArray();
                    //        stream.Write(sdzBuff, 0, sdzBuff.Length);
                    //    }
                    //}
                    return stream.ToArray();
                }
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
                        id = reader.ReadInt64();
                        DriverId = reader.ReadInt32();
                        LINK = reader.ReadInt64();
                        TRK_CDE = reader.ReadBytes(fldsz_TRK_CDE);
                        DESC = reader.ReadBytes(fldsz_DESC);
                        NOTES = reader.ReadBytes(fldsz_NOTES * sizeof(char));
                        SHIP_DTE = reader.ReadInt64();
                        SHP_QTY = reader.ReadInt16();

                        TRUCKISCLOSED = reader.ReadByte();

                        //TOT_MILES = reader.ReadDecimal();
                        //CST_MILE = reader.ReadDecimal();
                        //CST_UNIT = reader.ReadDecimal();

                     return this;
                    }
                }
                catch (Exception ex) { Logger.Debug($"Error manifestMaster (FromArray) ex= {ex.Message}"); }
                return null;
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
            public int MDL_NO;
            public int WIN_CNT;
            public string Status;

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
                MDL_NO = st.MDL_NO;
                WIN_CNT = st.WIN_CNT;
                Status = st.Status;
        }

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
                    if (Status == null)
                        writer.Write("Loaded");
                    else
                        writer.Write(Status);

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
                    MDL_NO = reader.ReadInt32();
                    WIN_CNT = reader.ReadInt32();
                    Status = reader.ReadString();

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
            public byte[] SHP_NME;  //30 bytes
            public byte[] SHP_ADDR; //30
            public byte[] SHP_ADDR2;//30
            public byte[] SHP_CSZ;  //30
            public byte[] SHP_TEL;  //12
            public byte[] DIR_1;  //44  
            public byte[] DIR_2;
            public byte[] DIR_3;
            public byte[] DIR_4;
            public short ManId;

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

                    if (SHP_NME == null)
                        SHP_NME = new byte[fldsz_SHP_NME];
                    writer.Write(SHP_NME);  //30 bytes
                    if (SHP_ADDR == null)
                        SHP_ADDR = new byte[fldsz_SHP_NME];
                    writer.Write(SHP_ADDR); //30
                    if (SHP_ADDR2 == null)
                        SHP_ADDR2 = new byte[fldsz_SHP_NME];
                    writer.Write(SHP_ADDR2); //30
                    if (SHP_CSZ == null)
                        SHP_CSZ = new byte[fldsz_SHP_NME];
                    writer.Write(SHP_CSZ); //30
                    if (SHP_TEL == null)
                        SHP_TEL = new byte[fldsz_SHP_TEL];
                    writer.Write(SHP_TEL); //12

                    //DIR_GRP <Group> (loc.151)
                    if (DIR_1 == null)
                        DIR_1 = new byte[fldsz_DIR];
                    writer.Write(DIR_1);   //44  
                    if (DIR_2 == null)
                        DIR_2 = new byte[fldsz_DIR];
                    writer.Write(DIR_2);   //44  
                    if (DIR_3 == null)
                        DIR_3 = new byte[fldsz_DIR];
                    writer.Write(DIR_3);//44  
                    if (DIR_4 == null)
                        DIR_4 = new byte[fldsz_DIR];
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
                    SHP_NME = reader.ReadBytes(fldsz_SHP_NME);  //30 bytes
                    SHP_ADDR = reader.ReadBytes(fldsz_SHP_NME); //30
                    SHP_ADDR2 = reader.ReadBytes(fldsz_SHP_NME); //30
                    SHP_CSZ = reader.ReadBytes(fldsz_SHP_NME);//30
                    SHP_TEL = reader.ReadBytes(fldsz_SHP_TEL); //12
                    //DIR_GRP <Group> (loc.151)
                    DIR_1 = reader.ReadBytes(fldsz_DIR);   //44  
                    DIR_2 = reader.ReadBytes(fldsz_DIR);   //44  
                    DIR_3 = reader.ReadBytes(fldsz_DIR);   //44  
                    DIR_4 = reader.ReadBytes(fldsz_DIR);    //44
                    ManId = reader.ReadInt16();
                };
                return this;
            }

        }

        public struct orderDetails : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public byte[] ORD_NO;                   //4
            public byte[] MDL_CNT;                  //2
            public byte PAT_POS;
            public byte[] MDL_NO;                   //4

            //OPT_TYPE, OPT_NUM, STOCK_ID, CALL_SIZE, 
            public byte[] OPT_TYPE;                 //2
            public byte[] OPT_NUM;                  //2
            public byte[] STOCK_ID;                 //30
            public byte[] CALL_SIZE;                //10

            public byte[] CLR;                      //3
            public byte[] DESC;                     //60

            //QTY, WIDTH, HEIGHT, CMT1, CMT2, SHP_DTE, TRUCK, SHP_SEQUENCE, LINK_WIN_CNT
            public byte[] QTY;                      //2
            public byte[] SHP_QTY;                  //2
            public byte[] WIDTH;                    //4
            public byte[] HEIGHT;                   //4
            public byte[] CMT1;                     //40
            public byte[] CMT2;                     //40
            public byte[] SHP_DTE;                  //4
            public byte[] TRUCK;
            public short SHP_SEQUENCE;             //2
            public byte[] LINK_WIN_CNT;             //2


            public orderDetails(OrderDetailsData odd)
            {
                command = odd.Command;
                requestId = odd.RequestId.ToByteArray();
                ORD_NO = odd.ORD_NO.GetBytes();
                MDL_CNT = odd.MDL_CNT.GetBytes();
                PAT_POS = 0; // odd.PAT_POS;
                MDL_NO = odd.MDL_NO.GetBytes();
                OPT_TYPE = new byte[fldsz_OPT_TYPE]; // odd.OPT_TYPE:
                OPT_NUM = odd.OPT_NUM.GetBytes();
                STOCK_ID = new byte[fldsz_STOCK_ID]; //odd.STOCK_ID;
                CALL_SIZE = new byte[fldsz_CALL_SIZE]; // odd.CALL_SIZE:
                CLR = odd.CLR.StringToByteArray(fldsz_CLR);
                DESC = odd.DESC.StringToByteArray(fldsz_DESC);
                QTY = new byte[fldsz_QTY]; //odd.QTY;
                SHP_QTY = new byte[fldsz_SHP_QTY];
                WIDTH = odd.WIDTH.GetBytes();
                HEIGHT = odd.HEIGHT.GetBytes();
                CMT1 = new byte[fldsz_CMT1]; //odd.CMT1;
                CMT2 = new byte[fldsz_CMT2]; //odd.CMT2;
                SHP_DTE = new byte[fldsz_SHP_DTE]; //odd.SHP_DTE.GetBytes(); 
                TRUCK = odd.RTE_CDE.GetBytes();
                SHP_SEQUENCE = (short)odd.LineNumber;
                LINK_WIN_CNT = odd.WIN_CNT.GetBytes();
            }

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    var writer = new BinaryWriter(stream);
                    //writer.Write((byte)eCommand.OrderDetails);
                    writer.Write((byte)command);
                    writer.Write(requestId);
                    if (ORD_NO == null)
                        ORD_NO = new byte[fldsz_MDL_NO];
                    writer.Write(ORD_NO);

                    if (MDL_CNT == null)
                        MDL_CNT = new byte[fldsz_MDL_CNT];
                    writer.Write(MDL_CNT);

                    writer.Write(PAT_POS);

                    if (MDL_NO == null)
                        MDL_NO = new byte[fldsz_MDL_NO];
                    writer.Write(MDL_NO);   //4 bytes

                    if (OPT_TYPE == null)
                        OPT_TYPE = new byte[fldsz_OPT_TYPE];
                    writer.Write(OPT_TYPE);

                    if (OPT_NUM == null)
                        OPT_NUM = new byte[fldsz_OPT_NUM];
                    writer.Write(OPT_NUM);

                    if (STOCK_ID == null)
                        STOCK_ID = new byte[fldsz_STOCK_ID];
                    writer.Write(STOCK_ID); //30 bytes

                    if (CALL_SIZE == null)
                        CALL_SIZE = new byte[fldsz_CALL_SIZE];
                    writer.Write(CALL_SIZE);   //10 bytes

                    if (CLR == null)
                        CLR = new byte[fldsz_CLR];
                    writer.Write(CLR);   //3 bytes

                    if (DESC == null)
                        DESC = new byte[fldsz_DESCOrd];
                    writer.Write(DESC);   //60 bytes

                    if (QTY == null)
                        QTY = new byte[fldsz_QTY];
                    writer.Write(QTY);    //2 bytes

                    if (SHP_QTY == null)
                        SHP_QTY = new byte[fldsz_SHP_QTY];
                    writer.Write(SHP_QTY);   //2 bytes

                    if (WIDTH == null)
                        WIDTH = new byte[fldsz_WIDTH];
                    writer.Write(WIDTH);   //4 bytes

                    if (HEIGHT == null)
                        HEIGHT = new byte[fldsz_HEIGHT];
                    writer.Write(HEIGHT);   //4 bytes

                    if (CMT1 == null)
                        CMT1 = new byte[fldsz_CMT1];
                    writer.Write(CMT1);   //40 bytes

                    if (CMT2 == null)
                        CMT2 = new byte[fldsz_CMT2];
                    writer.Write(CMT2);   //40 bytes

                    if (SHP_DTE == null)
                        SHP_DTE = new byte[fldsz_SHP_DTE];
                    writer.Write(SHP_DTE);   //4 bytes

                    if (TRUCK == null)
                        TRUCK = new byte[fldsz_TRUCK];
                    writer.Write(TRUCK);

                    writer.Write(SHP_SEQUENCE);   //2 bytes

                    if (LINK_WIN_CNT == null)
                        LINK_WIN_CNT = new byte[fldsz_LINK_WIN_CNT];
                    writer.Write(LINK_WIN_CNT);   //2 bytes

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                ///  var s = new orderOptions();

                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    ORD_NO = reader.ReadBytes(fldsz_MDL_NO);

                    MDL_CNT = reader.ReadBytes(fldsz_MDL_CNT);
                    PAT_POS = reader.ReadByte();
                    MDL_NO = reader.ReadBytes(fldsz_MDL_NO);
                    OPT_TYPE = reader.ReadBytes(fldsz_OPT_TYPE);
                    OPT_NUM = reader.ReadBytes(fldsz_OPT_TYPE);
                    STOCK_ID = reader.ReadBytes(fldsz_STOCK_ID);
                    CALL_SIZE = reader.ReadBytes(fldsz_CALL_SIZE);
                    CLR = reader.ReadBytes(fldsz_CLR);
                    DESC = reader.ReadBytes(fldsz_DESCOrd);
                    QTY = reader.ReadBytes(fldsz_QTY);
                    WIDTH = reader.ReadBytes(fldsz_WIDTH);
                    HEIGHT = reader.ReadBytes(fldsz_HEIGHT);
                    CMT1 = reader.ReadBytes(fldsz_CMT1);
                    CMT2 = reader.ReadBytes(fldsz_CMT2);
                    SHP_DTE = reader.ReadBytes(fldsz_SHP_DTE);
                    TRUCK = reader.ReadBytes(fldsz_TRUCK);
                    SHP_SEQUENCE = reader.ReadInt16();
                    LINK_WIN_CNT = reader.ReadBytes(fldsz_LINK_WIN_CNT);
                };
                return this;
            }
        }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct orderOptions : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public byte[] ORD_NO;                   //4
            public byte[] MDL_CNT;                  //2
            public byte PAT_POS;
            public byte[] MODEL;                   //4
            public byte[] MDL_NO;                   //4
            public byte[] OPT_TYPE;                 //2
            public byte[] OPT_NUM;                  //2
            public byte[] CLR;                      //3
            public byte[] DESC;                     //60


            public orderOptions(OrderOptionsData ood)
            {
                command = ood.Command;
                requestId = ood.RequestId.ToByteArray();
                ORD_NO = ood.ORD_NO.GetBytes();
                MDL_CNT = ood.MDL_CNT.GetBytes();
                PAT_POS = ood.PAT_POS; // odd.PAT_POS;
                MODEL = ood.MODEL.GetBytes(); ;
                MDL_NO = ood.MDL_NO.GetBytes();
                OPT_TYPE = ood.OPT_TYPE.GetBytes();
                OPT_NUM = ood.OPT_NUM.GetBytes();
                CLR = ood.CLR.StringToByteArray(fldsz_CLR);
                DESC = ood.DESC.StringToByteArray(fldsz_DESC);
            }

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    var writer = new BinaryWriter(stream);

                    writer.Write((byte)command);
                    writer.Write(requestId);
                    if (ORD_NO == null)
                        ORD_NO = new byte[fldsz_MDL_NO];
                    writer.Write(ORD_NO);

                    if (MDL_CNT == null)
                        MDL_CNT = new byte[fldsz_MDL_CNT];
                    writer.Write(MDL_CNT);

                    writer.Write(PAT_POS);

                    if (MODEL == null)
                        MODEL = new byte[fldsz_MODEL];
                    writer.Write(MODEL);   //4 bytes

                    if (MDL_NO == null)
                        MDL_NO = new byte[fldsz_MDL_NO];
                    writer.Write(MDL_NO);   //4 bytes

                    if (OPT_TYPE == null)
                        OPT_TYPE = new byte[fldsz_OPT_TYPE];
                    writer.Write(OPT_TYPE);

                    if (OPT_NUM == null)
                        OPT_NUM = new byte[fldsz_OPT_NUM];
                    writer.Write(OPT_NUM);

                    if (CLR == null)
                        CLR = new byte[fldsz_CLR];
                    writer.Write(CLR);   //3 bytes

                    if (DESC == null)
                        DESC = new byte[fldsz_DESCOrd];
                    writer.Write(DESC);   //60 bytes
                    
                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
              ///  var s = new orderOptions();

                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();
                    requestId = reader.ReadBytes(fldsz_GUID);
                    ORD_NO = reader.ReadBytes(fldsz_MDL_NO);
                    MDL_CNT = reader.ReadBytes(fldsz_MDL_CNT);
                    PAT_POS = reader.ReadByte();
                    MODEL = reader.ReadBytes(fldsz_MODEL);
                    MDL_NO = reader.ReadBytes(fldsz_MDL_NO);
                    OPT_TYPE = reader.ReadBytes(fldsz_OPT_TYPE);
                    OPT_NUM = reader.ReadBytes(fldsz_OPT_TYPE);
                    CLR = reader.ReadBytes(fldsz_CLR);
                    DESC = reader.ReadBytes(fldsz_DESCOrd);
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
            public byte[] MDL_NO;                   //4
            public byte[] OPT_TYPE;                 //2
            public short OPT_NUM;                   //2
            public byte[] CLR;                      //3
            public byte[] DESC;                     //60
            public byte[] OPT_SOURCE;               //5
            
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

                    if (MDL_NO == null)
                        MDL_NO = new byte[fldsz_MDL_NO];
                    writer.Write(MDL_NO);   //4 bytes

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
                    s.MDL_NO = reader.ReadBytes(fldsz_MDL_NO);                   //4
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
        public struct orderMaster : isaCommand
        {
            public eCommand command { get; set; }
            public byte[] requestId { get; set; }

            public byte[] ORD_NO;
            public byte[] DLR_NO;
            public int SHIP_DTE;        //Int32

            public byte[] DLR_NME;      //30
            public byte[] DLR_ADDR;     //30
            public byte[] DLR_ADDR2;    //30
            public byte[] DLR_TEL;      //12
            public short DLR_CT;       //short
            
            public byte[] SHP_NME;      //30
            public byte[] SHP_ADDR;     //30
            public byte[] SHP_ADDR2;    //30
            public byte[] SHP_TEL;      //309/12 DLR:SHP_TEL - FixedLengthString
            public byte[] SHP_ZIP;      //325/11 DLR:SHP_ZIP - FixedLengthString

            public byte[] CUS_NME;
            public byte[] RTE_CDE;      //4
            public byte[] SHP_QTY;

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

            public byte[] ToArray()
            {
                using (var stream = manager.GetStream())
                {
                    var writer = new BinaryWriter(stream);

                    writer.Write((byte)eCommand.Orders);
                    writer.Write(requestId);
                    if (ORD_NO == null)
                        ORD_NO = new byte[fldsz_ORD_NO];
                    writer.Write(ORD_NO);

                    if (DLR_NO == null)
                        DLR_NO = new byte[fldsz_ORD_NO];
                    writer.Write(DLR_NO);


                    writer.Write(SHIP_DTE);

                    if (DLR_NME == null)    
                        DLR_NME = new byte[fldsz_SHP_NME];
                    writer.Write(DLR_NME);

                    if (DLR_ADDR == null)
                        DLR_ADDR = new byte[fldsz_SHP_NME];
                    writer.Write(DLR_ADDR); 

                    if (DLR_ADDR2 == null)
                        DLR_ADDR2 = new byte[fldsz_SHP_NME];
                    writer.Write(DLR_ADDR2); 

                    if (DLR_TEL == null)
                        DLR_TEL = new byte[fldsz_SHP_TEL];
                    writer.Write(DLR_TEL);

                    writer.Write(DLR_CT);

                    if (SHP_NME == null)
                    SHP_NME = new byte[fldsz_SHP_NME];
                    writer.Write(SHP_NME);

                    if (SHP_ADDR == null)
                        SHP_ADDR = new byte[fldsz_SHP_NME];
                    writer.Write(SHP_ADDR);

                    if (SHP_ADDR2 == null)
                        SHP_ADDR2 = new byte[fldsz_SHP_NME];
                    writer.Write(SHP_ADDR2);

                    if (SHP_TEL == null)
                        SHP_TEL = new byte[fldsz_SHP_TEL];
                    writer.Write(SHP_TEL);

                    if (SHP_ZIP == null)
                        SHP_ZIP = new byte[fldsz_SHP_ZIP];
                    writer.Write(SHP_ZIP);
                   
                    if (CUS_NME == null)
                        CUS_NME = new byte[fldsz_CUS_NME];
                    writer.Write(CUS_NME);

                    if (RTE_CDE == null)
                        RTE_CDE = new byte[fldsz_RTE_CDE];
                    writer.Write(RTE_CDE);

                    if (SHP_QTY == null)
                        SHP_QTY = new byte[fldsz_SHP_QTY];
                    writer.Write(SHP_QTY);

                    return stream.ToArray();
                }
            }

            public isaCommand FromArray(byte[] bytes)
            {
                using (var reader = new BinaryReader(new MemoryStream(bytes)))
                {
                    command = (eCommand)reader.ReadByte();

                    requestId = reader.ReadBytes(fldsz_GUID);
                    if (ORD_NO == null)
                        ORD_NO = new byte[fldsz_ORD_NO];
                    ORD_NO = reader.ReadBytes(fldsz_ORD_NO);
                    
                    if (DLR_NO == null)
                        DLR_NO = new byte[fldsz_ORD_NO];
                    DLR_NO = reader.ReadBytes(fldsz_ORD_NO);

                    SHIP_DTE = reader.ReadInt32();

                    if (DLR_NME == null)
                        DLR_NME = new byte[fldsz_SHP_NME];
                    DLR_NME = reader.ReadBytes(fldsz_SHP_NME);

                    if (DLR_ADDR == null)
                        DLR_ADDR = new byte[fldsz_SHP_NME];
                    DLR_ADDR = reader.ReadBytes(fldsz_SHP_NME);

                    if (DLR_ADDR2 == null)
                        DLR_ADDR2 = new byte[fldsz_SHP_NME];
                    DLR_ADDR2 = reader.ReadBytes(fldsz_SHP_NME);

                    if (DLR_TEL == null)
                        DLR_TEL = new byte[fldsz_SHP_TEL];
                    DLR_TEL = reader.ReadBytes(fldsz_SHP_TEL);

                    DLR_CT = reader.ReadInt16();

                    if (SHP_NME == null)
                        SHP_NME = new byte[fldsz_SHP_NME];
                    SHP_NME = reader.ReadBytes(fldsz_SHP_NME);

                    if (SHP_ADDR == null)
                        SHP_ADDR = new byte[fldsz_SHP_NME];
                    SHP_ADDR=reader.ReadBytes(fldsz_SHP_NME);
                    
                    if (SHP_ADDR2 == null)
                        SHP_ADDR2 = new byte[fldsz_SHP_NME];
                    SHP_ADDR2 = reader.ReadBytes(fldsz_SHP_NME);

                    if (SHP_TEL == null)
                        SHP_TEL = new byte[fldsz_SHP_TEL];
                    SHP_TEL = reader.ReadBytes(fldsz_SHP_TEL);

                    if (SHP_ZIP == null)
                        SHP_ZIP = new byte[fldsz_SHP_ZIP];
                    SHP_ZIP = reader.ReadBytes(fldsz_SHP_ZIP);

                    if (CUS_NME == null)
                        CUS_NME = new byte[fldsz_CUS_NME];
                    CUS_NME = reader.ReadBytes(fldsz_CUS_NME);

                    if (RTE_CDE == null)
                        RTE_CDE = new byte[fldsz_RTE_CDE];
                    RTE_CDE = reader.ReadBytes(fldsz_RTE_CDE);

                    if (SHP_QTY == null)
                        SHP_QTY = new byte[fldsz_SHP_QTY];
                    SHP_QTY = reader.ReadBytes(fldsz_SHP_QTY);
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

