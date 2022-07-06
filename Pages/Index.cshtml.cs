using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnPostGetAjax(string name)
    {
        return new JsonResult("Hello " + name);
    }

    public IActionResult OnPostDoubleMoney(string description, int value)
    {
        if (description == "please")
        {
            return new JsonResult("Money doubled to: " + (value * 2));
        }
        else
        {
            return new JsonResult("Money multipled to: " + (value * 10));
        }

    }
    public void OnGet()
    {

    }
}