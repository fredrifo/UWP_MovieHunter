using System;
using System.Linq;

using GalaSoft.MvvmLight;

using MovieHunter.Core.Models;
using MovieHunter.Core.Services;

namespace MovieHunter.ViewModels
{
    public class ToWatchedDetailViewModel : ViewModelBase
    {
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public ToWatchedDetailViewModel()
        {
        }

        public void Initialize(long orderId)
        {
            var data = SampleDataService.GetContentGridData();
            Item = data.First(i => i.OrderId == orderId);
        }
    }
}
