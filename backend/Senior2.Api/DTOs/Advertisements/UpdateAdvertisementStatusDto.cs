namespace Senior2.Api.DTOs.Advertisements
{
    using Senior2.Api.Models;
    // Application/DTOs/Advertisements/UpdateAdvertisementStatusDto.cs
    using System.ComponentModel.DataAnnotations;


    public sealed class UpdateAdvertisementStatusDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [EnumDataType(typeof(AdvertisementStatus))]
        public AdvertisementStatus Status { get; set; }

        [StringLength(300)]
        public string? AdminNote { get; set; }
    }

}
