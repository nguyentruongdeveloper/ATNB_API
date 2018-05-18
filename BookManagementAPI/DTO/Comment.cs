using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    using System;
    using System.Collections.Generic;

    public partial class Comment
    {
        public int CommentID { get; set; }
        public int BookID { get; set; }
        public string CommentContent { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual Book Book { get; set; }
    }
}
