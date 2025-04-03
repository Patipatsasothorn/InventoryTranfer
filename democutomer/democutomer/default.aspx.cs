using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Globalization;
using System.Net;
using System.Web.Services;
using System.Configuration;

namespace democutomer
{
    public partial class main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            // รับค่า token, company, และ site จาก Query String
            string token = Request.QueryString["token"];
            string company = Request.QueryString["company"];
            string site = Request.QueryString["site"];
            Company.Text = company;

            // ตรวจสอบว่ามีค่า token, company, และ site หรือไม่
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(company) || string.IsNullOrEmpty(site))
            {
                // หากไม่มีค่า token, company, หรือ site ให้ทำการแสดงข้อความผิดพลาดหรือกระทำตามที่ต้องการ
                Response.Redirect("https://webapp.bpi-concretepile.co.th:8080/#/authen");
                return;
            }

            // ลบคำว่า "Bearer " ออกจาก Token
            var tokenString = token.Replace("Bearer ", "");
            var jwtHandler = new JwtSecurityTokenHandler();

            // อ่านและถอดรหัส Token
            var tokenDecoded = jwtHandler.ReadJwtToken(tokenString);

            // ดึงค่าของ Claim "username" ออกมา
            string empName = tokenDecoded.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value;

            //  ตรวจสอบว่ามีค่า "username" หรือไม่
            if (!string.IsNullOrEmpty(empName))
            {
                // ทำสิ่งที่คุณต้องการกับค่า empName ที่ได้
                Label4.Text = empName;
            }

            if (!IsPostBack)
            {
                BindPopupGridView();
                // Create a dummy DataTable with only headers
               
                // ตรวจสอบว่าตัวแปร ViewState มีค่าหรือไม่
                if (ViewState["Company"] == null)
                {
                    // ถ้าไม่มีค่าให้ดึงข้อมูลบริษัทจากฐานข้อมูลและเก็บไว้ใน ViewState
                    DataTable companyDataTable = GetCompanyDataFromDatabase();
                    ViewState["Company"] = companyDataTable;
                }

                // เซ็ตค่าให้ DropDownList1 ด้วยข้อมูลบริษัทที่ได้จาก ViewState
                DataTable dt = (DataTable)ViewState["Company"];
                // เพิ่มรายการแรกใน DropDownList1 เพื่อให้ผู้ใช้สามารถเลือกบริษัท
                TextBox5.Text = DateTime.Now.ToString("dd/MM/yyyy");


            }




        }


        private DataTable GetCompanyDataFromDatabase()
        {
            DataTable companyDataTable = new DataTable();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            string query = "SELECT distinct Company FROM ice.ud28"; // แทนที่ YourCompanyTable ด้วยชื่อตารางของคุณ

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(companyDataTable);
                    }
                }
            }

            return companyDataTable;
        }

        protected void TextBox10_TextChanged(object sender, EventArgs e)
        {
            TextBox1.Enabled = true; // ปิดการใช้งาน TextBox1
            TextBox6.Enabled = true;
            // เรียก Swal.fire เพื่อแสดง Loading เป็นเวลา 5 วินาที
            //ClientScript.RegisterStartupScript(this.GetType(), "showLoading", @"
            //    Swal.fire({
            //        title: 'กำลังโหลด...',
            //        text: 'กรุณารอสักครู่',
            //        allowOutsideClick: false,
            //        timer: 1500, // เวลาแสดง (5 วินาที = 5000 มิลลิวินาที)
            //        didOpen: () => {
            //            Swal.showLoading();
            //        },
            //        willClose: () => {
            //            console.log('Loading ปิดแล้ว');
            //        }
            //    });", true);

            if (TextBox10.Text != null)
            {

                Button5.Visible = true;

            }
            else {

                Button5.Visible = false;
            }

            if (ViewState["GridData"] != null)
            {
                ClearData();

            }
            // นำโค้ดที่ต้องการให้ทำงานเมื่อมีการเปลี่ยนแปลงใน TextBox10 มาวางที่นี่
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            string queryBinwh = @"
     SELECT 
    ud28.Company,
    ud28.Key1,
    ud28.Key2,
    ud28.Key3,
    ud28.Key4,
    ud29.Key5,
	s.Name,
    UD30.Character03  AS WH,
    UD30.Character04  AS Bin,
    ud28.ShortChar02,
    ud28.ShortChar04,
    ud28.Number04,
    ud28.Number09,
    ud28.Number11,
    ud28.Character09,
    ud28.ShortChar08,
    ud28.Character01,
    ud28.Character02,
    ud28.Character05,
    ud28.Character06,
    ud28.Date01,
    ud28.ShortChar01,
    ud28.ShortChar03,
    ud28.ShortChar05,
    ud28.ShortChar07,
    cust.CustID,
    ud100.Character01 AS UD100_Character01,
    ud28.Character04,
    ud28.ShortChar20,
    shipvia.Description,
    project.WarehouseCode,
    project.BinNum,
    ud28.ShortChar11,
    Project.Description AS ProjectDescription,
    p.PartDescription,
    p.IUM,
    ud28.Character03,  -- เพิ่ม Character03
    ud28.Character04, -- เพิ่ม Character04
    ud28.ShortChar16

FROM 
    Ice.UD28 ud28
LEFT JOIN (
    SELECT 
        Erp.Customer.CustID,
        Erp.Customer.Company,
        Erp.ShipTo.ShipToNum
    FROM 
        Erp.Customer
    INNER JOIN 
        Erp.ShipTo ON Erp.Customer.Company = Erp.ShipTo.Company 
                    AND Erp.Customer.CustNum = Erp.ShipTo.CustNum
    WHERE 
        Erp.ShipTo.ShipToNum = 'W0075003' 
        AND Erp.ShipTo.Company = @Company
) cust ON ud28.Company = cust.Company
LEFT JOIN Ice.UD100 ud100 ON ud28.Key5 = ud100.Key1 and ud28.Company = ud100.Company
LEFT JOIN BPI_Live.Erp.ShipVia shipvia ON ud28.ShortChar07 = shipvia.ShipViaCode  and ud28.Company =shipvia.Company
                                     AND shipvia.Company = @Company
LEFT JOIN Project project ON project.ProjectID = ud28.Character02 AND project.Company = ud28.Company
  LEFT JOIN ice.UD29 ud29 ON ud29.Date01 = ud28.Date01 AND ud29.Company = ud28.Company and ud28.ShortChar08 = ud29.Key5 and ud28.Key2 = UD29.Key2
  	LEFT join ShipTo s on s.Company = ud28.Company and s.ShipToNum = ud28.ShortChar05 
	Left Join ice.UD30 ON     UD30.Key1 = UD28.Key1 and UD30.Company = UD28.Company and UD30.Key5 = ud28.ShortChar08

LEFT JOIN ERP.Part p ON p.PartNum = ud28.ShortChar08 AND p.Company = ud28.Company
WHERE 
    ud28.Key1 = @Key1Value
ORDER BY 
    ud28.Company, 
    cust.ShipToNum
";
            string query2 = "SELECT s.Name,* FROM Ice.UD28 ud join ShipTo s on s.Company = ud.Company and s.ShipToNum = ud.ShortChar05  WHERE ud.Key1 = @Key1Value AND ud.Company = @CompanyValue  ";
            string query = "SELECT SUM(Number04) AS TotalNumber01 FROM Ice.UD28 WHERE Key1 = @Key1Value  AND Company = @CompanyValue ";
            string queryDATASAVE = "Select * From UD15 where BPI_PickingList_c = @Key1Value and Key5 = 'Transfer'";


            if (!string.IsNullOrEmpty(TextBox10.Text))
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(queryDATASAVE, connection))
                    {
                        command.Parameters.AddWithValue("@Key1Value", TextBox10.Text);

                        DataTable dtGridData = null;
                        DataTable dtGridData3 = null;
                        bool checkBox01 = false;
                        bool checkBox02 = false;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Button2.Visible = false;

                                if (ViewState["GridData"] != null)
                                {
                                    dtGridData = (DataTable)ViewState["GridData"];

                                }
                                else
                                {
                                    dtGridData = new DataTable();
                                    dtGridData.Columns.Add("LOT", typeof(string));
                                    dtGridData.Columns.Add("Qty", typeof(int));
                                    dtGridData.Columns.Add("PartNumber", typeof(string));
                                    dtGridData.Columns.Add("Bin", typeof(string));
                                    dtGridData.Columns.Add("Warehouse", typeof(string));
                                    dtGridData.Columns.Add("Valuescan", typeof(string));
                                    dtGridData.Columns.Add("CheckBox02Value", typeof(bool));
                                    dtGridData.Columns.Add("Description", typeof(string));

                                }

                                if (ViewState["GridData3"] != null)
                                {
                                    dtGridData3 = (DataTable)ViewState["GridData3"];
                                }
                                else
                                {
                                    dtGridData3 = new DataTable();
                                    dtGridData3.Columns.Add("No", typeof(string));
                                    dtGridData3.Columns.Add("TextColumn", typeof(string));
                                    dtGridData3.Columns.Add("PartNumber", typeof(string));
                                    dtGridData3.Columns.Add("LOT", typeof(string));
                                    dtGridData3.Columns.Add("WH", typeof(string));
                                    dtGridData3.Columns.Add("BIN", typeof(string));
                                    dtGridData3.Columns.Add("WH_C", typeof(string));
                                    dtGridData3.Columns.Add("BIN_C", typeof(string));
                                    dtGridData3.Columns.Add("Character09", typeof(string));
                                    dtGridData3.Columns.Add("UOM", typeof(string));
                                    dtGridData3.Columns.Add("Character02", typeof(string));
                                    dtGridData3.Columns.Add("ProjectDescription", typeof(string));
                                    dtGridData3.Columns.Add("ShortChar16", typeof(string));

                                }

                                while (reader.Read())
                                {
                                    TextBox5.Text = ((DateTime)reader["BPI_TranDate_c"]).ToString("dd/MM/yyyy");
                                    // ดึงค่า EnableTimeCheck จาก Web.config
                                    bool enableTimeCheck = bool.Parse(ConfigurationManager.AppSettings["EnableTimeCheck"]);

                                    if (enableTimeCheck)
                                    {
                                        // ดึงค่า StartTime และ EndTime จาก Web.config
                                        string startTimeValue = ConfigurationManager.AppSettings["StartTime"];
                                        string endTimeValue = ConfigurationManager.AppSettings["EndTime"];

                                        // แปลงค่าเป็น DateTime โดยรวมวันที่ปัจจุบัน
                                        DateTime startTime = DateTime.Today.Add(TimeSpan.Parse(startTimeValue));
                                        DateTime endTime = DateTime.Today.Add(TimeSpan.Parse(endTimeValue));
                                        DateTime currentTime = DateTime.Now;

                                        // ดึงค่าจาก TextBox10 ซึ่งคาดว่าจะมีรูปแบบวันที่เป็น "dd/MM/yyyy"
                                        DateTime dateLimit;
                                        if (DateTime.TryParseExact(TextBox5.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dateLimit))
                                        {
                                            // ตรวจสอบเงื่อนไขตามวันที่และเวลา
                                            if (DateTime.Today > dateLimit || (DateTime.Today == dateLimit && currentTime > endTime))
                                            {
                                                // ปิดใช้งาน TextBox และ Button ต่างๆ
                                                TextBox1.Text = "";
                                                TextBox6.Text = "";
                                                ENDUSE.Visible = true; // แสดงข้อความ
                                           

                                                ENDUSE.ForeColor = System.Drawing.Color.Red; // เปลี่ยนสีเป็นแดง
                                                string errorMessage = $"EndTime('ใบขึ้นของ :\\n {TextBox10.Text} \\nใช้งานได้ถึง 21:00');";

                                                // ใช้ ScriptManager เพื่อเรียกฟังก์ชัน EndTime ในฝั่ง client
                                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EndTime", errorMessage, true);
                                                TextBox10.Text = "";
                                                TextBox5.Text = DateTime.Now.ToString("dd/MM/yyyy");

                                                return;
                                            }
                                        }
                                        else
                                        {
                                            // แสดงข้อความหาก TextBox10 ไม่มีวันที่ในรูปแบบที่ถูกต้อง
                                            ENDUSE.Text = "รูปแบบวันที่ไม่ถูกต้อง";
                                            ENDUSE.Visible = true;
                                            ENDUSE.ForeColor = System.Drawing.Color.Red;
                                        }
                                    }

                                    // ดึงค่าจากฐานข้อมูล
                                    checkBox01 = Convert.ToBoolean(reader["CheckBox01"]);
                                    checkBox02 = Convert.ToBoolean(reader["CheckBox02"]);

                                    // ตรวจสอบ CheckBox01 และ CheckBox02 หากทั้งคู่เป็น true
                                    if (checkBox01 && checkBox02)
                                    {
                                        TextBox1.Enabled = false; // ปิดการใช้งาน TextBox1
                                        TextBox6.Enabled = false;
                                        Button2.Visible = false;
                                      
                                        string errorMessage = $"EndTime('ใบขึ้นของ :\\n {TextBox10.Text} \\nโอนของไปแล้ว');";

                                        // ใช้ ScriptManager เพื่อเรียกฟังก์ชัน EndTime ในฝั่ง client
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EndTime", errorMessage, true);
                                        
                                    }
                                  
                                    string partNumber = reader["BPI_PartNum_c"].ToString();
                                    string lot = reader["ShortChar10"].ToString();
                                    string warehouse = reader["BPI_WarehouseFrom_c"].ToString();
                                    string bin = reader["BPI_BinFrom_c"].ToString();
                                    string description = reader["BPI_PartDescription_c"].ToString();

                                    bool found = false;

                                    foreach (DataRow row in dtGridData.Rows)
                                    {
                                        if (row["PartNumber"].ToString() == partNumber &&
                                            row["LOT"].ToString() == lot &&
                                            row["Warehouse"].ToString() == warehouse &&
                                            row["Bin"].ToString() == bin)
                                        {
                                            int currentQty = Convert.ToInt32(row["Qty"]);
                                            row["Qty"] = currentQty + 1;
                                            found = true;
                                            break;
                                        }
                                    }

                                    if (!found)
                                    {
                                        DataRow newRow = dtGridData.NewRow();
                                        newRow["Valuescan"] = reader["Number04"].ToString().TrimEnd('0').TrimEnd('.');
                                        newRow["PartNumber"] = partNumber;
                                        newRow["Warehouse"] = warehouse;
                                        newRow["Bin"] = bin;
                                        newRow["LOT"] = lot;
                                        newRow["Qty"] = 1;
                                        newRow["CheckBox02Value"] = reader["CheckBox02"].ToString();
                                        newRow["Description"] = description;

                                        dtGridData.Rows.Add(newRow);
                                    }

                                    // Add data to dtGridData3
                                    DataRow newRowGridData3 = dtGridData3.NewRow();
                                    newRowGridData3["No"] = reader["Key3"].ToString();
                                    newRowGridData3["TextColumn"] = reader["Key2"].ToString();
                                    newRowGridData3["PartNumber"] = reader["BPI_PartNum_c"].ToString();
                                    newRowGridData3["LOT"] = reader["ShortChar10"].ToString();
                                    newRowGridData3["WH"] = reader["BPI_WarehouseFrom_c"].ToString();
                                    newRowGridData3["BIN"] = reader["BPI_BinFrom_c"].ToString();
                                    newRowGridData3["WH_C"] = reader["BPI_WareHouseTo_c"].ToString();
                                    newRowGridData3["BIN_C"] = reader["BPI_BinTo_c"].ToString();
                                    newRowGridData3["Character09"] = reader["BPI_PartDescription_c"].ToString();
                                    newRowGridData3["UOM"] = reader["BPI_UOM_c"].ToString();
                                     newRowGridData3["Character02"] = reader["BPI_ProjectID_c"].ToString();
                                    newRowGridData3["ProjectDescription"] = reader["BPI_ProjectDesc_c"].ToString();
                                    newRowGridData3["ShortChar16"] = reader["BPI_DeliveryOrder_c"].ToString();


                                    dtGridData3.Rows.Add(newRowGridData3);
                                    // ตรวจสอบค่า CheckBox01 และ CheckBox02
                                    checkBox01 = Convert.ToBoolean(reader["CheckBox01"]);
                                    checkBox02 = Convert.ToBoolean(reader["CheckBox02"]);
                                }

                                // Update ViewState and GridView1
                                ViewState["GridData"] = dtGridData;
                                GridView1.DataSource = dtGridData;
                                GridView1.DataBind();

                                // Update ViewState and GridView4
                                ViewState["GridData3"] = dtGridData3;
                                GridView4.DataSource = dtGridData3;
                                GridView4.DataBind();
                            }
                        }
                        if (checkBox01 && checkBox02)
                        {
                            Button2.Visible = false;
                            Button1.Visible = true;
                            Button4.Visible = false;

                            
                        }
                    }
                    if (ViewState["GridData"] != null)
                    {
                        DataTable dt2 = (DataTable)ViewState["GridData"];

                        // วนลูปผ่านแต่ละแถวใน DataTable
                        int totalQty = 0; // สร้างตัวแปรเพื่อเก็บผลรวมของ Qty
                        foreach (DataRow row2 in dt2.Rows)
                        {
                            // อ่านค่า Qty จากแถวและเพิ่มเข้าไปในผลรวม
                            totalQty += Convert.ToInt32(row2["Qty"]);
                        }

                        // กำหนดค่าผลรวมให้กับ TextBox14
                        TextBox14.Text = totalQty.ToString();
                    }
                    // ซ่อนปุ่ม Button2 หาก CheckBox01 และ CheckBox02 เป็น true
                 
                }



                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(queryBinwh, connection))
                    {
                        command.Parameters.AddWithValue("@Key1Value", TextBox10.Text);
                        command.Parameters.AddWithValue("@Company", Company.Text);

                        // เตรียม DataTable สำหรับเก็บข้อมูล
                        DataTable dataTable = new DataTable();

                        // ใช้ SqlDataAdapter เพื่อดึงข้อมูลจากฐานข้อมูลและเก็บลงใน DataTable
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }

                        // กำหนด DataSource ของ GridView6 เป็น DataTable ที่เตรียมไว้
                        GridView6.DataSource = dataTable;

                        // แสดงข้อมูลใน GridView6
                        GridView6.DataBind();
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@Key1Value", TextBox10.Text);
                        command.Parameters.AddWithValue("@CompanyValue", Company.Text);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime dateValue = Convert.ToDateTime(reader["Date01"]);
                                TextBox5.Text = dateValue.ToString("dd/MM/yyyy");
                                // ดึงค่า EnableTimeCheck จาก Web.config
                                bool enableTimeCheck = bool.Parse(ConfigurationManager.AppSettings["EnableTimeCheck"]);

                                if (enableTimeCheck)
                                {
                                    // ดึงค่า StartTime และ EndTime จาก Web.config
                                    string startTimeValue = ConfigurationManager.AppSettings["StartTime"];
                                    string endTimeValue = ConfigurationManager.AppSettings["EndTime"];

                                    // แปลงค่าเป็น DateTime โดยรวมวันที่ปัจจุบัน
                                    DateTime startTime = DateTime.Today.Add(TimeSpan.Parse(startTimeValue));
                                    DateTime endTime = DateTime.Today.Add(TimeSpan.Parse(endTimeValue));
                                    DateTime currentTime = DateTime.Now;

                                    // ดึงค่าจาก TextBox10 ซึ่งคาดว่าจะมีรูปแบบวันที่เป็น "dd/MM/yyyy"
                                    DateTime dateLimit;
                                    if (DateTime.TryParseExact(TextBox5.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dateLimit))
                                    {
                                        // ตรวจสอบเงื่อนไขตามวันที่และเวลา
                                        if (DateTime.Today > dateLimit || (DateTime.Today == dateLimit && currentTime > endTime))
                                        {
                                            // ปิดใช้งาน TextBox และ Button ต่างๆ
                                            TextBox1.Text = "";
                                            TextBox6.Text = "";
                                            ENDUSE.Visible = true; // แสดงข้อความ


                                            ENDUSE.ForeColor = System.Drawing.Color.Red; // เปลี่ยนสีเป็นแดง
                                            string errorMessage = $"EndTime('ใบขึ้นของ :\\n {TextBox10.Text} \\nใช้งานได้ถึง 18:00');";

                                            // ใช้ ScriptManager เพื่อเรียกฟังก์ชัน EndTime ในฝั่ง client
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EndTime", errorMessage, true);
                                            TextBox10.Text = "";
                                            TextBox5.Text = DateTime.Now.ToString("dd/MM/yyyy");

                                            return;
                                        }
                                    }
                                    else
                                    {
                                        // แสดงข้อความหาก TextBox10 ไม่มีวันที่ในรูปแบบที่ถูกต้อง
                                        ENDUSE.Text = "รูปแบบวันที่ไม่ถูกต้อง";
                                        ENDUSE.Visible = true;
                                        ENDUSE.ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                                Carid.Text = reader["Key5"].ToString();
                                Compname.Text = reader["ShortChar05"].ToString();
                                TextBox2.Text = reader["Name"].ToString();
                                TextBox4.Text =   Compname.Text+ " : "+ TextBox2.Text ;
                                Label7.Text = reader["Key2"].ToString();
                                Compname.Text = reader["ShortChar18"].ToString();
                                TextBox3.Text = reader["Character09"].ToString();
                              
                               

                            }
                            else
                            {
                                // แสดงข้อความแจ้งเตือนหากไม่พบข้อมูล
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('ไม่พบข้อมูล ชื่อหน่วยงาน')", true);

                                TextBox10.Text = "";
                                Button5.Visible = false;
                            }
                        }
                    }
                    connection.Close();
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Key1Value", TextBox10.Text);
                        command.Parameters.AddWithValue("@CompanyValue", Company.Text);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TextBox13.Text = reader["TotalNumber01"].ToString().TrimEnd('0').TrimEnd('.');
                            }
                            else
                            {
                                // แสดงข้อความแจ้งเตือนหากไม่พบข้อมูล
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('ไม่พบข้อมูล ต้องการส่ง')", true);

                                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ไม่พบข้อมูล ต้องการส่ง')", true);
                                TextBox10.Text = "";
                            }
                        }
                    }
                    connection.Close();

                }
            }

        }

        private void ClearData()
        {
            // เคลียร์ข้อมูลใน TextBox
            Label5.Text = "";
            Label6.Text = "";
            TextBox14.Text = "";
            TextBox13.Text = "";
            Carid.Text = "";
            Compname.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";

            // เคลียร์ DataSource และทำการ DataBind ใหม่ (ถ้ามี)
            GridView6.DataSource = null;
            GridView6.DataBind();
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView4.DataSource = null;
            GridView4.DataBind();
            GridView5.DataSource = null;
            GridView5.DataBind();
            // เคลียร์ ViewState หรือ Session ที่ต้องการ
            ViewState["GridData"] = null;
            ViewState["GridData3"] = null;
            ViewState["GridData5"] = null;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

   



        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            // เรียก Swal.fire เพื่อแสดง Loading เป็นเวลา 5 วินาที
            //ClientScript.RegisterStartupScript(this.GetType(), "showLoading", @"
            //    Swal.fire({
            //        title: 'กำลังโหลด...',
            //        text: 'กรุณารอสักครู่',
            //        allowOutsideClick: false,
            //        timer: 1500, // เวลาแสดง (5 วินาที = 5000 มิลลิวินาที)
            //        didOpen: () => {
            //            Swal.showLoading();
            //        },
            //        willClose: () => {
            //            console.log('Loading ปิดแล้ว');
            //        }
            //    });", true);
            TextBox1.Text = TextBox1.Text.ToUpper();

            string key2Value = TextBox1.Text;
            bool isDuplicate = false;
            bool ismoresum = false;

            // ตรวจสอบข้อมูล Key2 ในฐานข้อมูล
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string checkQuery = "SELECT COUNT(*) FROM ice.UD15 WHERE Key2 = @Key2";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Key2", key2Value);
                    int count = (int)checkCommand.ExecuteScalar();
                    isDuplicate = count > 0;
                }

                string checkSUM = "SELECT COUNT(*) FROM UD15 WHERE BPI_PickingList_c = @BPI_PickingList_c";
                using (SqlCommand checkCommand = new SqlCommand(checkSUM, connection))
                {
                    checkCommand.Parameters.AddWithValue("@BPI_PickingList_c", TextBox10.Text);
                    int sum = (int)checkCommand.ExecuteScalar();
                    int Countud30 = Convert.ToInt16(TextBox13.Text);
                    ismoresum = sum == Countud30;
                    TextBox14.Text = Convert.ToString(sum);

                }
            }

            if (isDuplicate)
            {
                // แจ้งเตือนเมื่อพบข้อมูลซ้ำ
                string errorMessage = $"error('Serail No: {TextBox1.Text} เคยถูกใช้ไปแล้ว');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);
                TextBox1.Text = "";
                return; // ยกเลิกการบันทึกถ้าพบข้อมูลซ้ำ
            }
            if (ismoresum)
            {
                string errorMessage = "error('คุณยิง Serial No: " + TextBox1.Text + " เกินจากที่กำหนด')";

                // หาก TextBox13 มีค่าน้อยกว่า TextBox14
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);
                TextBox1.Text = "";
                return;
            }

            if (Label5.Text == "" && Label6.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('กรุณา สแกน BIN และ WareHouse ก่อนครับ')", true);
                TextBox1.Text = "";
                return;


            }
            else if (Label5.Text == "" && Label6.Text != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('กรุณา สแกน BIN  ก่อนครับ')", true);
                TextBox1.Text = "";
                return;

            }
            else if (Label5.Text != "" && Label6.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('กรุณา สแกน WareHouse  ก่อนครับ')", true);
                TextBox1.Text = "";
                return;

            }
            if (!string.IsNullOrEmpty(TextBox14.Text))
            {
                if (TextBox13.Text == TextBox14.Text)
                {
                    string errorMessage = "error('คุณยิง Serial No: " + TextBox1.Text + " เกินจากที่กำหนด')";

                    // หาก TextBox13 มีค่าน้อยกว่า TextBox14
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);
                    TextBox1.Text = "";
                    return;
                }
            }
            if (!string.IsNullOrEmpty(TextBox1.Text))
            {
                UpdateData();

            }

        }

        private bool IsValueAlreadyExist(DataTable dt, string value)
        {

            foreach (DataRow row in dt.Rows)
            {
                if (row["TextColumn"].ToString() == value)
                {
                    return true;
                }
            }
            return false;
        }
        protected void UpdateData()
        {


            if (ViewState["GridData3"] != null && ViewState["GridData3"] is DataTable)
            {
                DataTable dt3 = (DataTable)ViewState["GridData3"];

                // วนลูปผ่านแต่ละแถวใน DataTable
                foreach (DataRow row in dt3.Rows)
                {
                    // ตรวจสอบค่าในคอลัมน์ TextColumn ว่าตรงกับค่าที่ใส่ใน TextBox1 หรือไม่
                    if (row["TextColumn"].ToString() == TextBox1.Text)
                    {
                        string errorMessage = "error('คุณสแกน Checking No: " + TextBox1.Text + " ซ้ำ')";

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);
                        TextBox1.Text = "";
                        return; // หากพบค่าที่ตรงกันใน GridView4 ให้หยุดการวนลูป
                    }
                }
            }


            // ดำเนินการตรวจสอบ ViewState ของ gvOrders และอัพเดทข้อมูลใหม่ตามที่ต้องการ
            if (ViewState["GridData3"] != null)
            {
                string connectionString2 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString2))
                {
                    string gridsave = @"SELECT 
    UD03.Character01, 
    UD03.Key5, 
    UD03.Key4, 
    UD03.Key3, 
    project.WarehouseCode,
    project.BinNum,
    ud28.Character09, 
    p.IUM, 
    ud28.Character02,
    Project.Description AS ProjectDescription,
    ud28.ShortChar16,
    WHT.DefShipWhse,
	WHT.DefShipBin
FROM 
    Ice.UD03 AS UD03
    JOIN Ice.UD28 AS UD28 ON UD03.Character01 = UD28.ShortChar08
    LEFT JOIN Project project ON project.ProjectID = ud28.Character02 AND project.Company = ud28.Company
    LEFT JOIN ERP.Part p ON p.PartNum = ud28.ShortChar08 AND p.Company = ud28.Company
    LEFT JOIN Ice.UD100 ud100 ON ud28.Key5 = ud100.Key1 AND ud28.Company = ud100.Company
	Left Join erp.PlantConfCtrl WHT ON   WHT.Company = ud28.Company
 WHERE UD03.Company = @CompanyValue
 AND UD03.Key1 = @Key1Value
 AND UD28.Key1 = @Key1Value28
 AND WHT.Plant = @site

GROUP BY 
    UD03.Key1, 
    UD03.Key2,
    UD03.Character01, 
    UD03.Key5, 
    UD03.Key4, 
    UD28.ShortChar10, 
    UD28.ShortChar09, 
    UD03.Key3, 
    project.WarehouseCode,
    project.BinNum,
    ud28.Character09, 
    p.IUM, 
    ud28.Character02,
    Project.Description,
    ud28.ShortChar16,
    WHT.DefShipWhse,
	WHT.DefShipBin
";
                    using (SqlCommand command = new SqlCommand(gridsave, connection))
                    {
                        connection.Open();
                        string site = Request.QueryString["site"];
                        command.Parameters.AddWithValue("@site", site);
                        command.Parameters.AddWithValue("@CompanyValue", Company.Text);
                        command.Parameters.AddWithValue("@Key1Value", TextBox1.Text);
                        command.Parameters.AddWithValue("@Key1Value28", TextBox10.Text);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            DataTable dt2 = (DataTable)ViewState["GridData3"];
                            if (!reader.HasRows) // ตรวจสอบว่ามีข้อมูลหรือไม่
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error",
                                    $"Swal.fire({{ icon: 'error', title: 'ข้อผิดพลาด', text: 'Serial {TextBox1.Text} Part ไม่ตรงกับใบเบิก {TextBox10.Text}' }});", true);

                                return; // ออกจากเมธอดทันที
                            }

                            if (!IsValueAlreadyExist(dt2, TextBox1.Text))
                            {// กำหนดค่าเริ่มต้นของ No
                                int rowIndex = 1;

                                // ตรวจสอบว่า DataTable มีแถวหรือไม่
                                if (dt2.Rows.Count > 0)
                                {
                                    // ใช้ LINQ เพื่อค้นหาค่าที่มากที่สุดในคอลัมน์ "No" และเพิ่มค่า 1
                                    // การใช้ Convert.ToInt32 เพื่อให้แน่ใจว่าค่าที่อ่านมาแปลงเป็น int ได้
                                    rowIndex = dt2.AsEnumerable()
                                        .Select(row => row.Field<object>("No"))
                                        .Where(value => value != DBNull.Value) // ตรวจสอบค่าที่ไม่ใช่ DBNull
                                        .Select(value => Convert.ToInt32(value))
                                        .Max() + 1;
                                }

                                while (reader.Read())
                                {
                                    DataRow newRow = dt2.NewRow();
                                    newRow["No"] = rowIndex++; // เพิ่มค่าของคอลัมน์ No
                                    newRow["TextColumn"] = TextBox1.Text;
                                    newRow["PartNumber"] = reader["Character01"]; // เพิ่มค่า Key2 จากผลลัพธ์ของ SQL SELECT
                                    newRow["LOT"] = reader["Key5"]; // เพิ่มค่า Key3 จากผลลัพธ์ของ SQL SELECT
                                    newRow["WH"] = Label5.Text; // เพิ่มค่า Key5 จากผลลัพธ์ของ SQL SELECT
                                    newRow["BIN"] = Label6.Text;// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                    newRow["WH_C"] = reader["DefShipWhse"]; // เพิ่มค่า Key5 จากผลลัพธ์ของ SQL SELECT
                                    newRow["BIN_C"] = reader["DefShipBin"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                    newRow["Character09"] = reader["Character09"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                    newRow["UOM"] = reader["IUM"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                    newRow["Character02"] = reader["Character02"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                    newRow["ProjectDescription"] = reader["ProjectDescription"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT

                                    Part.Text = reader["Character01"].ToString();

                                    dt2.Rows.Add(newRow);
                                }

                                // บันทึก DataTable ลงใน ViewState
                                ViewState["GridData3"] = dt2;
                            }

                        }
                    }
                }

            }
            else
            {
                string connectionString1 = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("No", typeof(string));
                dt2.Columns.Add("TextColumn", typeof(string));
                dt2.Columns.Add("PartNumber", typeof(string));
                dt2.Columns.Add("LOT", typeof(string));
                dt2.Columns.Add("WH", typeof(string));
                dt2.Columns.Add("BIN", typeof(string));
                dt2.Columns.Add("WH_C", typeof(string));
                dt2.Columns.Add("BIN_C", typeof(string));
                dt2.Columns.Add("Character09", typeof(string));
                dt2.Columns.Add("UOM", typeof(string));
                dt2.Columns.Add("Character02", typeof(string));
                dt2.Columns.Add("ProjectDescription", typeof(string));
                dt2.Columns.Add("ShortChar16", typeof(string));

                using (SqlConnection connection = new SqlConnection(connectionString1))
                {
                    string gridsave = @"SELECT 
    UD03.Character01, 
    UD03.Key5, 
    UD03.Key4, 
    UD03.Key3, 
    project.WarehouseCode,
    project.BinNum,
    ud28.Character09, 
    p.IUM, 
    ud28.Character02,
    Project.Description AS ProjectDescription,
    ud28.ShortChar16,
    WHT.DefShipWhse,
	WHT.DefShipBin
FROM 
    Ice.UD03 AS UD03
    JOIN Ice.UD28 AS UD28 ON UD03.Character01 = UD28.ShortChar08
    LEFT JOIN Project project ON project.ProjectID = ud28.Character02 AND project.Company = ud28.Company
    LEFT JOIN ERP.Part p ON p.PartNum = ud28.ShortChar08 AND p.Company = ud28.Company
    LEFT JOIN Ice.UD100 ud100 ON ud28.Key5 = ud100.Key1 AND ud28.Company = ud100.Company
	Left Join erp.PlantConfCtrl WHT ON   WHT.Company = ud28.Company
 WHERE UD03.Company = @CompanyValue
 AND UD03.Key1 = @Key1Value
 AND UD28.Key1 = @Key1Value28
 AND WHT.Plant = @site

GROUP BY 
    UD03.Key1, 
    UD03.Key2,
    UD03.Character01, 
    UD03.Key5, 
    UD03.Key4, 
    UD28.ShortChar10, 
    UD28.ShortChar09, 
    UD03.Key3, 
    project.WarehouseCode,
    project.BinNum,
    ud28.Character09, 
    p.IUM, 
    ud28.Character02,
    Project.Description,
    ud28.ShortChar16,
    WHT.DefShipWhse,
	WHT.DefShipBin
 ";

                    using (SqlCommand command = new SqlCommand(gridsave, connection))
                    {
                        connection.Open();
                        string site = Request.QueryString["site"];
                        command.Parameters.AddWithValue("@site", site);
                        command.Parameters.AddWithValue("@CompanyValue", Company.Text);
                        command.Parameters.AddWithValue("@Key1Value", TextBox1.Text);
                        command.Parameters.AddWithValue("@Key1Value28", TextBox10.Text);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows) // ตรวจสอบว่ามีข้อมูลหรือไม่
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error",
                                    $"Swal.fire({{ icon: 'error', title: 'ข้อผิดพลาด', text: 'Serial {TextBox1.Text} Part ไม่ตรงกับใบเบิก {TextBox10.Text}' }});", true);

                                return; // ออกจากเมธอดทันที
                            }

                            int rowIndex = 1; // กำหนดค่าเริ่มต้นของ No

                            while (reader.Read())
                            {
                                DataRow newRow = dt2.NewRow();
                                newRow["No"] = rowIndex++; // เพิ่มค่าของคอลัมน์ No
                                newRow["TextColumn"] = TextBox1.Text;
                                newRow["PartNumber"] = reader["Character01"]; // เพิ่มค่า Key2 จากผลลัพธ์ของ SQL SELECT
                                newRow["LOT"] = reader["Key5"]; // เพิ่มค่า Key3 จากผลลัพธ์ของ SQL SELECT
                                newRow["WH"] = Label5.Text; // เพิ่มค่า Key5 จากผลลัพธ์ของ SQL SELECT
                                newRow["BIN"] = Label6.Text;// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                newRow["WH_C"] = reader["DefShipWhse"]; // เพิ่มค่า Key5 จากผลลัพธ์ของ SQL SELECT
                                newRow["BIN_C"] = reader["DefShipBin"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                newRow["Character09"] = reader["Character09"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                newRow["UOM"] = reader["IUM"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                newRow["Character02"] = reader["Character02"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT
                                newRow["ProjectDescription"] = reader["ProjectDescription"];// เพิ่มค่า Key4 จากผลลัพธ์ของ SQL SELECT

                                Part.Text = reader["Character01"].ToString();

                                dt2.Rows.Add(newRow);
                            }

                        }
                    }
                }

                ViewState["GridData3"] = dt2;

            }

            if (ViewState["GridData3"] != null && ViewState["GridData3"] is DataTable)
            {
                DataTable dt3 = (DataTable)ViewState["GridData3"];

                // เช็คว่าคอลัมน์ TextColumn มีค่าซ้ำกันหรือไม่
                bool hasDuplicate = false;
                HashSet<string> uniqueValues = new HashSet<string>(); // เก็บค่าที่ไม่ซ้ำกัน
                foreach (DataRow row in dt3.Rows)
                {
                    string textValue = row["TextColumn"].ToString();

                    if (uniqueValues.Contains(textValue))
                    {
                        hasDuplicate = true;
                        break; // หยุดการวนลูปหากพบค่าที่ซ้ำกัน
                    }
                    else
                    {
                        uniqueValues.Add(textValue); // เพิ่มค่าลงใน HashSet เพื่อใช้ในการเช็คซ้ำ
                    }
                }

                // หากพบค่าที่ซ้ำกันในคอลัมน์ TextColumn
                if (hasDuplicate)
                {
                    // แสดงข้อความแจ้งเตือนผ่าน JavaScript
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('สแกนซ้ำ')", true);
                    DataTable dt2 = (DataTable)ViewState["GridData"];

                    int totalQty = 0; // สร้างตัวแปรเพื่อเก็บผลรวมของ Qty
                    if (dt2 != null)
                    {
                        foreach (DataRow row2 in dt2.Rows)
                        {
                            // อ่านค่า Qty จากแถวและเพิ่มเข้าไปในผลรวม
                            totalQty += Convert.ToInt32(row2["Qty"]);
                        }
                    }

                    DataTable dt4 = ViewState["GridData3"] as DataTable;

                    // Compare data in GridView2 with GridView4 and delete matching rows
                    if (dt4 != null)
                    {

                        string key1Value = TextBox1.Text; // Assuming "Key1" is the column name

                        for (int i = dt4.Rows.Count - 1; i >= 0; i--)
                        {
                            string key4Value = dt4.Rows[i]["TextColumn"].ToString(); // Assuming "Key4" is the column name

                            if (key1Value == key4Value)
                            {
                                dt4.Rows.RemoveAt(i);
                            }

                        }

                        // Rebind GridView4 after removing matching rows
                        GridView4.DataSource = dt4;
                        GridView4.DataBind();
                        TextBox1.Text = "";
                    }
                    TextBox14.Text = totalQty.ToString();

                    return;
                }
            }
            // หากคุณใช้ GridView ที่เป็น TemplateField เช่น gvOrders
            // คุณต้องอัพเดท DataSource และ Bind ข้อมูลใหม่ให้กับ gvOrders ด้วย
            GridView4.DataSource = ViewState["GridData3"];
            GridView4.DataBind();


            // ดึงข้อมูลที่ใส่ใน TextBox1
            string inputKey2 = TextBox1.Text.Trim();




            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            string Tranfer = "Select Key5 From ice.UD15 Where Key2 =@Key1Value";
            string query = @"
    SELECT DISTINCT 
        UD03.Key1, 
        UD03.Key2, 
        UD03.Character01, 
        UD03.CheckBox02, 
        UD03.Key5, 
        UD03.Key4, 
        UD03.Key1, 
        UD03.Key3,
	    UD28.Character09,

        SUM(UD28.Number04) AS Number04 
    FROM 
        ice.ud03 AS UD03 
    JOIN 
        Ice.UD28 AS UD28 
        ON UD03.Character01 = UD28.ShortChar08 
    WHERE 
        UD03.Company = @CompanyValue 
        AND UD03.Key1 = @Key1value 
        AND UD28.Key1 = @Key1value28 
       
    GROUP BY 
        UD03.Key1, 
        UD03.Key2, 
        UD03.Key5, 
        UD03.Character01, 
        UD03.Key4, 
        UD03.CheckBox02, 
        UD03.Key1, 
        UD28.ShortChar10, 
        UD28.ShortChar09, 
        UD28.ShortChar10,
	    UD28.Character09,

        UD03.Key3";
            string query3 = "SELECT * FROM Ice.UD28 WHERE Key1 = @Key1Value AND Company = @CompanyValue ";
            string queryPart = "SELECT Key2 FROM ice.ud03 WHERE  Company = @CompanyValue AND Key1 = @Key1value";
            string queryvalue = @"
  SELECT 
      UD28.ShortChar08 as ShortChar08, 
    UD30.Character03 AS WH,
    UD30.Character04 AS Bin,
    UD30.Number04 AS Number04,
	ud28.ShortChar17,
	ud28.ShortChar18,
	ud28.ShortChar19

FROM     
    Ice.UD30 
JOIN 
    Ice.UD28 
ON 
    UD30.Key1 = UD28.Key1 and UD30.Company = UD28.Company and UD30.Key5 = ud28.ShortChar08 and UD30.Key3 = UD28.Key3

WHERE   
    UD30.Key1 = @Key1Value
	--AND ud28.Date01 = ud30.Date01
	--and UD28.Company = 'BPI'
GROUP BY   
    --UD28.ShortChar08, 
	ud28.ShortChar08,
	ud30.Character03,
	ud30.Character04,
	ud30.Number04,
	ud28.ShortChar17,
	ud28.ShortChar18,
	ud28.ShortChar19


";
            string queryvalueforGridView8 = @"
  SELECT 
      UD30.Key5 as ShortChar08, 
    UD30.Character03 AS WH,
    UD30.Character04 AS Bin,
    Sum(UD30.Number04) AS Number04
FROM     
    Ice.UD30 
--JOIN 
--    Ice.UD28 
--ON 
--UD30.Key1 = UD28.Key1 and UD30.Company = UD28.Company and UD30.Key5 = ud28.ShortChar08

WHERE   
    UD30.Key1 = @Key1Value
	--AND ud28.Date01 = ud30.Date01
	--and UD28.Company = 'BPI'
GROUP BY   
    --UD28.ShortChar08, 
	ud30.Key5,
	ud30.Character03,
	ud30.Character04

";
            string queryvalueforSUBSO = @"
  SELECT 
      UD30.Key5 as ShortChar08, 
    UD30.Character03 AS WH,
    UD30.Character04 AS Bin,
	UD28.ShortChar17,
	UD28.ShortChar18,
	UD28.ShortChar19,
    Sum(UD30.Number04) AS Number04
FROM     
    Ice.UD30 
JOIN 
    Ice.UD28 
ON 
    UD30.Key1 = UD28.Key1 
    AND UD30.Company = UD28.Company 
    AND UD30.Key5 = UD28.ShortChar08

WHERE   
    UD30.Key1 = @Key1Value
	--AND ud28.Date01 = ud30.Date01
	--and UD28.Company = 'BPI'
GROUP BY   
    UD30.Key5,
    UD30.Character03,
    UD30.Character04,
	UD28.ShortChar17,
	UD28.ShortChar18,
	UD28.ShortChar19
HAVING 
    COUNT(UD30.Key5) = 1

";
            string queryvalueqty = "SELECT DISTINCT  UD03.Key3,UD03.Key4,UD03.Key2, SUM(UD28.Number04) AS Number04 FROM Ice.Ud03 UD03 JOIN Ice.UD28 UD28 ON UD03.Character01 = UD28.ShortChar08 WHERE UD03.Key1 = @Key1value AND UD28.Key1 = @Key1value28 and UD03.Key3 =@Warehouse AND UD03.Key4=@BIN  GROUP BY UD03.Key3,UD03.Key4,UD03.Key2;";
            DataTable dt;


            DataTable dtGrid5 = new DataTable(); // กำหนดค่าเริ่มต้นให้ dtGrid5 เป็น DataTable เปล่าๆ
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                using (SqlCommand command = new SqlCommand(Tranfer, connection))
                {
                    command.Parameters.AddWithValue("@Key1Value", TextBox1.Text);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // ใช้ while เพื่อ loop ผ่านทุก record ใน result set
                        {
                            string key5Value = reader.GetString(reader.GetOrdinal("Key5"));

                            // ตรวจสอบว่าค่าในคอลัมน์ Key5 เท่ากับ "Tranfer" หรือไม่
                            if (key5Value == "Tranfer")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('ค่านี้ Tranfer ไปแล้วครับ')", true);
                                return;
                            }
                        }
                    }
                }

            }


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryvalueqty, connection))
                {
                    command.Parameters.AddWithValue("@Key1Value", TextBox1.Text); // Add Key1Value from TextBox1
                    command.Parameters.AddWithValue("@Key1Value28", TextBox10.Text); // Add Key1Value28 from TextBox10
                    command.Parameters.AddWithValue("@Warehouse", Warehouse.Text); // Add Key1Value from TextBox1
                    command.Parameters.AddWithValue("@BIN", Label6.Text); // Add Key1Value28 from TextBox10

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if GridData5 exists in ViewState
                        if (ViewState["GridData5"] != null)
                        {
                            dtGrid5 = (DataTable)ViewState["GridData5"];
                        }
                        else
                        {
                            // If GridData5 doesn't exist, create a new DataTable with columns
                            dtGrid5.Columns.Add("Key2", typeof(string));
                            dtGrid5.Columns.Add("Bin", typeof(string));
                            dtGrid5.Columns.Add("WH", typeof(string));
                            dtGrid5.Columns.Add("ColumnQty", typeof(int));
                            dtGrid5.Columns.Add("Number04", typeof(double)); // Add Number04 column
                        }


                        // Save the DataTable to ViewState and bind it to GridView5
                        ViewState["GridData5"] = dtGrid5;
                        GridView5.DataSource = dtGrid5;
                        GridView5.DataBind();
                    }
                }
                connection.Close(); // Close the database connection

            }
            // SQL query to get Key2
            string queryPAT = @"
