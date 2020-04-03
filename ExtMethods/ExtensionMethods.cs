using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.ExtMethods
{

    public static class ExtensionMethods
    {
        static RecyclableMemoryStreamManager manager = new RecyclableMemoryStreamManager();
        public static List<Int64> BytesToList(byte[] bytes)
        {
            var list = new List<Int64>();
            for (int i = 0; i < bytes.Length; i += sizeof(Int64))
                list.Add(BitConverter.ToInt64(bytes, i));

            return list;
        }
        public static byte[] DictionaryToCmdBytes(Dictionary<short, isaCommand> dictMD)
        {
            Byte[] bytesMD = new byte[] { };

            using (var stream = manager.GetStream())
            using (BinaryWriter _writer = new BinaryWriter(stream))
            {
                _writer.Write(dictMD.Count);

                foreach (var pair in dictMD)
                {
                    _writer.Write(pair.Key);
                    _writer.Write(pair.Value.ToArray());
                }
                //return _writer;
                bytesMD = stream.ToArray();
            }
            return bytesMD;
        }

        public static Dictionary<short, T> BytesToMDDictionary<T>(byte[] bytes)
            where T : isaCommand, new()
        {
            var dictMD = new Dictionary<short, T>();
            T it = new T();
            using (var stream = manager.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            {
                // Get count.
                int count = reader.ReadInt32();
                // Read in all pairs.
                for (int i = 0; i < count; i++)
                {
                    short key = reader.ReadInt16();
                    T value = (T)it.FromArray(reader.ReadBytes((int)(reader.BaseStream.Length - (reader.BaseStream.Position - 1))));
                    dictMD[key] = value;
                }
            }

            return dictMD;
        }
        public static byte[] WriteList<T>(this List<T> value)
        {
            Byte[] barr = new byte[] { };

            using (var stream = manager.GetStream())
            {
                BinaryWriter _writer = new BinaryWriter(stream);

                for (int i = 0; i < value.Count; i++)
                {
                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        //_writer.Write(value[i]);
                        case TypeCode.Boolean:
                            _writer.Write((bool)(object)value[i]);
                            break;
                        case TypeCode.Byte:
                            _writer.Write((byte)(object)value[i]);
                            break;
                        case TypeCode.Char:
                            _writer.Write((char)(object)value[i]);
                            break;
                        case TypeCode.Decimal:
                            _writer.Write((decimal)(object)value[i]);
                            break;
                        case TypeCode.Double:
                            _writer.Write((double)(object)value[i]);
                            break;
                        case TypeCode.Single:
                            _writer.Write((float)(object)value[i]);
                            break;
                        case TypeCode.Int16:
                            _writer.Write((short)(object)value[i]);
                            break;
                        case TypeCode.Int32:
                            _writer.Write((int)(object)value[i]);
                            break;
                        case TypeCode.Int64:
                            _writer.Write((long)(object)value[i]);
                            break;
                        case TypeCode.String:
                            _writer.Write((string)(object)value[i]);
                            break;
                        case TypeCode.SByte:
                            _writer.Write((sbyte)(object)value[i]);
                            break;
                        case TypeCode.UInt16:
                            _writer.Write((ushort)(object)value[i]);
                            break;
                        case TypeCode.UInt32:
                            _writer.Write((uint)(object)value[i]);
                            break;
                        case TypeCode.UInt64:
                            _writer.Write((ulong)(object)value[i]);
                            break;
                        default:
                            if (typeof(T) == typeof(byte[]))
                            {
                                _writer.Write((byte[])(object)value[i]);
                            }
                            else if (typeof(T) == typeof(char[]))
                            {
                                _writer.Write((char[])(object)value[i]);
                            }
                            else
                            {
                                throw new ArgumentException("List type not supported");
                            }
                            break;
                    }
                }
                barr = stream.ToArray();
            }
            return barr;
        }
        public static double FromGregorianToJulian(this DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;

            DateTime dtCmp = new DateTime(year, month, day);
            TimeSpan tsp = dtCmp - new DateTime(1800, 12, 28, 0, 0, 0);
            return tsp.TotalDays;
            //return day + (153 * month - 457) / 5 + 365 * year + (year / 4) - (year / 100) + (year / 400) + 1721119;
        }

        public static string FromJulianToGregorian(long julianDate, string format = "dd/MM/yyyy")
        {
            try
            {
                var date = new DateTime(1800, 12, 28, 0, 0, 0);
                var theDate = date.AddDays(julianDate);

                // example format "dd/MM/yyyy"
                return theDate.ToString(format);
            }
            catch (Exception ex)
            {
                MobileDeliveryLogger.Logger.Debug("Error \"FromJulian\" : " + ex.Message);
            }
            return null;
        }

        public static DateTime FromJulianToGregorianDT(long julianDate, string format)
        {
            try
            {
                var date = new DateTime(1800, 12, 28); //, 0, 0, 0);
                var theDate = date.AddDays(julianDate);

                // example format "dd/MM/yyyy"
                return theDate.Date;
            }
            catch (Exception ex)
            {
                MobileDeliveryLogger.Logger.Debug("Error \"FromJulian\" : " + ex.Message);
            }
            return DateTime.Now;
        }
        public static double FromGregorianToJulianNew(this DateTime date)
        {
            return date.ToOADate() + 2415018.5;
        }

        public static char[] ReadNullTerminatedString(this System.IO.BinaryReader stream)
        {
            string str = "";
            char ch;

            while ((int)(ch = stream.ReadChar()) != 0)
                str = str + ch;

            return str.ToCharArray();
        }

        // TRK_CDE  
        public static String UMToString(this byte[] byteData, int length)
        {
            char[] charData = new char[length];
            for (int i = 0; i < byteData.Length; i++)
            {
                charData[i] = (char)(((int)byteData[i]) & 0xFF);
            }
            return new String(charData).Trim('\0');
        }
        public static byte[] GetBytes(this string str)
        {
            byte[] data = new byte[str.Length * 2];
            for (int i = 0; i < str.Length; ++i)
            {
                char ch = str[i];
                data[i * 2] = (byte)(ch & 0xFF);
                data[i * 2 + 1] = (byte)((ch & 0xFF00) >> 8);
            }

            return data;
        }
        public static string GetString(this byte[] arr)
        {
            char[] ch = new char[arr.Length / 2];
            for (int i = 0; i < ch.Length; ++i)
            {
                ch[i] = (char)((int)arr[i * 2] + (((int)arr[i * 2 + 1]) << 8));
            }
            return new String(ch);
        }

        //public static string GetString(this byte[] bytes)
        //{
        //    char[] chars = new char[bytes.Length / sizeof(char)+1];
        //    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        //    return new string(chars);
        //}
    }

}
