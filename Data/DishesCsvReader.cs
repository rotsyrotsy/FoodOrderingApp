using CsvHelper;
using CsvHelper.Configuration;
using FoodOrderingApp.Models;
using System.Globalization;

namespace FoodOrderingApp.Data
{
    public class DishesCsvReader
    {
        private readonly FoodOrderingApp.Data.FoodOrderingAppContext _context;
        public DishesCsvReader(FoodOrderingApp.Data.FoodOrderingAppContext context)
        {
            _context = context;
        }
        public List<Dish> ReadCsvFile(string filePath)
        {
            var dishes = new List<Dish>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                HeaderValidated = null,
                MissingFieldFound = null,
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var category = _context.Category.Where(c=>c.Name == csv.GetField<string>("Category")).FirstOrDefault();
                    var dish = new Dish
                    {
                        Name = csv.GetField<string>("Name"),
                        Description = csv.GetField<string>("Description"),
                        Price = csv.GetField<decimal>("Price"),
                        CategoryId = category.Id
                    };
                    dishes.Add(dish);
                }
            }

            return dishes;
        }
    }
}
