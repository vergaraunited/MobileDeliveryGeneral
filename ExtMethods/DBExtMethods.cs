using System;
using System.Text;

namespace MobileDeliveryGeneral.ExtMethods
{
    public static class DBExtMethods
    {
        public static byte[] GetBytes(this int I)
        {
            return BitConverter.GetBytes(I);
        }
        public static double GetInt(this byte[] b)
        {
            return BitConverter.ToInt32(b, 0);
        }
        public static DateTime GetDate(this byte[] d)
        {
            //deserilaize then convert to datetime
            string sd = BitConverter.ToString(d);
            return DateTime.Parse(sd);
        }
        //public static byte[] GetBytes2(string str, int length)
        //{
        //    return Encoding.ASCII.GetBytes(str.PadRight(length, ' '));
        //}
        public static byte[] StringToByteArray(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
        public static byte[] StringToByteArray(this string str, int length)
        {
            return Encoding.ASCII.GetBytes(str.PadRight(length, ' '));
        }

        public static byte[] GetBytes(this string str, int size=0)
        {
            if (size == 0)
                size = str.Length*sizeof(char);
            else if (size < str.Length)
                throw new Exception("buffer overflow in DBExtMethods::GetBytes");

            byte[] bytes = new byte[size];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, str.Length * sizeof(char));
            return bytes;
        }
        public static byte[] GetBytes(this DateTime dte)
        {
            byte[] bytes = new byte[dte.ToString().Length * sizeof(char)];
            System.Buffer.BlockCopy(dte.ToString().ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        
        public static byte[] GetBytes(this float I16)
        {
            return BitConverter.GetBytes(I16);
        }
        public static byte[] GetBytes(this Int16 I16)
        {
            return BitConverter.GetBytes(I16);
        }

        public static byte[] GetBytes(this Int64 I64)
        {
            return BitConverter.GetBytes(I64);
        }

        public static byte[] GetBytes(this decimal dec)
        {
            int [] arr = decimal.GetBits(dec);
            byte[] barr = new byte[arr.Length];

            int ret = -1;
            while (ret != 0)
                ret = IntToByte(barr, arr, arr.Length);

            return barr;
        }

        public static int IntToByte(byte[] arrayDst, int [] arrayOrg, int maxOrg)
        {
            int i;
            int idxDst;
            int maxDst;
            //
            maxDst = maxOrg * 4;
            //
            if (arrayDst == null)
                return 0;
            if (arrayOrg == null)
                return 0;
            if (arrayDst.Length < maxDst)
                return 0;
            if (arrayOrg.Length < maxOrg)
                return 0;
            //
            idxDst = 0;
            for (i = 0; i < maxOrg; i++)
            {
                // Copia o int, byte a byte.
                arrayDst[idxDst] = (byte)(arrayOrg[i]);
                idxDst++;
                arrayDst[idxDst] = (byte)(arrayOrg[i] >> 8);
                idxDst++;
                arrayDst[idxDst] = (byte)(arrayOrg[i] >> 16);
                idxDst++;
                arrayDst[idxDst] = (byte)(arrayOrg[i] >> 24);
                idxDst++;
            }
            //
            return idxDst;
        }

        public static int ByteToInt(int [] arrayDst, byte [] arrayOrg, int maxOrg)
        {
            int i;
            int v;
            int idxOrg;
            int maxDst;
            //
            maxDst = maxOrg / 4;
            //
            if (arrayDst == null)
                return 0;
            if (arrayOrg == null)
                return 0;
            if (arrayDst.Length < maxDst)
                return 0;
            if (arrayOrg.Length < maxOrg)
                return 0;
            //
            idxOrg = 0;
            for (i = 0; i < maxDst; i++)
            {
                arrayDst[i] = 0;
                //
                v = 0x000000FF & arrayOrg[idxOrg];
                arrayDst[i] = arrayDst[i] | v;
                idxOrg++;
                //
                v = 0x000000FF & arrayOrg[idxOrg];
                arrayDst[i] = arrayDst[i] | (v << 8);
                idxOrg++;
                //
                v = 0x000000FF & arrayOrg[idxOrg];
                arrayDst[i] = arrayDst[i] | (v << 16);
                idxOrg++;
                //
                v = 0x000000FF & arrayOrg[idxOrg];
                arrayDst[i] = arrayDst[i] | (v << 24);
                idxOrg++;
            }
            //
            return maxDst;
        }

    }
}
