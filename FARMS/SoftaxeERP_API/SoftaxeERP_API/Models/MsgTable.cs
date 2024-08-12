using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class MsgTable
{
    public int Id { get; set; }

    public string ChatMsg { get; set; }

    public string MsgSender { get; set; }

    public string MsgReceiver { get; set; }

    public int RecevierSeen { get; set; }

    public int? CmpId { get; set; }
}
