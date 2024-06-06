using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kovtun.Data;
using kovtun.Models;
using OfficeOpenXml;
using System.Linq;
using System.Data.Entity; 


namespace kovtun.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Главная страница
        public ActionResult Index()
        {
            return View();
        }

        // Экспорт данных в Excel
        public ActionResult ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var operations = db.Operations.Include(o => o.Employee).Include(o => o.Workplace).ToList();
            var workplaces = db.Workplaces.ToList();
            var employees = db.Employees.ToList();

            using (var package = new ExcelPackage())
            {
                // Лист с операциями
                var operationsWorksheet = package.Workbook.Worksheets.Add("Operations");
                operationsWorksheet.Cells[1, 1].Value = "ID";
                operationsWorksheet.Cells[1, 2].Value = "Description";
                operationsWorksheet.Cells[1, 3].Value = "Workplace";
                operationsWorksheet.Cells[1, 4].Value = "Employee";

                int row = 2;
                foreach (var operation in operations)
                {
                    operationsWorksheet.Cells[row, 1].Value = operation.Id;
                    operationsWorksheet.Cells[row, 2].Value = operation.Description;
                    operationsWorksheet.Cells[row, 3].Value = operation.Workplace.Name;
                    operationsWorksheet.Cells[row, 4].Value = operation.Employee.Name;
                    row++;
                }

                // Лист с рабочими местами
                var workplacesWorksheet = package.Workbook.Worksheets.Add("Workplaces");
                workplacesWorksheet.Cells[1, 1].Value = "ID";
                workplacesWorksheet.Cells[1, 2].Value = "Name";

                row = 2;
                foreach (var workplace in workplaces)
                {
                    workplacesWorksheet.Cells[row, 1].Value = workplace.Id;
                    workplacesWorksheet.Cells[row, 2].Value = workplace.Name;
                    row++;
                }

                // Лист с сотрудниками
                var employeesWorksheet = package.Workbook.Worksheets.Add("Employees");
                employeesWorksheet.Cells[1, 1].Value = "ID";
                employeesWorksheet.Cells[1, 2].Value = "Name";
                employeesWorksheet.Cells[1, 3].Value = "Position";

                row = 2;
                foreach (var employee in employees)
                {
                    employeesWorksheet.Cells[row, 1].Value = employee.Id;
                    employeesWorksheet.Cells[row, 2].Value = employee.Name;
                    employeesWorksheet.Cells[row, 3].Value = employee.Position;
                    row++;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DataExport.xlsx");
            }
        }

        // Импорт данных из Excel
        [HttpPost]
        public ActionResult ImportFromExcel(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file.InputStream))
                {
                    // Лист с операциями
                    var operationsWorksheet = package.Workbook.Worksheets["Operations"];
                    if (operationsWorksheet != null)
                    {
                        for (int row = 2; row <= operationsWorksheet.Dimension.End.Row; row++)
                        {
                            var operation = new Operation
                            {
                                Description = operationsWorksheet.Cells[row, 2].Text,
                                WorkplaceId = int.Parse(operationsWorksheet.Cells[row, 3].Text),
                                EmployeeId = int.Parse(operationsWorksheet.Cells[row, 4].Text)
                            };
                            db.Operations.Add(operation);
                        }
                    }

                    // Лист с рабочими местами
                    var workplacesWorksheet = package.Workbook.Worksheets["Workplaces"];
                    if (workplacesWorksheet != null)
                    {
                        for (int row = 2; row <= workplacesWorksheet.Dimension.End.Row; row++)
                        {
                            var workplace = new Workplace
                            {
                                Name = workplacesWorksheet.Cells[row, 2].Text
                            };
                            db.Workplaces.Add(workplace);
                        }
                    }

                    // Лист с сотрудниками
                    var employeesWorksheet = package.Workbook.Worksheets["Employees"];
                    if (employeesWorksheet != null)
                    {
                        for (int row = 2; row <= employeesWorksheet.Dimension.End.Row; row++)
                        {
                            var employee = new Employee
                            {
                                Name = employeesWorksheet.Cells[row, 2].Text,
                                Position = employeesWorksheet.Cells[row, 3].Text
                            };
                            db.Employees.Add(employee);
                        }
                    }

                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
