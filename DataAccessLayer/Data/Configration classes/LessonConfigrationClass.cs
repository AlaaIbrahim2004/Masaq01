﻿using DataAccessLayer.Models.Contents.Lessons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data.Configration_classes
{
    public class LessonConfigrationClass :BaseOfAllEntityConfigrationClass<Lesson>, IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasMany(a=>a.announcements).WithOne(l=>l.lesson).HasForeignKey(a=>a.LessonIdFK).OnDelete(DeleteBehavior.NoAction);
            builder.Property(l => l.Description).HasColumnType("nvarchar(200)");
            base.Configure(builder);
        }
    }
}
