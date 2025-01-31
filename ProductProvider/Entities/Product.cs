﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductProvider.Entities;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BatchNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Desctiption { get; set; }
    public List<string> Categories { get; set; } = [];
    public string Color { get; set; } = null!;

    public decimal Price { get; set; }

    public string Size { get; set; } = null!;
    public List<string> Images { get; set; } = [];
}