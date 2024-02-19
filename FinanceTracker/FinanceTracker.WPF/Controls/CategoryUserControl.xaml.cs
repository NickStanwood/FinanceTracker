using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    /// <summary>
    /// Interaction logic for CategoryUserControl.xaml
    /// </summary>
    public partial class CategoryUserControl : UserControl
    {
        public ObservableCollection<CategoryTreeItemViewModel> CategoryList { get; set; } = new ObservableCollection<CategoryTreeItemViewModel>();
        public LamdaCommand AddCategoryCmd { get; set; }

        public CategoryUserControl()
        {
            InitializeComponent();
            UpdateCategories();

            AddCategoryCmd = new LamdaCommand(
                (obj) => true,
                (obj) => AddChildCategory((CategoryTreeItemViewModel?)obj)
            );
        }

        private async void UpdateCategories()
        {
            CategoryList.Clear();

            //get all parent-most categories
            List<CategoryModel> categories = await SQLiteContext.GetChildCategories(null);

            //add each top level category to the list. each category will get its child categories during construction
            foreach (CategoryModel cat in categories)
            {
                CategoryList.Add(new CategoryTreeItemViewModel(cat));
            }
        }

        private async void AddChildCategory(CategoryTreeItemViewModel? parent)
        {
            if (parent == null)
                return;

            await SQLiteContext.AddCategory(parent.GetModel().Id, "[New Category]");
            UpdateCategories();
        }
    }
}
