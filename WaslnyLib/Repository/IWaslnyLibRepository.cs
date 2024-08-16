using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslnyLib.Entity;

namespace WaslnyLib.Repository
{
    public interface IWaslnyLibRepository
    {
        void CreateAudit(Utilities.ActionType actiontype, Utilities.Action action, int userId, Utilities.MasterEntity entity, String entityrecord);
        void CreateCustomer(string firstname,string lastname,bool gender,string address,string phone, string city, string email, bool isdeleted , DateTime customerdob);
        void EditCustomer(string firstname, string lastname, bool gender, string address, string phone, string city, string email, bool isdeleted,int customerid , DateTime customerdob); 
        void DeleteCustomer(int customerId);
        void CreateCarClassification(bool carclassificationquality, string carclassificationtype, bool isdeleted, int carid);
        void EditCarClassification(bool carclassificationquality, string carclassificationtype, bool ideleted, int carid , int carclassifiactionid);
        void DeleteCarClassification(int carclassificationId);
        void CreateCarMaintenance(DateTime carmaintenancedate, string carmaintenancedetails, bool isdeleted , int carid);
        void EditCarMaintenance(DateTime carmaintenancedate, string carmaintenancedetails, bool isdeleted, int carid,int carmaintenanceid);
        void DeleteCarMaintenance(int carmaintenanceId);
        void CreateCar(string carcolor, string carbrand, string carmodel, int carversion, string cartype, int carcapacity, string carpn, DateTime carled, bool carrented, int carkmdashboard, string carphoto,bool isavilable, bool isdeleted);
        void EditCar(string carcolor, string carbrand, string carmodel, int carversion, string cartype, int carcapacity, string carpn, DateTime carled, bool carrented, int carkmdashboard, string carphoto, bool isavilable, bool isdeleted,int carid);
        void DeleteCar(int carrId);
        void CreateDriver(string driverfname, string driverlname, string driverphone, long driverssn, float driversalary, DateTime driverdob, string driveraddress, string driverphoto, string drivercity, DateTime driverled, bool isdeleted);
        void EditDriver(string driverfname, string driverlname, string driverphone, long driverssn, float driversalary, DateTime driverdob, string driveraddress, string driverphoto, string drivercity, DateTime driverled, DateTime? driverlasttripdt, bool isdeleted,int driverid);
        void DeleteDriver(int driverId);
        void CreatePrice(float pricemoney,bool isdeleted, int routeid, int carclassificationid);
        void EditPrice(float pricemoney,bool isdeleted , int routeid, int carclassificationid , int priceid);
        void DeletePrice(int priceId);
        void CreateRoute(string routeto, string routefrom, bool isdeleted);
        void EditRoute(string routeto, string routefrom, bool isdeleted, int routeid);
        void DeleteRoute(int routeId);
        void CreateTrip(DateTime tripbookingdt, DateTime trippredictedtraveldt, int trippassengers, string tripstatus, bool isdeleted, int driverid, int carid, int routeid, int customerid);
        void EditTrip(DateTime tripbookingdt, DateTime? triprealtraveldt, DateTime trippredictedtraveldt, DateTime? triparrivaldt, int? tripstartkm, int? tripendkm, int trippassengers, string tripstatus, int? driverrating, int? carrating, string comments, bool isdeleted, int driverid, int carid, int routeid, int customerid, int tripid);
        void DeleteTrip(int tripId);
        void StartTrip(int tripId);
        void EndTrip(int tripId);

    }
}
