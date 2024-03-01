using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    public class CategoryRegexViewModel : ViewModelBase<CategoryRegexModel>
    {
        private string _categoryName = "";
        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; Notify(); }
        }
        public string CategoryRegex
        {
            get { return _m.Regex; }
            set { _m.Regex = value; Notify(); }
        }
        public CategoryRegexViewModel(CategoryRegexModel model) : base(model) { }

        protected override async void Initialize()
        {
            CategoryModel cm = await SQLiteContext.GetCategory(_m.CategoryId);
            CategoryName = cm.Name;
        }
    }
}
