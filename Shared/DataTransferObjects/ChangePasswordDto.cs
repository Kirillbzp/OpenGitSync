﻿namespace OpenGitSync.Shared.DataTransferObjects
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}