using System;
using System.Windows.Forms;

using FormatKFIV.FileFormat;
using FormatKFIV.Asset;

namespace ToolsForKFIV.UI.Control
{
    public partial class ToolFFParam : UserControl
    {
        //Members
        private Param paramData;

        public ToolFFParam()
        {
            InitializeComponent();
        }

        public void SetParamData(Param newParams)
        {
            if(newParams == null)
            {
                Logger.LogWarn("Invalid parameter file.");
                throw new Exception("Bad Parameter file.");
            }

            paramData = newParams;

            //Load Page Names
            ptPageBox.Items.Clear();

            for(int i = 0; i < paramData.PageCount; ++i)
            {
                Param.ParamPage page = paramData.Pages[i];
                ptPageBox.Items.Add($"Page #{i.ToString("D4")} - " + page.name);
            }

            //Load Page #0000
            ptPageBox.SelectedIndex = 0;
            ptPageBox.SelectedItem = ptPageBox.Items[0];
            SetParamsPage(0);
        }

        private void SetParamsPage(int index)
        {
            if(index < 0 || index > paramData.PageCount)
            {
                Logger.LogWarn("Invalid page index: " + index.ToString());
                return;
            }

            //Load Param Page
            Param.ParamPage page = paramData.Pages[index];

            //Columns
            tpeDataGrid.Columns.Clear();
            foreach(Param.ParamColumn column in page.layout.Columns)
            {
                tpeDataGrid.Columns.Add(column.Name.Replace(" ", ""), column.Name);
            }

            //Rows
            tpeDataGrid.Rows.Clear();
            foreach(Param.ParamRow row in page.rows)
            {
                int rowID = tpeDataGrid.Rows.Add();
                DataGridViewRow rowData = tpeDataGrid.Rows[rowID];

                for(int i = 0; i < page.layout.ColumnCount; ++i)
                {
                    rowData.Cells[i].Value = row.values[i];
                }

                rowData.HeaderCell.Value = string.Format("{0}", rowID + 1);
            }
        }

        #region Data Grid Events
        private void ToolFFParamEditor_Resize(object sender, EventArgs e)
        {
            tpeDataGrid.AutoResizeColumns();
        }

        #endregion

        private void ptPageBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ptPageBox.SelectedIndex >= 0 && ptPageBox.SelectedIndex < paramData.PageCount)
            {
                SetParamsPage(ptPageBox.SelectedIndex);
            }
        }
    }
}
