namespace FXCalculator.Data.Helpers
{
    public static class DataFileReader
    {
        public static string ReadFromFile(string path, string fileName)
        {
            var filePath = $@"{path}\{fileName}";
            try
            {
                return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath));
            }
            catch
            {
                throw new DataFileReaderException($"Error loading file with name {fileName}");
            }
        }
    }
}
