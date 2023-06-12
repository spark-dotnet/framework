using BlazorSpark.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Tests.Extensions
{
	[TestClass]
	public class ExtensionTests
	{
		[TestMethod]
		public void ShouldSlugifyString()
		{
			var str = "THis IS somE * String";

			var slug = str.ToSlug();

			Assert.AreEqual("this-is-some-string", slug);
		}
	}
}
