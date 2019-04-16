using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class DeLuyenThiModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public List<int> LayMa_CauHoi(string kqht, int mucDo, int soCau)
        {
            List<int> intList = new List<int>();
            var linq = db.tbl_cauhoi.Where(x => x.MaKQHT == kqht && x.MaMucDoCauHoi == mucDo).OrderBy(x => Guid.NewGuid()).Take(soCau);
            foreach(var l in linq)
            {
                intList.Add(l.MaCauHoi);
            }
            return intList;
        }

        
    }
}