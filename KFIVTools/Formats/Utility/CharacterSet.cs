using System.Text;

namespace KFIV.Utility.Charset
{
    public class CharacterSet
    {
        public static string ConvertKFStringLength(ushort[] data, int length)
        {
            //Use a string builder purely 'cause the code looks cleaner
            StringBuilder kfString = new StringBuilder();

            //Loop until length
            for(int i = 0; i < length; ++i)
            {
                kfString.Append(ConvertCharacter(data[i]));
            }

            return kfString.ToString();
        }

        public static string ConvertKFStringTerminated(ushort[] data)
        {
            StringBuilder kfString = new StringBuilder();
            ushort currentChar = 0x0000;
            int i = 0;

            while(currentChar != 0xFFFF)
            {
                //Some logic for line termination
                currentChar = data[i];
                if (currentChar == 0xFFFF)
                    break;

                kfString.Append(ConvertCharacter(data[i]));

                i++;
            }

            return kfString.ToString();
        }

        public static string ConvertCharacter(ushort data)
        {
            //This... Is fucking horrible.
            switch(data)
            {
                //Japanese Kanji/Kana
                case 0x0056: return "し";
                case 0x0063: return "の";
                case 0x0089: return "び";
                case 0x009B: return "ア";
                case 0x009C: return "イ";
                case 0x00A6: return "ッ";    //Could also be "ツ"?..
                case 0x00A9: return "ン";
                case 0x00AE: return "ト";
                case 0x00B6: return "フ";
                case 0x00C3: return "ル";
                case 0x00D8: return "ド";
                case 0x00DD: return "ボ";
                case 0x00E3: return "ァ";
                case 0x00EA: return "ョ";
                case 0x00F4: return "ー";

                case 0x0100: return "魔";
                case 0x0104: return "法";
                case 0x0112: return "回";
                case 0x0127: return "火";
                case 0x012C: return "装";
                case 0x012D: return "風";
                case 0x014D: return "備";
                case 0x014F: return "力";
                case 0x016A: return "像";
                case 0x016D: return "毒";
                case 0x016E: return "石";
                case 0x01AB: return "無";
                case 0x01AE: return "草";
                case 0x01B1: return "薬"; 
                case 0x01BC: return "復";
                case 0x01C5: return "消";

                case 0x0256: return "棒";
                case 0x0259: return "棍";

                case 0x039B: return "滅";
                case 0x03AA: return "糧";

                //Grammar #1
                case 0x0800: return " ";
                case 0x0801: return "!";
                case 0x0807: return "'";
                case 0X080C: return ",";
                case 0x080D: return "-";
                case 0x080E: return ".";

                //Numbers
                case 0x0810: return "0";
                case 0x0811: return "1";
                case 0x0812: return "2";
                case 0x0813: return "3";
                case 0x0814: return "4";
                case 0x0815: return "5";
                case 0x0816: return "6";
                case 0x0817: return "7";
                case 0x0818: return "8";
                case 0x0819: return "9";

                //Grammar #2
                case 0x081B: return ";";
                case 0x081F: return "?";

                //Uppercase
                case 0x0821: return "A";
                case 0x0822: return "B";
                case 0x0823: return "C";
                case 0x0824: return "D";
                case 0x0825: return "E";
                case 0x0826: return "F";
                case 0x0827: return "G";
                case 0x0828: return "H";
                case 0x0829: return "I";
                case 0x082A: return "J";
                case 0x082B: return "K";
                case 0x082C: return "L";
                case 0x082D: return "M";
                case 0x082E: return "N";
                case 0x082F: return "O";
                case 0x0830: return "P";
                case 0x0831: return "Q";
                case 0x0832: return "R";
                case 0x0833: return "S";
                case 0x0834: return "T";
                case 0x0835: return "U";
                case 0x0836: return "V";
                case 0x0837: return "W";
                case 0x0838: return "X";
                case 0x0839: return "Y";
                case 0x083A: return "Z";

                //Lowercase
                case 0x0841: return "a";
                case 0x0842: return "b";
                case 0x0843: return "c";
                case 0x0844: return "d";
                case 0x0845: return "e";
                case 0x0846: return "f";
                case 0x0847: return "g";
                case 0x0848: return "h";
                case 0x0849: return "i";
                case 0x084A: return "j";
                case 0x084B: return "k";
                case 0x084C: return "l";
                case 0x084D: return "m";
                case 0x084E: return "n";
                case 0x084F: return "o";
                case 0x0850: return "p";
                case 0x0851: return "q";
                case 0x0852: return "r";
                case 0x0853: return "s";
                case 0x0854: return "t";
                case 0x0855: return "u";
                case 0x0856: return "v";
                case 0x0857: return "w";
                case 0x0858: return "x";
                case 0x0859: return "y";
                case 0x085a: return "z";

                //Special Case
                case 0x2300: return "%var%";
                case 0x8000: return "\n";
            }

            return "�";
        }
    }
}
