﻿namespace IronPigeon.Relay.Models {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web;
	using Microsoft.WindowsAzure.StorageClient;
	using Validation;
#if NET40
	using ReadOnlyListOfAddressBookEmailEntity = System.Collections.ObjectModel.ReadOnlyCollection<AddressBookEmailEntity>;
#else
	using ReadOnlyListOfAddressBookEmailEntity = System.Collections.Generic.IReadOnlyList<AddressBookEmailEntity>;
#endif

	public class AddressBookContext : TableServiceContext {
		public AddressBookContext(CloudTableClient client, string primaryTableName, string emailAddressTableName)
			: base(client.BaseUri.AbsoluteUri, client.Credentials) {
			Requires.NotNullOrEmpty(primaryTableName, "primaryTableName");
			Requires.NotNullOrEmpty(emailAddressTableName, "emailAddressTableName");

			this.PrimaryTableName = primaryTableName;
			this.EmailAddressTableName = emailAddressTableName;
		}

		public string PrimaryTableName { get; private set; }

		public string EmailAddressTableName { get; private set; }

		public async Task<AddressBookEntity> GetAsync(string provider, string userId) {
			Requires.NotNullOrEmpty(provider, "provider");
			Requires.NotNullOrEmpty(userId, "userId");

			var query = (from inbox in this.CreateQuery<AddressBookEntity>(this.PrimaryTableName)
						 where inbox.RowKey == AddressBookEntity.ConstructRowKey(provider, userId)
						 select inbox).AsTableServiceQuery();
			var result = await query.ExecuteAsync();
			return result.FirstOrDefault();
		}

		public async Task<AddressBookEmailEntity> GetAddressBookEmailEntityAsync(string email) {
			var query = (from address in this.CreateQuery<AddressBookEmailEntity>(this.EmailAddressTableName)
						 where address.RowKey == email.ToLowerInvariant()
						 select address).AsTableServiceQuery();
			var result = await query.ExecuteAsync();
			return result.FirstOrDefault();
		}

		public async Task<AddressBookEmailEntity> GetAddressBookEmailEntityByHashAsync(string emailHash) {
			var query = (from address in this.CreateQuery<AddressBookEmailEntity>(this.EmailAddressTableName)
						 where address.MicrosoftEmailHash == emailHash
						 select address).AsTableServiceQuery();
			var result = await query.ExecuteAsync();
			return result.FirstOrDefault();
		}

		public async Task<AddressBookEntity> GetAddressBookEntityByEmailHashAsync(string emailHash) {
			Requires.NotNull(emailHash, "emailHash");

			var emailEntity = await this.GetAddressBookEmailEntityByHashAsync(emailHash);
			if (emailEntity == null) {
				return null;
			}

			var query = (from inbox in this.CreateQuery<AddressBookEntity>(this.PrimaryTableName)
						 where inbox.RowKey == emailEntity.AddressBookEntityRowKey
						 select inbox).AsTableServiceQuery();
			var result = await query.ExecuteAsync();
			var entryEntity = result.FirstOrDefault();
			return entryEntity;
		}

		public async Task<AddressBookEntity> GetAddressBookEntityByEmailAsync(string email) {
			Requires.NotNull(email, "email");

			var emailEntity = await this.GetAddressBookEmailEntityAsync(email);
			if (emailEntity == null) {
				return null;
			}

			var query = (from inbox in this.CreateQuery<AddressBookEntity>(this.PrimaryTableName)
						 where inbox.RowKey == emailEntity.AddressBookEntityRowKey
						 select inbox).AsTableServiceQuery();
			var result = await query.ExecuteAsync();
			var entryEntity = result.FirstOrDefault();
			return entryEntity;
		}

		public async Task<ReadOnlyListOfAddressBookEmailEntity> GetEmailAddressesAsync(AddressBookEntity entity) {
			var query = (from address in this.CreateQuery<AddressBookEmailEntity>(this.EmailAddressTableName)
						 where address.AddressBookEntityRowKey == entity.RowKey
						 select address).AsTableServiceQuery();
			var result = await query.ExecuteAsync();
			
#if NET40
			return new ReadOnlyListOfAddressBookEmailEntity(result.ToList());
#else
			return result.ToList();
#endif
		}

		public void AddObject(AddressBookEntity entity) {
			this.AddObject(this.PrimaryTableName, entity);
		}

		public void AddObject(AddressBookEmailEntity entity) {
			this.AddObject(this.EmailAddressTableName, entity);
		}
	}
}