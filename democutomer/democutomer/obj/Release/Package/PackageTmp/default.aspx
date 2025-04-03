<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="democutomer.main" %>

<!DOCTYPE html>
<html lang="en">
<head>
<title>โปรแกรมยิ่งเข็มขึ้นรถ</title>

<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
<link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;700&display=swap" rel="stylesheet">
<link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Poppins">
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet">
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet">
<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

     <script type="text/javascript">
         function Showerror() {
             Swal.fire({
                 icon: 'error', // กำหนดไอคอนเป็นข้อผิดพลาด
                 title: 'ไม่มีข้อมูล', // ข้อความแสดงในป็อปอัพ
                 showConfirmButton: true, // แสดงปุ่มยืนยัน
                 confirmButtonColor: '#3085d6', // สีของปุ่มยืนยัน
                 confirmButtonText: 'OK' // ข้อความบนปุ่มยืนยัน
             });
         }
         function confirmDelete() {
             Swal.fire({
                 icon: 'warning',
                 title: 'ยืนยันลบข้อมูล',
                 showConfirmButton: true,
                 showCancelButton: true,
                 confirmButtonColor: '#3085d6',
                 cancelButtonColor: '#d33',
                 confirmButtonText: 'ใช่, ฉันต้องการลบ',
                 cancelButtonText: 'ยกเลิก'
             }).then((result) => {
                 if (result.isConfirmed) {
                     // ถ้าผู้ใช้กด "ใช่, ฉันต้องการลบ"
                     deleteData();
                 } else {
                     // ถ้าผู้ใช้กด "ยกเลิก" หรือปิดหน้าต่างแจ้งเตือน
                     // ไม่ต้องทำอะไรเพิ่มเติม
                 }
             });
         }
         function deleteData() {
             // เรียกใช้งาน API หรือฟังก์ชันใน C# ที่ใช้สำหรับการลบข้อมูล
             var key2Value = '<%= Label8.Text.Trim() %>';
             var postData = { Key2: key2Value };
             // เก็บค่า TextBox10 ลงใน localStorage
             var textBox10Value = $('#TextBox10').val();
             var label5Value = $('#<%= Label5.ClientID %>').text().trim();
             var label6Value = $('#<%= Label6.ClientID %>').text().trim();
             localStorage.setItem('Label5Value', label5Value);
             localStorage.setItem('Label6Value', label6Value);
             localStorage.setItem('TextBox10Value', textBox10Value);


             // ส่งข้อมูลไปยังเซิร์ฟเวอร์ด้วย AJAX หรือเรียกใช้งานหน้าเว็บเพื่อลบข้อมูล
             $.ajax({
                 type: 'POST',
                 url: 'default.aspx/DeleteData', // ต้องแก้ไข URL ให้ตรงกับหน้า ASPX ของคุณ
                 data: JSON.stringify(postData),
                 contentType: 'application/json; charset=utf-8',
                 dataType: 'json',
                 success: function (response) {
                     // ทำบางสิ่งหลังจากลบข้อมูลเช่น แสดงข้อความสำเร็จหรือรีเฟรชข้อมูล
                     Swal.fire({
                         icon: 'success',
                         title: 'ลบข้อมูลเรียบร้อย',
                         showConfirmButton: false,
                         timer: 1500
                     }).then(() => {
                         // รีเฟรชหน้าหรือทำอย่างอื่นตามที่ต้องการ
                         $('#<%= popupGridView.ClientID %>').empty(); // เคลียร์ข้อมูลใน GridView ด้วย jQuery
                         closePopup();

                         window.location.href = window.location.href; // โหลดหน้าใหม่



                     });
                 },
                 error: function (error) {
                     // กรณีเกิดข้อผิดพลาดในการลบข้อมูล
                     console.error('Error deleting data:', error);
                     Swal.fire({
                         icon: 'error',
                         title: 'เกิดข้อผิดพลาดในการลบข้อมูล',
                         text: 'กรุณาลองใหม่อีกครั้ง',
                         confirmButtonColor: '#3085d6',
                         confirmButtonText: 'ตกลง'
                     });
                 }
             });
         }
         function Showerrormax() {
             Swal.fire({
                 icon: 'error', // กำหนดไอคอนเป็นข้อผิดพลาด
                 title: 'เกินกำหนดครับ', // ข้อความแสดงในป็อปอัพ
                 showConfirmButton: true, // แสดงปุ่มยืนยัน
                 confirmButtonColor: '#3085d6', // สีของปุ่มยืนยัน
                 confirmButtonText: 'OK' // ข้อความบนปุ่มยืนยัน
             });
         }
         function Showerrordub() {
             Swal.fire({
                 icon: 'error', // กำหนดไอคอนเป็นข้อผิดพลาด
                 title: 'สแกนซ้ำครับ', // ข้อความแสดงในป็อปอัพ
                 showConfirmButton: true, // แสดงปุ่มยืนยัน
                 confirmButtonColor: '#3085d6', // สีของปุ่มยืนยัน
                 confirmButtonText: 'OK' // ข้อความบนปุ่มยืนยัน
             });
         }
         function error(message) {
             Swal.fire({
                 icon: 'error', // กำหนดไอคอนเป็นข้อผิดพลาด
                 title: message, // กำหนดหัวข้อของแจ้งเตือน
                 showConfirmButton: true, // แสดงปุ่มยืนยัน
                 confirmButtonColor: '#3085d6', // สีของปุ่มยืนยัน
                 confirmButtonText: 'OK' // ข้อความบนปุ่มยืนยัน
             });
         }
         function EndTime(message) {
             Swal.fire({
                 icon: 'error', // กำหนดไอคอนเป็นข้อผิดพลาด
                 title: message, // กำหนดหัวข้อของแจ้งเตือน
                 showConfirmButton: true, // แสดงปุ่มยืนยัน
                 confirmButtonColor: '#3085d6', // สีของปุ่มยืนยัน
                 confirmButtonText: 'OK' // ข้อความบนปุ่มยืนยัน
             });
         }
         function success() {
             Swal.fire({
                 icon: 'success', // กำหนดไอคอนเป็นข้อผิดพลาด
                 title: 'บันทึกข้อมูลเรียบรอ้ยแล้ว', // กำหนดหัวข้อของแจ้งเตือน
                 showConfirmButton: true, // แสดงปุ่มยืนยัน
                 confirmButtonColor: '#3085d6', // สีของปุ่มยืนยัน
                 confirmButtonText: 'OK' // ข้อความบนปุ่มยืนยัน
             });
         }
         function successBIN() {
             Swal.fire({
                 icon: 'success', // กำหนดไอคอนเป็นข้อผิดพลาด
                 title: 'BIN กับ Warehouse ถูกต้อง', // กำหนดหัวข้อของแจ้งเตือน
                 showConfirmButton: false, // ไม่แสดงปุ่มยืนยัน
                 timer: 2000, // หน่วงเวลา 2 วินาที
                 timerProgressBar: true // แสดงแถบหน่วงเวลา
             });
         }
         function Warrning(message) {
             Swal.fire({
                 icon: 'warning', // กำหนดไอคอนเป็นเครื่องหมายคำเตือน
                 title: message, // กำหนดหัวข้อของแจ้งเตือน
                 showConfirmButton: true, // แสดงปุ่มยืนยัน
                 showCancelButton: true, // แสดงปุ่มยกเลิก
                 confirmButtonColor: '#3085d6', // สีของปุ่มยืนยัน
                 cancelButtonColor: '#d33', // สีของปุ่มยกเลิก
                 confirmButtonText: 'ใช่, ฉันต้องการลบ', // ข้อความบนปุ่มยืนยัน
                 cancelButtonText: 'ยกเลิก' // ข้อความบนปุ่มยกเลิก
             });
         }
         function successmes(message) {
             Swal.fire({
                 icon: 'success', // กำหนดไอคอนเป็นข้อผิดพลาด
                 title: message, // กำหนดหัวข้อของแจ้งเตือน
                 showConfirmButton: false, // ไม่แสดงปุ่มยืนยัน
                 timer: 2000, // หน่วงเวลา 2 วินาที
                 timerProgressBar: true // แสดงแถบหน่วงเวลา
             });
         }
     </script>
