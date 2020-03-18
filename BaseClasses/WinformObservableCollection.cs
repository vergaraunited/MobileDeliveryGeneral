using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace UMDGeneral.BaseClasses
{
    public class WinformObservableCollection<T> : ObservableCollection<T>, IList<T>, IEnumerable<T>
           // where T : class
        {

        private IBindingList _bindingList;

        bool ContainsListCollection { get { return true; } }

        //public IList IListSource.GetList()
        //{
        //    return _bindingList ?? (_bindingList = this.ToBindingList());
        //}

        public IList GetList()
        {
            return _bindingList ?? (_bindingList = new BindingList<T>());
        }
    }
}

