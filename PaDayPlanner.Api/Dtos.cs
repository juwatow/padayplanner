using System;
using System.ComponentModel.DataAnnotations;

namespace PaDayPlanner.Api.Dtos
{
    public record ActivityDto(Guid Id, string Name, string Description, string BusinessOwner, decimal Price, DateTimeOffset CreatedDate, DateTimeOffset UpdatedDate);
    public record CreateActivityDto([Required] string Name, string Description, [Required]string BusinessOwner, [Range(1, 1000)] decimal Price);
    public record UpdateActivityDto([Required] string Name, string Description, [Required]string BusinessOwner, [Range(1, 1000)] decimal Price);
}