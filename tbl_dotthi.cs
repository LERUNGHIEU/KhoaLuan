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
    
    public partial class tbl_dotthi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_dotthi()
        {
            this.tbl_danhsachthi = new HashSet<tbl_danhsachthi>();
            this.tbl_dethi = new HashSet<tbl_dethi>();
        }
    
        public string MaDotThi { get; set; }
        public string TenDotThi { get; set; }
        public System.DateTime NgayTao { get; set; }
        public int Nhom { get; set; }
        public string MaLop { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_danhsachthi> tbl_danhsachthi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_dethi> tbl_dethi { get; set; }
        public virtual tbl_lop tbl_lop { get; set; }
    }
}
