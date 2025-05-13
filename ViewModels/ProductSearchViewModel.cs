// ViewModels/ProductSearchViewModel.cs
using AgriConnect.Models;
using System;
using System.Collections.Generic;

namespace AgriConnect.ViewModels
{
    public class ProductSearchViewModel
    {
        public string ProductType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string FarmerName { get; set; }

        public List<Product> FilteredProducts { get; set; }
    }
}
