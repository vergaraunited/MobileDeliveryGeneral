using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Utilities
{

    public static class Serializer
    {
        /// <summary>
        /// converts byte[] to struct
        /// </summary>
        public static T RawDeserialize<T>(byte[] rawData, int position, int length=0)
        {
            try
            {
                //accept the length in order to process dynamilcally sized structures/messages.
                int rawsize = length;
                if (rawsize==0)
                    rawsize = Marshal.SizeOf(typeof(T));

                if (rawsize > rawData.Length - position)
                    throw new ArgumentException("Not enough data to fill struct. Array length from position: " + (rawData.Length - position) + ", Struct length: " + rawsize);
                IntPtr buffer = Marshal.AllocHGlobal(rawsize);
                Marshal.Copy(rawData, position, buffer, rawsize);
                T retobj = (T)Marshal.PtrToStructure(buffer, typeof(T));
                Marshal.FreeHGlobal(buffer);
                return retobj;
            }
            catch (Exception x) { }
            return default(T);
        }

        /// <summary>
        /// converts a struct to byte[]
        /// </summary>
        public static byte[] RawSerialize(object anything)
        {
            int rawSize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawDatas = new byte[rawSize];
            Marshal.Copy(buffer, rawDatas, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawDatas;
        }
        public static byte[] RawSerialize(isaCommand cmd, int length)
        {
            int rawSize = Marshal.SizeOf(cmd);// + cmd.value.GetLength(0);
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            IntPtr ptr = buffer;
            Marshal.StructureToPtr(cmd, buffer, false);
            //ptr = Marshal.SizeOf(cmd);
            //Marshal.StructureToPtr(cmd.value, ptr + Marshal.SizeOf(cmd), false);
            byte[] rawDatas = new byte[rawSize];
            Marshal.Copy(buffer, rawDatas, 0, rawSize);
            //byte[] rawVal = new byte[rawSize];
            //Marshal.Copy(valbuff, rawVal, 0, length);

            Marshal.FreeHGlobal(buffer);
            return rawDatas;
        }
    }
}