<style>
    .fixed-header {
    position: sticky;
    top: 0;
    z-index: 1;
    background-color: #fff; /* สีพื้นหลังของหัวตาราง */
}
    .full-width {
    width: 100%;
}
body,h1,h2,h3,h4,h5 {font-family: "Poppins", sans-serif}
body {font-size:16px;}
.w3-half img{margin-bottom:-6px;margin-top:16px;opacity:0.8;cursor:pointer}
.w3-half img:hover{opacity:1}
/* CSS สำหรับเนื้อหาหลัก */
/* เปลี่ยนสีเป็นสีเทา */
.w3-Back {
    background-color: #f2f2f2; /* สีพื้นหลังของ container */
    padding: 20px; /* ระยะห่างของข้อความภายใน container */
}
.w3-sidebar {
    background-color: #f2f2f2; /* เปลี่ยนสีเป็นสีเทา */
    box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2); /* เพิ่มเงา */
}
 .textbox-container {
        width: 100%;
    }
.responsive-textbox {
        width: 100%;
    }
   @media (max-width: 768px) {
        .responsive-textbox {
            width: 50%;
        }
         .textbox-container {
        width: 50%;
            }
    }
 .delivery-date {
        font-size: 20px; /* ปรับขนาดตามที่ต้องการ */
    }
  .custom-calendar {
        width: 200px; /* กำหนดความกว้างของ Calendar */
        font-size: 14px; /* กำหนดขนาดของตัวอักษรใน Calendar */
    }
  @media screen and (max-width: 780px) {
      .display-4 {
      font-size: 24px; /* ปรับขนาดข้อความให้เล็กลง */
  }
    
            .w3-main {
                margin-left: 0; /* ยกเลิกการให้ขอบซ้าย */
                margin-right: 0; /* ยกเลิกการให้ขอบขวา */
            }

            /* ปรับขนาดข้อความในหัวข้อใหญ่ */
            .w3-jumbo {
                font-size: 24px;
            }

            /* ปรับขนาดข้อความในส่วน delivery-date */
            .delivery-date {
                font-size: 14px;
            }

            /* ปรับขนาดของ TextBox และ DropDownList */
            #DropDownList2,
            #DropDownList3,
            #DropDownList4,
            #TextBox2,
            #TextBox1 {
                width: 100%; /* กว้างเต็มระบบ */
            }
    #TextBox10
    {
        width:150%;
    }
    
    #Carid 
    {
        width:70%;
    }
    #Company
{
    width:80%;
}
            #TextBox5
{
    width:144%;
}
    #calendarIcon
    {
        right: -70px;
    }
            #TextBox3,
            #TextBox4
            {
                font-size: 14px;
            }
             #TextBox6,
             #TextBox7,
             #TextBox8
          
            {
                 width: 100%; /* กว้างเต็มระบบ */
            }
            /* ปรับขนาดของ GridView */
            #GridView1 {
                width: 100%; /* กว้างเต็มระบบ */
                font-size: 12px; /* ขนาดข้อความเล็กลง */
            }
        }
  @media screen and (min-width: 600px) {
      .display-4 {
            font-size: 24px; /* ปรับขนาดข้อความให้เล็กลง */
        }
      .table-responsive {
        width: 100%;
        margin-bottom: 15px;
        overflow-x: scroll;
        overflow-y: hidden;
        -ms-overflow-style: -ms-autohiding-scrollbar;
        border: 1px solid #dddddd;
        -webkit-overflow-scrolling: touch;
    }
      .table-responsive > .table {
        margin-bottom: 0;
    }

    .table-responsive > .table > thead > tr > th,
    .table-responsive > .table > tbody > tr > th,
    .table-responsive > .table > tfoot > tr > th,
    .table-responsive > .table > thead > tr > td,
    .table-responsive > .table > tbody > tr > td,
    .table-responsive > .table > tfoot > tr > td {
        white-space: nowrap;
    }
    /* ปรับขนาดของ TextBox และ DropDownList เมื่อหน้าจอมีขนาดใหญ่ขึ้น */
    #TextBox5,
    #DropDownList2,
    #TextBox9,
    #DropDownList4,
    #TextBox2,
    #TextBox1,
    #TextBox3,
    #TextBox4,
    #TextBox6,
    #TextBox7,

    #TextBox8 {
        width: 100%;
    }
    #Button1,
    #Button2,
    #Button3,
    #Button5
    {
                width: 25%;


    }
    #Button4 {
                        width: 25%;

    }
}
  input[type="date"]::-webkit-calendar-picker-indicator {
    transform: scale(2); /* ปรับขนาดไอคอนใหญ่ขึ้น */
}
  .camera-input {
    padding-right: 30px; /* กำหนดระยะห่างด้านขวาของ TextBox เพื่อให้ไอคอนอยู่ใน TextBox */
}

.camera-icon {
    position: absolute; /* กำหนดให้ไอคอนอยู่ในตำแหน่งที่แน่นอนภายใน TextBox */
    right: 10px; /* ย้ายไอคอนไปทางขวาของ TextBox */
    top: 50%; /* จัดให้ไอคอนอยู่ตรงกลางด้านตัวอักษร */
    transform: translateY(-50%); /* จัดให้ไอคอนอยู่ตรงกลางด้านตัวอักษรตามแนวดิ่ง */
    color: gray; /* กำหนดสีของไอคอน */
    cursor: pointer; /* ทำให้เมาส์เป็นรูปแบบของลูกศรเมื่อชี้ที่ไอคอน */
}

.textbox-container {
    display: inline-block;
    position: relative;
}

.textbox-container .form-control {
    padding-right: 30px; /* ปรับขนาดของ textbox เพื่อให้ icon ไม่ทับข้อความ */
    border: 1px solid #ced4da; /* เป็นตัวอย่างเท่านั้น คุณสามารถปรับแต่งตามต้องการได้ */
    border-radius: .25rem; /* เป็นตัวอย่างเท่านั้น คุณสามารถปรับแต่งตามต้องการได้ */
}
 .ChildGrid {
     width: 2000px; /* ปรับขนาดความกว้างตามต้องการ เช่น 400px */
     font-size: 10px; /* ปรับขนาดตัวอักษร */
 }
 .ChildGrid th, .ChildGrid td {
     padding: 5px; /* ปรับระยะห่างของเซลล์ */
 }
  .textbox-container .fas.fa-qrcode {
    position: absolute;
    top: 50%;
    right: 5px;
    transform: translateY(-50%);
    font-size: 16px; /* ขนาดของไอคอน */
    color: #495057; /* สีของไอคอน */
}
.textbox-container .fas.fa-camera {
    position: absolute;
    top: 50%;
    right: 5px;
    transform: translateY(-50%);
    font-size: 16px; /* ขนาดของไอคอน */
    color: #495057; /* สีของไอคอน */
}
 #GridView1 tr {
        text-align: center;
    }
 #preview {
     width: 100%;
    height: 300px; /* ปรับความสูงของวิดีโอตามที่ต้องการ */
    object-fit: contain; 
    transform: scaleX(-2); /* กลับด้านของวิดีโอให้สะท้อน */
}
  .hideColumn {
        display: none;
    }
          .popup-someEntity{width: 500px;}
  .w3-main {
        margin: auto;
    }
  .table.table-striped.full-width td {
        font-size: 20px; /* ปรับขนาดตัวอักษรตามที่คุณต้องการ */
    }
 .popup-gridview th {
    background-color: #333; /* กำหนดสีพื้นหลังของ Header เป็นสีดำอ่อน */
    color: white; /* กำหนดสีตัวอักษรของ Header เป็นสีขาว */
    padding: 8px; /* กำหนดการเว้นระหว่างขอบของ Header */
}
 #popupContainer {
    display: none;
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background-color: white;
    padding: 20px;
    border: 1px solid #ccc;
    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
    z-index: 1000;
}
 .red-text {
    color: red;
    font-size:24px;
}
#popupGridView {
    margin-bottom: 10px; /* เพิ่มระยะห่างด้านล่างของ GridView */
}
  .popup-gridview {
        max-height: 300px; /* กำหนดความสูงสูงสุดของ GridView */
        overflow-y: auto; /* เพิ่ม scroll หากเนื้อหาเกินความสูง */
    }
