using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class KQHTModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public List<tbl_ketquahoctap> DanhSach_KQHT()
        {
            return db.tbl_ketquahoctap.ToList();
        }
        public string TenKQ(string maKQ)
        {
            var linq = db.tbl_ketquahoctap.FirstOrDefault(x => x.MaKQHT == maKQ);
            return linq.TenKQHT;
        }
        public int SoModule ()
        {
            return db.tbl_ketquahoctap.Count();
        }
        
    }
}