﻿

namespace Spl.Crm.SaleOrder.Modules.Auth.Model;

public class UserInfo
{
    public string user_id { get; set; }
    public string username { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string email { get; set; }
    public IEnumerable<string> role_name { get; set; }
}