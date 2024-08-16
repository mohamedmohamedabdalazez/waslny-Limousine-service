using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WaslnyLib.Repository
{
    public class Utilities
    {
        public enum ActionType
        {
            Add = 1,
            Edit = 2,
            Delete = 3
        }
        public enum Action
        {
            CreateCar = 1,
            EditCar = 2,
            DeleteCar = 3,

            CreateCarClassification = 4,
            EditCarClassification = 5,
            DeleteCarClassification = 6,

            CreateCarMaintenances = 7,
            EditCarMaintenances = 8,
            DeleteCarMaintenances = 9,

            CreateDriver = 10,
            EditDriver = 11,
            DeleteDriver = 12,

            CreateRoute = 13,
            EditRoute = 14,
            DeleteRoute = 15,

            CreateTrip = 16,
            EditTrip = 17,
            DeleteTrip = 18,

            CreateTripEvaluation = 19,
            EditTripEvaluation = 20,
            DeleteTripEvaluation = 21,

            CreatePrice = 22,
            EditPrice = 23,
            DeletePrice = 24,

            CreateCustomer = 25,
            EditCustomer = 26,
            DeleteCustomer = 27
        }
        public enum MasterEntity
        {
            Car = 1,
            CarClassification = 2,
            CarMaintenances = 3,
            Driver = 4,
            Route = 5,
            Trip = 6,
            TripEvaluation = 7,
            Price = 8,
            Customer = 9
        }
        public static string GetUserIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
        }
    }
}
