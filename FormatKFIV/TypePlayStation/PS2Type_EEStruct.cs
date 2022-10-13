using System.Runtime.InteropServices;

/**
 * To the curious:
 *   The pragma for disabling a warning here is because the code analysis detects that
 *   data0_8 etc values are unused, which is bullshit because I'm doing some sketchy
 *   fake union stuff here in order to read these types without adding a load of 
 *   crap methods inside the InputStream class.
 *   
 *   Do not re-enable that warning if you value your build output.
 *   Do not remove those variables, as they are VERY crucial.
**/

namespace FormatKFIV.TypePlayStation
{
    //16 BYTE REGISTER TYPES
    #pragma warning disable CS0649
    public struct sceGifTag
    {
        public ulong data0_8;
        public ulong data8_8;

        //DATA0_8
        /// <summary>Number of QWORDs in packet/QWORDS to copy?..</summary>
        public uint NLOOP
        {
            get { return (uint)((data0_8 >> 00) & 0x7FFF); }
        }
        /// <summary>End of Packet Marker</summary>
        public uint EOP
        {
            get { return (uint)((data0_8 >> 15) & 0x1); }
        }
        /// <summary>GIFTag ID?..</summary>
        public uint id
        {
            get { return (uint)((data0_8 >> 32) & 0x3FFF); }
        }
        /// <summary>???</summary>
        public uint PRE
        {
            get { return (uint)((data0_8 >> 46) & 0x1); }
        }
        /// <summary>GIFTag Type</summary>
        public uint PRIM
        {
            get { return (uint)((data0_8 >> 47) & 0x7FF); }
        }
        /// <summary>???</summary>
        public uint FLG
        {
            get { return (uint)((data0_8 >> 58) & 0x3); }
        }
        /// <summary>Number of registers. 0 defaults to 16.</summary>
        public uint NREG
        {
            get { return (uint)((data0_8 >> 60) & 0xF); }
        }

        //DATA8_8
        public uint REG0
        {
            get { return (uint)((data0_8 >> 00) & 0xF); }
        }
        public uint REG1
        {
            get { return (uint)((data0_8 >> 04) & 0xF); }
        }
        public uint REG2
        {
            get { return (uint)((data0_8 >> 08) & 0xF); }
        }
        public uint REG3
        {
            get { return (uint)((data0_8 >> 12) & 0xF); }
        }
        public uint REG4
        {
            get { return (uint)((data0_8 >> 16) & 0xF); }
        }
        public uint REG5
        {
            get { return (uint)((data0_8 >> 20) & 0xF); }
        }
        public uint REG6
        {
            get { return (uint)((data0_8 >> 24) & 0xF); }
        }
        public uint REG7
        {
            get { return (uint)((data0_8 >> 28) & 0xF); }
        }
        public uint REG8
        {
            get { return (uint)((data0_8 >> 32) & 0xF); }
        }
        public uint REG9
        {
            get { return (uint)((data0_8 >> 36) & 0xF); }
        }
        public uint REGA
        {
            get { return (uint)((data0_8 >> 40) & 0xF); }
        }
        public uint REGB
        {
            get { return (uint)((data0_8 >> 44) & 0xF); }
        }
        public uint REGC
        {
            get { return (uint)((data0_8 >> 48) & 0xF); }
        }
        public uint REGD
        {
            get { return (uint)((data0_8 >> 52) & 0xF); }
        }
        public uint REGE
        {
            get { return (uint)((data0_8 >> 56) & 0xF); }
        }
        public uint REGF
        {
            get { return (uint)((data0_8 >> 60) & 0xF); }
        }
    }
    public struct sceDmaTag
    {
        public ulong data0_8;
        public ulong data8_8;

        public uint QWC { get { return (uint)(data0_8 >> 0) & 0xFFFF; } }

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct sceRegister16Byte
    {
        [FieldOffset(0)]
        public ulong data0_8;

        [FieldOffset(8)]
        public ulong data8_8;

        //Bitfields
        [FieldOffset(0)]
        public sceGifTag _sceGifTag;
        [FieldOffset(0)]
        public sceDmaTag _sceDmaTag;
    }
    #pragma warning restore CS0649


    //8 BYTE REGISTER TYPES
    #pragma warning disable CS0649
    public struct sceGsTex0
    {
        private ulong data0_8;  //IGNORE THESE WARNINGS.

        /// <summary>Packed Texture Buffer Pointer. Multiply by 0x100 to restore actual pointer.</summary>
        public uint TBP
        { 
            get { return (uint)((data0_8 >> 00) & 0x3FFF); }
        }
        /// <summary>Texture Buffer Width. ???</summary>
        public uint TBW
        {
            get { return (uint)((data0_8 >> 14) & 0x3F); }
        }
        /// <summary>Texture Pixel Storage Mode.</summary>
        public uint PSM
        {
            get { return (uint)((data0_8 >> 20) & 0x3F); }
        }
        /// <summary>log2 of original Texture Width</summary>
        public uint TW
        {
            get { return (uint)((data0_8 >> 26) & 0xF); }
        }
        /// <summary>log2 of original Texture Height</summary>
        public uint TH
        {
            get { return (uint)((data0_8 >> 30) & 0xF); }
        }
        /// <summary>???</summary>
        public uint TCC
        {
            get { return (uint)((data0_8 >> 34) & 0x1); }
        }
        /// <summary>Texture Function</summary>
        public uint TFX
        {
            get { return (uint)((data0_8 >> 35) & 0x3); }
        }
        /// <summary>Packed Clut Buffer Pointer. Multiply by 0x100 to restore actual pointer.</summary>
        public uint CBP
        {
            get { return (uint)((data0_8 >> 37) & 0x3FFF); }
        }
        /// <summary>Clut Pixel Storage Mode</summary>
        public uint CPSM
        {
            get { return (uint)((data0_8 >> 51) & 0xF); }
        }
        /// <summary>Clut Storage Mode (Swizzled/Unswizzled)</summary>
        public uint CSM
        {
            get { return (uint)((data0_8 >> 55) & 0x1); }
        }
        /// <summary>???</summary>
        public uint CSA
        {
            get { return (uint)((data0_8 >> 56) & 0x1F); }
        }
        /// <summary>???</summary>
        public uint CLD
        {
            get { return (uint)((data0_8 >> 61) & 0x7); }
        }
    }
    public struct sceGsTex1
    {
        private ulong data0_8;

