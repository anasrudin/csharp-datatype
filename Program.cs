using System;

public enum TypeDescription
{
    NullData = 0,
    Array = 1,
    Structure = 2,
    Boolean = 3,
    BitString = 4,
    DoubleLong = 5,
    DoubleLongUnsigned = 6,
    OctetString = 9,
    VisibleString = 10,
    UTF8String = 12,
    BCD = 13,
    Integer = 0xF,
    Long = 0x10,
    Unsigned = 17,
    LongUnsigned = 18,
    CompactArray = 19,
    Long64 = 20,
    Long64Unsigned = 21,
    Enum = 22,
    Float = 23,
    Double = 24,
    DateTime = 25,
    Date = 26,
    Time = 27
}

public interface ITranslator
{
    TypeDescription GetTypeName(string hextoken);
    string Translate(string rawdata);
}



public class Translator : ITranslator
    {
        public TypeDescription GetTypeName(string hextoken)
        {
            byte[] Bytes = new byte[2];
            Bytes[0] = Convert.ToByte(hextoken.Substring(0, 2), 16);
            TypeDescription type = (TypeDescription)Bytes[0];
            return type;
        }

        public string Translate(string rawData)
        {
            int offset = 0;
            string result = "";
            while (offset < rawData.Length)
            {
                if (offset >= rawData.Length) break;
                byte[] Bytes = new byte[2];
                Bytes[0] = Convert.ToByte(rawData.Substring(offset, 2), 16);
                var data0 = rawData.Substring(offset, 2);
                TypeDescription type = (TypeDescription)Bytes[0];
                Bytes[1] = Convert.ToByte(rawData.Substring(offset + 2, 2), 16);
                result += "Type: " + type.ToString() + " (" + Bytes[1].ToString() + ")  ";
                offset += 4;

                switch (type)
                {
                    case TypeDescription.NullData:
                        break;
                    case TypeDescription.Array:
                        break;
                    case TypeDescription.Structure:
                        result += "\n";
                        break;
                    case TypeDescription.Boolean:
                        bool val;
                        if (Bytes[1].ToString() == "1")
                        {
                            val = true;
                        }
                        else
                        {
                            val = false;
                        }
                        result += "Value: " + val + "\n";
                        break;
                    case TypeDescription.BitString:
                        result += "Value: " + Convert.ToString(Convert.ToInt32(rawData.Substring(offset, 8), 16), 2) + "\n";
                        offset += 8;
                        break;
                    case TypeDescription.DoubleLong:
                        break;
                    case TypeDescription.DoubleLongUnsigned:
                        break;
                    case TypeDescription.OctetString:
                        result += "Value: " + rawData.Substring(offset, (Bytes[1] * 2)) + "\n";
                        offset += (Bytes[1] * 2);
                        break;
                    case TypeDescription.VisibleString:
                        result += "Value: " + rawData.Substring(offset, Bytes[1]) + "\n";
                        offset += 4;
                        break;
                    case TypeDescription.UTF8String:
                        break;
                    case TypeDescription.BCD:
                        break;
                    case TypeDescription.Integer:
                        result += "Value: " + Bytes[1].ToString() + "\n";
                        break;
                    case TypeDescription.Long:
                        var dec = Convert.ToInt64(rawData.Substring(offset-2, 4), 16);
                        result += "Value: " + dec + "\n";
                        offset += ((Bytes[1]) * 2);
                        break;
                    case TypeDescription.Unsigned:
                        result += "Value: " + Bytes[1].ToString() + "\n";
                        break;
                    case TypeDescription.LongUnsigned:
                        break;
                    case TypeDescription.CompactArray:
                        break;
                    case TypeDescription.Long64:
                        result += "Value: " + Convert.ToInt64(rawData.Substring(offset, 16), 16).ToString() + "\n";
                        offset += 16;
                        break;
                    case TypeDescription.Long64Unsigned:
                        break;
                    case TypeDescription.Enum:
                        result += "Value: " + Bytes[1].ToString() + "\n";
                        result += "Value: " + Bytes[1].ToString() + "\n";
                        offset += 2;
                        break;
                    case TypeDescription.Float:
                        break;
                    case TypeDescription.Double:
                        break;
                    case TypeDescription.DateTime:
                        break;
                    case TypeDescription.Date:
                        result += "Value: " + DateTime.ParseExact(rawData.Substring(offset, 8), "yyyyMMdd", null).ToString() + "\n";
                        offset += 8;
                        break;
                    case TypeDescription.Time:
                        result += "Value: " + DateTime.ParseExact(rawData.Substring(offset, 8), "HHmmssff", null).ToString() + "\n";
                        offset += 8;
                        break;
                }
            }
            return result;
        }


    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of Translator class
            ITranslator translator = new Translator();

            string rawdata1 = "020909060000010000FF090C07E7020F03031D1000FE20001001E01100090C000003FE07020000008000FF090C00000AFE07030000008000FF0F3C03001601";
            string rawdata2 = "020209060000830008FF02090300030103000420FFFFFFFE0420FFFFFFF9111E110A1101111E";

            Console.WriteLine(translator.Translate(rawdata1));
            Console.WriteLine(translator.Translate(rawdata2));

            Console.ReadLine();
        }
    }
}


