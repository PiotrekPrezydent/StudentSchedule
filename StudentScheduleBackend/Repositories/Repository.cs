using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Attributes;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;

namespace StudentScheduleBackend.Repositories
{
    //fully automatised crud with validation for every entity :)
    public class Repository<T> where T : Entity
    {
        readonly Context _context;

        public Repository(Context context) =>  _context = context;
        //if we adding we must check for adding real foreign key
        //TD WE MUST CHECK IF WE ADDING STUDENT WITH ALREADY USED account, is so throw exception
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

            // Get all ICollection<> properties on T
            var collectionProperties = typeof(T).GetProperties()
                .Where(p =>
                    p.PropertyType.IsGenericType &&
                    typeof(ICollection<>).IsAssignableFrom(p.PropertyType.GetGenericTypeDefinition()));

            // For each collection property, add Include
            foreach (var prop in collectionProperties)
                query = query.Include(prop.Name);

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!typeof(Entity).IsAssignableFrom(prop.PropertyType))
                    continue;
                query = query.Include(prop.Name);
            }


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
            var entityTypes = _context.Model.GetEntityTypes().Select(e => e.ClrType);
            foreach(var entityType in entityTypes)
            {
                //no need to check
                if (entityType == typeof(T))
                    continue;

                foreach(var prop in entityType.GetProperties())
                {
                    var attr = prop.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(ForeignKeyOf));
                    if (attr == null)
                        continue;

                    var foreignEntityType = (Type)attr.ConstructorArguments.First().Value!;
                    if (foreignEntityType != typeof(T))
                        continue;

                    foreach(var foreignEntity in _context.GetEntitiesByType(entityType))
                    {
                        int fkId = (int)prop.GetValue(foreignEntity)!;
                        int entityId = foreignEntity.Id;
                        if (fkId == entity.Id)
                            throw new ReferentialIntegrityException($"You cant delete {typeof(T).Name} with id: {entity.Id}, becouse it is being used in {foreignEntityType.Name} with id: {entityId}.");
                    }
                }
            }
            _context.ChangeTracker.Clear();
            _context.Set<T>().Remove(entity);
            return _context.SaveChanges() > 0;
        }
    }
}