        /// <summary>???</summary>
        public uint LCM
        {
            get { return (uint)((data0_8 >> 00) & 0x1); }
        }
        /// <summary>???</summary>
        public uint MXL
        {
            get { return (uint)((data0_8 >> 02) & 0x7); }
        }
        /// <summary>Magnification Filter</summary>
        public uint MMAG
        {
            get { return (uint)((data0_8 >> 05) & 0x1); }
        }
        /// <summary>Minification Filter</summary>
        public uint MMIN
        {
            get { return (uint)((data0_8 >> 06) & 0x7); }
        }
        /// <summary>???</summary>
        public uint MTBA
        {
            get { return (uint)((data0_8 >> 09) & 0x1); }
        }
        /// <summary>???</summary>
        public uint L
        {
            get { return (uint)((data0_8 >> 19) & 0x3); }
        }
        /// <summary>???</summary>
        public uint K
        {
            get { return (uint)((data0_8 >> 32) & 0xFFF); }
        }
    }
    public struct sceGsBitbltbuf
    {
        private ulong data0_8;

        public uint SBP
        {
            get { return (uint)((data0_8 >> 00) & 0x3FFF); }
        }
        public uint SBW
        {
            get { return (uint)((data0_8 >> 16) & 0x3F); }
        }
        public uint SPSM
        {
            get { return (uint)((data0_8 >> 24) & 0x3F); }
        }
        public uint DBP
        {
            get { return (uint)((data0_8 >> 32) & 0x3FFF); }
        }
        public uint DBW
        {
            get { return (uint)((data0_8 >> 48) & 0x3F); }
        }
        public uint DPSM
        {
            get { return (uint)((data0_8 >> 56) & 0x3F); }
        }
    }
    public struct sceGsTexflush
    {
        private ulong data0_8;

        public ulong TexFlush
        {
            get
            {
                return data0_8;
            }
        }
    }
    public struct sceGsTrxpos
    {
        private ulong data0_8;

        public uint SSAX
        {
            get { return (uint)((data0_8 >> 00) & 0x7FF); }
        }
        public uint SSAY
        {
            get { return (uint)((data0_8 >> 16) & 0x7FF); }
        }
        public uint DSAX
        {
            get { return (uint)((data0_8 >> 32) & 0x7FF); }
        }
        public uint DSAY
        {
            get { return (uint)((data0_8 >> 48) & 0x7FF); }
        }
        public uint DIR
        {
            get { return (uint)((data0_8 >> 59) & 0x3); }
        }
    }
    public struct sceGsTrxreg
    {
        private ulong data0_8;

        /// <summary>Can be visualized as texture width.</summary>
        public uint RRW
        {
            get { return (uint)((data0_8 >> 00) & 0xFFF); }
        }
        /// <summary>Can be visualized as texture height.</summary>
        public uint RRH
        {
            get { return (uint)((data0_8 >> 32) & 0xFFF); }
        }
    }
    public struct sceGsTrxdir
    {
        private ulong data0_8;

        public uint XDR
        {
            get { return (uint)((data0_8 >> 00) & 0x3); }
        }
    }
    public struct sceGsAlpha
    {
        private ulong data0_8;

        public uint A
        {
            get { return (uint)((data0_8 >> 00) & 0x3); }
        }
        public uint B
        {
            get { return (uint)((data0_8 >> 02) & 0x3); }
        }
        public uint C
        {
            get { return (uint)((data0_8 >> 04) & 0x3); }
        }
        public uint D
        {
            get { return (uint)((data0_8 >> 06) & 0x3); }
        }
        public uint FIX
        {
            get { return (uint)((data0_8 >> 32) & 0xFF); }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct sceRegister8Byte
    {
        [FieldOffset(0)]
        public ulong data0_8;

        [FieldOffset(0)]
        public sceGsTex0 _sceGsTex0;
        [FieldOffset(0)]
        public sceGsTex1 _sceGsTex1;
        [FieldOffset(0)]
        public sceGsAlpha _sceGsAlpha;

        [FieldOffset(0)]
        public sceGsTexflush _sceGsTexflush;
        [FieldOffset(0)]
        public sceGsBitbltbuf _sceGsBitbltbuf;
        [FieldOffset(0)]
        public sceGsTrxpos _sceGsTrxpos;
        [FieldOffset(0)]
        public sceGsTrxreg _sceGsTrxreg;
        [FieldOffset(0)]
        public sceGsTrxdir _sceGsTrxdir;
    }
    #pragma warning restore CS0649
}