using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
  public class Subscriber
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public DateTime SubscribedDate { get; set; }
    public DateTime? UnsubscribedDate { get; set; }
    public string UnsubscribedCausal { get; set; }
    public bool FlagIsBlockSubByAdmin { get; set; }
    public string AdminNotes { get; set; }

    public override string ToString()
    {
      return String.Format("{0,-5}{1,-30}{2,-20}{3,-20}{4,-30}{5,-5}{6,-30}",
        Id, Email, SubscribedDate, UnsubscribedDate,
        UnsubscribedCausal, FlagIsBlockSubByAdmin, AdminNotes);
    }
  }
}
