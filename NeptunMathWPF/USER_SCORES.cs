//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NeptunMathWPF
{
    using System;
    using System.Collections.Generic;
    
    public partial class USER_SCORES
    {
        public int SCORE_ID { get; set; }
        public int USERID { get; set; }
        public int TOPIC_ID { get; set; }
        public int SUBTOPIC_ID { get; set; }
        public int SCORE { get; set; }
    
        public virtual SUBTOPICS SUBTOPICS { get; set; }
        public virtual TOPICS TOPICS { get; set; }
        public virtual USERS USERS { get; set; }
    }
}
