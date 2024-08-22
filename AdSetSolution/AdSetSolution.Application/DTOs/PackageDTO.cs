using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class PackageDTO
{
    public int Id { get; }

    public string Name { get; }

    public int Total { get; }

    public int Used { get; }

    public int Available { get; }

    public PortalType PortalType { get; }

    public bool IsExhausted { get; }

public PackageDTO(Package package)
    {
        Id = package.Id;
        Name = package.Name;
        Total = package.Total;
        Used = package.Used;
        Available = package.Available;
        PortalType = package.PortalType;
        IsExhausted = package.IsExhausted;
    }
}
