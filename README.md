# Inventory - ระบบจัดการรายการหยิบสินค้า (Picking List)

ระบบจัดการรายการหยิบสินค้าสำหรับโรงงาน BPI พัฒนาด้วย ASP.NET WebForms รองรับการสแกนบาร์โค้ด และเชื่อมต่อกับระบบ ERP

## เทคโนโลยีที่ใช้

- **Framework:** ASP.NET WebForms (.NET Framework 4.8)
- **ภาษา:** C#
- **Database:** Microsoft SQL Server
- **Authentication:** JWT (JSON Web Tokens)
- **UI:** Bootstrap 4.5.2, W3.CSS, jQuery UI, Font Awesome 6.0.0
- **Notification:** SweetAlert2

## ความต้องการของระบบ

- Visual Studio 2022 หรือสูงกว่า
- .NET Framework 4.8
- IIS หรือ IIS Express
- SQL Server (ชื่อ instance: `erp`, database: `BPI_Live`)

## การติดตั้ง

1. Clone หรือ Download โปรเจค

2. เปิดไฟล์ solution ด้วย Visual Studio:
   ```
   democutomer.sln
   ```

3. ตั้งค่า Connection String ใน `Web.config`:
   ```xml
   <connectionStrings>
     <add name="MyConnectionString"
          connectionString="Data Source=erp;Initial Catalog=BPI_Live;User ID=webapp;Password=web@pp;" />
   </connectionStrings>
   ```

4. Restore NuGet Packages:
   ```
   Tools > NuGet Package Manager > Restore NuGet Packages
   ```

5. Build และ Run โปรเจค (F5)

## การใช้งาน

ระบบใช้ JWT Token สำหรับ Authentication โดยต้องส่ง Query Parameters ดังนี้:

```
https://<host>/Default.aspx?token=<JWT_TOKEN>&company=<COMPANY>&site=<SITE>
```

| Parameter | คำอธิบาย |
|-----------|----------|
| `token`   | JWT Token ที่ได้รับจากระบบ ERP |
| `company` | รหัสบริษัท |
| `site`    | รหัสโรงงาน / Plant |

## โครงสร้างโปรเจค

```
democutomer/
├── Default.aspx              # หน้าหลัก - จัดการ Picking List
├── Default.aspx.cs           # Code-behind หน้าหลัก
├── Home.aspx                 # หน้า Redirect / Entry Point
├── Home.aspx.cs              # Code-behind หน้า Home
├── index.html                # Redirect ไปยัง Default.aspx อัตโนมัติ
├── Web.config                # ไฟล์ตั้งค่าระบบ
├── packages.config           # NuGet Dependencies
├── Inventory.csproj          # Project File
├── images/                   # รูปภาพและไอคอน
├── Sound/                    # ไฟล์เสียง (Scanner Beep)
└── Properties/
    └── AssemblyInfo.cs
```

## ฟีเจอร์หลัก

- **Picking List Management** - แสดงและจัดการรายการหยิบสินค้า
- **Barcode Scanning** - รองรับการสแกนบาร์โค้ดพร้อมเสียงแจ้งเตือน
- **JWT Authentication** - ตรวจสอบสิทธิ์ผ่าน Token จาก ERP
- **Time-Based Access Control** - จำกัดเวลาการใช้งานได้ (ตั้งค่าใน Web.config)
- **Multi-Grid Display** - แสดงข้อมูลหลายมุมมองด้วย GridView
- **Real-time Validation** - ตรวจสอบข้อมูลแบบ Real-time
- **SweetAlert2 Notifications** - แจ้งเตือนผู้ใช้แบบ Interactive

## ตารางฐานข้อมูลที่เกี่ยวข้อง

| ตาราง | คำอธิบาย |
|-------|----------|
| `Ice.UD28` | ข้อมูล Picking List หลัก |
| `Ice.UD29` | รายละเอียด Picking |
| `Ice.UD30` | ข้อมูล Warehouse / Bin |
| `Ice.UD15` | การโอนย้าย Picking List |
| `Ice.UD100` | ข้อมูลเพิ่มเติม |
| `Erp.Customer` | ข้อมูลลูกค้า |
| `Erp.ShipTo` | ที่อยู่จัดส่ง |
| `Erp.Part` | ข้อมูลสินค้า / Part |

## การตั้งค่า Web.config

```xml
<appSettings>
  <!-- ตั้งเวลาเริ่มต้นและสิ้นสุดการใช้งาน -->
  <add key="StartTime" value="00:00" />
  <add key="EndTime" value="21:00" />
  <!-- เปิด/ปิด การตรวจสอบเวลา -->
  <add key="EnableTimeCheck" value="false" />
</appSettings>
```

## NuGet Dependencies

| Package | Version |
|---------|---------|
| Newtonsoft.Json | 13.0.3 |
| Microsoft.IdentityModel.Tokens | 7.4.0 |
| System.IdentityModel.Tokens.Jwt | 7.4.0 |
| System.Text.Json | 4.7.2 |

## การพัฒนา

- **IIS Express Port (HTTPS):** 44389
- **Development Server Port:** 8242
- รองรับ async page processing
