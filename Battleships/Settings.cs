namespace Battleships
{
    public interface ISettings
    {
        int BoardSize { get; }
    }
    
    public class Settings : ISettings
    {
        public int BoardSize { get; set; }
        
        public Settings()
        {
            BoardSize = 10;
        }
    }
}