#popupTextBox {
    margin-bottom: 10px; /* เพิ่มระยะห่างด้านล่างของ TextBox */
}
</style>

</head>
<body>

<!-- ส่วน Navbar -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <span class="navbar-brand" href="#"></span>

      
        <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item active">
                    <a class="nav-link" href="https://webapp.bpi-concretepile.co.th:8080/#/authen">Home <span class="sr-only">(current)</span></a>
                </li>
            </ul>
        </div>
          <div class="mr-auto">
        <span class="navbar-brand">Username: <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label></span>

    </div>
    </nav>

<!-- Top menu on small screens -->
        <header class="w3-bar w3-top w3-hide-large w3-black w3-xlarge">
 
                                <a class="nav-link" href="https://webapp.bpi-concretepile.co.th:8080/#/authen">Home <span class="sr-only">(current)</span></a>

</header>
<!-- Overlay effect when opening sidebar on small screens -->
        
<div class="overlay d-lg-none" onclick="w3_close()" title="close side menu" id="myOverlay"></div>

<!-- !PAGE CONTENT! -->
   <div class="w3-main" style="margin: auto;">
   <!-- Modal for video popup -->
<div class="modal fade" id="scanModal" tabindex="-1" role="dialog" aria-labelledby="scanModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content">
            <div class="modal-body">
                <div id="reader" style="width: 100%; height: 100%;"></div>
                <button type="button" class="btn btn-secondary mt-3" data-bs-dismiss="modal">Close</button>
                <input type="file" accept="image/*" capture="environment" id="cameraInput" style="display: none;">
            </div>
        </div>
    </div>
</div>

   <!-- Modal for video popup -->
<div class="modal fade" id="scanModal2" tabindex="-1" role="dialog" aria-labelledby="scanModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content">
            <div class="modal-body">
                <div id="reader2" style="width: 100%; height: 100%;"></div>
                <button type="button" class="btn btn-secondary mt-3" data-bs-dismiss="modal">Close</button>
                <input type="file" accept="image/*" capture="environment" id="cameraInput2" style="display: none;">
            </div>
        </div>
    </div>
</div>
   <!-- Modal for video popup -->
<div class="modal fade" id="scanModal3" tabindex="-1" role="dialog" aria-labelledby="scanModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content">
            <div class="modal-body">
                <div id="reader3" style="width: 100%; height: 100%;"></div>
                <button type="button" class="btn btn-secondary mt-3" data-bs-dismiss="modal">Close</button>
                <input type="file" accept="image/*" capture="environment" id="cameraInput3" style="display: none;">
            </div>
        </div>
    </div>
</div>
          <!-- Modal for video popup -->
<div class="modal fade" id="scanModal4" tabindex="-1" role="dialog" aria-labelledby="scanModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content">
            <div class="modal-body">
                <div id="reader4" style="width: 100%; height: 100%;"></div>
                <button type="button" class="btn btn-secondary mt-3" data-bs-dismiss="modal">Close</button>
                <input type="file" accept="image/*" capture="environment" id="cameraInput4" style="display: none;">
            </div>
        </div>
    </div>
</div>
</div>

<div class="w3-main" style="margin: auto; ">

  <!-- Header -->
        <form runat="server">
<asp:HiddenField ID="hiddenFieldTargetControl" runat="server" />
       <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="True" CssClass="ChildGrid" Visible="false">
    <Columns>
    </Columns>
     </asp:GridView>
              <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid" Visible="false">
      <Columns>

      </Columns>
       </asp:GridView>
            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="true" Visible="false" >
                <Columns>
      

     
    </Columns>
            </asp:GridView>
                    <asp:GridView ID="GridView8" runat="server" AutoGenerateColumns="true" Visible="false">
            <Columns>
  


</Columns>
                        
             
        </asp:GridView>
                                <asp:GridView ID="GridView9" runat="server" AutoGenerateColumns="true" Visible="false">
            <Columns>
  


</Columns>
                        
             
        </asp:GridView>
           
 <asp:Label ID="Label11" runat="server" Text="" Style="display:none;"></asp:Label>
<asp:Label ID="Label12" runat="server" Text="" Style="display:none;"></asp:Label>        
<asp:Label ID="Label13" runat="server" Text="" Style="display:none;"></asp:Label>

<asp:Label ID="Label14" runat="server" Text="" Style="display:none;"></asp:Label>
<asp:Label ID="Label15" runat="server" Text="" Style="display:none;"></asp:Label>
<asp:Label ID="Label16" runat="server" Text="" Style="display:none;"></asp:Label>

<asp:Label ID="Label17" runat="server" Text="" Style="display:none;"></asp:Label>
<asp:Label ID="Label18" runat="server" Text="" Style="display:none;"></asp:Label>
<asp:Label ID="Label19" runat="server" Text="" Style="display:none;"></asp:Label>


               <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="True"  CssClass="ChildGrid" style="display:none;">
       <Columns>
       
       </Columns>
        </asp:GridView>
                    <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="false" Visible="false">
            <Columns>
    
   <asp:BoundField HeaderText="Key2" DataField="Key2" />
  <asp:BoundField HeaderText="ColumnQty" DataField="ColumnQty" />
                <asp:BoundField HeaderText="Number04" DataField="Number04" />
</Columns>
        </asp:GridView>
                      <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="True" OnRowDataBound ="GridView4_RowDataBound" CssClass="ChildGrid" Visible="false">
  <Columns>
  
  </Columns>
   </asp:GridView>
<h1>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            

  <div class="w3-Back" style="margin-top:0px" id="showcase">

    <h1 class="display-4">โปรแกรมยิงเข็มขึ้นรถ  </h1><asp:Label ID="ENDUSE" runat="server" Text="" Visible="false" CssClass="red-text"></asp:Label>

      <div class="card">                 
      <div class="card-body">
         <h1 class="delivery-date w3-text-Black">
        <b>บริษัท:</b>
    <asp:TextBox ID="Company" runat="server" class="form-control responsive-textbox" ReadOnly="true" style="display: inline-block; margin-left: 10px;"></asp:TextBox>
    </h1>

 <div class="form-group">
    <h1 class="delivery-date w3-text-Black">
        <b>วันที่ส่งสินค้า:</b>
        <asp:Label ID="Part" runat="server" Text="Label" Visible="false"></asp:Label>
        <div class="textbox-container" style="position: relative; display: inline-block; margin-left: 1px;">
            <asp:TextBox ID="TextBox5" runat="server" class="form-control responsive-textbox" type="text" AutoPostBack="True"></asp:TextBox>
            <i id="calendarIcon" class="far fa-calendar-alt" style="position: absolute; top: 50%; right: 5px; transform: translateY(-50%); cursor: pointer;"></i>
        </div>
    </h1>
