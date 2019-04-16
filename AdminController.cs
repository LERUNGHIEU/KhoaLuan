using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using ThiOnline.Models;
using ThiOnline.Common;
using Newtonsoft.Json;
using System.Text;

namespace ThiOnline.Controllers
{
    public class AdminController : Controller
    {
        CauHoiModel m_CauHoi = new CauHoiModel();
        MucDoModel m_MucDo = new MucDoModel();
        KQHTModel m_Kqht = new KQHTModel();
        PhuongAnModel m_PhuongAn = new PhuongAnModel();
        DotThiModel m_DotThi = new DotThiModel();
        DotThi_SinhVienModel m_DT_SV = new DotThi_SinhVienModel();
        DeThiModel m_DeThi = new DeThiModel();
        DeThi_CauHoiModel m_DTCH = new DeThi_CauHoiModel();
        SinhVienModel m_SinhVien = new SinhVienModel();
        LopModel m_Lop = new LopModel();
        PhieuLamBaiModel m_Phieu = new PhieuLamBaiModel();
        NguoiDungModel m_NguoiDung = new NguoiDungModel();
        MailModel m_Mail = new MailModel();
        MD5 md5 = new MD5();
        
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.CauHoi = m_CauHoi.SoLuong();
            ViewBag.ThanhVien = m_NguoiDung.SoLuong();
            ViewBag.Lop = m_Lop.SoLuong();
            ViewBag.DotThi = m_DotThi.SoLuong();
            var model = m_Mail.DanhSach_Mail();
            return View(model);
        }
        // danh sách câu hỏi
        public ActionResult DanhSach_CauHoi()
        {            
            ViewBag.MucDo = m_MucDo.DanhSach_MucDo();
            ViewBag.Kqht = m_Kqht.DanhSach_KQHT();
            var model = m_CauHoi.DanhSach_CauHoi();
            return View("CauHoi", model);
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
                    string path = Server.MapPath("~/Uploads/Image/" + hinh.FileName);                   
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
        // thêm câu hỏi
        [HttpPost]
        public ActionResult ThemCauHoi(FormCollection form)
        {       
            string cauHoi = form["txtNoiDung"];
            string ghiChu = form["txtGhiChu"];
            string kqht = form["selModulCauHoi"];
            int mucDo = Convert.ToInt32(form["selMucDoCauHoi"]);
            string dapAnA = form["dapAnA"].ToString(); string dapAnB = form["dapAnB"].ToString();
            string dapAnC = form["dapAnC"].ToString(); string dapAnD = form["dapAnD"].ToString();

            string dapAnDung = form["dapAnDung"];
            Session["dapan"] = dapAnDung;
            HttpPostedFileBase hinh = Request.Files["fileHinhAnh"];
            HttpPostedFileBase hinhA = Request.Files["fileHinhAnhA"];
            HttpPostedFileBase hinhB = Request.Files["fileHinhAnhB"];
            HttpPostedFileBase hinhC = Request.Files["fileHinhAnhC"];
            HttpPostedFileBase hinhD = Request.Files["fileHinhAnhD"];           
            
           

            bool kt = m_CauHoi.KiemTra_CauHoi(cauHoi);
            if (kt)
            {  
                if (KiemTra_FileAnh(hinh)==true) //check what type of extension  
                {
                    bool themCH = m_CauHoi.Them_CauHoi(cauHoi,hinh.FileName, ghiChu, kqht, mucDo);                  
                }
                else
                {
                    bool themCH = m_CauHoi.Them_CauHoi(cauHoi, "", ghiChu, kqht, mucDo);
                }
                int cauHoiMax = Convert.ToInt32(m_CauHoi.CauHoi_Max());

                    if (dapAnDung == "A")
                    {
                    // A
                        if(KiemTra_FileAnh(hinhA) == true)
                        {
                        bool themPhuongAnA = m_PhuongAn.Them_PhuongAn(dapAnA,hinhA.FileName, 1, cauHoiMax);
                        }
                        else
                        {
                        bool themPhuongAnA = m_PhuongAn.Them_PhuongAn(dapAnA, "", 1, cauHoiMax);
                        }
                    // B
                        if (KiemTra_FileAnh(hinhB) == true)
                        {
                            bool themPhuongAnB = m_PhuongAn.Them_PhuongAn(dapAnB,hinhB.FileName, 0, cauHoiMax);
                        }
                        else
                        {
                            bool themPhuongAnB = m_PhuongAn.Them_PhuongAn(dapAnB, "", 0, cauHoiMax);
                        }
                    // C
                        if (KiemTra_FileAnh(hinhC) == true)
                        {
                            bool themPhuongAnC = m_PhuongAn.Them_PhuongAn(dapAnC, hinhC.FileName, 0, cauHoiMax);
                        }
                        else
                        {
                            bool themPhuongAnC = m_PhuongAn.Them_PhuongAn(dapAnC, "", 0, cauHoiMax);
                        }
                    // D
                        if (KiemTra_FileAnh(hinhD) == true)
                        {
                            bool themPhuongAnD = m_PhuongAn.Them_PhuongAn(dapAnD, hinhD.FileName, 0, cauHoiMax);
                        }
                        else
                        {
                            bool themPhuongAnD = m_PhuongAn.Them_PhuongAn(dapAnD, "", 0, cauHoiMax);
                        }

                }
                // BBB
                if (dapAnDung == "B")
                    {
                    // A
                    if (KiemTra_FileAnh(hinhA) == true)
                    {
                        bool themPhuongAnA = m_PhuongAn.Them_PhuongAn(dapAnA, hinhA.FileName, 0, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnA = m_PhuongAn.Them_PhuongAn(dapAnA, "", 0, cauHoiMax);
                    }
                    // B
                    if (KiemTra_FileAnh(hinhB) == true)
                    {
                        bool themPhuongAnB = m_PhuongAn.Them_PhuongAn(dapAnB,hinhB.FileName, 1, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnB = m_PhuongAn.Them_PhuongAn(dapAnB, "", 1, cauHoiMax);
                    }
                    // C
                    if (KiemTra_FileAnh(hinhC) == true)
                    {
                        bool themPhuongAnC = m_PhuongAn.Them_PhuongAn(dapAnC, hinhC.FileName, 0, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnC = m_PhuongAn.Them_PhuongAn(dapAnC, "", 0, cauHoiMax);
                    }
                    // D
                    if (KiemTra_FileAnh(hinhD) == true)
                    {
                        bool themPhuongAnD = m_PhuongAn.Them_PhuongAn(dapAnD,hinhD.FileName, 0, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnD = m_PhuongAn.Them_PhuongAn(dapAnD, "", 0, cauHoiMax);
                    }
                }
                // CCC
                if (dapAnDung == "C")
                    {
                    // A
                    if (KiemTra_FileAnh(hinhA) == true)
                    {
                        bool themPhuongAnA = m_PhuongAn.Them_PhuongAn(dapAnA, hinhA.FileName, 0, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnA = m_PhuongAn.Them_PhuongAn(dapAnA, "", 0, cauHoiMax);
                    }
                    // B
                    if (KiemTra_FileAnh(hinhB) == true)
                    {
                        bool themPhuongAnB = m_PhuongAn.Them_PhuongAn(dapAnB, hinhB.FileName, 0, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnB = m_PhuongAn.Them_PhuongAn(dapAnB, "", 0, cauHoiMax);
                    }
                    // C
                    if (KiemTra_FileAnh(hinhC) == true)
                    {
                        bool themPhuongAnC = m_PhuongAn.Them_PhuongAn(dapAnC, hinhC.FileName, 1, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnC = m_PhuongAn.Them_PhuongAn(dapAnC, "", 1, cauHoiMax);
                    }
                    // D
                    if (KiemTra_FileAnh(hinhD) == true)
                    {
                        bool themPhuongAnD = m_PhuongAn.Them_PhuongAn(dapAnD, hinhD.FileName, 0, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnD = m_PhuongAn.Them_PhuongAn(dapAnD, "", 0, cauHoiMax);
                    }
                }
                // DDD
                    if (dapAnDung == "D")
                    {
                    // A
                    if (KiemTra_FileAnh(hinhA) == true)
                    {
                        bool themPhuongAnA = m_PhuongAn.Them_PhuongAn(dapAnA, hinhA.FileName, 0, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnA = m_PhuongAn.Them_PhuongAn(dapAnA, "", 0, cauHoiMax);
                    }
                    // B
                    if (KiemTra_FileAnh(hinhB) == true)
                    {
                        bool themPhuongAnB = m_PhuongAn.Them_PhuongAn(dapAnB, hinhB.FileName, 0, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnB = m_PhuongAn.Them_PhuongAn(dapAnB, "", 0, cauHoiMax);
                    }
                    // C
                    if (KiemTra_FileAnh(hinhC) == true)
                    {
                        bool themPhuongAnC = m_PhuongAn.Them_PhuongAn(dapAnC,hinhC.FileName, 0, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnC = m_PhuongAn.Them_PhuongAn(dapAnC, "", 0, cauHoiMax);
                    }
                    // D
                    if (KiemTra_FileAnh(hinhD) == true)
                    {
                        bool themPhuongAnD = m_PhuongAn.Them_PhuongAn(dapAnD, hinhD.FileName, 1, cauHoiMax);
                    }
                    else
                    {
                        bool themPhuongAnD = m_PhuongAn.Them_PhuongAn(dapAnD, "", 1, cauHoiMax);
                    }
                }
                    ViewBag.Message = " Thêm thành công";
                
            }
            else
            {
                ViewBag.Message = "Thất bại";

            }
            return RedirectToAction("DanhSach_CauHoi", "Admin");
        }
        // import file excel
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelFile)
        {
            db_thitinhocEntities db = new db_thitinhocEntities();
            if (excelFile == null)
            {
                ViewBag.Error = "Vui lòng chọn file excel ";
                return DanhSach_CauHoi();
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Uploads/FileExcel/" + excelFile.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        //System.IO.File.Delete(path);
                        ViewBag.Error = "File đã tồn tại ";
                        return DanhSach_CauHoi();
                    }
                    else
                        excelFile.SaveAs(path);
                    // đọc file excel
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    // List<CauHoi> list = new List<CauHoi>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        tbl_cauhoi ch = new tbl_cauhoi();
                        ch.CauHoi = ((Excel.Range)range.Cells[row, 1]).Text;
                        bool kt = m_CauHoi.KiemTra_CauHoi(ch.CauHoi);
                        if (kt)
                        {
                            ch.HinhAnh = ((Excel.Range)range.Cells[row, 2]).Text;
                            ch.GhiChu = ((Excel.Range)range.Cells[row, 3]).Text;
                            ch.MaKQHT = ((Excel.Range)range.Cells[row, 4]).Text;
                            int maso = Convert.ToInt32(((Excel.Range)range.Cells[row, 5]).Text);
                            ch.MaMucDoCauHoi = maso;
                            db.tbl_cauhoi.Add(ch);
                            db.SaveChanges();
                            // phương án
                            var linq = db.tbl_cauhoi.OrderByDescending(x => x.MaCauHoi).FirstOrDefault();
                            int max = Convert.ToInt32(linq.MaCauHoi);
                            int dung = 1; int sai = 0;
                            tbl_phuongan pa1 = new tbl_phuongan();
                            pa1.PhuongAn = ((Excel.Range)range.Cells[row, 6]).Text;
                            pa1.HinhAnh = ""; pa1.TrangThai = sai; pa1.MaCauHoi = max;
                            db.tbl_phuongan.Add(pa1);
                            db.SaveChanges();
                            tbl_phuongan pa2 = new tbl_phuongan();
                            pa2.PhuongAn = ((Excel.Range)range.Cells[row, 7]).Text;
                            pa2.HinhAnh = ""; pa2.TrangThai = sai; pa2.MaCauHoi = max;
                            db.tbl_phuongan.Add(pa2);
                            db.SaveChanges();
                            tbl_phuongan pa3 = new tbl_phuongan();
                            pa3.PhuongAn = ((Excel.Range)range.Cells[row, 8]).Text;
                            pa3.HinhAnh = ""; pa3.TrangThai = sai; pa3.MaCauHoi = max;
                            db.tbl_phuongan.Add(pa3);
                            db.SaveChanges();
                            tbl_phuongan pa4 = new tbl_phuongan();
                            pa4.PhuongAn = ((Excel.Range)range.Cells[row, 9]).Text;
                            pa4.HinhAnh = ""; pa4.TrangThai = dung; pa4.MaCauHoi = max;
                            db.tbl_phuongan.Add(pa4);
                            db.SaveChanges();
                        }

                    }
                    ViewBag.List = db.tbl_cauhoi.ToList();

                }
                else
                {
                    ViewBag.Error = "Không đúng file ";

                }
            }

            return RedirectToAction("DanhSach_CauHoi", "Admin");
        }
        // sửa câu hỏi
        [HttpGet]
        public ActionResult SuaCauHoi(FormCollection form)
        {
            ViewBag.MucDo = m_MucDo.DanhSach_MucDo();
            ViewBag.Kqht = m_Kqht.DanhSach_KQHT();
            string id = Request.QueryString["idCH"];
            int idCH = Convert.ToInt32(id);
            Session["i"] = idCH;
            var model = m_CauHoi.Tim_CauHoi(idCH);
            return View("CapNhatCauHoi", model);           
           
        }
        [HttpPost]
        public ActionResult CapNhatCauHoi(FormCollection form)
        {
            int maCH = Convert.ToInt32(form["maCH"].ToString());
            string cauHoi = form["txtNoiDung"];
            string ghiChu = form["txtGhiChu"];
            string kqht = form["selModuleCauHoi"];
            int mucDo = Convert.ToInt32(form["selMucDoCauHoi"]);
            string dapAnA = form["dapAnA"]; string dapAnB = form["dapAnB"];
            string dapAnC = form["dapAnC"]; string dapAnD = form["dapAnD"];
            string dapAnDung = form["dapAnDung"];
            string hinhAnh = form["txt_hinhanh"];
            string hinhAnhA = form["txt_hinhA"]; string hinhAnhB = form["txt_hinhB"];
            string hinhAnhC = form["txt_hinhC"]; string hinhAnhD = form["txt_hinhD"];
            HttpPostedFileBase hinh = Request.Files["fileHinhAnh"];
            HttpPostedFileBase hinhA = Request.Files["fileHinhAnhA"];
            HttpPostedFileBase hinhB = Request.Files["fileHinhAnhB"];
            HttpPostedFileBase hinhC = Request.Files["fileHinhAnhC"];
            HttpPostedFileBase hinhD = Request.Files["fileHinhAnhD"];

            var listPhuongAn = m_PhuongAn.PhuongAn_CauHoi(maCH);
            List<int> listMaPhuongAn = new List<int>();
            foreach (var p in listPhuongAn)
            {
                listMaPhuongAn.Add(p.MaPhuongAn);
            }
            if(KiemTra_FileAnh(hinh)==true)
            {
                m_CauHoi.Sua_CauHoi(maCH, cauHoi, hinh.FileName, ghiChu, kqht, mucDo);
                ViewBag.MessageSua = " Cập nhật thành công";
            }
            else
            {
                if(hinhAnh != "")
                { m_CauHoi.Sua_CauHoi(maCH, cauHoi, hinhAnh, ghiChu, kqht, mucDo); }
                else
                { m_CauHoi.Sua_CauHoi(maCH, cauHoi, "", ghiChu, kqht, mucDo); }
                ViewBag.MessageSua = " Cập nhật thành công";

            }// A đúng
            if(dapAnDung=="A")
            {
                // A
                if(KiemTra_FileAnh(hinhA)==true)
                {
                   m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, hinhA.FileName, 1);
                }
                else
                {
                    if(hinhAnhA != null)
                    {m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, hinhAnhA, 1);}
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, "", 1); }
                } // B
                if (KiemTra_FileAnh(hinhB) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, hinhB.FileName, 0);
                }
                else
                {
                    if (hinhAnhB != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, hinhAnhB, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, "", 0); }
                }//C
                if (KiemTra_FileAnh(hinhC) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, hinhC.FileName, 0);
                }
                else
                {
                    if (hinhAnhC != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, hinhAnhC, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, "", 0); }
                }//D
                if (KiemTra_FileAnh(hinhD) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, hinhD.FileName, 0);
                }
                else
                {
                    if (hinhAnhD != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, hinhAnhD, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, "", 0); }
                }
                ViewBag.MessageSua = " Cập nhật thành công";
            }
            // B đúng
            if (dapAnDung == "B")
            {
                // A
                if (KiemTra_FileAnh(hinhA) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, hinhA.FileName, 0);
                }
                else
                {
                    if (hinhAnhA != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, hinhAnhA, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, "", 0); }
                } // B
                if (KiemTra_FileAnh(hinhB) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, hinhB.FileName, 1);
                }
                else
                {
                    if (hinhAnhB != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, hinhAnhB, 1); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, "", 1); }
                }//C
                if (KiemTra_FileAnh(hinhC) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, hinhC.FileName, 0);
                }
                else
                {
                    if (hinhAnhC != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, hinhAnhC, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, "", 0); }
                }//D
                if (KiemTra_FileAnh(hinhD) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, hinhD.FileName, 0);
                }
                else
                {
                    if (hinhAnhD != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, hinhAnhD, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, "", 0); }
                }
                ViewBag.MessageSua = " Cập nhật thành công";
            }// C đúng
            if (dapAnDung == "C")
            {
                // A
                if (KiemTra_FileAnh(hinhA) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, hinhA.FileName, 0);
                }
                else
                {
                    if (hinhAnhA != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, hinhAnhA, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, "", 0); }
                } // B
                if (KiemTra_FileAnh(hinhB) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, hinhB.FileName, 0);
                }
                else
                {
                    if (hinhAnhB != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, hinhAnhB, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, "", 0); }
                }//C
                if (KiemTra_FileAnh(hinhC) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, hinhC.FileName, 1);
                }
                else
                {
                    if (hinhAnhC != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, hinhAnhC, 1); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, "", 1); }
                }//D
                if (KiemTra_FileAnh(hinhD) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, hinhD.FileName, 0);
                }
                else
                {
                    if (hinhAnhD != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, hinhAnhD, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, "", 0); }
                }
                ViewBag.MessageSua = " Cập nhật thành công";
            }
            // D đúng
            if (dapAnDung == "D")
            {
                // A
                if (KiemTra_FileAnh(hinhA) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, hinhA.FileName, 0);
                }
                else
                {
                    if (hinhAnhA != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, hinhAnhA, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[0], maCH, dapAnA, "", 0); }
                } // B
                if (KiemTra_FileAnh(hinhB) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, hinhB.FileName, 0);
                }
                else
                {
                    if (hinhAnhB != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, hinhAnhB, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[1], maCH, dapAnB, "", 0); }
                }//C
                if (KiemTra_FileAnh(hinhC) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, hinhC.FileName, 0);
                }
                else
                {
                    if (hinhAnhC != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, hinhAnhC, 0); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[2], maCH, dapAnC, "", 0); }
                }//D
                if (KiemTra_FileAnh(hinhD) == true)
                {
                    m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, hinhD.FileName, 1);
                }
                else
                {
                    if (hinhAnhD != null)
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, hinhAnhD, 1); }
                    else
                    { m_PhuongAn.Sua_PhuongAn(listMaPhuongAn[3], maCH, dapAnD, "", 1); }
                }
                ViewBag.MessageSua = " Cập nhật thành công";
            }

            return RedirectToAction("DanhSach_CauHoi", "Admin");
        }
        // xóa câu hỏi
        [HttpPost]
        public ActionResult XoaCauHoi(FormCollection form)
        {
            int maCH = Convert.ToInt32(form["maCauHoi"]);
            bool ktra = m_CauHoi.TimCauHoi(maCH);            
            if(ktra == false)
            {
                bool xoaPA = m_PhuongAn.Xoa_PhuongAn(maCH);
                if(xoaPA)
                {
                    bool xoaCH = m_CauHoi.Xoa_CauHoi(maCH);
                    if(xoaCH)
                    ViewBag.MessageXoaCauHoi = " Xóa thành công";
                }
                else
                    ViewBag.MessageXoaCauHoi = " Xóa không thành công";

            }
            else
                ViewBag.MessageXoaCauHoi = " Không thể xóa vì câu hỏi có trong đề thi";

            return RedirectToAction("DanhSach_CauHoi", "Admin");
        }
        // mức độ câu hỏi
        public ActionResult DanhSach_MucDo()
        {
            var model = m_MucDo.DanhSach_MucDo();
            return View("MucDoCauHoi", model);
        }
        // lọc theo mức độ
        [HttpGet]
        public ActionResult CauHoi_MucDo()
        {
            int mucDo = Convert.ToInt32(Request.QueryString["maMucDo"]);
            var model = m_CauHoi.CauHoi_MucDo(mucDo);
            return View("DanhSachCauHoiMucDo", model);
        }
        // kết quả học tập
        public ActionResult DanhSach_KQHT()
        {
            var model = m_Kqht.DanhSach_KQHT();
            return View("KetQuaHocTap", model);
        }
        // lọc theo kết quả học tập
        [HttpGet]
        public ActionResult CauHoi_KetQua()
        {
            string kq = Request.QueryString["maKetQua"];
            var model = m_CauHoi.CauHoi_KetQua(kq);
            return View("DanhSachCauHoiModel", model);
        }
        // danh sách đợt thi
        public ActionResult DanhSach_DotThi()
        {
            var model = m_DotThi.DanhSach_DotThi();
            return View("DotThi",model);
        }
        [HttpPost]
        public ActionResult Them_DotThi(FormCollection form)
        {
            int loai = Convert.ToInt32(form["selLoai"].ToString());
            string tenDT = form["txtTenDotThi"];
            int nhom = Convert.ToInt32(form["selNhom"].ToString());
            string maLop = form["selLop"];
            string moTa = form["txtMota"];           
            DateTime ngayTao = DateTime.Now;
            if(loai==1)
            {
                bool kt = m_DotThi.KiemTra_NhomLop(nhom, maLop);
                if (kt)
                {
                    string maDT = m_DotThi.TaoMaDotThi("DotThi");                    
                    bool them = m_DotThi.Them_DotThi(maDT, tenDT, ngayTao, nhom, maLop, moTa, 0);
                    ViewBag.ThongBao = "Thêm đợt thi thành công";
                }
                else
                    ViewBag.ThongBao = "Đã tồn tạo nhóm/lớp:" + nhom.ToString() + "/" + maLop.ToString();
            }
           
            if(loai==2)
            {
                string maDT = m_DotThi.TaoMaLuyenThi("LuyenThi");
                bool them = m_DotThi.Them_DotThi(maDT, tenDT, ngayTao, nhom, maLop, moTa, 2);
            }           
            //  
            return DanhSach_DotThi();
        }       
        
        [HttpGet]
        public ActionResult ChiTiet_DotThi()
        {            
            string maDT = Request.QueryString["maDT"].Trim();
            Session["maDT"] = maDT;            
            var model = m_DT_SV.DanhSach_SinhVien(maDT);
            return View("ChiTietDotThi", model);
        }
        [HttpPost]
        public void XoaDotThi(string maDotThi)
        {
            List<string> dsMaSo = new List<string>();
            var dsSV = m_DT_SV.DanhSach_MaSV(maDotThi);            
            foreach (var ds in dsSV)
            {
                dsMaSo.Add(ds.MaSinhVien);
            }
            int l = dsMaSo.Count();
            for(int i=0;i<l;i++)
            {
                bool xoaDS = m_DT_SV.Xoa_DT_SV(dsMaSo[i]);
                if(xoaDS)
                {
                    bool xoaSV = m_SinhVien.Xoa_SinhVien(dsMaSo[i]);
                }
            }
            bool xoaDotThi = m_DotThi.XoaDotThi(maDotThi);
          
        }
        [HttpPost]
        public void CapNhatDotThi(string maDotThi, string tenDotThi, string nhom, string lop, string moTa)
        {
            int nhomLop = Convert.ToInt32(nhom);
            m_DotThi.CapNhat_DotThi(maDotThi, tenDotThi, nhomLop, lop, moTa);
        }

        [HttpPost]
        public ActionResult Them_SinhVien(FormCollection form)
        {
            string maDT = form["txt_maDT"];
            string maSo = form["txtMaSinhVien"];
            string hoTen = form["txtHoTen"];
            int gioiTinh = Convert.ToInt32(form["selGioiTinh"]); 
            DateTime ngaySinh = Convert.ToDateTime(form["txtNgaySinh"]);            
            string mahoa = ngaySinh.ToString("dd/MM/yyyy");
            bool kt = m_SinhVien.KiemTra_SinhVien(maSo);               
            if (kt)
            {
                bool themSV;
                if (gioiTinh==0)
                {
                    themSV = m_SinhVien.ThemSinhVien(maSo, hoTen, mahoa, "Nam", 0);
                }
                else
                {
                    themSV = m_SinhVien.ThemSinhVien(maSo, hoTen, mahoa, "Nữ", 0);
                }

                if(themSV)
                {
                    bool them = m_DT_SV.Them_SinhVien(maDT, maSo);
                    ViewBag.ThongBao = "Thêm thành công";
                }

               
            }
            else
            {
                ViewBag.ThongBao = "Sinh viên đã có trong danh sách";
            }           
            var model = m_DT_SV.DanhSach_SinhVien(maDT);
            return View("ChiTietDotThi", model);            
        }
        [HttpPost]
        public ActionResult Them_DSSV(string maDT, HttpPostedFileBase excelFile)
        {

            db_thitinhocEntities db = new db_thitinhocEntities();
            if (excelFile == null)
            {
                ViewBag.Error = "Vui lòng chọn file excel ";
                var model1 = m_DT_SV.DanhSach_SinhVien(maDT);
                return View("ChiTietDotThi", model1);
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Uploads/FileExcel/" + excelFile.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        //System.IO.File.Delete(path);
                        ViewBag.Error = "File đã tồn tại ";
                        var model2 = m_DT_SV.DanhSach_SinhVien(maDT);
                        return View("ChiTietDotThi", model2);
                    }
                    else
                        excelFile.SaveAs(path);
                    // đọc file excel
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    // List<CauHoi> list = new List<CauHoi>();
                    for (int row = 14; row <= range.Rows.Count; row++)
                    {
                        
                        string maSo = ((Excel.Range)range.Cells[row, 2]).Text;
                        if (maSo != "")
                        {
                        string hoTen = ((Excel.Range)range.Cells[row, 3]).Text;
                        hoTen += ((Excel.Range)range.Cells[row, 4]).Text;
                        hoTen += ((Excel.Range)range.Cells[row, 6]).Text;
                        hoTen += ((Excel.Range)range.Cells[row, 7]).Text;
                        string ngaySinh = ((Excel.Range)range.Cells[row, 8]).Text;
                        string phai = ((Excel.Range)range.Cells[row, 9]).Text;
                        string ngaySinh_ = ngaySinh.Replace("/", "");                        
                            bool kt = m_SinhVien.KiemTra_SinhVien(maSo);
                            if (kt)
                            {
                                bool themSV = m_SinhVien.ThemSinhVien(maSo, hoTen, ngaySinh_, phai, 0);
                                if (themSV)
                                {
                                    bool them = m_DT_SV.Them_SinhVien(maDT, maSo);
                                    ViewBag.ThongBao = "Thêm thành công";
                                }
                            }
                            else
                                ViewBag.ThongBao = "Sinh viên này ";
                        }
                        else
                            break;
                    }
                }
                else
                {
                    ViewBag.Error = "Không đúng file ";

                }
            }

            var model = m_DT_SV.DanhSach_SinhVien(maDT);
            return View("ChiTietDotThi", model);
        }
        // cập nhật sinh viên
        [HttpPost]
        public ActionResult CapNhat_SinhVien(FormCollection form)
        {
            string maDT = form["_maDotThi"];
            string maSo = form["_txtMaSinhVien"];
            string hoTen = form["_txtHoTen"];
            int gioiTinh = Convert.ToInt32(form["_selGioiTinh"]);
            DateTime ngaySinh = Convert.ToDateTime(form["_txtNgaySinh"]);
            string mahoa = ngaySinh.ToString("dd/MM/yyyy");
            if(gioiTinh==0)
            {
                m_SinhVien.CapNhat_SinhVien(maSo, hoTen, mahoa, "Nam");
            }
            else
            {
                m_SinhVien.CapNhat_SinhVien(maSo, hoTen, mahoa, "Nữ");
            }
            var model = m_DT_SV.DanhSach_SinhVien(maDT);
            return View("ChiTietDotThi", model);

        }
        // xóa sinh viên
        [HttpPost]
        public ActionResult Xoa_SinhVien(string _maDT, string _maSV)
        {
            bool xoaDTSV = m_DT_SV.Xoa_DT_SV(_maSV);
            if (xoaDTSV)
            {
                bool xoa = m_SinhVien.Xoa_SinhVien(_maSV);
                if (xoa)
                    ViewData["thongbao"] = "Xóa thành công";                
            }
            else ViewData["thongbao"] = "Xóa thất bại";
            var model = m_DT_SV.DanhSach_SinhVien(_maDT);           
            return View("ChiTietDotThi", model);
        }
        [HttpPost]
        public ActionResult KhoaSinhVien(string maDT, string maSV)
        {
            int trangThai = Convert.ToInt32(m_SinhVien.TrangThai_SinhVien(maSV));

            if (trangThai == 0)
                m_SinhVien.CapNhat_TrangThai(maSV, 1);
            else
                m_SinhVien.CapNhat_TrangThai(maSV, 0);

            var model = m_DT_SV.DanhSach_SinhVien(maDT);
            return View("ChiTietDotThi", model);
        }
        public ActionResult BoDeThi()
        {
            var model = m_DeThi.DanhSach_DeThi();
            return View("BoDeThi", model);
        }
        
        public ActionResult TaoDeThi()
        {         
            return View("TaoDeThi");
        }
        
        [HttpPost]
        public ActionResult TaoBoDeThi(FormCollection form)
        {
            int soModule = Convert.ToInt32(m_Kqht.SoModule());            
            List<string> listModule = new List<string>();
            List<int> listDe = new List<int>();
            List<int> listTB = new List<int>();
            List<int> listKho = new List<int>();
            List<int> list = new List<int>();
            var module = m_Kqht.DanhSach_KQHT();
            foreach (var m in module)
            {
                listModule.Add(m.MaKQHT);
            }
            for (int i = 1; i <= soModule; i++)
            {
                string de = "txt_de" + i.ToString() + "";
                string tb = "txt_tb" + i.ToString() + "";
                string kho = "txt_kho" + i.ToString() + "";
                listDe.Add(Convert.ToInt32(form["" + de + ""]));
                listTB.Add(Convert.ToInt32(form["" + tb + ""]));
                listKho.Add(Convert.ToInt32(form["" + kho + ""]));
            }           
            for (int j = 1; j <= 6; j++)
            {
                string maKQ = "Module" + j + "";
                for (int i = 1; i <= 3; i++)
                {

                    if (i == 1)
                    {
                        var de = m_CauHoi.LayMa_CauHoi(maKQ, (i - 1), listDe[j - 1]);                        
                        foreach (var m in de)
                        {
                            list.Add(m.MaCauHoi);
                        }
                    }
                    if (i == 2)
                    {
                        var tb = m_CauHoi.LayMa_CauHoi(maKQ, (i - 1), listTB[j - 1]);
                        foreach (var m in tb)
                        {
                            list.Add(m.MaCauHoi);
                        }
                    }
                    if (i == 3)
                    {
                        var kho = m_CauHoi.LayMa_CauHoi(maKQ, (i - 1), listKho[j - 1]);
                        foreach (var m in kho)
                        {
                            list.Add(m.MaCauHoi);
                        }
                    }

                }

            }
            string maDT = form["selDotThi"];
            bool kiemtra = m_DeThi.KiemTra_DotThi(maDT.Trim());           
            string nhom = ""; string lop = "";
            if (kiemtra)
            {
                var linq = m_DotThi.NhomLop(maDT);
                foreach (var l in linq)
                {
                    nhom = l.Nhom.ToString();
                    lop = l.MaLop.ToString();
                }
                DateTime ngayTao = DateTime.Now;
                DateTime ngayThi = Convert.ToDateTime(form["dateNgayThi"]);

                string ma = nhom + "/" + lop;
                string deThi1 = ma.Trim() + "_1";
                string tenDe1 = "Đề 1";

                string deThi2 = ma.Trim() + "_2";
                string tenDe2 = "Đề 2";

                string deThi3 = ma.Trim() + "_3";
                string tenDe3 = "Đề 3";

                string deThi4 = ma.Trim() + "_4";
                string tenDe4 = "Đề 4";
                string maGV = Session["maSo"].ToString();
                string maCaThi = form["txt_maVaoPhong"];

                bool ktDeThi1 = m_DeThi.ThemDeThi(deThi1.Trim(), tenDe1, maCaThi, ngayTao, ngayThi, 0, maGV, maDT);
                if (ktDeThi1)
                {
                    bool kt;
                    foreach (int p in list)
                    {
                        int i = Convert.ToInt32(p);
                        kt = m_DTCH.ThemDeThi_CauHoi(deThi1.Trim(), i);
                    }
                }
                bool ktDeThi2 = m_DeThi.ThemDeThi(deThi2.Trim(), tenDe2, maCaThi, ngayTao, ngayThi, 0, maGV, maDT);
                if (ktDeThi2)
                {
                    bool kt;
                    var m = m_DTCH.DanhSachCauHoiTron(deThi1);
                    foreach (var p in m)
                    {
                        int i = Convert.ToInt32(p.MaCauHoi);
                        kt = m_DTCH.ThemDeThi_CauHoi(deThi2.Trim(), i);
                    }
                }
                bool ktDeThi3 = m_DeThi.ThemDeThi(deThi3.Trim(), tenDe3, maCaThi, ngayTao, ngayThi, 0, maGV, maDT);
                if (ktDeThi3)
                {
                    bool kt;
                    var m = m_DTCH.DanhSachCauHoiTron(deThi1);
                    foreach (var p in m)
                    {
                        int i = Convert.ToInt32(p.MaCauHoi);
                        kt = m_DTCH.ThemDeThi_CauHoi(deThi3.Trim(), i);
                    }
                }
                bool ktDeThi4 = m_DeThi.ThemDeThi(deThi4.Trim(), tenDe4, maCaThi, ngayTao, ngayThi, 0, maGV, maDT);
                if (ktDeThi4)
                {
                    bool kt;
                    var m = m_DTCH.DanhSachCauHoiTron(deThi1);
                    foreach (var p in m)
                    {
                        int i = Convert.ToInt32(p.MaCauHoi);
                        kt = m_DTCH.ThemDeThi_CauHoi(deThi4.Trim(), i);
                    }
                }
                ViewData["thongbao"] = "Tạo đề thi thành công";
                var model = m_DeThi.DanhSach_DeThi();
                return View("BoDeThi", model);
            }
            else
            {
                ViewData["thongbao"] = "Tạo đề không thành công";              
                return View("TaoDeThi");
            }           
        }
        [HttpPost]
        public ActionResult CapNhat_NgayThi(FormCollection form)
        {
            string maDe = form["txt_made"];
            DateTime ngayThi = Convert.ToDateTime(form["txt_ngaythi"]);
            m_DeThi.CapNhat(maDe, ngayThi);
            ViewBag.ThongBao = "Cập nhật ngày thi thành công";
            return BoDeThi();
        }
        [HttpGet]
        public ActionResult XemDeThi()
        {
            string maDT = Request.QueryString["idMD"].Trim();
            Session["maDe"] = maDT;
            ViewBag.De = m_DTCH.SoMucDo(maDT, 0);
            ViewBag.TB = m_DTCH.SoMucDo(maDT, 1);
            ViewBag.Kho = m_DTCH.SoMucDo(maDT, 2);
            var model = m_DTCH.BaiThi(maDT);
            return View("DeThi", model);
        }
        [HttpPost]
        public ActionResult Xoa_BoDeThi(FormCollection form)
        {
            string maDT = form["txt_maDe"];
            bool kt = m_DeThi.TrangThai(maDT);
            if(kt)
            {
                bool xoaCT = m_DTCH.XoaDeThiCauHoi(maDT);
                if(xoaCT)
                {
                    m_DeThi.XoaBoDe(maDT);
                }
                ViewBag.ThongBao = "Xóa bộ đề thi thành công";
            }
            else
                ViewBag.ThongBao = "Xóa bộ đề thi thất bại";
            return BoDeThi();
        }
        [HttpGet]
        public ActionResult ThayTheCauHoi()
        {
            string maDT = Request.QueryString["idDT"].Trim();
            Session["maDe"] = maDT;
            int maCH = Convert.ToInt32(Request.QueryString["idCH"].Trim().ToString());
            Session["maCH"] = maCH;
            var model = m_DTCH.CauHoi(maCH);
            return View("ThayTheCauHoi", model);
        }
        [HttpPost]
        public ActionResult ThayThe(FormCollection form)
        {
            string maDe = Session["maDe"].ToString();
            int maCH1 = Convert.ToInt32(Session["maCH"].ToString());
           
            var model1 = m_DTCH.LayMaTru_CauHoi(maDe, maCH1);

            foreach (var m in model1)
            {
                Session["ma"] = Convert.ToInt32(m.MaCauHoi);
            }
            int maCH2 = Convert.ToInt32(Session["ma"].ToString());
            int l = maDe.Trim().Length;
            string str = maDe.Substring(0, l - 1).Trim();
            m_DTCH.ThayTheCauHoi(str, maCH1, maCH2);
            var model = m_DTCH.CauHoiThuocDeThi(maDe, maCH2);            
            return View("ThayTheCauHoi", model);
        }
        public ActionResult DanhSachLop()
        {
            var model = m_Lop.DanhSach_Lop();
            return View("Lop",model);
        }
        // thêm lớp
        [HttpPost]
        public ActionResult ThemLop(FormCollection form)
        {
            string maLop = form["txtMaLop"];
            string tenLop = form["txtTenLop"];
            bool ktLop = m_Lop.KiemTra_MaLop(maLop);
            if(ktLop)
            {
                bool themLop = m_Lop.Them_Lop(maLop, tenLop);
                ViewBag.ThongBao = "Thêm lớp thành công";
            }
            else
                ViewBag.ThongBao = "Mã lớp đã tồn tại thành công";

            var model = m_Lop.DanhSach_Lop();
            return View("Lop", model);
        }
        [HttpPost]
        public ActionResult SuaLop(string maLop, string tenLop)
        {
            m_Lop.SuaLop(maLop, tenLop);
            var model = m_Lop.DanhSach_Lop();
            return View("Lop", model);
        }
        [HttpPost] 
        public ActionResult XoaLop(string maLop)
        {
            bool ktLop = m_DotThi.KiemTra_MaLop(maLop);
            if(ktLop)
            {
                bool xoa = m_Lop.XoaLop(maLop);
                ViewBag.ThongBao = "Xóa lớp thành công";
            }
           else ViewBag.ThongBao = "Không thể xóa lớp này";

            var model = m_Lop.DanhSach_Lop();
            return View("Lop", model);
        }
        // Danh sách thành viên
        public ActionResult DanhSach_ThanhVien()
        {
            var model = m_NguoiDung.DanhSach_ThanhVien();
            return View("ThanhVien", model);
        }
        // thêm thành viên
        [HttpPost]
        public ActionResult ThemThanhVien(FormCollection form)
        {
            string hoTen = form["txtHoten"];           
            int quyen = Convert.ToInt32(form["selQuyen"]);
            string email = form["txtEmail"];
            DateTime ngaySinh = Convert.ToDateTime(form["txtNgaysinh"]);
            string mahoa = ngaySinh.ToString("ddMMyyyy");
            HttpPostedFileBase hinh = Request.Files["fileHinhAnh"];
           
            string matKhau = Convert.ToString(md5.MaHoa(mahoa.Trim()));
            bool kt = m_NguoiDung.KiemTra_Mail(email);
            if (kt)
            {
                if (quyen == 1)
                {
                    string maSo = form["maGV"];
                    if (KiemTra_FileAnh(hinh) == true)
                    {
                        bool them = m_NguoiDung.DangKy(maSo, hoTen, mahoa, hinh.FileName, email, matKhau, quyen);
                    }
                    else
                    {
                        bool them = m_NguoiDung.DangKy(maSo, hoTen, mahoa, "", email, matKhau, quyen);
                    }
                    ViewBag.error = "Tạo tài khoản thành công";
                }
                if(quyen==0)
                {
                    string maSo = m_NguoiDung.TaoMaThanhVien();
                    if (KiemTra_FileAnh(hinh) == true)
                    {
                        bool them = m_NguoiDung.DangKy(maSo, hoTen, mahoa, hinh.FileName, email, matKhau, quyen);
                    }
                    else
                    {
                        bool them = m_NguoiDung.DangKy(maSo, hoTen, mahoa, "", email, matKhau, quyen);
                    }
                    ViewBag.error = "Tạo tài khoản thành công";
                }
            }
            else
            {
                ViewBag.error = "Email này đã tồn tại";
            }
            var model = m_NguoiDung.DanhSach_ThanhVien();
            return View("ThanhVien", model);
        }
        [HttpPost]
        public void XoaThanhVien(string ma)
        {
            bool xoa = m_NguoiDung.XoaThanhVien(ma);
            if(xoa)
                ViewBag.error = "Xóa thành công";
            else
                ViewBag.error = "Xóa không thành công";
        }
        public ActionResult ThongKe()
        {
            var model = m_DotThi.ChiTiet_DotThi();
            return View("ThongKe", model);
        }
        [HttpGet]
        public ActionResult BieuDo()
        {
            string maDT = Request.QueryString["maDotThi"].Trim();
            ViewBag.MaDotThi = maDT;
            Session["maDotThi"] = maDT;
            var model = m_Phieu.DanhSach_KetQua(maDT);
            try
            {
                ViewBag.DataPoints = JsonConvert.SerializeObject(model.ToList(), _jsonSetting);

                return View("BieuDo",model);
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return View("Error");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return View("Error");
            }
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        public ActionResult XuatExcel()
        {
            return View("XuatExcel");
        }
        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            return File(Encoding.UTF8.GetBytes(GridHtml), "application/vnd.ms-excel", "KetQua.xls");
        }
        public ActionResult Test()
        {
            return View("test");
        }
    }




    
}