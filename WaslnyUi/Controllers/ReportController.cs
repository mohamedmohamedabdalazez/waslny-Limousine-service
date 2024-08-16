using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaslnyLib.Entity;

namespace WaslnyUi.Controllers
{
    public class ReportController : Controller
    {
        private readonly WaslnyLibDbContext _db;

        public ReportController()
        {
            _db = new WaslnyLibDbContext();
        }
        //GET: Report
        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public FileContentResult report(string path, string dataSetName, object reportdata, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(path);

            ReportDataSource reportDataSource = new ReportDataSource(dataSetName, reportdata);
            localReport.DataSources.Add(reportDataSource);

            string reportType = format;
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
             "  <OutputFormat>jpeg</OutputFormat>" +
             "  <PageWidth>8.5in</PageWidth>" +
             "  <PageHeight>11in</PageHeight>" +
             "  <MarginTop>0.5in</MarginTop>" +
             "  <MarginLeft>1in</MarginLeft>" +
             "  <MarginRight>1in</MarginRight>" +
             "  <MarginBottom>0.5in</MarginBottom>" +
             "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            return File(renderedBytes, mimeType);
        }



        //PDF
        public ActionResult auditsreportPDF()
        {
            var ReportData = _db.AuditDetailsViews.ToList();
            return report("~/Reports/AuditDetailsReport.rdlc", "DataSet1", ReportData, "PDF");
        }

        public ActionResult driversreportPDF()
        {
            var ReportData = _db.DriversEvaluations.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/DriversEvaluationReport.rdlc", "DriverDataSet", ReportData, "PDF");
        }

        public ActionResult carsreportPDF()
        {
            var ReportData = _db.CarsEvaluations.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/CarsEvaluationReport.rdlc", "CarDataSet", ReportData, "PDF");
        }

        public ActionResult tripsreportPDF()
        {
            var ReportData = _db.TripDetails.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/TripDetailsReport.rdlc", "TripDataSet", ReportData, "PDF");
        }


        //Word
        public ActionResult auditsreportWord()
        {

            var ReportData = _db.AuditDetailsViews.ToList();
            return report("~/Reports/AuditDetailsReport.rdlc", "DataSet1", ReportData, "Word");
        }

        public ActionResult driversreportWord()
        {

            var ReportData = _db.DriversEvaluations.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/DriversEvaluationReport.rdlc", "DriverDataSet", ReportData, "Word");
        }

        public ActionResult carsreportWord()
        {

            var ReportData = _db.CarsEvaluations.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/CarsEvaluationReport.rdlc", "CarDataSet", ReportData, "Word");
        }

        public ActionResult tripsreportWord()
        {

            var ReportData = _db.TripDetails.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/TripDetailsReport.rdlc", "TripDataSet", ReportData, "Word");
        }


        //Excel
        public ActionResult auditsreportExcel()
        {

            var ReportData = _db.AuditDetailsViews.ToList();
            return report("~/Reports/AuditDetailsReport.rdlc", "DataSet1", ReportData, "Excel");
        }

        public ActionResult driversreportExcel()
        {

            var ReportData = _db.DriversEvaluations.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/DriversEvaluationReport.rdlc", "DriverDataSet", ReportData, "Excel");
        }

        public ActionResult carsreportExcel()
        {

            var ReportData = _db.CarsEvaluations.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/CarsEvaluationReport.rdlc", "CarDataSet", ReportData, "Excel");
        }

        public ActionResult tripsreportExcel()
        {

            var ReportData = _db.TripDetails.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/TripDetailsReport.rdlc", "TripDataSet", ReportData, "Excel");
        }



        //Image
        public ActionResult auditsreportImage()
        {

            var ReportData = _db.AuditDetailsViews.ToList();
            return report("~/Reports/AuditDetailsReport.rdlc", "DataSet1", ReportData, "Image");
        }

        public ActionResult driversreportImage()
        {

            var ReportData = _db.DriversEvaluations.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/DriversEvaluationReport.rdlc", "DriverDataSet", ReportData, "Image");
        }

        public ActionResult carsreportImage()
        {

            var ReportData = _db.CarsEvaluations.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/CarsEvaluationReport.rdlc", "CarDataSet", ReportData, "Image");
        }

        public ActionResult tripsreportImage()
        {

            var ReportData = _db.TripDetails.Where(a => !a.IsDeleted).ToList();
            return report("~/Reports/TripDetailsReport.rdlc", "TripDataSet", ReportData, "Image");
        }

    }
}