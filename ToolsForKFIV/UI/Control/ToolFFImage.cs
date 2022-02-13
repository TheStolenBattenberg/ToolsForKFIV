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
    public partial class ToolFFImage : UserControl
    {
        //ComboBox Item
        private class ITComboboxItem
        {
            public ITComboboxItem(string text, FIFormat<Texture> value)
            {
                Text = text;
                Value = value;
            }

            public string Text { get; set; }
            public FIFormat<Texture> Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        //Members
        private List<FIFormat<Texture>> exportableFormat = new List<FIFormat<Texture>>();
        private List<Bitmap> bitmaps = new List<Bitmap>();
        private Texture texture;

        public ToolFFImage()
        {
            InitializeComponent();
        }

        #region Preview Tab Events
        private void itImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetViewImage(itImageList.SelectedIndex);
        }

        #endregion

        #region Export Tab Events
        private void tiExport_Enter(object sender, EventArgs e)
        {
            //clear exportable format list
            exportableFormat.Clear();
            itcbExpFmt.Items.Clear();

            //Get all exportable texture formats
            exportableFormat.AddRange(ResourceManager.GetExportableTextureFormats());

            //Add Exportable formats to list
            foreach(FIFormat<Texture> fmt in exportableFormat)
            {
                itcbExpFmt.Items.Add(new ITComboboxItem(fmt.Parameters.FormatDescription, fmt));
            }
            itcbExpFmt.SelectedItem = itcbExpFmt.Items[0];
        }
        private void itbtExpSelPath_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                //Copy format details for FIFormat Parameters
                FIFormat<Texture> fmt = ((ITComboboxItem)itcbExpFmt.SelectedItem).Value;
                sfd.Filter = fmt.Parameters.FormatFilter;

                //Show the dialog
                switch (sfd.ShowDialog())
                {
                    case DialogResult.OK:
                        ittbExpPath.Text = sfd.FileName;
                        break;
                }
            }
        }
        private void itBtDoExport_Click(object sender, EventArgs e)
        {
            FIFormat<Texture> fmt = ((ITComboboxItem)itcbExpFmt.SelectedItem).Value;
            fmt.SaveToFile(ittbExpPath.Text, texture);
        }

        #endregion

        public void SetTextureData(Texture texture)
        {
            //Clear Old Data From List
            itImageList.Items.Clear();

            //Clear old bitmaps
            for (int i = 0; i < bitmaps.Count; ++i)
                bitmaps[i].Dispose();
            bitmaps.Clear();

            //Clear Picture Box
            if (itPicture.Image != null)
            {
                itPicture.Image = null;
            }

            //Set Texture
            this.texture = texture;

            //Enumurate Images
            for (int i = 0; i < texture.SubimageCount; ++i)
            {
                //Get Subimage
                Texture.ImageBuffer? subimg = texture.GetSubimage(i);

                if(subimg.Value.Name != null)
                {
                    itImageList.Items.Add(subimg.Value.Name);
                }
                else
                {
                    itImageList.Items.Add("Image # " + (i + 1));
                }

                bitmaps.Add(texture.GetBitmap(i));
            }

            //Load first subimage
            SetViewImage(0);
        }
        private void SetViewImage(int index)
        {
            //Only needed because netcore winforms is total garbage.
            if(index < 0 || index > bitmaps.Count)
                return;

            //Update Picture Box with Bitmap
            itPicture.Image = bitmaps[index];

            //Clear Properties
            itIData.Rows.Clear();
            itCData.Rows.Clear();

            //Update Image Properties
            Texture.ImageBuffer? subImage = texture.GetSubimage(index);
            if (subImage.HasValue)
            {
                //Image
                itIData.Rows.Add("Width (px)", subImage.Value.Width);
                itIData.Rows.Add("Height (px)", subImage.Value.Height);
                itIData.Rows.Add("Length (byte)", subImage.Value.Length);
                itIData.Rows.Add("Format", Enum.GetName(typeof(Texture.ColourMode), subImage.Value.Format));
                itIData.Rows.Add("Clut Count", subImage.Value.ClutCount);

                for (int i = 0; i < subImage.Value.ClutCount; ++i)
                {
                    Texture.ClutBuffer? clut = texture.GetClut(subImage.Value.ClutIDs[i]);
                    itIData.Rows.Add("Clut #" + i.ToString() + "ID", subImage.Value.ClutIDs[i]);

                    itCData.Rows.Add("Clut #" + i.ToString() + " Width (px)", clut.Value.Width);
                    itCData.Rows.Add("Clut #" + i.ToString() + " Height (px)", clut.Value.Height);
                    itCData.Rows.Add("Clut #" + i.ToString() + " Length (byte)", clut.Value.Length);
                    itCData.Rows.Add("Clut #" + i.ToString() + " Format", Enum.GetName(typeof(Texture.ColourMode), clut.Value.Format));
                }
            }
        }
    }
}
