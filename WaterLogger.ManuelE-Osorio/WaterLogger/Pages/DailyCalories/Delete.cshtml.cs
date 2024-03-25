using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WaterLogger.Models;
using WaterLogger.Data;

namespace WaterLogger.Pages.DailyCalories;
public class DeleteModel(DrinkingWaterContext context) : PageModel
{
    private readonly DrinkingWaterContext _context = context;

    [BindProperty]
    public DailyCaloriesModel DailyCalories { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var dailyCalories = await _context.DailyCalories.FirstOrDefaultAsync(m => m.Id == id);

        if (dailyCalories == null)
            return NotFound();
        else
            DailyCalories = dailyCalories;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var dailyCalories = await _context.DailyCalories.FindAsync(id);
        if (dailyCalories != null)
        {
            DailyCalories = dailyCalories;
            _context.DailyCalories.Remove(DailyCalories);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
