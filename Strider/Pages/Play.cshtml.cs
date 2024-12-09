using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Strider.Pages;

public class PlayModel : PageModel
{
    private readonly ILogger<PlayModel> _logger;

    public PlayModel(ILogger<PlayModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