</div>
          <h1 class="delivery-date w3-text-Black">

    <b>ใบขึ้นของ :</b>     

        <div class="textbox-container" style="position: relative; display: inline-block; margin-left: 2px;">

<i id="cameraIcon" class="fas fa-qrcode" style="font-size: 24px; pointer-events: none; display:none"></i>

    <asp:TextBox ID="TextBox10" runat="server" class="form-control" ReadOnly="false" OnTextChanged="TextBox10_TextChanged" AutoPostBack="True" >

    </asp:TextBox><asp:Label ID="Label7" runat="server" Visible="False"></asp:Label>

</div>

</h1>
             <h1 class="delivery-date w3-text-Black" ><b>ทะเบียนรถ:</b> 
<%--                        <asp:TextBox ID="Carid" runat="server" class="form-control" readonly="true" style="display: inline-block; margin-left: 10px;"></asp:TextBox>--%>
                     <asp:TextBox ID="Carid" runat="server" class="form-control responsive-textbox" ReadOnly="True" style="display: inline-block; margin-left: 18px;"></asp:TextBox>

   </h1>

<h1 class="delivery-date w3-text-Black"><b>ชื่อหน่วยงาน:</b> <asp:TextBox ID="Compname" runat="server" class="form-control" ReadOnly="true"  Visible="False"></asp:TextBox>  </h1>
          <h1 class="delivery-date w3-text-Black"><b>   </b> <asp:TextBox ID="TextBox4" runat="server" class="form-control" readonly="true" ></asp:TextBox></h1>
 <h1 class="delivery-date w3-text-Black"><b></b> <asp:TextBox ID="TextBox2" runat="server" class="form-control" readonly="true" Visible="False" ></asp:TextBox></h1>

         
<h1 class="delivery-date w3-text-Black">
    <asp:TextBox ID="NODoc" runat="server" ReadOnly="true" class="form-control" Visible="False"></asp:TextBox>
</h1>            <asp:Label ID="Label9" runat="server" Text="" Visible ="False"></asp:Label>

       <div style="display: flex;">
    <div style="flex: 100%;">
        <h1 class="delivery-date w3-text-Black"><b>โรงงาน:</b>

            <asp:Label ID="Label5" runat="server" Text=""></asp:Label>

       <h class="delivery-date w3-text-Black"><b>แท่น:</b><asp:Label ID="Label6" runat="server" Text=""></asp:Label>

      </h>

            <div class="textbox-container" style="position: relative; display: inline-block; width: 100%;">
                                <asp:TextBox ID="TextBox6" runat="server" class="form-control" ReadOnly="False" Style="margin-left: 5px; padding-right: 30px; width: 100%;" AutoPostBack="True" OnTextChanged="TextBoxClear_TextChanged"></asp:TextBox>

            <i id="Warehousecamera" class="fas fa-qrcode" style="cursor: pointer; position: absolute; top: 50%; right: 5px; transform: translateY(-50%); font-size: 24px; pointer-events: none;"></i>
            </div>
        </h1>
    </div>
            <div  style= "flex: 1; display: none;">
                                                <asp:TextBox ID="Bin" runat="server" class="form-control" ReadOnly="False" Style="margin-left: 5px; padding-right: 30px; width: 100%;" AutoPostBack="True" OnTextChanged="TextBox_TextChanged"></asp:TextBox>
                                <asp:TextBox ID="Warehouse" runat="server" class="form-control" ReadOnly="False" Style="margin-left: 5px; padding-right: 30px; width: 100%;" AutoPostBack="True" Visible="True" OnTextChanged="whare_TextChanged"></asp:TextBox>

                </div>
    <div style="flex: 1; display: none;">
        <h1 class="delivery-date w3-text-Black"><b>BIN:</b>
            <div class="textbox-container" style="position: relative; display: inline-block; width: 100%;">
                <i id="BinCmera" class="fas fa-qrcode" style="cursor: pointer; position: absolute; top: 52%; right: 5px; transform: translateY(-52%); font-size: 24px;"></i>
            </div>
        </h1>
    </div>
    <div style="flex: 1;"></div>
</div>

         
    <h1 class="delivery-date w3-text-Black"><b></b> <asp:TextBox ID="TextBox3" runat="server" class="form-control" ReadOnly="true" Visible="False"></asp:TextBox></h1>
           
           <h1 class="delivery-date w3-text-Black">
           <b style="flex-grow: 1;">Serial No:</b>
               <asp:Label ID="Label10" runat="server" Text="" Visible ="False"></asp:Label>
     </h1>
           <h1 class="delivery-date w3-text-Black" style="display: flex; align-items: center;">
    <div class="textbox-container" style="position: relative; display: inline-block; width: 100%;">

        <asp:TextBox ID="TextBox1" runat="server" class="form-control" ReadOnly="False" Style="margin-left: 5px; padding-right: 30px; width: 100%;" OnTextChanged="TextBox1_TextChanged" AutoPostBack="True"></asp:TextBox>
<i id="Camerabarcode" class="fas fa-camera" style="display:none; position: absolute; top: 50%; right: 5px; transform: translateY(-50%); font-size: 24px; pointer-events: none;"></i>
    </div>
</h1>
           <div style="display: flex;">
    <div >
        <h1 class="delivery-date w3-text-Black"><b>ต้องการส่ง:</b>
      <asp:TextBox ID="TextBox13" runat="server" class="form-control" ReadOnly="true" Width="30%"></asp:TextBox>

        </h1>
    </div>
    <div style="flex: 1000;">
        <asp:Label ID="Label2" runat="server" Text="Label" Visible="False"></asp:Label>
        <asp:Label ID="Label3" runat="server" Text="Label" Visible="False"></asp:Label>
    </div>
    <div style="flex: 50;">
        <h1 class="delivery-date w3-text-Black"><b>จำนวน:</b><asp:Label ID="Label1" runat="server" Text="Low value" Visible="False"></asp:Label>
            <asp:TextBox ID="TextBox14" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>

        </h1>
    </div>
    <div style="flex: 1;">
    </div>
    <div style="flex: 20;">
        <h1 class="delivery-date w3-text-Black"><b></b>
                    

        </h1>
    </div>
</div>
          <asp:HiddenField ID="HiddenFieldIndex" runat="server" Value="" />
    <h1 class="delivery-date w3-text-Black"><b>รายการ:         </b>
<div class="table-responsive" style="max-height: 300px; overflow-x: auto; overflow-y: auto;">
    <div class="table-container">

    <asp:GridView ID="GridView1" CssClass="table table-striped full-width" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand" Style="max-width: 100%; overflow-y: auto;" OnRowDataBound="GridView1_RowDataBound">
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
    <FooterStyle BackColor="Tan" />
        <HeaderStyle CssClass="fixed-header" BackColor="Tan" Font-Bold="True" />
    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
    <SortedAscendingCellStyle BackColor="#FAFAE7" />
    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
    <SortedDescendingCellStyle BackColor="#E1DB9C" />
    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
    <Columns>
     
        <asp:TemplateField HeaderText="Seq">
    <ItemTemplate>
        <%# Container.DataItemIndex + 1 %>
    </ItemTemplate>
