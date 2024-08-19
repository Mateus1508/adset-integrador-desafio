using System.ComponentModel.DataAnnotations;

namespace AdSetSolution.Application.DTOs
{
    public class VehicleImgDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O ID do veículo é obrigatório.")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "A imagem é obrigatória.")]
        public byte[] ImageData { get; set; }

        [StringLength(255, ErrorMessage = "O nome do arquivo não pode exceder 255 caracteres.")]
        public string? FileName { get; set; }

        [StringLength(20, ErrorMessage = "O tipo de conteúdo não pode exceder 20 caracteres.")]
        public string? ContentType { get; set; }

        public string? ImageBase64
        {
            get
            {
                if (ImageData == null || ContentType == null)
                    return null;

                var base64String = Convert.ToBase64String(ImageData);
                return $"data:{ContentType};base64,{base64String}";
            }
        }
    }
}
