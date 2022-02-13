using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Asset;
using FormatKFIV.Utility;

namespace FormatKFIV.FileFormat
{
    public class FFParamItemName : FIFormat<Param>
    {
        public FIParameters Parameters => throw new NotImplementedException();

        public Param LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }
        public Param LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }

        public Param ImportItemNamPrm(InputStream ins)
        {
            Param ReturningParam = new Param();

            //Create Param Layout
            ReturningParam.SetLayout(new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[8],
            });
            ReturningParam.Layout.Columns[0] = new Param.ParamColumn
            {
                Name = "Item Name",
                DataType = Param.ParamColumnFormat.DTString
            };

            //Create Param Page
            Param.ParamPage itemPage = new Param.ParamPage
            {
                pageRows = new List<Param.ParamRow>()
            };

            return ReturningParam;
        }


        public void SaveToFile(string filepath, Param data)
        {
            throw new NotImplementedException();
        }
    }
}
