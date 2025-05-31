namespace StudentScheduleBackend.Entities
{
    public abstract class Entity
    {
        //all entities has to have id for them
        public int Id { get; protected set; }

        //base ToString for all entities it will return entity type name + (name = value) for all properties
        public override string ToString()
        {
            string msg = this.GetType().Name;
            foreach(var p in this.GetType().GetProperties())
            {
                //this might cose infinity loop
                if (typeof(Entity).IsAssignableFrom(p.PropertyType))
                    continue;
                msg += "\t" + p.Name + " = " + p.GetValue(this);
            }
            msg += "\n";
            return msg;
        }
    }
}