using MappingSystem.Core;

namespace MappingSystem.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var _mapper = new MapHandler();
            var model = _mapper.Map(null, sourceType: "Model.Reservation", targetType: " Google.Reservation");
            Console.WriteLine("Hello, World!");
        }
    }
}
