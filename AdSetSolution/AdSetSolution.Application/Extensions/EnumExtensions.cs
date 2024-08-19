using AdSetSolution.Domain.Enums;

namespace AdSetSolution.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string getPortalName(this PortalType portalType)
        {
            return portalType switch
            {
                PortalType.ICarros => "iCarros",
                PortalType.WebMotors => "WebMotors",
                _ => "Desconhecido"
            };
        }
    }
}
