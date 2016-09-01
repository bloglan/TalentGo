using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.IO;
using TalentGo.Recruitment;
using TalentGo.EntityFramework;

namespace TalentGo.Utilities
{
    public class BaseDataManger
    {
		TalentGoDbContext database;

        public BaseDataManger(TalentGoDbContext database) {
            this.database = database;
        }

        //获取所有图片资料定义
        public IEnumerable<ArchiveCategory> GetArchiveCategories()
        {
            var allArchiveCategory = from ac in this.database.ArchiveCategory select ac;

            return allArchiveCategory;
        }

        //添加图片资料定义
        public void Update(ArchiveCategory Item)
        {

        
                //Update
                var orginal = this.database.ArchiveCategory.SingleOrDefault(e => e.id == Item.id);
                if (orginal == null)
                    throw new ArgumentException("找不到指定的资料项。");

                Item.WhenCreated = orginal.WhenCreated;
                Item.WhenChanged = DateTime.Now;
                Item.SampleImage = orginal.SampleImage;
                Item.MimeType = orginal.MimeType;

                DbEntityEntry orgEntry = this.database.Entry<ArchiveCategory>(orginal);
                orgEntry.CurrentValues.SetValues(Item);
            //
            try
            {
                this.database.SaveChanges();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message.ToString() + "创建失败！");
            }
            
        }
    }
    

   
}
