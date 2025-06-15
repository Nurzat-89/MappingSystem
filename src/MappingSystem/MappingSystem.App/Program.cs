using Autofac;
using MappingSystem.Core;
using System.Reflection;

namespace MappingSystem.App
{
    internal class Program
    {
        private static IContainer _container;

        static void Main(string[] args)
        {
            RegisterTypes();
            using var scope = _container.BeginLifetimeScope();
            var mapper = scope.Resolve<MapHandler>();

            var dirs21ModelSource = new Model.Reservation
            {
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                Email = "test@mail.com",
                FirstName = "Nurzat",
                LastName = "Kenzhebayev",
                Phone = "+49160332221",
                TotalPrice = 1200,
                TotalNumberOfGuests = 1,
                ReservationCode = "123"
            };
            try
            {
                var googleModel = mapper.Map(dirs21ModelSource, sourceType: "Model.Reservation", targetType: "Google.Reservation");
                WriteProperties(googleModel);
                WriteLine("Mapping Model.Reservation -> Google.Reservation succeeded", ConsoleColor.Green);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                WriteLine($"Error mapping Model.Reservation -> Google.Reservation: {ex.Message}", ConsoleColor.Red);
            }


            var googleModelSource = new Google.Reservation
            {
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                GuestEmail = "test@mail.com",
                GuestFullName = "Nurzat Kenzhebayev",
                GuestPhoneNumber = "+49160332221",
                TotalPrice = 1200,
                HotelName = "Hilton",
                NumberOfGuests = 1,
                Code = "123"
            };

            try
            {
                var dirs21Model = mapper.Map(googleModelSource, sourceType: "Google.Reservation", targetType: "Model.Reservation");
                WriteProperties(dirs21Model);
                WriteLine("Mapping Google.Reservation -> Model.Reservation succeeded", ConsoleColor.Green);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                WriteLine($"Error mapping Google.Reservation -> Model.Reservation: {ex.Message}", ConsoleColor.Red);
            }
        }

        private static void RegisterTypes()
        {
            ILifetimeScope? scope = null;
            try
            {
                var builder = new ContainerBuilder();

                // Load the Mappers assembly manually
                var assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MappingSystem.Mappers.dll");
                var mappersAssembly = Assembly.LoadFrom(assemblyPath);
                
                builder.RegisterAssemblyTypes(mappersAssembly)
                    .Where(t => typeof(IMapper).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                    .As<IMapper>();

                builder.RegisterType<MapHandler>();
                _container = builder.Build();

                scope = _container.BeginLifetimeScope();
            }
            catch
            {
                scope?.Dispose();
                throw;
            }
        }

        private static void WriteLine(string message, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = previousColor;
        }

        private static void WriteProperties(object data)
        {
            if (data == null)
            {
                WriteLine("Object is null", ConsoleColor.Yellow);
                return;
            }

            var type = data.GetType();
            WriteLine($"--- Mapped fields of {type.FullName} ---", ConsoleColor.Cyan);

            var props = type.GetProperties();
            foreach (var prop in props)
            {
                object value = null;
                try
                {
                    value = prop.GetValue(data);
                }
                catch
                {
                    value = "[unavailable]";
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{prop.Name}: ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(value ?? "null");
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
