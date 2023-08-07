using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models;

public partial class PhotosUser
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PhotoId { get; set; }
}
