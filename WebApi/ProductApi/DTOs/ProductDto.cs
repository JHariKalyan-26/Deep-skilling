using System.ComponentModel.DataAnnotations;

namespace ProductApi.DTOs;

public class ProductDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }
}
