﻿using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using vCardLib.Collections;
using vCardLib.Helpers;
using vCardLib.Models;
using Version = vCardLib.Helpers.Version;

namespace vCardLib.Tests
{
    [TestFixture]
	public class Test
	{
		vCard vcard = new vCard();
		[Test]
		public void GenerateValidVcard()
		{
			Assert.DoesNotThrow(delegate
			{
				vcard.Addresses = new AddressCollection();
				vcard.BirthDay = new DateTime();
				vcard.BirthPlace = "Mississipi";
				vcard.DeathPlace = "Washington";
				vcard.EmailAddresses = new EmailAddressCollection();
				vcard.Expertises = new ExpertiseCollection();
				vcard.FamilyName = "Gump";
			    vcard.GivenName = "Forrest";
			    vcard.MiddleName = "Johnson";
			    vcard.Prefix = "HRH";
			    vcard.Suffix = "PhD";
				vcard.Gender = GenderType.Female;
				vcard.Hobbies = new HobbyCollection();
				vcard.Interests = new InterestCollection();
				vcard.Kind = ContactType.Application;
				vcard.Language = "English";
				vcard.NickName = "Gumpy";
				vcard.Organization = "Google";
				vcard.PhoneNumbers = new PhoneNumberCollection();
				vcard.Pictures = new PhotoCollection();
				vcard.TimeZone = "GMT+1";
				vcard.Title = "Mr";
				vcard.Url = "http://google.com";
			    vcard.Note = "Hello World";
				vcard.Version = Version.V2;
			});
		}

		[Test]
		public void ReadsCardsWithoutErrors()
		{
			string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = Path.Combine(assemblyFolder, "v2.vcf");
			vCardCollection collection = null;
			Assert.DoesNotThrow(delegate {
				collection = vCard.FromFile(filePath);
			});
			Assert.IsNotNull(collection);
			Assert.IsTrue(collection.Count > 0);
			filePath = Path.Combine(assemblyFolder, "v3.vcf");
			collection = null;
			Assert.DoesNotThrow(delegate {
				collection = vCard.FromFile(filePath);
			});
			Assert.IsNotNull(collection);
			Assert.IsTrue(collection.Count > 0);
		}

		[Test]
		public void DoesNotOverwriteExceptInstructed()
		{
			Assert.IsNotNull(vcard);
			string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = Path.Combine(assemblyFolder, "invalid.vcf");
			Assert.Throws<InvalidOperationException>(delegate {
				vcard.Save(filePath);
			});
		}

        [Test]
        public void SavesV2CardWithoutErrors()
        {
			string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = Path.Combine(assemblyFolder, "newv2.vcf");
			Assert.IsNotNull(vcard);
            Assert.DoesNotThrow(delegate
            {
                vcard.GivenName = "Forrest";
                vcard.FamilyName = "Gump";
                PhoneNumber num1 = new PhoneNumber();
                num1.Number = "(111) 555-1212";
				num1.Type = PhoneNumberType.None;
                vcard.PhoneNumbers.Add(num1);
                PhoneNumber num2 = new PhoneNumber();
                num2.Number = "(404) 555-1212";
                num2.Type = PhoneNumberType.Home;
                vcard.PhoneNumbers.Add(num2);
				PhoneNumber num3 = new PhoneNumber();
				num3.Number = "(404) 555-1212";
				num3.Type = PhoneNumberType.MainNumber;
				vcard.PhoneNumbers.Add(num3);
                EmailAddress email1 = new EmailAddress();
                email1.Email = new System.Net.Mail.MailAddress("forrestgump@example.com");
                email1.Type = EmailType.None;
                vcard.EmailAddresses.Add(email1);
				EmailAddress email2 = new EmailAddress();
				email2.Email = new System.Net.Mail.MailAddress("forrestgump@example.com");
				email2.Type = EmailType.Internet;
				vcard.EmailAddresses.Add(email2);
                Address address1 = new Address();
                address1.Location = "Sabo, Yaba";
				address1.Type = AddressType.None;
                vcard.Addresses.Add(address1);
				Address address2 = new Address();
				address2.Location = "Sabo, Yaba";
				address2.Type = AddressType.Work;
				vcard.Addresses.Add(address2);
                Photo photo1 = new Photo();
                photo1.Encoding = PhotoEncoding.JPEG;
                photo1.PhotoURL = "www.google/images";
                photo1.Type = PhotoType.URL;
                vcard.Pictures.Add(photo1);

				var request = System.Net.WebRequest.Create("https://jpeg.org/images/jpeg-logo-plain.png");
				System.Net.WebResponse response = request.GetResponse();
				Stream responseStream = response.GetResponseStream();

				Photo photo2 = new Photo();
				photo2.Type = PhotoType.Image;
				photo2.Encoding = PhotoEncoding.JPEG;
				photo2.Picture = new System.Drawing.Bitmap(responseStream);
				vcard.Pictures.Add(photo2);

                Hobby hobby = new Hobby();
                hobby.Activity = "Watching Hobbits";
                hobby.Level = Level.Medium;
                vcard.Hobbies.Add(hobby);
                Interest interest = new Interest();
                interest.Activity = "Watching Hobbits";
                interest.Level = Level.Medium;
                vcard.Interests.Add(interest);
                Expertise expertise = new Expertise();
                expertise.Area = "Watching Hobbits";
                expertise.Level = Level.Medium;
                vcard.Expertises.Add(expertise);

                vcard.Save(filePath, WriteOptions.Overwrite);
            });
			Assert.IsTrue(File.Exists(filePath));
        }

        [Test]
        public void SavesV3CardWithoutErrors()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = Path.Combine(assemblyFolder, "newv3.vcf");
			Assert.IsNotNull(vcard);
			Assert.DoesNotThrow(delegate {
				vcard.Save(filePath, Version.V3, WriteOptions.Overwrite);
			});
			Assert.IsTrue(File.Exists(filePath));
        }

		[Test]
		public void SavesV4CardThrowsException()
		{
			Assert.Throws<NotImplementedException> (delegate {
				vcard.Save ("", Version.V4, WriteOptions.Overwrite);
			});
		}
    }
}

