using Spice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.ViewModels
{
    public class CategoriesAndSubCategoriesViewModel
    {
        public IEnumerable<Category> CategoriesList { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<string> SubCategoriesList { get; set; }
        public string StatusMessage { get; set; }
    }
}
