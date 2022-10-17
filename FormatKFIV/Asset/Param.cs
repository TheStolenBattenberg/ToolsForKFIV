using System;
using System.Collections.Generic;
using System.Text;

namespace FormatKFIV.Asset
{
    public class Param
    {
        //Types
        public enum ParamColumnFormat
        {
            DTString,
            DTFloat,
            DTDouble,
            DTUInt64,
            DTUInt32,
            DTUInt16,
            DTUInt8,
            DTInt64,
            DTInt32,
            DTInt16,
            DTInt8,
        }

        [Flags]
        public enum ParamColumnFlag
        {
            None     = 0x0000,  //No Flags
            NoExport = 0x0001,  //Columns with this flag shouldn't be exported to files
            NoShow   = 0x0002,  //Columns with this flag shouldn't be shown to the user
        }

        //Structures
        public struct ParamColumn
        {
            public string Name;
            public ParamColumnFormat DataType;
            public ParamColumnFlag flags;
        }

        public struct ParamRow
        {
            public ParamRow(params object[] vargs)
            {
                values = new object[vargs.Length];
                for(int i = 0; i < vargs.Length; ++i)
                {
                    values[i] = vargs[i];
                }
            }

            public object[] values;
        }
        public struct ParamLayout
        {
            public ParamColumn[] Columns;

            public uint ColumnCount
            {
                get { return (uint)Columns.Length; }
            }
        }
        public struct ParamPage
        {
            public string name;
            public List<ParamRow> rows;
            public ParamLayout layout;

            public void AddRow(ParamRow row)
            {
                rows.Add(row);
            }
            public ParamRow? GetRow(int index)
            {
                if (index < 0 || index >= rows.Count)
                    return null;

                return rows[index];
            }
        }

        //Data
        public List<ParamPage> Pages;

        //Accessors
        public uint PageCount
        {
            get { return (uint) Pages.Count; }
        }
    }
}