</asp:TemplateField>
       <asp:BoundField HeaderText="PartNumber" DataField="PartNumber" visible ="false"/>
               <asp:BoundField HeaderText="Part Description" DataField="Description" />

                <asp:BoundField HeaderText="LOT" DataField="LOT" visible="true" />
         <asp:BoundField HeaderText="Warehouse" DataField="Warehouse"  visible="True"/>
        <asp:BoundField HeaderText="Bin" DataField="Bin" visible="True" />
        <asp:BoundField HeaderText="Valuescan" DataField="Valuescan" visible="false"/>
        <asp:BoundField HeaderText="Qty"  DataField="QTY" />
                <asp:BoundField HeaderText="CheckBox02Value"  DataField="CheckBox02Value" visible="false" />

                        <asp:TemplateField HeaderText="Delete" visible="false">
    <ItemTemplate>
        <asp:LinkButton 
    ID="DeleteButton" 
    runat="server" 
    CssClass="btn btn-danger" 
    CommandName="DeleteRow" 
    CommandArgument='<%# Container.DataItemIndex %>' 
    OnClientClick="return confirm('คุณแน่ใจหรือไม่ว่าต้องการลบรายการนี้?');">
    ลบ
</asp:LinkButton>
    <i class="fas fa-trash-alt"></i>
</asp:LinkButton>
    </ItemTemplate>
   </asp:TemplateField>
    </Columns>
</asp:GridView>
        </div>
            </div>
<asp:Button ID="Button2" runat="server" Text="ลบข้อมูล" CssClass="btn btn-danger" OnClick="Button2_Click1" UseSubmitBehavior="false" OnClientClick="showPopup(); return false;" />
        <asp:Button ID="Button1" runat="server" Text="ล้างหน้าจอ" CssClass="btn btn-warning" OnClick="Button1_Click" UseSubmitBehavior="False" /> 

<%--        <asp:Button ID="Button3" runat="server" Text="บันทึก" CssClass="btn btn-success" OnClick="Button3_Click1" UseSubmitBehavior="False"  />--%>

    </h1>
<asp:Button ID="Button4" runat="server" Text="ลบข้อมูลทั้งหมด" CssClass="btn btn-danger" />
<asp:Button ID="Button5" runat="server" Text="แสดง Serial ทั้งหมด" CssClass="btn btn-primary" Visible="false" OnClientClick="showModal(); return false;"  />

</h1>
                        
            <asp:HiddenField ID="HiddenRowIndex" runat="server" />
<!-- Add this HTML inside your .aspx file -->
<div id="popupContainer" style="display:none; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); background-color: white; padding: 20px; border: 1px solid #ccc; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1); z-index: 1000; width: 80%; max-width: 600px;">
    <h2>Serial ที่ต้องการลบ</h2>
    <div class="textbox-container" style="position: relative; display: inline-block; width: 100%;">

    <asp:TextBox ID="popupTextBox" runat="server" class="form-control" ReadOnly="False" Style="margin-left: 5px; padding-right: 30px; width: 100%;" OnTextChanged="popupTextBox_TextChanged" AutoPostBack="True"></asp:TextBox>
    <i id="popupCamera" class="fas fa-qrcode" style="cursor: pointer; position: absolute; top: 45%; right: 2px; transform: translateY(-60%); font-size: 30px;"></i>
</div>
  <%--  <asp:TextBox ID="popupTextBox" runat="server" AutoPostBack="true" OnTextChanged="popupTextBox_TextChanged"></asp:TextBox>
            <i id="popupCamera" class="fas fa-qrcode" style="cursor: pointer; position: absolute; top: 50%; right: 5px; transform: translateY(-50%); font-size: 24px;"></i>--%>

    <br />
    <b style="flex-grow: 1;">Serial No:</b>
    <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
    <div class="table-responsive" style="max-height: 300px; overflow-x: auto; overflow-y: auto;">
    <div class="table-container">
    <asp:GridView ID="popupGridView" CssClass="table table-striped full-width" runat="server" AutoGenerateColumns="true"  Style="max-width: 100%; overflow-y: auto;" >
    <Columns>
    </Columns>
</asp:GridView>
        </div>
        </div>
    <br />
    <asp:Button ID="popupCloseButton" runat="server" Text="ปิด" CssClass="btn btn-secondary" OnClick="popupCloseButton_Click1" OnClientClick="closePopup();" />
    <asp:Button ID="Button3" runat="server" Text="ลบข้อมูล" CssClass="btn btn-danger" />
</div>
<!-- Bootstrap Modal -->
<div class="modal fade" id="serialModal" tabindex="-1" role="dialog" aria-labelledby="serialModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="serialModalLabel">Serial ทั้งหมด</h5>
                <button type="button" class="close" id="closeModalButton" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <!-- ตารางที่จะใช้แสดงข้อมูล -->
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th>Serial</th>
                        </tr>
                    </thead>
                    <tbody id="gridViewData">
                        <!-- ข้อมูลจาก GridView4 จะถูกเพิ่มที่นี่ -->
                    </tbody>
                </table>
            </div>
            <div class="modal-footer">
                <!-- ปุ่มปิด modal -->
                <button type="button" class="btn btn-secondary" id="closeModalFooterButton">ปิด</button>
            </div>
        </div>
    </div>
</div>

<div id="overlay" style="display:none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5); z-index: 999;"></div>

</div>
  
  </form>
</div>  
</div>

  </div>
    

  <!-- Photo grid (modal) -->
   
  <!-- Modal for full size images on click-->
  <div id="modal01" class="w3-modal w3-black" style="padding-top:0" onclick="this.style.display='none'">
    <span class="w3-button w3-black w3-xxlarge w3-display-topright">×</span>
    <div class="w3-modal-content w3-animate-zoom w3-center w3-transparent w3-padding-64">
      <img id="img01" class="w3-image">
      <p id="caption"></p>
    </div>
  </div>
    
  <!-- Services -->
  
  
  <!-- Designers -->
  
  <!-- The Team -->
  

  <!-- Packages / Pricing Tables -->
  
  
  <!-- Contact -->
  
<!-- End page content -->
     <footer style="text-align: left; padding: 10px; background-color: #f1f1f1; position: relative; bottom: 0; width: 100%;">
    🅥2.1.4 DevBy : ⒾⓉ ⒷⓟⒾ ⓉⒺⒶⓂ
</footer>

<!-- W3.CSS Container -->

<script>
    // Script to open and close sidebar
    function w3_open() {
        document.getElementById("mySidebar").style.display = "block";
        document.getElementById("myOverlay").style.display = "block";
    }

    function w3_close() {
        document.getElementById("mySidebar").style.display = "none";
        document.getElementById("myOverlay").style.display = "none";
    }

    // Modal Image Gallery
    function onClick(element) {
        document.getElementById("img01").src = element.src;
        document.getElementById("modal01").style.display = "block";
        var captionText = document.getElementById("caption");
        captionText.innerHTML = element.alt;
    }
</script>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
 <!-- Include Bootstrap JS and Html5Qrcode library -->
<%--    <script src="https://unpkg.com/html5-qrcode" type="text/javascript"></script>--%>
 <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
<%-- <script src="https://unpkg.com/html5-qrcode/minified/html5-qrcode.min.js"></script>--%>

