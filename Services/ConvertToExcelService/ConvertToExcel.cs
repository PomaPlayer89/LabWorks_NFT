using Microsoft.EntityFrameworkCore;
using nat.Storage.Migrations;
using OfficeOpenXml;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Services
{
    public class ConvertToExcel : IConvertToExcel
    {
        private readonly CenterDataContext _dbContext;

        public ConvertToExcel(CenterDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<byte[]> ConvertDbToExcel()
        {
            var centers = await _dbContext.Center.AsNoTracking().ToListAsync();
            var customers = await _dbContext.Customer.AsNoTracking().ToListAsync();
            var trainers = await _dbContext.Trainer.AsNoTracking().ToListAsync();


            bool error = false;
            if(centers == null) error = true;
            if(customers == null) error = true;
            if(trainers == null) error = true;
            
            if(error) return new byte[0];
            
            try
            {
                byte[] excelData;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
            
                    var centersSheet = package.Workbook.Worksheets.Add("Centers");
                    centersSheet.Cells[1, 1].Value = "Id";
                    centersSheet.Cells[1, 2].Value = "Название";
                    centersSheet.Cells[1, 3].Value = "Город";
                    centersSheet.Cells[1, 4].Value = "Улица";
                    centersSheet.Cells[1, 5].Value = "Дом";

                    Parallel.For(0, centers.Count,  i =>
                    {
                        centersSheet.Cells[i + 2, 1].Value = centers[i].Id;
                        centersSheet.Cells[i + 2, 2].Value = centers[i].Name;
                        centersSheet.Cells[i + 2, 3].Value = centers[i].AdressCity;
                        centersSheet.Cells[i + 2, 4].Value = centers[i].AdressStreet;
                        centersSheet.Cells[i + 2, 5].Value = centers[i].AdressNumberHouse.ToString();
                    });
            
                    var customersSheet = package.Workbook.Worksheets.Add("Customers");
                    customersSheet.Cells[1, 1].Value = "Id";
                    customersSheet.Cells[1, 2].Value = "Фамилия";
                    customersSheet.Cells[1, 3].Value = "Имя";
                    customersSheet.Cells[1, 4].Value = "Отчество";
                    customersSheet.Cells[1, 5].Value = "Дата рождения";
            
                    Parallel.For(0, customers!.Count, i =>
                    {
                        customersSheet.Cells[i + 2, 1].Value = customers[i].Id;
                        customersSheet.Cells[i + 2, 2].Value = customers[i].SurName;
                        customersSheet.Cells[i + 2, 3].Value = customers[i].Name;
                        customersSheet.Cells[i + 2, 4].Value = customers[i].LastName;
                        customersSheet.Cells[i + 2, 5].Value = customers[i].Birthday.ToString("dd-MM-yyyy");
                    });
            
                    var trainerssSheet = package.Workbook.Worksheets.Add("Trainers");
                    trainerssSheet.Cells[1, 1].Value = "Id";
                    trainerssSheet.Cells[1, 2].Value = "Фамилия";
                    trainerssSheet.Cells[1, 3].Value = "Имя";
                    trainerssSheet.Cells[1, 4].Value = "Отчество";
                    trainerssSheet.Cells[1, 5].Value = "Специализация";

                    Parallel.For(0, trainers!.Count, i =>
                    {
                        trainerssSheet.Cells[i + 2, 1].Value = trainers[i].Id;
                        trainerssSheet.Cells[i + 2, 2].Value = trainers[i].SurName;
                        trainerssSheet.Cells[i + 2, 3].Value = trainers[i].Name;
                        trainerssSheet.Cells[i + 2, 4].Value = trainers[i].LastName;
                        trainerssSheet.Cells[i + 2, 5].Value = trainers[i].Specialization;
                    });
            
                    excelData = await package.GetAsByteArrayAsync();
                }
            
                return excelData;
            }
            catch
            {
                return new byte[0];
            }
        }
    }
}
