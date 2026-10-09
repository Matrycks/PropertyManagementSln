using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Data.Entities;
using Client.Models;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public DetailsModel(IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public ApplicationUserModel? UserAccount { get; set; }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null)
            UserAccount = _mapper.Map<ApplicationUserModel>(user);
    }
}