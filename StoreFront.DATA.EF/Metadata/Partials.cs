using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace StoreFront.DATA.EF.Models//.Metadata
{
    [ModelMetadataType(typeof(SupplierMetadata))]
    public partial class Supplier { }

    [ModelMetadataType(typeof(GameMetadata))]
    public partial class Game { }

    [ModelMetadataType(typeof(ProductMetadata))]
    public partial class Product 
    {
        [NotMapped]
        public IFormFile? Image { get; set; }    
    }

    [ModelMetadataType(typeof(ProductCategoryMetadata))]
    public partial class ProductCategory { }

    [ModelMetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail { }
}
