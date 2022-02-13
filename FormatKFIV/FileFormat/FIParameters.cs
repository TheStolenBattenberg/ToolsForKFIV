namespace FormatKFIV.FileFormat
{
    public delegate bool ValidateFile(byte[] buffer);

    public enum FEType
    {
        None,
        Model,
        Texture,
        Scene,
        Param
    }

    public struct FIParameters
    {
        public string[] Extensions;
        public FEType Type;
        public ValidateFile Validator;

        public bool AllowExport;
        public string FormatFilter;
        public string FormatDescription;
    }
}
