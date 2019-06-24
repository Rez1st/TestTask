namespace App.Infrastructure
{
    public interface ITagFinderService
    {
        string Locate(string filePath);
    }
}