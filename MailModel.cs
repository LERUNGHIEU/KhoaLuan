using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThiOnline.Models
{
    public class MailModel
    {
        db_thitinhocEntities db = new db_thitinhocEntities();
        public List<tbl_mail> DanhSach_Mail()
        {
            return db.tbl_mail.OrderByDescending(x=>x.Id).Take(10).ToList();
        }
        public bool KiemTraMail(string mail)
        {
            var linq = db.tbl_mail.FirstOrDefault(m => m.Mail == mail);
            if (linq == null)
                return true;
            return false;
        }
        public bool ThemMail(string hinhAnh, string mail, string hoTen)
        {
            var m = new tbl_mail();
            m.Image = hinhAnh;
            m.Mail = mail;
            m.HoTen = hoTen;
            try
            {
                db.tbl_mail.Add(m);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public bool XoaMail(string mail)
        {
            try
            {
                var xoa = (from l in db.tbl_mail where l.Mail == mail select l).Single();
                db.tbl_mail.Remove(xoa);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;

        }
    }
}