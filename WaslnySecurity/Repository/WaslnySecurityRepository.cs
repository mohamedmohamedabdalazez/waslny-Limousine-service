using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslnySecurity.Entity;
using WebMatrix.WebData;

namespace WaslnySecurity.Repository
{
    public class WaslnySecurityRepository : IWaslnySecurityRepository
    {
        private readonly WaslnySecurityDbContext _db;


        public WaslnySecurityRepository()
        {
            _db = new WaslnySecurityDbContext();
        }




    }
}
