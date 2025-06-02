using System.Windows;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;
using StudentScheduleBackend.Extensions;
using StudentScheduleBackend.Repositories;
using StudentScheduleClient.Popups;

namespace StudentScheduleClient.AdminPages
{
    class AdminEntityPage<T> : BaseAdminPage where T : Entity, new()
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
            string msg = "";
            foreach(var kvp in _btnContext.GetColumnsWithValues())
            {
                msg += kvp.Key + " --- " + kvp.Value + "\n";
            }
            MessageBox.Show(msg);
            popup.Owner = Window.GetWindow(this);

            bool? result = popup.ShowDialog();
            if (result == false)
                return;
            var kvps = popup.ReadedValues;
            //check if readed values are correct
            try
            {
                var entity = Entity.CreateFromKVP<T>(kvps);
                _repository.Update(entity);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            Entities.ItemsSource = _repository.GetAll().PanDa5ZaTenSuperFilter(_filters);
        }

        protected override void OnDelete()
        {
            if (_btnContext == null)
                return;
            try
            {
                _repository.Delete(_btnContext.Id);
            }
            catch(ReferentialIntegrityException ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            Entities.ItemsSource = _repository.GetAll().PanDa5ZaTenSuperFilter(_filters);
        }

        //td create object from kvp
        protected override void OnAdd()
        {
            var popup = new ShowColumnsPopup(typeof(T),PopupType.Add,new List<KeyValuePair<string, string>> { new("Id", "AUTOADDED" ) });
            popup.Owner = Window.GetWindow(this);

            bool? result = popup.ShowDialog();
            if (result == false)
                return;
            var kvps = popup.ReadedValues;
            //id is setted by default in ms sql
            kvps.RemoveAt(0);

            try
            {
                var entity = Entity.CreateFromKVP<T>(kvps);
                _repository.Add(entity);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            Entities.ItemsSource = _repository.GetAll().PanDa5ZaTenSuperFilter(_filters);
        }

        //show filters
        protected override void OnFilter()
        {
            var popup = new ShowColumnsPopup(typeof(T), PopupType.Filter,_filters);
            popup.Owner = Window.GetWindow(this);

            bool? result = popup.ShowDialog();
            if (result == false)
                return;
            var kvps = popup.ReadedValues;

            _filters = kvps;

            Entities.ItemsSource = _repository.GetAll().PanDa5ZaTenSuperFilter(_filters);
        }
    }
}
