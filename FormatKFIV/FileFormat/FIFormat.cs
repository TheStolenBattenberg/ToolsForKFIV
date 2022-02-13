namespace FormatKFIV.FileFormat
{
    public interface FIFormat<T>
    {
        FIParameters Parameters { get; }

        T LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4);
        T LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4);

        void SaveToFile(string filepath, T data);
    }
}