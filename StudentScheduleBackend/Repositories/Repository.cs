using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Attributes;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;

namespace StudentScheduleBackend.Repositories
{
    //fully automatised crud with validation for every entity :)
    class Repository<T> where T : Entity
    {
        readonly Context _context;

        public Repository(Context context) =>  _context = context;

        public bool Add(T entity)
        {
            //if we using foreign keys, check if added entity contains proper foreign key
            foreach (var p in typeof(T).GetProperties())
            {
                //chceck if entity has any foreign key attributes
                var attr = p.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(ForeignKeyOf));
                if (attr == null)
                    continue;

                //get int value of this foreign key
                int fkId = (int)p.GetValue(entity)!;

                //throw exception if could not find entity with this foreign key
                var foreignEntityType = (Type)attr.ConstructorArguments.First().Value!;
                if (!_context.GetEntitiesByType(foreignEntityType).Any(e => e.Id == fkId))
                    throw new ForeignKeyNotFoundException($"{foreignEntityType.Name} with id: {fkId} could not be found");
            }

            _context.Set<T>().Add(entity);
            return _context.SaveChanges() > 0;
        }

        public List<T> GetAll()
        {
            IQueryable<T> query = _context.Set<T>();

            // Get all ICollection<> properties on T (navigation collections)
            var collectionProperties = typeof(T).GetProperties()
                .Where(p =>
                    p.PropertyType.IsGenericType &&
                    typeof(ICollection<>).IsAssignableFrom(p.PropertyType.GetGenericTypeDefinition()));

            // For each collection property, add Include
            foreach (var prop in collectionProperties)
                query = query.Include(prop.Name);

            return query.ToList();
        }

        public T GetById(int id)
        {
            IQueryable<T> query = _context.Set<T>();
            if (!query.Any(e => e.Id == id))
                throw new KeyNotFoundException($"{typeof(T).Name} with id: {id} could not be found");

            // Get all ICollection<> properties on T (navigation collections)
            var collectionProperties = typeof(T).GetProperties()
                .Where(p =>
                    p.PropertyType.IsGenericType &&
                    typeof(ICollection<>).IsAssignableFrom(p.PropertyType.GetGenericTypeDefinition()));

            // For each collection property, add Include
            foreach (var prop in collectionProperties)
                query = query.Include(prop.Name);

            return query.First(e => e.Id == id);
        }

        public bool Update(T entity)
        {
            IQueryable<T> query = _context.Set<T>();
            if (!query.Any(e => e.Id == entity.Id))
                throw new KeyNotFoundException($"{typeof(T).Name} with id: {entity.Id} could not be found");

            //if we using foreign keys, check if added entity contains proper foreign key
            foreach (var p in typeof(T).GetProperties())
            {
                //chceck if entity has any foreign key attributes
                var attr = p.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(ForeignKeyOf));
                if (attr == null)
                    continue;

                //get int value of this foreign key
                int fkId = (int)p.GetValue(entity)!;

                //throw exception if could not find entity with this foreign key
                var foreignEntityType = (Type)attr.ConstructorArguments.First().Value!;
                if (!_context.GetEntitiesByType(foreignEntityType).Any(e => e.Id == fkId))
                    throw new ForeignKeyNotFoundException($"{foreignEntityType.Name} with id: {fkId} could not be found");
            }

            _context.ChangeTracker.Clear();
            _context.Set<T>().Update(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            IQueryable<T> query = _context.Set<T>();
            if (!query.Any(e => e.Id == id))
                throw new KeyNotFoundException($"{typeof(T).Name} with id: {id} could not be found");

            T entity = query.First(e => e.Id == id);

            //if we using foreign keys, check if added entity contains proper foreign key
            foreach (var p in typeof(T).GetProperties())
            {
                //chceck if entity has any foreign key attributes
                var attr = p.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(ForeignKeyOf));
                if (attr == null)
                    continue;

                //get int value of this foreign key
                int fkId = (int)p.GetValue(entity)!;

                //throw exception if could not find entity with this foreign key
                var foreignEntityType = (Type)attr.ConstructorArguments.First().Value!;
                if (_context.GetEntitiesByType(foreignEntityType).Any(e => e.Id == fkId))
                    throw new ReferentialIntegrityException($"Can't delete {typeof(T).Name} with id {id}, becouse some {foreignEntityType.Name} is referencing to it.");
            }

            _context.ChangeTracker.Clear();
            _context.Set<T>().Remove(entity);
            return _context.SaveChanges() > 0;
        }

    }
}
