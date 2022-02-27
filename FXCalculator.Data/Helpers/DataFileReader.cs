namespace FXCalculator.Data.Helpers
{
    public static class DataFileReader
    {
        public static string ReadFromFile(string path, string fileName)
        {
            try
            {
                return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path, fileName));
            }
            catch
            {
                throw new FileNotFoundException($"Error loading file '{path}/{fileName}'");
            }
        }
    }
}