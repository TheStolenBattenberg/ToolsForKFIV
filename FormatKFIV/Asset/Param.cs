using System;
using System.Collections.Generic;
using System.Text;

namespace FormatKFIV.Asset
{
    public class Param
    {
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

        public struct ParamColumn
        {
            public string Name;
            public ParamColumnFormat DataType;
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
        }
        public struct ParamPage
        {
            public string pageName;
            public List<ParamRow> pageRows;

            public void AddRow(ParamRow row)
            {
                pageRows.Add(row);
            }
            public ParamRow? GetRow(int index)
            {
                if (index > pageRows.Count || index < 0)
                    return null;

                return pageRows[index];
            }
        }

        //Properties
        public ParamLayout Layout
        {
            get { return _layout; }
        }
        public int ColumnCount
        {
            get { return _layout.Columns.Length; }
        }
        public int PageCount
        {
            get { return _data.Count; }
        }

        //Members
        private ParamLayout _layout;
        private List<ParamPage> _data = new List<ParamPage>();

        public void AddPage(ParamPage page)
        {
            _data.Add(page);
        }
        public void SetLayout(ParamLayout layout)
        {
            _layout = layout;

        }

        public ParamPage GetPage(int index)
        {
            return _data[index];
        }
    }
}