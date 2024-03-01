using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    public class CategoryTreeItemViewModel : ViewModelBase<CategoryModel>
    {
        public ObservableCollection<CategoryTreeItemViewModel> Children { get; set; } = new ObservableCollection<CategoryTreeItemViewModel>();

        public string Name { get { return _m.Name; } set { _m.Name = value; Notify(); } }

        public CategoryTreeItemViewModel(CategoryModel model) : base(model) { }
        protected override async void Initialize()
        {

            List<CategoryModel> children = await SQLiteContext.GetChildCategories(_m.Id);
            foreach(var child in children)
            {
                Children.Add(new CategoryTreeItemViewModel(child));
            }
        }
    }
}
