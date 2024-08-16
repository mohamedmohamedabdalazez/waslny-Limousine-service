using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using WaslnyLib.Entity;
using WebMatrix.WebData;
using static WaslnyLib.Repository.Utilities;

namespace WaslnyLib.Repository
{
    public class WaslnyLibRepository : IWaslnyLibRepository
    {
        private readonly WaslnyLibDbContext _db;

        public int currentuserId;

        public WaslnyLibRepository()
        {
            _db = new WaslnyLibDbContext();
            currentuserId = WebSecurity.CurrentUserId;
        }
        public void CreateAudit(Utilities.ActionType actiontype, Utilities.Action action, int userId, Utilities.MasterEntity entity, string entityrecord)
        {
            try
            {
                var audit = new AuditTrial
                {
                    ActionTypeID = (int)actiontype,
                    ActionID = (int)action,
                    UserID = userId,
                    IPAddress = Utilities.GetUserIP(),
                    TransactionTime = DateTime.Now,
                    EntityID = (int)entity,
                    EntityRecord = entityrecord
                };
                _db.AuditTrials.Add(audit);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void CreateCustomer(string firstname, string lastname, bool gender, string address, string phone, string city, string email, bool isdeleted,DateTime customerdob)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (_db.Customers.Any(c => c.CustomerPhone == phone))
                    {
                        throw new Exception("The Customer Phone Number already exists.");
                    }

                    if (_db.Customers.Any(c => c.CustomerEmail == email))
                    {
                        throw new Exception("The Customer Email already exists.");
                    }
                    var customer = new Customer
                    {
                        CustomerFName = firstname,
                        CustomerLName = lastname,
                        CustomerPhone = phone,
                        CustomerCity = city,
                        CustomerEmail = email,
                        CustomerAddress = address,
                        CustomerGender = gender,
                        IsDeleted = isdeleted,
                        CustomerDOB = customerdob
                    };
                    if (customer.IsUnderAge())
                    {
                        throw new Exception("Customer must be at least 16 years old.");
                    }
                    _db.Customers.Add(customer);
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Add, Utilities.Action.CreateCustomer, currentuserId, Utilities.MasterEntity.Customer, "Create Customer "+ customer.CustomerFName+""+customer.CustomerLName);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }

        }
        public void EditCustomer(string firstname, string lastname, bool gender, string address, string phone, string city, string email, bool isdeleted, int customerid, DateTime customerdob)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (_db.Customers.Any(c => c.CustomerPhone == phone))
                    {
                        throw new Exception("The Customer Phone Number already exists.");
                    }

                    if (_db.Customers.Any(c => c.CustomerEmail == email))
                    {
                        throw new Exception("The Customer Email already exists.");
                    }

                    var customer = _db.Customers.Find(customerid);

