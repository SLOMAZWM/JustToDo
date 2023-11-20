using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustToDo
{
    public class Quest
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", Name, Description, Priority, Date);
        }
    }
}
