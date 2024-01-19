using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Meowy.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public int Code { get; set; }
    public string? ReasonPhrase { get; set; }

    public void OnGet(int code)
    {
        Code = code;
        ReasonPhrase = ReasonPhraseFromCode(code);
    }

    public string ReasonPhraseFromCode(int code) =>
        code switch
        {
            404 => "Страница не найдена",
            _ => ReasonPhrases.GetReasonPhrase(code)
        };
}