                    customer.CustomerFName = firstname;
                    customer.CustomerLName = lastname;
                    customer.CustomerPhone = phone;
                    customer.CustomerCity = city;
                    customer.CustomerEmail = email;
                    customer.CustomerAddress = address;
                    customer.CustomerGender = gender;
                    customer.IsDeleted = isdeleted;
                    customer.CustomerDOB = customerdob;
                    if (customer.IsUnderAge())
                    {
                        throw new Exception("The Customer must be at least 16 years old.");
                    }
                    _db.Entry(customer).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Edit, Utilities.Action.EditCustomer,currentuserId, Utilities.MasterEntity.Customer, "Edit Customer " + customer.CustomerFName + " " + customer.CustomerLName);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }

        }
        public void DeleteCustomer(int customerId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Customer customer = _db.Customers.Find(customerId);
                    customer.IsDeleted = true; 
                    _db.Entry(customer).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Delete, Utilities.Action.DeleteCustomer,currentuserId, Utilities.MasterEntity.Customer, "Delete Customer " + customer.CustomerFName + " " + customer.CustomerLName);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }

        }
        public void CreateCarClassification(bool carclassificationquality, string carclassificationtype, bool isdeleted, int carid)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var carclassification = new CarClassification
                    {
                        CarClassificationQuality = carclassificationquality,
                        CarClassificationType = carclassificationtype,
                        IsDeleted = isdeleted,
                        CarID = carid
                    };
                    var car = _db.Cars.FirstOrDefault(c => c.CarID == carid);
                    var carPN = car.CarPN;
                    _db.CarClassifications.Add(carclassification);
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Add, Utilities.Action.CreateCarClassification, currentuserId, Utilities.MasterEntity.CarClassification, "Create Car Classification for" + carPN);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void EditCarClassification(bool carclassificationquality, string carclassificationtype, bool isdelete, int carid, int carclassifiactionid)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var carclassification = _db.CarClassifications.Find(carclassifiactionid);

                    carclassification.CarClassificationQuality = carclassificationquality;
                    carclassification.CarClassificationType = carclassificationtype;
                    carclassification.IsDeleted = isdelete;
                    carclassification.CarID = carid;
                    var car = _db.Cars.FirstOrDefault(c => c.CarID == carid);
                    var carPN = car.CarPN;
                    _db.Entry(carclassification).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Add, Utilities.Action.EditCarClassification, currentuserId, Utilities.MasterEntity.CarClassification, "Edit Car Classification" + carPN);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void DeleteCarClassification(int carclassificationId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    CarClassification carclassification = _db.CarClassifications.Find(carclassificationId);
                    carclassification.IsDeleted = true;
                    _db.Entry(carclassification).State = EntityState.Modified;
                    var car = _db.Cars.FirstOrDefault(c => c.CarID == carclassification.CarID);
                    var carPN = car.CarPN;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Delete, Utilities.Action.DeleteCarClassification, currentuserId, Utilities.MasterEntity.CarClassification, "Delete Car Classification" + carPN);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }

        }
        public void CreateCarMaintenance(DateTime carmaintenancedate, string carmaintenancedetails, bool isdeleted, int carid)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var carmaintenance = new CarMaintenance
                    {
                        CarMaintenanceDate = carmaintenancedate,
                        CarMaintenanceDetails = carmaintenancedetails,
                        IsDeleted = isdeleted,
                        CarID = carid
                    };
                    var car = _db.Cars.FirstOrDefault(c => c.CarID == carid);
                    var carPN = car.CarPN;
                    _db.CarMaintenances.Add(carmaintenance);
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Add, Utilities.Action.CreateCarMaintenances, currentuserId, Utilities.MasterEntity.CarMaintenances, "Create Car Maintenance" + carPN);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void EditCarMaintenance(DateTime carmaintenancedate, string carmaintenancedetails, bool isdeleted, int carid, int carmaintenanceid)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var carmaintenance = _db.CarMaintenances.Find(carmaintenanceid);

                    carmaintenance.CarMaintenanceDate = carmaintenancedate;
                    carmaintenance.CarMaintenanceDetails = carmaintenancedetails;
                    carmaintenance.IsDeleted = isdeleted;
                    carmaintenance.CarID = carid;
                    var car = _db.Cars.FirstOrDefault(c => c.CarID == carid);
                    var carPN = car.CarPN;
                    _db.Entry(carmaintenance).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Edit, Utilities.Action.EditCarMaintenances, currentuserId, Utilities.MasterEntity.CarMaintenances, "Edit Car Maintenance" + carPN);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void DeleteCarMaintenance(int carmaintenanceId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    CarMaintenance carmaintenance = _db.CarMaintenances.Find(carmaintenanceId);
                    carmaintenance.IsDeleted = true;
                    _db.Entry(carmaintenance).State = EntityState.Modified;
                    var car = _db.Cars.FirstOrDefault(c => c.CarID == carmaintenance.CarID);
                    var carPN = car.CarPN;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Delete, Utilities.Action.DeleteCarMaintenances, currentuserId, Utilities.MasterEntity.CarMaintenances, "Delete Car Maintenance" + carPN);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void CreateCar(string carcolor, string carbrand, string carmodel, int carversion, string cartype, int carcapacity, string carpn, DateTime carled, bool carrented, int carkmdashboard, string carphoto, bool isavilable, bool isdeleted) 
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (_db.Cars.Any(c => c.CarPN == carpn))
                    {
                        throw new Exception("The car plate number already exists.");
                    }
                    var car = new Car
                    {
                        CarColor = carcolor,
                        CarBrand = carbrand,
                        CarModel = carmodel,
                        CarVersion = carversion,
                        CarType = cartype,
                        CarCapacity = carcapacity,
                        CarPN = carpn,
                        CarLED = carled,
                        CarRented = carrented,
                        CarKMDashboard = carkmdashboard,
                        CarPhoto = carphoto,
                        CarIsAvailable = isavilable,
                        IsDeleted = isdeleted
                    };
                    if (car.IsLicenseExpired())
                    {
                        throw new Exception("The Car's license has expired.");
                    }
                    _db.Cars.Add(car);
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Add, Utilities.Action.CreateCar, currentuserId, Utilities.MasterEntity.Car, "Create Car " + car.CarPN);

                    scope.Complete();
                }
               
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void EditCar(string carcolor, string carbrand, string carmodel, int carversion, string cartype, int carcapacity, string carpn, DateTime carled, bool carrented, int carkmdashboard, string carphoto, bool isavilable, bool isdeleted , int carid) 
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (_db.Cars.Any(c => c.CarPN == carpn))
                    {
                        throw new Exception("The car plate number already exists.");
                    }

                    var car = _db.Cars.Find(carid);

                    car.CarColor = carcolor;
                    car.CarBrand = carbrand;
                    car.CarModel = carmodel;
                    car.CarVersion = carversion;
                    car.CarType = cartype;
                    car.CarCapacity = carcapacity;
                    car.CarPN = carpn;
                    car.CarLED = carled;
                    car.CarRented = carrented;
                    car.CarKMDashboard = carkmdashboard;
                    car.CarPhoto = carphoto;
                    car.CarIsAvailable = isavilable;
                    car.IsDeleted = isdeleted;
                    if (car.IsLicenseExpired())
                    {
                        throw new Exception("The Car's license has expired.");
                    }
                    _db.Entry(car).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Edit, Utilities.Action.EditCar, currentuserId, Utilities.MasterEntity.Car, "Edit Car " + car.CarPN);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void DeleteCar(int carrId) 
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Car car = _db.Cars.Find(carrId);
                    car.IsDeleted = true;
                    _db.Entry(car).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Delete, Utilities.Action.DeleteCar, currentuserId, Utilities.MasterEntity.Car, "Delete Car" + car.CarPN);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void CreateDriver(string driverfname, string driverlname, string driverphone, long driverssn, float driversalary, DateTime driverdob, string driveraddress, string driverphoto, string drivercity, DateTime driverled, bool isdeleted)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (_db.Drivers.Any(c => c.DriverPhone == driverphone))
                    {
                        throw new Exception("The Driver Phone Number already exists.");
                    }
                    if (_db.Drivers.Any(c => c.DriverSSN == driverssn))
                    {
                        throw new Exception("The Driver SSN Number already exists.");
                    }
                    var driver = new Driver
                    {
                        DriverFName = driverfname,
                        DriverLName = driverlname,
                        DriverPhone = driverphone,
                        DriverSSN = driverssn,
                        DriverSalary = driversalary,
                        DriverDOB = driverdob,
                        DriverAddress = driveraddress,
                        DriverPhoto = driverphoto,
                        DriverCity = drivercity,
                        DriverLED = driverled,
                        IsDeleted = isdeleted
                    };
                    if (driver.IsLicenseExpired())
                    {
                        throw new Exception("The driver's license has expired.");
                    }
                    _db.Drivers.Add(driver);
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Add, Utilities.Action.CreateDriver, currentuserId, Utilities.MasterEntity.Driver, "Create Driver " + driver.DriverFName+" "+driver.DriverLName);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void EditDriver(string driverfname, string driverlname, string driverphone, long driverssn, float driversalary, DateTime driverdob, string driveraddress, string driverphoto, string drivercity, DateTime driverled, DateTime? driverlasttripdt, bool isdeleted,int driverid)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (_db.Drivers.Any(c => c.DriverPhone == driverphone))
                    {
                        throw new Exception("The Driver Phone Number already exists.");
                    }
                    if (_db.Drivers.Any(c => c.DriverSSN == driverssn))
                    {
                        throw new Exception("The Driver SSN Number already exists.");
                    }

                    var driver = _db.Drivers.Find(driverid);

                    driver.DriverFName = driverfname;
                    driver.DriverLName = driverlname;
                    driver.DriverPhone = driverphone;
                    driver.DriverSSN = driverssn;
                    driver.DriverSalary = driversalary;
                    driver.DriverDOB = driverdob;
                    driver.DriverAddress = driveraddress;
                    driver.DriverPhoto = driverphoto;
                    driver.DriverCity = drivercity;
                    driver.DriverLED = driverled;
                    driver.DriverLastTripDT = driverlasttripdt; 
                    driver.IsDeleted = isdeleted;
                    if (driver.IsLicenseExpired())
                    {
                        throw new Exception("The driver's license has expired.");
                    }
                    _db.Entry(driver).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Edit, Utilities.Action.EditDriver, currentuserId, Utilities.MasterEntity.Driver, "Edit Driver " +driver.DriverFName + " " + driver.DriverLName);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void DeleteDriver(int driverId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Driver driver = _db.Drivers.Find(driverId);
                    driver.IsDeleted = true;
                    _db.Entry(driver).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Delete, Utilities.Action.DeleteDriver, currentuserId, Utilities.MasterEntity.Driver, "Delete Driver " +driver.DriverFName + " " + driver.DriverLName);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void CreatePrice(float pricemoney, bool isdeleted, int routeid, int carclassificationid)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var price = new Price
                    {
                        PriceMoney = pricemoney,
                        IsDeleted = isdeleted,
                        RouteID = routeid,
                        CarClassificationID = carclassificationid
                    };
                    var route = _db.Routes.FirstOrDefault(c => c.RouteID == routeid);
                    var routeTo = route.RouteTo;
                    _db.Prices.Add(price);
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Add, Utilities.Action.CreatePrice, currentuserId, Utilities.MasterEntity.Price, "Create Price " + routeTo);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void EditPrice(float pricemoney, bool isdeleted, int routeid, int carclassificationid, int priceid)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var price = _db.Prices.Find(priceid);

                    price.PriceMoney = pricemoney;
                    price.IsDeleted = isdeleted;
                    price.RouteID = routeid;
                    price.CarClassificationID = carclassificationid;
                    var route = _db.Routes.FirstOrDefault(c => c.RouteID == routeid);
                    var routeTo = route.RouteTo;
                    _db.Entry(price).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Edit, Utilities.Action.EditPrice, currentuserId, Utilities.MasterEntity.Price, "Edit Price " + routeTo);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void DeletePrice(int priceId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Price price = _db.Prices.Find(priceId);
                    price.IsDeleted = true;
                    _db.Entry(price).State = EntityState.Modified;
                    var route = _db.Routes.FirstOrDefault(c => c.RouteID == price.RouteID);
                    var routeTo = route.RouteTo;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Delete, Utilities.Action.DeletePrice, currentuserId, Utilities.MasterEntity.Price, "Delete Price" + routeTo);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void CreateRoute(string routeto, string routefrom, bool isdeleted)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var route = new Route
                    {
                        RouteTo = routeto,
                        RouteFrom = routefrom,
                        IsDeleted = isdeleted
                    };
                    _db.Routes.Add(route);
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Add, Utilities.Action.CreateRoute, currentuserId, Utilities.MasterEntity.Route, "Create Route " + route.RouteTo);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void EditRoute(string routeto, string routefrom, bool isdeleted,int routeid) 
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var route = _db.Routes.Find(routeid);
                 
                    route.RouteTo = routeto;
                    route.RouteFrom = routefrom;
                    route.IsDeleted = isdeleted;

                    _db.Entry(route).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Edit, Utilities.Action.EditRoute, currentuserId, Utilities.MasterEntity.Route, "Edit Route " + route.RouteTo);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void DeleteRoute(int routeId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Route route = _db.Routes.Find(routeId);
                    route.IsDeleted = true;
                    _db.Entry(route).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Delete, Utilities.Action.DeleteRoute, currentuserId, Utilities.MasterEntity.Route, "Delete Route " + route.RouteTo);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void CreateTrip(DateTime tripbookingdt, DateTime trippredictedtraveldt, int trippassengers, string tripstatus, bool isdeleted, int driverid, int carid, int routeid, int customerid)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var driver = _db.Drivers.Find(driverid);
                    if (driver == null)
                    {
                        throw new Exception("Driver not found.");
                    }
                    if (driver.IsLicenseExpired())
                    {
                        throw new Exception("The Driver's License has expired.");
                    }
                    var car = _db.Cars.Find(carid);
                    if (car == null)
                    {
                        throw new Exception("Car not found.");
                    }
                    if (car.IsLicenseExpired())
                    {
                        throw new Exception("The Car's license has expired.");
                    }
                    var trip = new Trip
                    {
                        TripBookingDT = tripbookingdt,
                        TripPredictedTravelDT = trippredictedtraveldt,
                        TripPassengers = trippassengers,
                        TripStatus = tripstatus,
                        IsDeleted = isdeleted,
                        DriverID = driverid,
                        CarID = carid,
                        RouteID = routeid,
                        CustomerID = customerid
                    };
                    _db.Trips.Add(trip);
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Add, Utilities.Action.CreateTrip, currentuserId, Utilities.MasterEntity.Trip, "Create Trip " + trip.TripBookingDT);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void EditTrip(DateTime tripbookingdt, DateTime? triprealtraveldt, DateTime trippredictedtraveldt, DateTime? triparrivaldt, int? tripstartkm, int? tripendkm, int trippassengers,string tripstatus, int? driverrating, int? carrating, string comments, bool isdeleted, int driverid, int carid, int routeid, int customerid,int tripid)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var driver = _db.Drivers.Find(driverid);
                    if (driver == null)
                    {
                        throw new Exception("Driver not found.");
                    }
                    if (driver.IsLicenseExpired())
                    {
                        throw new Exception("The Driver's License has expired.");
                    }
                    var car = _db.Cars.Find(carid);
                    if (car == null)
                    {
                        throw new Exception("Car not found.");
                    }
                    if (car.IsLicenseExpired())
                    {
                        throw new Exception("The Car's license has expired.");
                    }

                    var trip = _db.Trips.Find(tripid);

                    trip.TripBookingDT = tripbookingdt;
                    trip.TripRealTravelDT = triprealtraveldt;
                    trip.TripPredictedTravelDT = trippredictedtraveldt;
                    trip.TripArrivalDT = triparrivaldt ;
                    trip.TripStartKM = tripstartkm ; 
                    trip.TripEndKM = tripendkm;
                    trip.TripPassengers = trippassengers;
                    trip.TripStatus = tripstatus;
                    trip.TripDriverRating = driverrating;
                    trip.TripCarRating = carrating;
                    trip.TripComments = comments;
                    trip.IsDeleted = isdeleted;
                    trip.DriverID = driverid;
                    trip.CarID = carid;
                    trip.RouteID = routeid;
                    trip.CustomerID = customerid;

                    _db.Entry(trip).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Edit, Utilities.Action.EditTrip, currentuserId, Utilities.MasterEntity.Trip, "Edit Trip " + trip.TripBookingDT);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void DeleteTrip(int tripId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Trip trip = _db.Trips.Find(tripId);
                    trip.IsDeleted = true;
                    _db.Entry(trip).State = EntityState.Modified;
                    _db.SaveChanges();
                    CreateAudit(Utilities.ActionType.Delete, Utilities.Action.DeleteTrip, currentuserId, Utilities.MasterEntity.Trip, "Delete Trip " + trip.TripBookingDT);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }
        public void StartTrip(int tripId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var trip = _db.Trips.Find(tripId);
                    if (trip == null)
                    {
                        throw new Exception("Trip not found");
                    }
                    trip.TripStatus = "Pending";

                    var car = _db.Cars.Find(trip.CarID);
                    if (car == null)
                    {
                        throw new Exception("Car not found");
                    }
                    car.CarIsAvailable = false;

                    _db.Entry(trip).State = EntityState.Modified;
                    _db.Entry(car).State = EntityState.Modified;
                    _db.SaveChanges();

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw; 
                }
            }
        }
        public void EndTrip(int tripId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var trip = _db.Trips.Find(tripId);
                    if (trip == null)
                    {
                        throw new Exception("Trip not found");
                    }
                    trip.TripStatus = "Completed";

                    var car = _db.Cars.Find(trip.CarID);
                    if (car == null)
                    {
                        throw new Exception("Car not found");
                    }
                    car.CarIsAvailable = true;

                    var driver = _db.Drivers.Find(trip.DriverID);
                    if (driver == null)
                    {
                        throw new Exception("Driver not found");
                    }
                    driver.DriverLastTripDT = DateTime.Now;

                    _db.Entry(trip).State = EntityState.Modified;
                    _db.Entry(car).State = EntityState.Modified;
                    _db.Entry(driver).State = EntityState.Modified;

                    _db.SaveChanges();

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }
            }
        }
    }
}
