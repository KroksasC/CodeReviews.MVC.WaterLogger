using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            DrinkingWater = GetById(id);

            return Page();
        }

        private DrinkingWaterModel GetById(int id)
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $@"SELECT * FROM drinking_water WHERE Id = {id}";
                var model = new DrinkingWaterModel(); 
                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Id = reader.GetInt32(0);
                    model.Date = DateTime.Parse(reader.GetString(1), CultureInfo.CurrentCulture.DateTimeFormat);
                    model.Quantity = reader.GetInt32(2);
                }
                connection.Close();
                return model;
            }
        }
        public IActionResult OnPost(int id)
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $@"DELETE FROM drinking_water WHERE Id = {id}";
                tableCmd.ExecuteNonQuery();
                connection.Close();
                return RedirectToPage("./Index");
            }
        }
    }
}