SELECT Key2 
FROM Ice.UD03
WHERE Key2 IN (
    SELECT ShortChar08 
    FROM Ice.UD28
    WHERE Key1 = @Textbox10
) AND Key1 = @Textbox1";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(queryPAT, conn))
                    {
                        cmd.Parameters.AddWithValue("@Textbox10", TextBox10.Text);
                        cmd.Parameters.AddWithValue("@Textbox1", TextBox1.Text);

                        conn.Open();

                        object result = cmd.ExecuteScalar();
                        Label10.Text = result != null ? result.ToString() : "No data found.";
                    }
                }
            }
            catch (Exception ex)
            {
                Label10.Text = "Error: " + ex.Message;
            }

            //Create DataTable for query value

            DataTable dtQueryPAT = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand commandQueryValue = new SqlCommand(queryvalueforGridView8, connection))
                {
                    commandQueryValue.Parameters.AddWithValue("@Key1Value", TextBox10.Text);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(commandQueryValue))
                    {
                        adapter.Fill(dtQueryPAT);
                        dtQueryPAT.Columns.Add("QTY", typeof(int));

                        Dictionary<string, int> partNumberCounts = new Dictionary<string, int>();

                        string partNumberValue = Label10?.Text;
                        string binValue = Label6?.Text;
                        string whValue = Label5?.Text;

                        if (partNumberValue != null && binValue != null && whValue != null)
                        {
                            string key1 = $"{partNumberValue}_{binValue}_{whValue}";

                            if (partNumberCounts.ContainsKey(key1))
                            {
                                partNumberCounts[key1]++;
                            }
                            else
                            {
                                partNumberCounts[key1] = 1;
                            }
                        }

                        bool hasMatchingData = false;

                        foreach (DataRow row in dtQueryPAT.Rows)
                        {
                            string partNumber = row["ShortChar08"].ToString();
                            string bin = row["BIN"].ToString();
                            string wh = row["WH"].ToString();
                            double number04Double = double.Parse(row["Number04"].ToString());
                            int number04 = (int)number04Double;
                            string key = $"{partNumber}_{bin}_{wh}";

                            Label9.Text = Convert.ToString(number04);

                            if (partNumberCounts.ContainsKey(key))
                            {

                                int currentQty = partNumberCounts[key];
                                hasMatchingData = true;

                                if (currentQty > number04)
                                {
                                    string errorMessage = $"error('คุณยิง Part Num : {Part?.Text} จำนวนเกินจากที่กำหนด')";
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);

                                    DataTable dt2 = ViewState["GridData"] as DataTable;
                                    if (dt2 != null)
                                    {
                                        int totalQty = dt2.AsEnumerable().Sum(row2 => row2.Field<int>("Qty"));

                                        DataTable dt4 = ViewState["GridData3"] as DataTable;
                                        if (dt4 != null)
                                        {
                                            string key1Value = TextBox1?.Text;

                                            for (int i = dt4.Rows.Count - 1; i >= 0; i--)
                                            {
                                                string key4Value = dt4.Rows[i]["TextColumn"].ToString();

                                                if (key1Value == key4Value)
                                                {
                                                    dt4.Rows.RemoveAt(i);
                                                }
                                            }

                                            GridView4.DataSource = dt4;
                                            GridView4.DataBind();

                                            TextBox1.Text = "";
                                        }
                                        TextBox14.Text = totalQty.ToString();
                                        ViewState["HasError"] = true;

                                        return;
                                    }
                                }
                                else if (currentQty <= number04)
                                {
                                    // อัปเดตค่า QTY ในแถวที่มีค่า ShortChar18 น้อยที่สุด
                                    row["QTY"] = partNumberCounts[key];
                                }
                                else
                                {
                                    // หากไม่พบคีย์ใน Dictionary ให้กำหนด QTY เป็น 0
                                    row["QTY"] = 0;
                                }
                            }

                        }

                        if (!hasMatchingData)
                        {
                            string errorMessage = $"error('คุณยิง Part Num : {Part?.Text} ไม่ตรงกับ WH หรือ BIN ที่คอนเฟิร์ม')";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);

                            DataTable dt2 = ViewState["GridData"] as DataTable;
                            if (dt2 != null)
                            {
                                int totalQty = dt2.AsEnumerable().Sum(row2 => row2.Field<int>("Qty"));

                                DataTable dt4 = ViewState["GridData3"] as DataTable;
                                if (dt4 != null)
                                {
                                    string key1Value = TextBox1?.Text;

                                    for (int i = dt4.Rows.Count - 1; i >= 0; i--)
                                    {
                                        string key4Value = dt4.Rows[i]["TextColumn"].ToString();

                                        if (key1Value == key4Value)
                                        {
                                            dt4.Rows.RemoveAt(i);
                                        }
                                    }

                                    GridView4.DataSource = dt4;
                                    GridView4.DataBind();

                                    TextBox1.Text = "";
                                }
                                TextBox14.Text = totalQty.ToString();
                                ViewState["HasError"] = true;

                                return;
                            }
                        }
                    }
                }
            }

            // Bind GridView8 with DataTable
            GridView8.DataSource = dtQueryPAT;
            GridView8.DataBind();

            DataTable dtforSO = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand commandQueryValue = new SqlCommand(queryvalueforGridView8, connection))
                {
                    commandQueryValue.Parameters.AddWithValue("@Key1Value", TextBox10.Text); // รับค่า TextBox10 แทนที่ @Key1Value
                    using (SqlDataAdapter adapter = new SqlDataAdapter(commandQueryValue))
                    {
                        adapter.Fill(dtforSO); // เติมข้อมูลจาก queryvalue ลงใน DataTable dtQueryValue
                        dtforSO.Columns.Add("QTY", typeof(int)); // เพิ่มคอลัมน์ QTY โดยให้มีชนิดข้อมูลเป็น int

                        // Dictionary สำหรับเก็บจำนวน PartNumber ที่พบไปแล้ว
                        Dictionary<string, int> partNumberCounts = new Dictionary<string, int>();
                        List<GridViewRow> recentRows = new List<GridViewRow>();

                        foreach (GridViewRow row in GridView4.Rows)
                        {
                            string partNumberValue = row.Cells[2].Text;
                            string binValue = row.Cells[5].Text;
                            string whValue = row.Cells[4].Text;

                            // สร้าง key สำหรับ Dictionary
                            string key = $"{partNumberValue}_{binValue}_{whValue}";

                            // เพิ่มหรืออัปเดตจำนวน PartNumber ใน Dictionary
                            if (partNumberCounts.ContainsKey(key))
                            {
                                partNumberCounts[key]++;
                            }
                            else
                            {
                                partNumberCounts[key] = 1;
                            }
                            recentRows.Add(row);

                        }
                        var latestRows = recentRows
                            .GroupBy(row => new
                            {
                                PartNumber = row.Cells[2].Text,
                                Bin = row.Cells[5].Text,
                                WH = row.Cells[4].Text
                            })
                            .Select(group => group.OrderByDescending(row => row.RowIndex).First())
                            .ToList();
                        bool hasMatchingData = false; // ตัวแปรเพื่อเก็บสถานะการตรวจสอบข้อมูลที่ตรงกัน

                        // อัปเดตค่า QTY ใน DataTable ตามจำนวนที่พบใน Dictionary
                        foreach (DataRow row in dtforSO.Rows)
                        {
                            string partNumber = row["ShortChar08"].ToString();
                            string bin = row["BIN"].ToString();
                            string wh = row["WH"].ToString();
                            double number04Double = double.Parse(row["Number04"].ToString());
                            int number04 = (int)number04Double;
                            string key = $"{partNumber}_{bin}_{wh}";
                   
                            if (number04 != null)
                            {
                                Label9.Text = Convert.ToString(number04);
                            }

                            if (partNumberCounts.ContainsKey(key))
                            {
                                int currentQty = partNumberCounts[key];
                                hasMatchingData = true; // พบข้อมูลที่ตรงกัน

                                if (currentQty > number04) // ตรวจสอบก่อนว่าค่า QTY >= Number04 หรือไม่
                                {
                                    // แสดงข้อผิดพลาด (error)
                                    string errorMessage = $"error('คุณยิง Part Num 9: {Part.Text}  จำนวนเกินจากที่กำหนด')";
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);

                                    DataTable dt2 = (DataTable)ViewState["GridData"];
                                    int totalQty = 0; // สร้างตัวแปรเพื่อเก็บผลรวมของ Qty

                                    if (dt2 != null)
                                    {
                                        foreach (DataRow row2 in dt2.Rows)
                                        {
                                            // อ่านค่า Qty จากแถวและเพิ่มเข้าไปในผลรวม
                                            totalQty += Convert.ToInt32(row2["Qty"]);
                                        }
                                    }

                                    DataTable dt4 = ViewState["GridData3"] as DataTable;

                                    // Compare data in GridView2 with GridView4 and delete matching rows
                                    if (dt4 != null)
                                    {
                                        string key1Value = TextBox1.Text; // Assuming "Key1" is the column name

                                        for (int i = dt4.Rows.Count - 1; i >= 0; i--)
                                        {
                                            string key4Value = dt4.Rows[i]["TextColumn"].ToString(); // Assuming "Key4" is the column name

                                            if (key1Value == key4Value)
                                            {
                                                dt4.Rows.RemoveAt(i);
                                            }
                                        }

                                        // Rebind GridView4 after removing matching rows
                                        GridView4.DataSource = dt4;
                                        GridView4.DataBind();

                                        TextBox1.Text = "";
                                    }
                                    TextBox14.Text = totalQty.ToString();
                                    ViewState["HasError"] = true; // ตั้งค่าสถานะข้อผิดพลาดใน ViewState

                                    return; // ออกจากการทำงานของเมธอด UpdateData()
                                }
                                else if (currentQty <= number04)
                                {
                                    row["QTY"] = partNumberCounts[key];
                                }
                            }
                            else
                            {
                                // หากไม่พบคีย์ใน Dictionary ให้กำหนด QTY เป็น 0
                                row["QTY"] = 0;

                            }
                        }

                        // ตรวจสอบว่ามีข้อมูลที่ตรงกันหรือไม่
                        if (!hasMatchingData)
                        {
                            string errorMessage = $"error('คุณยิง Part Num : {Part?.Text} ไม่ตรงกับ WH หรือ BIN ที่คอนเฟิร์ม')";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);

                            DataTable dt2 = ViewState["GridData"] as DataTable;
                            if (dt2 != null)
                            {
                                int totalQty = dt2.AsEnumerable().Sum(row2 => row2.Field<int>("Qty"));

                                DataTable dt4 = ViewState["GridData3"] as DataTable;
                                if (dt4 != null)
                                {
                                    string key1Value = TextBox1?.Text;

                                    for (int i = dt4.Rows.Count - 1; i >= 0; i--)
                                    {
                                        string key4Value = dt4.Rows[i]["TextColumn"].ToString();

                                        if (key1Value == key4Value)
                                        {
                                            dt4.Rows.RemoveAt(i);
                                        }
                                    }

                                    GridView4.DataSource = dt4;
                                    GridView4.DataBind();

                                    TextBox1.Text = "";
                                }
                                TextBox14.Text = totalQty.ToString();
                                ViewState["HasError"] = true;

                                return;
                            }
                        }
                    }
                }
            }
            // Bind GridView8 with DataTable
            GridView9.DataSource = dtforSO;
            GridView9.DataBind();
            //สร้าง DataTable เพื่อเก็บข้อมูลที่ได้จาก queryvalue
            DataTable dtQueryValue = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand commandQueryValue = new SqlCommand(queryvalue, connection))
                {
                    commandQueryValue.Parameters.AddWithValue("@Key1Value", TextBox10.Text); // รับค่า TextBox10 แทนที่ @Key1Value
                    using (SqlDataAdapter adapter = new SqlDataAdapter(commandQueryValue))
                    {
                        adapter.Fill(dtQueryValue); // เติมข้อมูลจาก queryvalue ลงใน DataTable dtQueryValue
                        dtQueryValue.Columns.Add("QTY", typeof(int)); // เพิ่มคอลัมน์ QTY โดยให้มีชนิดข้อมูลเป็น int
                        dtQueryValue.Columns.Add("Update", typeof(int)); // เพิ่มคอลัมน์ QTY โดยให้มีชนิดข้อมูลเป็น int

                        // Dictionary สำหรับเก็บจำนวน PartNumber ที่พบไปแล้ว
                        Dictionary<string, int> partNumberCounts = new Dictionary<string, int>();
                        Dictionary<string, int> partNumberCounts2 = new Dictionary<string, int>();

                        List<GridViewRow> recentRows = new List<GridViewRow>();

                        foreach (GridViewRow row in GridView4.Rows)
                        {
                            string partNumberValue = row.Cells[2].Text;
                            string binValue = row.Cells[5].Text;
                            string whValue = row.Cells[4].Text;

                            string partNumberValue2 = Label10?.Text;
                            string binValue2 = Label6?.Text;
                            string whValue2 = Label5?.Text;

                            

                            // สร้าง key สำหรับ Dictionary
                            string key = $"{partNumberValue}_{binValue}_{whValue}";
                            string key2 = $"{partNumberValue2}_{binValue2}_{whValue2}";

                            // เพิ่มหรืออัปเดตจำนวน PartNumber ใน Dictionary
                            if (partNumberCounts.ContainsKey(key))
                            {
                                partNumberCounts[key]++;
                            }
                            else
                            {
                                partNumberCounts[key] = 1;
                            }
                            // เพิ่มหรืออัปเดตจำนวน PartNumber ใน Dictionary
                            if (partNumberCounts2.ContainsKey(key2))
                            {
                                partNumberCounts2[key2]++;
                            }
                            else
                            {
                                partNumberCounts2[key2] = 1;
                            }
                            recentRows.Add(row);

                        }
                        var latestRows = recentRows
                            .GroupBy(row => new
                            {
                                PartNumber = row.Cells[2].Text,
                                Bin = row.Cells[5].Text,
                                WH = row.Cells[4].Text
                            })
                            .Select(group => group.OrderByDescending(row => row.RowIndex).First())
                            .ToList();
                        bool hasMatchingData = false; // ตัวแปรเพื่อเก็บสถานะการตรวจสอบข้อมูลที่ตรงกัน

                        foreach (DataRow row in dtQueryValue.Rows)
                        {
                            string partNumber = row["ShortChar08"].ToString();
                            string bin = row["BIN"].ToString();
                            string wh = row["WH"].ToString();
                            double number04Double = double.Parse(row["Number04"].ToString());
                            int number04 = (int)number04Double;
                            string key = $"{partNumber}_{bin}_{wh}";

                            if (number04 != null)
                            {
                                Label9.Text = Convert.ToString(number04);
                            }

                            if (partNumberCounts.ContainsKey(key))
                            {
                                int currentQty = partNumberCounts[key];
                                hasMatchingData = true; // พบข้อมูลที่ตรงกัน
                                Label12.Text = "";
                                Label11.Text = "";
                                // ค้นหาแถวที่ตรงกับ key และเรียงลำดับตามค่า ShortChar18 จากมากไปน้อย
                                var matchingRows = dtQueryValue.AsEnumerable()
                                    .Where(r =>
                                        r["ShortChar08"].ToString() == partNumber &&
                                        r["BIN"].ToString() == bin &&
                                        r["WH"].ToString() == wh)
                                    .OrderByDescending(r => Convert.ToInt32(r["ShortChar18"]))
                                    .ToList();
                                // เช็คจำนวนแถวที่ตรงกัน
                                bool qtyUpdated = false;
                                int qtyToUpdate = currentQty;  // เก็บค่า QTY ที่ต้องการอัปเดต

                                // ลองอัปเดตค่า QTY ตามลำดับของ ShortChar18 จากมากไปน้อย
                                foreach (var r in matchingRows)
                                {
                          
                                    int rowShortChar18 = Convert.ToInt32(r["ShortChar18"]);
                                    int rowShortChar17 = Convert.ToInt32(r["ShortChar17"]);

                                    int rowNumber04 = Convert.ToInt32(r["Number04"]);
                                    // ตั้งค่า Label11 และ Label12 เป็นค่า ShortChar17 และ ShortChar18
                                  
                                    // ถ้าสามารถเพิ่ม QTY ในแถวนี้ได้
                                    if (qtyToUpdate <= rowNumber04)
                                    {
                                        r["QTY"] = qtyToUpdate;  // อัปเดต QTY ของแถวที่เลือก

                                        qtyUpdated = true;
                              

                                        break; // ออกจากลูปเมื่ออัปเดตสำเร็จ

                                    }

                                    else
                                    {
                                        // ถ้าแถวนี้ไม่สามารถรองรับ QTY ที่ต้องการได้, เก็บค่า QTY ในแถวนี้ไว้
                                        r["QTY"] = 0;

                                        qtyToUpdate -= rowNumber04;  // ลดค่าที่เหลือจาก QTY ที่ต้องการอัปเดต

                                    }
                                }

                                // หากไม่สามารถอัปเดต QTY ในแถวที่ตรงกันได้ ให้แสดงข้อผิดพลาด
                                if (!qtyUpdated)
                                {
                                    string errorMessage = $"error('ไม่สามารถเพิ่มค่า QTY G8ได้ เนื่องจากไม่มีแถวที่สามารถรองรับการเพิ่มค่าได้สำหรับ Part Num : {partNumber}')";
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);
                                    ViewState["HasError"] = true; // ตั้งค่าสถานะข้อผิดพลาดใน ViewState
                                    return; // ออกจากเมธอดเพื่อหยุดการดำเนินการ
                                }
                            }
                            else
                            {
                                // หากไม่พบคีย์ใน Dictionary ให้กำหนด QTY เป็น 0
                                row["QTY"] = 0;

                            }
                        }
                        foreach (DataRow row in dtQueryValue.Rows)
                        {
                            string partNumber = row["ShortChar08"].ToString();
                            string bin = row["BIN"].ToString();
                            string wh = row["WH"].ToString();
                            double number04Double = double.Parse(row["Number04"].ToString());
                            int number04 = (int)number04Double;
                            string key2 = $"{partNumber}_{bin}_{wh}";

                            if (number04 != null)
                            {
                                Label9.Text = Convert.ToString(number04);
                            }

                            if (partNumberCounts2.ContainsKey(key2))
                            {
                                int currentQty = partNumberCounts[key2];
                                hasMatchingData = true; // พบข้อมูลที่ตรงกัน
                                Label12.Text = "";
                                Label11.Text = "";
                                // ค้นหาแถวที่ตรงกับ key และเรียงลำดับตามค่า ShortChar18 จากมากไปน้อย
                                var matchingRows = dtQueryValue.AsEnumerable()
                                    .Where(r =>
                                        r["ShortChar08"].ToString() == partNumber &&
                                        r["BIN"].ToString() == bin &&
                                        r["WH"].ToString() == wh)
                                    .OrderByDescending(r => Convert.ToInt32(r["ShortChar18"]))
                                    .ToList();
                                // เช็คจำนวนแถวที่ตรงกัน
                                bool qtyUpdated = false;
                                int qtyToUpdate = currentQty;  // เก็บค่า QTY ที่ต้องการอัปเดต

                                // ลองอัปเดตค่า QTY ตามลำดับของ ShortChar18 จากมากไปน้อย
                                foreach (var r in matchingRows)
                                {

                                    int rowShortChar18 = Convert.ToInt32(r["ShortChar18"]);
                                    int rowShortChar17 = Convert.ToInt32(r["ShortChar17"]);

                                    int rowNumber04 = Convert.ToInt32(r["Number04"]);
                                    // ตั้งค่า Label11 และ Label12 เป็นค่า ShortChar17 และ ShortChar18

                                    // ถ้าสามารถเพิ่ม QTY ในแถวนี้ได้
                                    if (qtyToUpdate <= rowNumber04)
                                    {
                                        r["Update"] = 1;  // อัปเดต QTY ของแถวที่เลือก

                                        qtyUpdated = true;
                                        // กำหนดค่า ShortChar17 และ ShortChar18 ให้กับ Label11 และ Label12 จากแถวที่ถูกอัปเดต QTY
                                        Label11.Text = r["ShortChar17"].ToString();
                                        Label12.Text = r["ShortChar18"].ToString();
                                        Label13.Text = r["ShortChar19"].ToString();

                                        break; // ออกจากลูปเมื่ออัปเดตสำเร็จ

                                    }

                                    else
                                    {
                                        // ถ้าแถวนี้ไม่สามารถรองรับ QTY ที่ต้องการได้, เก็บค่า QTY ในแถวนี้ไว้
                                        r["Update"] = 0;

                                        qtyToUpdate -= rowNumber04;  // ลดค่าที่เหลือจาก QTY ที่ต้องการอัปเดต

                                    }
                                }

                                // หากไม่สามารถอัปเดต QTY ในแถวที่ตรงกันได้ ให้แสดงข้อผิดพลาด
                                if (!qtyUpdated)
                                {
                                    string errorMessage = $"error('ไม่สามารถเพิ่มค่า QTY G3 ได้ เนื่องจากไม่มีแถวที่สามารถรองรับการเพิ่มค่าได้สำหรับ Part Num : {partNumber}')";
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);
                                    ViewState["HasError"] = true; // ตั้งค่าสถานะข้อผิดพลาดใน ViewState
                                    return; // ออกจากเมธอดเพื่อหยุดการดำเนินการ
                                }
                            }
                            else
                            {
                                // หากไม่พบคีย์ใน Dictionary ให้กำหนด QTY เป็น 0
                                row["QTY"] = 0;

                            }
                        }

                        // ตรวจสอบว่ามีข้อมูลที่ตรงกันหรือไม่
                        if (!hasMatchingData)
                        {// แสดงข้อผิดพลาด (error)
                            string errorMessage = $"error('คุณยิง Part Num 3: {Part.Text}  ไม่ตรงกับ WH หรือ BIN ที่คอนเฟิร์ม')";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);
                            DataTable dt2 = (DataTable)ViewState["GridData"];
                            int totalQty = 0; // สร้างตัวแปรเพื่อเก็บผลรวมของ Qty

                            if (dt2 != null)
                            {
                                foreach (DataRow row2 in dt2.Rows)
                                {
                                    // อ่านค่า Qty จากแถวและเพิ่มเข้าไปในผลรวม
                                    totalQty += Convert.ToInt32(row2["Qty"]);
                                }
                            }

                            DataTable dt4 = ViewState["GridData3"] as DataTable;

                            // Compare data in GridView2 with GridView4 and delete matching rows
                            if (dt4 != null)
                            {
                                string key1Value = TextBox1.Text; // Assuming "Key1" is the column name

                                for (int i = dt4.Rows.Count - 1; i >= 0; i--)
                                {
                                    string key4Value = dt4.Rows[i]["TextColumn"].ToString(); // Assuming "Key4" is the column name

                                    if (key1Value == key4Value)
                                    {
                                        dt4.Rows.RemoveAt(i);
                                    }
                                }

                                // Rebind GridView4 after removing matching rows
                                GridView4.DataSource = dt4;
                                GridView4.DataBind();

                                TextBox1.Text = "";
                            }
                            TextBox14.Text = totalQty.ToString();
                            ViewState["HasError"] = true; // ตั้งค่าสถานะข้อผิดพลาดใน ViewState

                            return; // ออกจากการทำงานของเมธอด UpdateData()
                        }
                    }
                }
            }

            // กำหนด DataSource ของ GridView3 เป็น DataTable ที่ได้จาก queryvalue
            GridView3.DataSource = dtQueryValue;
            GridView3.DataBind();
            


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command2 = new SqlCommand(query3, connection))
                {
                    command2.Parameters.AddWithValue("@Key1Value", TextBox10.Text);
                    command2.Parameters.AddWithValue("@CompanyValue", Company.Text);

                    using (SqlDataReader reader = command2.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            string script = $"error('ผิดที่นี้นะจ้ะ')";

                            // ลงทะเบียนสคริปต์ JavaScript
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", script, true);

                        }
                        else
                        {
                            string script = $"error('ไม่พบข้อมูลของ Serial: {TextBox10.Text}')";

                            // ลงทะเบียนสคริปต์ JavaScript
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", script, true);
                            GridView3.DataSource = null;
                        }
                    }
                }




                // เชื่อมต่อกับฐานข้อมูลและดึงข้อมูล
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Key1value28", TextBox10.Text);
           

                    command.Parameters.AddWithValue("@CompanyValue", Company.Text);
                    command.Parameters.AddWithValue("@Key1value", TextBox1.Text);
                    

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                               
                                bool checkBoxValue = Convert.ToBoolean(reader["CheckBox02"]);

                                if (ViewState["GridData"] != null)
                                {
                                
                                    dt = (DataTable)ViewState["GridData"];
                                    

                                }

                                else
                                {
                                    dt = new DataTable();
                                    dt.Columns.Add("LOT", typeof(string));
                                    dt.Columns.Add("Qty", typeof(int));
                                    dt.Columns.Add("PartNumber", typeof(string));
                                    dt.Columns.Add("Bin", typeof(string));
                                    dt.Columns.Add("Warehouse", typeof(string));
                                    dt.Columns.Add("Valuescan", typeof(string));
                                    dt.Columns.Add("CheckBox02Value", typeof(bool)); // เพิ่มคอลัมน์สำหรับเก็บค่า CheckBox02
                                    dt.Columns.Add("Description", typeof(string));

                                }

                                bool found = false;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow row = dt.Rows[i];

                                    if (row["LOT"].ToString() == reader["Key5"].ToString() &&
                                        row["PartNumber"].ToString() == reader["Character01"].ToString() && 
                                        row["Warehouse"].ToString() == Label5.Text && row["Bin"].ToString() == Label6.Text)
                                    {
                                        // เพิ่มจำนวน QTY ในแถวปัจจุบันของ DataTable ของเรา
                                        int currentQty = Convert.ToInt32(row["Qty"]);

                                        row["Qty"] = currentQty + 1;
                                        found = true;
                                        break;

                                    }
                                   
                                }


                                if (!found)
                                {


                                    DataRow newRow = dt.NewRow();


                                    newRow["Valuescan"] = reader["Number04"].ToString().TrimEnd('0').TrimEnd('.'); ;
                                    newRow["PartNumber"] = reader["Character01"].ToString();
                                    newRow["Warehouse"] = Label5.Text;
                                    newRow["Bin"] = Label6.Text;
                                    newRow["LOT"] = reader["Key5"].ToString();
                                    newRow["CheckBox02Value"] = reader["CheckBox02"].ToString();
                                    newRow["Description"] = reader["Character09"].ToString();

                                    newRow["Qty"] = 1;

                                    dt.Rows.Add(newRow);

                                }

                                ViewState["GridData"] = dt;
                                GridView1.DataSource = dt;
                                GridView1.DataBind();


                            }
                            DataTable dt2 = (DataTable)ViewState["GridData"];

                            // วนลูปผ่านแต่ละแถวใน DataTable
                            int totalQty = 0; // สร้างตัวแปรเพื่อเก็บผลรวมของ Qty
                            foreach (DataRow row2 in dt2.Rows)
                            {
                                // อ่านค่า Qty จากแถวและเพิ่มเข้าไปในผลรวม
                                totalQty += Convert.ToInt32(row2["Qty"]);
                            }

                            // กำหนดค่าผลรวมให้กับ TextBox14
                            TextBox14.Text = totalQty.ToString();
                        }
                        else
                        {
                            //หากไม่พบข้อมูล ให้แสดงข้อความแจ้งเตือน
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('ไม่พบข้อมูลของ Serial:'"+TextBox10.Text+")", true);

                            DataTable dt2 = (DataTable)ViewState["GridData"];

                            int totalQty = 0; // สร้างตัวแปรเพื่อเก็บผลรวมของ Qty
                            if (dt2 != null)
                            {
                                foreach (DataRow row2 in dt2.Rows)
                                {
                                    // อ่านค่า Qty จากแถวและเพิ่มเข้าไปในผลรวม
                                    totalQty += Convert.ToInt32(row2["Qty"]);
                                }
                            }
                            DataTable dt4 = ViewState["GridData3"] as DataTable;

                            // Compare data in GridView2 with GridView4 and delete matching rows
                            if (dt4 != null)
                            {

                                string key1Value = TextBox1.Text; // Assuming "Key1" is the column name

                                for (int i = dt4.Rows.Count - 1; i >= 0; i--)
                                {
                                    string key4Value = dt4.Rows[i]["TextColumn"].ToString(); // Assuming "Key4" is the column name

                                    if (key1Value == key4Value)
                                    {
                                        dt4.Rows.RemoveAt(i);
                                    }

                                }

                                // Rebind GridView4 after removing matching rows
                                GridView4.DataSource = dt4;
                                //    GridView4.DataBind();
                               
                                TextBox1.Text = "";
                            }
                            TextBox14.Text = totalQty.ToString();

                            return;
                        }
                    }
                    connection.Close(); // ปิดการเชื่อมต่อกับฐานข้อมูล
                }


            }

            TextBox1.Text = "";

            GridView7.DataSource = ViewState["GridData3"];
            GridView7.DataBind();
        }
        protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!string.IsNullOrEmpty(Label14.Text) && !string.IsNullOrEmpty(Label15.Text) && !string.IsNullOrEmpty(Label16.Text))
            {
                Label11.Text = Label14.Text;
                Label12.Text = Label15.Text;
                Label13.Text = Label16.Text;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime date = DateTime.ParseExact(TextBox5.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string formattedDate = date.ToString("yyyy-MM-dd");

                try
                {
                    string key2Value = CleanValue(e.Row.Cells[1].Text);
                    bool isDuplicate = false;

                    // ตรวจสอบข้อมูล Key2 ในฐานข้อมูล
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string checkQuery = "SELECT COUNT(*) FROM ice.Ud15 WHERE Key2 = @Key2";
                        using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                        {
                            checkCommand.Parameters.AddWithValue("@Key2", key2Value);
                            int count = (int)checkCommand.ExecuteScalar();
                            isDuplicate = count > 0;
                        }
                    }

                    if (isDuplicate)
                    {
                        // แจ้งเตือนเมื่อพบข้อมูลซ้ำ
                        string errorMessage = $"error('คุณสแกน Serail No: {TextBox1.Text} ซ้ำ');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMessage, true);
                        return; // ยกเลิกการบันทึกถ้าพบข้อมูลซ้ำ
                    }
                    string deviceType = Request.UserAgent.ToLower().Contains("mobi") ? "M" : "C";

                    // ดำเนินการบันทึกข้อมูลถ้าไม่มีข้อมูลซ้ำ
                    var requestData = new
                    {
                        BPI_Plant_c = Request.QueryString["site"],
                        Company = Request.QueryString["company"],
                        Key1 = CleanValue(e.Row.Cells[10].Text),
                        Key2 = key2Value,
                        Key3 = CleanValue(e.Row.Cells[0].Text),
                        Key5 = "Transfer",
                        ShortChar17 = Label11.Text,
                        ShortChar18 = Label12.Text,
                        ShortChar19 = Label13.Text,
                        BPI_ProjectID_c = CleanValue(e.Row.Cells[10].Text),
                        BPI_ProjectDesc_c = CleanValue(e.Row.Cells[11].Text),
                        BPI_PartNum_c = CleanValue(e.Row.Cells[2].Text),
                        BPI_PickingList_c = CleanValue(TextBox10.Text),
                        BPI_PartDescription_c = CleanValue(e.Row.Cells[8].Text),
                        BPI_DeliveryOrder_c = CleanValue(e.Row.Cells[12].Text),
                        BPI_UOM_c = CleanValue(e.Row.Cells[9].Text),
                        BPI_TranDate_c = formattedDate,
                        BPI_Loaded_c = "W",
                        BPI_CreateDate_c = DateTime.Now.ToString("yyyy-MM-dd"),  // ผลลัพธ์: 13/11/2024
                        BPI_EmpID_c = "IT000005",
                        BPI_CreateUser_c = CleanValue(Label4.Text),
                        BPI_CreateTime_c = DateTime.Now.ToString("HH:mm:ss"),
                        BPI_BinFrom_c = CleanValue(e.Row.Cells[5].Text),
                        BPI_WarehouseFrom_c = CleanValue(e.Row.Cells[4].Text),
                        BPI_WareHouseTo_c = CleanValue(e.Row.Cells[6].Text),
                        BPI_BinTo_c = CleanValue(e.Row.Cells[7].Text),
                        BPI_LotNum_c = "LOT-00000120221231",
                        BPI_TranQty_c = "1",
                        ShortChar10 = CleanValue(e.Row.Cells[3].Text),
                        CheckBox01 = false,
                        //Character01 = deviceType, // เก็บค่า C หรือ M ใน Character01

                        CheckBox02 = false
                    };

                    using (HttpClient client = new HttpClient())
                    {
                        string token = Request.QueryString["token"];
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        string apiUrl = "https://erp.bpi-concretepile.co.th/BPI_Live/api/v1/Ice.BO.UD15Svc/UD15s/";
                        string jsonData = JsonConvert.SerializeObject(requestData);
                        HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                        Task.Run(async () =>
                        {
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                            if (response.IsSuccessStatusCode)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "successMes", $"successmes('บันทึกข้อมูล Picking No {TextBox10.Text} เรียบร้อยแล้ว');", true);
                                foreach (GridViewRow row in GridView1.Rows)
                                {
                                    LinkButton deleteButton = (LinkButton)row.FindControl("DeleteButton");
                                    deleteButton.Visible = false;
                                    Label14.Text = "";
                                    Label15.Text = "";
                                    Label16.Text = "";
                                }
                            }
                            else
                            {
                                string errorDetails = await response.Content.ReadAsStringAsync();
                                string errorMsg = $"error('บันทึกข้อมูลไม่สำเร็จ: {errorDetails.Replace("'", "\\'")}');";
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", errorMsg, true);
                            }

                        }).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            }
        }

        protected void TextBoxClear_TextChanged(object sender, EventArgs e)
        {
            Label5.Text = "";
            Label6.Text = "";
            whare_TextChanged(sender, e);


        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Calculate sequence number starting from 1
                int rowIndex = e.Row.RowIndex;
                int sequenceNumber = rowIndex + 2; // Starting from 1

                // Find the Label control and set its value
                Label lblSeq = (Label)e.Row.FindControl("lblSeq");
                if (lblSeq != null)
                {
                    lblSeq.Text = sequenceNumber.ToString();
                }
                // Find the delete button in the current row
                LinkButton deleteButton = (LinkButton)e.Row.FindControl("DeleteButton");

                // Find the value of CheckBox02Value in the current row
                bool checkBoxValue = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "CheckBox02Value"));

                // Hide the delete button if CheckBox02Value is true
                if (checkBoxValue)
                {
                    deleteButton.Visible = false;
                    Button2.Visible = false;
                    Button1.Visible = false;
                }
                else 
                {
                    Button2.Visible = true;
                    Button1.Visible = true;

                }
            }
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataTable dt = ViewState["GridData"] as DataTable;

                GridViewRow row = GridView1.Rows[rowIndex];

                if (dt != null && dt.Rows.Count > rowIndex)
                {
                    string deletedPartNumber = row.Cells[1].Text; // Assuming the PartNumber is in the second column
                    string deletedQty = dt.Rows[rowIndex]["Qty"].ToString(); // Assuming "Qty" is the column name

                    // Set the deleted quantity value to Label3
                    Label3.Text = deletedQty;

                    // Delete the row from the DataTable
                    dt.Rows[rowIndex].Delete();
                    dt.AcceptChanges(); // Ensure changes are committed
                    ViewState["GridData"] = dt;

                    // Update ColumnQty in GridView5
                    UpdateColumnQty(deletedPartNumber);

                    // Calculate new quantity value
                    int newQtyValue = CalculateNewQty(dt);
                    TextBox14.Text = newQtyValue.ToString();

                    // Rebind GridView1
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    // Update data in GridView2 based on SQL query
                    string value1 = row.Cells[1].Text;
                    string value2 = Company.Text;
                    string value3 = TextBox10.Text;
                    string value4 = row.Cells[3].Text;
                    string value5 = row.Cells[4].Text;
                    string value6 = row.Cells[5].Text;

                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sqlQuery = @"SELECT DISTINCT UD03.Key1 ,UD03.Character01
                   FROM ice.ud03 AS UD03 
                   JOIN Ice.UD28 AS UD28 ON UD03.Character01 = UD28.ShortChar08 
                   WHERE UD03.Character01 = @Column2
                   AND UD03.Company = @Company
                   AND UD28.Key1 = @Key1
                   AND UD03.Key5 = @ColumnLot
                   
                  GROUP BY UD03.Key1, UD03.Key2,UD03.Character01, UD03.Key5, UD03.Key4, UD28.ShortChar10, UD28.ShortChar09, UD03.Key3";

                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@Column2", value1); // Provide value1
                        command.Parameters.AddWithValue("@Company", value2); // Provide value2
                        command.Parameters.AddWithValue("@Key1", value3); // Provide value3
                        command.Parameters.AddWithValue("@ColumnLot", value4); // Provide value4
                        command.Parameters.AddWithValue("@WH", value5); // Provide value4
                        command.Parameters.AddWithValue("@BIN", value6); // Provide value4

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dtResults = new DataTable();
                            dtResults.Load(reader);

                            GridView2.DataSource = dtResults;
                            GridView2.DataBind();

                            DataTable dt4 = ViewState["GridData3"] as DataTable;

                            if (dt4 != null)
                            {
                                foreach (DataRow row2 in dtResults.Rows)
                                {
                                    string key1Value = row2["Key1"].ToString(); // Assuming "Key1" is the column name

                                    for (int i = dt4.Rows.Count - 1; i >= 0; i--)
                                    {
                                        string key4Value = dt4.Rows[i]["TextColumn"].ToString(); // Assuming "TextColumn" is the column name

                                        if (key1Value == key4Value)
                                        {
                                            dt4.Rows.RemoveAt(i);
                                        }
                                    }
                                }

                                // Reorder the No column in GridView4
                                ReorderGridView4(dt4);

                                // Rebind GridView4 after removing matching rows
                                GridView4.DataSource = dt4;
                                GridView4.DataBind();
                            }
                        }
                    }
                }
                TextBox1.Text = "";
            }
        }

        private void ReorderGridView4(DataTable dt)
        {
            int no = 1;
            foreach (DataRow row in dt.Rows)
            {
                row["No"] = no++;
            }
        }

        private void UpdateColumnQty(string deletedPartNumber)
        {
            if (ViewState["GridData5"] != null && ViewState["GridData5"] is DataTable)
            {
                DataTable dtGrid5 = (DataTable)ViewState["GridData5"];
                DataTable dt = (DataTable)ViewState["GridData"];
                // Loop through each row in GridView5
                foreach (DataRow row in dtGrid5.Rows)
                {

                    if (row["Key2"].ToString() == deletedPartNumber)
                    {
                        // Decrease the ColumnQty value by 1
                        int currentQty = Convert.ToInt32(row["ColumnQty"]);
                        int Qty = Convert.ToInt32(Label3.Text);
                        if (currentQty > 0)
                        {
                            row["ColumnQty"] = currentQty - Qty;
                        }


                    }
                }

                // Rebind GridView5
                GridView5.DataSource = dtGrid5;
                GridView5.DataBind();
            }
        }

        private int CalculateNewQty(DataTable dt)
        {
            int newQtyValue = 0;
            foreach (DataRow row in dt.Rows)
            {
                newQtyValue += Convert.ToInt32(row["Qty"]);

            }
            return newQtyValue;
        }
        // ประกาศฟังก์ชัน showLoadingSpinner ในคลาส

        protected void BindPopupGridView()
        {
            // Your logic to bind data to popupGridView
            // Example:
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Age", typeof(int));

            // Add an empty row to ensure headers are shown even if no data
            dt.Rows.Add(dt.NewRow());

            // Bind the dummy DataTable to GridView
            popupGridView.DataSource = dt;
            popupGridView.DataBind();
        }

        protected void popupTextBox_TextChanged(object sender, EventArgs e)
        {
            string Packing = TextBox10.Text;
            string inputValue = popupTextBox.Text.Trim();
            Label8.Text = popupTextBox.Text;
            popupTextBox.Text = "";

            // ตรวจสอบว่ามีข้อมูลที่ป้อนเข้ามาหรือไม่
            if (!string.IsNullOrEmpty(inputValue))
            {
                string query = "SELECT Company,Key1,Key2,Key3,Key5,ShortChar10 FROM ice.UD15 WHERE Key2 = @Key2 and Key5 = 'Transfer' and SysRowID in (Select ForeignSysRowID from ice.UD15_UD where BPI_PickingList_c = @Key1)";

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Key2", inputValue);
                    command.Parameters.AddWithValue("@Key1", Packing);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();

                    connection.Open();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        popupGridView.DataSource = dt;
                        popupGridView.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "showPopup", "showPopup();", true);
                    }
                    else
                    {
                        // แจ้งว่าไม่พบข้อมูล
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('ไม่พบข้อมูล');", true);
                    }
                }
            }
        }
        [System.Web.Services.WebMethod]
        public static void DeleteDataall(string PickingLis, string Company)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Delete from ice.ud15 where SysRowID in (Select ForeignSysRowID from ice.UD15_UD where BPI_PickingList_c = @PickingLis)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PickingLis", PickingLis);
                    command.Parameters.AddWithValue("@Company", Company);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception
                        throw new Exception("Error deleting data: " + ex.Message);
                    }
                }
            }
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            // Logic for Button2 click event
            // Example: Delete data or any other action
            // Refresh popupGridView after action
            BindPopupGridView();
        }
        protected void Button5_Click1(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static void DeleteData(string Key2, string textBox10Value ,string company)
        {
            // เชื่อมต่อกับฐานข้อมูล
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // สร้างคำสั่ง SQL สำหรับลบข้อมูล
                string query = "Delete from ice.ud15  where SysRowID in (Select ForeignSysRowID from ice.UD15_UD where  BPI_PickingList_c =@PickingLis )and Key2 = @Key2 and Company = @Company";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // เพิ่มพารามิเตอร์และกำหนดค่า
                    command.Parameters.AddWithValue("@Key2", Key2);
                    command.Parameters.AddWithValue("@PickingLis", textBox10Value);
                    command.Parameters.AddWithValue("@Company", company);

                    try
                    {
                        // เปิดการเชื่อมต่อและดำเนินการลบข้อมูล
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // ตรวจสอบว่ามีการลบข้อมูลสำเร็จหรือไม่
                        if (rowsAffected > 0)
                        {
                            // สามารถเพิ่มการแจ้งเตือนหรือการปรับปรุง UI อื่น ๆ ได้ตามต้องการ

                        }
                        else
                        {
                            // กรณีที่ไม่พบข้อมูลที่ต้องการลบ
                        }
                    }
                    catch (Exception ex)
                    {
                        // จัดการข้อผิดพลาดที่เกิดขึ้นในการเชื่อมต่อฐานข้อมูลหรือการดำเนินการลบข้อมูล
                        Console.WriteLine("Error deleting data: " + ex.Message);
                        throw; // ส่งข้อผิดพลาดกลับไปยัง AJAX สำหรับการจัดการข้อผิดพลาดใน JavaScript
                    }
                }
            }
        }
        protected void popupCloseButton_Click1(object sender, EventArgs e)
        {
            // Logic for Button2 click event
            // Example: Delete data or any other action
            // Refresh popupGridView after action
            Label8.Text = "";
            popupGridView.DataSource = null;
            popupGridView.DataBind();


        }

        private void CompareTextBoxValues()
        {
            // ตรวจสอบว่าค่าของ TextBox13 มากกว่า TextBox14 หรือไม่
            if (String.Compare(TextBox13.Text, TextBox14.Text) > 0)
            {
                // หากเป็นเช่นนั้น ให้แสดงข้อความแจ้งเตือนให้ผู้ใช้ทราบ
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('ยังรับค่ามาไม่ครบครับ')", true);
                return;

            }
            else if (String.Compare(TextBox13.Text, TextBox14.Text) < 0)
            {
                // หาก TextBox13 มีค่าน้อยกว่า TextBox14
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('ค่าของ ที่รับมา น้อยกว่า ที่ต้องส่งครับ')", true);
                return;

            }




        }

        protected void TextBox_TextChanged(object sender, EventArgs e)
        {

            if (Label5.Text != "")
            {
                // ตรวจสอบว่า Label6.Text ต้องตรงกับค่าในแถวเดียวกับ Label5.Text ใน GridView6
                bool BInmat = false;
                foreach (GridViewRow row in GridView6.Rows)
                {
                    if (row.Cells[7].Text == Label5.Text && row.Cells[8].Text == Bin.Text)
                    {
                        Label6.Text = Bin.Text;
                        Bin.Text = "";
                        TextBox6.Text = "";

                        BInmat = true;
                        break;
                    }
                }

                if (!BInmat)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('Bin ไม่ตรงครับ')", true);
                    Bin.Text = "";
                    Label6.Text = "";
                    TextBox6.Text = "";
                    return;
                }
            }
            else
            {
                Label6.Text = Bin.Text;
                Bin.Text = "";
                bool binMatched = false; // สร้างตัวแปรเพื่อตรวจสอบว่าพบข้อมูลที่ตรงค่าในคอลัมน์ Key4 หรือไม่

                for (int a = 0; a < GridView6.Rows.Count; a++)
                {
                    if (Label6.Text == GridView6.Rows[a].Cells[8].Text)
                    {
                        binMatched = true; // กำหนดค่า binMatched เป็น true หากพบข้อมูลที่ตรงค่าในคอลัมน์ Key4
                        break; // ออกจากลูป for เพื่อไม่ต้องทำการตรวจสอบในแถวอื่นอีก
                    }
                }

                if (!binMatched)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('Bin ไม่ตรงครับ')", true);

                    Label6.Text = "";
                    TextBox6.Text = "";

                    return;
                }


            }

        }

        protected void whare_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Warehouse.Text))
            {
                if (!string.IsNullOrEmpty(Label6.Text))
                {
                    // ตรวจสอบว่า Label6.Text ต้องตรงกับค่าในแถวเดียวกับ Label5.Text ใน GridView6
                    bool BInmat = false;
                    for (int a = 0; a < GridView6.Rows.Count; a++)
                    {
                        if (GridView6.Rows[a].Cells[7].Text == Warehouse.Text && GridView6.Rows[a].Cells[8].Text == Label6.Text)
                        {
                            Label5.Text = Warehouse.Text;
                            Warehouse.Text = "";
                            TextBox6.Text = "";
                            BInmat = true;
                            break;
                        }
                    }

                    if (!BInmat)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('Warehouse ไม่ตรงครับ')", true);
                        Warehouse.Text = "";
                        TextBox6.Text = "";

                        Label5.Text = "";
                        return;
                    }
                }
                else
                {
                    Label5.Text = Warehouse.Text;
                    Warehouse.Text = "";
                    bool WHMatched = false; // สร้างตัวแปรเพื่อตรวจสอบว่าพบข้อมูลที่ตรงค่าในคอลัมน์ Key4 หรือไม่

                    for (int a = 0; a < GridView6.Rows.Count; a++)
                    {
                        if (Label5.Text == GridView6.Rows[a].Cells[7].Text)
                        {
                            WHMatched = true; // กำหนดค่า binMatched เป็น true หากพบข้อมูลที่ตรงค่าในคอลัมน์ Key4
                            break; // ออกจากลูป for เพื่อไม่ต้องทำการตรวจสอบในแถวอื่นอีก
                        }
                    }

                    if (!WHMatched)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error('Warehouse ไม่ตรงครับ')", true);
                        TextBox6.Text = "";

                        Label5.Text = "";
                        return;
                    }
                }
            }
        }

        private string CleanValue(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "&nbsp;")
            {
                return "";
            }
            return value;
        }
        //protected async void Button4_Click1(object sender, EventArgs e)
        //{

        //    Label5.Text = "";
        //    Label6.Text = "";
        //}
        //protected async void Button3_Click1(object sender, EventArgs e)
        //{
        //    DateTime date = DateTime.ParseExact(TextBox5.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    string formattedDate = date.ToString("yyyy-MM-dd");

         
        //    try
        //    {
        //        // วนลูป GridView4 เพื่อดึงข้อมูลที่ต้องการโอนย้าย
        //        foreach (GridViewRow row4 in GridView4.Rows)
        //        {
        //            var requestData = new
        //            {
        //                BPI_Plant_c = Request.QueryString["site"],
        //                Company = Request.QueryString["company"],
        //                Key1 = CleanValue(row4.Cells[10].Text),
        //                Key2 = CleanValue(row4.Cells[1].Text),
        //                Key3 = CleanValue(row4.Cells[0].Text),
        //                Key5 = "Transfer",
        //                BPI_ProjectID_c = CleanValue(row4.Cells[10].Text),
        //                BPI_ProjectDesc_c = CleanValue(row4.Cells[11].Text), ////
        //                BPI_PartNum_c = CleanValue(row4.Cells[2].Text),
        //                BPI_PickingList_c = CleanValue(TextBox10.Text),
        //                BPI_PartDescription_c = CleanValue(row4.Cells[8].Text),
        //                BPI_DeliveryOrder_c = CleanValue(row4.Cells[12].Text),
        //                BPI_UOM_c = CleanValue(row4.Cells[9].Text),
        //                BPI_TranDate_c = formattedDate,
        //                BPI_Loaded_c = "T",
        //                BPI_CreateDate_c = formattedDate,
        //                BPI_EmpID_c = "IT000005",
        //                BPI_CreateUser_c = CleanValue(Label4.Text),
        //                BPI_CreateTime_c = DateTime.Now.ToString("HH:mm:ss"),
        //                BPI_BinFrom_c = CleanValue(row4.Cells[5].Text),
        //                BPI_WarehouseFrom_c = CleanValue(row4.Cells[4].Text),
        //                BPI_WareHouseTo_c = CleanValue(row4.Cells[6].Text),
        //                BPI_BinTo_c = CleanValue(row4.Cells[7].Text),
        //                BPI_LotNum_c = "LOT-00000120221231",
        //                BPI_TranQty_c = "1",
        //                ShortChar10 = CleanValue(row4.Cells[3].Text),
        //                CheckBox01 = false,
        //                CheckBox02 = false
        //            };

        //            // สร้าง HttpClient ในแต่ละรอบ
        //            using (HttpClient client = new HttpClient())
        //            {
        //                string token = Request.QueryString["token"];
        //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //                string apiUrl = "https://erp.bpi-concretepile.co.th/BPI_Live/api/v1/Ice.BO.UD15Svc/UD15s/";

        //                // แปลง JSON object เป็น JSON string
        //                string jsonData = JsonConvert.SerializeObject(requestData);
        //                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        //                // ทำ HTTP POST request ไปยัง API
        //                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

        //                if (response.IsSuccessStatusCode)
        //                {
        //                    string responseBody = await response.Content.ReadAsStringAsync();
        //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "successmes", "successmes('โอนย้าย Picking No" + TextBox10.Text + "เรียบร้อยแล้ว')", true);

        //                    foreach (GridViewRow row in GridView1.Rows)
        //                    {
        //                        LinkButton deleteButton = (LinkButton)row.FindControl("DeleteButton");
        //                        deleteButton.Visible = false;
        //                    }
        //                }
        //                else if (response.StatusCode == HttpStatusCode.Conflict)
        //                {
        //                    // ข้ามการโพสต์ข้อมูลแถวนี้แล้วลงไปในการวนลูปถัดไปใน GridView4
        //                    continue;
        //                }
        //                else
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "successmes", "successmes('บันทึกข้อมูล Picking No" + TextBox10.Text + "เรียบร้อยแล้ว')", true);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("Error: " + ex.Message);
        //    }
        //}
        protected async void Button5_Click(object sender, EventArgs e)
        {
           
        }



    }


}

 


