//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThiOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_sinhvien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_sinhvien()
        {
            this.tbl_danhsachthi = new HashSet<tbl_danhsachthi>();
        }
    
        public string MaSinhVien { get; set; }
        public string HoTen { get; set; }
        public string NgaySinh { get; set; }
        public string Phai { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string HinhAnh { get; set; }
        public int Khoa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_danhsachthi> tbl_danhsachthi { get; set; }
    }
}