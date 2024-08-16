//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WaslnyLib.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class CarsEvaluation
    {
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public int CarVersion { get; set; }
        public string CarPN { get; set; }
        public bool CarClassificationQuality { get; set; }
        public string CarClassificationType { get; set; }
        public string RouteTo { get; set; }
        public System.DateTime TripPredictedTravelDT { get; set; }
        public Nullable<int> TripCarRating { get; set; }
        public string TripComments { get; set; }
        public string CarType { get; set; }
        public int CarCapacity { get; set; }
        public string CarColor { get; set; }
        public int CarID { get; set; }
        public System.DateTime CarLED { get; set; }
        public bool CarRented { get; set; }
        public int CarKMDashboard { get; set; }
        public string CarPhoto { get; set; }
        public bool CarIsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public int CarClassificationID { get; set; }
        public bool Expr1 { get; set; }
        public bool Expr2 { get; set; }
        public int RouteID { get; set; }
        public int TripID { get; set; }
        public System.DateTime TripBookingDT { get; set; }
        public Nullable<System.DateTime> TripRealTravelDT { get; set; }
        public Nullable<System.DateTime> TripArrivalDT { get; set; }
        public Nullable<int> TripStartKM { get; set; }
        public Nullable<int> TripEndKM { get; set; }
        public int TripPassengers { get; set; }
        public string TripStatus { get; set; }
        public Nullable<int> TripDriverRating { get; set; }
        public bool Expr3 { get; set; }
    }
}