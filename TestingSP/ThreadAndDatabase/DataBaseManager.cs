using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ThreadAndDatabase
{
    public class DataBaseManager
    {
        private readonly ThreadAppContext _threadAppContext;

        public DataBaseManager()
        {
            _threadAppContext = new ThreadAppContext();
            
        }
    }
}
