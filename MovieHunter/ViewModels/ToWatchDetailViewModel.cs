using System;
using System.Linq;

using GalaSoft.MvvmLight;

using MovieHunter.Core.Models;
using MovieHunter.Core.Services;

namespace MovieHunter.ViewModels
{
    public class ToWatchDetailViewModel : ViewModelBase
    {
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public ToWatchDetailViewModel()
        {
        }

        public void Initialize(long orderId)
        {
            var data = SampleDataService.GetContentGridData();
            Item = data.First(i => i.OrderId == orderId);
        }
    }
}
