using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TalentGo;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
    /// <summary>
    /// 用户列表的ViewModel
    /// </summary>
    public class UserListViewModel
    {
        public UserListViewModel()
        {
            this.OrderColumn = "WhenCreated";
            this.DownDirection = true;
            this.PageSize = 30;
        }
        [Display(Name = "关键字")]
        public string Keywords { get; set; }

        public IEnumerable<Person> AppUserList { get; set; }

        //参加考试次数
        public int ExamTimes { get; set; }

        public String ExamYears { get; set; }

        public string OrderColumn { get; set; }

        public bool DownDirection { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int AllCount { get; set; }

    }
}