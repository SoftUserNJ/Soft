using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int CmpId { get; set; }

    public int SenderId { get; set; }

    public string SenderName { get; set; }

    public int ReceiverId { get; set; }

    public string ReceiverName { get; set; }

    public string Message { get; set; }

    public DateTime DateTime { get; set; }

    public DateTime UtcDateTime { get; set; }

    public bool? SeenStatus { get; set; }

    public string Conversionid { get; set; }
}
