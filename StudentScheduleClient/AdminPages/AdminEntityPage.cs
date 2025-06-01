using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Repositories;

namespace StudentScheduleClient.AdminPages
{
    class AdminEntityPage<T> : BaseAdminPage where T : Entity 
    {
        Repository<T> _repository;
        public AdminEntityPage() : base(typeof(T))
        {
            _repository = new(App.DBContext);
            _entities = _repository.GetAll().Cast<Entity>().ToList();
            //temp
            Entities.ItemsSource = _entities;
        }

        protected override bool OnAdd(object o)
        {
            throw new NotImplementedException();
        }

        protected override bool OnDelete(object o)
        {
            throw new NotImplementedException();
        }

        protected override bool OnEdit(object o)
        {
            throw new NotImplementedException();
        }
    }
}
