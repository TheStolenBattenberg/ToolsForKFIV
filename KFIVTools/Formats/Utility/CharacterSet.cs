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
                case 0x0048: return "　";
                case 0x0049: return "　";
                case 0x004A: return "　";

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
                case 0x00F5: return "　";
                case 0x00F6: return "　";
                case 0x00F7: return "　";
                case 0x00F8: return "　";
                case 0x00F9: return "　";

                // Kanji (Block 1)
                //攻―表使用撃魔示動移法更話重景行背音果面量変画会回
                case 0x00FA: return "攻";
                case 0x00FB: return "―";
                case 0x00FC: return "表";
                case 0x00FD: return "使";
                case 0x00FE: return "用";
                case 0x00FF: return "撃";
                case 0x0100: return "魔";
                case 0x0101: return "示";
                case 0x0102: return "動";
                case 0x0103: return "移";
                case 0x0104: return "法";
                case 0x0105: return "更";
                case 0x0106: return "話";
                case 0x0107: return "重";
                case 0x0108: return "景";
                case 0x0109: return "行";
                case 0x010A: return "背";
                case 0x010B: return "音";
                case 0x010C: return "果";
                case 0x010D: return "面";
                case 0x010E: return "量";
                case 0x010F: return "変";
                case 0x0110: return "画";
                case 0x0111: return "会";
                case 0x0112: return "回";

                //効右終選前転点斬刺防進値熱経計度土想操盾水合視武助
                case 0x0113: return "効";
                case 0x0114: return "右";
                case 0x0115: return "終";
                case 0x0116: return "選";
                case 0x0117: return "前";
                case 0x0118: return "転";
                case 0x0119: return "点";
                case 0x011A: return "斬";
                case 0x011B: return "刺";
                case 0x011C: return "防";
                case 0x011D: return "進";
                case 0x011E: return "値";
                case 0x011F: return "熱";
                case 0x0120: return "経";
                case 0x0121: return "計";
                case 0x0122: return "度";
                case 0x0123: return "土";
                case 0x0124: return "想";
                case 0x0125: return "操";
                case 0x0126: return "盾";
                case 0x0127: return "水";
                case 0x0128: return "合";
                case 0x0129: return "視";
                case 0x012A: return "武";
                case 0x012B: return "助";

                //装風理未殴整験左長聖了下声平体火態替細切詳設退歩補
                case 0x012C: return "装";
                case 0x012D: return "風";
                case 0x012E: return "理";
                case 0x012F: return "未";
                case 0x0130: return "殴";
                case 0x0131: return "整";
                case 0x0132: return "験";
                case 0x0133: return "左";
                case 0x0134: return "長";
                case 0x0135: return "聖";
                case 0x0136: return "了";
                case 0x0137: return "下";
                case 0x0138: return "声";
                case 0x0139: return "平";
                case 0x013A: return "体";
                case 0x013B: return "火";
                case 0x013C: return "態";
                case 0x013D: return "替";
                case 0x013E: return "細";
                case 0x013F: return "切";
                case 0x0140: return "詳";
                case 0x0141: return "設";
                case 0x0142: return "退";
                case 0x0143: return "歩";
                case 0x0144: return "補";

                //後上御定作器択状備練力逃生先帰残人出行汲私臭何家戻
                case 0x0145: return "後";
                case 0x0146: return "上";
                case 0x0147: return "御";
                case 0x0148: return "定";
                case 0x0149: return "作";
                case 0x014A: return "器";
                case 0x014B: return "択";
                case 0x014C: return "状";
                case 0x014D: return "備";
                case 0x014E: return "練";
                case 0x014F: return "力";
                case 0x0150: return "逃";
                case 0x0151: return "生";
                case 0x0152: return "先";
                case 0x0153: return "帰";
                case 0x0154: return "残";
                case 0x0155: return "人";
                case 0x0156: return "出";
                case 0x0157: return "行";
                case 0x0158: return "汲";
                case 0x0159: return "私";
                case 0x015A: return "臭";
                case 0x015B: return "何";
                case 0x015C: return "家";
                case 0x015D: return "戻";

                //手放入墓士死所戦気開地掛像鍵竜毒石者泉兵物見薄真奴
                case 0x015E: return "手";
                case 0x015F: return "放";
                case 0x0160: return "入";
                case 0x0161: return "墓";
                case 0x0162: return "士";
                case 0x0163: return "死";
                case 0x0164: return "所";
                case 0x0165: return "戦";
                case 0x0166: return "気";
                case 0x0167: return "開";
                case 0x0168: return "地";
                case 0x0169: return "掛";
                case 0x016A: return "像";
                case 0x016B: return "鍵";
                case 0x016C: return "竜";
                case 0x016D: return "毒";
                case 0x016E: return "石";
                case 0x016F: return "者";
                case 0x0170: return "泉";
                case 0x0171: return "兵";
                case 0x0172: return "物";
                case 0x0173: return "見";
                case 0x0174: return "薄";
                case 0x0175: return "真";
                case 0x0176: return "奴";

                //仲来遠征分少呼闇間輪目扉取場護具光怪殺職誰嘘篭鎧腕
                case 0x0177: return "仲";
                case 0x0178: return "来";
                case 0x0179: return "遠";
                case 0x017A: return "征";
                case 0x017B: return "分";
                case 0x017C: return "少";
                case 0x017D: return "呼";
                case 0x017E: return "闇";
                case 0x017F: return "間";
                case 0x0180: return "輪";
                case 0x0181: return "目";
                case 0x0182: return "扉";
                case 0x0183: return "取";
                case 0x0184: return "場";
                case 0x0185: return "護";
                case 0x0186: return "具";
                case 0x0187: return "光";
                case 0x0188: return "怪";
                case 0x0189: return "殺";
                case 0x018A: return "職";
                case 0x018B: return "誰";
                case 0x018C: return "嘘";
                case 0x018D: return "篭";
                case 0x018E: return "鎧";
                case 0x018F: return "腕";

                //思指倒符兜末書錫奥自足恐眠道多命希駄番憑隊我門疲守
                case 0x0190: return "思";
                case 0x0191: return "指";
                case 0x0192: return "倒";
                case 0x0193: return "符";
                case 0x0194: return "兜";
                case 0x0195: return "末";
                case 0x0196: return "書";
                case 0x0197: return "錫";
                case 0x0198: return "奥";
                case 0x0199: return "自";
                case 0x019A: return "足";
                case 0x019B: return "恐";
                case 0x019C: return "眠";
                case 0x019D: return "道";
                case 0x019E: return "多";
                case 0x019F: return "命";
                case 0x01A0: return "希";
                case 0x01A1: return "駄";
                case 0x01A2: return "番";
                case 0x01A3: return "憑";
                case 0x01A4: return "隊";
                case 0x01A5: return "我";
                case 0x01A6: return "門";
                case 0x01A7: return "疲";
                case 0x01A8: return "守";

                //一向無図実草大矢薬樹祠継老妖針廃倍本刀復王浮賢導枚
                case 0x01A9: return "一";
                case 0x01AA: return "向";
                case 0x01AB: return "無";
                case 0x01AC: return "図";
                case 0x01AD: return "実";
                case 0x01AE: return "草";
                case 0x01AF: return "大";
                case 0x01B0: return "矢";
                case 0x01B1: return "薬";
                case 0x01B2: return "樹";
                case 0x01B3: return "祠";
                case 0x01B4: return "継";
                case 0x01B5: return "老";
                case 0x01B6: return "妖";
                case 0x01B7: return "針";
                case 0x01B8: return "廃";
                case 0x01B9: return "倍";
                case 0x01BA: return "本";
                case 0x01BB: return "刀";
                case 0x01BC: return "復";
                case 0x01BD: return "王";
                case 0x01BE: return "浮";
                case 0x01BF: return "賢";
                case 0x01C0: return "導";
                case 0x01C1: return "枚";

                //雫精村消主種愚空日病掘当方谷親額父東落聞古金年心集
                case 0x01C2: return "雫";
                case 0x01C3: return "精";
                case 0x01C4: return "村";
                case 0x01C5: return "消";
                case 0x01C6: return "主";
                case 0x01C7: return "種";
                case 0x01C8: return "愚";
                case 0x01C9: return "空";
                case 0x01CA: return "日";
                case 0x01CB: return "病";
                case 0x01CC: return "掘";
                case 0x01CD: return "当";
                case 0x01CE: return "方";
                case 0x01CF: return "谷";
                case 0x01D0: return "親";
                case 0x01D1: return "額";
                case 0x01D2: return "父";
                case 0x01D3: return "東";
                case 0x01D4: return "落";
                case 0x01D5: return "聞";
                case 0x01D6: return "古";
                case 0x01D7: return "金";
                case 0x01D8: return "年";
                case 0x01D9: return "心";
                case 0x01DA: return "集";

                //々都休持彫俺明以医速修訪起外念寝山探振住噂静神伝殿
                case 0x01DB: return "々";
                case 0x01DC: return "都";
                case 0x01DD: return "休";
                case 0x01DE: return "持";
                case 0x01DF: return "彫";
                case 0x01E0: return "俺";
                case 0x01E1: return "明";
                case 0x01E2: return "以";
                case 0x01E3: return "医";
                case 0x01E4: return "速";
                case 0x01E5: return "修";
                case 0x01E6: return "訪";
                case 0x01E7: return "起";
                case 0x01E8: return "外";
                case 0x01E9: return "念";
                case 0x01EA: return "寝";
                case 0x01EB: return "山";
                case 0x01EC: return "探";
                case 0x01ED: return "振";
                case 0x01EE: return "住";
                case 0x01EF: return "噂";
                case 0x01F0: return "静";
                case 0x01F1: return "神";
                case 0x01F2: return "伝";
                case 0x01F3: return "殿";

                //議省僅編遮様確覚却荒途不購鉱×早売中緒数剣封徐衝今
                case 0x01F4: return "議";
                case 0x01F5: return "省";
                case 0x01F6: return "僅";
                case 0x01F7: return "編";
                case 0x01F8: return "遮";
                case 0x01F9: return "様";
                case 0x01FA: return "確";
                case 0x01FB: return "覚";
                case 0x01FC: return "却";
                case 0x01FD: return "荒";
                case 0x01FE: return "途";
                case 0x01FF: return "不";
                case 0x0200: return "購";
                case 0x0201: return "鉱";
                case 0x0202: return "×";
                case 0x0203: return "早";
                case 0x0204: return "売";
                case 0x0205: return "中";
                case 0x0206: return "緒";
                case 0x0207: return "数";
                case 0x0208: return "剣";
                case 0x0209: return "封";
                case 0x020A: return "徐";
                case 0x020B: return "衝";
                case 0x020C: return "今";

                //世離小強屋続調尺元感頂在化言個違閉馴良昔対待跡盤秘
                case 0x020D: return "世";
                case 0x020E: return "離";
                case 0x020F: return "小";
                case 0x0210: return "強";
                case 0x0211: return "屋";
                case 0x0212: return "続";
                case 0x0213: return "調";
                case 0x0214: return "尺";
                case 0x0215: return "元";
                case 0x0216: return "感";
                case 0x0217: return "頂";
                case 0x0218: return "在";
                case 0x0219: return "化";
                case 0x021A: return "言";
                case 0x021B: return "個";
                case 0x021C: return "違";
                case 0x021D: return "閉";
                case 0x021E: return "馴";
                case 0x021F: return "良";
                case 0x0220: return "昔";
                case 0x0221: return "対";
                case 0x0222: return "待";
                case 0x0223: return "跡";
                case 0x0224: return "盤";
                case 0x0225: return "秘";

                //遺難印受口商→近店品立妻機有崖新渡降止欲買決注事劣
                case 0x0226: return "遺";
                case 0x0227: return "難";
                case 0x0228: return "印";
                case 0x0229: return "受";
                case 0x022A: return "口";
                case 0x022B: return "商";
                case 0x022C: return "→";
                case 0x022D: return "近";
                case 0x022E: return "店";
                case 0x022F: return "品";
                case 0x0230: return "立";
                case 0x0231: return "妻";
                case 0x0232: return "機";
                case 0x0233: return "有";
                case 0x0234: return "崖";
                case 0x0235: return "新";
                case 0x0236: return "渡";
                case 0x0237: return "降";
                case 0x0238: return "止";
                case 0x0239: return "欲";
                case 0x023A: return "買";
                case 0x023B: return "決";
                case 0x023C: return "注";
                case 0x023D: return "事";
                case 0x023E: return "劣";

                //←発険橋霧昇加最追採意禁染代乗危溢役壊民師女氷棒炎
                case 0x023F: return "←";
                case 0x0240: return "発";
                case 0x0241: return "険";
                case 0x0242: return "橋";
                case 0x0243: return "霧";
                case 0x0244: return "昇";
                case 0x0245: return "加";
                case 0x0246: return "最";
                case 0x0247: return "追";
                case 0x0248: return "採";
                case 0x0249: return "意";
                case 0x024A: return "禁";
                case 0x024B: return "染";
                case 0x024C: return "代";
                case 0x024D: return "乗";
                case 0x024E: return "危";
                case 0x024F: return "溢";
                case 0x0250: return "役";
                case 0x0251: return "壊";
                case 0x0252: return "民";
                case 0x0253: return "師";
                case 0x0254: return "女";
                case 0x0255: return "氷";
                case 0x0256: return "棒";
                case 0x0257: return "炎";

                //国棍敵錆牢短騒素労葉灯骨時冠根幻憐候耐鱗異折爪常費
                case 0x0258: return "国";
                case 0x0259: return "棍";
                case 0x025A: return "敵";
                case 0x025B: return "錆";
                case 0x025C: return "牢";
                case 0x025D: return "短";
                case 0x025E: return "騒";
                case 0x025F: return "素";
                case 0x0260: return "労";
                case 0x0261: return "葉";
                case 0x0262: return "灯";
                case 0x0263: return "骨";
                case 0x0264: return "時";
                case 0x0265: return "冠";
                case 0x0266: return "根";
                case 0x0267: return "幻";
                case 0x0268: return "憐";
                case 0x0269: return "候";
                case 0x026A: return "耐";
                case 0x026B: return "鱗";
                case 0x026C: return "異";
                case 0x026D: return "折";
                case 0x026E: return "爪";
                case 0x026F: return "常";
                case 0x0270: return "費";

                // Kanji (Block 2)
                

                //halfwidth grammar #1
                case 0x0800: return " ";
                case 0x0801: return "!";
                case 0x0802: return "\"";
                case 0x0803: return "#";
                case 0x0804: return "$";
                case 0x0805: return "%";
                case 0x0806: return "&";
                case 0x0807: return "'";
                case 0x0808: return "(";
                case 0x0809: return ")";
                case 0x080A: return "*";
                case 0x080B: return "+";
                case 0X080C: return ",";
                case 0x080D: return "-";
                case 0x080E: return ".";
                case 0x080F: return "/";

                //halfwidth numbers
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

                //halfwidth grammer #2
                case 0x081A: return ":";
                case 0x081B: return ";";
                case 0x081C: return "<";
                case 0x081D: return "=";
                case 0x081E: return ">";
                case 0x081F: return "?";
                case 0x0820: return "`";

                //halfwidth uppercase
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

                //halfwidth grammar #3
                case 0x083B: return "[";
                case 0x083C: return "\\";
                case 0x083D: return "]";
                case 0x083E: return "^";
                case 0x083F: return "_";
                case 0x0840: return "`";

                //halfwidth lowercase
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

                //Special
                case 0x2300: return "%var_gold%";
                case 0x4000: return "%var_id0%";
                case 0x4100: return "%var_id1%";
                case 0x4200: return "%var_id2%";
                case 0x4300: return "%var_id3%";
                case 0x4400: return "%var_id4%";
                case 0x4500: return "%var_id5%";
                case 0x4600: return "%var_id6%";
                case 0x4700: return "%var_id7%";
                case 0x4800: return "%var_id8%";
                case 0x4900: return "%var_id9%";
                case 0x8000: return "\n";
            }

            return "�{"+ data.ToString("X4")+ "}";
        }
    }
}
