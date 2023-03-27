﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
  public class AuthorQuery
  {
    public int Id { get; set; }
    public string KeyWord { get; set; }
    public int JoinedYear { get; set; }
    public int JoinedMonth { get; set; }
  }
}
