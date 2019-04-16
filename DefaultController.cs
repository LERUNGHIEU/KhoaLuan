using ASPSnippets.GoogleAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ThiOnline.Common;
using ThiOnline.Models;
namespace ThiOnline.Controllers
{
    public class DefaultController : Controller
    {
        // Model
        DefaultModel modelDefault = new DefaultModel();
        NguoiDungModel m_NguoiDung = new NguoiDungModel();
        DeThiModel m_DeThi = new DeThiModel();
        MailModel m_Mail = new MailModel();
        // mã hóa MD5
        MD5 maHoa = new MD5();
        // kiểm tra mã số
        public bool KiemTra_NguoiDung(string tenDangNhap)
        {
            string str = tenDangNhap.Substring(0,3);
            int maSo = Convert.ToInt32(str.ToString().Trim());
            if (maSo == 111)
                return true;

            return false;
        }
        // kiểm tra chữ hay số
        public bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }
       
        // kiểm tra file ảnh
        public bool KiemTra_FileAnh(HttpPostedFileBase hinh)
        {
            if (hinh != null && hinh.ContentLength > 0)
            {
                string[] strArr = hinh.FileName.Split('.');//Tach đuôi ănh
                string phanMoRong = strArr[strArr.Length - 1].ToLower();
                if (phanMoRong == "png" || phanMoRong == "jpg" || phanMoRong == "jpeg")
                {
                    var fileName = Path.GetFileName(hinh.FileName);
                    string path = Server.MapPath("~/Uploads/Users/" + hinh.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        hinh.SaveAs(path); //upload file
                    }
                    return true;
                }
            }
            return false;

        }
        // GET: Default
        public ActionResult Index()
        {
            return View("Index");
        }
        // đăng ký thành viên
        [HttpPost]
        public ActionResult ThemThanhVien(FormCollection form)
        {
            string maSo = m_NguoiDung.TaoMaThanhVien();
            string hoTen = form["txtHoten"];
           
            DateTime ngaySinh = Convert.ToDateTime(form["txtNgaysinh"]);            
            string mahoa = ngaySinh.ToString("ddMMyyyy");
            HttpPostedFileBase hinh = Request.Files["fileHinhAnh"];           
            string email = form["txtEmail"];           
            string matKhau = Convert.ToString(maHoa.MaHoa(mahoa.Trim()));
            bool kt = m_NguoiDung.KiemTra_Mail(email);          

            if (kt)
            {

                if (KiemTra_FileAnh(hinh) == true)
                {
                    bool them = m_NguoiDung.DangKy(maSo, hoTen, mahoa, hinh.FileName, email, matKhau, 0);
                }
                else
                {
                    bool them = m_NguoiDung.DangKy(maSo, hoTen, mahoa, "", email, matKhau, 0);
                }
                ViewBag.error = "Đăng ký thành công";
            }
            else
            {
                ViewBag.error = "Đăng ký thất bại vì Gmail này đã tồn tại";
            }


            return Index();
        }
        // đăng nhâp

        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
            string tenDangNhap = form["exampleInputEmail1"].ToString().Trim();
            string matKhau = form["exampleInputPassword1"];
            string mk = Convert.ToString(maHoa.MaHoa(matKhau.Trim()));
            bool kt = KiemTra_NguoiDung(tenDangNhap);
            Session["kt"] = kt;
            if (kt)
            {
                bool ktGV = modelDefault.KiemTra_GV(tenDangNhap, mk);
                if (ktGV)
                {
                    
                    var model = modelDefault.ThongTin_GV(tenDangNhap.Trim());
                    foreach (var gv in model)
                    {
                        Session["maSo"] = gv.maSo.ToString();
                        Session["hoTen"] = gv.hoTen;
                        Session["hinhAnh"] = gv.hinhAnh;
                        Session["matKhau"] = gv.matKhau;
                        Session["mail"] = gv.mail;                       
                    }
                    if(Session["mail"] != null)
                    {
                        bool ktMail = m_Mail.KiemTraMail(Session["mail"].ToString());
                        if(ktMail)
                        {
                            bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                        }
                        else
                        {
                            bool xoa = m_Mail.XoaMail(Session["mail"].ToString());
                            if(xoa)
                            {
                                bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                            }
                        }
                    }

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.error = "Thông tin không tồn tại";
                    
                }
            }
           else if (kt == false)
            {
                bool ktSV = modelDefault.KiemTra_SV(tenDangNhap, mk);
                if (ktSV)
                {
                    string ngayHeThong = DateTime.Now.ToString("MM/dd/yyyy");
                    DateTime ngayHienTai = Convert.ToDateTime(ngayHeThong);                   
                    string tgianThi = "";
                    foreach(var l in m_DeThi.DeThi(tenDangNhap))
                    {
                        tgianThi = l.NgayThi.ToString("MM/dd/yyyy");
                    }
                    DateTime ngayThi = Convert.ToDateTime(tgianThi);
                    if (ngayHienTai < ngayThi)
                    {
                        var model = modelDefault.ThongTin_SV(tenDangNhap.Trim());
                        foreach (var gv in model)
                        {
                            Session["maSo"] = gv.maSo.ToString();
                            Session["hoTen"] = gv.hoTen;
                            Session["hinhAnh"] = gv.hinhAnh;
                            Session["matKhau"] = gv.matKhau;
                            Session["mail"] = gv.mail;
                            if (Session["mail"] != null)
                            {
                                bool ktMail = m_Mail.KiemTraMail(Session["mail"].ToString());
                                if (ktMail)
                                {
                                    bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                                }
                                else
                                {
                                    bool xoa = m_Mail.XoaMail(Session["mail"].ToString());
                                    if (xoa)
                                    {
                                        bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                                    }
                                }
                            }
                        }
                       
                        return RedirectToAction("Index", "SinhVien");
                    }
                    else
                    {
                        var model = modelDefault.ThongTin_SV(tenDangNhap.Trim());
                        foreach (var gv in model)
                        {
                            Session["maSo"] = gv.maSo.ToString();
                            Session["hoTen"] = gv.hoTen;
                            Session["hinhAnh"] = gv.hinhAnh;
                            Session["matKhau"] = gv.matKhau;
                            Session["mail"] = gv.mail;
                            if (Session["mail"] != null)
                            {
                                bool ktMail = m_Mail.KiemTraMail(Session["mail"].ToString());
                                if (ktMail)
                                {
                                    bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                                }
                                else
                                {
                                    bool xoa = m_Mail.XoaMail(Session["mail"].ToString());
                                    if (xoa)
                                    {
                                        bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                                    }
                                }
                            }
                        }
                       

                        return RedirectToAction("ChoThi", "SinhVien");
                    }
                }
                else
                {
                    ViewBag.error = "Thông tin không tồn tại";
                    
                }
            }
            else
            {
                ViewBag.error = "Tài khoản hoặc mật khẩu không đúng";
                
            }
          return  Index();
        }
        // cập nhật thông tin
        [HttpPost]
        public ActionResult CapNhatThongTin(FormCollection form)
        {
            string maSo = Session["maSo"].ToString();
            string hoTen = form["txtHoten"];
            DateTime ngay = Convert.ToDateTime(form["txtNgaysinh"]);
            string ngaySinh = ngay.ToString("dd/MM/yyyy");
            HttpPostedFileBase hinhAnh = Request.Files["fileHinhAnh"];
            string hinhcu = form["hinhCu"];
            if(KiemTra_FileAnh(hinhAnh)==true)
            {
                modelDefault.CapNhatNguoiDung(maSo, hoTen, ngaySinh, hinhAnh.FileName);
            }
            else
            {
                modelDefault.CapNhatNguoiDung(maSo, hoTen, ngaySinh, hinhcu);
            }
            return RedirectToAction("Index", "Admin");
        }
        // controller đăng nhập GG+
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void LoginWithGooglePlus()
        {
            GoogleConnect.ClientId = "618863968324-4a64tsm3ehm6m86135hqpoq75umk1719.apps.googleusercontent.com";
            GoogleConnect.ClientSecret = "G7Zb7XHW-JUjVo6yaQ6USP6_";
            GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split('?')[0];
            GoogleConnect.Authorize("profile", "email");
        }
        [ActionName("LoginWithGooglePlus")]
        public ActionResult LoginWithGooglePlusConfirmed()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                string code = Request.QueryString["code"];
                string json = GoogleConnect.Fetch("me", code);
                GoogleProfile profile = new JavaScriptSerializer().Deserialize<GoogleProfile>(json);
               db_thitinhocEntities db = new db_thitinhocEntities();
                string mail = profile.Emails.Find(email => email.Type == "account").Value;
                string [] ktMail = mail.Split('@');               
                string gmail = ktMail[0].ToLower();
                if(IsNumber(gmail)==false)
                {
                    var linq = db.tbl_thanhvien.FirstOrDefault(x => x.Email == mail);
                    if (linq != null)
                    {
                        Session["maSo"] = linq.MaGiangVien;
                        Session["mail"] = linq.Email;
                        Session["hoTen"] = linq.HoTen;
                        Session["hinhAnh"] = linq.HinhAnh;
                        Session["quyen"] = linq.Quyen;
                        Session["matKhau"] = linq.MatKhau;

                        if (Session["mail"] != null)
                        {
                            bool ktMail_ = m_Mail.KiemTraMail(Session["mail"].ToString());
                            if (ktMail_)
                            {
                                bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                            }
                            else
                            {
                                bool xoa = m_Mail.XoaMail(Session["mail"].ToString());
                                if (xoa)
                                {
                                    bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                                }
                            }
                        }

                        if (Session["quyen"].ToString() == "0")
                            return RedirectToAction("Index", "SinhVien");
                        else
                            return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ViewBag.error = "Bạn không có quyền truy cập";
                        return Index();
                    }
                }
                else
                {
                    var linq = db.tbl_sinhvien.FirstOrDefault(x => x.Email == mail);
                    if (linq != null)
                    {
                        Session["maSo"] = linq.MaSinhVien;
                        Session["mail"] = linq.Email;
                        Session["hoTen"] = linq.HoTen;
                        Session["hinhAnh"] = linq.HinhAnh;
                        Session["matKhau"] = linq.MatKhau;
                        if (Session["mail"] != null)
                        {
                            bool ktMail_ = m_Mail.KiemTraMail(Session["mail"].ToString());
                            if (ktMail_)
                            {
                                bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                            }
                            else
                            {
                                bool xoa = m_Mail.XoaMail(Session["mail"].ToString());
                                if (xoa)
                                {
                                    bool them = m_Mail.ThemMail(Session["hinhAnh"].ToString(), Session["mail"].ToString(), Session["hoTen"].ToString());
                                }
                            }
                        }

                        return RedirectToAction("Index", "SinhVien");
                    }
                    else
                    {
                        ViewBag.error = "Bạn không có quyền truy cập";
                        return Index();
                    }
                }

            }
            if (Request.QueryString["error"] == "access_denied")
            {
                return Content("access_denied");
            }
            return Index();

        }
        [HttpPost] 
        public ActionResult DoiMatKhau(FormCollection form)
        {
            string matKhau = form["txtMatKhauMoi"];
            string _matKhau = maHoa.MaHoa(matKhau.Trim());
            modelDefault.DoiMatKhau(Session["maSo"].ToString(), _matKhau);
            return RedirectToAction("Index", "Default");
        }
        public ActionResult DangXuat()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Default");

        }
        
    }
}
    public class GoogleProfile
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public ImageProfile Image { get; set; }
        public List<Email> Emails { get; set; }
        public string Gender { get; set; }
        public string ObjectType { get; set; }
    }
    public class Email
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
    public class ImageProfile
    {
        public string Url { get; set; }
    }
