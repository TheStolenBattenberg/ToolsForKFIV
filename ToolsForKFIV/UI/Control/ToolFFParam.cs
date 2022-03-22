using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FormatKFIV.FileFormat;
using FormatKFIV.Asset;

namespace ToolsForKFIV.UI.Control
{
    public partial class ToolFFParam : UserControl
    {
        //Members
        private Param paramData;
        private int currentPage;

        public ToolFFParam()
        {
            InitializeComponent();
        }


        public void SetParamData(Param newParams)
        {
            paramData = newParams;

            //Get Param Layout
            Param.ParamLayout layout = paramData.Layout;

            //Clear row data
            tpeDataGrid.Columns.Clear();

            //Fill Column data
            foreach (Param.ParamColumn column in layout.Columns)
            {
                tpeDataGrid.Columns.Add(column.Name.Replace(" ", ""), column.Name);
            }

            //Fill Page Data
            ptPageBox.Items.Clear();
            for(int i = 0; i < paramData.PageCount; ++i)
            {
                Param.ParamPage page = paramData.GetPage(i);
                ptPageBox.Items.Add($"Page #{i.ToString("D4")} - " + page.pageName);
            }

            //Set first page
            ptPageBox.SelectedIndex = 0;
            ptPageBox.SelectedItem = ptPageBox.Items[0];
        }
        private void SetParamsPage(int index)
        {
            //Get param page.
            Param.ParamPage page = paramData.GetPage(index);
            Console.WriteLine("Page Name: " + page.pageName);

            //Clear row data
            tpeDataGrid.Rows.Clear();

            //Fill Row Data
            foreach (Param.ParamRow row in page.pageRows)
            {
                int newRowID = tpeDataGrid.Rows.Add();
                DataGridViewRow newRow = tpeDataGrid.Rows[newRowID];

                //For each column...
                for (int i = 0; i < paramData.ColumnCount; ++i)
                {
                    newRow.Cells[i].Value = row.values[i];
                }
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