<script>
    function showModal(event) {
        // ป้องกันการรีเฟรชหน้าจอถ้าถูกเรียกจากการกดปุ่ม
        if (event) {
            event.preventDefault();
        }

        // ดึงข้อมูลจาก GridView4
        var gridView = document.getElementById('<%= GridView4.ClientID %>');
        var rows = gridView.getElementsByTagName('tr');
        var tableBody = document.getElementById('gridViewData');

        // ลบข้อมูลเก่าทั้งหมดใน tbody ของตาราง modal
        tableBody.innerHTML = '';

        // ตรวจสอบว่า GridView4 มีข้อมูลหรือไม่
        if (rows.length <= 1) { // ถ้าไม่มีข้อมูล (header อย่างเดียว)
            Swal.fire({
                icon: 'warning',
                title: 'ไม่มีข้อมูล',
                text: 'ใบนี้ยังไม่มีข้อมูล',
                confirmButtonText: 'ตกลง'
            });
            return; // ออกจากฟังก์ชัน
        }

        // ลูปผ่านข้อมูลแต่ละแถวใน GridView4 แล้วนำมาแสดงใน Modal
        for (var i = 1; i < rows.length; i++) { // เริ่มที่ 1 เพื่อข้าม header ของ GridView
            var cells = rows[i].getElementsByTagName('td');
            if (cells.length > 0) {
                var newRow = document.createElement('tr');
                var newCell = document.createElement('td');
                newCell.textContent = cells[1].textContent; // นำข้อมูลจากคอลัมน์แรก (TextColumn)
                newRow.appendChild(newCell);
                tableBody.appendChild(newRow);
            }
        }

        // แสดง modal
        $('#serialModal').modal('show');
    }
</script>


    <script>
        // ฟังก์ชันเพื่อปิด modal
        function closeModal() {
            $('#serialModal').modal('hide');
        }

        // เพิ่ม event listener ให้กับปุ่มปิดใน footer
        $('#closeModalFooterButton').on('click', closeModal);

        // เพิ่ม event listener ให้กับปุ่ม "X"
        $('#closeModalButton').on('click', closeModal);
</script>
    <script>
        $(document).ready(function () {
            // จับเหตุการณ์คลิกของปุ่ม
            $('#<%= Button4.ClientID %>').click(function (e) {
                    e.preventDefault(); // ป้องกันการทำงานปกติของปุ่ม

                    // เรียกใช้งานฟังก์ชัน deleteData
                confirmDeleteall();
                });
            });
        function confirmDeleteall() {
            // เรียกใช้งาน API หรือฟังก์ชันใน C# ที่ใช้สำหรับการลบข้อมูล
            var key2Value = '<%= Label8.Text.Trim() %>';

            // เก็บค่า TextBox10 ลงใน localStorage
            var textBox10Value = $('#TextBox10').val();
            var company = $('#Company').val();

            var postData = { Key2: key2Value, textBox10Value: textBox10Value, company: company };

            var label5Value = $('#<%= Label5.ClientID %>').text().trim();
            var label6Value = $('#<%= Label6.ClientID %>').text().trim();
            localStorage.setItem('Label5Value', label5Value);
            localStorage.setItem('Label6Value', label6Value);
            localStorage.setItem('TextBox10Value', textBox10Value);
            Swal.fire({
                title: 'คุณยืนยันจะลบข้อมูลทั้งหมดหรือไม่?',
                text: "การกระทำนี้ไม่สามารถย้อนกลับได้!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'ยืนยัน',
                cancelButtonText: 'ยกเลิก'
            }).then((result) => {
                if (result.isConfirmed) {
                    // ถ้ายืนยันให้ส่งคำขอ AJAX เพื่อลบข้อมูล
                    $.ajax({
                        type: "POST",
                        url: "default.aspx/DeleteDataall",  // URL ที่จะส่งคำขอไปยัง
                        data: JSON.stringify({ PickingLis: $('#<%= TextBox10.ClientID %>').val(), Company: $('#<%= Company.ClientID %>').val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                        success: function (response) {
                            Swal.fire({
                                title: 'ลบสำเร็จ!',
                                text: 'ข้อมูลถูกลบแล้ว.',
                                icon: 'success'
                            }).then(() => {
                                // รอจนกว่าจะแสดงผลการแจ้งเตือนเสร็จ จากนั้นรีโหลดหน้า
                                window.location.href = window.location.href;
                            });
                        },
                error: function (xhr, status, error) {
                    Swal.fire('เกิดข้อผิดพลาด!', 'ไม่สามารถลบข้อมูลได้.', 'error');
                }
            });
        }
    });
        }
    </script>

    <script>
        $(document).ready(function () {
            // จับเหตุการณ์คลิกของปุ่ม
            $('#<%= Button3.ClientID %>').click(function (e) {
            e.preventDefault(); // ป้องกันการทำงานปกติของปุ่ม

            // เรียกใช้งานฟังก์ชัน deleteData
            deleteData();
        });
    });

        function deleteData() {
            // เรียกใช้งาน API หรือฟังก์ชันใน C# ที่ใช้สำหรับการลบข้อมูล
            var key2Value = '<%= Label8.Text.Trim() %>';

        // เก็บค่า TextBox10 ลงใน localStorage
            var textBox10Value = $('#TextBox10').val();
            var company = $('#Company').val();

            var postData = { Key2: key2Value, textBox10Value: textBox10Value, company: company };

        var label5Value = $('#<%= Label5.ClientID %>').text().trim();
        var label6Value = $('#<%= Label6.ClientID %>').text().trim();
        localStorage.setItem('Label5Value', label5Value);
        localStorage.setItem('Label6Value', label6Value);
        localStorage.setItem('TextBox10Value', textBox10Value);

        // ส่งข้อมูลไปยังเซิร์ฟเวอร์ด้วย AJAX หรือเรียกใช้งานหน้าเว็บเพื่อลบข้อมูล
        $.ajax({
            type: 'POST',
            url: 'default.aspx/DeleteData', // ต้องแก้ไข URL ให้ตรงกับหน้า ASPX ของคุณ
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                // ทำบางสิ่งหลังจากลบข้อมูลเช่น แสดงข้อความสำเร็จหรือรีเฟรชข้อมูล
                Swal.fire({
                    icon: 'success',
                    title: 'ลบข้อมูลเรียบร้อย',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    // รีเฟรชหน้าหรือทำอย่างอื่นตามที่ต้องการ
                    $('#<%= popupGridView.ClientID %>').empty(); // เคลียร์ข้อมูลใน GridView ด้วย jQuery
                    closePopup();

                    window.location.href = window.location.href; // โหลดหน้าใหม่
                });
            },
            error: function (error) {
                // กรณีเกิดข้อผิดพลาดในการลบข้อมูล
                console.error('Error deleting data:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'เกิดข้อผิดพลาดในการลบข้อมูล',
                    text: 'กรุณาลองใหม่อีกครั้ง',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'ตกลง'
                });
            }
        });
        }
    </script>

<script>
    function showPopup() {
        document.getElementById('popupContainer').style.display = 'block';
        document.getElementById('overlay').style.display = 'block';
        document.getElementById('popupTextBox').focus();

    }

    function closePopup() {
        document.getElementById('popupContainer').style.display = 'none';
        document.getElementById('overlay').style.display = 'none';
        document.getElementById('<%= Label8.ClientID %>').innerText = '';

    }

    window.onload = function () {
        var button2 = document.getElementById('<%= Button2.ClientID %>');
        button2.addEventListener('click', function () {
            showPopup();
        });
    };
</script>
 
