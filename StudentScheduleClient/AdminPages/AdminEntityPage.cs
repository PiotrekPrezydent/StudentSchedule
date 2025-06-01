using System.Windows;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Extensions;
using StudentScheduleBackend.Repositories;
using StudentScheduleClient.Popups;

namespace StudentScheduleClient.AdminPages
{
    class AdminEntityPage<T> : BaseAdminPage where T : Entity 
    {
        Repository<T> _repository;
        List<KeyValuePair<string, string>> _filters;

        public AdminEntityPage() : base(typeof(T))
        {
            _repository = new(App.DBContext);
            Entities.ItemsSource = _repository.GetAll();
            _filters = new();
        }

        //td create object form kvp and find
        protected override void OnEdit()
        {
            if (_btnContext == null)
                return;

            var popup = new ShowColumnsPopup(typeof(T), PopupType.Edit, _btnContext.GetColumnsWithValues());
            popup.Owner = Window.GetWindow(this);

            bool? result = popup.ShowDialog();
            var kvps = popup.ReadedValues;

            Entities.ItemsSource = _repository.GetAll().PanDa5ZaTenSuperFilter(_filters);
        }

        protected override void OnDelete()
        {
            if (_btnContext == null)
                return;
            try
            {
                _repository.Delete(_btnContext.Id,out string msg);
                MessageBox.Show(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Entities.ItemsSource = _repository.GetAll().PanDa5ZaTenSuperFilter(_filters);
        }

        //td create object from kvp
        protected override void OnAdd()
        {
            var popup = new ShowColumnsPopup(typeof(T),PopupType.Add,new List<KeyValuePair<string, string>> { new("Id", (_repository.GetAll().Last().Id+1).ToString() ) });
            popup.Owner = Window.GetWindow(this);

            bool? result = popup.ShowDialog();
            var kvps = popup.ReadedValues;

            Entities.ItemsSource = _repository.GetAll().PanDa5ZaTenSuperFilter(_filters);
        }

        //show filters
        protected override void OnFilter()
        {
            var popup = new ShowColumnsPopup(typeof(T), PopupType.Filter,_filters);
            popup.Owner = Window.GetWindow(this);

            bool? result = popup.ShowDialog();
            var kvps = popup.ReadedValues;

            _filters = kvps;

            Entities.ItemsSource = _repository.GetAll().PanDa5ZaTenSuperFilter(_filters);
        }
    }
}
