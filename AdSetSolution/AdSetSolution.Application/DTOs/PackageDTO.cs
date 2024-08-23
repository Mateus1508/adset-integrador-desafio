using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class PackageDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Total { get; set;  }

    public int Used { get; set;  }

    public int Available { get; set;  }

    public PortalType PortalType { get; set; }

    public bool IsExhausted { get; set; }

}
