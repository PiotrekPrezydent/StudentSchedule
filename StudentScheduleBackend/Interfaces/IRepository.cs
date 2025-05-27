namespace StudentScheduleBackend.Interfaces
{
    public interface IRepository<T>
    {
        public bool Add(T entity);

        public List<T> GetAll();

        public T GetById(int id);

        public bool Update(T entity);

        public bool Delete(int id);
    }
}
