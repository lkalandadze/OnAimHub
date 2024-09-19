﻿namespace OnAim.Admin.Shared.DTOs.User
{
    public class UserPreferences
    {
        public bool DarkMode { get; set; }
        public IDictionary<string, bool> TableColumnVisibility { get; set; } = new Dictionary<string, bool>();
    }
}
