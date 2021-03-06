﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductManagerMVC.Models {

  public class Product {
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Please enter product name.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Please enter product category.")]
    public string Category { get; set; }
    [DisplayName("List Price"), DisplayFormat(DataFormatString = "{0:C2}")]
    [Range(1, 10000, ErrorMessage = "Please enter product price between $1 and $10,000.")]
    public double ListPrice { get; set; }
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage = "Please enter product description.")]
    public string Description { get; set; }
    [DisplayName("Product Image")]
    public string ProductImageUrl { get; set; }
  }

}