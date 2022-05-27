namespace MyShuttle.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public static class MyShuttleDataInitializer
    {
        private static readonly Random _randomize = new Random();

        private const int _defaultCarrierId = 1;

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var context = serviceProvider.GetService<MyShuttleContext>();


            var databaseCreated = await context.Database.EnsureCreatedAsync();
            if (databaseCreated)
            {
                await CreateSampleData(serviceProvider, context, configuration);
            }

        }

        private static async Task CreateSampleData(IServiceProvider serviceProvider, MyShuttleContext context, IConfiguration configuration)
        {
            _ = CreateCarrier_01(context);
            await CreateDefaultUser(serviceProvider, configuration);
            CreateCarriers(context);
        }

        /// <summary>
        /// Creates a store manager user who can manage the inventory.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>        
        private static async Task CreateDefaultUser(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            const string defaultLogin = "carrier";
            const string defaultPassword = "P@ssw0rd"; // [SuppressMessage("Microsoft.Security", "CS001:SecretInline", Justification="Just a Sample Password")]


            var login = string.IsNullOrEmpty(configuration["DefaultUsername"]) ? defaultLogin : configuration["DefaultUsername"];
            var password = string.IsNullOrEmpty(configuration["DefaultPassword"]) ? defaultPassword : configuration["DefaultPassword"];

            var user = await userManager.FindByNameAsync(login);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = login,
                    CarrierId = _defaultCarrierId,
                    EmailConfirmed = true,
                    Email = $"{login}@myshuttle.biz"
                };

                await userManager.CreateAsync(user, password);
                await userManager.AddClaimAsync(user, new Claim("ManageStore", "Allowed"));
            }
        }


        private static void CreateCarriers(MyShuttleContext context)
        {
            CreateCustomer_01(context);

            CreateVehicleAndDriver_01(_defaultCarrierId, context);
            CreateVehicleAndDriver_02(_defaultCarrierId, context);
            CreateVehicleAndDriver_03(_defaultCarrierId, context);
            CreateVehicleAndDriver_04(_defaultCarrierId, context);
            CreateVehicleAndDriver_05(_defaultCarrierId, context);
            CreateVehicleAndDriver_06(_defaultCarrierId, context);
            CreateVehicleAndDriver_07(_defaultCarrierId, context);
            CreateVehicleAndDriver_08(_defaultCarrierId, context);
            CreateVehicleAndDriver_09(_defaultCarrierId, context);
            CreateVehicleAndDriver_10(_defaultCarrierId, context);

            CreateVehicleAndDriver_11(_defaultCarrierId, context);
            CreateVehicleAndDriver_12(_defaultCarrierId, context);
            CreateVehicleAndDriver_13(_defaultCarrierId, context);
            CreateVehicleAndDriver_14(_defaultCarrierId, context);
            CreateVehicleAndDriver_15(_defaultCarrierId, context);
            CreateVehicleAndDriver_16(_defaultCarrierId, context);
            CreateVehicleAndDriver_17(_defaultCarrierId, context);
            CreateVehicleAndDriver_18(_defaultCarrierId, context);
            CreateVehicleAndDriver_19(_defaultCarrierId, context);

            var carrierId_02 = CreateCarrier_02(context);
            CreateVehicleAndDriver_20(carrierId_02, context);
            CreateVehicleAndDriver_21(carrierId_02, context);

            CreateCarrier_03(context);
            CreateCarrier_04(context);
            CreateCarrier_05(context);
            CreateCarrier_06(context);
            CreateCarrier_07(context);
            CreateCarrier_08(context);

            CreateDrivers(_defaultCarrierId, context);
            CreateRides(_defaultCarrierId, context);
        }

        private static void CreateCustomer_01(MyShuttleContext context)
        {
            var customer = new Customer
            {
                Address = "15010 NE 36th Street",
                City = "Redmond",
                Country = "United States",
                CompanyID = "1234-344",
                Email = "mail@microsoft.com",
                Name = "Microsoft",
                Phone = "555-555-555",
                State = "Washington",
                ZipCode = "98052"
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            CreateEmployees(customer.CustomerId, context);
        }

        private static void CreateEmployees(int customerId, MyShuttleContext context)
        {
            int employeeId = 0;
            if (context.Employees.Any())
            {
                employeeId = context.Employees.Max(e => e.EmployeeId);
            }

            var employees = new List<Employee>()
            {
                new Employee
                {
                    Id = "90a0d28b-4342-40d5-92b6-af2d0ea7630b",
                    CustomerId = customerId,
                    Email = "scottha@microsoft.com",
                    Name = "Scott Hanselman",
                    Picture = GetEmplyoyee(1),
                },
                new Employee
                {
                    Id = "bbe41151-abfa-42da-80c1-91a56cad538f",
                    CustomerId = customerId,
                    Email = "mitra@microsoft.com",
                    Name = "Mitra Azizirad",
                    Picture = GetEmplyoyee(2),
                },
                new Employee
                {
                    Id = "c7a9c8a2-a5e3-48d4-99e4-4117599af048",
                    CustomerId = customerId,
                    Email = "amanda@microsoft.com",
                    Name = "Amanda Silver",
                    Picture = GetEmplyoyee(3),
                },
                new Employee
                {
                    Id = "4e6767fb-a723-4ce0-9547-f0d349658f9d",
                    CustomerId = customerId,
                    Email = "nicole@microsoft.com",
                    Name = "Nicole Herskowitz",
                    Picture = GetEmplyoyee(4),
                },
                new Employee
                {
                    Id = "b9b62397-7c1b-4fb8-898f-a9bbc0234fdf",
                    CustomerId = customerId,
                    Email = "scottgu@microsoft.com",
                    Name = "Scott Guthrie",
                    Picture = GetDriver(10),
                }
            };

            context.Employees.AddRange(employees.ToArray<Employee>());
            context.SaveChanges();
        }

        private static int CreateCarrier_01(MyShuttleContext context)
        {
            var carrier = new Carrier
            {
                Name = "James&McCarthy",
                Description = "James&McCarthy is your best option of available carriers, and you know it!",
                CompanyID = "03-00-00427-CV",
                Address = "3956 Broadway, New York, NY 10032 (212)",
                ZipCode = "568-3700",
                City = "New York City",
                State = "New York",
                Country = "USA",
                Phone = "555-567",
                Email = "info@carrier.com",
                Picture = GetCarrier(3),
                RatingAvg = _randomize.Next(0, 5),
            };

            context.Carriers.Add(carrier);
            context.SaveChanges();

            return carrier.CarrierId;
        }

        private static int CreateCarrier_02(MyShuttleContext context)
        {
            var carrier = new Carrier
            {
                Name = "Robert Cars",
                Description = "Robert Cars is your best option of available carriers.",
                CompanyID = "03-00-00427-CV",
                Address = "3956 Broadway, New York, NY 10032 (212)",
                ZipCode = "568-3700",
                City = "New York City",
                State = "New York",
                Country = "USA",
                Phone = "555-FAST&CALM",
                Email = "info@FastCalm.com",
                Picture = GetCarrier(5),
                RatingAvg = _randomize.Next(0, 5),
            };

            context.Carriers.Add(carrier);

            context.SaveChanges();

            return carrier.CarrierId;
        }

        private static int CreateCarrier_03(MyShuttleContext context)
        {
            var carrier = new Carrier
            {
                Name = "Light red cabs",
                Description = "Light red cabs is your best option of available carriers-.",
                CompanyID = "03-00-00427-CV",
                Address = "3956 Broadway, New York, NY 10032 (212)",
                ZipCode = "568-3700",
                City = "New York City",
                State = "New York",
                Country = "USA",
                Phone = "555-FAST&CALM",
                Email = "info@FastCalm.com",
                Picture = GetCarrier(4),
                RatingAvg = _randomize.Next(0, 5),
            };

            context.Carriers.Add(carrier);

            context.SaveChanges();

            return carrier.CarrierId;
        }

        private static int CreateCarrier_04(MyShuttleContext context)
        {
            var carrier = new Carrier
            {
                Name = "S&W Shuttles",
                Description = "S&W Shuttles is your best option of available carriers-.",
                CompanyID = "03-00-00427-CV",
                Address = "3956 Broadway, New York, NY 10032 (212)",
                ZipCode = "568-3700",
                City = "New York City",
                State = "New York",
                Country = "USA",
                Phone = "555-FAST&CALM",
                Email = "info@FastCalm.com",
                Picture = GetCarrier(7),
                RatingAvg = _randomize.Next(0, 5),
            };

            context.Carriers.Add(carrier);

            context.SaveChanges();

            return carrier.CarrierId;
        }

        private static int CreateCarrier_05(MyShuttleContext context)
        {
            var carrier = new Carrier
            {
                Name = "Smith Cabs",
                Description = "Smith Cabs is your best option of available carriers, and you know it!",
                CompanyID = "03-00-00427-CV",
                Address = "3956 Broadway, New York, NY 10032 (212)",
                ZipCode = "568-3700",
                City = "New York City",
                State = "New York",
                Country = "USA",
                Phone = "555-FAST&CALM",
                Email = "info@FastCalm.com",
                Picture = GetCarrier(6),
                RatingAvg = _randomize.Next(0, 5),
            };

            context.Carriers.Add(carrier);
            context.SaveChanges();
            return carrier.CarrierId;
        }

        private static int CreateCarrier_06(MyShuttleContext context)
        {
            var carrier = new Carrier
            {
                Name = "United Drivers&Vehicles",
                Description = "CompanyOne is your best option of available carriers-.",
                CompanyID = "03-00-00427-CV",
                Address = "3956 Broadway, New York, NY 10032 (212)",
                ZipCode = "568-3700",
                City = "New York City",
                State = "New York",
                Country = "USA",
                Phone = "555-FAST&CALM",
                Email = "info@FastCalm.com",
                Picture = GetCarrier(9),
                RatingAvg = _randomize.Next(0, 5),
            };

            context.Carriers.Add(carrier);

            context.SaveChanges();

            return carrier.CarrierId;
        }

        private static int CreateCarrier_07(MyShuttleContext context)
        {
            var carrier = new Carrier
            {
                Name = "Emblem",
                Description = "Emblem is your best option of available carriers-.",
                CompanyID = "03-00-00427-CV",
                Address = "3956 Broadway, New York, NY 10032 (212)",
                ZipCode = "568-3700",
                City = "New York City",
                State = "New York",
                Country = "USA",
                Phone = "555-FAST&CALM",
                Email = "info@FastCalm.com",
                Picture = GetCarrier(2),
                RatingAvg = _randomize.Next(0, 5),
            };

            context.Carriers.Add(carrier);

            context.SaveChanges();

            return carrier.CarrierId;
        }

        private static int CreateCarrier_08(MyShuttleContext context)
        {
            var carrier = new Carrier
            {
                Name = "Threeangle",
                Description = "Threeangle is your best option of available carriers-.",
                CompanyID = "03-00-00427-CV",
                Address = "3956 Broadway, New York, NY 10032 (212)",
                ZipCode = "568-3700",
                City = "New York City",
                State = "New York",
                Country = "USA",
                Phone = "555-FAST&CALM",
                Email = "info@FastCalm.com",
                Picture = GetCarrier(8),
                RatingAvg = _randomize.Next(0, 5),
            };

            context.Carriers.Add(carrier);

            context.SaveChanges();

            return carrier.CarrierId;
        }

        private static void CreateDrivers(int carrierId, MyShuttleContext context)
        {
            var drivers = new[]
            {
                new Driver
                {
                    Name = "Carlos Zimmermann",
                    Phone = "555-178895",
                    Picture = GetDriver(11),
                    CarrierId = carrierId,
                    RatingAvg = _randomize.Next(0, 5),
                    TotalRides = _randomize.Next(50, 100),
                },
                new Driver
                {
                    Name = "Andrew Davis",
                    Phone = "555-178895",
                    Picture = GetDriver(12),
                    CarrierId = carrierId,
                    RatingAvg = _randomize.Next(0, 5),
                    TotalRides = _randomize.Next(50, 100),
                },
                new Driver
                {
                    Name = "Carolina Anderson",
                    Phone = "555-178895",
                    Picture = GetDriver(13),
                    CarrierId = carrierId,
                    RatingAvg = _randomize.Next(0, 5),
                    TotalRides = _randomize.Next(50, 100),
                },
                new Driver
                {
                    Name = "Julian Thomas",
                    Phone = "555-178895",
                    Picture = GetDriver(14),
                    CarrierId = carrierId,
                    RatingAvg = _randomize.Next(0, 5),
                    TotalRides = _randomize.Next(50, 100),
                }
            };

            context.Drivers.AddRange(drivers);
            context.SaveChanges();

        }

        private static void CreateVehicleAndDriver_01(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Cesar de la Torre",
                Phone = "555-178895",
                Picture = GetDriver(1),
                CarrierId = carrierId,
                RatingAvg = 4.5,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "SLV-4335",
                Model = "Pacifica",
                Make = "Chrysler",
                Type = VehicleType.Compact,
                Seats = 4,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                CarrierId = carrierId,
                Picture = GetVehicle(1),
                Latitude = 47.621931,
                Longitude = -122.127232,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "SLV-4335",
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_02(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "David Carmona",
                Phone = "555-48970",
                Picture = GetDriver(2),
                CarrierId = carrierId,
                RatingAvg = 4.6,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "MAX-9876",
                Model = "Town Car",
                Make = "Lincoln",
                Type = VehicleType.Van,
                Seats = 7,
                VehicleStatus = VehicleStatus.Free,
                DriverId = driver.DriverId,
                Picture = GetVehicle(2),
                Latitude = 47.621934,
                Longitude = 122.127222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "MAX-9876"
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_03(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "David Salgado",
                Phone = "555-48970",
                Picture = GetDriver(3),
                CarrierId = carrierId,
                RatingAvg = 3,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "ZCC-1432",
                Model = "Taurus",
                Make = "Ford",
                Type = VehicleType.Van,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(3),
                Latitude = 47.741924,
                Longitude = 122.127222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "CCC-1432"
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_04(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Dmitry Lyalin",
                Phone = "555-48970",
                Picture = GetDriver(4),
                CarrierId = carrierId,
                RatingAvg = _randomize.Next(0, 4),
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "JNV-9876",
                Model = "300C",
                Make = "Chrysler",
                Type = VehicleType.Van,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(4),
                Latitude = 47.841914,
                Longitude = -122.127222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "JNV-9876"
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_05(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Scott Hanselman",
                Phone = "555-48970",
                Picture = GetDriver(5),
                CarrierId = carrierId,
                RatingAvg = 4.8,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "FGH-9876",
                Model = "Volt2",
                Make = "Chevrolet",
                Type = VehicleType.Van,
                Seats = 7,
                VehicleStatus = VehicleStatus.Free,
                DriverId = driver.DriverId,
                Picture = GetVehicle(5),
                Latitude = 47.631925,
                Longitude = -122.127220,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "FGH-9876"
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_06(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Erich Gamma",
                Phone = "555-48970",
                Picture = GetDriver(6),
                CarrierId = carrierId,
                RatingAvg = _randomize.Next(0, 3),
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "ZUA-1456",
                Model = "Caprice PPV",
                Make = "Chevrolet",
                Type = VehicleType.Van,
                Seats = 7,
                VehicleStatus = VehicleStatus.Free,
                DriverId = driver.DriverId,
                Picture = GetVehicle(6),
                Latitude = 47.631935,
                Longitude = -122.127222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "CUA-1456",
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_07(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Mitra Azizirad",
                Phone = "555-48970",
                Picture = GetDriver(7),
                CarrierId = carrierId,
                RatingAvg = 4.6,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "JOP-9876",
                Model = "BLS",
                Make = "Cadillac",
                Type = VehicleType.Van,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(7),
                Latitude = 47.631915,
                Longitude = -122.127221,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "JOP-9876",
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_08(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Rob Caron",
                Phone = "555-48970",
                Picture = GetDriver(8),
                CarrierId = carrierId,
                RatingAvg = _randomize.Next(0, 3),
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "HTY-1243",
                Model = "ELR",
                Make = "Cadilac",
                Type = VehicleType.Van,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(8),
                Latitude = 47.621954,
                Longitude = -122.127212,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "HTY-1243",
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_09(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "S. Somasegar",
                Phone = "555-48970",
                Picture = GetDriver(9),
                CarrierId = carrierId,
                RatingAvg = _randomize.Next(0, 3),
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "VVV-4444",
                Model = "AveoLT",
                Make = "Chevrolet",
                Type = VehicleType.Van,
                Seats = 7,
                VehicleStatus = VehicleStatus.Free,
                DriverId = driver.DriverId,
                Picture = GetVehicle(9),
                Latitude = 47.641964,
                Longitude = -122.127212,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "VVV-4444",
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_10(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Scott Guthrie",
                Phone = "555-48970",
                Picture = GetDriver(10),
                CarrierId = carrierId,
                RatingAvg = 5,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "ERT-1256",
                Model = "Caliber",
                Make = "Dodge",
                Type = VehicleType.Van,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(10),
                Latitude = 48.641924,
                Longitude = -122.127212,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "ERT-1256",
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_11(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "James Rodriguez",
                Phone = "555-48970",
                Picture = GetNoMsDriver(1),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "WWW-1256",
                Model = "Dart",
                Make = "Dodge",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(11),
                Latitude = 47.642944,
                Longitude = -122.128222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "deviceVehicleSIM"
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_12(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Rose Anderson",
                Phone = "555-48970",
                Picture = GetNoMsDriver(2),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "ZVC-1256",
                Model = "G8",
                Make = "Pontiac",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(12),
                Latitude = 47.643944,
                Longitude = -122.126222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = string.Empty,
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_13(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Aaron McDonall",
                Phone = "555-48970",
                Picture = GetNoMsDriver(3),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "HUL-5678",
                Model = "Aura XR",
                Make = "Saturn",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(13),
                Latitude = 47.644944,
                Longitude = -122.125222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "deviceVehicleRC"
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_14(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Alan Anderson",
                Phone = "555-48970",
                Picture = GetNoMsDriver(4),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "WTW-6182",
                Model = "G6",
                Make = "Pontiac",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(14),
                Latitude = 47.645944,
                Longitude = -122.157222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = string.Empty
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_15(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Austin Lumbert",
                Phone = "555-48970",
                Picture = GetNoMsDriver(5),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "OUJ-6182",
                Model = "G5",
                Make = "Pontiac",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(15),
                Latitude = 47.641950,
                Longitude = -122.167222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = string.Empty
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_16(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Wendy Torre",
                Phone = "555-48970",
                Picture = GetNoMsDriver(14),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "ERT-1289",
                Model = "Regal",
                Make = "Buick",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(16),
                Latitude = 47.641951,
                Longitude = -122.177222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = string.Empty
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_17(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Ray J. Poland",
                Phone = "555-48970",
                Picture = GetNoMsDriver(7),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "RTY-2345",
                Model = "Impala LTZ",
                Make = "Chevrolet",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(17),
                Latitude = 47.641952,
                Longitude = -122.187222,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = string.Empty
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_18(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Ibon Campa",
                Phone = "555-48970",
                Picture = GetNoMsDriver(10),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "RTY-2345",
                Model = "Riviera",
                Make = "Buick",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(18),
                Latitude = 47.641944,
                Longitude = -122.187212,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = "deviceVehicleLED"
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_19(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Carlos Rodriguez",
                Phone = "555-48970",
                Picture = GetNoMsDriver(9),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "ERT-2345",
                Model = "Impala LTZ",
                Make = "Chevrolet",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(19),
                Latitude = 47.641944,
                Longitude = -122.197232,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = string.Empty
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_20(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Alec Watson",
                Phone = "555-48970",
                Picture = GetNoMsDriver(10),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "TRT-9473",
                Model = "G6",
                Make = "Pontiac",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(20),
                Latitude = 47.641944,
                Longitude = -122.227242,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = string.Empty
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateVehicleAndDriver_21(int carrierId, MyShuttleContext context)
        {
            var driver = new Driver
            {
                Name = "Andry Jackson",
                Phone = "555-48970",
                Picture = GetNoMsDriver(11),
                CarrierId = carrierId,
                RatingAvg = 2,
                TotalRides = _randomize.Next(50, 100),
            };
            context.Drivers.Add(driver);
            context.SaveChanges();

            var vehicle = new Vehicle
            {
                LicensePlate = "WIE-4545",
                Model = "Grand Prix",
                Make = "Pontiac",
                Type = VehicleType.Luxury,
                Seats = 7,
                VehicleStatus = VehicleStatus.Occupied,
                DriverId = driver.DriverId,
                Picture = GetVehicle(21),
                Latitude = 47.641944,
                Longitude = -122.327252,
                CarrierId = carrierId,
                Rate = _randomize.Next(1, 5),
                RatingAvg = _randomize.Next(0, 5),
                TotalRides = _randomize.Next(50, 100),
                DeviceId = string.Empty
            };

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private static void CreateRides(int carrierId, MyShuttleContext context)
        {
            var employeeList = context.Employees.Select(e => e.EmployeeId).ToArray();

            var rides = new List<Ride>();

            int rideId = 0;
            if (context.Rides.Any())
                rideId = context.Rides.Max(e => e.RideId);

            foreach (Vehicle vehicle in context.Vehicles)
            {
                var startDateTime01 = DateTime.UtcNow.AddDays(-_randomize.Next(15, 30));
                var startDateTime02 = DateTime.UtcNow.AddDays(-_randomize.Next(15, 30));
                var startDateTime03 = DateTime.UtcNow.AddDays(-_randomize.Next(0, 15));
                var startDateTime04 = DateTime.UtcNow.AddDays(-_randomize.Next(0, 15));
                var startDateTime05 = DateTime.UtcNow.AddDays(-_randomize.Next(10, 20));
                var startDateTime06 = DateTime.UtcNow.AddDays(-_randomize.Next(0, 10));


                var distance = _randomize.Next(20, 40);
                var endTime = startDateTime01.AddMinutes(3 * distance);
                var duration = distance * 3;
                var cost = distance * vehicle.Rate;

                var ride = new Ride
                {
                    Id = Guid.NewGuid().ToString(),
                    StartDateTime = startDateTime01,
                    EndDateTime = endTime,
                    VehicleId = vehicle.VehicleId,
                    DriverId = vehicle.DriverId,
                    Cost = cost,
                    EmployeeId = employeeList[_randomize.Next(0, employeeList.Count() - 1)],
                    Comments = string.Empty,
                    CarrierId = carrierId,
                    Distance = distance,
                    Duration = duration,
                    Rating = _randomize.Next(3, 5),
                    Signature = null,
                    StartAddress = "Madison Ave 10037",
                    EndAddress = "217 Broadway",
                    StartLatitude = 40.721847,
                    StartLongitude = -74.007326,
                    EndLatitude = 40.721847,
                    EndLongitude = -74.007326,
                };
                rides.Add(ride);

                distance = _randomize.Next(20, 40);
                endTime = startDateTime02.AddMinutes(3 * distance);
                duration = distance * 3;
                cost = distance * vehicle.Rate;

                ride = new Ride
                {
                    Id = Guid.NewGuid().ToString(),
                    StartDateTime = startDateTime02,
                    EndDateTime = endTime,
                    VehicleId = vehicle.VehicleId,
                    DriverId = vehicle.DriverId,
                    Cost = cost,
                    EmployeeId = employeeList[_randomize.Next(0, employeeList.Count() - 1)],
                    Comments = string.Empty,
                    CarrierId = carrierId,
                    Distance = distance,
                    Duration = 20,
                    Rating = _randomize.Next(3, 5),
                    Signature = null,
                    StartAddress = "217 Broadway",
                    EndAddress = "E 156th St, Bronx",
                    StartLatitude = 40.721847,
                    StartLongitude = -74.007326,
                    EndLatitude = 40.721847,
                    EndLongitude = -74.007326,
                };
                rides.Add(ride);

                distance = _randomize.Next(20, 40);
                endTime = startDateTime03.AddMinutes(3 * distance);
                duration = distance * 3;
                cost = distance * vehicle.Rate;

                ride = new Ride
                {
                    Id = Guid.NewGuid().ToString(),
                    StartDateTime = startDateTime03,
                    EndDateTime = endTime,
                    VehicleId = vehicle.VehicleId,
                    DriverId = vehicle.DriverId,
                    Cost = cost,
                    EmployeeId = employeeList[_randomize.Next(0, employeeList.Count() - 1)],
                    Comments = string.Empty,
                    CarrierId = carrierId,
                    Distance = distance,
                    Duration = duration,
                    Rating = _randomize.Next(2, 5),
                    Signature = null,
                    StartAddress = "54 Fulton St",
                    EndAddress = "E 156th St, Bronx",
                    StartLatitude = 40.721847,
                    StartLongitude = -74.007326,
                    EndLatitude = 40.721847,
                    EndLongitude = -74.007326,
                };
                rides.Add(ride);

                distance = _randomize.Next(20, 40);
                endTime = startDateTime04.AddMinutes(3 * distance);
                duration = distance * 3;
                cost = distance * vehicle.Rate;
                ride = new Ride
                {
                    Id = Guid.NewGuid().ToString(),
                    StartDateTime = startDateTime04,
                    EndDateTime = endTime,
                    VehicleId = vehicle.VehicleId,
                    DriverId = vehicle.DriverId,
                    Cost = cost,
                    EmployeeId = employeeList[_randomize.Next(0, employeeList.Count() - 1)],
                    Comments = string.Empty,
                    CarrierId = carrierId,
                    Distance = distance,
                    Duration = duration,
                    Rating = _randomize.Next(2, 5),
                    Signature = null,
                    StartAddress = "E 156th St, Bronx",
                    EndAddress = "33 Wooster St",
                    StartLatitude = 40.721847,
                    StartLongitude = -74.007326,
                    EndLatitude = 40.721847,
                    EndLongitude = -74.007326,
                };
                rides.Add(ride);

                distance = _randomize.Next(20, 40);
                endTime = startDateTime05.AddMinutes(3 * distance);
                duration = distance * 3;
                cost = distance * vehicle.Rate;
                ride = new Ride
                {
                    Id = Guid.NewGuid().ToString(),
                    StartDateTime = startDateTime05,
                    EndDateTime = endTime,
                    VehicleId = vehicle.VehicleId,
                    DriverId = vehicle.DriverId,
                    Cost = cost,
                    EmployeeId = employeeList[_randomize.Next(0, employeeList.Count() - 1)],
                    Comments = string.Empty,
                    CarrierId = carrierId,
                    Distance = distance,
                    Duration = duration,
                    Rating = _randomize.Next(2, 5),
                    Signature = null,
                    StartAddress = "48 Wall St",
                    EndAddress = "Madison Ave 10037",
                    StartLatitude = 40.721847,
                    StartLongitude = -74.007326,
                    EndLatitude = 40.721847,
                    EndLongitude = -74.007326,
                };
                rides.Add(ride);

                distance = _randomize.Next(20, 40);
                endTime = startDateTime06.AddMinutes(3 * distance);
                duration = distance * 3;
                cost = distance * vehicle.Rate;
                ride = new Ride
                {
                    Id = Guid.NewGuid().ToString(),
                    StartDateTime = startDateTime06,
                    EndDateTime = endTime,
                    VehicleId = vehicle.VehicleId,
                    DriverId = vehicle.DriverId,
                    Cost = cost,
                    EmployeeId = employeeList[_randomize.Next(0, employeeList.Count() - 1)],
                    Comments = "Great service!",
                    CarrierId = carrierId,
                    Distance = distance,
                    Duration = duration,
                    Rating = _randomize.Next(2, 5),
                    Signature = null,
                    StartAddress = "33 Wooster St",
                    EndAddress = "48 Wall St",
                    StartLatitude = 40.721847,
                    StartLongitude = -74.007326,
                    EndLatitude = 40.721847,
                    EndLongitude = -74.007326,
                };
                rides.Add(ride);

            }

            context.Rides.AddRange(rides.ToArray<Ride>());
            context.SaveChanges();
        }

        private static byte[] GetDriver(int index)
        {
            return Convert.FromBase64String(FakeImages.Drivers[index - 1]);
        }

        private static byte[] GetNoMsDriver(int index)
        {
            return Convert.FromBase64String(FakeImages.NoMsDrivers[index - 1]);
        }

        private static byte[] GetVehicle(int index)
        {
            return Convert.FromBase64String(FakeImages.Vehicles[index - 1]);
        }

        private static byte[] GetEmplyoyee(int index)
        {
            return Convert.FromBase64String(FakeImages.Employees[index - 1]);
        }

        private static byte[] GetCarrier(int index)
        {
            return Convert.FromBase64String(FakeImages.Carriers[index - 1]);
        }
    }
}