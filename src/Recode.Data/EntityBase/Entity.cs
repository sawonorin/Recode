﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Recode.Data.EntityContract;

namespace Recode.Data.EntityBase
{
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        ///---------------------------------------------------------------------------------------------
        /// <summary>
        ///   Is a Entity class which inherit for IEntity to aid fast development
        /// </summary>
        /// <copyright>
        /// *****************************************************************************
        ///     ----- Fadipe Wasiu Ayobami . All Rights Reserved. Copyright (c) 2017
        /// *****************************************************************************
        /// </copyright>
        /// <remarks>
        /// *****************************************************************************
        ///     ---- Created For: Public Use (All Products)
        ///     ---- Created By: Fadipe Wasiu Ayobami
        ///     ---- Original Language: C#
        ///     ---- Current Version: v1.0.0.0.1
        ///     ---- Current Language: C#
        /// *****************************************************************************
        /// </remarks>
        /// <history>
        /// *****************************************************************************
        ///     --- Date First Created : 08 - 11 - 2017
        ///     --- Author: Fadipe Wasiu Ayobami
        ///     --- Date First Reviewed: 
        ///     --- Date Last Reviewed:
        /// *****************************************************************************
        /// </history>
        /// <usage>
        /// Class a new class i.e Animal
        /// public class Animal:Entity<int>
        /// {
        /// 
        /// }
        /// 
        /// -- Fadipe Wasiu Ayobami
        /// </usage>
        /// ----------------------------------------------------------------------------------------------
        ///
        public Entity()
        {
            IsActive = true;
            IsDeleted = false;
            DateCreated = DateTime.Now;
        }

        public TPrimaryKey Id { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
        public bool IsTransient()
        {
            return EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey));
        }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string CreateById { get; set; }
    }
}