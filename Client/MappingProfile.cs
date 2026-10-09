using Mapster;
using Client.Data.Entities;
using Client.Models;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, ApplicationUserModel>();
    }
}