<script>
    var textBox1 = document.getElementById('<%= TextBox1.ClientID %>'); // TextBox1

    function simulateEnterKey(element) {
        var event = new KeyboardEvent('keydown', {
            bubbles: true,
            cancelable: true,
            keyCode: 13,
            which: 13,
            key: "Enter",
            code: "Enter"
        });
        element.dispatchEvent(event);
    }

    if (textBox1) {
        textBox1.addEventListener('focus', function () {
            simulateEnterKey(textBox1);
        });
        textBox1.focus();
    }
</script>


<script>

    window.onload = function () {
        var textBox10 = document.getElementById('<%= TextBox10.ClientID %>');
        var textBox6 = document.getElementById('<%= TextBox6.ClientID %>');
        var textBox1 = document.getElementById('<%= TextBox1.ClientID %>'); // TextBox1
        var bin = document.getElementById('<%= Bin.ClientID %>');

        var warehouse = document.getElementById('<%= Warehouse.ClientID %>');
        var label5 = document.getElementById('<%= Label5.ClientID %>');
    var label6 = document.getElementById('<%= Label6.ClientID %>');
    var label8Value = document.getElementById('<%= Label8.ClientID %>');

    // ตรวจสอบว่า label8 มีค่าหรือไม่
    if (label8Value.innerText) {
        document.getElementById('popupContainer').style.display = 'block';
        document.getElementById('overlay').style.display = 'block';
    }

    // ตรวจสอบว่า label5, label6, TextBox10 และ TextBox6 มีค่าหรือไม่
    if (label5.innerText && label6.innerText && textBox10.value) {
        textBox1.focus();
    } else if (!textBox10.value) {
        textBox10.focus();
    } else {
        textBox6.focus();
    }

    textBox6.addEventListener('input', function () {
        var value = textBox6.value;
        var parts = value.split(':');

        if (parts.length === 2) {
            label5.innerText = parts[0];
            warehouse.value = parts[0]; // Update Warehouse TextBox
            label6.innerText = parts[1];
            bin.value = parts[1]; // Update Bin TextBox

            // Clear TextBox6 value

          
        }
    });

    warehouse.addEventListener('change', function () {
        var warehouseValue = warehouse.value;
        if (warehouseValue) {
            // ส่งข้อมูลไปยังเซิร์ฟเวอร์เมื่อมีการเปลี่ยนแปลงค่าใน Warehouse
            __doPostBack(warehouse.name, '');
        }
    });

  
};
</script>
  
<script>
  $(function () {
            $("#TextBox5").datepicker({
                dateFormat: 'dd/mm/yy', // รูปแบบวันที่ที่ต้องการ
                onSelect: function (dateText, inst) {
                    $(this).val(dateText); // ตั้งค่าค่าวันที่ที่เลือกใน TextBox
                }
            });
            $("#calendarIcon").click(function (event) {
                event.stopPropagation();
                $("#TextBox5").datepicker({
                    dateFormat: 'dd/mm/yy', // รูปแบบวันที่ที่ต้องการ
                    onSelect: function (dateText, inst) {
                        $("#TextBox5").val(dateText); // ตั้งค่าค่าวันที่ที่เลือกใน TextBox
                    }
                }).datepicker("show");
            });

            // ป้องกันเหตุการณ์ click ที่ TextBox5
            $("#TextBox5").click(function (event) {
                event.stopPropagation();
            });
        });
    </script>

   <%--     <script>
            document.addEventListener("DOMContentLoaded", async function () {
                const html5QrCode = new Html5Qrcode("reader");
                const qrCodeResultInput = document.getElementById("popupTextBox");
                const startScanBtn = document.getElementById("popupCamera");
                const scanModal = new bootstrap.Modal(document.getElementById('scanModal'));
                var Sound = new Audio("Sound/store-scanner-beep-90395.mp3");

                function onScanSuccess(decodedText, decodedResult) {
                    qrCodeResultInput.value = decodedText;
                    console.log("Scanned QR Code:", decodedText);
                    Sound.play(); // เล่นเสียงสำหรับสำเร็จ

                    html5QrCode.stop().then(() => {
                        scanModal.hide();
                        // ส่งอีเวนต์ keypress ให้กับ TextBox เพื่อจำลองการกดปุ่ม Enter
                        const enterKeyEvent = new KeyboardEvent("keypress", {
                            key: "Enter",
                            code: "Enter",
                            keyCode: 13,
                            which: 13,
                            bubbles: true,
                            cancelable: true,
                        });
                        qrCodeResultInput.dispatchEvent(enterKeyEvent);
                    }).catch(err => console.error("Error stopping QR code scanner:", err));
                    document.getElementById('popupTextBox').dispatchEvent(enterKeyEvent);

                }

                function onScanError(errorMessage) {
                    console.error("Error scanning QR code:", errorMessage);
                }



                startScanBtn.addEventListener("click", function () {
                    scanModal.show();
                    scanModal._element.addEventListener('shown.bs.modal', function () {
                        html5QrCode.start(
                            { facingMode: "environment" },
                            { fps: 20, qrbox: { width: 150, height: 150 }, aspectRatio: 1.0 },
                            onScanSuccess,
                            onScanError
                        );
                    }, { once: true });
                });

                scanModal._element.addEventListener('hidden.bs.modal', function () {
                    html5QrCode.stop().catch(err => console.error("Error stopping QR code scanner:", err));
                });
            });
        </script>
    <script>
        document.addEventListener("DOMContentLoaded", async function () {
            const html5QrCode = new Html5Qrcode("reader");
            const qrCodeResultInput = document.getElementById("TextBox10");
            const startScanBtn = document.getElementById("cameraIcon");
            const scanModal = new bootstrap.Modal(document.getElementById('scanModal'));
            var Sound = new Audio("Sound/store-scanner-beep-90395.mp3");

            function onScanSuccess(decodedText, decodedResult) {
                qrCodeResultInput.value = decodedText;
                console.log("Scanned QR Code:", decodedText);
                Sound.play(); // เล่นเสียงสำหรับสำเร็จ

                html5QrCode.stop().then(() => {
                    scanModal.hide();
                    // ส่งอีเวนต์ keypress ให้กับ TextBox เพื่อจำลองการกดปุ่ม Enter
                    const enterKeyEvent = new KeyboardEvent("keypress", {
                        key: "Enter",
                        code: "Enter",
                        keyCode: 13,
                        which: 13,
                        bubbles: true,
                        cancelable: true,
                    });
                    qrCodeResultInput.dispatchEvent(enterKeyEvent);
                }).catch(err => console.error("Error stopping QR code scanner:", err));
                document.getElementById('TextBox10').dispatchEvent(enterKeyEvent);

            }

            function onScanError(errorMessage) {
                console.error("Error scanning QR code:", errorMessage);
            }



            startScanBtn.addEventListener("click", function () {
                scanModal.show();
                scanModal._element.addEventListener('shown.bs.modal', function () {
                    html5QrCode.start(
                        { facingMode: "environment" },
                        { fps: 20, qrbox: { width: 150, height: 150 }, aspectRatio: 1.0 },
                        onScanSuccess,
                        onScanError
                    );
                }, { once: true });
            });

            scanModal._element.addEventListener('hidden.bs.modal', function () {
                html5QrCode.stop().catch(err => console.error("Error stopping QR code scanner:", err));
            });
        });
    </script>
