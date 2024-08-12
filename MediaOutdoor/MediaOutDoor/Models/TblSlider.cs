using System;
using System.Collections.Generic;

namespace MediaOutDoor.Models;

public partial class TblSlider
{
    public int SlideNo { get; set; }

    public string Heading { get; set; } = null!;

    public string Text { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string? HtmlStyling { get; set; }
}
