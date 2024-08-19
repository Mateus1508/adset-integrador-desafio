using AdSetSolution.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AdSetSolution.Application.DTOs
{
    public class PackageDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do pacote é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome do pacote não pode exceder 100 caracteres.")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O total deve ser um valor positivo.")]
        public int Total { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O número de usados deve ser um valor positivo.")]
        public int Used { get; set; }

        public int Available => Total - Used;

        [Required(ErrorMessage = "O tipo de portal é obrigatório.")]
        public PortalType PortalType { get; set; }

        public bool IsExhausted => Total <= Used;
    }
}
