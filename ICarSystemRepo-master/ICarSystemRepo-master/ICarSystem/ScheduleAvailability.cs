using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICarSystem
{
    public class ScheduleAvailability
    {
        private int id;
        private DateTime startDate;
        private DateTime endDate;

        public ScheduleAvailability(int id, DateTime startDate, DateTime endDate)
        {
            this.id = id;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
    }
}
