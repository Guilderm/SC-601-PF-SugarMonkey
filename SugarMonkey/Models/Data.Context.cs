﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SugarMonkey.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GeneralPurposeDBEntities : DbContext
    {
        public GeneralPurposeDBEntities()
            : base("name=GeneralPurposeDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AppSetting> AppSettings { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<DeliveryOption> DeliveryOptions { get; set; }
        public virtual DbSet<OrderedItem> OrderedItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderStage> OrderStages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductsCategory> ProductsCategories { get; set; }
        public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ZipCode> ZipCodes { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
    
        public virtual ObjectResult<STP_CreateUser_Result> STP_CreateUser(string firstName, string firstLastName, string secondLastName, Nullable<int> cellphone, string email, string password, string salt)
        {
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var firstLastNameParameter = firstLastName != null ?
                new ObjectParameter("FirstLastName", firstLastName) :
                new ObjectParameter("FirstLastName", typeof(string));
    
            var secondLastNameParameter = secondLastName != null ?
                new ObjectParameter("SecondLastName", secondLastName) :
                new ObjectParameter("SecondLastName", typeof(string));
    
            var cellphoneParameter = cellphone.HasValue ?
                new ObjectParameter("Cellphone", cellphone) :
                new ObjectParameter("Cellphone", typeof(int));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var saltParameter = salt != null ?
                new ObjectParameter("Salt", salt) :
                new ObjectParameter("Salt", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_CreateUser_Result>("STP_CreateUser", firstNameParameter, firstLastNameParameter, secondLastNameParameter, cellphoneParameter, emailParameter, passwordParameter, saltParameter);
        }
    
        public virtual ObjectResult<string> STP_GetAppSetting(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("STP_GetAppSetting", nameParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> STP_GetCredential(string email, string password)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("STP_GetCredential", emailParameter, passwordParameter);
        }
    
        public virtual ObjectResult<STP_GetUserByResetPasswordCode_Result> STP_GetUserByResetPasswordCode(string resetPasswordCode)
        {
            var resetPasswordCodeParameter = resetPasswordCode != null ?
                new ObjectParameter("ResetPasswordCode", resetPasswordCode) :
                new ObjectParameter("ResetPasswordCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_GetUserByResetPasswordCode_Result>("STP_GetUserByResetPasswordCode", resetPasswordCodeParameter);
        }
    
        public virtual ObjectResult<STP_GetUsersInfo_Result> STP_GetUsersInfo()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_GetUsersInfo_Result>("STP_GetUsersInfo");
        }
    
        public virtual ObjectResult<STP_GetUsersInfoByEmail_Result> STP_GetUsersInfoByEmail(string email)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_GetUsersInfoByEmail_Result>("STP_GetUsersInfoByEmail", emailParameter);
        }
    
        public virtual ObjectResult<STP_GetUsersInfoByID_Result> STP_GetUsersInfoByID(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_GetUsersInfoByID_Result>("STP_GetUsersInfoByID", userIDParameter);
        }
    
        public virtual int STP_NewAppSetting(string name, string value, string @default, string description)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var valueParameter = value != null ?
                new ObjectParameter("Value", value) :
                new ObjectParameter("Value", typeof(string));
    
            var defaultParameter = @default != null ?
                new ObjectParameter("Default", @default) :
                new ObjectParameter("Default", typeof(string));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("STP_NewAppSetting", nameParameter, valueParameter, defaultParameter, descriptionParameter);
        }
    
        public virtual int STP_ResetAppSetting()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("STP_ResetAppSetting");
        }
    
        public virtual int STP_SetAppSetting(string name, string value)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var valueParameter = value != null ?
                new ObjectParameter("Value", value) :
                new ObjectParameter("Value", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("STP_SetAppSetting", nameParameter, valueParameter);
        }
    
        public virtual ObjectResult<STP_SetResetPasswordCode_Result> STP_SetResetPasswordCode(string email, string resetPasswordCode)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var resetPasswordCodeParameter = resetPasswordCode != null ?
                new ObjectParameter("ResetPasswordCode", resetPasswordCode) :
                new ObjectParameter("ResetPasswordCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_SetResetPasswordCode_Result>("STP_SetResetPasswordCode", emailParameter, resetPasswordCodeParameter);
        }
    
        public virtual ObjectResult<STP_UpdateCredentials_Result> STP_UpdateCredentials(Nullable<int> userID, string password, string salt)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var saltParameter = salt != null ?
                new ObjectParameter("Salt", salt) :
                new ObjectParameter("Salt", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_UpdateCredentials_Result>("STP_UpdateCredentials", userIDParameter, passwordParameter, saltParameter);
        }
    }
}
