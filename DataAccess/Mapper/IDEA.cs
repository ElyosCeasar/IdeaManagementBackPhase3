//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Mapper
{
    using System;
    using System.Collections.Generic;
    
    public partial class IDEA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IDEA()
        {
            this.IDEA_COMMENTS = new HashSet<IDEA_COMMENTS>();
            this.IDEA_POINTS = new HashSet<IDEA_POINTS>();
        }
    
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public byte STATUS_ID { get; set; }
        public string TITLE { get; set; }
        public string CURRENT_SITUATION { get; set; }
        public string PREREQUISITE { get; set; }
        public string STEPS { get; set; }
        public string ADVANTAGES { get; set; }
        public System.DateTime SAVE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
    
        public virtual COMMITTEE_VOTE_DETAIL COMMITTEE_VOTE_DETAIL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IDEA_COMMENTS> IDEA_COMMENTS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IDEA_POINTS> IDEA_POINTS { get; set; }
        public virtual IDEA_STATUS IDEA_STATUS { get; set; }
        public virtual USER USER { get; set; }
        public virtual SELECTED_IDEA SELECTED_IDEA { get; set; }
    }
}