using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.DATA.EF.Models//.Metadata
{

    public class GameMetadata
    {
        public int GameId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Game")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string GameName { get; set; } = null!;
    }

    public class ProductMetadata
    {
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [StringLength(200)]
        public string ProductName { get; set; } = null!;

        [Required]
        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Range(0, (double)decimal.MaxValue)]
        public decimal ProductPrice { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Description")]
        [StringLength(500)]
        public string? ProductDescription { get; set; }

        [Required]
        [Display(Name = "In stock")]
        [Range(0, short.MaxValue)]
        public short Inventory { get; set; }

        [Required]
        [Display(Name = "On order")]
        [Range(0, short.MaxValue)]
        public short UnitsOnOrder { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Discontinued?")]
        public bool? IsDiscontinued { get; set; }

        [Required]
        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Game ID")]
        public int? GameId { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Supplier ID")]
        public int? SupplierId { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        [StringLength(75)]
        [Display(Name = "Image")]
        public string? ProductImg { get; set; }
    }

    public class ProductCategoryMetadata
    {
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Category")]
        [StringLength(50)]
        public string CategoryName { get; set; } = null!;

        [Display(Name = "Description")]
        [StringLength(100)]
        public string? CategoryDescription { get; set; }
    }

    public class OrdersMetadata
    {
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [StringLength(128)]
        public string UserId { get; set; } = null!;

        [Required]
        [Display(Name = "Order Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Ship To")]
        [StringLength(100)]
        public string ShipToName { get; set; } = null!;

        [Required]
        [Display(Name = "City")]
        [StringLength(50)]
        public string ShipCity { get; set; } = null!;

        [Display(Name = "State")]
        [StringLength(2)]
        public string ShipState { get; set; } = null!;

        [Required]
        [StringLength(5)]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public string ShipZip { get; set; } = null!;
    }

    public class SupplierMetadata
    {
        public int SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Supplier")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string SupplierName { get; set; } = null!;

        [StringLength(150)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(2)]
        public string? State { get; set; }

        [StringLength(5)]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public string? Zip { get; set; }

        [StringLength(24)]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [StringLength(128)]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }

    public class UserDetailMetadata
    {
        public string UserId { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [StringLength(150)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(2)]
        public string? State { get; set; }

        [StringLength(5)]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public string? Zip { get; set; }

        [StringLength(24)]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [StringLength(128)]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }

}
