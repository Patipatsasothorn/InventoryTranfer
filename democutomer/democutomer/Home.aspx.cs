using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace democutomer
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ดึงค่า StartTime และ EndTime จาก Web.config
            string startTimeValue = ConfigurationManager.AppSettings["StartTime"];
            string endTimeValue = ConfigurationManager.AppSettings["EndTime"];

            // แปลงค่าเป็น DateTime โดยรวมวันที่ปัจจุบัน
            DateTime startTime = DateTime.Today.Add(TimeSpan.Parse(startTimeValue));
            DateTime endTime = DateTime.Today.Add(TimeSpan.Parse(endTimeValue));
            DateTime currentTime = DateTime.Now;

            //// ตรวจสอบว่าเวลาปัจจุบันอยู่ในช่วงหรือไม่
            //if (currentTime < startTime || currentTime > endTime)
            //{
            //    // ตรวจสอบว่าเวลาปัจจุบันอยู่ในช่วงหรือไม่
            //    if (currentTime < startTime || currentTime > endTime)
            //    {
            //        // ปิดใช้งาน TextBox ทั้ง 3
            //        TextBox10.Enabled = false;
            //        TextBox1.Enabled = false;
            //        TextBox6.Enabled = false;
            //        return; // หยุดการทำงานในกรณีที่เป็นเวลาไม่อนุญาต
            //    }
            //    return; // หยุดการทำงานในกรณีที่เป็นเวลาไม่อนุญาต
            //}
            // รับค่า token, company, และ site จาก Query String
            string token = Request.QueryString["token"];
            string company = Request.QueryString["company"];
            string site = Request.QueryString["site"];

            // ตรวจสอบว่ามีค่า token, company, และ site หรือไม่
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(site))
            {
                // ทำการเก็บค่า token, company, และ site ลงใน session หรือใน ViewState ตามที่เหมาะสม
                // เช่น Session["Token"] = token; Session["Company"] = company; Session["Site"] = site;
                // หรือ ViewState["Token"] = token; ViewState["Company"] = company; ViewState["Site"] = site;

                // ทำการส่งต่อไปยังหน้าที่ต้องการ ในที่นี้เราจะส่งไปยังหน้า default.aspx โดยใช้ Response.Redirect
                //Live
                Response.Redirect("https://webapp.bpi-concretepile.co.th:9013/default.aspx?token=" + token + "&company=" + company + "&site=" + site);
                //UAT2
                //Response.Redirect("https://webapp.bpi-concretepile.co.th:9024/default.aspx?token=" + token + "&company=" + company + "&site=" + site);

            }
            else
            {
                Response.Redirect("https://webapp.bpi-concretepile.co.th:8080/#/authen");
                // หากไม่มีค่า token, company, หรือ site ให้ทำการแสดงข้อความผิดพลาดหรือกระทำตามที่ต้องการ
                // เช่นแสดงข้อความแจ้งเตือนหรือ redirect ไปยังหน้าที่เหมาะสม

            }

        }
    }
}