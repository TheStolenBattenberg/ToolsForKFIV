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
				// Fullwidth numbers
                case 0x0000: return "０";
                case 0x0001: return "１";
                case 0x0002: return "２";
                case 0x0003: return "３";
                case 0x0004: return "４";
                case 0x0005: return "５";
                case 0x0006: return "６";
                case 0x0007: return "７";
                case 0x0008: return "８";
                case 0x0009: return "９";
                
                // Fullwidth grammar
                case 0x000A: return "（";
                case 0x000B: return "）";
                case 0x000C: return "，";
                case 0x000D: return "．";
                case 0x000E: return "：";
                case 0x000F: return "＜";
                case 0x0010: return "＞";
                case 0x0011: return "＊";
                case 0x0012: return "－";
                case 0x0013: return "＋";
                
                // Fullwidth alphabet
                case 0x0014: return "ａ";
                case 0x0015: return "ｂ";
                case 0x0016: return "ｃ";
                case 0x0017: return "ｄ";
                case 0x0018: return "ｅ";
                case 0x0019: return "ｆ";
                case 0x001A: return "ｇ";
                case 0x001B: return "ｈ";
                case 0x001C: return "ｉ";
                case 0x001D: return "ｊ";
                case 0x001E: return "ｋ";
                case 0x001F: return "ｌ";
                case 0x0020: return "ｍ";
                case 0x0021: return "ｎ";
                case 0x0022: return "ｏ";
                case 0x0023: return "ｐ";
                case 0x0024: return "ｑ";
                case 0x0025: return "ｒ";
                case 0x0026: return "ｓ";
                case 0x0027: return "ｔ";
                case 0x0028: return "ｕ";
                case 0x0029: return "ｖ";
                case 0x002A: return "ｗ";
                case 0x002B: return "ｘ";
                case 0x002C: return "ｙ";
                case 0x002D: return "ｚ";
                case 0x002E: return "Ａ";
                case 0x002F: return "Ｂ";
                case 0x0030: return "Ｃ";
                case 0x0031: return "Ｄ";
                case 0x0032: return "Ｅ";
                case 0x0033: return "Ｆ";
                case 0x0034: return "Ｇ";
                case 0x0035: return "Ｈ";
                case 0x0036: return "Ｉ";
                case 0x0037: return "Ｊ";
                case 0x0038: return "Ｋ";
                case 0x0039: return "Ｌ";
                case 0x003A: return "Ｍ";
                case 0x003B: return "Ｎ";
                case 0x003C: return "Ｏ";
                case 0x003D: return "Ｐ";
                case 0x003E: return "Ｑ";
                case 0x003F: return "Ｒ";
                case 0x0040: return "Ｓ";
                case 0x0041: return "Ｔ";
                case 0x0042: return "Ｕ";
                case 0x0043: return "Ｖ";
                case 0x0044: return "Ｗ";
                case 0x0045: return "Ｘ";
                case 0x0046: return "Ｙ";
                case 0x0047: return "Ｚ";
                
                // Hiragana
                case 0x004B: return "あ";
                case 0x004C: return "い";
                case 0x004D: return "う";
                case 0x004E: return "え";
                case 0x004F: return "お";
                case 0x0050: return "か";
                case 0x0051: return "き";
                case 0x0052: return "く";
                case 0x0053: return "け";
                case 0x0054: return "こ";
                case 0x0055: return "さ";
                case 0x0056: return "し";
                case 0x0057: return "す";
                case 0x0058: return "せ";
                case 0x0059: return "そ";
                case 0x005A: return "た";
                case 0x005B: return "ち";
                case 0x005C: return "つ";
                case 0x005D: return "て";
                case 0x005E: return "と";
                case 0x005F: return "な";
                case 0x0060: return "に";
                case 0x0061: return "ぬ";
                case 0x0062: return "ね";
                case 0x0063: return "の";
                case 0x0064: return "は";
                case 0x0065: return "ひ";
                case 0x0066: return "ふ";
                case 0x0067: return "へ";
                case 0x0068: return "ほ";
                case 0x0069: return "ま";
                case 0x006A: return "み";
                case 0x006B: return "む";
                case 0x006C: return "め";
                case 0x006D: return "も";
                case 0x006E: return "や";
                case 0x006F: return "ゆ";
                case 0x0070: return "よ";
                case 0x0071: return "ら";
                case 0x0072: return "り";
                case 0x0073: return "る";
                case 0x0074: return "れ";
                case 0x0075: return "ろ";
                case 0x0076: return "わ";
                case 0x0077: return "を";
                case 0x0078: return "ん";
                case 0x0079: return "が";
                case 0x007A: return "ぎ";
                case 0x007B: return "ぐ";
                case 0x007C: return "げ";
                case 0x007D: return "ご";
                case 0x007E: return "ざ";
                case 0x007F: return "じ";
                case 0x0080: return "ず";
                case 0x0081: return "ぜ";
                case 0x0082: return "ぞ";
                case 0x0083: return "だ";
                case 0x0084: return "ぢ";
                case 0x0085: return "づ";
                case 0x0086: return "で";
                case 0x0087: return "ど";
                case 0x0088: return "ば";
                case 0x0089: return "び";
                case 0x008A: return "ぶ";
                case 0x008B: return "べ";
                case 0x008C: return "ぼ";
                case 0x008D: return "ぱ";
                case 0x008E: return "ぴ";
                case 0x008F: return "ぷ";
                case 0x0090: return "ぺ";
                case 0x0091: return "ぽ";
                case 0x0092: return "ぁ";
                case 0x0093: return "ぃ";
                case 0x0094: return "ぅ";
                case 0x0095: return "ぇ";
                case 0x0096: return "ぉ";
                case 0x0097: return "ゃ";
                case 0x0098: return "ゅ";
                case 0x0099: return "ょ";
                case 0x009A: return "っ";
                
                // Katakana
                case 0x009B: return "ア";
                case 0x009C: return "イ";
                case 0x009D: return "ウ";
                case 0x009E: return "エ";
                case 0x009F: return "オ";
                case 0x00A0: return "カ";
                case 0x00A1: return "キ";
                case 0x00A2: return "ク";
                case 0x00A3: return "ケ";
                case 0x00A4: return "コ";
                case 0x00A5: return "サ";
                case 0x00A6: return "シ";
                case 0x00A7: return "ス";
                case 0x00A8: return "セ";
                case 0x00A9: return "ソ";
                case 0x00AA: return "タ";
                case 0x00AB: return "チ";
                case 0x00AC: return "ツ";
                case 0x00AD: return "テ";
                case 0x00AE: return "ト";
                case 0x00AF: return "ナ";
                case 0x00B0: return "ニ";
                case 0x00B1: return "ヌ";
                case 0x00B2: return "ネ";
                case 0x00B3: return "ノ";
                case 0x00B4: return "ハ";
                case 0x00B5: return "ヒ";
                case 0x00B6: return "フ";
                case 0x00B7: return "ヘ";
                case 0x00B8: return "ホ";
                case 0x00B9: return "マ";
                case 0x00BA: return "ミ";
                case 0x00BB: return "ム";
                case 0x00BC: return "メ";
                case 0x00BD: return "モ";
                case 0x00BE: return "ヤ";
                case 0x00BF: return "ユ";
                case 0x00C0: return "ヨ";
                case 0x00C1: return "ラ";
                case 0x00C2: return "リ";
                case 0x00C3: return "ル";
                case 0x00C4: return "レ";
                case 0x00C5: return "ロ";
                case 0x00C6: return "ワ";
                case 0x00C7: return "ヲ";
                case 0x00C8: return "ン";
                case 0x00C9: return "ヴ";
                case 0x00CA: return "ガ";
                case 0x00CB: return "ギ";
                case 0x00CC: return "グ";
                case 0x00CD: return "ゲ";
                case 0x00CE: return "ゴ";
                case 0x00CF: return "ザ";
                case 0x00D0: return "ジ";
                case 0x00D1: return "ズ";
                case 0x00D2: return "ゼ";
                case 0x00D3: return "ゾ";
                case 0x00D4: return "ダ";
                case 0x00D5: return "ヂ";
                case 0x00D6: return "ヅ";
                case 0x00D7: return "デ";
                case 0x00D8: return "ド";
                case 0x00D9: return "バ";
                case 0x00DA: return "ビ";
                case 0x00DB: return "ブ";
                case 0x00DC: return "ベ";
                case 0x00DD: return "ボ";
                case 0x00DE: return "パ";
                case 0x00DF: return "ピ";
                case 0x00E0: return "プ";
                case 0x00E1: return "ペ";
                case 0x00E2: return "ポ";
                case 0x00E3: return "ァ";
                case 0x00E4: return "ィ";
                case 0x00E5: return "ゥ";
                case 0x00E6: return "ェ";
                case 0x00E7: return "ォ";
                case 0x00E8: return "ャ";
                case 0x00E9: return "ュ";
                case 0x00EA: return "ョ";
                case 0x00EB: return "ッ";
                
                // Japanese punctuation marks
                case 0x00EC: return "！";
                case 0x00ED: return "？";
                case 0x00EE: return "、";
                case 0x00EF: return "・";
                case 0x00F0: return "。";
                case 0x00F1: return "「";
                case 0x00F2: return "」";
                case 0x00F3: return "〳";　// I'm not sure whether this really is 〳 or just a slash
                case 0x00F4: return "ー";
                
                // Kanji
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
