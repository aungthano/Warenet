// This file was automatically generated by the Dapper.SimpleCRUD T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: `Warenet`
//     Provider:               `System.Data.SqlClient`
//     Connection String:      `Server=.\SQL2014;Database=UNILYGN;User ID=sa;Password=P@$$w0rd`
//     Include Views:          `False`

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Warenet.WebApi.Models
{
    /// <summary>
    /// A class which represents the whgi2 table.
    /// </summary>
	[Table("whgi2")]
	public partial class whgi2
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual int LineItemNo { get; set; }
		public virtual string BinNo { get; set; }
		public virtual string BatchNo { get; set; }
		public virtual int BatchLineItemNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string DimensionFlag { get; set; }
		public virtual DateTime? ExpiryDate { get; set; }
		public virtual decimal? Height { get; set; }
		public virtual decimal? Length { get; set; }
		public virtual int? Qty { get; set; }
		public virtual DateTime? ManufactureDate { get; set; }
		public virtual string ItemCode { get; set; }
		public virtual string PurchaseOrderNo { get; set; }
		public virtual decimal? SpaceArea { get; set; }
		public virtual string UomCode { get; set; }
		public virtual decimal? Volume { get; set; }
		public virtual decimal? Weight { get; set; }
		public virtual decimal? Width { get; set; }
	}

    /// <summary>
    /// A class which represents the AspNetRoles table.
    /// </summary>
	[Table("AspNetRoles")]
	public partial class AspNetRole
	{
		[Key]
		public virtual string Id { get; set; }
		public virtual string Name { get; set; }
	}

    /// <summary>
    /// A class which represents the AspNetUserRoles table.
    /// </summary>
	[Table("AspNetUserRoles")]
	public partial class AspNetUserRole
	{
		[Key]
		public virtual string UserId { get; set; }
		public virtual string RoleId { get; set; }
	}

    /// <summary>
    /// A class which represents the cmvd1 table.
    /// </summary>
	[Table("cmvd1")]
	public partial class cmvd1
	{
		[Key]
		public virtual string UserId { get; set; }
		public virtual string ViewName { get; set; }
		public virtual string ViewContent { get; set; }
		public virtual string ViewWidth { get; set; }
	}

    /// <summary>
    /// A class which represents the AspNetUsers table.
    /// </summary>
	[Table("AspNetUsers")]
	public partial class AspNetUser
	{
		[Key]
		public virtual string Id { get; set; }
		public virtual string Email { get; set; }
		public virtual bool EmailConfirmed { get; set; }
		public virtual string PasswordHash { get; set; }
		public virtual string SecurityStamp { get; set; }
		public virtual string PhoneNumber { get; set; }
		public virtual bool PhoneNumberConfirmed { get; set; }
		public virtual bool TwoFactorEnabled { get; set; }
		public virtual DateTime? LockoutEndDateUtc { get; set; }
		public virtual bool LockoutEnabled { get; set; }
		public virtual int AccessFailedCount { get; set; }
		public virtual string UserName { get; set; }
	}

    /// <summary>
    /// A class which represents the AspNetUserClaims table.
    /// </summary>
	[Table("AspNetUserClaims")]
	public partial class AspNetUserClaim
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual string UserId { get; set; }
		public virtual string ClaimType { get; set; }
		public virtual string ClaimValue { get; set; }
	}

    /// <summary>
    /// A class which represents the AspNetUserLogins table.
    /// </summary>
	[Table("AspNetUserLogins")]
	public partial class AspNetUserLogin
	{
		[Key]
		public virtual string LoginProvider { get; set; }
		public virtual string ProviderKey { get; set; }
		public virtual string UserId { get; set; }
	}

    /// <summary>
    /// A class which represents the whwh1 table.
    /// </summary>
	[Table("whwh1")]
	public partial class whwh1
	{
		[Key]
		public virtual string WarehouseCode { get; set; }
		public virtual string StoreTypeCode { get; set; }
		public virtual string Address { get; set; }
		public virtual string CityCode { get; set; }
		public virtual string ContactName { get; set; }
		public virtual string CountryCode { get; set; }
		public virtual string LicensedFlag { get; set; }
		public virtual string Telephone { get; set; }
		public virtual string WarehouseName { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the __MigrationHistory table.
    /// </summary>
	[Table("__MigrationHistory")]
	public partial class MigrationHistory
	{
		[Key]
		public virtual string MigrationId { get; set; }
		public virtual string ContextKey { get; set; }
		public virtual byte[] Model { get; set; }
		public virtual string ProductVersion { get; set; }
	}

    /// <summary>
    /// A class which represents the whgr1 table.
    /// </summary>
	[Table("whgr1")]
	public partial class whgr1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string GoodsReceiptNoteNo { get; set; }
		public virtual string AsnOrderNo { get; set; }
		public virtual string SupplierGrnNo { get; set; }
		public virtual string SupplierCode { get; set; }
		public virtual DateTime ReceiptDate { get; set; }
		public virtual string ReceiveBy { get; set; }
		public virtual string WarehouseCode { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whit1 table.
    /// </summary>
	[Table("whit1")]
	public partial class whit1
	{
		[Key]
		public virtual string ItemCode { get; set; }
		public virtual string ItemName { get; set; }
		public virtual string BrandName { get; set; }
		public virtual string CountryOfOrigin { get; set; }
		public virtual string SupplierCode { get; set; }
		public virtual string DgIndicator { get; set; }
		public virtual string DimensionFlag { get; set; }
		public virtual string IssueMethod { get; set; }
		public virtual decimal? LooseHeight { get; set; }
		public virtual decimal? LooseLength { get; set; }
		public virtual int? LooseQty { get; set; }
		public virtual decimal? LooseSpaceArea { get; set; }
		public virtual string LooseUomCode { get; set; }
		public virtual decimal? LooseVolume { get; set; }
		public virtual decimal? LooseWeight { get; set; }
		public virtual decimal? LooseWidth { get; set; }
		public virtual string Model { get; set; }
		public virtual decimal? PackingHeight { get; set; }
		public virtual decimal? PackingLength { get; set; }
		public virtual int? PackingQty { get; set; }
		public virtual decimal? PackingSpaceArea { get; set; }
		public virtual string PackingUomCode { get; set; }
		public virtual decimal? PackingVolume { get; set; }
		public virtual decimal? PackingWeight { get; set; }
		public virtual decimal? PackingWidth { get; set; }
		public virtual string ItemClassCode { get; set; }
		public virtual string ItemRefNo { get; set; }
		public virtual decimal? WholeHeight { get; set; }
		public virtual decimal? WholeLength { get; set; }
		public virtual int? WholeQty { get; set; }
		public virtual decimal? WholeSpaceArea { get; set; }
		public virtual string WholeUomCode { get; set; }
		public virtual decimal? WholeVolume { get; set; }
		public virtual decimal? WholeWeight { get; set; }
		public virtual decimal? WholeWidth { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whgr2 table.
    /// </summary>
	[Table("whgr2")]
	public partial class whgr2
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual short LineItemNo { get; set; }
		public virtual string BinNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string DimensionFlag { get; set; }
		public virtual DateTime? ExpiryDate { get; set; }
		public virtual decimal? Height { get; set; }
		public virtual decimal? Length { get; set; }
		public virtual int? Qty { get; set; }
		public virtual DateTime? ManufactureDate { get; set; }
		public virtual string ItemCode { get; set; }
		public virtual decimal? SpaceArea { get; set; }
		public virtual string UomCode { get; set; }
		public virtual decimal? Volume { get; set; }
		public virtual decimal? Weight { get; set; }
		public virtual decimal? Width { get; set; }
	}

    /// <summary>
    /// A class which represents the rfum1 table.
    /// </summary>
	[Table("rfum1")]
	public partial class rfum1
	{
		[Key]
		public virtual string UomCode { get; set; }
		public virtual string UomDescription { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whpl1 table.
    /// </summary>
	[Table("whpl1")]
	public partial class whpl1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string PickNo { get; set; }
		public virtual string PickBy { get; set; }
		public virtual DateTime? PickDate { get; set; }
		public virtual string WarehouseCode { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whgt1 table.
    /// </summary>
	[Table("whgt1")]
	public partial class whgt1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string CustomerCode { get; set; }
		public virtual string Description1 { get; set; }
		public virtual string Description2 { get; set; }
		public virtual string GoodsTransferNoteNo { get; set; }
		public virtual string Remark { get; set; }
		public virtual string TransferBy { get; set; }
		public virtual DateTime? TransferDateTime { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whpo1 table.
    /// </summary>
	[Table("whpo1")]
	public partial class whpo1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string PurchaseOrderNo { get; set; }
		public virtual string VendorRefNo { get; set; }
		public virtual string VendorCode { get; set; }
		public virtual string SupplierCode { get; set; }
		public virtual DateTime PurchaseOrderDate { get; set; }
		public virtual string WarehouseCode { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whpl2 table.
    /// </summary>
	[Table("whpl2")]
	public partial class whpl2
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual int LineItemNo { get; set; }
		public virtual string BatchNo { get; set; }
		public virtual string BinNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string DimensionFlag { get; set; }
		public virtual DateTime? ExpiryDate { get; set; }
		public virtual decimal? Height { get; set; }
		public virtual decimal? Length { get; set; }
		public virtual int Qty { get; set; }
		public virtual DateTime? ManufactureDate { get; set; }
		public virtual string ItemCode { get; set; }
		public virtual decimal? SpaceArea { get; set; }
		public virtual string UomCode { get; set; }
		public virtual decimal? Volume { get; set; }
		public virtual decimal? Weight { get; set; }
		public virtual decimal? Width { get; set; }
	}

    /// <summary>
    /// A class which represents the whgt2 table.
    /// </summary>
	[Table("whgt2")]
	public partial class whgt2
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual short LineItemNo { get; set; }
		public virtual DateTime? ExpiryDate { get; set; }
		public virtual int? LooseQty { get; set; }
		public virtual DateTime? ManufactureDate { get; set; }
		public virtual int? MovementTrxNo { get; set; }
		public virtual string NewStoreNo { get; set; }
		public virtual string NewWarehouseCode { get; set; }
		public virtual int? PackingQty { get; set; }
		public virtual int? ProductTrxNo { get; set; }
		public virtual decimal? SpaceArea { get; set; }
		public virtual string StoreNo { get; set; }
		public virtual decimal? Volume { get; set; }
		public virtual string WarehouseCode { get; set; }
		public virtual decimal? Weight { get; set; }
		public virtual int? WholeQty { get; set; }
		public virtual string Updateby { get; set; }
	}

    /// <summary>
    /// A class which represents the whpn1 table.
    /// </summary>
	[Table("whpn1")]
	public partial class whpn1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string PartNo { get; set; }
		public virtual string PartDescription { get; set; }
		public virtual string ItemCode { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whwh2 table.
    /// </summary>
	[Table("whwh2")]
	public partial class whwh2
	{
		[Key]
		public virtual string WarehouseCode { get; set; }
		public virtual int LineItemNo { get; set; }
		public virtual string BinNo { get; set; }
		public virtual string Description { get; set; }
		public virtual decimal? Height { get; set; }
		public virtual decimal? Length { get; set; }
		public virtual decimal? PalletSpace { get; set; }
		public virtual string UseFlag { get; set; }
		public virtual decimal? StoreSpace { get; set; }
		public virtual decimal? Width { get; set; }
	}

    /// <summary>
    /// A class which represents the whpo2 table.
    /// </summary>
	[Table("whpo2")]
	public partial class whpo2
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual short LineItemNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string DimensionFlag { get; set; }
		public virtual DateTime? ExpiryDate { get; set; }
		public virtual decimal? Height { get; set; }
		public virtual decimal? Length { get; set; }
		public virtual int? Qty { get; set; }
		public virtual DateTime? ManufactureDate { get; set; }
		public virtual string ItemCode { get; set; }
		public virtual decimal? SpaceArea { get; set; }
		public virtual string UomCode { get; set; }
		public virtual decimal? Volume { get; set; }
		public virtual decimal? Weight { get; set; }
		public virtual decimal? Width { get; set; }
	}

    /// <summary>
    /// A class which represents the saal1 table.
    /// </summary>
	[Table("saal1")]
	public partial class saal1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual int LineItemNo { get; set; }
		public virtual string TableName { get; set; }
		public virtual string FieldName { get; set; }
		public virtual string NewValue { get; set; }
		public virtual string OldValue { get; set; }
		public virtual int? PrimaryKeyLineItemNo { get; set; }
		public virtual string PrimaryKeyName { get; set; }
		public virtual string PrimaryKeyValue { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whar1 table.
    /// </summary>
	[Table("whar1")]
	public partial class whar1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string AsnOrderNo { get; set; }
		public virtual string SupplierAsnNo { get; set; }
		public virtual string SupplierCode { get; set; }
		public virtual DateTime? AsnOrderDate { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WarehouseCode { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the rfbp1 table.
    /// </summary>
	[Table("rfbp1")]
	public partial class rfbp1
	{
		[Key]
		public virtual string BusinessPartyCode { get; set; }
		public virtual string BusinessPartyName { get; set; }
		public virtual string Address { get; set; }
		public virtual string BranchId { get; set; }
		public virtual string CityCode { get; set; }
		public virtual string ContactName { get; set; }
		public virtual string CountryCode { get; set; }
		public virtual string Email { get; set; }
		public virtual string Fax { get; set; }
		public virtual string LocalName { get; set; }
		public virtual string PartyType { get; set; }
		public virtual string PostalCode { get; set; }
		public virtual string Remark { get; set; }
		public virtual string TaxId { get; set; }
		public virtual string Telephone { get; set; }
		public virtual string WarehouseCode { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the rfcy1 table.
    /// </summary>
	[Table("rfcy1")]
	public partial class rfcy1
	{
		[Key]
		public virtual string CountryCode { get; set; }
		public virtual string CountryName { get; set; }
		public virtual string RegionCode { get; set; }
		public virtual string Remark { get; set; }
		public virtual string ZoneCode { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whbi1 table.
    /// </summary>
	[Table("whbi1")]
	public partial class whbi1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string TablePrefix { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime? CreateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the sanb1 table.
    /// </summary>
	[Table("sanb1")]
	public partial class sanb1
	{
		[Key]
		public virtual string TableName { get; set; }
		public virtual string FieldName { get; set; }
		public virtual string Format { get; set; }
	}

    /// <summary>
    /// A class which represents the whbi2 table.
    /// </summary>
	[Table("whbi2")]
	public partial class whbi2
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string TablePrefix { get; set; }
		public virtual int LineItemNo { get; set; }
		public virtual string ItemCode { get; set; }
		public virtual string ItemName { get; set; }
		public virtual string DimensionFlag { get; set; }
		public virtual int? Qty { get; set; }
		public virtual string UomCode { get; set; }
		public virtual decimal? Length { get; set; }
		public virtual decimal? Width { get; set; }
		public virtual decimal? Height { get; set; }
		public virtual decimal? Weight { get; set; }
		public virtual decimal? Volume { get; set; }
		public virtual decimal? SpaceArea { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the rfct1 table.
    /// </summary>
	[Table("rfct1")]
	public partial class rfct1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string CityCode { get; set; }
		public virtual string CityName { get; set; }
		public virtual string CountryCode { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the saus1 table.
    /// </summary>
	[Table("saus1")]
	public partial class saus1
	{
		[Key]
		public virtual string UserId { get; set; }
		public virtual string UserName { get; set; }
		public virtual bool? IsAdministrator { get; set; }
		public virtual string JobTitle { get; set; }
		public virtual string Password { get; set; }
		public virtual string Remark { get; set; }
		public virtual string UserRoleId { get; set; }
		public virtual string Telephone { get; set; }
		public virtual string Email { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the cmct1 table.
    /// </summary>
	[Table("cmct1")]
	public partial class cmct1
	{
		[Key]
		public virtual string ContentId { get; set; }
		public virtual string ParentContentId { get; set; }
		public virtual string ModuleId { get; set; }
		public virtual string ContentName { get; set; }
		public virtual string FormName { get; set; }
		public virtual string FormPath { get; set; }
		public virtual string ViewName { get; set; }
		public virtual string ViewKey { get; set; }
		public virtual string ViewFilter { get; set; }
		public virtual int SortNo { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whiv1 table.
    /// </summary>
	[Table("whiv1")]
	public partial class whiv1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string WarehouseCode { get; set; }
		public virtual string BinNo { get; set; }
		public virtual string BatchNo { get; set; }
		public virtual short BatchLineItemNo { get; set; }
		public virtual string BatchRefNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string DimensionFlag { get; set; }
		public virtual DateTime? ExpiryDate { get; set; }
		public virtual decimal? Height { get; set; }
		public virtual decimal? Length { get; set; }
		public virtual int Qty { get; set; }
		public virtual DateTime? ManufactureDate { get; set; }
		public virtual string ItemCode { get; set; }
		public virtual decimal? SpaceArea { get; set; }
		public virtual string UomCode { get; set; }
		public virtual decimal? Volume { get; set; }
		public virtual decimal? Weight { get; set; }
		public virtual decimal? Width { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whic1 table.
    /// </summary>
	[Table("whic1")]
	public partial class whic1
	{
		[Key]
		public virtual string ItemClassCode { get; set; }
		public virtual string Description { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whar2 table.
    /// </summary>
	[Table("whar2")]
	public partial class whar2
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual short LineItemNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string DimensionFlag { get; set; }
		public virtual DateTime? ExpiryDate { get; set; }
		public virtual decimal? Height { get; set; }
		public virtual decimal? Length { get; set; }
		public virtual int? Qty { get; set; }
		public virtual DateTime? ManufactureDate { get; set; }
		public virtual string ItemCode { get; set; }
		public virtual decimal? SpaceArea { get; set; }
		public virtual string UomCode { get; set; }
		public virtual decimal? Volume { get; set; }
		public virtual decimal? Weight { get; set; }
		public virtual decimal? Width { get; set; }
	}

    /// <summary>
    /// A class which represents the cmmd1 table.
    /// </summary>
	[Table("cmmd1")]
	public partial class cmmd1
	{
		[Key]
		public virtual string ModuleId { get; set; }
		public virtual string ModuleName { get; set; }
		public virtual string ModulePath { get; set; }
		public virtual int SortNo { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the saur1 table.
    /// </summary>
	[Table("saur1")]
	public partial class saur1
	{
		[Key]
		public virtual string UserRoleId { get; set; }
		public virtual string UserRoleName { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whst1 table.
    /// </summary>
	[Table("whst1")]
	public partial class whst1
	{
		[Key]
		public virtual string StoreTypeCode { get; set; }
		public virtual string StoreTypeDesc { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

    /// <summary>
    /// A class which represents the whgi1 table.
    /// </summary>
	[Table("whgi1")]
	public partial class whgi1
	{
		[Key]
		public virtual int TrxNo { get; set; }
		public virtual string GoodsIssueNoteNo { get; set; }
		public virtual string SupplierCode { get; set; }
		public virtual string IssueBy { get; set; }
		public virtual DateTime IssueDate { get; set; }
		public virtual string VendorCode { get; set; }
		public virtual string WarehouseCode { get; set; }
		public virtual string Remark { get; set; }
		public virtual string WorkStation { get; set; }
		public virtual string StatusCode { get; set; }
		public virtual string CreateBy { get; set; }
		public virtual DateTime CreateDateTime { get; set; }
		public virtual string UpdateBy { get; set; }
		public virtual DateTime UpdateDateTime { get; set; }
	}

}
