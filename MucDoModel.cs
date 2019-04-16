using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class MucDoModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public List<tbl_mucdocauhoi> DanhSach_MucDo()
        {
            return db.tbl_mucdocauhoi.ToList();
        }
        public string TenMD(int maMD)
        {
            var linq = db.tbl_mucdocauhoi.FirstOrDefault(x => x.MaMucDoCauHoi == maMD);
            return linq.TenMucDoCauHoi;
        }
       

    }
}