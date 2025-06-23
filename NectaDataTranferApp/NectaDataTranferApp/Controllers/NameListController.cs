
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NectaDataTranferApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NameListController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public NameListController(IWebHostEnvironment _webHostEnvironment)
        {
            this.webHostEnvironment = _webHostEnvironment;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
        }
        [HttpGet]
        public IActionResult Get()
        {

            try
            {
                string mintype = "";
                int extension = 1;
                var path = $"{this.webHostEnvironment.WebRootPath}\\NameList.rdlc";
                Dictionary<string, string> paremetre = new Dictionary<string, string>();
                paremetre.Add("prm1", "Names List");
                //LocalReport localreport = new LocalReport(path);
                //localreport.AddDataSource("NameDataSet", ApiGetNames());
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                //var results = localreport.Execute(RenderType.Pdf,extension,paremetre,mintype);

                // return File(results.MainStream, "application/pdf");
                // return File(results.MainStream, "application/pdf");
               return new JsonResult(paremetre);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }


        public DataTable ApiGetNames()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ExamNumber");
            dt.Columns.Add("ExamYear");
            dt.Columns.Add("ExamName");
            dt.Columns.Add("Fname");
            dt.Columns.Add("Oname");
            dt.Columns.Add("Sname");
            dt.Columns.Add("SifaTable");
            dt.Columns.Add("EmisDB");
            DataRow dr;
            for (int i = 0; i <= 20; i++)
            {
                dr = dt.NewRow();
                dr["ExamNumber"]="number" + i.ToString();
                dr["ExamYear"] =i;
                dr["ExamName"] ="number" + i.ToString();
                dr["Fname"] ="number" + i.ToString();
                dr["Oname"] ="number" + i.ToString();
                dr["Sname"] ="number" + i.ToString();
                dr["SifaTable"] = "number" + i.ToString();
                dr["EmisDB"] = "number" + i.ToString();
                dt.Rows.Add(dr);

            }

            return dt;
        }
    }
}
