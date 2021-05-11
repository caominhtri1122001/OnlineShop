using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContentDAO
    {
        OnlineShopDbContext db = null;
        public ContentDAO()
        {
            db = new OnlineShopDbContext();
        }

        public Content GetByID(long ID)
        {
            return db.Contents.Find(ID);
        }
    }
}