<script>
    document.addEventListener("DOMContentLoaded", async function () {
        const html5QrCode = new Html5Qrcode("reader3");
        const qrCodeResultInput = document.getElementById("Warehouse");
        const startScanBtn = document.getElementById("Warehousecamera");
        const scanModal = new bootstrap.Modal(document.getElementById('scanModal3'));
        var Sound = new Audio("Sound/store-scanner-beep-90395.mp3");

        function onScanSuccess(decodedText, decodedResult) {
            console.log("Scanned QR Code:", decodedText);
            Sound.play(); // เล่นเสียงสำหรับสำเร็จ

            // แยกข้อความเป็นสองส่วนตามที่คุณต้องการ
            const parts = decodedText.split(':');
            if (parts.length === 2) {
                const warehouseText = parts[0];
                const binText = parts[1];

                // นำค่าไปใส่ในฟิลด์ Warehouse และ Bin
                qrCodeResultInput.value = warehouseText;
                document.getElementById("Bin").value = binText;
            } else {
                qrCodeResultInput.value = decodedText; // กรณีข้อความไม่มีการแบ่งด้วย ':'
            }

            html5QrCode.stop().then(() => {
                scanModal.hide();
                // ส่งอีเวนต์ keypress ให้กับ TextBox เพื่อจำลองการกดปุ่ม Enter
                const enterKeyEvent = new KeyboardEvent("keypress", {
                    key: "Enter",
                    code: "Enter",
                    keyCode: 13,
                    which: 13,
                    bubbles: true,
                    cancelable: true,
                });
                qrCodeResultInput.dispatchEvent(enterKeyEvent);
            }).catch(err => console.error("Error stopping QR code scanner:", err));
            qrCodeResultInput.dispatchEvent(enterKeyEvent);
        }

        function onScanError(errorMessage) {
            console.error("Error scanning QR code:", errorMessage);
        }

        startScanBtn.addEventListener("click", function () {
            scanModal.show();
            scanModal._element.addEventListener('shown.bs.modal', function () {
                html5QrCode.start(
                    { facingMode: "environment" },
                    { fps: 20, qrbox: { width: 150, height: 150 }, aspectRatio: 1.0 },
                    onScanSuccess,
                    onScanError
                );
            }, { once: true });
        });

        scanModal._element.addEventListener('hidden.bs.modal', function () {
            html5QrCode.stop().catch(err => console.error("Error stopping QR code scanner:", err));
        });
    });
</script>

<script>
    document.addEventListener("DOMContentLoaded", async function () {
        const html5QrCode = new Html5Qrcode("reader4");
        const qrCodeResultInput = document.getElementById("Bin");
        const startScanBtn = document.getElementById("BinCmera");
        const scanModal = new bootstrap.Modal(document.getElementById('scanModal4'));
        var Sound = new Audio("Sound/store-scanner-beep-90395.mp3");

        function onScanSuccess(decodedText, decodedResult) {
            qrCodeResultInput.value = decodedText;
            console.log("Scanned QR Code:", decodedText);
            Sound.play(); // เล่นเสียงสำหรับสำเร็จ

            html5QrCode.stop().then(() => {
                scanModal.hide();
                // ส่งอีเวนต์ keypress ให้กับ TextBox เพื่อจำลองการกดปุ่ม Enter
                const enterKeyEvent = new KeyboardEvent("keypress", {
                    key: "Enter",
                    code: "Enter",
                    keyCode: 13,
                    which: 13,
                    bubbles: true,
                    cancelable: true,
                });
                qrCodeResultInput.dispatchEvent(enterKeyEvent);
            }).catch(err => console.error("Error stopping QR code scanner:", err));
            document.getElementById('TextBox10').dispatchEvent(enterKeyEvent);

        }

        function onScanError(errorMessage) {
            console.error("Error scanning QR code:", errorMessage);
        }


        startScanBtn.addEventListener("click", function () {
            scanModal.show();
            scanModal._element.addEventListener('shown.bs.modal', function () {
                html5QrCode.start(
                    { facingMode: "environment" },
                    { fps: 20, qrbox: { width: 250, height: 250 }, aspectRatio: 1.0 },
                    onScanSuccess,
                    onScanError
                );
            }, { once: true });
        });

        scanModal._element.addEventListener('hidden.bs.modal', function () {
            html5QrCode.stop().catch(err => console.error("Error stopping QR code scanner:", err));
        });
    });
</script>
    <script>
        document.addEventListener("DOMContentLoaded", async function () {
            const html5QrCode = new Html5Qrcode("reader2");
            const qrCodeResultInput = document.getElementById("TextBox1");
            const startScanBtn = document.getElementById("Camerabarcode");
            const scanModal = new bootstrap.Modal(document.getElementById('scanModal2'));
            var Sound = new Audio("Sound/store-scanner-beep-90395.mp3");

            function onScanSuccess(decodedText, decodedResult) {
                qrCodeResultInput.value = decodedText;
                console.log("Scanned QR Code:", decodedText);
                Sound.play(); // เล่นเสียงสำหรับสำเร็จ

                html5QrCode.stop().then(() => {
                    scanModal.hide();
                    // ส่งอีเวนต์ keypress ให้กับ TextBox เพื่อจำลองการกดปุ่ม Enter
                    const enterKeyEvent = new KeyboardEvent("keypress", {
                        key: "Enter",
                        code: "Enter",
                        keyCode: 13,
                        which: 13,
                        bubbles: true,
                        cancelable: true,
                    });
                    qrCodeResultInput.dispatchEvent(enterKeyEvent);
                }).catch(err => console.error("Error stopping QR code scanner:", err));
                document.getElementById('TextBox1').dispatchEvent(enterKeyEvent);

            }

            function onScanError(errorMessage) {
                console.error("Error scanning QR code:", errorMessage);
            }


            startScanBtn.addEventListener("click", function () {
                scanModal.show();
                scanModal._element.addEventListener('shown.bs.modal', function () {
                    html5QrCode.start(
                        { facingMode: "environment" },
                        { fps: 20, qrbox: { width: 100, height: 100 }, aspectRatio: 2 },
                        onScanSuccess,
                        onScanError
                    );
                }, { once: true });
            });

            scanModal._element.addEventListener('hidden.bs.modal', function () {
                html5QrCode.stop().catch(err => console.error("Error stopping QR code scanner:", err));
            });
        });
    </script>--%>
<script>
    $(document).ready(function () {
        // ดึงค่า TextBox10 จาก localStorage
        var textBox10Value = localStorage.getItem('TextBox10Value');
        if (textBox10Value) {
            $('#TextBox10').val(textBox10Value);
            localStorage.removeItem('TextBox10Value');

            // สร้างเหตุการณ์ Enter และส่งไปยัง TextBox10
            var event = new KeyboardEvent('keypress', {
                key: 'Enter',
                code: 'Enter',
                keyCode: 13,
                which: 13,
                bubbles: true
            });
            $('#TextBox10')[0].dispatchEvent(event);
        }

        // ดึงค่า Label5 และ Label6 จาก localStorage
        var label5Value = localStorage.getItem('Label5Value');
        var label6Value = localStorage.getItem('Label6Value');

        if (label5Value) {
            $('#<%= Label5.ClientID %>').text(label5Value);
            $('#Warehouse').val(label5Value);

            localStorage.removeItem('Label5Value');
        }

        if (label6Value) {
            $('#<%= Label6.ClientID %>').text(label6Value);
            $('#Bin').val(label6Value);

            localStorage.removeItem('Label6Value');
            var enterEvent = new KeyboardEvent('keypress', {
                key: 'Enter',
                code: 'Enter',
                keyCode: 13,
                which: 13,
                bubbles: true
            });
            $('#Bin')[0].dispatchEvent(enterEvent);
        }
    });
</script>

<script>
    function DeleteButton() {
        window.open('popupPage.aspx', 'ชื่อหน้าต่าง', 'width=500,height=400');
    }
</script>

</body>
</html>

