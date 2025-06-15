using Autofac;
using MappingSystem.Core;

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
            var googleModel = mapper.Map(dirs21ModelSource, sourceType: "Model.Reservation", targetType: "Google.Reservation");

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
            
            var dirs21Model = mapper.Map(googleModelSource, sourceType: "Google.Reservation", targetType: "Model.Reservation");
            
            Console.WriteLine("Hello, World!");
        }

        private static void RegisterTypes()
        {
            ILifetimeScope? scope = null;
            try
            {
                var builder = new ContainerBuilder();
                
                // Register all types implementing IMapper from MappingSystem.Mappers assembly
                builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => x.FullName.Split(',')[0] == "MappingSystem.Mappers").ToArray())
                    .Where(t => typeof(IMapper).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
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
    }
}
