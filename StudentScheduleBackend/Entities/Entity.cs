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

        public List<KeyValuePair<string,string>> GetColumnsWithValues()
        {
            List<KeyValuePair<string, string>> ret = new();
            foreach (var p in this.GetType().GetProperties())
            {
                //this might cose infinity loop
                if (typeof(Entity).IsAssignableFrom(p.PropertyType))
                    continue;

                string val = p.GetValue(this)!.ToString() ?? "";
                KeyValuePair<string, string> kvp = new(p.Name, val);
                if (p.Name == "Id")
                    ret.Insert(0, kvp);
                else
                    ret.Add(kvp);
            }
            return ret;
        }

    }
}