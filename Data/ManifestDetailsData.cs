using System;
using System.Runtime.InteropServices;
using MobileDeliveryGeneral.ExtMethods;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
   // [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ManifestDetailsData : BaseData<ManifestDetailsData>
    {

        //public Guid RequestId { get; set; }
        public int LINK;
        public int ManId;
        public short DEL_SEQ;
        public short DSP_SEQ;
        public short EXTRA_STOP;
        public long LOADUNITS;   //6 bytes
        public short UNITSONTRUCK;
        //KEY <Group> (loc.15)
        public long DLR_NO;
        public string SHP_NME;  //30 bytes
        public string SHP_ADDR; //30
        public string SHP_ADDR2;//30
        public string SHP_CSZ;  //30
        public string SHP_TEL;  //12
                                //DIR_GRP <Group> (loc.151)
        public string DIR_1;  //44  
        public string DIR_2;
        public string DIR_3;
        public string DIR_4;

        //public eCommand Command { get; set; }
        /*
        // public Stop stop { get; set; }
        public String dealerPO { get; set; }
        public String orderDesc { get; set; }
        public int regularCnt { get; set; }
        public int partialCnt { get; set; }
        public int total { get; set; }
        */

        public ManifestDetailsData() : base()
        { }
        public ManifestDetailsData(manifestDetails dat)
        {
            Command = dat.command;
            RequestId = new Guid(dat.requestId);
            LINK = BitConverter.ToInt32(dat.LINK, 0);
            ManId = dat.ManId;
            DEL_SEQ = dat.DEL_SEQ;
            DSP_SEQ = dat.DSP_SEQ;
            EXTRA_STOP = dat.EXTRA_STOP;
            //LOADUNITS = BitConverter.ToInt64(dat.LOADUNITS, 0);   //6 bytes
            UNITSONTRUCK = dat.UNITSONTRUCK;
            DLR_NO = dat.DLR_NO;
            SHP_NME = dat.SHP_NME.UMToString(fldsz_SHP_NME);  //30 bytes
            SHP_ADDR = dat.SHP_ADDR.UMToString(fldsz_SHP_NME); //30
            SHP_ADDR2 = dat.SHP_ADDR2.UMToString(fldsz_SHP_NME);//30
            SHP_CSZ = dat.SHP_CSZ.UMToString(fldsz_SHP_NME);   //30
            SHP_TEL = dat.SHP_TEL.UMToString(fldsz_SHP_TEL);  //12

            DIR_1 = dat.DIR_1.UMToString(fldsz_DIR);  //44  
            DIR_2 = dat.DIR_2.UMToString(fldsz_DIR);
            DIR_3 = dat.DIR_3.UMToString(fldsz_DIR);
            DIR_4 = dat.DIR_4.UMToString(fldsz_DIR);
        }

        public ManifestDetailsData(ManifestDetailsData dat)
        {
            Command = dat.Command;
            RequestId = dat.RequestId;
            LINK = dat.LINK;
            ManId = dat.ManId;
            DEL_SEQ = dat.DEL_SEQ;
            DSP_SEQ = dat.DSP_SEQ;
            EXTRA_STOP=dat.EXTRA_STOP;
            LOADUNITS = dat.LOADUNITS;   //6 bytes
            UNITSONTRUCK = dat.UNITSONTRUCK;
            DLR_NO=dat.DLR_NO;
            SHP_NME=dat.SHP_NME;  //30 bytes
            SHP_ADDR=dat.SHP_ADDR; //30
            SHP_ADDR2=dat.SHP_ADDR2;//30
            SHP_CSZ=dat.SHP_CSZ;  //30
            SHP_TEL=dat.SHP_TEL;  //12

            DIR_1=dat.DIR_1;  //44  
            DIR_2=dat.DIR_2;
            DIR_3=dat.DIR_3;
            DIR_4=dat.DIR_4;
        }

        public override bool Equals(ManifestDetailsData other)
        {
            return DLR_NO == other.DLR_NO && DSP_SEQ == other.DSP_SEQ && LINK == other.LINK;
            //throw new NotImplementedException();
        }

        public override int CompareTo(ManifestDetailsData mmd)
        {
            if (mmd.LINK == this.LINK && this.DSP_SEQ == mmd.DSP_SEQ && this.SHP_ADDR == mmd.SHP_ADDR
                && this.SHP_NME == mmd.SHP_NME && this.SHP_TEL == mmd.SHP_TEL)
                return 0;
            else
                return 1;
        }
    }
}
