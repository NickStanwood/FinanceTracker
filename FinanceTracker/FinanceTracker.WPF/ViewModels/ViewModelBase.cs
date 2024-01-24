using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace FinanceTracker.WPF
{
    public abstract class ViewModelBase<T> : INotifyPropertyChanged where T : new()
    {
        protected T _m;
        public ViewModelBase()
        {
            _m = new T();
            Initialize();
        }
        public ViewModelBase(T model)
        {
            _m = model;
            Initialize();
        }

        protected abstract void Initialize();

        public T GetModel()
        {
            return _m;
        }

        public void SetModel(T newModel)
        {
            _m = newModel;
            Initialize();
        }
        /// <summary>
        ///  Adds the values from the model to the viewModel and updates the model whenever a value is added to the viewModel 
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <typeparam name="ViewModel"></typeparam>
        /// <param name="modelList"></param>
        /// <param name="viewModelList"></param>
        protected void TieModelListToViewModelList<Model, ViewModel>(List<Model> modelList, ObservableCollection<ViewModel> viewModelList)
            where Model : new()
            where ViewModel : ViewModelBase<Model>, new()
        {
            viewModelList.Clear();
            foreach (Model m in modelList)
            {
                ViewModel vm = new ViewModel { _m = m };
                vm.Initialize();
                viewModelList.Add(vm);
            }

            viewModelList.CollectionChanged += (o, e) => { UpdateModelList(modelList, e); };
        }

        private void UpdateModelList<Model>(List<Model> list, NotifyCollectionChangedEventArgs e)
            where Model : new()
        {
            if (e.OldItems != null)
            {
                foreach (ViewModelBase<Model> vm in e.OldItems)
                    list.Remove(vm.GetModel());
            }

            if (e.NewItems != null)
            {
                foreach (ViewModelBase<Model> vm in e.NewItems)
                    list.Add(vm.GetModel());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
